using System;

namespace RimWorld
{
	// Token: 0x02001F02 RID: 7938
	[DefOf]
	public static class AnomalyPlaystyleDefOf
	{
		// Token: 0x0600CABA RID: 51898 RVA: 0x004A651C File Offset: 0x004A471C
		static AnomalyPlaystyleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AnomalyPlaystyleDefOf));
		}

		// Token: 0x0400811A RID: 33050
		[MayRequireAnomaly]
		public static AnomalyPlaystyleDef Standard;

		// Token: 0x0400811B RID: 33051
		[MayRequireAnomaly]
		public static AnomalyPlaystyleDef AmbientHorror;
	}
}
