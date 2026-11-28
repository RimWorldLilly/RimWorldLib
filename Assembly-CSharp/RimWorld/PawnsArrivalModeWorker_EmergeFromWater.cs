using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E9 RID: 5353
	public class PawnsArrivalModeWorker_EmergeFromWater : PawnsArrivalModeWorker
	{
		// Token: 0x060082CF RID: 33487 RVA: 0x002EF19C File Offset: 0x002ED39C
		public override bool CanUseWith(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return false;
			}
			bool flag = !surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>();
			bool isCoastal = map.TileInfo.IsCoastal;
			return flag || isCoastal;
		}

		// Token: 0x060082D0 RID: 33488 RVA: 0x002EF1E4 File Offset: 0x002ED3E4
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			if (pawns.Empty<Pawn>())
			{
				return;
			}
			List<IntVec3> list = new List<IntVec3>();
			List<IntVec3> list2 = new List<IntVec3>();
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 item = PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.RandomElement<IntVec3>();
				PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Remove(item);
				(this.IsBigSpawn(pawns[i]) ? list2 : list).Add(item);
				pawns[i].stances.stunner.StunFor(PawnsArrivalModeWorker_EmergeFromWater.StunRange.RandomInRangeSeeded(pawns[i].ThingID.GetHashCode()).SecondsToTicks(), pawns[i], true, false, false);
			}
			float num = 8f / (float)pawns.Count;
			SpawnRequest spawnRequest = new SpawnRequest((from p in pawns
			where !this.IsBigSpawn(p)
			select p).Cast<Thing>().ToList<Thing>(), list, 1, num, null);
			SpawnRequest spawnRequest2 = new SpawnRequest((from p in pawns
			where this.IsBigSpawn(p)
			select p).Cast<Thing>().ToList<Thing>(), list2, 1, num * 2.7f, null);
			spawnRequest.spawnSound = SoundDefOf.EmergeFromWater;
			spawnRequest.spawnEffect = EffecterDefOf.PawnEmergeFromWater;
			spawnRequest.preSpawnEffect = EffecterDefOf.WaterMist;
			spawnRequest.preSpawnEffecterOffsetTicks = -40;
			spawnRequest.initialDelay = 40;
			spawnRequest.lord = parms.lord;
			spawnRequest2.spawnSound = SoundDefOf.EmergeFromWater;
			spawnRequest2.spawnEffect = EffecterDefOf.PawnEmergeFromWaterLarge;
			spawnRequest2.preSpawnEffect = EffecterDefOf.WaterMist;
			spawnRequest2.preSpawnEffecterOffsetTicks = -40;
			spawnRequest2.initialDelay = 40;
			spawnRequest2.lord = parms.lord;
			Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest, true);
			Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest2, true);
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x002EF3A0 File Offset: 0x002ED5A0
		private bool IsBigSpawn(Pawn pawn)
		{
			return pawn.BodySize > 1.5f;
		}

		// Token: 0x060082D2 RID: 33490 RVA: 0x002EF3B0 File Offset: 0x002ED5B0
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = 20;
			for (;;)
			{
				IntVec3 spawnCenter = parms.spawnCenter;
				if (!(parms.spawnCenter == IntVec3.Invalid) || num <= 0)
				{
					return false;
				}
				PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Clear();
				parms.spawnCenter = CellFinderLoose.RandomCellWith((IntVec3 cell) => (cell.GetTerrain(map).IsRiver || cell.GetTerrain(map).IsOcean) && cell.Walkable(map) && map.reachability.CanReachColony(cell), map, 1000);
				if (parms.spawnCenter != IntVec3.Invalid)
				{
					map.floodFiller.FloodFill(parms.spawnCenter, (IntVec3 cell) => cell.GetTerrain(map).IsWater && cell.Walkable(map) && cell.InHorDistOf(parms.spawnCenter, 30f), delegate(IntVec3 cell)
					{
						PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Add(cell);
					}, int.MaxValue, false, null);
					if (PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Count > 50)
					{
						break;
					}
				}
				num--;
			}
			return true;
		}

		// Token: 0x040050F8 RID: 20728
		private const int LargeBodyOfWaterThreshold = 50;

		// Token: 0x040050F9 RID: 20729
		private const float MaxSpawnRadius = 30f;

		// Token: 0x040050FA RID: 20730
		private const float SpawnDurationSeconds = 8f;

		// Token: 0x040050FB RID: 20731
		private const int SpawnDelayTicks = 40;

		// Token: 0x040050FC RID: 20732
		public static readonly FloatRange StunRange = new FloatRange(2f, 4f);

		// Token: 0x040050FD RID: 20733
		private static readonly HashSet<IntVec3> WaterBodyCells = new HashSet<IntVec3>();
	}
}
