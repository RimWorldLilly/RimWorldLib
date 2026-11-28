using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CD RID: 4813
	public class GenStep_Space : GenStep
	{
		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x0600776B RID: 30571 RVA: 0x002A1F70 File Offset: 0x002A0170
		public override int SeedPart
		{
			get
			{
				return 196743;
			}
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x002A1F78 File Offset: 0x002A0178
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = false;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in map.AllCells)
			{
				terrainGrid.SetTerrain(c, TerrainDefOf.Space);
			}
		}
	}
}
