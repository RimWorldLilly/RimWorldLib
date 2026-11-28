using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000564 RID: 1380
	[StaticConstructorOnStartup]
	public class Graphic_Fleck : Graphic_Single
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002904 RID: 10500 RVA: 0x000FC368 File Offset: 0x000FA568
		protected virtual bool AllowInstancing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000FC36B File Offset: 0x000FA56B
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000FC374 File Offset: 0x000FA574
		public virtual void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			Color value;
			if (drawData.overrideColor != null)
			{
				value = drawData.overrideColor.Value;
			}
			else
			{
				float alpha = drawData.alpha;
				if (alpha <= 0f)
				{
					if (drawData.propertyBlock != null)
					{
						batch.ReturnPropertyBlock(drawData.propertyBlock);
					}
					return;
				}
				value = base.Color * drawData.color;
				value.a *= alpha;
			}
			Vector3 scale = drawData.scale;
			scale.x *= this.data.drawSize.x;
			scale.z *= this.data.drawSize.y;
			Mesh mesh = MeshPool.plane10;
			float num = drawData.rotation;
			if (scale.x < 0f && scale.y >= 0f)
			{
				scale.x = -scale.x;
				mesh = MeshPool.plane10Flip;
			}
			else if (scale.x >= 0f && scale.y < 0f)
			{
				scale.y = -scale.y;
				mesh = MeshPool.plane10Flip;
				num += 180f;
			}
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawData.pos, Quaternion.AngleAxis(num, Vector3.up), scale);
			Material matSingle = this.MatSingle;
			batch.DrawMesh(mesh, matrix, matSingle, drawData.drawLayer, new Color?(value), this.data.renderInstanced && this.AllowInstancing, drawData.propertyBlock);
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000FC4F0 File Offset: 0x000FA6F0
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Fleck(path=";
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
