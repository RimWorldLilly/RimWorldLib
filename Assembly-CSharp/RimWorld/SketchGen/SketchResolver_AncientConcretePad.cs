using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C9 RID: 8393
	public class SketchResolver_AncientConcretePad : SketchResolver
	{
		// Token: 0x0600D34F RID: 54095 RVA: 0x004DD21F File Offset: 0x004DB41F
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D350 RID: 54096 RVA: 0x004DD23C File Offset: 0x004DB43C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			Sketch sketch = new Sketch();
			foreach (IntVec3 pos in parms.rect.Value.EdgeCells)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
			foreach (IntVec3 pos2 in parms.rect.Value.ContractedBy(1))
			{
				sketch.AddTerrain(TerrainDefOf.AncientConcrete, pos2, true);
			}
			foreach (IntVec3 pos3 in parms.rect.Value.Corners)
			{
				sketch.AddThing(ThingDefOf.AncientShipBeacon, pos3, Rot4.North, null, 1, null, null, true, 2f);
			}
			parms.sketch.Merge(sketch, true);
		}
	}
}
