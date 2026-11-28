using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000566 RID: 1382
	public class Graphic_FleckSplash : Graphic_Fleck
	{
		// Token: 0x0600290D RID: 10509 RVA: 0x000FC634 File Offset: 0x000FA834
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, drawData.alpha));
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, drawData.calculatedShockwaveSpan);
			drawData.drawLayer = SubcameraDefOf.WaterDepth.LayerId;
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000FC6B4 File Offset: 0x000FA8B4
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "FleckSplash(path=";
			array[1] = this.path;
			array[2] = ", shader=";
			int num = 3;
			Shader shader = base.Shader;
			array[num] = ((shader != null) ? shader.ToString() : null);
			array[4] = ", color=";
			array[5] = this.color.ToString();
			array[6] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}
	}
}
