using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000246 RID: 582
	public static class CompressibilityDeciderUtility
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x000625E4 File Offset: 0x000607E4
		public static bool IsSaveCompressible(this Thing t)
		{
			if (Scribe.saver.savingForDebug)
			{
				return false;
			}
			if (!t.def.saveCompressible)
			{
				return false;
			}
			if (t.def.useHitPoints && t.HitPoints != t.MaxHitPoints)
			{
				return false;
			}
			if (!t.Spawned)
			{
				return false;
			}
			if (t.Position.GetMaxItemsAllowedInCell(t.Map) > 1 && t.def.category == ThingCategory.Item)
			{
				return false;
			}
			Building building = t as Building;
			if (building != null && !building.canChangeTerrainOnDestroyed)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].compressor != null)
				{
					flag = true;
					if (maps[i].compressor.compressibilityDecider.IsReferenced(t))
					{
						return false;
					}
				}
			}
			if (!flag)
			{
				Log.ErrorOnce("Called IsSaveCompressible but there are no maps with compressor != null. This should never happen. It probably means that we're not saving any map at the moment?", 1935111328);
			}
			return true;
		}
	}
}
