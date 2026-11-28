using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023C2 RID: 9154
	public class SitePartParams : IExposable
	{
		// Token: 0x0600E458 RID: 58456 RVA: 0x0054D89C File Offset: 0x0054BA9C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.lootMarketValue, "lootMarketValue", 0f, false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousLumpResources, "preciousLumpResources");
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.turretsCount, "turretsCount", 0, false);
			Scribe_Values.Look<int>(ref this.mortarsCount, "mortarsCount", 0, false);
			Scribe_Deep.Look<LayoutStructureSketch>(ref this.ancientLayoutStructureSketch, "ancientComplexSketch", Array.Empty<object>());
			Scribe_Defs.Look<ThingSetMakerDef>(ref this.ancientComplexRewardMaker, "ancientComplexRewardMaker");
			Scribe_Values.Look<string>(ref this.triggerSecuritySignal, "triggerSecuritySignal", null, false);
			Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
			Scribe_Values.Look<string>(ref this.relicLostSignal, "relicLostSignal", null, false);
			Scribe_Values.Look<float>(ref this.interiorThreatPoints, "interiorThreatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.exteriorThreatPoints, "exteriorThreatPoints", 0f, false);
			Scribe_Values.Look<int>(ref this.stabilizerCount, "stabilizerCount", 0, false);
		}

		// Token: 0x04009145 RID: 37189
		public int randomValue;

		// Token: 0x04009146 RID: 37190
		public float threatPoints;

		// Token: 0x04009147 RID: 37191
		public float lootMarketValue;

		// Token: 0x04009148 RID: 37192
		public float points;

		// Token: 0x04009149 RID: 37193
		public Precept_Relic relic;

		// Token: 0x0400914A RID: 37194
		public ThingDef preciousLumpResources;

		// Token: 0x0400914B RID: 37195
		public PawnKindDef animalKind;

		// Token: 0x0400914C RID: 37196
		public int turretsCount;

		// Token: 0x0400914D RID: 37197
		public int mortarsCount;

		// Token: 0x0400914E RID: 37198
		public LayoutStructureSketch ancientLayoutStructureSketch;

		// Token: 0x0400914F RID: 37199
		public ThingSetMakerDef ancientComplexRewardMaker;

		// Token: 0x04009150 RID: 37200
		public string triggerSecuritySignal;

		// Token: 0x04009151 RID: 37201
		public string relicLostSignal;

		// Token: 0x04009152 RID: 37202
		public Thing relicThing;

		// Token: 0x04009153 RID: 37203
		public float interiorThreatPoints;

		// Token: 0x04009154 RID: 37204
		public float exteriorThreatPoints;

		// Token: 0x04009155 RID: 37205
		public int stabilizerCount;
	}
}
