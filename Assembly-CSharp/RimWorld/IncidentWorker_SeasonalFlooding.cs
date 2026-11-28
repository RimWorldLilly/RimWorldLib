using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001195 RID: 4501
	public class IncidentWorker_SeasonalFlooding : IncidentWorker
	{
		// Token: 0x060070AB RID: 28843 RVA: 0x00276EF8 File Offset: 0x002750F8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.SeasonalFlood, map.Center, map, WipeMode.Vanish);
			return true;
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x00276F28 File Offset: 0x00275128
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			SurfaceTile surfaceTile = map.TileInfo as SurfaceTile;
			if (surfaceTile == null)
			{
				return false;
			}
			if (surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>())
			{
				return false;
			}
			if (map.mapTemperature.OutdoorTemp < -7f)
			{
				return false;
			}
			Vector2 longLat = Find.WorldGrid.LongLatOf(Find.WorldSelector.SelectedTile);
			Season season = GenDate.Season((long)Find.TickManager.TicksGame, longLat);
			return season == Season.Spring || season == Season.PermanentSummer;
		}
	}
}
