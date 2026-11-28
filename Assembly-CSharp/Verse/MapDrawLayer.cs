using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A8 RID: 680
	public abstract class MapDrawLayer
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x00078B28 File Offset: 0x00076D28
		protected Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x00078B30 File Offset: 0x00076D30
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x00078B33 File Offset: 0x00076D33
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x00078B3B File Offset: 0x00076D3B
		public bool Dirty { get; set; }

		// Token: 0x060014E1 RID: 5345 RVA: 0x00078B44 File Offset: 0x00076D44
		public MapDrawLayer(Map map)
		{
			this.map = map;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00078B60 File Offset: 0x00076D60
		public static LayerSubMesh CreateFreeSubMesh(Material material, Map map = null)
		{
			if (material == null)
			{
				return null;
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "SectionLayerSubMesh_Free" + ((map == null) ? "" : ("_" + map.Tile.ToString()));
			}
			return new LayerSubMesh(mesh, material, null);
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00078BD0 File Offset: 0x00076DD0
		public LayerSubMesh GetSubMesh(Material material)
		{
			if (material == null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].material == material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "SectionLayerSubMesh_" + base.GetType().Name + "_" + this.Map.Tile.ToString();
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, null);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00078C89 File Offset: 0x00076E89
		public virtual CellRect GetBoundaryRect()
		{
			return this.map.BoundsRect(0);
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00078C98 File Offset: 0x00076E98
		public virtual void RefreshSubMeshBounds()
		{
			Bounds bounds = this.GetBoundaryRect().ExpandedBy(2).ToBounds();
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].mesh.bounds = bounds;
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00078CEC File Offset: 0x00076EEC
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00078D3C File Offset: 0x00076F3C
		public virtual void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, layerSubMesh.renderLayer);
				}
			}
		}

		// Token: 0x060014E8 RID: 5352
		public abstract void Regenerate();

		// Token: 0x060014E9 RID: 5353 RVA: 0x00078DA4 File Offset: 0x00076FA4
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00078DF8 File Offset: 0x00076FF8
		public void Dispose()
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				Object.Destroy(layerSubMesh.mesh);
			}
			this.subMeshes.Clear();
		}

		// Token: 0x04001265 RID: 4709
		private Map map;

		// Token: 0x04001266 RID: 4710
		public ulong relevantChangeTypes;

		// Token: 0x04001267 RID: 4711
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
