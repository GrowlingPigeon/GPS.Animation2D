using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

#nullable enable

namespace GrowlingPigeonStudio.Animation2D.Editor
{
  /// <summary>
  /// Editor animation 2d.
  /// </summary>
  internal class EditorAnimation2D
  {
    /// <summary>
    /// Local inspector.
    /// </summary>
    private readonly AnimationInspector inspector;

    /// <summary>
    /// Local animation.
    /// </summary>
    private GPSAnimation2D animation;

    /// <summary>
    /// Seconds per frame.
    /// </summary>
    private float secondsPerFrame;

    /// <summary>
    /// Current frame.
    /// </summary>
    private int currentFrame;

    /// <summary>
    /// Local coroutine.
    /// </summary>
    private EditorCoroutine? couroutine;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorAnimation2D"/> class.
    /// </summary>
    /// <param name="animation">Animation this class wraps.</param>
    /// <param name="inspector">Inspector.</param>
    public EditorAnimation2D(GPSAnimation2D animation, AnimationInspector inspector)
    {
      this.inspector = inspector;
      this.animation = animation;
      this.secondsPerFrame = this.CalculateSecondsPerFrame();
      this.StartDrawCoroutine();
    }

    /// <summary>
    /// Sets animation.
    /// </summary>
    /// <param name="animation">Animation to assign.</param>
    public void SetAnimation(GPSAnimation2D animation)
    {
      this.animation = animation;
      this.secondsPerFrame = this.CalculateSecondsPerFrame();
    }

    /// <summary>
    /// Disables this editor animation.
    /// </summary>
    public void Disable()
    {
      if (this.couroutine is { })
      {
        EditorCoroutineUtility.StopCoroutine(this.couroutine);
        this.couroutine = null;
      }
    }

    /// <summary>
    /// Draw helper.
    /// </summary>
    public void Draw()
    {
      if (this.animation.sprites is null)
      {
        return;
      }

      if (this.currentFrame >= this.animation.sprites.Length)
      {
        this.currentFrame = 0;
      }

      var sprite = this.animation.sprites[this.currentFrame];
      GUILayout.Label(AssetPreview.GetAssetPreview(sprite));
    }

    /// <summary>
    /// Draws this animation.
    /// </summary>
    /// <returns>Enumerator.</returns>
    private IEnumerator DrawCoroutine()
    {
      this.inspector.Repaint();

      if (this.animation.sprites is null)
      {
        yield break;
      }

      this.currentFrame++;
      if (this.currentFrame >= this.animation.sprites.Length)
      {
        this.currentFrame = 0;
      }

      yield return new EditorWaitForSeconds(this.secondsPerFrame);
      this.StartDrawCoroutine();
    }

    /// <summary>
    /// Starts draw coroutine.
    /// </summary>
    private void StartDrawCoroutine()
    {
      this.couroutine = EditorCoroutineUtility.StartCoroutine(this.DrawCoroutine(), this);
    }

    /// <summary>
    /// Calculates seconds per frame.
    /// </summary>
    /// <returns>Seconds per frame.</returns>
    private float CalculateSecondsPerFrame()
    {
      return 1f / this.animation.framesPerSecond;
    }
  }
}