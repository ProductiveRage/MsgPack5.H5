using System;
using System.Reflection;

namespace MessagePack
{
    internal sealed class MemberSummary
    {
        private readonly Action<(object Instance, object ValueToSet)> _setter;
        public MemberSummary(Type type, MemberInfo memberInfo, Action<(object Instance, object ValueToSet)> setter)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
            _setter = setter ?? throw new ArgumentNullException(nameof(setter));
        }
        public Type Type { get; }
        public MemberInfo MemberInfo { get; }
        public void Set(object instance, object valueToSet) => _setter((instance, valueToSet));
    }
}