using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB6 RID: 2998
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x0600567E RID: 22142 RVA: 0x00202797 File Offset: 0x00200997
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0020279E File Offset: 0x0020099E
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
