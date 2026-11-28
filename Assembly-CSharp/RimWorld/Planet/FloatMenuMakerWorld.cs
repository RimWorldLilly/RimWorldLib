using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E8 RID: 9192
	public static class FloatMenuMakerWorld
	{
		// Token: 0x0600E59A RID: 58778 RVA: 0x00554868 File Offset: 0x00552A68
		public static bool TryMakeFloatMenu(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePositionOnUI, caravan);
			if (list.Count == 0)
			{
				return false;
			}
			FloatMenuWorld window = new FloatMenuWorld(list, caravan.LabelCap, mousePositionOnUI);
			Find.WindowStack.Add(window);
			return true;
		}

		// Token: 0x0600E59B RID: 58779 RVA: 0x005548B4 File Offset: 0x00552AB4
		public static List<FloatMenuOption> ChoicesAtFor(Vector2 mousePos, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> list2 = GenWorldUI.WorldObjectsUnderMouse(mousePos);
			for (int i = 0; i < list2.Count; i++)
			{
				list.AddRange(list2[i].GetFloatMenuOptions(caravan));
			}
			return list;
		}

		// Token: 0x0600E59C RID: 58780 RVA: 0x005548F4 File Offset: 0x00552AF4
		public static List<FloatMenuOption> ChoicesAtFor(PlanetTile tile, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile)
				{
					list.AddRange(allWorldObjects[i].GetFloatMenuOptions(caravan));
				}
			}
			return list;
		}
	}
}
