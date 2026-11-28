using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9E RID: 3742
	public static class TerrainDefGenerator_Stone
	{
		// Token: 0x06006293 RID: 25235 RVA: 0x002378FE File Offset: 0x00235AFE
		public static IEnumerable<TerrainDef> ImpliedTerrainDefs(bool hotReload = false)
		{
			TerrainDefGenerator_Stone.<ImpliedTerrainDefs>d__0 <ImpliedTerrainDefs>d__ = new TerrainDefGenerator_Stone.<ImpliedTerrainDefs>d__0(-2);
			<ImpliedTerrainDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedTerrainDefs>d__;
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x00237910 File Offset: 0x00235B10
		private static void SetAsteroidEdgeData(TerrainDef terrain)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (terrain.spaceEdgeGraphicData == null)
			{
				terrain.spaceEdgeGraphicData = new TerrainDef.SpaceEdgeGraphicData
				{
					OShapeTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_OShape",
					UShapeTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_UShape",
					CornerInnerTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_CornerInner",
					CornerOuterTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_CornerOuter",
					FlatTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_Flat",
					LoopLeftTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Left",
					LoopRightTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Right",
					LoopSingleTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Single",
					LoopTexPaths = new List<string>
					{
						"Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop1",
						"Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop2",
						"Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop3",
						"Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop4",
						"Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop5"
					}
				};
			}
		}
	}
}
