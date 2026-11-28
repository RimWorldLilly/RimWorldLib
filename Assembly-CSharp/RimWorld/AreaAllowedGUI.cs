using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C25 RID: 7205
	public static class AreaAllowedGUI
	{
		// Token: 0x0600B7BE RID: 47038 RVA: 0x0040C4AC File Offset: 0x0040A6AC
		public static void DoAllowedAreaSelectors(Rect rect, Pawn p)
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			List<Area> allAreas = Find.CurrentMap.areaManager.AllAreas;
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++)
			{
				if (allAreas[i].AssignableAsAllowed())
				{
					num++;
				}
			}
			float num2 = rect.width / (float)num;
			Text.WordWrap = false;
			Text.Font = GameFont.Tiny;
			AreaAllowedGUI.DoAreaSelector(new Rect(rect.x + 0f, rect.y, num2, rect.height), p, null);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].AssignableAsAllowed())
				{
					float num4 = (float)num3 * num2;
					AreaAllowedGUI.DoAreaSelector(new Rect(rect.x + num4, rect.y, num2, rect.height), p, allAreas[j]);
					num3++;
				}
			}
			Text.WordWrap = true;
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600B7BF RID: 47039 RVA: 0x0040C5A4 File Offset: 0x0040A7A4
		private static void DoAreaSelector(Rect rect, Pawn p, Area area)
		{
			MouseoverSounds.DoRegion(rect);
			rect = rect.ContractedBy(1f);
			GUI.DrawTexture(rect, (area != null) ? area.ColorTexture : BaseContent.GreyTex);
			Text.Anchor = TextAnchor.MiddleLeft;
			string text = AreaUtility.AreaAllowedLabel_Area(area);
			Rect rect2 = rect;
			rect2.xMin += 3f;
			rect2.yMin += 2f;
			Widgets.Label(rect2, text);
			if (p.playerSettings.AreaRestrictionInPawnCurrentMap == area)
			{
				Widgets.DrawBox(rect, 2, null);
			}
			if (Event.current.rawType == EventType.mouseUp && Event.current.button == 0)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (!Input.GetMouseButton(0) && Event.current.type != EventType.mouseDown)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (Mouse.IsOver(rect))
			{
				if (area != null)
				{
					area.MarkForDraw();
				}
				if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
				{
					AreaAllowedGUI.dragging = true;
				}
				if (AreaAllowedGUI.dragging && p.playerSettings.AreaRestrictionInPawnCurrentMap != area)
				{
					p.playerSettings.AreaRestrictionInPawnCurrentMap = area;
					SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, text);
		}

		// Token: 0x0400694E RID: 26958
		private static bool dragging;
	}
}
