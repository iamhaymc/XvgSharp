﻿using System;

namespace Xvg.Humanize.Localisation.DateToOrdinalWords;

internal class FrDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
  public override string Convert(DateTime date)
  {
    var day = date.Day > 1 ? date.Day.ToString() : date.Day.Ordinalize();
    return day + date.ToString(" MMMM yyyy");
  }
}
