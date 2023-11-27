#if NET6_0_OR_GREATER

using System;

using Xvg.Humanize.Configuration;

namespace Xvg.Humanize.Localisation.DateToOrdinalWords
{
    internal class EsDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
    {
        public override string Convert(DateOnly date)
        {
            var equivalentDateTime = date.ToDateTime(TimeOnly.MinValue);
            return Configurator.DateToOrdinalWordsConverter.Convert(equivalentDateTime);
        }
    }
}

#endif
