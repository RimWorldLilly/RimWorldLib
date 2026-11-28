using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105D RID: 4189
	public class QuestPart_Filter_AllPawnsDestroyed : QuestPart_Filter
	{
		// Token: 0x06006959 RID: 26969 RVA: 0x00252C70 File Offset: 0x00250E70
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Destroyed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x00252CD8 File Offset: 0x00250ED8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x00252D30 File Offset: 0x00250F30
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

		// Token: 0x04004308 RID: 17160
		public List<Pawn> pawns;

		// Token: 0x04004309 RID: 17161
		public string inSignalRemovePawn;
	}
}
