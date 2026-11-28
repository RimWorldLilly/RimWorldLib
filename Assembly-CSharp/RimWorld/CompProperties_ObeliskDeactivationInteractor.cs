using System;

namespace RimWorld
{
	// Token: 0x0200191A RID: 6426
	public class CompProperties_ObeliskDeactivationInteractor : CompProperties_Interactable
	{
		// Token: 0x0600A12B RID: 41259 RVA: 0x00399B78 File Offset: 0x00397D78
		public CompProperties_ObeliskDeactivationInteractor()
		{
			this.compClass = typeof(CompObeliskDeactivationInteractor);
		}

		// Token: 0x04005F31 RID: 24369
		public int shardsRequired = 2;
	}
}
