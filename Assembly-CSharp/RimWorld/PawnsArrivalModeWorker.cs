using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBD RID: 4029
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x0600665E RID: 26206 RVA: 0x00245A10 File Offset: 0x00243C10
		public virtual bool CanUseWith(IncidentParms parms)
		{
			if (parms.faction != null)
			{
				if (this.def.minTechLevel != TechLevel.Undefined && parms.faction.def.techLevel < this.def.minTechLevel)
				{
					return false;
				}
				List<PawnsArrivalModeDef> arrivalModeWhitelist = parms.faction.def.arrivalModeWhitelist;
				List<PawnsArrivalModeDef> arrivalModeBlacklist = parms.faction.def.arrivalModeBlacklist;
				if (!arrivalModeWhitelist.NullOrEmpty<PawnsArrivalModeDef>() && !arrivalModeWhitelist.Contains(this.def))
				{
					return false;
				}
				if (!arrivalModeBlacklist.NullOrEmpty<PawnsArrivalModeDef>() && arrivalModeBlacklist.Contains(this.def))
				{
					return false;
				}
			}
			if (parms.raidArrivalModeForQuickMilitaryAid && !this.def.forQuickMilitaryAid)
			{
				return false;
			}
			if (parms.raidStrategy != null && !parms.raidStrategy.arriveModes.Contains(this.def))
			{
				return false;
			}
			Map map = parms.target as Map;
			return map == null || this.CanUseOnMap(map);
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x00245AF8 File Offset: 0x00243CF8
		public virtual bool CanUseOnMap(Map map)
		{
			return (this.def.biomeWhitelist.NullOrEmpty<BiomeDef>() || this.def.biomeWhitelist.Contains(map.Biome)) && (this.def.biomeBlacklist.NullOrEmpty<BiomeDef>() || !this.def.biomeBlacklist.Contains(map.Biome)) && (!map.Biome.onlyAllowWhitelistedArrivalModes || (!this.def.biomeWhitelist.NullOrEmpty<BiomeDef>() && this.def.biomeWhitelist.Contains(map.Biome))) && (!map.Tile.Valid || this.CanUseOnTile(map.Tile)) && (!this.def.walkIn || map.CanEverExit);
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x00245BD0 File Offset: 0x00243DD0
		public virtual bool CanUseOnTile(PlanetTile tile)
		{
			return (this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || this.def.layerWhitelist.Contains(tile.LayerDef)) && (this.def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !this.def.layerBlacklist.Contains(tile.LayerDef)) && (!tile.LayerDef.onlyAllowWhitelistedArrivalModes || (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && this.def.layerWhitelist.Contains(tile.LayerDef)));
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x00245C74 File Offset: 0x00243E74
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			if (this.def.selectionWeightCurvesPerFaction != null && parms.faction != null)
			{
				List<FactionCurve> selectionWeightCurvesPerFaction = this.def.selectionWeightCurvesPerFaction;
				for (int i = 0; i < selectionWeightCurvesPerFaction.Count; i++)
				{
					if (selectionWeightCurvesPerFaction[i].faction == parms.faction.def)
					{
						return selectionWeightCurvesPerFaction[i].Evaluate(parms.points);
					}
				}
			}
			if (this.def.selectionWeightCurve != null)
			{
				return this.def.selectionWeightCurve.Evaluate(parms.points);
			}
			return 0f;
		}

		// Token: 0x06006662 RID: 26210
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x06006663 RID: 26211 RVA: 0x00245D08 File Offset: 0x00243F08
		public virtual void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06006664 RID: 26212
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);

		// Token: 0x04003EC7 RID: 16071
		public PawnsArrivalModeDef def;
	}
}
