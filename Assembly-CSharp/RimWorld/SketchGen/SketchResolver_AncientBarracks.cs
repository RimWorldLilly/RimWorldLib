using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B8 RID: 8376
	public class SketchResolver_AncientBarracks : SketchResolver
	{
		// Token: 0x0600D30C RID: 54028 RVA: 0x004DA5C3 File Offset: 0x004D87C3
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D30D RID: 54029 RVA: 0x004DA5E0 File Offset: 0x004D87E0
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient barracks"))
			{
				return;
			}
			CellRect rect = parms.rect.Value;
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			ThingDef ancientBed = ThingDefOf.AncientBed;
			CellRect lhs = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any<SketchThing>(), null, 3);
			if (lhs == CellRect.Empty)
			{
				return;
			}
			SketchResolveParams parms3 = parms;
			parms3.chance = new float?(1f);
			parms3.wallEdgeThing = ThingDefOf.AncientLockerBank;
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
			SketchResolver_AncientBarracks.tmpCells.Clear();
			if (lhs.Width > lhs.Height)
			{
				SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.North));
				SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.South));
				using (List<IntVec3>.Enumerator enumerator = SketchResolver_AncientBarracks.tmpCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 intVec = enumerator.Current;
						if (this.CanPlaceBedAt(ancientBed, intVec, Rot4.North, parms.sketch))
						{
							parms.sketch.AddThing(ancientBed, intVec, Rot4.North, null, 1, null, null, false, 2f);
						}
						if (this.CanPlaceBedAt(ancientBed, intVec, Rot4.South, parms.sketch))
						{
							parms.sketch.AddThing(ancientBed, intVec, Rot4.South, null, 1, null, null, false, 2f);
						}
					}
					goto IL_2E3;
				}
			}
			SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.East));
			SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.West));
			foreach (IntVec3 intVec2 in SketchResolver_AncientBarracks.tmpCells)
			{
				if (this.CanPlaceBedAt(ancientBed, intVec2, Rot4.East, parms.sketch))
				{
					parms.sketch.AddThing(ancientBed, intVec2, Rot4.East, null, 1, null, null, false, 2f);
				}
				if (this.CanPlaceBedAt(ancientBed, intVec2, Rot4.West, parms.sketch))
				{
					parms.sketch.AddThing(ancientBed, intVec2, Rot4.West, null, 1, null, null, false, 2f);
				}
			}
			IL_2E3:
			SketchResolver_AncientBarracks.tmpCells.Clear();
		}

		// Token: 0x0600D30E RID: 54030 RVA: 0x004DA8F8 File Offset: 0x004D8AF8
		private bool CanPlaceBedAt(ThingDef def, IntVec3 position, Rot4 rot, Sketch sketch)
		{
			CellRect cellRect = GenAdj.OccupiedRect(position, rot, def.size);
			foreach (IntVec3 pos in cellRect.Cells)
			{
				if (sketch.ThingsAt(pos).Any((SketchThing x) => x.def == ThingDefOf.Wall))
				{
					return false;
				}
			}
			bool result = false;
			foreach (IntVec3 pos2 in cellRect.ExpandedBy(1).EdgeCells)
			{
				using (IEnumerator<SketchThing> enumerator2 = sketch.ThingsAt(pos2).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.def != ThingDefOf.Wall)
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x040085D4 RID: 34260
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
