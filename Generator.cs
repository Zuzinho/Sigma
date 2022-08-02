using System;

namespace Sigma
{
    public class Generator
    {
        private static readonly Random _rnd = new Random();
        private static readonly string _numbers = "0123456789";
        public static string GenerateCode()
        {
            string code = "";

            for (int i = 0; i < 6; i++) {
                code += _numbers[_rnd.Next(_numbers.Length)];
            }
            return code;
        }
    }
}