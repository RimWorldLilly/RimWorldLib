using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C61 RID: 3169
	public class LordJob_BossgroupAssaultColony : LordJob
	{
		// Token: 0x060059EF RID: 23023 RVA: 0x002119DB File Offset: 0x0020FBDB
		public LordJob_BossgroupAssaultColony()
		{
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x002119EE File Offset: 0x0020FBEE
		public LordJob_BossgroupAssaultColony(Faction faction, IntVec3 stageLoc, IEnumerable<Pawn> bosses)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.bosses.AddRange(bosses);
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x00211A1C File Offset: 0x0020FC1C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			LordToil_MoveInBossgroup lordToil_MoveInBossgroup = new LordToil_MoveInBossgroup(this.bosses);
			stateGraph.AddToil(lordToil_MoveInBossgroup);
			LordToil_AssaultColonyBossgroup lordToil_AssaultColonyBossgroup = new LordToil_AssaultColonyBossgroup();
			stateGraph.AddToil(lordToil_AssaultColonyBossgroup);
			Transition transition = new Transition(lordToil_Stage, lordToil_MoveInBossgroup, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(LordJob_BossgroupAssaultColony.PrepareTicksRange.RandomInRange));
			if (this.faction != null)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, null, 1f, null));
			}
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Stage, lordToil_AssaultColonyBossgroup, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_MoveInBossgroup, lordToil_AssaultColonyBossgroup, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x00211B54 File Offset: 0x0020FD54
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Collections.Look<Pawn>(ref this.bosses, "bosses", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.bosses.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04003392 RID: 13202
		private static readonly IntRange PrepareTicksRange = new IntRange(5000, 10000);

		// Token: 0x04003393 RID: 13203
		private Faction faction;

		// Token: 0x04003394 RID: 13204
		private IntVec3 stageLoc;

		// Token: 0x04003395 RID: 13205
		private List<Pawn> bosses = new List<Pawn>();
	}
}
