using System;

namespace Verse
{
	// Token: 0x0200022B RID: 555
	public static class TranslatorFormattedStringExtensions
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x0005D5E9 File Offset: 0x0005B7E9
		public static TaggedString Translate(this string key, NamedArgument arg1)
		{
			return key.Translate().Formatted(arg1);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0005D5F7 File Offset: 0x0005B7F7
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2)
		{
			return key.Translate().Formatted(arg1, arg2);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0005D606 File Offset: 0x0005B806
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return key.Translate().Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0005D616 File Offset: 0x0005B816
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0005D628 File Offset: 0x0005B828
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005D63C File Offset: 0x0005B83C
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0005D652 File Offset: 0x0005B852
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0005D66C File Offset: 0x0005B86C
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0005D691 File Offset: 0x0005B891
		public static TaggedString Translate(this string key, params NamedArgument[] args)
		{
			return key.Translate().Formatted(args);
		}
	}
}
