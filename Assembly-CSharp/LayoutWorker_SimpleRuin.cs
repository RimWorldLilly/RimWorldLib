using System;
using RimWorld;
using Verse;

// Token: 0x02000014 RID: 20
public class LayoutWorker_SimpleRuin : LayoutWorker_Structure
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000079 RID: 121 RVA: 0x000062FC File Offset: 0x000044FC
	protected override float RoomToExteriorDoorRatio
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00006303 File Offset: 0x00004503
	public LayoutWorker_SimpleRuin(LayoutDef def) : base(def)
	{
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000630C File Offset: 0x0000450C
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		int minRoomHeight = base.Def.minRoomHeight;
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0f, false, false, null, 2, new IntRange?(IntRange.Zero), CorridorShape.All, true, false);
	}
}
