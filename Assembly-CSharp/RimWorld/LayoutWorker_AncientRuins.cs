using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001223 RID: 4643
	public class LayoutWorker_AncientRuins : LayoutWorker_Structure
	{
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06007337 RID: 29495 RVA: 0x00284EC7 File Offset: 0x002830C7
		protected override float RoomToExteriorDoorRatio
		{
			get
			{
				return 0.33f;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06007338 RID: 29496 RVA: 0x00284ECE File Offset: 0x002830CE
		protected virtual float RandomBlastDoorRatio
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06007339 RID: 29497 RVA: 0x00284ED5 File Offset: 0x002830D5
		protected virtual bool SpawnRandomBlastDoors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600733A RID: 29498 RVA: 0x00284ED8 File Offset: 0x002830D8
		protected override ThingDef GetWallDoorStuff(StructureGenParams parms)
		{
			ThingDef wallDoorStuff = base.GetWallDoorStuff(parms);
			if (wallDoorStuff != ThingDefOf.WoodLog)
			{
				return wallDoorStuff;
			}
			return ThingDefOf.Steel;
		}

		// Token: 0x0600733B RID: 29499 RVA: 0x00284EFC File Offset: 0x002830FC
		public LayoutWorker_AncientRuins(LayoutDef def) : base(def)
		{
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x00284F08 File Offset: 0x00283108
		protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
		{
			LayoutStructureSketch sketch = parms.sketch;
			float areaPrunePercent = base.Def.areaPrunePercent;
			int minRoomHeight = base.Def.minRoomHeight;
			int minRoomWidth = base.Def.minRoomWidth;
			int minRoomHeight2 = minRoomHeight;
			float areaPrunePercent2 = areaPrunePercent;
			bool canRemoveRooms = true;
			bool generateDoors = false;
			IntRange? maxMergeRoomsRange = new IntRange?(IntRange.One);
			return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent2, canRemoveRooms, generateDoors, base.Def.corridorDef ?? LayoutRoomDefOf.AncientRuinsCorridor, 2, maxMergeRoomsRange, base.Def.corridorShapes, base.Def.canDisconnectRooms, false);
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x00284F81 File Offset: 0x00283181
		protected override void PostLayoutFlushedToSketch(LayoutStructureSketch parms)
		{
			base.PostLayoutFlushedToSketch(parms);
			if (this.SpawnRandomBlastDoors)
			{
				this.ReplaceRandomBlastDoors(parms.layoutSketch);
			}
		}

		// Token: 0x0600733E RID: 29502 RVA: 0x00284FA0 File Offset: 0x002831A0
		protected virtual void ReplaceRandomBlastDoors(LayoutSketch sketch)
		{
			int num = Mathf.CeilToInt((float)sketch.Things.Count((SketchThing thing) => thing.def.IsDoor) * this.RandomBlastDoorRatio);
			foreach (SketchThing sketchThing in sketch.Things.InRandomOrder(null))
			{
				if (sketchThing.def.IsDoor && sketchThing.def != ThingDefOf.AncientBlastDoor)
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
	}
}
