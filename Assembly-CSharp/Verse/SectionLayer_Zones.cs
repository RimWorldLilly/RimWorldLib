using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C8 RID: 712
	internal class SectionLayer_Zones : SectionLayer
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001599 RID: 5529 RVA: 0x0008128B File Offset: 0x0007F48B
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays && !WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess;
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000812A7 File Offset: 0x0007F4A7
		public SectionLayer_Zones(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Zone;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x000812C0 File Offset: 0x0007F4C0
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawZones)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000812D0 File Offset: 0x0007F4D0
		public override void Regenerate()
		{
			float y = AltitudeLayer.Zone.AltitudeFor();
			ZoneManager zoneManager = base.Map.zoneManager;
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Zone zone = zoneManager.ZoneAt(new IntVec3(i, 0, j));
					if (zone != null && !zone.Hidden)
					{
						LayerSubMesh subMesh = base.GetSubMesh(zone.Material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 1);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count + 3);
					}
				}
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}
	}
}
