using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace VerikaiSolution
{
    [DelimitedRecord("\t"), IgnoreFirst(1)]
    public class Data
    {
        [FieldOrder(1)]
        public string first_name;

        [FieldOrder(2)]
        public string last_name;

        [FieldOrder(3)]
        [FieldConverter(typeof(CustomGenderConverter))]
        public char gender;

        [FieldOrder(4)]
        [FieldConverter(typeof(CustomDateTimeConverter))]
        public string dob;

        [FieldOrder(5)]
        public string state;

        [FieldOrder(6)]
        [FieldConverter(typeof(CustomPhoneNumberConverter))]
        public Int64 phone;

        [FieldOrder(7)]
        public string zip;

        [FieldOptional]
        [FieldOrder(8)]
        public int? age;

        [FieldOptional]
        [FieldOrder(9)]
        public decimal? cost;



    }
}
