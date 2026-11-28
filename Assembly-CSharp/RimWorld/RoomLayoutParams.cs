using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001263 RID: 4707
	public struct RoomLayoutParams
	{
		// Token: 0x04004934 RID: 18740
		public LayoutStructureSketch sketch;

		// Token: 0x04004935 RID: 18741
		public CellRect container;

		// Token: 0x04004936 RID: 18742
		public int minRoomWidth;

		// Token: 0x04004937 RID: 18743
		public int minRoomHeight;

		// Token: 0x04004938 RID: 18744
		public float areaPrunePercent;

		// Token: 0x04004939 RID: 18745
		public IntRange? maxMergeRoomsRange;

		// Token: 0x0400493A RID: 18746
		public int entranceCount;

		// Token: 0x0400493B RID: 18747
		public bool canRemoveRooms;

		// Token: 0x0400493C RID: 18748
		public bool generateDoors;

		// Token: 0x0400493D RID: 18749
		public bool canDisconnectRooms;

		// Token: 0x0400493E RID: 18750
		public LayoutRoomDef corridor;

		// Token: 0x0400493F RID: 18751
		public int corridorExpansion;

		// Token: 0x04004940 RID: 18752
		public CorridorShape corridorShapes;

		// Token: 0x04004941 RID: 18753
		public bool singleRoom;
	}
}
