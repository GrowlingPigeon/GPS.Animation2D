using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GrowlingPigeonStudio.Utilities;
using UnityEngine;

#nullable enable

namespace GrowlingPigeonStudio.Animation2D
{
  /// <summary>
  /// GPS animator 2D.
  /// </summary>
  public class GPSAnimator2D
    : MonoBehaviour
  {
    /// <summary>
    /// Animation dictionary.
    /// </summary>
    private readonly Dictionary<int, GPSAnimation2D> animationDictionary = new Dictionary<int, GPSAnimation2D>();

    /// <summary>
    /// Collection of animation ids keyed by string.
    /// </summary>
    private readonly Dictionary<string, AnimationID> animationIDs = new Dictionary<string, AnimationID>();

    /// <summary>
    /// Animation id provider.
    /// </summary>
    private readonly AnimationIDProvider idProvider = new AnimationIDProvider();

    /// <summary>
    /// Collection of animations.
    /// </summary>
    [SerializeField]
    private List<GPSAnimation2D> animations = new List<GPSAnimation2D>();

    /// <summary>
    /// Next animation to play.
    /// </summary>
    private GPSAnimation2D? nextAnimation = null;

    /// <summary>
    /// Current animation.
    /// </summary>
    private GPSAnimation2D? currentAnimation = null;

    /// <summary>
    /// Previous animation.
    /// </summary>
    private GPSAnimation2D? previousAnimation = null;

    /// <summary>
    /// Renderer to update.
    /// </summary>
    private SpriteRenderer? target = null;

    /// <summary>
    /// Current frame.
    /// </summary>
    private float frame = 0f;

    /// <summary>
    /// Current transforms.
    /// </summary>
    private AnimationTransforms currentTransforms = AnimationTransforms.NoTransform;

    /// <summary>
    /// Gets current frame.
    /// </summary>
    public int CurrentFrame => Mathf.FloorToInt(this.frame);

    /// <summary>
    /// Gets current animation name.
    /// </summary>
    public string? CurrentAnimation => this.currentAnimation?.name ?? null;

    /// <summary>
    /// End of animation event.
    /// </summary>
    public event GPSEvent<string>? EndOfAnimationEvent;

    /// <summary>
    /// Sets target sprite renderer.
    /// </summary>
    /// <param name="target">Target sprite renderer.</param>
    public void SetTargetRenderer(SpriteRenderer target)
    {
      this.target = target;
    }

    /// <summary>
    /// Sets animation. WARNING slow method - use SetAnimation with AnimationID instead for better performance.
    /// </summary>
    /// <param name="animationName">Name of animation to set.</param>
    /// <returns>Whether successful.</returns>
    public bool SetAnimation(string animationName)
    {
      if (animationName is null)
      {
        return false;
      }

      if (!this.animationIDs.TryGetValue(animationName, out var animationID))
      {
        return false;
      }

      var animation = this.animationDictionary[animationID.id];
      return this.SetAnimation(animation);
    }

    /// <summary>
    /// Sets animation.
    /// </summary>
    /// <param name="animationID">ID of animation to set.</param>
    /// <returns>Whether successful.</returns>
    public bool SetAnimation(AnimationID animationID)
    {
      if (!this.animationDictionary.TryGetValue(animationID.id, out var animation))
      {
        return false;
      }

      return this.SetAnimation(animation);
    }

    /// <summary>
    /// Gets animation ids.
    /// </summary>
    /// <returns>Animation ids.</returns>
    public IEnumerable<AnimationID> GetAnimationIDs()
    {
      foreach (var id in this.animationIDs.Values)
      {
        yield return id;
      }
    }

    /// <summary>
    /// Adds animation.
    /// </summary>
    /// <param name="animation">Animation to add.</param>
    /// <returns>Whether adding animation was successful.</returns>
    public bool AddAnimation(GPSAnimation2D animation)
    {
      if (animation is null)
      {
        return false;
      }

      if (this.animationIDs.ContainsKey(animation.name))
      {
        return false;
      }

      this.AddAnimationHelper(animation);
      this.animations.Add(animation);
      return true;
    }

    /// <summary>
    /// Called before <see cref="Start"/>.
    /// </summary>
    private void Awake()
    {
      foreach (var animation in this.animations)
      {
        if (animation is null)
        {
          GPSLogger.LogError($"The game object {this.name} has a {nameof(GPSAnimation2D)} class with a null animation!");
          return;
        }

        if (this.animationIDs.ContainsKey(animation.name))
        {
          continue;
        }

        this.AddAnimationHelper(animation);
      }
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update()
    {
      this.UpdateCurrentAnimation();
    }

    /// <summary>
    /// Sets animation with safety checks.
    /// </summary>
    /// <param name="animation">Animation.</param>
    /// <returns>Whether successful.</returns>
    private bool SetAnimation(GPSAnimation2D animation)
    {
      if (animation is null)
      {
        return false;
      }

      if (ReferenceEquals(animation, this.currentAnimation))
      {
        return false;
      }

      if (this.currentAnimation is null || !this.currentAnimation.blockOtherAnimations)
      {
        this.SetAnimationWithoutSafety(animation);
        return true;
      }

      this.nextAnimation = animation;
      return true;
    }

    /// <summary>
    /// Sets animation without safety checks.
    /// </summary>
    /// <param name="animation">Animation.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetAnimationWithoutSafety(GPSAnimation2D animation)
    {
      this.frame = 0f;
      this.previousAnimation = this.currentAnimation;
      this.currentAnimation = animation;
    }

    /// <summary>
    /// Adds animation helper method.
    /// </summary>
    /// <param name="animation">Animation to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddAnimationHelper(GPSAnimation2D animation)
    {
      string animationName = animation.name;
      if (string.IsNullOrWhiteSpace(animationName))
      {
        GPSLogger.LogError($"Animation name cannot be null, empty, or all blanks! Offending resource {animation.name}");
        return;
      }

      var animationID = this.idProvider.GetAnimationID(animationName);
      this.animationIDs[animationName] = animationID;
      this.animationDictionary[animationID.id] = animation;
    }

    /// <summary>
    /// Updates target renderer.
    /// </summary>
    /// <param name="sprite">Sprite to use.</param>
    private void UpdateTargetRenderer(Sprite sprite)
    {
      if (this.target is null)
      {
        return;
      }

      this.target.sprite = sprite;
      this.ApplyAnimationTransforms();
    }

    /// <summary>
    /// Applies animatin transforms.
    /// </summary>
    private void ApplyAnimationTransforms()
    {
      if (this.currentAnimation is null || this.target is null)
      {
        return;
      }

      if (this.currentAnimation.transform == this.currentTransforms)
      {
        return;
      }

      bool performHorizontalFlip = this.currentAnimation.transform.HasFlag(AnimationTransforms.FlipHorizontal) ^
        this.currentTransforms.HasFlag(AnimationTransforms.FlipHorizontal);
      bool performVerticalFlip = this.currentAnimation.transform.HasFlag(AnimationTransforms.FlipVertical) ^
        this.currentTransforms.HasFlag(AnimationTransforms.FlipVertical);

      if (performHorizontalFlip)
      {
        this.target.flipX = !this.target.flipX;
      }

      if (performVerticalFlip)
      {
        this.target.flipY = !this.target.flipY;
      }
    }

    /// <summary>
    /// Updates current animation.
    /// </summary>
    private void UpdateCurrentAnimation()
    {
      if (this.currentAnimation is null || this.currentAnimation.sprites is null)
      {
        return;
      }

      this.frame += Time.deltaTime * this.currentAnimation.framesPerSecond;
      if (this.frame > this.currentAnimation.sprites.Length)
      {
        this.HandleEndOfAnimation();
      }

      this.UpdateTargetRenderer(this.currentAnimation.sprites[this.CurrentFrame]);
    }

    /// <summary>
    /// Handles end of animation.
    /// </summary>
    private void HandleEndOfAnimation()
    {
      if (this.currentAnimation is null || this.currentAnimation.sprites is null)
      {
        return;
      }

      if (this.currentAnimation.animationOption == GPSAnimationOptionsEnum.AnimationEndedEvent)
      {
        this.EndOfAnimationEvent?.Invoke(this.currentAnimation.name);
      }

      if (this.currentAnimation.blockOtherAnimations && this.nextAnimation is { })
      {
        this.SetAnimationWithoutSafety(this.nextAnimation);
        this.nextAnimation = null;
        return;
      }

      if (this.currentAnimation.animationOption == GPSAnimationOptionsEnum.BackToPrevious && this.previousAnimation is { })
      {
        this.SetAnimationWithoutSafety(this.previousAnimation);
        return;
      }

      this.frame %= this.currentAnimation.sprites.Length;
    }
  }
}