using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AE RID: 4270
	public class QuestPart_BiocodeWeapons : QuestPart
	{
		// Token: 0x06006AD5 RID: 27349 RVA: 0x00258544 File Offset: 0x00256744
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].equipment != null)
					{
						foreach (ThingWithComps thingWithComps in this.pawns[i].equipment.AllEquipmentListForReading)
						{
							CompBiocodable comp = thingWithComps.GetComp<CompBiocodable>();
							if (comp != null && !comp.Biocoded)
							{
								comp.CodeFor(this.pawns[i]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x0025860C File Offset: 0x0025680C
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

		// Token: 0x06006AD7 RID: 27351 RVA: 0x0025867A File Offset: 0x0025687A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040043EB RID: 17387
		public string inSignal;

		// Token: 0x040043EC RID: 17388
		public List<Pawn> pawns = new List<Pawn>();
	}
}
