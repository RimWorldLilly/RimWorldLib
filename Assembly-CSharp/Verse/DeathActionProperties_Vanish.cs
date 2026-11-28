using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000CB RID: 203
	public class DeathActionProperties_Vanish : DeathActionProperties
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0002C92D File Offset: 0x0002AB2D
		public DeathActionProperties_Vanish()
		{
			this.workerClass = typeof(DeathActionWorker_Vanish);
		}

		// Token: 0x0400036B RID: 875
		public FleckDef fleck;

		// Token: 0x0400036C RID: 876
		public ThingDef filth;

		// Token: 0x0400036D RID: 877
		public IntRange filthCountRange = IntRange.One;

		// Token: 0x0400036E RID: 878
		public FleshbeastUtility.MeatExplosionSize? meatExplosionSize;
	}
}
