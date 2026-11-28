using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002287 RID: 8839
	public class QuestNode_Root_MysteriousCargoUnnaturalCorpse : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x0600DA4D RID: 55885 RVA: 0x00511E89 File Offset: 0x00510089
		protected override Thing GenerateThing(Pawn pawn)
		{
			return AnomalyUtility.MakeUnnaturalCorpse(pawn);
		}

		// Token: 0x0600DA4E RID: 55886 RVA: 0x00511E91 File Offset: 0x00510091
		protected override bool ValidatePawn(Pawn pawn)
		{
			return base.ValidatePawn(pawn) && pawn.RaceProps.unnaturalCorpseDef != null && !Find.Anomaly.PawnHasUnnaturalCorpse(pawn);
		}

		// Token: 0x0600DA4F RID: 55887 RVA: 0x00511EBC File Offset: 0x005100BC
		protected override void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
			quest.PawnDestroyed(pawn, null, delegate
			{
				quest.LinkUnnaturalCorpse(pawn, thing as UnnaturalCorpse, null);
			}, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}
	}
}
