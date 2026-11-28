using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020D4 RID: 8404
	public class SketchResolver_MechhiveProcessorArray : SketchResolver
	{
		// Token: 0x0600D383 RID: 54147 RVA: 0x004DF830 File Offset: 0x004DDA30
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null && parms.rect != null;
		}

		// Token: 0x0600D384 RID: 54148 RVA: 0x004DF848 File Offset: 0x004DDA48
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Mechhive guard outpost"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			CellRect value = parms.rect.Value;
			int num = parms.rect.Value.Width / (ThingDefOf.MechanoidProcessor.size.x + 1);
			int num2 = parms.rect.Value.Height / (ThingDefOf.MechanoidProcessor.size.z + 1);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					IntVec3 pos = new IntVec3(2, 0, 2) + new IntVec3(i * (ThingDefOf.MechanoidProcessor.size.x + 1), 0, j * (ThingDefOf.MechanoidProcessor.size.z + 1));
					sketch.AddThing(ThingDefOf.MechanoidProcessor, pos, Rot4.North, null, 1, null, null, true, 2f);
				}
			}
			parms.sketch.Merge(sketch, true);
		}
	}
}
