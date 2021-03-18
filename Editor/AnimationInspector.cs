using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

#nullable enable

namespace GrowlingPigeonStudio.Animation2D.Editor
{
  /// <summary>
  /// Animation inspector.
  /// </summary>
  [CustomEditor(typeof(GPSAnimation2D))]
  [CanEditMultipleObjects]
  public class AnimationInspector
    : UnityEditor.Editor
  {
    /// <summary>
    /// Local animation.
    /// </summary>
    private EditorAnimation2D? animation;

    /// <summary>
    /// On inspector GUI redraw.
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (this.targets.Length == 1)
      {
        var targetAnimation = (GPSAnimation2D)this.targets[0];
        if (this.animation is null)
        {
          this.animation = new EditorAnimation2D(targetAnimation, this);
        }
        else
        {
          this.animation.SetAnimation(targetAnimation);
        }

        this.animation.Draw();
      }
      else
      {
        this.DisableAndNullAnimation();
      }
    }

    /// <summary>
    /// Called when destroyed.
    /// </summary>
    private void OnDestroy()
    {
      this.DisableAndNullAnimation();
    }

    /// <summary>
    /// Called when disabled.
    /// </summary>
    private void OnDisable()
    {
      this.DisableAndNullAnimation();
    }

    /// <summary>
    /// Disables all animations.
    /// </summary>
    private void DisableAndNullAnimation()
    {
      if (this.animation is { })
      {
        this.animation.Disable();
        this.animation = null;
      }
    }
  }
}