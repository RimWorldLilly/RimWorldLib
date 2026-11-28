using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C2 RID: 706
	public static class Printer_Plane
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x00080560 File Offset: 0x0007E760
		public static void GetUVs(Rect rect, out Vector2 uv1, out Vector2 uv2, out Vector2 uv3, out Vector2 uv4, bool flipUv)
		{
			if (flipUv)
			{
				uv1 = new Vector2(rect.xMax, rect.yMin);
				uv2 = new Vector2(rect.xMax, rect.yMax);
				uv3 = new Vector2(rect.xMin, rect.yMax);
				uv4 = new Vector2(rect.xMin, rect.yMin);
				return;
			}
			uv1 = new Vector2(rect.xMin, rect.yMin);
			uv2 = new Vector2(rect.xMin, rect.yMax);
			uv3 = new Vector2(rect.xMax, rect.yMax);
			uv4 = new Vector2(rect.xMax, rect.yMin);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0008063C File Offset: 0x0007E83C
		public static void PrintPlane(MapDrawLayer layer, Vector3 center, Vector2 size, Material mat, float rot = 0f, bool flipUv = false, Vector2[] uvs = null, Color32[] colors = null, float topVerticesAltitudeBias = 0.01f, float uvzPayload = 0f)
		{
			if (colors == null)
			{
				colors = Printer_Plane.DefaultColors;
			}
			if (uvs == null)
			{
				uvs = ((!flipUv) ? Printer_Plane.DefaultUvs : Printer_Plane.DefaultUvsFlipped);
			}
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(-0.5f * size.x, 0f, -0.5f * size.y));
			subMesh.verts.Add(new Vector3(-0.5f * size.x, topVerticesAltitudeBias, 0.5f * size.y));
			subMesh.verts.Add(new Vector3(0.5f * size.x, topVerticesAltitudeBias, 0.5f * size.y));
			subMesh.verts.Add(new Vector3(0.5f * size.x, 0f, -0.5f * size.y));
			if (rot != 0f)
			{
				float num = rot * 0.017453292f;
				num *= -1f;
				for (int i = 0; i < 4; i++)
				{
					float x = subMesh.verts[count + i].x;
					float z = subMesh.verts[count + i].z;
					float num2 = Mathf.Cos(num);
					float num3 = Mathf.Sin(num);
					float x2 = x * num2 - z * num3;
					float z2 = x * num3 + z * num2;
					subMesh.verts[count + i] = new Vector3(x2, subMesh.verts[count + i].y, z2);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				List<Vector3> verts = subMesh.verts;
				int index = count + j;
				verts[index] += center;
				subMesh.uvs.Add(new Vector3(uvs[j].x, uvs[j].y, uvzPayload));
				subMesh.colors.Add(colors[j]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x040012B9 RID: 4793
		private static readonly Color32[] DefaultColors = new Color32[]
		{
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		};

		// Token: 0x040012BA RID: 4794
		private static readonly Vector2[] DefaultUvs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040012BB RID: 4795
		private static readonly Vector2[] DefaultUvsFlipped = new Vector2[]
		{
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f)
		};
	}
}
