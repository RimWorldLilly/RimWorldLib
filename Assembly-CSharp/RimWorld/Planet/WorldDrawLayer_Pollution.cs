using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200231E RID: 8990
	public class WorldDrawLayer_Pollution : WorldDrawLayer
	{
		// Token: 0x170026E5 RID: 9957
		// (get) Token: 0x0600DE06 RID: 56838 RVA: 0x00529CA0 File Offset: 0x00527EA0
		private Material LightPollution
		{
			get
			{
				if (this.lightPollution == null)
				{
					this.lightPollution = MaterialPool.MatFrom("World/Pollution/Light", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.lightPollution;
			}
		}

		// Token: 0x170026E6 RID: 9958
		// (get) Token: 0x0600DE07 RID: 56839 RVA: 0x00529CD0 File Offset: 0x00527ED0
		private Material ModeratePollution
		{
			get
			{
				if (this.moderatePollution == null)
				{
					this.moderatePollution = MaterialPool.MatFrom("World/Pollution/Moderate", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.moderatePollution;
			}
		}

		// Token: 0x170026E7 RID: 9959
		// (get) Token: 0x0600DE08 RID: 56840 RVA: 0x00529D00 File Offset: 0x00527F00
		private Material ExtremePollution
		{
			get
			{
				if (this.extemePollution == null)
				{
					this.extemePollution = MaterialPool.MatFrom("World/Pollution/Extreme", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.extemePollution;
			}
		}

		// Token: 0x0600DE09 RID: 56841 RVA: 0x00529D30 File Offset: 0x00527F30
		private int GetRegionIdForTile(PlanetTile tile)
		{
			return Mathf.FloorToInt((float)tile.tileId / 500f);
		}

		// Token: 0x0600DE0A RID: 56842 RVA: 0x00529D44 File Offset: 0x00527F44
		public List<LayerSubMesh> GetSubMeshesForRegion(int regionId)
		{
			if (!this.subMeshesByRegion.ContainsKey(regionId))
			{
				this.subMeshesByRegion[regionId] = new List<LayerSubMesh>();
			}
			return this.subMeshesByRegion[regionId];
		}

		// Token: 0x0600DE0B RID: 56843 RVA: 0x00529D74 File Offset: 0x00527F74
		public LayerSubMesh GetSubMeshForMaterialAndRegion(Material material, int regionId)
		{
			List<LayerSubMesh> subMeshesForRegion = this.GetSubMeshesForRegion(regionId);
			for (int i = 0; i < subMeshesForRegion.Count; i++)
			{
				if (subMeshesForRegion[i].material == material)
				{
					return subMeshesForRegion[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, null);
			subMeshesForRegion.Add(layerSubMesh);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x0600DE0C RID: 56844 RVA: 0x00529E18 File Offset: 0x00528018
		private void RegenerateRegion(int regionId)
		{
			List<LayerSubMesh> subMeshesForRegion = this.GetSubMeshesForRegion(regionId);
			for (int i = 0; i < subMeshesForRegion.Count; i++)
			{
				subMeshesForRegion[i].Clear(MeshParts.All);
			}
			int num = regionId * 500;
			int num2 = num + 500;
			for (int j = num; j < num2; j++)
			{
				PlanetTile tile = new PlanetTile(j, this.planetLayer);
				if (!Find.World.grid.InBounds(tile))
				{
					break;
				}
				this.TryAddMeshForTile(tile);
			}
			for (int k = 0; k < subMeshesForRegion.Count; k++)
			{
				if (subMeshesForRegion[k].verts.Count > 0)
				{
					subMeshesForRegion[k].FinalizeMesh(MeshParts.All);
				}
			}
		}

		// Token: 0x0600DE0D RID: 56845 RVA: 0x00529EC9 File Offset: 0x005280C9
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Pollution.<Regenerate>d__20 <Regenerate>d__ = new WorldDrawLayer_Pollution.<Regenerate>d__20(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DE0E RID: 56846 RVA: 0x00529EDC File Offset: 0x005280DC
		private bool TryAddMeshForTile(PlanetTile tile)
		{
			PollutionLevel pollution = tile.Tile.PollutionLevel();
			Material materialForTilePollution = this.GetMaterialForTilePollution(pollution);
			if (materialForTilePollution == null)
			{
				return false;
			}
			int regionIdForTile = this.GetRegionIdForTile(tile);
			LayerSubMesh subMeshForMaterialAndRegion = this.GetSubMeshForMaterialAndRegion(materialForTilePollution, regionIdForTile);
			Find.WorldGrid.GetTileVertices(tile, this.verts);
			Find.WorldGrid.GetTileNeighbors(tile, this.tmpNeighbors);
			int count = subMeshForMaterialAndRegion.verts.Count;
			this.tmpBordersUnpollutedVerts.Clear();
			this.tmpVerts.Clear();
			for (int i = 0; i < this.tmpNeighbors.Count; i++)
			{
				if (this.planetLayer[this.tmpNeighbors[i]].PollutionLevel() < PollutionLevel.Moderate)
				{
					Vector3 center = Find.WorldGrid.GetTileCenter(this.tmpNeighbors[i]);
					this.tmpVerts.AddRange(this.verts);
					this.tmpVerts.SortBy((Vector3 v) => Vector2.Distance(center, v));
					for (int j = 0; j < 2; j++)
					{
						this.tmpBordersUnpollutedVerts.Add(this.tmpVerts[j]);
					}
				}
			}
			int k = 0;
			int count2 = this.verts.Count;
			while (k < count2)
			{
				Vector3 vector = this.verts[k] + this.verts[k].normalized * 0.02f;
				subMeshForMaterialAndRegion.verts.Add(vector);
				subMeshForMaterialAndRegion.uvs.Add(vector * 0.1f);
				Color c = this.tmpBordersUnpollutedVerts.Contains(this.verts[k]) ? WorldDrawLayer_Pollution.BordersUnpollutedTileColor : WorldDrawLayer_Pollution.DefaultTileColor;
				subMeshForMaterialAndRegion.colors.Add(c);
				if (k < count2 - 2)
				{
					subMeshForMaterialAndRegion.tris.Add(count + k + 2);
					subMeshForMaterialAndRegion.tris.Add(count + k + 1);
					subMeshForMaterialAndRegion.tris.Add(count);
				}
				k++;
			}
			this.tmpBordersUnpollutedVerts.Clear();
			this.tmpVerts.Clear();
			return true;
		}

		// Token: 0x0600DE0F RID: 56847 RVA: 0x0052A11C File Offset: 0x0052831C
		private Material GetMaterialForTilePollution(PollutionLevel pollution)
		{
			switch (pollution)
			{
			case PollutionLevel.Light:
				return this.LightPollution;
			case PollutionLevel.Moderate:
				return this.ModeratePollution;
			case PollutionLevel.Extreme:
				return this.ExtremePollution;
			default:
				return null;
			}
		}

		// Token: 0x0600DE10 RID: 56848 RVA: 0x0052A14C File Offset: 0x0052834C
		public void Notify_TilePollutionChanged(PlanetTile tileId)
		{
			int regionIdForTile = this.GetRegionIdForTile(tileId);
			if (!this.regionsToRegenerate.Contains(regionIdForTile))
			{
				this.regionsToRegenerate.Enqueue(regionIdForTile);
			}
			Find.WorldGrid.GetTileNeighbors(tileId, WorldDrawLayer_Pollution.TmpChangedNeighbours);
			for (int i = 0; i < WorldDrawLayer_Pollution.TmpChangedNeighbours.Count; i++)
			{
				int regionIdForTile2 = this.GetRegionIdForTile(WorldDrawLayer_Pollution.TmpChangedNeighbours[i]);
				if (!this.regionsToRegenerate.Contains(regionIdForTile2))
				{
					this.regionsToRegenerate.Enqueue(regionIdForTile2);
				}
			}
			WorldDrawLayer_Pollution.TmpChangedNeighbours.Clear();
		}

		// Token: 0x0600DE11 RID: 56849 RVA: 0x0052A1D8 File Offset: 0x005283D8
		public override void Render()
		{
			if (this.regionsToRegenerate.Count > 0)
			{
				int regionId = this.regionsToRegenerate.Dequeue();
				this.RegenerateRegion(regionId);
			}
			base.Render();
		}

		// Token: 0x04008E8B RID: 36491
		private const int TilesPerSubMesh = 500;

		// Token: 0x04008E8C RID: 36492
		private const float ScaleUVFactor = 0.1f;

		// Token: 0x04008E8D RID: 36493
		private static readonly Color DefaultTileColor = Color.white;

		// Token: 0x04008E8E RID: 36494
		private static readonly Color BordersUnpollutedTileColor = new Color(1f, 1f, 1f, 0.4f);

		// Token: 0x04008E8F RID: 36495
		private readonly List<Vector3> verts = new List<Vector3>();

		// Token: 0x04008E90 RID: 36496
		private readonly Dictionary<int, List<LayerSubMesh>> subMeshesByRegion = new Dictionary<int, List<LayerSubMesh>>();

		// Token: 0x04008E91 RID: 36497
		private readonly Queue<int> regionsToRegenerate = new Queue<int>();

		// Token: 0x04008E92 RID: 36498
		private Material lightPollution;

		// Token: 0x04008E93 RID: 36499
		private Material moderatePollution;

		// Token: 0x04008E94 RID: 36500
		private Material extemePollution;

		// Token: 0x04008E95 RID: 36501
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x04008E96 RID: 36502
		private readonly HashSet<Vector3> tmpBordersUnpollutedVerts = new HashSet<Vector3>();

		// Token: 0x04008E97 RID: 36503
		private readonly List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04008E98 RID: 36504
		private static readonly List<PlanetTile> TmpChangedNeighbours = new List<PlanetTile>();
	}
}
