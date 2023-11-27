#if NET6_0_OR_GREATER

using Xvg.Humanize.Localisation.TimeToClockNotation;

namespace Xvg.Humanize.Configuration
{
    internal class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
    {
        public TimeOnlyToClockNotationConvertersRegistry() : base(new DefaultTimeOnlyToClockNotationConverter())
        {
            Register("pt-BR", new BrazilianPortugueseTimeOnlyToClockNotationConverter());
            Register("fr", new FrTimeOnlyToClockNotationConverter());
            Register("es", new EsTimeOnlyToClockNotationConverter());
        }
    }
}

#endif
