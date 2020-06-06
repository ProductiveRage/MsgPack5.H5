﻿using System;
using System.Text;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestLongerString : ConcreteTypeTestItem<string>
    {
        public TestLongerString() : base(Repeat(UpperCaseAlphabetString, 100)) { }

        private static readonly string UpperCaseAlphabetString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static string Repeat(string value, uint repetitions)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("may not be null or empty", nameof(value));
            if (repetitions == 0)
                throw new ArgumentOutOfRangeException("must not be zero", nameof(repetitions));

            var content = new StringBuilder();
            for (uint i = 0; i < repetitions; i++)
                content.Append(value);
            return content.ToString();
        }
    }
}