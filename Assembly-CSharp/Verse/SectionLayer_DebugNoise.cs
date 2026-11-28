using System;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B1 RID: 689
	[StaticConstructorOnStartup]
	internal class SectionLayer_DebugNoise : SectionLayer
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0007BAFF File Offset: 0x00079CFF
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays;
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0007BB08 File Offset: 0x00079D08
		public SectionLayer_DebugNoise(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.None;
			SectionLayer_DebugNoise.material = (SectionLayer_DebugNoise.material ? SectionLayer_DebugNoise.material : new Material(ShaderDatabase.VertexColor));
			SectionLayer_DebugNoise.material.renderQueue = 3800;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0007BB5D File Offset: 0x00079D5D
		private bool TryGetWindow(out Dialog_DevNoiseMap window)
		{
			return Find.WindowStack.TryGetWindow<Dialog_DevNoiseMap>(out window);
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0007BB6C File Offset: 0x00079D6C
		public override void DrawLayer()
		{
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (!this.TryGetWindow(out dialog_DevNoiseMap) || dialog_DevNoiseMap.alpha <= 0f)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0007BB98 File Offset: 0x00079D98
		public override void Regenerate()
		{
			float y = AltitudeLayer.Zone.AltitudeFor();
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (!this.TryGetWindow(out dialog_DevNoiseMap))
			{
				base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
				return;
			}
			byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(dialog_DevNoiseMap.alpha * 255.999f), 0, 255);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt((float)dialog_DevNoiseMap.noise.GetValue((double)i, 0.0, (double)j) * 255.999f), 0, 255);
					Color32 item = new Color32(b, b, b, a);
					if (!dialog_DevNoiseMap.cutoffEnabled || dialog_DevNoiseMap.noise.GetValue((double)i, 0.0, (double)j) >= (double)dialog_DevNoiseMap.cutoff)
					{
						LayerSubMesh subMesh = base.GetSubMesh(SectionLayer_DebugNoise.material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 1);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count + 3);
					}
				}
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
		}

		// Token: 0x04001288 RID: 4744
		private static Material material;
	}
}
