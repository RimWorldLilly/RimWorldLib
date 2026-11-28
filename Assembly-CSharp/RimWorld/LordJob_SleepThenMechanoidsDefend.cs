using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C87 RID: 3207
	public class LordJob_SleepThenMechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x00216AFB File Offset: 0x00214CFB
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x00216AFE File Offset: 0x00214CFE
		public LordJob_SleepThenMechanoidsDefend()
		{
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x00216B06 File Offset: 0x00214D06
		public LordJob_SleepThenMechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			if (things != null)
			{
				this.things.AddRange(things);
			}
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x00216B44 File Offset: 0x00214D44
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_MechanoidsDefend(this.things, this.faction, this.defendRadius, this.defSpot, this.canAssaultColony, this.isMechCluster).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			transition.AddTrigger(new Trigger_OnHumanlikeHarmAnyThing(this.things));
			transition.AddTrigger(new Trigger_OnPlayerMechHarmAnything(this.things));
			transition.AddTrigger(new Trigger_PawnHarmed(1f, true, null, null, null));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Find.SignalManager.SendSignal(new Signal("CompCanBeDormant.WakeUp", this.things.First<Thing>().Named("SUBJECT"), Faction.OfMechanoids.Named("FACTION")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.defSpot, base.Map, false));
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x00216C6A File Offset: 0x00214E6A
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			if (!p.Dead)
			{
				CompCanBeDormant compCanBeDormant = p.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null && !compCanBeDormant.Awake)
				{
					return false;
				}
			}
			return base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x00216C95 File Offset: 0x00214E95
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
		}

		// Token: 0x0400341B RID: 13339
		public bool awakeOnClamor;
	}
}
