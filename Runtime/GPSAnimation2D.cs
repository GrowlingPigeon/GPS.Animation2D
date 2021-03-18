using GrowlingPigeonStudio.Utilities;
using UnityEngine;

#nullable enable

namespace GrowlingPigeonStudio.Animation2D
{
  /// <summary>
  /// GPS animation 2D.
  /// </summary>
  [CreateAssetMenu(fileName = "NewGPSAnimation", menuName = "ScriptableObjects/GPSAnimation")]
  public class GPSAnimation2D
    : ScriptableObject
  {
    /// <summary>
    /// Animation behavior option.
    /// </summary>
    public GPSAnimationOptionsEnum animationOption = GPSAnimationOptionsEnum.Loop;

    /// <summary>
    /// Whether this animation must finish before playing other blocks.
    /// </summary>
    public bool blockOtherAnimations;

    /// <summary>
    /// Animation transforms.
    /// </summary>
    [EnumFlag]
    public AnimationTransforms transform;

    /// <summary>
    /// Number of frames per second.
    /// </summary>
    [Min(1)]
    public float framesPerSecond = 16;

    /// <summary>
    /// Sprites.
    /// </summary>
    public Sprite[]? sprites;
  }
}