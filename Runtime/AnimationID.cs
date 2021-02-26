using GrowlingPigeonStudio.Utilities;

#nullable enable

/// <summary>
/// Animation id.
/// </summary>
public readonly struct AnimationID
{
  /// <summary>
  /// Name for animation id.
  /// </summary>
  public readonly string name;

  /// <summary>
  /// ID for animation id.
  /// </summary>
  public readonly int id;

  /// <summary>
  /// Initializes a new instance of the <see cref="AnimationID"/> struct.
  /// </summary>
  /// <param name="name">Name.</param>
  /// <param name="id">Id.</param>
  public AnimationID(string? name, int id)
  {
    if (name is null)
    {
      GPSLogger.LogError("Name cannot be null!");
      name = string.Empty;
    }

    this.name = string.Intern(name);
    this.id = id;
  }
}