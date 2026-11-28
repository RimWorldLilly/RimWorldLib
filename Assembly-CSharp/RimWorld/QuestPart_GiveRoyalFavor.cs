using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001113 RID: 4371
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06006D4F RID: 27983 RVA: 0x00264C2B File Offset: 0x00262E2B
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06006D50 RID: 27984 RVA: 0x00264C33 File Offset: 0x00262E33
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_GiveRoyalFavor.<get_InvolvedFactions>d__8 <get_InvolvedFactions>d__ = new QuestPart_GiveRoyalFavor.<get_InvolvedFactions>d__8(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x00264C44 File Offset: 0x00262E44
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn pawn = this.giveToAccepter ? this.quest.AccepterPawn : this.giveTo;
				if (pawn == null)
				{
					signal.args.TryGetArg<Pawn>("CHOSEN", out pawn);
				}
				if (pawn != null && pawn.royalty != null)
				{
					pawn.royalty.GainFavor(this.faction, this.amount);
				}
			}
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x00264CC0 File Offset: 0x00262EC0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x00264D1C File Offset: 0x00262F1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x00264D87 File Offset: 0x00262F87
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		// Token: 0x0400459D RID: 17821
		public Pawn giveTo;

		// Token: 0x0400459E RID: 17822
		public bool giveToAccepter;

		// Token: 0x0400459F RID: 17823
		public string inSignal;

		// Token: 0x040045A0 RID: 17824
		public int amount;

		// Token: 0x040045A1 RID: 17825
		public Faction faction;
	}
}
