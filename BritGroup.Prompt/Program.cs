using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BritGroup.Application;
using Microsoft.Extensions.DependencyInjection;

namespace BritGroup.Prompt
{
    class Program
    {
        static string _startingCounter;
        private static StringBuilder _sbText = new StringBuilder();

        static void Main(string[] args)
        {
            ProcessFile();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IParseNumbers, ParseNumbers>(x => new ParseNumbers(_startingCounter))
                .BuildServiceProvider();

            var parser = serviceProvider.GetService<IParseNumbers>();
            var result = parser.Parse(_sbText.ToString().ToLower());

            Console.WriteLine($"Result is : {result}");

            Console.ReadLine();
        }

        public static void ProcessFile()
        {
            var fileLines = File.ReadAllLines("D:\\test.txt");

            var lastLine = fileLines.Last();
            
            _startingCounter = Regex.Split(lastLine, @"\D+").FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            foreach (var singleLine in fileLines)
            {
                _sbText.Append(singleLine);
            }
        }
    }
}
