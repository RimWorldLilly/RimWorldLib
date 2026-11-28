using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001295 RID: 4757
	public abstract class GenStep_ArchonexusResearchBuildings : GenStep_ScattererBestFit
	{
		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x060075FC RID: 30204 RVA: 0x00297D56 File Offset: 0x00295F56
		public override int SeedPart
		{
			get
			{
				return 746395733;
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x060075FD RID: 30205 RVA: 0x00297D5D File Offset: 0x00295F5D
		protected override IntVec2 Size
		{
			get
			{
				return new IntVec2(20, 20);
			}
		}

		// Token: 0x060075FE RID: 30206 RVA: 0x00297D68 File Offset: 0x00295F68
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = false;
			base.Generate(map, parms);
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x00297D80 File Offset: 0x00295F80
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
			if (cell.Roofed(map))
			{
				return true;
			}
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
	}
}
