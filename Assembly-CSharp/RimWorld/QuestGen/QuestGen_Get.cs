using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200215D RID: 8541
	public static class QuestGen_Get
	{
		// Token: 0x0600D5A1 RID: 54689 RVA: 0x004F464C File Offset: 0x004F284C
		public static Map GetMap(bool mustBeInfestable = false, int? preferMapWithMinFreeColonists = null, bool canBeSpace = false)
		{
			QuestGen_Get.<>c__DisplayClass0_0 CS$<>8__locals1 = new QuestGen_Get.<>c__DisplayClass0_0();
			CS$<>8__locals1.mustBeInfestable = mustBeInfestable;
			CS$<>8__locals1.canBeSpace = canBeSpace;
			CS$<>8__locals1.minCount = preferMapWithMinFreeColonists.GetValueOrDefault(1);
			List<Map> source = Find.Maps.Where(new Func<Map, bool>(CS$<>8__locals1.<GetMap>g__Validator|0)).ToList<Map>();
			Map map;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= CS$<>8__locals1.minCount
			select x).TryRandomElement(out map))
			{
				(from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out map);
			}
			if ((map == null && !CS$<>8__locals1.mustBeInfestable) & CS$<>8__locals1.canBeSpace)
			{
				map = Find.Maps.FirstOrDefault((Map x) => x.IsPlayerHome);
			}
			return map;
		}
	}
}
