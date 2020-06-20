using System;
using System.Reflection.Emit;

namespace VoidMain.Text.Templates.Formatter
{
    public class ValueFormatter : IValueFormatter
    {
        private readonly TryFormatToSpanFunc _tryFormatToSpan;

        public ValueFormatter()
        {
            _tryFormatToSpan = TryFormatToSpanDelegate.Value;
        }

        public bool TryFormatToSpan(object value, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, Span<char> destination, out int charsWritten)
        {
            if (_tryFormatToSpan == null)
            {
                charsWritten = 0;
                return false;
            }
            return _tryFormatToSpan(value, destination, out charsWritten, format, formatProvider);
        }

        public string Format(object value, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, ICustomFormatter customFormatter)
        {
            string result = null;
            string formatAsString = null;

            if (customFormatter != null)
            {
                if (!format.IsEmpty)
                {
                    formatAsString = new string(format);
                }

                result = customFormatter.Format(formatAsString, value, formatProvider);
            }

            if (result == null)
            {
                if (value is IFormattable formattable)
                {
                    if (formatAsString == null && !format.IsEmpty)
                    {
                        formatAsString = new string(format);
                    }

                    result = formattable.ToString(formatAsString, formatProvider);
                }
                else
                {
                    result = value?.ToString() ?? String.Empty;
                }
            }

            return result;
        }

        public static readonly Lazy<TryFormatToSpanFunc> TryFormatToSpanDelegate = new Lazy<TryFormatToSpanFunc>(EmitTryFormatToSpan);

        public delegate bool TryFormatToSpanFunc(
            object value, Span<char> destination, out int charsWritten,
            ReadOnlySpan<char> format, IFormatProvider provider);

        private static TryFormatToSpanFunc EmitTryFormatToSpan()
        {
            var type = typeof(int).GetInterface("ISpanFormattable");
            if (type == null)
            {
                return null;
            }

            var method = type.GetMethod("TryFormat");

            var parameters = new[]
            {
                typeof(object),
                typeof(Span<char>),
                typeof(int).MakeByRefType(),
                typeof(ReadOnlySpan<char>),
                typeof(IFormatProvider)
            };

            var dynamicMethod = new DynamicMethod("TryFormatToSpan", typeof(bool), parameters, typeof(object).Module);

            var il = dynamicMethod.GetILGenerator();
            il.DeclareLocal(type);
            var lable = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Isinst, type);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Brfalse_S, lable);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Ldarg_3);
            il.Emit(OpCodes.Ldarg_S, 4);
            il.Emit(OpCodes.Callvirt, method);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(lable);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stind_I4);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var func = (TryFormatToSpanFunc)dynamicMethod.CreateDelegate(typeof(TryFormatToSpanFunc));

            return func;
        }
    }
}
