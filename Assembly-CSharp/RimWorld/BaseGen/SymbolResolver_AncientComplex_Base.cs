using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020DC RID: 8412
	public abstract class SymbolResolver_AncientComplex_Base : SymbolResolver
	{
		// Token: 0x170025EB RID: 9707
		// (get) Token: 0x0600D3B7 RID: 54199
		protected abstract LayoutDef DefaultLayoutDef { get; }

		// Token: 0x0600D3B8 RID: 54200 RVA: 0x004E2AE4 File Offset: 0x004E0CE4
		public void ResolveComplex(ResolveParams rp)
		{
			if (rp.ancientLayoutStructureSketch == null)
			{
				StructureGenParams parms = new StructureGenParams
				{
					size = new IntVec2(rp.rect.Width, rp.rect.Height)
				};
				rp.ancientLayoutStructureSketch = this.DefaultLayoutDef.Worker.GenerateStructureSketch(parms);
			}
			ResolveParams resolveParams = rp;
			resolveParams.ancientLayoutStructureSketch = rp.ancientLayoutStructureSketch;
			BaseGen.symbolStack.Push("ancientComplexSketch", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = TerrainDefOf.AncientConcrete;
			resolveParams2.allowBridgeOnAnyImpassableTerrain = new bool?(true);
			resolveParams2.floorOnlyIfTerrainSupports = new bool?(false);
			foreach (LayoutRoom layoutRoom in rp.ancientLayoutStructureSketch.structureLayout.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					resolveParams2.rect = cellRect.MovedBy(rp.rect.Min);
					BaseGen.symbolStack.Push("floor", resolveParams2, null);
					BaseGen.symbolStack.Push("clear", resolveParams2, null);
				}
			}
		}
	}
}
