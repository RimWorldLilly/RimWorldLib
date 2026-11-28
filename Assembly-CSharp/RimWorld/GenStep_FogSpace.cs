using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A4 RID: 4772
	public class GenStep_FogSpace : GenStep
	{
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06007652 RID: 30290 RVA: 0x0029A94C File Offset: 0x00298B4C
		public override int SeedPart
		{
			get
			{
				return 492371731;
			}
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x0029A954 File Offset: 0x00298B54
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_FogSpace.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.map.fogGrid.SetAllFogged();
			foreach (IntVec3 intVec in CS$<>8__locals1.map.BoundsRect(0).Corners)
			{
				if (GenStep_FogSpace.<Generate>g__Validator|2_0(intVec, ref CS$<>8__locals1))
				{
					FloodFillerFog.FloodUnfog(intVec, CS$<>8__locals1.map);
				}
			}
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], CS$<>8__locals1.map);
			}
		}

		// Token: 0x06007655 RID: 30293 RVA: 0x0029AA14 File Offset: 0x00298C14
		[CompilerGenerated]
		internal static bool <Generate>g__Validator|2_0(IntVec3 c, ref GenStep_FogSpace.<>c__DisplayClass2_0 A_1)
		{
			return c.GetEdifice(A_1.map) == null && !c.Roofed(A_1.map);
		}
	}
}
