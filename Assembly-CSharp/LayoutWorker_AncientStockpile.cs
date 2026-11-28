using System;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x02000011 RID: 17
public class LayoutWorker_AncientStockpile : LayoutWorker_Structure
{
	// Token: 0x0600005C RID: 92 RVA: 0x00004D3C File Offset: 0x00002F3C
	public LayoutWorker_AncientStockpile(LayoutDef def) : base(def)
	{
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004D48 File Offset: 0x00002F48
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		int minRoomHeight = base.Def.minRoomHeight;
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.25f, true, false, null, 2, new IntRange?(new IntRange(2, 4)), CorridorShape.All, false, false);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004D94 File Offset: 0x00002F94
	protected override void PostGraphsGenerated(StructureLayout layout, StructureGenParams parms)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			layoutRoom.noExteriorDoors = (base.Def.exteriorDoorDef == null);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00004DF4 File Offset: 0x00002FF4
	protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
	{
		base.PostLayoutFlushedToSketch(parms);
		LayoutWorker_AncientStockpile.ReplaceDoors(parms.layoutSketch);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004E08 File Offset: 0x00003008
	private static void ReplaceDoors(LayoutSketch sketch)
	{
		int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * 0.5f);
		foreach (SketchThing sketchThing in sketch.Things.InRandomOrder(null))
		{
			if (sketchThing.def.IsDoor)
			{
				sketchThing.def = ThingDefOf.AncientBlastDoor;
				sketchThing.stuff = null;
				num--;
				if (num <= 0)
				{
					break;
				}
			}
		}
	}

	// Token: 0x0400001B RID: 27
	private const float BlastDoorRatio = 0.5f;
}
