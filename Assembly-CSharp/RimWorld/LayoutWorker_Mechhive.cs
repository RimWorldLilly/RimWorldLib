using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001226 RID: 4646
	public class LayoutWorker_Mechhive : LayoutWorker_Structure
	{
		// Token: 0x06007345 RID: 29509 RVA: 0x0028513C File Offset: 0x0028333C
		public LayoutWorker_Mechhive(LayoutDef def) : base(def)
		{
		}

		// Token: 0x06007346 RID: 29510 RVA: 0x00285148 File Offset: 0x00283348
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			int minRoomHeight = base.Def.minRoomHeight;
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, base.Def.minRoomWidth, minRoomHeight, 0.1f, true, false, null, 2, new IntRange?(new IntRange(2, 4)), CorridorShape.All, true, false);
		}

		// Token: 0x06007347 RID: 29511 RVA: 0x00285192 File Offset: 0x00283392
		protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
		{
			base.PostLayoutFlushedToSketch(parms);
			LayoutWorker_Mechhive.ReplaceDoors(parms.layoutSketch);
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x002851A8 File Offset: 0x002833A8
		private static void ReplaceDoors(LayoutSketch sketch)
		{
			int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * 0.2f);
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

		// Token: 0x040048E2 RID: 18658
		private const float BlastDoorRatio = 0.2f;
	}
}
