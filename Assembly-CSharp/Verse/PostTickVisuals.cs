using System;

namespace Verse
{
	// Token: 0x020002EA RID: 746
	public class PostTickVisuals
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x00087FAB File Offset: 0x000861AB
		public PostTickVisuals(Map map)
		{
			this.map = map;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00087FBC File Offset: 0x000861BC
		public void ProcessPostTickVisuals()
		{
			int ticksThisFrame = Find.TickManager.TicksThisFrame;
			if (ticksThisFrame > 0)
			{
				CellRect viewRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
				foreach (Pawn pawn in this.map.mapPawns.AllPawnsSpawned)
				{
					pawn.ProcessPostTickVisuals(ticksThisFrame, viewRect);
				}
			}
		}

		// Token: 0x04001372 RID: 4978
		private Map map;
	}
}
