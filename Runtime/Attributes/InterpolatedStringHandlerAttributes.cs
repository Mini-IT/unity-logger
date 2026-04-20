namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false)]
    internal sealed class InterpolatedStringHandlerAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class InterpolatedStringHandlerArgumentAttribute : Attribute
    {
        public InterpolatedStringHandlerArgumentAttribute(string argument) { }
        public InterpolatedStringHandlerArgumentAttribute(params string[] arguments) { }
    }
}
