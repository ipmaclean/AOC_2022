﻿namespace AOC_2022.Day2
{
    internal class Day2InputHelper : InputHelper<List<int>>
    {
        public Day2InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<int> Parse()
        {
            var output = new List<int>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                }
            }
            return output;
        }
    }
}