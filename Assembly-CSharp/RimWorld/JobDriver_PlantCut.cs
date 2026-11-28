using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A94 RID: 2708
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x001E75E7 File Offset: 0x001E57E7
		protected override PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Cut;
			}
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001E75EC File Offset: 0x001E57EC
		protected override void Init()
		{
			Plant plant = base.Plant;
			if (((plant != null) ? plant.def.plant.harvestedThingDef : null) != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.085f;
				return;
			}
			this.xpPerTick = 0f;
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001E763B File Offset: 0x001E583B
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
