using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001227 RID: 4647
	public class LayoutWorker_SingleRoom : LayoutWorker_Structure
	{
		// Token: 0x06007349 RID: 29513 RVA: 0x00285254 File Offset: 0x00283454
		public LayoutWorker_SingleRoom(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x00285260 File Offset: 0x00283460
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.2f, false, false, null, 2, null, CorridorShape.All, true, true);
		}
	}
}
