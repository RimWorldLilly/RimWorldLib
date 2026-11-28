using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023FC RID: 9212
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x0600E651 RID: 58961 RVA: 0x0055A9B5 File Offset: 0x00558BB5
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>d__0 <MultiSelectableWorldObjectsInScreenRectDistinct>d__ = new WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>d__0(-2);
			<MultiSelectableWorldObjectsInScreenRectDistinct>d__.<>3__rect = rect;
			return <MultiSelectableWorldObjectsInScreenRectDistinct>d__;
		}

		// Token: 0x0600E652 RID: 58962 RVA: 0x0055A9C8 File Offset: 0x00558BC8
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos) || (!o.VisibleInBackground && o.Tile.Layer != PlanetLayer.Selected);
		}

		// Token: 0x0600E653 RID: 58963 RVA: 0x0055AA06 File Offset: 0x00558C06
		public static Vector2 ScreenPos(this WorldObject o)
		{
			return GenWorldUI.WorldToUIPosition(o.DrawPos);
		}

		// Token: 0x0600E654 RID: 58964 RVA: 0x0055AA14 File Offset: 0x00558C14
		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(point);
		}

		// Token: 0x0600E655 RID: 58965 RVA: 0x0055AA60 File Offset: 0x00558C60
		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}
	}
}
