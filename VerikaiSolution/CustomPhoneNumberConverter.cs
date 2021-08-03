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
    public class CustomPhoneNumberConverter : ConverterBase
    {

        public override object StringToField(string from)
        {

            var output = Regex.Replace(from, @"[^\d]", "");

            return Int64.Parse(output.Substring(0, 10));


        }
    }
}
