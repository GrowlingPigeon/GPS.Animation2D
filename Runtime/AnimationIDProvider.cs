#nullable enable

/// <summary>
/// Animation id provider.
/// </summary>
public class AnimationIDProvider
{
  /// <summary>
  /// Next id.
  /// </summary>
  private int nextID = 1;

  /// <summary>
  /// Provides animation id.
  /// </summary>
  /// <param name="name">Name of animation.</param>
  /// <returns>Generated animation id.</returns>
  public AnimationID GetAnimationID(string? name)
  {
    return new AnimationID(name, this.nextID++);
  }
}