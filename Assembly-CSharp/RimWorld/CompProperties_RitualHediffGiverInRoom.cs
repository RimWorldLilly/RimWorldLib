using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002099 RID: 8345
	public class CompProperties_RitualHediffGiverInRoom : CompProperties
	{
		// Token: 0x0600D259 RID: 53849 RVA: 0x004D41DE File Offset: 0x004D23DE
		public CompProperties_RitualHediffGiverInRoom()
		{
			this.compClass = typeof(CompRitualHediffGiverInRoom);
		}

		// Token: 0x04008540 RID: 34112
		public HediffDef hediff;

		// Token: 0x04008541 RID: 34113
		public float minRadius = 999f;

		// Token: 0x04008542 RID: 34114
		public float severity = -1f;

		// Token: 0x04008543 RID: 34115
		public bool resetLastRecreationalDrugTick;
	}
}
