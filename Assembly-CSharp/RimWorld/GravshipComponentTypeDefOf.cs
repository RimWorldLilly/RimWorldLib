using System;

namespace RimWorld
{
	// Token: 0x02001F08 RID: 7944
	[DefOf]
	public static class GravshipComponentTypeDefOf
	{
		// Token: 0x0600CAC0 RID: 51904 RVA: 0x004A6582 File Offset: 0x004A4782
		static GravshipComponentTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GravshipComponentTypeDefOf));
		}

		// Token: 0x04008148 RID: 33096
		[MayRequireOdyssey]
		public static GravshipComponentTypeDef SignalJammer;
	}
}
