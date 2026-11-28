using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FB RID: 2555
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x06004D50 RID: 19792 RVA: 0x001DA1F8 File Offset: 0x001D83F8
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position), null);
		}
	}
}
