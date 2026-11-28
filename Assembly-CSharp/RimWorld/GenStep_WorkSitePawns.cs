using System;
using System.Linq;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012FE RID: 4862
	public class GenStep_WorkSitePawns : GenStep
	{
		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x0600788F RID: 30863 RVA: 0x002AAC86 File Offset: 0x002A8E86
		public override int SeedPart
		{
			get
			{
				return 237483478;
			}
		}

		// Token: 0x06007890 RID: 30864 RVA: 0x002AAC90 File Offset: 0x002A8E90
		public static int GetEnemiesCount(Site site, SitePartParams parms, PawnGroupKindDef workerGroupKind)
		{
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms);
			return PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsWorkers(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed, workerGroupKind)).Count<PawnKindDef>() + PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsFighters(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed)).Count<PawnKindDef>();
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x002AACEC File Offset: 0x002A8EEC
		private static PawnGroupMakerParms GroupMakerParmsWorkers(PlanetTile tile, Faction faction, float points, int seed, PawnGroupKindDef workerGroupKind)
		{
			float a = points / 2f;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = workerGroupKind,
				tile = tile,
				faction = faction,
				inhabitants = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x06007892 RID: 30866 RVA: 0x002AAD50 File Offset: 0x002A8F50
		private static PawnGroupMakerParms GroupMakerParmsFighters(PlanetTile tile, Faction faction, float points, int seed)
		{
			float a = points / 2f;
			PawnGroupKindDef groupKindDef = PawnGroupKindDefOf.Combat;
			if (!faction.def.pawnGroupMakers.Any((PawnGroupMaker maker) => maker.kindDef == groupKindDef))
			{
				groupKindDef = PawnGroupKindDefOf.Settlement;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = groupKindDef,
				tile = tile,
				faction = faction,
				inhabitants = true,
				generateFightersOnly = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x002AADF8 File Offset: 0x002A8FF8
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rect;
			IntVec3 baseCenter;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rect))
			{
				baseCenter = rect.CenterCell;
				Log.Error("No rect of interest set when running GenStep_WorkSitePawns!");
			}
			else
			{
				baseCenter = map.Center;
			}
			Faction faction = parms.sitePart.site.Faction;
			Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, baseCenter, 25000, false), map, null);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms));
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			resolveParams.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsWorkers(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed, ((SitePartWorker_WorkSite)parms.sitePart.def.Worker).WorkerGroupKind);
			resolveParams.pawnGroupKindDef = resolveParams.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams, null);
			ResolveParams resolveParams2 = resolveParams;
			resolveParams2.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsFighters(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed);
			resolveParams2.pawnGroupKindDef = resolveParams2.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams2, null);
			BaseGen.globalSettings.map = map;
			BaseGen.Generate();
		}
	}
}
