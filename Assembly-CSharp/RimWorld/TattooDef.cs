using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF8 RID: 4088
	public class TattooDef : StyleItemDef
	{
		// Token: 0x06006730 RID: 26416 RVA: 0x00248CB8 File Offset: 0x00246EB8
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string maskPath = (this.tattooType == TattooType.Body) ? pawn.story.bodyType.bodyNakedGraphicPath : pawn.story.headType.graphicPath;
			string texPath = this.texPath;
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(texPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutSkinOverlay, Vector2.one, color, Color.white, null, maskPath);
		}

		// Token: 0x040040E7 RID: 16615
		public TattooType tattooType;

		// Token: 0x040040E8 RID: 16616
		public bool visibleNorth = true;
	}
}
