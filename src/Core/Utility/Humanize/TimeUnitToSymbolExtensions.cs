﻿using System.Globalization;

using Xvg.Humanize.Localisation;
using Xvg.Humanize.Configuration;

namespace Xvg.Humanize;

/// <summary>
/// Transform a time unit into a symbol; e.g. <see cref="TimeUnit.Year"/> => "a"
/// </summary>
public static class TimeUnitToSymbolExtensions
{
  /// <summary>
  /// TimeUnit.Day.ToSymbol() -> "d"
  /// </summary>
  /// <param name="unit">Unit of time to be turned to a symbol</param>
  /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
  /// <returns></returns>
  public static string ToSymbol(this TimeUnit unit, CultureInfo culture = null)
  {
    return Configurator.GetFormatter(culture).TimeUnitHumanize(unit);
  }
}
