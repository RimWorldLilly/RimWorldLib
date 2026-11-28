using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF6 RID: 4086
	public class BeardDef : StyleItemDef
	{
		// Token: 0x0600672D RID: 26413 RVA: 0x00248C04 File Offset: 0x00246E04
		public Vector3 GetOffset(HeadTypeDef headType, Rot4 rot)
		{
			if (!headType.narrow || rot == Rot4.North)
			{
				return Vector3.zero;
			}
			if (rot == Rot4.South)
			{
				return this.offsetNarrowSouth;
			}
			if (rot == Rot4.East)
			{
				return this.offsetNarrowEast;
			}
			return new Vector3(-this.offsetNarrowEast.x, 0f, this.offsetNarrowEast.z);
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x00248C75 File Offset: 0x00246E75
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

		// Token: 0x040040E2 RID: 16610
		public Vector3 offsetNarrowEast;

		// Token: 0x040040E3 RID: 16611
		public Vector3 offsetNarrowSouth;
	}
}
