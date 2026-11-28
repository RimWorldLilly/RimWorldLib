using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AA RID: 170
	public static class ShaderUtility
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x00029A20 File Offset: 0x00027C20
		public static bool SupportsMaskTex(this Shader shader)
		{
			int propertyCount = shader.GetPropertyCount();
			for (int i = 0; i < propertyCount; i++)
			{
				if (shader.GetPropertyNameId(i) == ShaderPropertyIDs.MaskTex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00029A54 File Offset: 0x00027C54
		public static Shader GetSkinShader(Pawn pawn)
		{
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.def.skinShader != null)
				{
					return hediff.def.skinShader.Shader;
				}
			}
			bool dead = pawn.Dead || (pawn.IsMutant && pawn.mutant.HasTurned && pawn.mutant.Def.useCorpseGraphics);
			return ShaderUtility.GetSkinShaderAbstract(pawn.story != null && pawn.story.SkinColorOverriden, dead);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00029B1C File Offset: 0x00027D1C
		public static Shader GetSkinShaderAbstract(bool skinColorOverriden, bool dead)
		{
			if (skinColorOverriden)
			{
				return ShaderDatabase.CutoutSkinColorOverride;
			}
			if (dead)
			{
				return ShaderDatabase.Cutout;
			}
			return ShaderDatabase.CutoutSkin;
		}
	}
}
