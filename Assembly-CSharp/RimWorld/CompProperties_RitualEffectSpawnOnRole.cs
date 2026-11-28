using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001749 RID: 5961
	public class CompProperties_RitualEffectSpawnOnRole : CompProperties_RitualEffectSpawnOnPawn
	{
		// Token: 0x06009456 RID: 37974 RVA: 0x00360F78 File Offset: 0x0035F178
		public CompProperties_RitualEffectSpawnOnRole()
		{
			this.compClass = typeof(CompRitualEffect_SpawnOnRole);
		}

		// Token: 0x04005992 RID: 22930
		[NoTranslate]
		public string roleId;
	}
}
