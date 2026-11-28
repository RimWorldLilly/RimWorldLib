using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E83 RID: 7811
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x0600CA3E RID: 51774 RVA: 0x004A5CE0 File Offset: 0x004A3EE0
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x04007A27 RID: 31271
		public static BodyDef Human;

		// Token: 0x04007A28 RID: 31272
		public static BodyDef MechanicalCentipede;
	}
}
