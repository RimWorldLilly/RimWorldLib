using System;

namespace Verse
{
	// Token: 0x020005EE RID: 1518
	public static class ModToolUtility
	{
		// Token: 0x06002FB4 RID: 12212 RVA: 0x00131AF7 File Offset: 0x0012FCF7
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
