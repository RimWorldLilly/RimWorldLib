using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024F RID: 591
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06001151 RID: 4433 RVA: 0x000634C0 File Offset: 0x000616C0
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0006350C File Offset: 0x0006170C
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04000FD6 RID: 4054
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04000FD7 RID: 4055
		public const int MaterialCount = 100;

		// Token: 0x04000FD8 RID: 4056
		private const float Opacity = 0.25f;
	}
}
