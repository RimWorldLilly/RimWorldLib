using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020CF RID: 8399
	public class SketchResolver_Gravship : SketchResolver
	{
		// Token: 0x0600D36E RID: 54126 RVA: 0x004DEAD1 File Offset: 0x004DCCD1
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x0600D36F RID: 54127 RVA: 0x004DEADC File Offset: 0x004DCCDC
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Ancient launch pad"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			sketch.AddThing(ThingDefOf.GravEngine, new IntVec3(7, 0, 4), Rot4.North, null, 1, null, null, true, 0.5f);
			this.GenerateBaseStructure(sketch);
			sketch.AddThing(ThingDefOf.ChemfuelTank, new IntVec3(6, 0, 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.ChemfuelTank, new IntVec3(6, 0, 6), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.SmallThruster, new IntVec3(0, 0, 1), Rot4.East, null, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.SmallThruster, new IntVec3(0, 0, 7), Rot4.East, null, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.PilotConsole, new IntVec3(4, 0, 4), Rot4.East, null, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Stool, new IntVec3(3, 0, 4), Rot4.East, ThingDefOf.WoodLog, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(1, 0, 3), Rot4.East, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(1, 0, 6), Rot4.East, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.ShelfSmall, new IntVec3(1, 0, 4), Rot4.East, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(2, 0, 1), Rot4.North, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(3, 0, 7), Rot4.South, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(5, 0, 1), Rot4.West, ThingDefOf.Steel, 1, null, null, true, 2f);
			sketch.AddThing(ThingDefOf.Shelf, new IntVec3(5, 0, 6), Rot4.West, ThingDefOf.Steel, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}

		// Token: 0x0600D370 RID: 54128 RVA: 0x004DEDFC File Offset: 0x004DCFFC
		private void GenerateBaseStructure(Sketch sketch)
		{
			for (int i = 0; i < SketchResolver_Gravship.BaseStructure.Length; i++)
			{
				int[] array = SketchResolver_Gravship.BaseStructure[i];
				for (int j = 0; j < array.Length; j++)
				{
					IntVec3 pos = new IntVec3(j, 0, i);
					switch (array[j])
					{
					case 1:
						sketch.AddTerrain(TerrainDefOf.Substructure, pos, true);
						break;
					case 2:
						sketch.AddTerrain(TerrainDefOf.Substructure, pos, true);
						sketch.AddThing(ThingDefOf.GravshipHull, pos, Rot4.North, ThingDefOf.Steel, 1, null, null, true, 2f);
						break;
					case 3:
						sketch.AddTerrain(TerrainDefOf.Substructure, pos, true);
						sketch.AddThing(ThingDefOf.Door, pos, Rot4.North, ThingDefOf.Steel, 1, null, null, true, 2f);
						break;
					}
				}
			}
		}

		// Token: 0x040085F1 RID: 34289
		private static readonly int[][] BaseStructure = new int[][]
		{
			new int[]
			{
				0,
				2,
				2,
				2,
				3,
				2,
				2,
				2,
				2,
				0
			},
			new int[]
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2,
				0
			},
			new int[]
			{
				2,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2,
				2
			},
			new int[]
			{
				2,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2
			},
			new int[]
			{
				2,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2
			},
			new int[]
			{
				2,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2
			},
			new int[]
			{
				2,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2,
				2
			},
			new int[]
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2,
				0
			},
			new int[]
			{
				0,
				2,
				2,
				2,
				3,
				2,
				2,
				2,
				2,
				0
			}
		};
	}
}
