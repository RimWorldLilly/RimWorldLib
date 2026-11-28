using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200214D RID: 8525
	public class SymbolResolver_Interior_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x0600D52D RID: 54573 RVA: 0x004F06B0 File Offset: 0x004EE8B0
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 max = rp.rect.Max;
			max.x -= rp.rect.Width / 2;
			max.z--;
			CellRect rect = rp.rect;
			rect.maxZ -= 3;
			ResolveParams resolveParams = rp;
			if (rp.sitePart != null)
			{
				resolveParams.singleThingToSpawn = rp.sitePart.things.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientTerminal_Worshipful);
			}
			if (resolveParams.singleThingToSpawn == null)
			{
				resolveParams.singleThingToSpawn = ThingMaker.MakeThing(ThingDefOf.AncientTerminal_Worshipful, null);
			}
			resolveParams.rect = CellRect.CenteredOn(max, 1, 1);
			resolveParams.thingRot = new Rot4?(Rot4.South);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			if (rect.Height > 1)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				resolveParams2.singleThingDef = ThingDefOf.SteleLarge;
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
				if (Rand.Chance(0.5f))
				{
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
		}
	}
}
