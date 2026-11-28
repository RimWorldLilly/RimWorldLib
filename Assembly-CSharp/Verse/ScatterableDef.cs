using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B7 RID: 439
	public class ScatterableDef : Def
	{
		// Token: 0x06000C2C RID: 3116 RVA: 0x00040654 File Offset: 0x0003E854
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
				base.ResolveDefNameHash();
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Terrain, ContentFinder<Texture2D>.Get(this.texturePath, true), null);
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x04000B80 RID: 2944
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000B81 RID: 2945
		public float minSize;

		// Token: 0x04000B82 RID: 2946
		public float maxSize;

		// Token: 0x04000B83 RID: 2947
		public float scatterChance = 0.5f;

		// Token: 0x04000B84 RID: 2948
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04000B85 RID: 2949
		public bool placeUnderNaturalRoofs = true;

		// Token: 0x04000B86 RID: 2950
		public Material mat;
	}
}
