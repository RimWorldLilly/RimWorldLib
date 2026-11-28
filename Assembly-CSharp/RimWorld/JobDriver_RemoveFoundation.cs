using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class JobDriver_RemoveFoundation : JobDriver_AffectFloor
	{
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x001D643B File Offset: 0x001D463B
		protected override int BaseWorkAmount
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x001D6442 File Offset: 0x001D4642
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFoundation;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x001D6449 File Offset: 0x001D4649
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x001D6450 File Offset: 0x001D4650
		public JobDriver_RemoveFoundation()
		{
			this.clearSnow = true;
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x001D645F File Offset: 0x001D465F
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				base.Map.terrainGrid.RemoveFoundation(base.TargetLocA, true);
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
