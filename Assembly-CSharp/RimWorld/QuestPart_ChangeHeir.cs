using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110E RID: 4366
	public class QuestPart_ChangeHeir : QuestPart
	{
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06006D1A RID: 27930 RVA: 0x00262F28 File Offset: 0x00261128
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_ChangeHeir.<get_QuestLookTargets>d__6 <get_QuestLookTargets>d__ = new QuestPart_ChangeHeir.<get_QuestLookTargets>d__6(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06006D1B RID: 27931 RVA: 0x00262F38 File Offset: 0x00261138
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_ChangeHeir.<get_InvolvedFactions>d__8 <get_InvolvedFactions>d__ = new QuestPart_ChangeHeir.<get_InvolvedFactions>d__8(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x00262F48 File Offset: 0x00261148
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x00262F9A File Offset: 0x0026119A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.holder == replace)
			{
				this.holder = with;
			}
			if (this.heir == replace)
			{
				this.heir = with;
			}
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x00262FBC File Offset: 0x002611BC
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		// Token: 0x0400455F RID: 17759
		public Faction faction;

		// Token: 0x04004560 RID: 17760
		public Pawn holder;

		// Token: 0x04004561 RID: 17761
		public Pawn heir;

		// Token: 0x04004562 RID: 17762
		public string inSignal;

		// Token: 0x04004563 RID: 17763
		public bool done;
	}
}
