using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001224 RID: 4644
	public class LayoutWorker_AncientRuins_Reactor : LayoutWorker_AncientRuins
	{
		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x0600733F RID: 29503 RVA: 0x0028505C File Offset: 0x0028325C
		protected override float RoomToExteriorDoorRatio
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x00285063 File Offset: 0x00283263
		protected override bool SpawnRandomBlastDoors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007341 RID: 29505 RVA: 0x00285066 File Offset: 0x00283266
		public LayoutWorker_AncientRuins_Reactor(LayoutDef def) : base(def)
		{
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x00285070 File Offset: 0x00283270
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			int minRoomWidth = base.Def.minRoomWidth;
			int minRoomHeight2 = minRoomHeight;
			float areaPrunePercent = 0.1f;
			bool canRemoveRooms = true;
			bool generateDoors = false;
			IntRange? maxMergeRoomsRange = new IntRange?(new IntRange(2, 4));
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent, canRemoveRooms, generateDoors, base.Def.corridorDef, 2, maxMergeRoomsRange, base.Def.corridorShapes, false, false);
		}
	}
}
