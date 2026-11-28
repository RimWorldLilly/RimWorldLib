using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F8 RID: 2552
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x06004D49 RID: 19785 RVA: 0x001DA0C8 File Offset: 0x001D82C8
		protected override void WatchTickAction(int delta)
		{
			if (this.pawn.IsHashIntervalTick(400, delta))
			{
				FleckMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction(delta);
		}

		// Token: 0x04003061 RID: 12385
		private const int StoneThrowInterval = 400;
	}
}
