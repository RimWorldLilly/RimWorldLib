using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132B RID: 4907
	public class TileMutatorWorker_IceCaves : TileMutatorWorker_Caves
	{
		// Token: 0x060079A8 RID: 31144 RVA: 0x002B4E4A File Offset: 0x002B304A
		public TileMutatorWorker_IceCaves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079A9 RID: 31145 RVA: 0x002B4E53 File Offset: 0x002B3053
		public override void GeneratePostElevationFertility(Map map)
		{
		}

		// Token: 0x060079AA RID: 31146 RVA: 0x002B4E58 File Offset: 0x002B3058
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.GenerateCaves(map);
			MapGenFloatGrid caves = MapGenerator.Caves;
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f)
				{
					Thing edifice = intVec.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
						list.Add(intVec);
					}
				}
			}
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
			base.GeneratePostTerrain(map);
		}

		// Token: 0x060079AB RID: 31147 RVA: 0x002B4EF4 File Offset: 0x002B30F4
		protected override bool ShouldCarve(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			if (c.InBounds(map))
			{
				Building edifice = c.GetEdifice(map);
				return ((edifice != null) ? edifice.def : null) == ThingDefOf.SolidIce;
			}
			return false;
		}
	}
}
