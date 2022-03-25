using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sigma
{
    public class Generator
    {
        private static Random rnd = new Random();
        private static string numbers = "0123456789";
        public static string GenerateCode()
        {
            string code = "";

            for (int i = 0; i < 6; i++) {
                code += numbers[rnd.Next(numbers.Length)];
            }
            return code;
        }
    }
}