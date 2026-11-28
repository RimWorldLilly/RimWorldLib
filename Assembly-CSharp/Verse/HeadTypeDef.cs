using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public class HeadTypeDef : Def
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003BE7C File Offset: 0x0003A07C
		public Graphic_Multi GetGraphic(Pawn pawn, Color color)
		{
			Shader shader = (pawn.Drawer.renderer.StatueColor != null) ? ShaderDatabase.Cutout : ShaderUtility.GetSkinShader(pawn);
			for (int i = 0; i < this.graphics.Count; i++)
			{
				if (color.IndistinguishableFrom(this.graphics[i].Key) && this.graphics[i].Value.Shader == shader)
				{
					return this.graphics[i].Value;
				}
			}
			Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, shader, Vector2.one, color);
			this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
			return graphic_Multi;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0003BF47 File Offset: 0x0003A147
		public virtual Texture2D Icon
		{
			get
			{
				return ContentFinder<Texture2D>.Get(this.graphicPath + "_south", true);
			}
		}

		// Token: 0x0400086E RID: 2158
		public string graphicPath;

		// Token: 0x0400086F RID: 2159
		public Gender gender;

		// Token: 0x04000870 RID: 2160
		public bool narrow;

		// Token: 0x04000871 RID: 2161
		public Vector2 hairMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04000872 RID: 2162
		public Vector2 beardMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04000873 RID: 2163
		public Vector3 beardOffset;

		// Token: 0x04000874 RID: 2164
		public Vector3? eyeOffsetEastWest;

		// Token: 0x04000875 RID: 2165
		public float beardOffsetXEast;

		// Token: 0x04000876 RID: 2166
		public float selectionWeight = 1f;

		// Token: 0x04000877 RID: 2167
		public bool randomChosen = true;

		// Token: 0x04000878 RID: 2168
		public List<GeneDef> requiredGenes;

		// Token: 0x04000879 RID: 2169
		[Unsaved(false)]
		private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
	}
}
