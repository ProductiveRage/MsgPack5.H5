using System;
using System.Reflection;

namespace MessagePack
{
    internal sealed class MemberSummary
    {
        private readonly Action<(object Instance, object ValueToSet)> _setterIfWritable;
        public MemberSummary(Type type, MemberInfo memberInfo, Action<(object Instance, object ValueToSet)> setterIfWritable)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
            _setterIfWritable = setterIfWritable; // This will be null if the member if not settable
        }

        public Type Type { get; }

        public MemberInfo MemberInfo { get; }

        /// <summary>
        /// This will do nothing if the member is not settable
        /// </summary>
        public void SetIfWritable(object instance, object valueToSet) => _setterIfWritable?.Invoke((instance, valueToSet));
    }
}