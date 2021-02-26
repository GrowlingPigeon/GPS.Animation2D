using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#pragma warning disable UNT0011 // ScriptableObject instance creation

public class GPSAnimator2DTests
{
  private static readonly GPSAnimation2D Animation_Loop1 = new GPSAnimation2D
  {
    name = nameof(Animation_Loop1),
    animationOption = GPSAnimationOptionsEnum.Loop,
    blockOtherAnimations = false,
    framesPerSecond = 30,
    sprites = new Sprite[]
    {
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero)
    }
  };

  private static readonly GPSAnimation2D Animation_Loop2 = new GPSAnimation2D
  {
    name = nameof(Animation_Loop2),
    animationOption = GPSAnimationOptionsEnum.Loop,
    blockOtherAnimations = false,
    framesPerSecond = 30,
    sprites = new Sprite[]
    {
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero)
    }
  };

  private static readonly GPSAnimation2D Animation_ReturnToPrevious = new GPSAnimation2D
  {
    name = nameof(Animation_ReturnToPrevious),
    animationOption = GPSAnimationOptionsEnum.BackToPrevious,
    blockOtherAnimations = false,
    framesPerSecond = 30,
    sprites = new Sprite[]
    {
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero)
    }
  };

  private static readonly GPSAnimation2D Animation_AnimationEndEvent = new GPSAnimation2D
  {
    name = nameof(Animation_AnimationEndEvent),
    animationOption = GPSAnimationOptionsEnum.AnimationEndedEvent,
    blockOtherAnimations = false,
    framesPerSecond = 30,
    sprites = new Sprite[]
    {
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero)
    }
  };

  private static readonly GPSAnimation2D Animation_BlockOthers = new GPSAnimation2D
  {
    name = nameof(Animation_BlockOthers),
    animationOption = GPSAnimationOptionsEnum.AnimationEndedEvent,
    blockOtherAnimations = true,
    framesPerSecond = 30,
    sprites = new Sprite[]
    {
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero),
      Sprite.Create(null, Rect.zero, Vector2.zero)
    }
  };

#pragma warning restore UNT0011 // ScriptableObject instance creation

  [UnityTest]
  public IEnumerator GetAnimationFrame_AnimationProgresses()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);

    animator.SetAnimation(nameof(Animation_Loop1));
    while (animator.CurrentFrame == 0)
    {
      yield return null;
    }

    Assert.AreNotEqual(0, animator.CurrentFrame);
  }

  [UnityTest]
  public IEnumerator GetAnimationFrame_JustSwitched()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    animator.AddAnimation(Animation_Loop2);

    animator.SetAnimation(nameof(Animation_Loop1));
    while (animator.CurrentFrame == 0)
    {
      yield return null;
    }

    animator.SetAnimation(nameof(Animation_Loop2));

    Assert.AreEqual(0, animator.CurrentFrame);
  }

  [UnityTest]
  public IEnumerator AnimationLoops()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);

    animator.SetAnimation(nameof(Animation_Loop1));
    while (animator.CurrentFrame == 0)
    {
      yield return null;
    }

    while (animator.CurrentFrame != 0)
    {
      yield return null;
    }

    Assert.AreEqual(0, animator.CurrentFrame);
  }

  [UnityTest]
  public IEnumerator GetCurrentAnimation()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    animator.AddAnimation(Animation_Loop2);

    animator.SetAnimation(nameof(Animation_Loop1));
    Assert.AreEqual(nameof(Animation_Loop1), animator.CurrentAnimation);
    yield return null;
    animator.SetAnimation(nameof(Animation_Loop2));
    Assert.AreEqual(nameof(Animation_Loop2), animator.CurrentAnimation);
  }

  [UnityTest]
  public IEnumerator AnimationReturnsToPrevious()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    animator.AddAnimation(Animation_ReturnToPrevious);

    animator.SetAnimation(nameof(Animation_Loop1));
    yield return null;
    animator.SetAnimation(nameof(Animation_ReturnToPrevious));
    while (animator.CurrentAnimation == nameof(Animation_ReturnToPrevious))
    {
      yield return null;
    }

    Assert.AreEqual(nameof(Animation_Loop1), animator.CurrentAnimation);
  }

  [UnityTest]
  public IEnumerator AnimationFiresFinishedEvent()
  {
    bool called = false;
    string eventParameter = string.Empty;

    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_AnimationEndEvent);
    animator.EndOfAnimationEvent += x =>
    {
      called = true;
      eventParameter = x;
    };

    animator.SetAnimation(nameof(Animation_AnimationEndEvent));
    while (!called)
    {
      yield return null;
    }

    Assert.IsTrue(called);
    Assert.AreEqual(nameof(Animation_AnimationEndEvent), eventParameter);
  }

  [UnityTest]
  public IEnumerator UpdatesSpriteInRenderer()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    var spriteRenderer = go.AddComponent<SpriteRenderer>();
    animator.SetTargetRenderer(spriteRenderer);

    animator.SetAnimation(nameof(Animation_Loop1));
    yield return null;
    Assert.IsTrue(ReferenceEquals(Animation_Loop1.sprites[0], spriteRenderer.sprite));
    while (animator.CurrentFrame != 2)
    {
      yield return null;
    }

    Assert.IsTrue(ReferenceEquals(Animation_Loop1.sprites[2], spriteRenderer.sprite));
  }

  [UnityTest]
  public IEnumerator BlockAnimation_BlocksAnimation()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    animator.AddAnimation(Animation_BlockOthers);

    animator.SetAnimation(nameof(Animation_BlockOthers));
    yield return null;
    animator.SetAnimation(nameof(Animation_Loop1));

    Assert.AreEqual(nameof(Animation_BlockOthers), animator.CurrentAnimation);
  }

  [UnityTest]
  public IEnumerator BlockAnimation_PlayAsNext()
  {
    var go = new GameObject();
    var animator = go.AddComponent<GPSAnimator2D>();
    animator.AddAnimation(Animation_Loop1);
    animator.AddAnimation(Animation_BlockOthers);

    animator.SetAnimation(nameof(Animation_BlockOthers));
    yield return null;
    animator.SetAnimation(nameof(Animation_Loop1));
    Assert.AreEqual(nameof(Animation_BlockOthers), animator.CurrentAnimation);

    while (animator.CurrentAnimation == nameof(Animation_BlockOthers))
    {
      yield return null;
    }

    Assert.AreEqual(nameof(Animation_Loop1), animator.CurrentAnimation);
  }
}