using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C7 RID: 711
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06001596 RID: 5526 RVA: 0x000811F8 File Offset: 0x0007F3F8
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00081211 File Offset: 0x0007F411
		public override Material GetMaterialFor(CellTerrain terrain)
		{
			return terrain.def.waterDepthMaterial;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00081220 File Offset: 0x0007F420
		public override void DrawLayer()
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
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}
