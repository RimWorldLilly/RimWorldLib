using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BA RID: 4538
	public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06007162 RID: 29026 RVA: 0x0027B718 File Offset: 0x00279918
		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Peaceful;
			}
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x0027B720 File Offset: 0x00279920
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			if (!base.FactionCanBeGroupSource(f, parms, desperate))
			{
				return false;
			}
			if (f.Hidden || f.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (f.def.pawnGroupMakers == null || !f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.PawnGroupKindDef))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, f) && (f.def.neutralArrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() || f.def.neutralArrivalLayerWhitelist.Contains(map.Tile.LayerDef)) && (f.def.neutralArrivalLayerBlacklist.NullOrEmpty<PlanetLayerDef>() || !f.def.neutralArrivalLayerBlacklist.Contains(map.Tile.LayerDef));
		}

		// Token: 0x06007164 RID: 29028 RVA: 0x0027B800 File Offset: 0x00279A00
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			using (List<GameCondition>.Enumerator enumerator = ((Map)parms.target).GameConditionManager.ActiveConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.preventNeutralVisitors)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007165 RID: 29029 RVA: 0x0027B878 File Offset: 0x00279A78
		protected bool TryResolveParms(IncidentParms parms)
		{
			if (!this.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			this.ResolveParmsPoints(parms);
			return true;
		}

		// Token: 0x06007166 RID: 29030 RVA: 0x0027B890 File Offset: 0x00279A90
		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return (parms.spawnCenter.IsValid || RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, false, null)) && (parms.faction != null || base.CandidateFactions(parms, false).TryRandomElement(out parms.faction) || base.CandidateFactions(parms, true).TryRandomElement(out parms.faction));
		}

		// Token: 0x06007167 RID: 29031
		protected abstract void ResolveParmsPoints(IncidentParms parms);

		// Token: 0x06007168 RID: 29032 RVA: 0x0027B904 File Offset: 0x00279B04
		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true), false).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				List<Pawn> storeGeneratedNeutralPawns = parms.storeGeneratedNeutralPawns;
				if (storeGeneratedNeutralPawns != null)
				{
					storeGeneratedNeutralPawns.Add(pawn);
				}
			}
			return list;
		}
	}
}
