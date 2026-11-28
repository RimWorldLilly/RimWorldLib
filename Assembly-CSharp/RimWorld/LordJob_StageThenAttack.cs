using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C88 RID: 3208
	public class LordJob_StageThenAttack : LordJob
	{
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x00216D3A File Offset: 0x00214F3A
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x00216D3D File Offset: 0x00214F3D
		public LordJob_StageThenAttack()
		{
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00216D48 File Offset: 0x00214F48
		public LordJob_StageThenAttack(Faction faction, IntVec3 stageLoc, int raidSeed, bool canTimeoutFlee = true, bool canKidnap = true, bool canSteal = true, IntRange? delay = null)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.raidSeed = raidSeed;
			this.canTimeoutFlee = canTimeoutFlee;
			this.canKidnap = canKidnap;
			this.canSteal = canSteal;
			this.delay = (delay ?? new IntRange(5000, 15000));
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x00216DB4 File Offset: 0x00214FB4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			Faction assaulterFaction = this.faction;
			bool canTimeoutOrFlee = this.canTimeoutFlee;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(assaulterFaction, this.canKidnap, canTimeoutOrFlee, false, false, this.canSteal, false, false).CreateGraph()).StartingToil;
			int tickLimit = Rand.RangeSeeded(this.delay.min, this.delay.max, this.raidSeed);
			Transition transition = new Transition(lordToil_Stage, startingToil, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(tickLimit));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, "MessageRaidersBeginningAssault-" + this.raidSeed.ToString(), 1f, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			stateGraph.transitions.Find((Transition x) => x.triggers.Any((Trigger y) => y is Trigger_BecameNonHostileToPlayer)).AddSource(lordToil_Stage);
			return stateGraph;
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x00216F04 File Offset: 0x00215104
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.raidSeed, "raidSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutFlee, "canTimeoutFlee", false, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", false, false);
			Scribe_Values.Look<IntRange>(ref this.delay, "delay", default(IntRange), false);
		}

		// Token: 0x0400341C RID: 13340
		private Faction faction;

		// Token: 0x0400341D RID: 13341
		private IntVec3 stageLoc;

		// Token: 0x0400341E RID: 13342
		private int raidSeed;

		// Token: 0x0400341F RID: 13343
		private bool canTimeoutFlee;

		// Token: 0x04003420 RID: 13344
		private bool canKidnap;

		// Token: 0x04003421 RID: 13345
		private bool canSteal;

		// Token: 0x04003422 RID: 13346
		private IntRange delay;
	}
}
