using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0A RID: 7946
	[DefOf]
	public static class OrbitalDebrisDefOf
	{
		// Token: 0x0600CAC2 RID: 51906 RVA: 0x004A65A4 File Offset: 0x004A47A4
		static OrbitalDebrisDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OrbitalDebrisDefOf));
		}

		// Token: 0x0400814D RID: 33101
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Asteroid;

		// Token: 0x0400814E RID: 33102
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Manmade;

		// Token: 0x0400814F RID: 33103
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Mechanoid;
	}
}
