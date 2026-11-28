using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184F RID: 6223
	public class CompProperties_Chimera : CompProperties
	{
		// Token: 0x06009BD2 RID: 39890 RVA: 0x0038279E File Offset: 0x0038099E
		public CompProperties_Chimera()
		{
			this.compClass = typeof(CompChimera);
		}

		// Token: 0x04005CA8 RID: 23720
		public float rageEndHealthPercentThreshold = 0.98f;

		// Token: 0x04005CA9 RID: 23721
		[MustTranslate]
		public string simpleAnimalLabel;
	}
}
