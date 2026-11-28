using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E9 RID: 9193
	public static class GenWorldUI
	{
		// Token: 0x17002887 RID: 10375
		// (get) Token: 0x0600E59D RID: 58781 RVA: 0x0055494B File Offset: 0x00552B4B
		public static float CaravanDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.AverageTileSize;
			}
		}

		// Token: 0x17002888 RID: 10376
		// (get) Token: 0x0600E59E RID: 58782 RVA: 0x0055495D File Offset: 0x00552B5D
		private static float CaravanWideClickRadius
		{
			get
			{
				return 0.75f * Find.WorldGrid.AverageTileSize;
			}
		}

		// Token: 0x17002889 RID: 10377
		// (get) Token: 0x0600E59F RID: 58783 RVA: 0x0055496F File Offset: 0x00552B6F
		private static float DynamicallyDrawnObjectDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.AverageTileSize;
			}
		}

		// Token: 0x0600E5A0 RID: 58784 RVA: 0x00554984 File Offset: 0x00552B84
		public static List<WorldObject> WorldObjectsUnderMouse(Vector2 mousePos)
		{
			List<WorldObject> list = new List<WorldObject>();
			ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(mousePos, list);
			float caravanDirectClickRadius = GenWorldUI.CaravanDirectClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.DistanceToMouse(mousePos) < caravanDirectClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int j = 0; j < GenWorldUI.clickedCaravans.Count; j++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[j]))
				{
					list.Add(GenWorldUI.clickedCaravans[j]);
				}
			}
			float dynamicallyDrawnObjectDirectClickRadius = GenWorldUI.DynamicallyDrawnObjectDirectClickRadius;
			GenWorldUI.clickedDynamicallyDrawnObjects.Clear();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int k = 0; k < allWorldObjects.Count; k++)
			{
				WorldObject worldObject = allWorldObjects[k];
				if (worldObject.def.useDynamicDrawer && worldObject.DistanceToMouse(mousePos) < dynamicallyDrawnObjectDirectClickRadius)
				{
					GenWorldUI.clickedDynamicallyDrawnObjects.Add(worldObject);
				}
			}
			GenWorldUI.clickedDynamicallyDrawnObjects.SortBy((WorldObject x) => x.DistanceToMouse(mousePos));
			for (int l = 0; l < GenWorldUI.clickedDynamicallyDrawnObjects.Count; l++)
			{
				if (!list.Contains(GenWorldUI.clickedDynamicallyDrawnObjects[l]))
				{
					list.Add(GenWorldUI.clickedDynamicallyDrawnObjects[l]);
				}
			}
			PlanetTile rhs = GenWorld.TileAt(mousePos, false);
			List<WorldObject> allWorldObjects2 = Find.WorldObjects.AllWorldObjects;
			for (int m = 0; m < allWorldObjects2.Count; m++)
			{
				if (allWorldObjects2[m].Tile == rhs && !list.Contains(allWorldObjects2[m]))
				{
					list.Add(allWorldObjects2[m]);
				}
			}
			float caravanWideClickRadius = GenWorldUI.CaravanWideClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans2 = Find.WorldObjects.Caravans;
			for (int n = 0; n < caravans2.Count; n++)
			{
				Caravan caravan2 = caravans2[n];
				if (caravan2.DistanceToMouse(mousePos) < caravanWideClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan2);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int num = 0; num < GenWorldUI.clickedCaravans.Count; num++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[num]))
				{
					list.Add(GenWorldUI.clickedCaravans[num]);
				}
			}
			GenWorldUI.clickedCaravans.Clear();
			return list;
		}

		// Token: 0x0600E5A1 RID: 58785 RVA: 0x00554C3C File Offset: 0x00552E3C
		public static Vector2 WorldToUIPosition(Vector3 worldLoc)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldLoc) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600E5A2 RID: 58786 RVA: 0x00554C78 File Offset: 0x00552E78
		public static float CurUITileSize()
		{
			Transform transform = Find.WorldCamera.transform;
			Vector3 localPosition = transform.localPosition;
			Quaternion rotation = transform.rotation;
			transform.localPosition = new Vector3(0f, 0f, localPosition.magnitude);
			transform.rotation = Quaternion.identity;
			float x = (GenWorldUI.WorldToUIPosition(new Vector3(-Find.WorldGrid.AverageTileSize / 2f, 0f, 100f)) - GenWorldUI.WorldToUIPosition(new Vector3(Find.WorldGrid.AverageTileSize / 2f, 0f, 100f))).x;
			transform.localPosition = localPosition;
			transform.rotation = rotation;
			return x;
		}

		// Token: 0x040091EA RID: 37354
		private static List<Caravan> clickedCaravans = new List<Caravan>();

		// Token: 0x040091EB RID: 37355
		private static List<WorldObject> clickedDynamicallyDrawnObjects = new List<WorldObject>();
	}
}
