using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020BA RID: 8378
	public class SketchResolver_AncientHatch : SketchResolver
	{
		// Token: 0x0600D316 RID: 54038 RVA: 0x004DAB5C File Offset: 0x004D8D5C
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x0600D317 RID: 54039 RVA: 0x004DAB68 File Offset: 0x004D8D68
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Ancient hatch"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect cellRect = new CellRect(0, 0, 7, 7);
			foreach (IntVec3 pos in cellRect)
			{
				sketch.AddTerrain(TerrainDefOf.AncientConcrete, pos, true);
			}
			foreach (IntVec3 pos2 in cellRect.ContractedBy(1))
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos2, true);
			}
			foreach (IntVec3 pos3 in cellRect.ContractedBy(1).Corners)
			{
				sketch.AddThing(ThingDefOf.AncientSecurityTurret, pos3, Rot4.North, null, 1, null, null, true, 2f);
			}
			sketch.AddThing(ThingDefOf.AncientHatch, cellRect.CenterCell, Rot4.North, null, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}
	}
}
