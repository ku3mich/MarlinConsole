using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarlinConsole.Extensions
{
    public static class Parse
    {
        public static double? Double(string num) => string.IsNullOrWhiteSpace(num) ? null : double.Parse(num);
        public static int? Int(string num) => string.IsNullOrWhiteSpace(num) ? null : int.Parse(num);
    }
}
