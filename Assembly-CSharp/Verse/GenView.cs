using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000587 RID: 1415
	public static class GenView
	{
		// Token: 0x060029C6 RID: 10694 RVA: 0x000FF8ED File Offset: 0x000FDAED
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map, bool drawOffscreen = true)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map, drawOffscreen);
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000FF8FC File Offset: 0x000FDAFC
		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map, bool drawOffscreen = true)
		{
			if (map != Find.CurrentMap)
			{
				return false;
			}
			if (!loc.InBounds(map))
			{
				return false;
			}
			if (drawOffscreen)
			{
				return true;
			}
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			return GenView.viewRect.Contains(loc);
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000FF94D File Offset: 0x000FDB4D
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x04001CF3 RID: 7411
		private static CellRect viewRect;

		// Token: 0x04001CF4 RID: 7412
		private const int ViewRectMargin = 5;
	}
}
