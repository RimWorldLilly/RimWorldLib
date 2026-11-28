using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000397 RID: 919
	public static class InvisibilityMatPool
	{
		// Token: 0x06001BFE RID: 7166 RVA: 0x000AD954 File Offset: 0x000ABB54
		public static Material GetInvisibleMat(Material baseMat)
		{
			if (baseMat == null)
			{
				return null;
			}
			Material material;
			if (!InvisibilityMatPool.materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.shader = ShaderDatabase.Invisible;
				material.SetTexture(InvisibilityMatPool.NoiseTex, TexGame.InvisDistortion);
				material.color = InvisibilityMatPool.color;
				InvisibilityMatPool.materials.Add(baseMat, material);
			}
			return material;
		}

		// Token: 0x040016FF RID: 5887
		private static readonly Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x04001700 RID: 5888
		private static readonly Color color = new Color(0.75f, 0.93f, 0.98f, 0.5f);

		// Token: 0x04001701 RID: 5889
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
