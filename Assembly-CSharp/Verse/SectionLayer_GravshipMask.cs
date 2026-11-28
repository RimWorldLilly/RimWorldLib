using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B6 RID: 694
	public class SectionLayer_GravshipMask : SectionLayer
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0007CFDB File Offset: 0x0007B1DB
		// (set) Token: 0x06001538 RID: 5432 RVA: 0x0007CFE2 File Offset: 0x0007B1E2
		public static Building_GravEngine Engine { get; set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x0007CFEA File Offset: 0x0007B1EA
		// (set) Token: 0x0600153A RID: 5434 RVA: 0x0007CFF1 File Offset: 0x0007B1F1
		public static SectionLayer_GravshipMask.MaskOverrideMode OverrideMode { get; set; }

		// Token: 0x0600153B RID: 5435 RVA: 0x0007CFF9 File Offset: 0x0007B1F9
		public SectionLayer_GravshipMask(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.None;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0007D012 File Offset: 0x0007B212
		public static void ResetStaticData()
		{
			SectionLayer_GravshipMask.Engine = null;
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.None;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0007D020 File Offset: 0x0007B220
		public static LayerSubMesh BakeGravshipShadowMask(Map map, Material mat, IEnumerable<IntVec3> cells)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat, null);
			SectionLayer_GravshipMask.RegenerateGravshipShadowMask(map, layerSubMesh, cells);
			if (layerSubMesh.verts.Count > 0)
			{
				layerSubMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
			return layerSubMesh;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0007D053 File Offset: 0x0007B253
		public static bool IsValidSubstructure(IntVec3 c)
		{
			return SectionLayer_GravshipMask.Engine != null && SectionLayer_GravshipMask.Engine.Spawned && SectionLayer_GravshipMask.Engine.ValidSubstructureNoRegen != null && SectionLayer_GravshipMask.Engine.ValidSubstructureNoRegen.Contains(c);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0007D088 File Offset: 0x0007B288
		public static LayerSubMesh BakeDummyShadowMask(Material mat, Vector3 origin, float width, float height)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat, null);
			Vector3 vector = new Vector3(width * 0.5f, 0f, height * 0.5f);
			layerSubMesh.verts.Add(origin - vector);
			layerSubMesh.verts.Add(origin + new Vector3(-vector.x, 0f, vector.z));
			layerSubMesh.verts.Add(origin + vector);
			layerSubMesh.verts.Add(origin + new Vector3(vector.x, 0f, -vector.z));
			int count = layerSubMesh.verts.Count;
			layerSubMesh.tris.Add(count - 4);
			layerSubMesh.tris.Add(count - 3);
			layerSubMesh.tris.Add(count - 2);
			layerSubMesh.tris.Add(count - 4);
			layerSubMesh.tris.Add(count - 2);
			layerSubMesh.tris.Add(count - 1);
			layerSubMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			return layerSubMesh;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0007D194 File Offset: 0x0007B394
		private void RegenerateGravshipMask(LayerSubMesh subMesh)
		{
			Building_GravEngine engine = SectionLayer_GravshipMask.Engine;
			if (engine == null || !engine.Spawned)
			{
				return;
			}
			HashSet<IntVec3> validSubstructure = SectionLayer_GravshipMask.Engine.ValidSubstructure;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				if (validSubstructure.Contains(intVec))
				{
					SectionLayer_IndoorMask.AppendQuadToMesh(subMesh, (float)intVec.x, (float)intVec.z, 0f);
				}
			}
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0007D22C File Offset: 0x0007B42C
		private static void RegenerateGravshipShadowMask(Map map, LayerSubMesh subMesh, IEnumerable<IntVec3> cells)
		{
			string defName = TerrainDefOf.Space.defName;
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.InBounds(map) && map.terrainGrid.TerrainAt(intVec).defName != defName)
				{
					SectionLayer_IndoorMask.AppendQuadToMesh(subMesh, (float)intVec.x, (float)intVec.z, 0f);
				}
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0007D2B4 File Offset: 0x0007B4B4
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.GravshipMask);
			subMesh.Clear(MeshParts.All);
			switch (SectionLayer_GravshipMask.OverrideMode)
			{
			case SectionLayer_GravshipMask.MaskOverrideMode.None:
				if (WorldComponent_GravshipController.CutsceneInProgress || Find.GravshipController.IsGravshipTravelling)
				{
					SectionLayer_GravshipMask.RegenerateGravshipShadowMask(base.Map, subMesh, this.section.CellRect);
				}
				else
				{
					this.RegenerateGravshipMask(subMesh);
				}
				break;
			case SectionLayer_GravshipMask.MaskOverrideMode.Shadow:
				SectionLayer_GravshipMask.RegenerateGravshipShadowMask(base.Map, subMesh, this.section.CellRect);
				break;
			case SectionLayer_GravshipMask.MaskOverrideMode.Gravship:
				this.RegenerateGravshipMask(subMesh);
				break;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
		}

		// Token: 0x020025FD RID: 9725
		public enum MaskOverrideMode
		{
			// Token: 0x04009BD1 RID: 39889
			None,
			// Token: 0x04009BD2 RID: 39890
			Shadow,
			// Token: 0x04009BD3 RID: 39891
			Gravship
		}
	}
}
