using System;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x020022D4 RID: 8916
	public class GameSetupStep_Grids : GameSetupStep
	{
		// Token: 0x1700263B RID: 9787
		// (get) Token: 0x0600DBDE RID: 56286 RVA: 0x0051E824 File Offset: 0x0051CA24
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x0600DBDF RID: 56287 RVA: 0x0051E82B File Offset: 0x0051CA2B
		public override void GenerateFresh()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(WorldTerrainColliderManager.ClearCache));
			Find.World.grid = new WorldGrid();
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
			Find.WorldGrid.GenerateFreshWorld();
		}

		// Token: 0x0600DBE0 RID: 56288 RVA: 0x0051E86B File Offset: 0x0051CA6B
		public override void GenerateFromScribe()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(WorldTerrainColliderManager.ClearCache));
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}
	}
}
