using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using System.Text.RegularExpressions;

namespace VerikaiSolution
{
    public class CustomGenderConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            char? Gender = null;
            if (from.ToLower().StartsWith("f"))
                Gender = 'F';
            else if (from.ToLower().StartsWith("m"))
                Gender = 'M';
            return Gender;


        }
    }
}
