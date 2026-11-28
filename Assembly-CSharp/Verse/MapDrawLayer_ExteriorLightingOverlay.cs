using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A9 RID: 681
	[StaticConstructorOnStartup]
	public class MapDrawLayer_ExteriorLightingOverlay : MapDrawLayer
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x00078E58 File Offset: 0x00077058
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay && !base.Map.DrawMapClippers;
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00078E71 File Offset: 0x00077071
		public MapDrawLayer_ExteriorLightingOverlay(Map map) : base(map)
		{
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00078E7C File Offset: 0x0007707C
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			LayerSubMesh subMesh2 = base.GetSubMesh(MatBases.ShadowMask);
			subMesh.renderLayer = MapDrawLayer_ExteriorLightingOverlay.RenderLayer;
			subMesh2.renderLayer = MapDrawLayer_ExteriorLightingOverlay.RenderLayer;
			MapDrawLayer_ExteriorLightingOverlay.MakeBaseGeometry(base.Map, subMesh);
			MapDrawLayer_ExteriorLightingOverlay.MakeBaseGeometry(base.Map, subMesh2);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00078ECF File Offset: 0x000770CF
		private static IEnumerable<Rect> GetClipperRects(Map map)
		{
			MapDrawLayer_ExteriorLightingOverlay.<GetClipperRects>d__6 <GetClipperRects>d__ = new MapDrawLayer_ExteriorLightingOverlay.<GetClipperRects>d__6(-2);
			<GetClipperRects>d__.<>3__map = map;
			return <GetClipperRects>d__;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00078EE0 File Offset: 0x000770E0
		private static void MakeBaseGeometry(Map map, LayerSubMesh sm)
		{
			float y = AltitudeLayer.LightingOverlay.AltitudeFor();
			sm.Clear(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
			foreach (Rect rect in MapDrawLayer_ExteriorLightingOverlay.GetClipperRects(map))
			{
				sm.verts.Add(new Vector3(rect.x, y, rect.y));
				sm.verts.Add(new Vector3(rect.x, y, rect.yMax));
				sm.verts.Add(new Vector3(rect.xMax, y, rect.yMax));
				sm.verts.Add(new Vector3(rect.xMax, y, rect.y));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.tris.Add(sm.verts.Count - 4);
				sm.tris.Add(sm.verts.Count - 3);
				sm.tris.Add(sm.verts.Count - 2);
				sm.tris.Add(sm.verts.Count - 4);
				sm.tris.Add(sm.verts.Count - 2);
				sm.tris.Add(sm.verts.Count - 1);
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
		}

		// Token: 0x04001269 RID: 4713
		private static readonly int RenderLayer = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x0400126A RID: 4714
		private const int Size = 200;
	}
}
