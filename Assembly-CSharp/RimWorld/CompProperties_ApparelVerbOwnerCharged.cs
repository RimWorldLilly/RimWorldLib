using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001813 RID: 6163
	public class CompProperties_ApparelVerbOwnerCharged : CompProperties_ApparelVerbOwner
	{
		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x060099FF RID: 39423 RVA: 0x0037A4A5 File Offset: 0x003786A5
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x06009A00 RID: 39424 RVA: 0x0037A4B7 File Offset: 0x003786B7
		public CompProperties_ApparelVerbOwnerCharged()
		{
			this.compClass = typeof(CompApparelVerbOwner_Charged);
		}

		// Token: 0x04005BBD RID: 23485
		public int maxCharges;

		// Token: 0x04005BBE RID: 23486
		public bool destroyOnEmpty;

		// Token: 0x04005BBF RID: 23487
		[MustTranslate]
		public string chargeNoun = "charge";
	}
}
