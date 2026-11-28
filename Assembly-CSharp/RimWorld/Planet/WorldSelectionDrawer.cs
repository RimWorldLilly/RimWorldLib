using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023FE RID: 9214
	public static class WorldSelectionDrawer
	{
		// Token: 0x170028BA RID: 10426
		// (get) Token: 0x0600E66E RID: 58990 RVA: 0x0055B8DD File Offset: 0x00559ADD
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x0600E66F RID: 58991 RVA: 0x0055B8E4 File Offset: 0x00559AE4
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x0600E670 RID: 58992 RVA: 0x0055B8F6 File Offset: 0x00559AF6
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x0600E671 RID: 58993 RVA: 0x0055B904 File Offset: 0x00559B04
		public static void SelectionOverlaysOnGUI()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				WorldSelectionDrawer.DrawSelectionBracketOnGUIFor(worldObject);
				worldObject.ExtraSelectionOverlaysOnGUI();
			}
		}

		// Token: 0x0600E672 RID: 58994 RVA: 0x0055B940 File Offset: 0x00559B40
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				selectedObjects[i].DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x0600E673 RID: 58995 RVA: 0x0055B978 File Offset: 0x00559B78
		private static void DrawSelectionBracketOnGUIFor(WorldObject obj)
		{
			Vector2 vector = obj.ScreenPos();
			Rect rect = new Rect(vector.x - 17.5f, vector.y - 17.5f, 35f, 35f);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * 0.4f, (float)SelectionDrawerUtility.SelectedTexGUI.height * 0.4f);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(WorldSelectionDrawer.bracketLocs, obj, rect, WorldSelectionDrawer.selectTimes, textureSize, 25f);
			GUI.color = (obj.HiddenBehindTerrainNow() ? WorldSelectionDrawer.HiddenSelectionBracketColor : Color.white);
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(WorldSelectionDrawer.bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, 0.4f, null);
				num += 90;
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400926A RID: 37482
		private static readonly Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x0400926B RID: 37483
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x0400926C RID: 37484
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x0400926D RID: 37485
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x0400926E RID: 37486
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x0400926F RID: 37487
		private static readonly Vector2[] bracketLocs = new Vector2[4];
	}
}
