using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020FE RID: 8446
	public class SymbolResolver_EmptyRoom : SymbolResolver
	{
		// Token: 0x0600D430 RID: 54320 RVA: 0x004E6544 File Offset: 0x004E4744
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, false, rp.faction);
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(rp.rect);
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				BaseGen.symbolStack.Push("roof", rp, null);
			}
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = thingDef;
			BaseGen.symbolStack.Push("edgeWalls", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams2, null);
			BaseGen.symbolStack.Push("clear", rp, null);
			if (rp.addRoomCenterToRootsToUnfog != null && rp.addRoomCenterToRootsToUnfog.Value && Current.ProgramState == ProgramState.MapInitializing)
			{
				MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
			}
		}
	}
}
