#nullable enable

namespace GrowlingPigeonStudio.Animation2D
{
  /// <summary>
  /// GPS animation options enum.
  /// </summary>
  public enum GPSAnimationOptionsEnum
    : byte
  {
    /// <summary>
    /// Animation loops.
    /// </summary>
    Loop = 0,

    /// <summary>
    /// Animation falls back to previous animation.
    /// </summary>
    BackToPrevious = 1,

    /// <summary>
    /// Fire animation ended event to trigger re-calculation.
    /// </summary>
    AnimationEndedEvent = 2
  }
}