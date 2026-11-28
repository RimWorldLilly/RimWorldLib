using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A3 RID: 4771
	public class GenStep_Fog : GenStep
	{
		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x0600764E RID: 30286 RVA: 0x0029A7D6 File Offset: 0x002989D6
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x0600764F RID: 30287 RVA: 0x0029A7E0 File Offset: 0x002989E0
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("GenerateInitialFogGrid");
			map.fogGrid.SetAllFogged();
			if (MapGenerator.PlayerStartSpot.IsValid)
			{
				IntVec3 intVec = MapGenerator.PlayerStartSpot;
				Building edifice = intVec.GetEdifice(map);
				if (edifice != null && edifice.def.MakeFog)
				{
					intVec = CellFinder.StandableCellNear(intVec, map, 5f, (IntVec3 c) => !c.Roofed(map));
				}
				FloodFillerFog.FloodUnfog(intVec, map);
			}
			else
			{
				GenStep_Fog.UnfogMapFromEdge(map);
			}
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], map);
				map.fogGrid.Unfog(rootsToUnfog[i]);
			}
			DeepProfiler.End();
		}

		// Token: 0x06007650 RID: 30288 RVA: 0x0029A8CC File Offset: 0x00298ACC
		private static void UnfogMapFromEdge(Map map)
		{
			Predicate<IntVec3> validator = (IntVec3 c) => c.Standable(map) && !c.Roofed(map) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.NoPassClosedDoorsOrWater, Danger.Deadly, false, false, false, true, false));
			IntVec3 root;
			if (!CellFinder.TryFindRandomCellNear(map.Center, map, 30, validator, out root, -1) && !CellFinder.TryFindRandomEdgeCellWith(validator, map, 0f, out root) && !CellFinder.TryFindRandomCell(map, validator, out root))
			{
				return;
			}
			FloodFillerFog.FloodUnfog(root, map);
		}
	}
}
