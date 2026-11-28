using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001105 RID: 4357
	public abstract class QuestPart_TransporterPawns : QuestPart
	{
		// Token: 0x06006CC1 RID: 27841 RVA: 0x00262324 File Offset: 0x00260524
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (this.pawns != null)
				{
					foreach (Pawn pawn in this.pawns)
					{
						this.Process(pawn);
					}
				}
				if (this.pawnsInTransporter != null)
				{
					foreach (Thing thing in ((IEnumerable<Thing>)this.pawnsInTransporter.TryGetComp<CompTransporter>().innerContainer))
					{
						Pawn pawn2 = thing as Pawn;
						if (pawn2 != null)
						{
							this.Process(pawn2);
						}
					}
				}
			}
		}

		// Token: 0x06006CC2 RID: 27842
		public abstract void Process(Pawn pawn);

		// Token: 0x06006CC3 RID: 27843 RVA: 0x002623EC File Offset: 0x002605EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.pawnsInTransporter, "pawnsInTransporter", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006CC4 RID: 27844 RVA: 0x0026246B File Offset: 0x0026066B
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawns != null)
			{
				this.pawns.Replace(replace, with);
			}
		}

		// Token: 0x04004540 RID: 17728
		public string inSignal;

		// Token: 0x04004541 RID: 17729
		public Thing pawnsInTransporter;

		// Token: 0x04004542 RID: 17730
		public List<Pawn> pawns = new List<Pawn>();
	}
}
