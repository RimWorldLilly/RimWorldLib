using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C1 RID: 8385
	public class SketchResolver_AncientStorageRoom : SketchResolver
	{
		// Token: 0x170025E3 RID: 9699
		// (get) Token: 0x0600D32D RID: 54061 RVA: 0x004DB368 File Offset: 0x004D9568
		private static IEnumerable<ThingDef> PossibleBuildings
		{
			get
			{
				return new SketchResolver_AncientStorageRoom.<get_PossibleBuildings>d__1(-2);
			}
		}

		// Token: 0x0600D32E RID: 54062 RVA: 0x004DB371 File Offset: 0x004D9571
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D32F RID: 54063 RVA: 0x004DB38C File Offset: 0x004D958C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient storage room"))
			{
				return;
			}
			CellRect rect = parms.rect.Value;
			CellRect lhs = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any<SketchThing>(), null, 3);
			if (lhs == CellRect.Empty)
			{
				return;
			}
			SketchResolver_AncientStorageRoom.tmpCells.Clear();
			Rot4 dir = (lhs.Width > lhs.Height) ? Rot4.North : Rot4.East;
			SketchResolver_AncientStorageRoom.tmpCells.AddRange(lhs.GetEdgeCells(dir));
			SketchResolver_AncientStorageRoom.tmpCells.AddRange(lhs.GetEdgeCells(dir.Opposite));
			ThingDef def = SketchResolver_AncientStorageRoom.PossibleBuildings.RandomElement<ThingDef>();
			foreach (IntVec3 intVec in SketchResolver_AncientStorageRoom.tmpCells)
			{
				if (Rand.Chance(SketchResolver_AncientStorageRoom.WallChance) && this.CanPlaceWallAdjacentAt(intVec, parms.sketch))
				{
					parms.sketch.AddThing(def, intVec, Rot4.North, null, 1, null, null, true, 2f);
				}
			}
			SketchResolver_AncientStorageRoom.tmpCells.Clear();
			foreach (IntVec3 center in rect.Cells)
			{
				CellRect rect2 = CellRect.CenteredOn(center, 2, 2);
				if (this.CanPlaceAt(rect2, parms.sketch))
				{
					foreach (IntVec3 intVec2 in rect2)
					{
						ThingDef thingDef = SketchResolver_AncientStorageRoom.PossibleBuildings.RandomElement<ThingDef>();
						parms.sketch.AddThing(thingDef, intVec2, Rot4.North, null, 1, null, null, true, 2f);
						ScatterDebrisUtility.ScatterAround(intVec2, thingDef.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.15f, 1, int.MaxValue, null);
					}
				}
			}
		}

		// Token: 0x0600D330 RID: 54064 RVA: 0x004DB604 File Offset: 0x004D9804
		private bool CanPlaceAt(CellRect rect, Sketch sketch)
		{
			foreach (IntVec3 pos in rect.Cells)
			{
				if (sketch.ThingsAt(pos).Any<SketchThing>())
				{
					return false;
				}
			}
			foreach (IntVec3 pos2 in rect.ExpandedBy(1).EdgeCells)
			{
				if (sketch.ThingsAt(pos2).Any((SketchThing t) => t.def != ThingDefOf.Wall && t.def != ThingDefOf.Door))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D331 RID: 54065 RVA: 0x004DB6D4 File Offset: 0x004D98D4
		private bool CanPlaceWallAdjacentAt(IntVec3 position, Sketch sketch)
		{
			bool result = false;
			IntVec3[] cardinalDirectionsAndInside = GenAdj.CardinalDirectionsAndInside;
			for (int i = 0; i < cardinalDirectionsAndInside.Length; i++)
			{
				IntVec3 pos = cardinalDirectionsAndInside[i] + position;
				foreach (SketchThing sketchThing in sketch.ThingsAt(pos))
				{
					if (sketchThing.def == ThingDefOf.Wall)
					{
						result = true;
					}
					else if (sketchThing.def == ThingDefOf.Door)
					{
						return false;
					}
				}
			}
			return result;
		}

		// Token: 0x040085D8 RID: 34264
		private static float WallChance = 0.8f;

		// Token: 0x040085D9 RID: 34265
		private const float OilSmearChance = 0.15f;

		// Token: 0x040085DA RID: 34266
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
