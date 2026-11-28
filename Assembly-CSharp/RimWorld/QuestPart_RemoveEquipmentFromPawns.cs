using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DF RID: 4319
	public class QuestPart_RemoveEquipmentFromPawns : QuestPart
	{
		// Token: 0x06006BF8 RID: 27640 RVA: 0x0025EC38 File Offset: 0x0025CE38
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i] != null && this.pawns[i].equipment != null)
					{
						this.pawns[i].equipment.DestroyAllEquipment(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06006BF9 RID: 27641 RVA: 0x0025ECB0 File Offset: 0x0025CEB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040044C8 RID: 17608
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040044C9 RID: 17609
		public string inSignal;
	}
}
