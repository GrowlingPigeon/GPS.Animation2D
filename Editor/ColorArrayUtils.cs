using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace GrowlingPigeonStudio.Animation2D.Editor
{
  /// <summary>
  /// Collection of color array utilities.
  /// </summary>
  internal static class ColorArrayUtils
  {
    /// <summary>
    /// Flips contents of image color buffer horizontally.
    /// </summary>
    /// <param name="buffer">Buffer.</param>
    /// <param name="width">Width.</param>
    public static void FlipBufferHorizontally(Color32[]? buffer, int width)
    {
      if (buffer is null)
      {
        return;
      }

      if (width <= 0)
      {
        return;
      }

      if (buffer.Length % width != 0)
      {
        throw new DataMisalignedException(
          $"Color buffer length {buffer.Length} is incompatable with given width {width}.");
      }

      var workBuffer = new Color32[width];
      for (int index = 0; index < buffer.Length; index += width)
      {
        Array.Copy(buffer, index, workBuffer, 0, width);
        Array.Reverse(workBuffer);
        Array.Copy(workBuffer, 0, buffer, index, width);
      }
    }

    /// <summary>
    /// Flips contents of image color buffer vertically.
    /// </summary>
    /// <param name="buffer">Buffer.</param>
    /// <param name="width">Width.</param>
    public static void FlipBufferVertically(Color32[]? buffer, int width)
    {
      if (buffer is null)
      {
        return;
      }

      if (width <= 0)
      {
        return;
      }

      if (buffer.Length % width != 0)
      {
        throw new DataMisalignedException(
          $"Color buffer length {buffer.Length} is incompatable with given width {width}.");
      }

      int height = buffer.Length / width;
      var workBuffer = new Color32[height];
      for (int index = 0; index < buffer.Length; index += width)
      {
        VerticalCopy(buffer, index, width, workBuffer, 0, 1, height);
        Array.Reverse(workBuffer);
        VerticalCopy(workBuffer, 0, 1, buffer, index, width, height);
      }
    }

    /// <summary>
    /// Vertical copy helper.
    /// </summary>
    /// <typeparam name="T">Array type.</typeparam>
    /// <param name="from">From.</param>
    /// <param name="fromIndex">From index.</param>
    /// <param name="fromSkip">From skip.</param>
    /// <param name="to">To.</param>
    /// <param name="toIndex">To index.</param>
    /// <param name="toSkip">To skip.</param>
    /// <param name="count">Count.</param>
    private static void VerticalCopy<T>(T[] from, int fromIndex, int fromSkip, T[] to, int toIndex, int toSkip, int count)
    {
      int number = 0;
      int iFrom = fromIndex;
      int iTo = toIndex;
      while (number++ < count)
      {
        to[iTo] = from[iFrom];
        iFrom += fromSkip;
        iTo += toSkip;
      }
    }
  }
}