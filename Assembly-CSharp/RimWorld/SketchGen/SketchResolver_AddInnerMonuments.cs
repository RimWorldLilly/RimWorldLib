using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C6 RID: 8390
	public class SketchResolver_AddInnerMonuments : SketchResolver
	{
		// Token: 0x0600D344 RID: 54084 RVA: 0x004DC710 File Offset: 0x004DA910
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect outerRect = parms.rect ?? parms.sketch.OccupiedRect;
			HashSet<IntVec3> processed = new HashSet<IntVec3>();
			using (IEnumerator<IntVec3> enumerator = outerRect.Cells.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					CellRect cellRect = SketchGenUtility.FindBiggestRectAt(c, outerRect, parms.sketch, processed, (IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && parms.sketch.AnyTerrainAt(c));
					if (cellRect.Width >= 7 && cellRect.Height >= 7)
					{
						int newX = Rand.RangeInclusive(5, cellRect.Width - 2);
						int newZ = Rand.RangeInclusive(5, cellRect.Height - 2);
						Sketch sketch = new Sketch();
						SketchResolveParams parms2 = parms;
						parms2.sketch = sketch;
						parms2.monumentSize = new IntVec2?(new IntVec2(newX, newZ));
						parms2.rect = null;
						SketchResolverDefOf.Monument.Resolve(parms2);
						parms.sketch.MergeAt(sketch, cellRect.CenterCell, Sketch.SpawnPosType.OccupiedCenter, false);
					}
				}
			}
		}

		// Token: 0x0600D345 RID: 54085 RVA: 0x004DC8A4 File Offset: 0x004DAAA4
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x040085E0 RID: 34272
		private const int MinRectWidth = 7;

		// Token: 0x040085E1 RID: 34273
		private const int MinRectHeight = 7;
	}
}
