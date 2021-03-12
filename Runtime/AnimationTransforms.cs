using System;

namespace GrowlingPigeonStudio.Animation2D
{
  /// <summary>
  /// Animation transform flags.
  /// </summary>
  [Flags]
  public enum AnimationTransforms
    : byte
  {
    /// <summary>
    /// No transform.
    /// </summary>
    NoTransform = 0x00,

    /// <summary>
    /// FLip horizontally.
    /// </summary>
    FlipHorizontal = 0x01,

    /// <summary>
    /// Flip vertically.
    /// </summary>
    FlipVertical = 0x02
  }
}