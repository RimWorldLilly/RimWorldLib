using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200227D RID: 8829
	public class QuestNode_Root_MechanitorStartingMech : QuestNode
	{
		// Token: 0x0600D9FD RID: 55805 RVA: 0x0050F7E0 File Offset: 0x0050D9E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Pawn pawn = slate.Get<Pawn>("asker", null, false);
			PawnKindDef pawnKindDef = this.mechTypes.RandomElement<PawnKindDef>();
			Map map = pawn.MapHeld;
			PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, pawn.Faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			Pawn mech = quest.GeneratePawn(request, false);
			slate.Set<Pawn>("mechanitor", pawn, false);
			slate.Set<PawnKindDef>("mech", pawnKindDef, false);
			quest.Delay(600, delegate
			{
				Quest quest;
				quest.AssignMechToMechanitor(pawn, mech, null);
				quest = quest;
				MapParent parent = map.Parent;
				IEnumerable<Thing> contents = Gen.YieldSingle<Pawn>(mech);
				string customLetterLabel = null;
				RulePack customLetterLabelRules = null;
				string customLetterText = null;
				RulePack customLetterTextRules = null;
				IntVec3? dropSpot = new IntVec3?(pawn.PositionHeld);
				quest.DropPods(parent, contents, customLetterLabel, customLetterLabelRules, customLetterText, customLetterTextRules, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, dropSpot, true, false, false, true, null);
				quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Pawn>(mech), false, "[arrivalLetterText]", null, "[arrivalLetterLabel]", null, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("asker.Killed"), QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0600D9FE RID: 55806 RVA: 0x0050F948 File Offset: 0x0050DB48
		protected override bool TestRunInt(Slate slate)
		{
			Pawn pawn = slate.Get<Pawn>("asker", null, false);
			return pawn != null && pawn.SpawnedOrAnyParentSpawned && MechanitorUtility.IsMechanitor(pawn);
		}

		// Token: 0x04008B64 RID: 35684
		private const int DelayTicks = 600;

		// Token: 0x04008B65 RID: 35685
		public List<PawnKindDef> mechTypes;
	}
}
