using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB4 RID: 3252
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06005C5D RID: 23645 RVA: 0x0021F58F File Offset: 0x0021D78F
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06005C5E RID: 23646 RVA: 0x0021F592 File Offset: 0x0021D792
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06005C5F RID: 23647 RVA: 0x0021F599 File Offset: 0x0021D799
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06005C60 RID: 23648 RVA: 0x0021F5A0 File Offset: 0x0021D7A0
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x0021F5A7 File Offset: 0x0021D7A7
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x0021F5AE File Offset: 0x0021D7AE
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x0021F5B6 File Offset: 0x0021D7B6
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x0021F5D6 File Offset: 0x0021D7D6
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x0021F5E7 File Offset: 0x0021D7E7
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x0021F5F8 File Offset: 0x0021D7F8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil party = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition.AddPreAction(new TransitionAction_Message(this.gatheringDef.calledOffMessage, MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = this.GetTimeoutTrigger();
			Transition transition2 = new Transition(party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition2.AddPreAction(new TransitionAction_Message(this.gatheringDef.finishedMessage, MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x0021F760 File Offset: 0x0021D960
		private void ApplyOutcome(LordToil_Party toil)
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			LordToilData_Gathering lordToilData_Gathering = (LordToilData_Gathering)toil.data;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				bool flag = pawn == this.organizer;
				int num;
				if (lordToilData_Gathering.presentForTicks.TryGetValue(pawn, out num) && num > 0)
				{
					if (ownedPawns[i].needs.mood != null)
					{
						ThoughtDef thoughtDef = flag ? this.OrganizerThought : this.AttendeeThought;
						float num2 = 0.5f / thoughtDef.stages[0].baseMoodEffect;
						float moodPowerFactor = Mathf.Min((float)num / (float)this.durationTicks + num2, 1f);
						Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
						thought_Memory.moodPowerFactor = moodPowerFactor;
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					}
					TaleRecorder.RecordTale(flag ? this.OrganizerTale : this.AttendeeTale, new object[]
					{
						ownedPawns[i],
						this.organizer
					});
				}
			}
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x0021F893 File Offset: 0x0021DA93
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}
	}
}
