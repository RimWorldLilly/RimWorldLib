using System;
using RimWorld;
using Verse;

// Token: 0x02000013 RID: 19
public class LayoutWorker_OrbitalPlatform : LayoutWorker_Structure
{
	// Token: 0x06000077 RID: 119 RVA: 0x000062A9 File Offset: 0x000044A9
	public LayoutWorker_OrbitalPlatform(LayoutDef def) : base(def)
	{
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000062B4 File Offset: 0x000044B4
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		int minRoomWidth = 10;
		int minRoomHeight = 10;
		float areaPrunePercent = 0.1f;
		bool canRemoveRooms = true;
		bool generateDoors = false;
		IntRange? maxMergeRoomsRange = new IntRange?(new IntRange(2, 4));
		CorridorShape corridorShapes = base.Def.corridorShapes;
		return RoomLayoutGenerator.GenerateRandomLayout(rect, minRoomWidth, minRoomHeight, areaPrunePercent, canRemoveRooms, generateDoors, base.Def.corridorDef, 2, maxMergeRoomsRange, corridorShapes, false, false);
	}
}
