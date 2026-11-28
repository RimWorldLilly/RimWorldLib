using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200210D RID: 8461
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x0600D46B RID: 54379 RVA: 0x004E82C8 File Offset: 0x004E64C8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof != null && rp.noRoof.Value)
			{
				return;
			}
			RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
			RoofDef def = rp.roofDef ?? RoofDefOf.RoofConstructed;
			foreach (IntVec3 c in rp.rect)
			{
				if (!roofGrid.Roofed(c))
				{
					roofGrid.SetRoof(c, def);
				}
			}
		}
	}
}
