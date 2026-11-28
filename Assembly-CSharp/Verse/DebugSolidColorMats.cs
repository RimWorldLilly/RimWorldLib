using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024D RID: 589
	public static class DebugSolidColorMats
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x000632F0 File Offset: 0x000614F0
		public static Material MaterialOf(Color col)
		{
			Material material;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				return material;
			}
			material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
			DebugSolidColorMats.colorMatDict.Add(col, material);
			return material;
		}

		// Token: 0x04000FD1 RID: 4049
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
