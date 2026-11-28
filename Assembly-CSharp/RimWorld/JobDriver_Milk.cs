using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06004AD5 RID: 19157 RVA: 0x001D1E95 File Offset: 0x001D0095
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x001D1E9C File Offset: 0x001D009C
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
