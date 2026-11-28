using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AB RID: 4267
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		// Token: 0x06006ACB RID: 27339 RVA: 0x002582B3 File Offset: 0x002564B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		// Token: 0x040043E3 RID: 17379
		public Pawn lodger;
	}
}
