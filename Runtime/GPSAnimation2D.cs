using UnityEngine;

#nullable enable

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
  /// Number of frames per second.
  /// </summary>
  public float framesPerSecond = 16;

  /// <summary>
  /// Sprites.
  /// </summary>
  public Sprite[]? sprites;
}