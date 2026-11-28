using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001079 RID: 4217
	public class QuestPart_Filter_ThingAnalyzed : QuestPart_Filter
	{
		// Token: 0x060069AE RID: 27054 RVA: 0x00253CBC File Offset: 0x00251EBC
		protected override bool Pass(SignalArgs args)
		{
			CompProperties_CompAnalyzableUnlockResearch compProperties_CompAnalyzableUnlockResearch = this.thingDef.CompDefForAssignableFrom<CompAnalyzable>() as CompProperties_CompAnalyzableUnlockResearch;
			if (compProperties_CompAnalyzableUnlockResearch == null)
			{
				return false;
			}
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(compProperties_CompAnalyzableUnlockResearch.analysisID, out analysisDetails);
			return analysisDetails != null && analysisDetails.Satisfied;
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x00253CFD File Offset: 0x00251EFD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x0400432C RID: 17196
		public ThingDef thingDef;
	}
}
