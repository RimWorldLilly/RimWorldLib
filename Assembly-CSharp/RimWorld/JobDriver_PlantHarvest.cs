using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A93 RID: 2707
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x001E75C2 File Offset: 0x001E57C2
		protected override PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Chop;
			}
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x001E75C5 File Offset: 0x001E57C5
		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001E75D2 File Offset: 0x001E57D2
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
