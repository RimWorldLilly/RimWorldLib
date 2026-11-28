using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E5 RID: 4837
	public class GenStep_AncientAltar : GenStep_ScattererBestFit
	{
		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x060077FE RID: 30718 RVA: 0x002A748E File Offset: 0x002A568E
		public override int SeedPart
		{
			get
			{
				return 572334943;
			}
		}

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x060077FF RID: 30719 RVA: 0x002A7495 File Offset: 0x002A5695
		protected override IntVec2 Size
		{
			get
			{
				return SymbolResolver_AncientAltar.Size;
			}
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x002A749C File Offset: 0x002A569C
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.warnOnFail = false;
			base.Generate(map, parms);
		}

		// Token: 0x06007801 RID: 30721 RVA: 0x002A74B4 File Offset: 0x002A56B4
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
			TerrainDef terrain = cell.GetTerrain(map);
			if (terrain != null && (terrain.IsWater || terrain.IsRoad))
			{
				return true;
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial || (thingList[i].def.building != null && thingList[i].def.building.isNaturalRock))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x002A7538 File Offset: 0x002A5738
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (!base.TryFindScatterCell(map, out result))
			{
				result = map.Center;
			}
			return true;
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x002A7554 File Offset: 0x002A5754
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.triggerSecuritySignal = parms2.triggerSecuritySignal;
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.relicThing = parms2.relicThing;
			resolveParams.interiorThreatPoints = ((parms2.interiorThreatPoints > 0f) ? new float?(parms2.interiorThreatPoints) : null);
			resolveParams.exteriorThreatPoints = ((parms2.exteriorThreatPoints > 0f) ? new float?(parms2.exteriorThreatPoints) : null);
			resolveParams.rect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientAltar", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(resolveParams.rect);
			parms.sitePart.relicWasSpawned = true;
		}
	}
}
