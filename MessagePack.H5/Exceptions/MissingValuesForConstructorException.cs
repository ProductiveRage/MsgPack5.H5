using System;

namespace MessagePack
{
    /// <summary>
    /// This will be throw if a target type is encountered that must be instantiated via a non-parameterless constructor where the number of keyed members is insufficient to populate the constructor parameters
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