using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001067 RID: 4199
	public class QuestPart_Filter_AnyOnTransporterCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x06006977 RID: 26999 RVA: 0x002532F8 File Offset: 0x002514F8
		protected override bool Pass(SignalArgs args)
		{
			foreach (Thing thing in ((IEnumerable<Thing>)this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings()))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && HackUtility.IsCapableOfHacking(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x00253360 File Offset: 0x00251560
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
		}

		// Token: 0x04004314 RID: 17172
		public Thing transporter;
	}
}
