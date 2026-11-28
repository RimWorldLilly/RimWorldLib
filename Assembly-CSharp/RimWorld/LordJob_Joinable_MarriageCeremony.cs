using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB2 RID: 3250
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06005C44 RID: 23620 RVA: 0x0021E808 File Offset: 0x0021CA08
		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06005C45 RID: 23621 RVA: 0x0021E81D File Offset: 0x0021CA1D
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x0021E820 File Offset: 0x0021CA20
		public LordJob_Joinable_MarriageCeremony()
		{
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x0021E828 File Offset: 0x0021CA28
		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.firstPawnSpouseCount = firstPawn.GetSpouseCount(false);
			this.secondPawnSpouseCount = firstPawn.GetSpouseCount(false);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x0021E860 File Offset: 0x0021CA60
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party);
			LordToil_MarriageCeremony lordToil_MarriageCeremony = new LordToil_MarriageCeremony(this.firstPawn, this.secondPawn, this.spot);
			stateGraph.AddToil(lordToil_MarriageCeremony);
			LordToil_Party lordToil_Party2 = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party2);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_MarriageCeremony, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 5000 && this.AreFiancesInPartyArea(), 1));
			transition.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyStarts".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_MarriageCeremony, lordToil_Party2, false, true);
			transition2.AddTrigger(new Trigger_TickCondition(() => this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.secondPawn), 1));
			transition2.AddPreAction(new TransitionAction_Message("MessageNewlyMarried".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			transition2.AddPreAction(new TransitionAction_Custom(new Action(this.WeddingSucceeded)));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition3.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldAfterPartyBeCalledOff), 1));
			transition3.AddTrigger(new Trigger_PawnKilled());
			stateGraph.AddTransition(transition3, false);
			this.afterPartyTimeoutTrigger = new Trigger_TicksPassed(7500);
			Transition transition4 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition4.AddTrigger(this.afterPartyTimeoutTrigger);
			transition4.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyAfterPartyFinished".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition5.AddSource(lordToil_Party);
			transition5.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 120000 && (this.firstPawn.Drafted || this.secondPawn.Drafted || !this.firstPawn.Position.InHorDistOf(this.spot, 4f) || !this.secondPawn.Position.InHorDistOf(this.spot, 4f)), 1));
			transition5.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition6.AddSource(lordToil_Party);
			transition6.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldCeremonyBeCalledOff), 1));
			transition6.AddTrigger(new Trigger_PawnKilled());
			transition6.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x0021EC74 File Offset: 0x0021CE74
		private bool AreFiancesInPartyArea()
		{
			return this.lord.ownedPawns.Contains(this.firstPawn) && this.lord.ownedPawns.Contains(this.secondPawn) && this.firstPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.firstPawn.Position, this.spot, base.Map) && this.secondPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.secondPawn.Position, this.spot, base.Map);
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x0021ED1C File Offset: 0x0021CF1C
		private bool ShouldCeremonyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || !this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn, this.secondPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn, this.firstPawn));
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x0021EDD0 File Offset: 0x0021CFD0
		private bool ShouldAfterPartyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || (this.firstPawn.Downed || this.secondPawn.Downed) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x0021EE5C File Offset: 0x0021D05C
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				if (!MarriageCeremonyUtility.FianceCanContinueCeremony(p, (p == this.firstPawn) ? this.secondPawn : this.firstPawn))
				{
					return 0f;
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyFiance;
			}
			else
			{
				if (!this.IsGuest(p))
				{
					return 0f;
				}
				if (!MarriageCeremonyUtility.ShouldGuestKeepAttendingCeremony(p))
				{
					return 0f;
				}
				if (!this.lord.ownedPawns.Contains(p))
				{
					if (this.IsCeremonyAboutToEnd())
					{
						return 0f;
					}
					LordToil_MarriageCeremony lordToil_MarriageCeremony = this.lord.CurLordToil as LordToil_MarriageCeremony;
					IntVec3 intVec;
					if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, null, null, null))
					{
						return 0f;
					}
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
			}
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x0021EF24 File Offset: 0x0021D124
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<int>(ref this.firstPawnSpouseCount, "firstPawnSpouseCount", 0, false);
			Scribe_Values.Look<int>(ref this.secondPawnSpouseCount, "secondPawnSpouseCount", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x0021EF91 File Offset: 0x0021D191
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x0021EFA2 File Offset: 0x0021D1A2
		private bool IsCeremonyAboutToEnd()
		{
			return this.afterPartyTimeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x0021EFB9 File Offset: 0x0021D1B9
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x0021EFD0 File Offset: 0x0021D1D0
		private bool IsGuest(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p == this.firstPawn || p == this.secondPawn)
			{
				return false;
			}
			HistoryEvent ev = new HistoryEvent(SpouseRelationUtility.GetHistoryEventForSpouseCount(this.firstPawnSpouseCount + 1), p.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(SpouseRelationUtility.GetHistoryEventForSpouseCount(this.secondPawnSpouseCount + 1), p.Named(HistoryEventArgsNames.Doer));
			return ev.DoerWillingToDo() && ev2.DoerWillingToDo() && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x0021F078 File Offset: 0x0021D278
		private void WeddingSucceeded()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && ownedPawns[i].needs.mood != null && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null, null);
				}
			}
		}

		// Token: 0x040034BE RID: 13502
		public Pawn firstPawn;

		// Token: 0x040034BF RID: 13503
		public Pawn secondPawn;

		// Token: 0x040034C0 RID: 13504
		private IntVec3 spot;

		// Token: 0x040034C1 RID: 13505
		private int firstPawnSpouseCount;

		// Token: 0x040034C2 RID: 13506
		private int secondPawnSpouseCount;

		// Token: 0x040034C3 RID: 13507
		private Trigger_TicksPassed afterPartyTimeoutTrigger;
	}
}
