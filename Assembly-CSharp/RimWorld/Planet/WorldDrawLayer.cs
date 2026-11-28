using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002314 RID: 8980
	public abstract class WorldDrawLayer : WorldDrawLayerBase
	{
		// Token: 0x170026CE RID: 9934
		// (get) Token: 0x0600DDCB RID: 56779 RVA: 0x0052900F File Offset: 0x0052720F
		public override bool Visible
		{
			get
			{
				return base.Visible && (PlanetLayer.Selected == this.planetLayer || this.VisibleWhenLayerNotSelected);
			}
		}

		// Token: 0x170026CF RID: 9935
		// (get) Token: 0x0600DDCC RID: 56780 RVA: 0x00529030 File Offset: 0x00527230
		public override Vector3 Position
		{
			get
			{
				if (this.CameraIsOrigin)
				{
					return Find.WorldCameraDriver.CameraPosition;
				}
				return this.planetLayer.Origin;
			}
		}

		// Token: 0x170026D0 RID: 9936
		// (get) Token: 0x0600DDCD RID: 56781 RVA: 0x00529050 File Offset: 0x00527250
		public bool Raycastable
		{
			get
			{
				return !this.meshCollidersInOrder.NullOrEmpty<MeshCollider>() && PlanetLayer.Selected == this.planetLayer;
			}
		}

		// Token: 0x0600DDCE RID: 56782 RVA: 0x0052906E File Offset: 0x0052726E
		public void Initialize(PlanetLayer layer)
		{
			this.planetLayer = layer;
		}

		// Token: 0x0600DDCF RID: 56783 RVA: 0x00529077 File Offset: 0x00527277
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer.<Regenerate>d__10 <Regenerate>d__ = new WorldDrawLayer.<Regenerate>d__10(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DDD0 RID: 56784 RVA: 0x00529088 File Offset: 0x00527288
		protected void AppendRaycastableTriangle(int subMeshIndex, int tileIndex)
		{
			if (this.triangleIndexToTileID == null)
			{
				this.triangleIndexToTileID = new List<List<int>>();
			}
			while (this.triangleIndexToTileID.Count < this.subMeshes.Count)
			{
				this.triangleIndexToTileID.Add(new List<int>());
			}
			this.triangleIndexToTileID[subMeshIndex].Add(tileIndex);
		}

		// Token: 0x0600DDD1 RID: 56785 RVA: 0x005290E4 File Offset: 0x005272E4
		public bool TryGetTileFromRayHit(RaycastHit hit, out PlanetTile id)
		{
			id = PlanetTile.Invalid;
			for (int i = 0; i < this.meshCollidersInOrder.Count; i++)
			{
				if (this.meshCollidersInOrder[i] == hit.collider)
				{
					id = new PlanetTile(this.triangleIndexToTileID[i][hit.triangleIndex], this.planetLayer);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DDD2 RID: 56786 RVA: 0x00529158 File Offset: 0x00527358
		protected IEnumerable RegenerateWorldMeshColliders()
		{
			WorldDrawLayer.<RegenerateWorldMeshColliders>d__13 <RegenerateWorldMeshColliders>d__ = new WorldDrawLayer.<RegenerateWorldMeshColliders>d__13(-2);
			<RegenerateWorldMeshColliders>d__.<>4__this = this;
			return <RegenerateWorldMeshColliders>d__;
		}

		// Token: 0x04008E6D RID: 36461
		public PlanetLayer planetLayer;

		// Token: 0x04008E6E RID: 36462
		private List<MeshCollider> meshCollidersInOrder;

		// Token: 0x04008E6F RID: 36463
		private List<List<int>> triangleIndexToTileID;
	}
}
