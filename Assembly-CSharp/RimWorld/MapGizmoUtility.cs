using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001352 RID: 4946
	[StaticConstructorOnStartup]
	public static class MapGizmoUtility
	{
		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x06007A66 RID: 31334 RVA: 0x002B9EA7 File Offset: 0x002B80A7
		public static Gizmo LastMouseOverGizmo
		{
			get
			{
				return MapGizmoUtility.lastMouseOverGizmo;
			}
		}

		// Token: 0x06007A67 RID: 31335 RVA: 0x002B9EB0 File Offset: 0x002B80B0
		public static void MapUIOnGUI()
		{
			if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				return;
			}
			MapGizmoUtility.tmpObjectsList.Clear();
			MapGizmoUtility.tmpObjectsList.AddRange(Find.Selector.SelectedObjects);
			GizmoGridDrawer.DrawGizmoGridFor(MapGizmoUtility.tmpObjectsList, out MapGizmoUtility.mouseoverGizmo);
		}

		// Token: 0x06007A68 RID: 31336 RVA: 0x002B9F08 File Offset: 0x002B8108
		public static void MapUIUpdate()
		{
			MapGizmoUtility.lastMouseOverGizmo = MapGizmoUtility.mouseoverGizmo;
			if (MapGizmoUtility.mouseoverGizmo != null)
			{
				MapGizmoUtility.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x04004D5A RID: 19802
		private static Gizmo mouseoverGizmo;

		// Token: 0x04004D5B RID: 19803
		private static Gizmo lastMouseOverGizmo;

		// Token: 0x04004D5C RID: 19804
		private static int cacheFrame;

		// Token: 0x04004D5D RID: 19805
		private static readonly List<object> tmpObjectsList = new List<object>();
	}
}
