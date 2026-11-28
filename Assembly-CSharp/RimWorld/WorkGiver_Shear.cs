using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB8 RID: 3000
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x002028A8 File Offset: 0x00200AA8
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x002028AF File Offset: 0x00200AAF
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
