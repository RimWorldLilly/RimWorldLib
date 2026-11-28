using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AE RID: 8878
	public class QuestNode_Root_WandererJoinAbasia : QuestNode_Root_WandererJoin
	{
		// Token: 0x0600DB34 RID: 56116 RVA: 0x0051A178 File Offset: 0x00518378
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x0600DB35 RID: 56117 RVA: 0x0051A1C6 File Offset: 0x005183C6
		public override Pawn GeneratePawn()
		{
			return this.GeneratePawn_NewTemp(null);
		}

		// Token: 0x0600DB36 RID: 56118 RVA: 0x0051A1D0 File Offset: 0x005183D0
		public override Pawn GeneratePawn_NewTemp(Map map)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Refugee, null, PawnGenerationContext.NonPlayer, (map != null) ? new PlanetTile?(map.Tile) : null, true, false, false, true, false, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.health.AddHediff(HediffDefOf.Abasia, null, null, null);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600DB37 RID: 56119 RVA: 0x0051A2D8 File Offset: 0x005184D8
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			base.AddSpawnPawnQuestParts(quest, map, pawn);
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600DB38 RID: 56120 RVA: 0x0051A36A File Offset: 0x0051856A
		[Obsolete]
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			this.SendLetter_NewTemp(quest, pawn, Find.AnyPlayerHomeMap);
		}

		// Token: 0x0600DB39 RID: 56121 RVA: 0x0051A37C File Offset: 0x0051857C
		public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
		{
			TaggedString label = "LetterLabelWandererJoinsAbasia".Translate(pawn);
			TaggedString text = "LetterTextWandererJoinsAbasia".Translate(pawn, HediffDefOf.Abasia);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("AfflictedJoinerChartityInfo".Translate(pawn, this.AllowKilledBeforeTicks.ToStringTicksToPeriod(true, false, true, true, false)), ref text);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			QuestNode_Root_WandererJoin_WalkIn.ApplyBestSkillInfoToLetter(ref text, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, text, LetterDefOf.AcceptJoiner, pawn, null, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null, 0, true);
		}

		// Token: 0x04008C45 RID: 35909
		private const int TimeoutTicks = 60000;

		// Token: 0x04008C46 RID: 35910
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04008C47 RID: 35911
		private string signalAccept;

		// Token: 0x04008C48 RID: 35912
		private string signalReject;
	}
}
