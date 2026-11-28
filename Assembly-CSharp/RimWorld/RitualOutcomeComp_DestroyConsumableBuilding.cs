using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D1 RID: 5841
	public class RitualOutcomeComp_DestroyConsumableBuilding : RitualOutcomeComp
	{
		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x0600922A RID: 37418 RVA: 0x00356458 File Offset: 0x00354658
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600922B RID: 37419 RVA: 0x00356465 File Offset: 0x00354665
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.HasThing;
		}
	}
}
