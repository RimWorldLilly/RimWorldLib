using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AF RID: 2479
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x001D2A3A File Offset: 0x001D0C3A
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x001D2A41 File Offset: 0x001D0C41
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
