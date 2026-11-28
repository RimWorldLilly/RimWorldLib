using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105E RID: 4190
	public class QuestPart_Filter_AllPawnsDowned : QuestPart_Filter
	{
		// Token: 0x0600695D RID: 26973 RVA: 0x00252DA8 File Offset: 0x00250FA8
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Dead && !pawn.Downed)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x00252E1C File Offset: 0x0025101C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x00252E74 File Offset: 0x00251074
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

		// Token: 0x0400430A RID: 17162
		public List<Pawn> pawns;

		// Token: 0x0400430B RID: 17163
		public string inSignalRemovePawn;
	}
}
