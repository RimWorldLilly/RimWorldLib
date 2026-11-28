using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F9 RID: 2553
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		// Token: 0x06004D4B RID: 19787 RVA: 0x001DA110 File Offset: 0x001D8310
		protected override void WatchTickAction(int delta)
		{
			if (this.pawn.IsHashIntervalTick(400, delta))
			{
				FleckMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction(delta);
		}

		// Token: 0x04003062 RID: 12386
		private const int HorseshoeThrowInterval = 400;
	}
}
