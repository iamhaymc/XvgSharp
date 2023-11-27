using System;

namespace Xvg.Humanize.Localisation.DateToOrdinalWords;

internal class EsDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
  public override string Convert(DateTime date)
  {
    return date.ToString("d 'de' MMMM 'de' yyyy");
  }
}
