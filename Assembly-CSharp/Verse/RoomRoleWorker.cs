using System;

namespace Verse
{
	// Token: 0x020001B2 RID: 434
	public abstract class RoomRoleWorker
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x00040387 File Offset: 0x0003E587
		public virtual string PostProcessedLabel(string baseLabel, Room room)
		{
			return baseLabel;
		}

		// Token: 0x06000C16 RID: 3094
		public abstract float GetScore(Room room);

		// Token: 0x06000C17 RID: 3095 RVA: 0x0004038A File Offset: 0x0003E58A
		public virtual float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			return 0f;
		}
	}
}
