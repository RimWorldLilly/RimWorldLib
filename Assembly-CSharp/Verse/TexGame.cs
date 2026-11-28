using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E3 RID: 227
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x06000862 RID: 2146 RVA: 0x00030260 File Offset: 0x0002E460
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
			Shader.SetGlobalTexture("_SpecularNoiseTex", TexGame.SpecularNoiseTex);
		}

		// Token: 0x040003BF RID: 959
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040003C0 RID: 960
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040003C1 RID: 961
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x040003C2 RID: 962
		public static readonly Texture2D InvisDistortion = ContentFinder<Texture2D>.Get("Other/InvisDistortion", true);

		// Token: 0x040003C3 RID: 963
		public static readonly Texture2D SpecularNoiseTex = ContentFinder<Texture2D>.Get("Other/SpecularMetal", true);
	}
}
