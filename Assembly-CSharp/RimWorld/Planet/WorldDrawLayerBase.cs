using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002312 RID: 8978
	public abstract class WorldDrawLayerBase
	{
		// Token: 0x170026C3 RID: 9923
		// (get) Token: 0x0600DDB2 RID: 56754 RVA: 0x00528A83 File Offset: 0x00526C83
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty && this.Visible;
			}
		}

		// Token: 0x170026C4 RID: 9924
		// (get) Token: 0x0600DDB3 RID: 56755 RVA: 0x00528A95 File Offset: 0x00526C95
		protected virtual int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170026C5 RID: 9925
		// (get) Token: 0x0600DDB4 RID: 56756 RVA: 0x00528A9C File Offset: 0x00526C9C
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170026C6 RID: 9926
		// (get) Token: 0x0600DDB5 RID: 56757 RVA: 0x00528AA3 File Offset: 0x00526CA3
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170026C7 RID: 9927
		// (get) Token: 0x0600DDB6 RID: 56758 RVA: 0x00528AAA File Offset: 0x00526CAA
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170026C8 RID: 9928
		// (get) Token: 0x0600DDB7 RID: 56759 RVA: 0x00528AB2 File Offset: 0x00526CB2
		public virtual bool Visible
		{
			get
			{
				return !WorldRendererUtility.WorldBackgroundNow || this.VisibleInBackground;
			}
		}

		// Token: 0x170026C9 RID: 9929
		// (get) Token: 0x0600DDB8 RID: 56760 RVA: 0x00528AC3 File Offset: 0x00526CC3
		public virtual bool VisibleWhenLayerNotSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170026CA RID: 9930
		// (get) Token: 0x0600DDB9 RID: 56761 RVA: 0x00528AC6 File Offset: 0x00526CC6
		public virtual bool VisibleInBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170026CB RID: 9931
		// (get) Token: 0x0600DDBA RID: 56762 RVA: 0x00528AC9 File Offset: 0x00526CC9
		public virtual Vector3 Position
		{
			get
			{
				if (this.CameraIsOrigin)
				{
					return Find.WorldCameraDriver.CameraPosition;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x170026CC RID: 9932
		// (get) Token: 0x0600DDBB RID: 56763 RVA: 0x00528AE3 File Offset: 0x00526CE3
		public virtual bool CameraIsOrigin
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026CD RID: 9933
		// (get) Token: 0x0600DDBC RID: 56764 RVA: 0x00528AE8 File Offset: 0x00526CE8
		public MaterialPropertyBlock MatPropBlock
		{
			get
			{
				MaterialPropertyBlock result;
				if ((result = this.propertyBlock) == null)
				{
					result = (this.propertyBlock = new MaterialPropertyBlock());
				}
				return result;
			}
		}

		// Token: 0x0600DDBD RID: 56765 RVA: 0x00528B10 File Offset: 0x00526D10
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x0600DDBE RID: 56766 RVA: 0x00528B28 File Offset: 0x00526D28
		protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
				{
					subMeshIndex = i;
					return layerSubMesh;
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material, null);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		// Token: 0x0600DDBF RID: 56767 RVA: 0x00528BE8 File Offset: 0x00526DE8
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

		// Token: 0x0600DDC0 RID: 56768 RVA: 0x00528C36 File Offset: 0x00526E36
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x0600DDC1 RID: 56769 RVA: 0x00528C4C File Offset: 0x00526E4C
		public virtual void Render()
		{
			if (!this.Visible)
			{
				return;
			}
			if (this.ShouldRegenerate)
			{
				this.RegenerateNow();
			}
			int renderLayer = this.RenderLayer;
			Vector3 position = this.Position;
			Quaternion rotation = this.Rotation;
			float alpha = this.Alpha;
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].finalized)
				{
					if (!Mathf.Approximately(alpha, 1f))
					{
						Color color = this.subMeshes[i].material.color;
						this.MatPropBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
						Graphics.DrawMesh(this.subMeshes[i].mesh, position, rotation, this.subMeshes[i].material, renderLayer, null, 0, this.MatPropBlock);
					}
					else
					{
						Graphics.DrawMesh(this.subMeshes[i].mesh, position, rotation, this.subMeshes[i].material, renderLayer);
					}
				}
			}
		}

		// Token: 0x0600DDC2 RID: 56770 RVA: 0x00528D81 File Offset: 0x00526F81
		public virtual IEnumerable Regenerate()
		{
			WorldDrawLayerBase.<Regenerate>d__31 <Regenerate>d__ = new WorldDrawLayerBase.<Regenerate>d__31(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DDC3 RID: 56771 RVA: 0x00528D91 File Offset: 0x00526F91
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600DDC4 RID: 56772 RVA: 0x00528D9C File Offset: 0x00526F9C
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x0600DDC5 RID: 56773 RVA: 0x00528DD4 File Offset: 0x00526FD4
		public void Dispose()
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				Object.Destroy(layerSubMesh.mesh);
			}
			this.subMeshes.Clear();
		}

		// Token: 0x04008E67 RID: 36455
		protected bool dirty = true;

		// Token: 0x04008E68 RID: 36456
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x04008E69 RID: 36457
		protected readonly List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04008E6A RID: 36458
		private const int MaxVerticesPerMesh = 40000;
	}
}
