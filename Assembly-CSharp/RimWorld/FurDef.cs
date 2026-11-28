using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFA RID: 4090
	public class FurDef : StyleItemDef
	{
		// Token: 0x06006734 RID: 26420 RVA: 0x00248D7C File Offset: 0x00246F7C
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string furBodyGraphicPath = this.GetFurBodyGraphicPath(pawn);
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(furBodyGraphicPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutSkinOverlay, Vector2.one, color);
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x00248DB8 File Offset: 0x00246FB8
		public string GetFurBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x040040EB RID: 16619
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths;
	}
}
