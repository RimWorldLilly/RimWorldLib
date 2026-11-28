using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CD RID: 2509
	public class JobDriver_FillIn : JobDriver_RemoveBuilding
	{
		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x001D68E6 File Offset: 0x001D4AE6
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x001D68ED File Offset: 0x001D4AED
		protected override float TotalNeededWork
		{
			get
			{
				return base.Target.def.building.uninstallWork;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x001D6904 File Offset: 0x001D4B04
		protected override EffecterDef WorkEffecter
		{
			get
			{
				if (!(base.Target is PitBurrow))
				{
					return EffecterDefOf.FillingInCrater;
				}
				return EffecterDefOf.FillingInPitGate;
			}
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x001D6920 File Offset: 0x001D4B20
		protected override void FinishedRemoving()
		{
			Crater crater = base.Target as Crater;
			if (crater != null)
			{
				crater.FillIn();
			}
		}
	}
}
