using Common;
using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main()
        {
            int validPasswordsCount = 0;
            foreach (string line in File.ReadLines(Constants.FILE_NAME))
            {
                var pp = new PasswordPolicy(line);
                if (pp.IsValid())
                {
                    validPasswordsCount++;
                }
            }
            Console.WriteLine($"Number of valid passwords: {validPasswordsCount}");
        }
    }

    class PasswordPolicy
    {
        private readonly int MinCount;

        private readonly int MaxCount;

        private readonly char Symbol;

        private readonly string Password;

        public PasswordPolicy(string policy)
        {
            // policy is of form "1-3 b: cdefg"
            // "1" represents minimum number of appearances
            // "3" represents maximum number of appearances
            // "b" represents symbol to which these constraints apply
            // "cdefg" represents password where constraints must be checked
            string[] temp;
            temp = policy.Split(":");
            Password = temp[1].Trim();
            temp = temp[0].Split(" ");
            Symbol = temp[1][0];
            temp = temp[0].Split("-");
            MinCount = Int32.Parse(temp[0]);
            MaxCount = Int32.Parse(temp[1]);
        }

        public bool IsValid()
        {
            // password is valid if it contains between MinCount and MaxCount appearances of Symbol
            int symbolCount = Password.Count(c => c == Symbol);
            return MinCount <= symbolCount && symbolCount <= MaxCount;
        }
    }
}
