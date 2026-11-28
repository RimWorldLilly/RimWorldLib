using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FF RID: 1279
	public static class MaterialLoader
	{
		// Token: 0x060025EC RID: 9708 RVA: 0x000E7190 File Offset: 0x000E5390
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			return (from Texture2D tex in Resources.LoadAll("Textures/" + dirPath, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000E71E8 File Offset: 0x000E53E8
		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
				return BaseContent.BadMat;
			}
			return material;
		}
	}
}
