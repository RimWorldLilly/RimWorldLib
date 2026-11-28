using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020013A4 RID: 5028
	public static class LightningBoltMeshMaker
	{
		// Token: 0x06007CB2 RID: 31922 RVA: 0x002C8F14 File Offset: 0x002C7114
		public static Mesh NewBoltMesh()
		{
			LightningBoltMeshMaker.lightningTop = new Vector2(Rand.Range(-50f, 50f), 200f);
			LightningBoltMeshMaker.MakeVerticesBase();
			LightningBoltMeshMaker.PeturbVerticesRandomly();
			LightningBoltMeshMaker.DoubleVertices();
			return LightningBoltMeshMaker.MeshFromVerts();
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x002C8F48 File Offset: 0x002C7148
		private static void MakeVerticesBase()
		{
			int num = (int)Math.Ceiling((double)((Vector2.zero - LightningBoltMeshMaker.lightningTop).magnitude / 0.25f));
			Vector2 b = LightningBoltMeshMaker.lightningTop / (float)num;
			LightningBoltMeshMaker.verts2D = new List<Vector2>();
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < num; i++)
			{
				LightningBoltMeshMaker.verts2D.Add(vector);
				vector += b;
			}
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x002C8FBC File Offset: 0x002C71BC
		private static void PeturbVerticesRandomly()
		{
			Perlin perlin = new Perlin(0.007000000216066837, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			List<Vector2> list = LightningBoltMeshMaker.verts2D.ListFullCopy<Vector2>();
			LightningBoltMeshMaker.verts2D.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				float d = 12f * (float)perlin.GetValue((double)i, 0.0, 0.0);
				Vector2 item = list[i] + d * Vector2.right;
				LightningBoltMeshMaker.verts2D.Add(item);
			}
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x002C9064 File Offset: 0x002C7264
		private static void DoubleVertices()
		{
			List<Vector2> list = LightningBoltMeshMaker.verts2D.ListFullCopy<Vector2>();
			Vector3 vector = default(Vector3);
			Vector2 a = default(Vector2);
			LightningBoltMeshMaker.verts2D.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (i <= list.Count - 2)
				{
					vector = Quaternion.AngleAxis(90f, Vector3.up) * (list[i] - list[i + 1]);
					a = new Vector2(vector.y, vector.z);
					a.Normalize();
				}
				Vector2 item = list[i] - 1f * a;
				Vector2 item2 = list[i] + 1f * a;
				LightningBoltMeshMaker.verts2D.Add(item);
				LightningBoltMeshMaker.verts2D.Add(item2);
			}
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x002C914C File Offset: 0x002C734C
		private static Mesh MeshFromVerts()
		{
			Vector3[] array = new Vector3[LightningBoltMeshMaker.verts2D.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector3(LightningBoltMeshMaker.verts2D[i].x, 0f, LightningBoltMeshMaker.verts2D[i].y);
			}
			float num = 0f;
			Vector2[] array2 = new Vector2[LightningBoltMeshMaker.verts2D.Count];
			for (int j = 0; j < LightningBoltMeshMaker.verts2D.Count; j += 2)
			{
				array2[j] = new Vector2(0f, num);
				array2[j + 1] = new Vector2(1f, num);
				num += 0.04f;
			}
			int[] array3 = new int[LightningBoltMeshMaker.verts2D.Count * 3];
			for (int k = 0; k < LightningBoltMeshMaker.verts2D.Count - 2; k += 2)
			{
				int num2 = k * 3;
				array3[num2] = k;
				array3[num2 + 1] = k + 1;
				array3[num2 + 2] = k + 2;
				array3[num2 + 3] = k + 2;
				array3[num2 + 4] = k + 1;
				array3[num2 + 5] = k + 3;
			}
			return new Mesh
			{
				vertices = array,
				uv = array2,
				triangles = array3,
				name = "MeshFromVerts()"
			};
		}

		// Token: 0x04004E7F RID: 20095
		private static List<Vector2> verts2D;

		// Token: 0x04004E80 RID: 20096
		private static Vector2 lightningTop;

		// Token: 0x04004E81 RID: 20097
		private const float LightningHeight = 200f;

		// Token: 0x04004E82 RID: 20098
		private const float LightningRootXVar = 50f;

		// Token: 0x04004E83 RID: 20099
		private const float VertexInterval = 0.25f;

		// Token: 0x04004E84 RID: 20100
		private const float MeshWidth = 2f;

		// Token: 0x04004E85 RID: 20101
		private const float UVIntervalY = 0.04f;

		// Token: 0x04004E86 RID: 20102
		private const float PerturbAmp = 12f;

		// Token: 0x04004E87 RID: 20103
		private const float PerturbFreq = 0.007f;
	}
}
