using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B3 RID: 8627
	public class QuestNode_GenerateThingSet : QuestNode
	{
		// Token: 0x0600D6F3 RID: 55027 RVA: 0x004FCAE4 File Offset: 0x004FACE4
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6F4 RID: 55028 RVA: 0x004FCAE8 File Offset: 0x004FACE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = this.totalMarketValueRange.GetValue(slate);
			Thing value = this.factionOf.GetValue(slate);
			parms.makingFaction = ((value == null) ? null : value.Faction);
			parms.qualityGenerator = this.qualityGenerator.GetValue(slate);
			List<Thing> list = this.thingSetMaker.GetValue(slate).root.Generate(parms);
			QuestGen.slate.Set<List<Thing>>(this.storeAs.GetValue(slate), list, false);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
		}

		// Token: 0x04008851 RID: 34897
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008852 RID: 34898
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		// Token: 0x04008853 RID: 34899
		public SlateRef<FloatRange?> totalMarketValueRange;

		// Token: 0x04008854 RID: 34900
		public SlateRef<Thing> factionOf;

		// Token: 0x04008855 RID: 34901
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
