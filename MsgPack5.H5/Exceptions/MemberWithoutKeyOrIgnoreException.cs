using System;
using System.Reflection;

namespace MessagePack
{
    /// <summary>
    /// This will be thrown if a member is encountered on a type that is to be deserialised that is identified as a MessagePackObject but that member has neither a Key nor IgnoreMember attribute on it
    /// </summary>
    public sealed class MemberWithoutKeyOrIgnoreException : Exception
    {
        public MemberWithoutKeyOrIgnoreException(Type type, MemberInfo member) : base(GetMessage(type, member))
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Member = member ?? throw new ArgumentNullException(nameof(member));
        }

        public Type Type { get; }
        public MemberInfo Member { get; }

        private static string GetMessage(Type type, MemberInfo member) => $"All fields and properties in [MessagePackObject] type must have either [Key(..)] or [IgnoreMember] attributes, which is not the case for type {type?.FullName} and its member {member?.Name}";
    }
}