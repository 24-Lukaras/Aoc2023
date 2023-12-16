using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal class InputReader : IDisposable
    {
        private const string path = @"C:\Users\Lukin\Documents\temp\aoc2023\";
        private StreamReader _reader;

        public InputReader(int task)
        {
            _reader = new StreamReader($"{path}{task}.txt");
        }

        public string? ReadLine()
        {
            return _reader.ReadLine();
        }

        public string? ReadAll()
        {
            return _reader.ReadToEnd();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(_reader);
            GC.SuppressFinalize(this);
        }
    }
}
