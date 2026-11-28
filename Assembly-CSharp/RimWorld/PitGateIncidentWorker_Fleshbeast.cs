using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011C3 RID: 4547
	public class PitGateIncidentWorker_Fleshbeast : PitGateIncidentWorker
	{
		// Token: 0x0600718D RID: 29069 RVA: 0x0027C9F8 File Offset: 0x0027ABF8
		public override void Setup(float points)
		{
			base.Setup(points);
			List<Pawn> fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(points, this.pitGate.Map, false);
			List<PawnFlyer> list = new List<PawnFlyer>();
			List<IntVec3> list2 = new List<IntVec3>();
			CellRect cellRect = GenAdj.OccupiedRect(this.pitGate.Position, Rot4.North, ThingDefOf.PitGate.Size).ContractedBy(2);
			foreach (Pawn pawn in fleshbeastsForPoints)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				GenSpawn.Spawn(pawn, randomCell, this.pitGate.Map, WipeMode.Vanish);
				IntVec3 intVec;
				CellFinder.TryFindRandomCellNear(this.pitGate.Position, this.pitGate.Map, ThingDefOf.PitGate.size.x / 2 + 1, (IntVec3 cell) => !cell.Fogged(this.pitGate.Map) && cell.Walkable(this.pitGate.Map) && !cell.Impassable(this.pitGate.Map), out intVec, -1);
				pawn.rotationTracker.FaceCell(intVec);
				list.Add(PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, new Vector3?(randomCell.ToVector3() + new Vector3(0f, 0f, -1f)), null, default(LocalTargetInfo)));
				list2.Add(randomCell);
			}
			float intervalSeconds = 600.TicksToSeconds() / (float)fleshbeastsForPoints.Count;
			SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList<Thing>(), list2, 1, intervalSeconds, null);
			spawnRequest.lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), this.pitGate.Map, null);
			this.pitGate.Map.deferredSpawner.AddRequest(spawnRequest, true);
			SoundDefOf.Pawn_Fleshbeast_EmergeFromPitGate.PlayOneShot(this.pitGate);
		}

		// Token: 0x040047D2 RID: 18386
		private const int FleshBeastEmergenceDuration = 600;

		// Token: 0x040047D3 RID: 18387
		private Lord lord;
	}
}
