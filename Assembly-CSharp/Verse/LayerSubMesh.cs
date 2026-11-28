using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public class LayerSubMesh
	{
		// Token: 0x060014C8 RID: 5320 RVA: 0x00077FA8 File Offset: 0x000761A8
		public LayerSubMesh(Mesh mesh, Material material, Bounds? bounds = null)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00078018 File Offset: 0x00076218
		public void Clear(MeshParts parts)
		{
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				this.verts.Clear();
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				this.tris.Clear();
			}
			if ((parts & MeshParts.Colors) != MeshParts.None)
			{
				this.colors.Clear();
			}
			if ((parts & MeshParts.UVs1) != MeshParts.None)
			{
				this.uvs.Clear();
			}
			if ((parts & MeshParts.UVs2) != MeshParts.None)
			{
				this.uvsChannelTwo.Clear();
			}
			if ((parts & MeshParts.Normals) != MeshParts.None)
			{
				this.normals.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00078090 File Offset: 0x00076290
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?");
			}
			if ((parts & MeshParts.Verts) != MeshParts.None || (parts & MeshParts.Tris) != MeshParts.None)
			{
				this.mesh.Clear();
			}
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(96, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot cook Verts for ");
					defaultInterpolatedStringHandler.AppendFormatted<Material>(this.material);
					defaultInterpolatedStringHandler.AppendLiteral(": no ingredients data. If you want to not render this submesh, disable it.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Cannot cook Tris for ");
					defaultInterpolatedStringHandler2.AppendFormatted<Material>(this.material);
					defaultInterpolatedStringHandler2.AppendLiteral(": no ingredients data.");
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			if ((parts & MeshParts.Colors) != MeshParts.None && this.colors.Count > 0)
			{
				this.mesh.SetColors(this.colors);
			}
			if ((parts & MeshParts.UVs1) != MeshParts.None && this.uvs.Count > 0)
			{
				this.mesh.SetUVs(0, this.uvs);
			}
			if ((parts & MeshParts.UVs2) != MeshParts.None && this.uvsChannelTwo.Count > 0)
			{
				this.mesh.SetUVs(1, this.uvsChannelTwo);
			}
			if ((parts & MeshParts.Normals) != MeshParts.None && this.normals.Count > 0)
			{
				this.mesh.SetNormals(this.normals);
			}
			this.finalized = true;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00078224 File Offset: 0x00076424
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("LayerSubMesh(");
			defaultInterpolatedStringHandler.AppendFormatted<Material>(this.material);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001255 RID: 4693
		public bool finalized;

		// Token: 0x04001256 RID: 4694
		public bool disabled;

		// Token: 0x04001257 RID: 4695
		public Material material;

		// Token: 0x04001258 RID: 4696
		public int renderLayer;

		// Token: 0x04001259 RID: 4697
		public Mesh mesh;

		// Token: 0x0400125A RID: 4698
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x0400125B RID: 4699
		public List<int> tris = new List<int>();

		// Token: 0x0400125C RID: 4700
		public List<Color32> colors = new List<Color32>();

		// Token: 0x0400125D RID: 4701
		public List<Vector3> pollution = new List<Vector3>();

		// Token: 0x0400125E RID: 4702
		public List<Vector3> uvs = new List<Vector3>();

		// Token: 0x0400125F RID: 4703
		public List<Vector3> uvsChannelTwo = new List<Vector3>();

		// Token: 0x04001260 RID: 4704
		public List<Vector3> normals = new List<Vector3>();
	}
}
