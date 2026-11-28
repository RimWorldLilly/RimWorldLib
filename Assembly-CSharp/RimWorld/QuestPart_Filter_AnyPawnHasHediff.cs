using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106B RID: 4203
	public class QuestPart_Filter_AnyPawnHasHediff : QuestPart_Filter
	{
		// Token: 0x06006983 RID: 27011 RVA: 0x0025354C File Offset: 0x0025174C
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn != null && pawn.health.hediffSet.HasHediff(this.hediff, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x002535CC File Offset: 0x002517CC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (!this.inSignalRemovePawn.NullOrEmpty() && signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x00253630 File Offset: 0x00251830
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04004317 RID: 17175
		public List<Pawn> pawns;

		// Token: 0x04004318 RID: 17176
		public HediffDef hediff;

		// Token: 0x04004319 RID: 17177
		public string inSignalRemovePawn;
	}
}
