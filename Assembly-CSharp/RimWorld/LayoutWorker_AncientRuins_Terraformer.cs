using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001225 RID: 4645
	public class LayoutWorker_AncientRuins_Terraformer : LayoutWorker_AncientRuins
	{
		// Token: 0x06007343 RID: 29507 RVA: 0x002850D0 File Offset: 0x002832D0
		public LayoutWorker_AncientRuins_Terraformer(LayoutDef def) : base(def)
		{
		}

		// Token: 0x06007344 RID: 29508 RVA: 0x002850DC File Offset: 0x002832DC
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
