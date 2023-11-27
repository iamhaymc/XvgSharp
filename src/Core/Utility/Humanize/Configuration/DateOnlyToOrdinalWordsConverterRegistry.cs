#if NET6_0_OR_GREATER
using Xvg.Humanize.Localisation.DateToOrdinalWords;

namespace Xvg.Humanize.Configuration
{
    internal class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
    {
        public DateOnlyToOrdinalWordsConverterRegistry() : base(new DefaultDateOnlyToOrdinalWordConverter())
        {
            Register("en-US", new UsDateOnlyToOrdinalWordsConverter());
            Register("fr", new FrDateOnlyToOrdinalWordsConverter());
            Register("es", new EsDateOnlyToOrdinalWordsConverter());
        }
    }
}
#endif
