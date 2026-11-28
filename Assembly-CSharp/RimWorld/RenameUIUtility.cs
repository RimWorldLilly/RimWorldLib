using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020AC RID: 8364
	public static class RenameUIUtility
	{
		// Token: 0x0600D2CE RID: 53966 RVA: 0x004D88CC File Offset: 0x004D6ACC
		public static void DrawRenameButton(Rect rect, Pawn pawn)
		{
			TooltipHandler.TipRegionByKey(rect, pawn.RaceProps.IsMechanoid ? "RenameMech" : "RenameAnimal");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(pawn.NamePawnDialog(null));
			}
		}

		// Token: 0x0600D2CF RID: 53967 RVA: 0x004D8918 File Offset: 0x004D6B18
		public static void DrawRenameButton(Rect rect, IRenameable renamable)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameBuildingStorage(renamable));
			}
		}

		// Token: 0x0600D2D0 RID: 53968 RVA: 0x004D8944 File Offset: 0x004D6B44
		public static void DrawRenameButton(Rect rect, IStorageGroupMember building)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameBuildingStorage_CreateNew(building));
			}
		}

		// Token: 0x0600D2D1 RID: 53969 RVA: 0x004D8970 File Offset: 0x004D6B70
		public static void DrawRenameButton(Rect rect, CompAnimalPenMarker marker)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameAnimalPen(marker.parent.Map, marker));
			}
		}

		// Token: 0x0600D2D2 RID: 53970 RVA: 0x004D89A7 File Offset: 0x004D6BA7
		public static void DrawRenameButton(Rect rect, Building_GravEngine engine)
		{
			TooltipHandler.TipRegionByKey(rect, "Rename");
			if (Widgets.ButtonImage(rect, TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameGravship(engine));
			}
		}
	}
}
