using System;

namespace RimWorld
{
	// Token: 0x02001EE9 RID: 7913
	[DefOf]
	public static class ComplexThreatDefOf
	{
		// Token: 0x0600CAA3 RID: 51875 RVA: 0x004A6395 File Offset: 0x004A4595
		static ComplexThreatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ComplexThreatDefOf));
		}

		// Token: 0x040080BD RID: 32957
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingInsects;

		// Token: 0x040080BE RID: 32958
		[MayRequireIdeology]
		public static ComplexThreatDef Infestation;

		// Token: 0x040080BF RID: 32959
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingMechanoids;

		// Token: 0x040080C0 RID: 32960
		[MayRequireIdeology]
		public static ComplexThreatDef CryptosleepPods;
	}
}
