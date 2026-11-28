using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000565 RID: 1381
	public class Graphic_FleckPulse : Graphic_Fleck
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x000FC562 File Offset: 0x000FA762
		protected override bool AllowInstancing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000FC568 File Offset: 0x000FA768
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, drawData.ageSecs);
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, drawData.id);
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000FC5C0 File Offset: 0x000FA7C0
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Graphic_FleckPulse(path=";
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
