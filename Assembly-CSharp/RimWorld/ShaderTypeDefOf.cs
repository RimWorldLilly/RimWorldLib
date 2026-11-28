using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECE RID: 7886
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x0600CA89 RID: 51849 RVA: 0x004A61DB File Offset: 0x004A43DB
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDefOf));
		}

		// Token: 0x04007F63 RID: 32611
		public static ShaderTypeDef Cutout;

		// Token: 0x04007F64 RID: 32612
		public static ShaderTypeDef CutoutHair;

		// Token: 0x04007F65 RID: 32613
		public static ShaderTypeDef CutoutTiling;

		// Token: 0x04007F66 RID: 32614
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x04007F67 RID: 32615
		public static ShaderTypeDef Transparent;

		// Token: 0x04007F68 RID: 32616
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04007F69 RID: 32617
		public static ShaderTypeDef EdgeDetect;

		// Token: 0x04007F6A RID: 32618
		public static ShaderTypeDef TerrainFadeRoughLinearAdd;

		// Token: 0x04007F6B RID: 32619
		public static ShaderTypeDef MoteGlow;

		// Token: 0x04007F6C RID: 32620
		[MayRequireBiotech]
		public static ShaderTypeDef TerrainFadeRoughLinearBurn;
	}
}
