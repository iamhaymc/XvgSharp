using System;
using System.Collections.Generic;
using System.Text;

namespace Xvg.Humanize.Localisation.Ordinalizers
{
    internal class ArmenianOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            if (number == 1 || number == -1)
            {
                return numberString + "-ին";
            }

            return numberString + "-րդ";
        }
    }
}
