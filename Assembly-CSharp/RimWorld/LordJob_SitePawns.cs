using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C84 RID: 3204
	public class LordJob_SitePawns : LordJob
	{
		// Token: 0x06005AC5 RID: 23237 RVA: 0x0021621E File Offset: 0x0021441E
		public LordJob_SitePawns()
		{
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x00216226 File Offset: 0x00214426
		public LordJob_SitePawns(Faction faction, IntVec3 baseCenter, int delayBeforeAssault)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
			this.delayBeforeAssault = delayBeforeAssault;
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x00216244 File Offset: 0x00214444
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_HuntDownColonists lordToil_HuntDownColonists = new LordToil_HuntDownColonists();
			stateGraph.AddToil(lordToil_HuntDownColonists);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_HuntDownColonists, false, true);
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null, null, null));
			transition.AddTrigger(new Trigger_ChanceOnTickInterval(2500, 0.03f));
			transition.AddTrigger(new Trigger_TicksPassed(this.delayBeforeAssault));
			transition.AddTrigger(new Trigger_UrgentlyHungry());
			transition.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition.AddTrigger(new Trigger_OnClamor(ClamorDefOf.Ability));
			transition.AddPostAction(new TransitionAction_WakeAll());
			TaggedString taggedString = this.faction.def.messageDefendersAttacking.Formatted(this.faction.def.pawnsPlural, this.faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
			transition.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f, null));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x00216388 File Offset: 0x00214588
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.delayBeforeAssault, "delayBeforeAssault", 25000, false);
		}

		// Token: 0x0400340B RID: 13323
		private Faction faction;

		// Token: 0x0400340C RID: 13324
		private IntVec3 baseCenter;

		// Token: 0x0400340D RID: 13325
		private int delayBeforeAssault;

		// Token: 0x0400340E RID: 13326
		private List<Pawn> tmpPawns;

		// Token: 0x0400340F RID: 13327
		private List<IntVec3> tmpCells;
	}
}
