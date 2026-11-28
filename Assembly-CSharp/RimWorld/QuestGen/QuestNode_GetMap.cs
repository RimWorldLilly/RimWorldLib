using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C4 RID: 8644
	public class QuestNode_GetMap : QuestNode
	{
		// Token: 0x0600D739 RID: 55097 RVA: 0x004FDB40 File Offset: 0x004FBD40
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return true;
			}
			if (this.TryFindMap(slate, out map))
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600D73A RID: 55098 RVA: 0x004FDB94 File Offset: 0x004FBD94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map;
			if (QuestGen.slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return;
			}
			if (this.TryFindMap(slate, out map))
			{
				QuestGen.slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
			}
		}

		// Token: 0x0600D73B RID: 55099 RVA: 0x004FDBF0 File Offset: 0x004FBDF0
		private bool TryFindMap(Slate slate, out Map map)
		{
			int minCount;
			if (!this.preferMapWithMinFreeColonists.TryGetValue(slate, out minCount) || minCount < 1)
			{
				minCount = 1;
			}
			IEnumerable<Map> source = from x in Find.Maps
			where x.IsPlayerHome && this.IsAcceptableMap(x, slate)
			select x;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= minCount
			select x).TryRandomElement(out map))
			{
				return (from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out map);
			}
			return true;
		}

		// Token: 0x0600D73C RID: 55100 RVA: 0x004FDC98 File Offset: 0x004FBE98
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			if (map == null)
			{
				return false;
			}
			if (map.IsPocketMap)
			{
				return false;
			}
			if (!this.canBeSpace.GetValue(slate) && map.Tile.Valid && map.Tile.LayerDef.isSpace)
			{
				return false;
			}
			IntVec3 intVec;
			if (this.mustBeInfestable.GetValue(slate) && !InfestationCellFinder.TryFindCell(out intVec, map))
			{
				return false;
			}
			List<PlanetLayerDef> value = this.layerWhitelist.GetValue(slate);
			List<PlanetLayerDef> value2 = this.layerBlacklist.GetValue(slate);
			return (value.NullOrEmpty<PlanetLayerDef>() || !map.Tile.Valid || value.Contains(map.Tile.LayerDef)) && (value2.NullOrEmpty<PlanetLayerDef>() || !map.Tile.Valid || !value2.Contains(map.Tile.LayerDef));
		}

		// Token: 0x04008892 RID: 34962
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04008893 RID: 34963
		public SlateRef<bool> mustBeInfestable;

		// Token: 0x04008894 RID: 34964
		public SlateRef<bool> canBeSpace;

		// Token: 0x04008895 RID: 34965
		public SlateRef<int> preferMapWithMinFreeColonists;

		// Token: 0x04008896 RID: 34966
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x04008897 RID: 34967
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
