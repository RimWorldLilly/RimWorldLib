using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B7 RID: 4535
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06007143 RID: 28995 RVA: 0x0027A47B File Offset: 0x0027867B
		protected virtual bool MustHaveSettlementOnLayer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007144 RID: 28996 RVA: 0x0027A480 File Offset: 0x00278680
		protected IEnumerable<Faction> CandidateFactions(IncidentParms parms, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, parms, desperate)
			select f;
		}

		// Token: 0x06007145 RID: 28997 RVA: 0x0027A4C4 File Offset: 0x002786C4
		public virtual bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			Map map = (Map)parms.target;
			return !f.IsPlayer && !f.defeated && !f.temporary && (desperate || (f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp))) && (!this.MustHaveSettlementOnLayer || f.Hidden || !map.Tile.Valid || Find.WorldObjects.AnyFactionSettlementOnLayer(f, map.Tile.Layer)) && (f.def.arrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() || f.def.arrivalLayerWhitelist.Contains(map.Tile.LayerDef)) && (f.def.arrivalLayerBlacklist.NullOrEmpty<PlanetLayerDef>() || !f.def.arrivalLayerBlacklist.Contains(map.Tile.LayerDef)) && (!map.Tile.LayerDef.onlyAllowWhitelistedArrivals || (!f.def.arrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && f.def.arrivalLayerWhitelist.Contains(map.Tile.LayerDef)));
		}

		// Token: 0x06007146 RID: 28998 RVA: 0x0027A629 File Offset: 0x00278829
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return parms.faction != null || this.CandidateFactions(parms, false).Any<Faction>();
		}

		// Token: 0x06007147 RID: 28999 RVA: 0x0027A644 File Offset: 0x00278844
		public string DebugListingOfGroupSources()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IncidentParms parms = new IncidentParms
			{
				target = Find.CurrentMap
			};
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				stringBuilder.Append(faction.Name);
				if (this.FactionCanBeGroupSource(faction, parms, false))
				{
					stringBuilder.Append("    YES");
				}
				else if (this.FactionCanBeGroupSource(faction, parms, true))
				{
					stringBuilder.Append("    YES-DESPERATE");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
