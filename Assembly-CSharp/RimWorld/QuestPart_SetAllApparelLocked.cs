using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F5 RID: 4341
	public class QuestPart_SetAllApparelLocked : QuestPart
	{
		// Token: 0x06006C58 RID: 27736 RVA: 0x002601E8 File Offset: 0x0025E3E8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].apparel != null)
					{
						this.pawns[i].apparel.LockAll();
					}
				}
			}
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x00260250 File Offset: 0x0025E450
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006C5A RID: 27738 RVA: 0x002602BE File Offset: 0x0025E4BE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040044F4 RID: 17652
		public string inSignal;

		// Token: 0x040044F5 RID: 17653
		public List<Pawn> pawns = new List<Pawn>();
	}
}
