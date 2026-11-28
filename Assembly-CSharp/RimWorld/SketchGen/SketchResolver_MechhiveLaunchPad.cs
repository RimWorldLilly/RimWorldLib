using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020D3 RID: 8403
	public class SketchResolver_MechhiveLaunchPad : SketchResolver
	{
		// Token: 0x0600D380 RID: 54144 RVA: 0x004DF664 File Offset: 0x004DD864
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null && parms.rect != null;
		}

		// Token: 0x0600D381 RID: 54145 RVA: 0x004DF67C File Offset: 0x004DD87C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Mechhive guard outpost"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect value = parms.rect.Value;
			foreach (IntVec3 pos in value.Cells)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(1, 0, 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(1, 0, value.maxZ - 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(value.maxX - 1, 0, 1), Rot4.North, null, 1, null, null, true, 2f);
			sketch.AddThing(Rand.Bool ? ThingDefOf.MechanoidDropPod : ThingDefOf.Filth_BlastMark, new IntVec3(value.maxX - 1, 0, value.maxZ - 1), Rot4.North, null, 1, null, null, true, 2f);
			parms.sketch.Merge(sketch, true);
		}
	}
}
