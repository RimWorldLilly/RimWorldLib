using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000389 RID: 905
	public static class DamagedMatPool
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x000A98E2 File Offset: 0x000A7AE2
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000A98F0 File Offset: 0x000A7AF0
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			if (damPct < 0.01f)
			{
				return baseMat;
			}
			Material material;
			if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				DamagedMatPool.damagedMats.Add(baseMat, material);
			}
			material.color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
			return material;
		}

		// Token: 0x04001692 RID: 5778
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04001693 RID: 5779
		public static readonly Color DamagedMatStartingColor = Color.red;
	}
}
