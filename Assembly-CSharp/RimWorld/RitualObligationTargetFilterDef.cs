using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001692 RID: 5778
	public class RitualObligationTargetFilterDef : Def
	{
		// Token: 0x060090DA RID: 37082 RVA: 0x003523DA File Offset: 0x003505DA
		public RitualObligationTargetFilter GetInstance()
		{
			return (RitualObligationTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0400584E RID: 22606
		public Type workerClass;

		// Token: 0x0400584F RID: 22607
		public bool colonistThingsOnly = true;

		// Token: 0x04005850 RID: 22608
		public List<ThingDef> thingDefs;

		// Token: 0x04005851 RID: 22609
		public int minUnroofedCells;

		// Token: 0x04005852 RID: 22610
		public int unroofedCellSearchRadius;

		// Token: 0x04005853 RID: 22611
		public int woodPerParticipant;

		// Token: 0x04005854 RID: 22612
		public int maxSpeakerDistance;

		// Token: 0x04005855 RID: 22613
		public int maxDrumDistance;
	}
}
