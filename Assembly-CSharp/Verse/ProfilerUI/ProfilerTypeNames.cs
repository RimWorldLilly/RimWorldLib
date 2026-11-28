using System;
using System.Collections.Generic;
using System.Text;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A4 RID: 1956
	internal static class ProfilerTypeNames
	{
		// Token: 0x06003EE2 RID: 16098 RVA: 0x0019094C File Offset: 0x0018EB4C
		public static string Get(Type type)
		{
			string text;
			if (ProfilerTypeNames.Cache.TryGetValue(type, out text))
			{
				return text;
			}
			ProfilerTypeNames.Builder.Clear();
			ProfilerTypeNames.Append(ProfilerTypeNames.Builder, type);
			text = ProfilerTypeNames.Builder.ToString();
			ProfilerTypeNames.Cache[type] = text;
			return text;
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00190998 File Offset: 0x0018EB98
		private static void Append(StringBuilder builder, Type type)
		{
			string value;
			if (ProfilerTypeNames.PrimitiveNames.TryGetValue(type, out value))
			{
				builder.Append(value);
				return;
			}
			if (type.IsArray)
			{
				ProfilerTypeNames.Append(builder, type.GetElementType());
				builder.Append('[');
				builder.Append(',', type.GetArrayRank() - 1);
				builder.Append(']');
				return;
			}
			if (type.IsGenericParameter)
			{
				builder.Append(type.Name);
				return;
			}
			Type[] genericArgs = type.IsGenericType ? type.GetGenericArguments() : Type.EmptyTypes;
			int num;
			ProfilerTypeNames.AppendSegments(builder, type, genericArgs, out num);
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00190A2C File Offset: 0x0018EC2C
		private static void AppendSegments(StringBuilder builder, Type type, Type[] genericArgs, out int argsConsumed)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				ProfilerTypeNames.AppendSegments(builder, declaringType, genericArgs, out argsConsumed);
				builder.Append('.');
			}
			else
			{
				argsConsumed = 0;
				if (!string.IsNullOrEmpty(type.Namespace))
				{
					builder.Append(type.Namespace);
					builder.Append('.');
				}
			}
			string name = type.Name;
			int num = name.IndexOf('`');
			if (num < 0)
			{
				builder.Append(name);
				return;
			}
			builder.Append(name, 0, num);
			int num2;
			if (!int.TryParse(name.AsSpan(num + 1), out num2) || num2 <= 0)
			{
				return;
			}
			builder.Append('<');
			int num3 = 0;
			while (num3 < num2 && argsConsumed + num3 < genericArgs.Length)
			{
				if (num3 > 0)
				{
					builder.Append(", ");
				}
				ProfilerTypeNames.Append(builder, genericArgs[argsConsumed + num3]);
				num3++;
			}
			builder.Append('>');
			argsConsumed += num2;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00190B0C File Offset: 0x0018ED0C
		// Note: this type is marked as 'beforefieldinit'.
		static ProfilerTypeNames()
		{
			Dictionary<Type, string> dictionary = new Dictionary<Type, string>();
			Type typeFromHandle = typeof(void);
			dictionary[typeFromHandle] = "void";
			Type typeFromHandle2 = typeof(bool);
			dictionary[typeFromHandle2] = "bool";
			Type typeFromHandle3 = typeof(byte);
			dictionary[typeFromHandle3] = "byte";
			Type typeFromHandle4 = typeof(sbyte);
			dictionary[typeFromHandle4] = "sbyte";
			Type typeFromHandle5 = typeof(char);
			dictionary[typeFromHandle5] = "char";
			Type typeFromHandle6 = typeof(short);
			dictionary[typeFromHandle6] = "short";
			Type typeFromHandle7 = typeof(ushort);
			dictionary[typeFromHandle7] = "ushort";
			Type typeFromHandle8 = typeof(int);
			dictionary[typeFromHandle8] = "int";
			Type typeFromHandle9 = typeof(uint);
			dictionary[typeFromHandle9] = "uint";
			Type typeFromHandle10 = typeof(long);
			dictionary[typeFromHandle10] = "long";
			Type typeFromHandle11 = typeof(ulong);
			dictionary[typeFromHandle11] = "ulong";
			Type typeFromHandle12 = typeof(float);
			dictionary[typeFromHandle12] = "float";
			Type typeFromHandle13 = typeof(double);
			dictionary[typeFromHandle13] = "double";
			Type typeFromHandle14 = typeof(decimal);
			dictionary[typeFromHandle14] = "decimal";
			Type typeFromHandle15 = typeof(object);
			dictionary[typeFromHandle15] = "object";
			Type typeFromHandle16 = typeof(string);
			dictionary[typeFromHandle16] = "string";
			Type typeFromHandle17 = typeof(IntPtr);
			dictionary[typeFromHandle17] = "nint";
			Type typeFromHandle18 = typeof(UIntPtr);
			dictionary[typeFromHandle18] = "nuint";
			ProfilerTypeNames.PrimitiveNames = dictionary;
		}

		// Token: 0x04002980 RID: 10624
		private static readonly Dictionary<Type, string> Cache = new Dictionary<Type, string>();

		// Token: 0x04002981 RID: 10625
		private static readonly StringBuilder Builder = new StringBuilder(128);

		// Token: 0x04002982 RID: 10626
		private static readonly Dictionary<Type, string> PrimitiveNames;
	}
}
