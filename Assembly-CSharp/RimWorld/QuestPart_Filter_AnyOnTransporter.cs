using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001066 RID: 4198
	public class QuestPart_Filter_AnyOnTransporter : QuestPart_Filter
	{
		// Token: 0x06006974 RID: 26996 RVA: 0x00253204 File Offset: 0x00251404
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			ThingOwner directlyHeldThings = this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings();
			foreach (Pawn item in this.pawns)
			{
				if (directlyHeldThings.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x00253280 File Offset: 0x00251480
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04004312 RID: 17170
		public Thing transporter;

		// Token: 0x04004313 RID: 17171
		public List<Pawn> pawns;
	}
}
