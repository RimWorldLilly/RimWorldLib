using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000505 RID: 1285
	public static class MeshMakerShadows
	{
		// Token: 0x06002614 RID: 9748 RVA: 0x000E7F78 File Offset: 0x000E6178
		public static Mesh NewShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			Color32 item = new Color32(byte.MaxValue, 0, 0, (byte)(255f * tallness));
			float num = baseWidth / 2f;
			float num2 = baseHeight / 2f;
			MeshMakerShadows.vertsList.Clear();
			MeshMakerShadows.colorsList.Clear();
			MeshMakerShadows.trianglesList.Clear();
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, num2));
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, num2));
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(1);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(3);
			int count = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(count);
			MeshMakerShadows.trianglesList.Add(count + 1);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(count + 1);
			MeshMakerShadows.trianglesList.Add(1);
			int count2 = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(count2);
			MeshMakerShadows.trianglesList.Add(count2 + 1);
			MeshMakerShadows.trianglesList.Add(count2 + 1);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(2);
			int count3 = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(count3);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(count3 + 1);
			MeshMakerShadows.trianglesList.Add(count3);
			return new Mesh
			{
				name = "NewShadowMesh()",
				vertices = MeshMakerShadows.vertsList.ToArray(),
				colors32 = MeshMakerShadows.colorsList.ToArray(),
				triangles = MeshMakerShadows.trianglesList.ToArray(),
				bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 1000f))
			};
		}

		// Token: 0x04001B69 RID: 7017
		private static List<Vector3> vertsList = new List<Vector3>();

		// Token: 0x04001B6A RID: 7018
		private static List<Color32> colorsList = new List<Color32>();

		// Token: 0x04001B6B RID: 7019
		private static List<int> trianglesList = new List<int>();

		// Token: 0x04001B6C RID: 7020
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);
	}
}
