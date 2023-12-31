﻿namespace Xvg.Humanize;

/// <summary>
/// Options for specifying readable clock notation
/// </summary>
public enum ClockNotationRounding
{
  /// <summary>
  /// Do not round minutes
  /// </summary>
  None,

  /// <summary>
  /// Round time to nearest five minutes
  /// </summary>
  NearestFiveMinutes
}
