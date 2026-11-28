using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F21 RID: 7969
	public static class GenWorld
	{
		// Token: 0x0600CB52 RID: 52050 RVA: 0x004AD02C File Offset: 0x004AB22C
		public static PlanetTile MouseTile(bool snapToExpandableWorldObjects = false)
		{
			PlanetLayer selected = PlanetLayer.Selected;
			if (snapToExpandableWorldObjects)
			{
				if (GenWorld.cachedFrame_snap == Time.frameCount && GenWorld.cachedLayer == selected)
				{
					return GenWorld.cachedTile_snap;
				}
				GenWorld.cachedTile_snap = GenWorld.TileAt(UI.MousePositionOnUI, true);
				GenWorld.cachedFrame_snap = Time.frameCount;
				GenWorld.cachedLayer = selected;
				return GenWorld.cachedTile_snap;
			}
			else
			{
				if (GenWorld.cachedFrame_noSnap == Time.frameCount && GenWorld.cachedLayer == selected)
				{
					return GenWorld.cachedTile_noSnap;
				}
				GenWorld.cachedTile_noSnap = GenWorld.TileAt(UI.MousePositionOnUI, false);
				GenWorld.cachedFrame_noSnap = Time.frameCount;
				GenWorld.cachedLayer = selected;
				return GenWorld.cachedTile_noSnap;
			}
		}

		// Token: 0x0600CB53 RID: 52051 RVA: 0x004AD0C4 File Offset: 0x004AB2C4
		public static PlanetTile TileAt(Vector2 clickPos, bool snapToExpandableWorldObjects = false)
		{
			Camera worldCamera = Find.WorldCamera;
			if (!worldCamera.gameObject.activeInHierarchy)
			{
				return PlanetTile.Invalid;
			}
			if (snapToExpandableWorldObjects)
			{
				ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(UI.MousePositionOnUI, GenWorld.tmpWorldObjectsUnderMouse);
				if (GenWorld.tmpWorldObjectsUnderMouse.Any<WorldObject>())
				{
					PlanetTile tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
					GenWorld.tmpWorldObjectsUnderMouse.Clear();
					return tile;
				}
			}
			Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			WorldTerrainColliderManager.EnsureRaycastCollidersUpdated();
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
			{
				return Find.World.renderer.GetTileFromRayHit(hit);
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x0600CB54 RID: 52052 RVA: 0x004AD16C File Offset: 0x004AB36C
		public static string GetPollutionDescription(float pollution)
		{
			if (pollution <= 0f)
			{
				return "TilePollutionNone".Translate();
			}
			if (pollution <= 0.333f)
			{
				return "TilePollutionLight".Translate();
			}
			if (pollution <= 0.666f)
			{
				return "TilePollutionModerate".Translate();
			}
			return "TilePollutionExtreme".Translate();
		}

		// Token: 0x040081A1 RID: 33185
		private static PlanetTile cachedTile_noSnap = PlanetTile.Invalid;

		// Token: 0x040081A2 RID: 33186
		private static int cachedFrame_noSnap = -1;

		// Token: 0x040081A3 RID: 33187
		private static PlanetTile cachedTile_snap = PlanetTile.Invalid;

		// Token: 0x040081A4 RID: 33188
		private static int cachedFrame_snap = -1;

		// Token: 0x040081A5 RID: 33189
		private static PlanetLayer cachedLayer;

		// Token: 0x040081A6 RID: 33190
		public const float MaxRayLength = 1500f;

		// Token: 0x040081A7 RID: 33191
		private static readonly List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();
	}
}
