using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF5 RID: 4085
	public class HairDef : StyleItemDef
	{
		// Token: 0x0600672B RID: 26411 RVA: 0x00248BC4 File Offset: 0x00246DC4
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string texPath = this.texPath;
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(texPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutHair, Vector2.one, color);
		}
	}
}
