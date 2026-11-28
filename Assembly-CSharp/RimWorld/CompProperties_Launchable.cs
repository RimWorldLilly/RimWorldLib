using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D5 RID: 6357
	public class CompProperties_Launchable : CompProperties
	{
		// Token: 0x06009F61 RID: 40801 RVA: 0x003922A1 File Offset: 0x003904A1
		public CompProperties_Launchable()
		{
			this.compClass = typeof(CompLaunchable);
		}

		// Token: 0x04005E45 RID: 24133
		public int fixedLaunchDistanceMax = -1;

		// Token: 0x04005E46 RID: 24134
		public float fuelPerTile = 2.25f;

		// Token: 0x04005E47 RID: 24135
		public float minFuelCost = -1f;

		// Token: 0x04005E48 RID: 24136
		public ThingDef skyfallerLeaving;

		// Token: 0x04005E49 RID: 24137
		public ThingDef activeTransporterDef;

		// Token: 0x04005E4A RID: 24138
		public WorldObjectDef worldObjectDef;

		// Token: 0x04005E4B RID: 24139
		public int cooldownTicks;

		// Token: 0x04005E4C RID: 24140
		[MustTranslate]
		public string cooldownEndedMessage;
	}
}
