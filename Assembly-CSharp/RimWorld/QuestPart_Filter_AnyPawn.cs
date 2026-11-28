using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001068 RID: 4200
	public abstract class QuestPart_Filter_AnyPawn : QuestPart_Filter
	{
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x0600697A RID: 27002
		protected abstract int PawnsCount { get; }

		// Token: 0x0600697B RID: 27003 RVA: 0x00253381 File Offset: 0x00251581
		protected override bool Pass(SignalArgs args)
		{
			return this.PawnsCount > 0;
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x0025338C File Offset: 0x0025158C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				signal.args.Add(this.PawnsCount.Named("PAWNSALIVECOUNT"));
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x00253418 File Offset: 0x00251618
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

		// Token: 0x04004315 RID: 17173
		public List<Pawn> pawns;

		// Token: 0x04004316 RID: 17174
		public string inSignalRemovePawn;
	}
}
