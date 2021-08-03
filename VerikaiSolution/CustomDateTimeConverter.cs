using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace VerikaiSolution
{
    public class CustomDateTimeConverter : ConverterBase
    {

        public override object StringToField(string from)
        {
            return DateTime.Parse(from).ToShortDateString();
        }
    }
}
