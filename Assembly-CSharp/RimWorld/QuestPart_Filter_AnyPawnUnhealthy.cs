using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106D RID: 4205
	public class QuestPart_Filter_AnyPawnUnhealthy : QuestPart_Filter
	{
		// Token: 0x0600698B RID: 27019 RVA: 0x002537FC File Offset: 0x002519FC
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn.Destroyed || pawn.InMentalState || pawn.health.hediffSet.BleedRateTotal > 0.001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x00253888 File Offset: 0x00251A88
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600698D RID: 27021 RVA: 0x002538E0 File Offset: 0x00251AE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400431C RID: 17180
		public List<Pawn> pawns;

		// Token: 0x0400431D RID: 17181
		public string inSignalRemovePawn;
	}
}
