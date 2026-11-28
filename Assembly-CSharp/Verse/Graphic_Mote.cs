using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000571 RID: 1393
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x000FD856 File Offset: 0x000FBA56
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000FD859 File Offset: 0x000FBA59
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000FD868 File Offset: 0x000FBA68
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Graphic_Mote.DrawMote(this.data, this.MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000FD89C File Offset: 0x000FBA9C
		public static void DrawMote(GraphicData data, Material material, Color color, Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer, bool forcePropertyBlock = false, MaterialPropertyBlock overridePropertyBlock = null)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Color color2 = color * mote.instanceColor;
			color2.a *= alpha;
			Vector3 exactScale = mote.ExactScale;
			exactScale.x *= data.drawSize.x;
			exactScale.z *= data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
			if (!forcePropertyBlock && color2.IndistinguishableFrom(material.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color2);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, overridePropertyBlock ?? Graphic_Mote.propertyBlock);
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000FD98C File Offset: 0x000FBB8C
		public static void DrawMote(GraphicData data, Material material, Vector3 loc, float rotation, int layer, bool forcePropertyBlock = false, MaterialPropertyBlock overridePropertyBlock = null)
		{
			Color color = data.color;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.AngleAxis(rotation, Vector3.up), new Vector3(data.drawSize.x, 1f, data.drawSize.y));
			if (!forcePropertyBlock && color.IndistinguishableFrom(material.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, overridePropertyBlock ?? Graphic_Mote.propertyBlock);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000FDA2C File Offset: 0x000FBC2C
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Mote(path=";
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

		// Token: 0x04001CD0 RID: 7376
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
