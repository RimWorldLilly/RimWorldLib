using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023E RID: 574
	public static class AreaUtility
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x00060E18 File Offset: 0x0005F018
		public static void MakeAllowedAreaListFloatMenu(Action<Area> selAction, bool addNullAreaOption, bool addManageOption, Map map)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (addNullAreaOption)
			{
				list.Add(new FloatMenuOption("NoAreaAllowed".Translate(), delegate()
				{
					selAction(null);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0));
			}
			foreach (Area localArea2 in from a in map.areaManager.AllAreas
			where a.AssignableAsAllowed()
			select a)
			{
				Area localArea = localArea2;
				FloatMenuOption item = new FloatMenuOption(localArea.Label, delegate()
				{
					selAction(localArea);
				}, MenuOptionPriority.Default, delegate(Rect _)
				{
					localArea.MarkForDraw();
				}, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			if (addManageOption)
			{
				list.Add(new FloatMenuOption("ManageAreas".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageAreas(map));
				}, MenuOptionPriority.Low, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00060F78 File Offset: 0x0005F178
		public static string AreaAllowedLabel(Pawn pawn)
		{
			if (pawn.playerSettings != null)
			{
				return AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.AreaRestrictionInPawnCurrentMap);
			}
			return AreaUtility.AreaAllowedLabel_Area(null);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00060F99 File Offset: 0x0005F199
		public static string AreaAllowedLabel_Area(Area area)
		{
			if (area != null)
			{
				return area.Label;
			}
			return "NoAreaAllowed".Translate();
		}
	}
}
