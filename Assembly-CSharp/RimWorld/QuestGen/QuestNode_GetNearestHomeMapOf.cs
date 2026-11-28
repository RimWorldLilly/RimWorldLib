using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CC RID: 8652
	public class QuestNode_GetNearestHomeMapOf : QuestNode
	{
		// Token: 0x0600D75D RID: 55133 RVA: 0x004FEBD9 File Offset: 0x004FCDD9
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D75E RID: 55134 RVA: 0x004FEBE3 File Offset: 0x004FCDE3
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D75F RID: 55135 RVA: 0x004FEBF0 File Offset: 0x004FCDF0
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				Map mapHeld = this.mapOf.GetValue(slate).MapHeld;
				if (mapHeld != null && mapHeld.IsPlayerHome)
				{
					slate.Set<Map>(this.storeAs.GetValue(slate), mapHeld, false);
					return;
				}
				PlanetTile tile = this.mapOf.GetValue(slate).Tile;
				if (tile.Valid)
				{
					Map map = null;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						if (maps[i].IsPlayerHome && (map == null || Find.WorldGrid.ApproxDistanceInTiles(tile, maps[i].Tile) < Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile)))
						{
							map = maps[i];
						}
					}
					if (map != null)
					{
						slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
					}
				}
			}
		}

		// Token: 0x040088B5 RID: 34997
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040088B6 RID: 34998
		public SlateRef<Thing> mapOf;
	}
}
