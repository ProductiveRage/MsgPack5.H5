using System;

namespace MessagePack
{
    /// <summary>
    /// This will be thrown if a target type is encountered and there are no constructors that can be satisfed with the number of values in the available data
    /// </summary>
    public sealed class MissingValuesForConstructorException : Exception
    {
        public MissingValuesForConstructorException(Type type, uint numberOfValuesAvailable) : base(GetMessage(type, numberOfValuesAvailable))
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            NumberOfValuesAvailable = numberOfValuesAvailable;
        }

        public Type Type { get; }
        public uint NumberOfValuesAvailable { get; }

        private static string GetMessage(Type type, uint numberOfValuesAvailable) => $"There are no available constructors on type {type.FullName} that can be satisfed with the number of available values ({numberOfValuesAvailable})";
    }
}