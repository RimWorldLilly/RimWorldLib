using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002285 RID: 8837
	public class QuestNode_Root_MysteriousCargoUnnaturalCube : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x0600DA46 RID: 55878 RVA: 0x00511DD3 File Offset: 0x0050FFD3
		protected override Thing GenerateThing(Pawn pawn)
		{
			return ThingMaker.MakeThing(ThingDefOf.GoldenCube, null);
		}

		// Token: 0x0600DA47 RID: 55879 RVA: 0x00511DE0 File Offset: 0x0050FFE0
		protected override void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
			quest.PawnDestroyed(pawn, null, delegate
			{
				quest.GiveHediff(pawn, HediffDefOf.CubeInterest, null);
			}, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600DA48 RID: 55880 RVA: 0x00511E24 File Offset: 0x00510024
		protected override bool ValidatePawn(Pawn pawn)
		{
			return base.ValidatePawn(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false);
		}
	}
}
