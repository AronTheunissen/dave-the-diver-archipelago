// Polyfill: provides NullableAttribute so the compiler can handle inline ? annotations
// even when the referenced il2cppmscorlib.dll doesn't include it.
// This file can be removed if the interop DLLs are updated to a version that includes it.
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field |
                    AttributeTargets.GenericParameter | AttributeTargets.Parameter |
                    AttributeTargets.Property | AttributeTargets.ReturnValue,
                    AllowMultiple = false, Inherited = false)]
    internal sealed class NullableAttribute : Attribute
    {
        public readonly byte[] NullableFlags;
        public NullableAttribute(byte P_0) { NullableFlags = new byte[] { P_0 }; }
        public NullableAttribute(byte[] P_0) { NullableFlags = P_0; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate |
                    AttributeTargets.Interface | AttributeTargets.Method |
                    AttributeTargets.Module | AttributeTargets.Struct,
                    AllowMultiple = false, Inherited = false)]
    internal sealed class NullableContextAttribute : Attribute
    {
        public readonly byte Flag;
        public NullableContextAttribute(byte P_0) { Flag = P_0; }
    }
}
