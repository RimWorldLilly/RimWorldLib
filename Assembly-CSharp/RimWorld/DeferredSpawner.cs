using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200120C RID: 4620
	public class DeferredSpawner : IExposable
	{
		// Token: 0x060072BD RID: 29373 RVA: 0x00281DCE File Offset: 0x0027FFCE
		public DeferredSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x060072BE RID: 29374 RVA: 0x00281DE8 File Offset: 0x0027FFE8
		public void DeferredSpawnerTick()
		{
			this.requests.RemoveWhere((SpawnRequest r) => r.done);
			foreach (SpawnRequest request in this.requests)
			{
				this.TickRequest(request);
			}
		}

		// Token: 0x060072BF RID: 29375 RVA: 0x00281E68 File Offset: 0x00280068
		public void AddRequest(SpawnRequest request, bool spawnFirstBatchImmediately = true)
		{
			if (request.spawnPositions.Count != request.unspawnedThings.Count)
			{
				Log.Error("Spawn positions count does not match things to spawn count!");
				return;
			}
			this.requests.Add(request);
			if (spawnFirstBatchImmediately)
			{
				this.SpawnBatch(request, request.unspawnedThings.Count);
			}
		}

		// Token: 0x060072C0 RID: 29376 RVA: 0x00281EBC File Offset: 0x002800BC
		public SpawnRequest GetRequestByLord(Lord lord)
		{
			return this.requests.FirstOrDefault((SpawnRequest r) => r.lord == lord);
		}

		// Token: 0x060072C1 RID: 29377 RVA: 0x00281EF0 File Offset: 0x002800F0
		private void TickRequest(SpawnRequest request)
		{
			int count = request.unspawnedThings.Count;
			if (request.preSpawnEffect != null && Find.TickManager.TicksGame >= request.PreEffecterTick && this.map.IsHashIntervalTick(request.intervalTicks, request.preSpawnEffecterOffsetTicks) && count > 0)
			{
				this.SpawnPreSpawnEffecter(request, count);
			}
			if (Find.TickManager.TicksGame < request.startedTick + request.initialDelay)
			{
				return;
			}
			if (this.map.IsHashIntervalTick(request.intervalTicks) && count > 0)
			{
				this.SpawnBatch(request, count);
			}
		}

		// Token: 0x060072C2 RID: 29378 RVA: 0x00281F84 File Offset: 0x00280184
		private void SpawnPreSpawnEffecter(SpawnRequest request, int spawnsLeft)
		{
			for (int i = Mathf.Min(request.batchSize, spawnsLeft) - 1; i >= 0; i--)
			{
				IntVec3 target = request.spawnPositions[i];
				EffecterDef preSpawnEffect = request.preSpawnEffect;
				if (preSpawnEffect != null)
				{
					preSpawnEffect.SpawnMaintained(target, this.map, 1f);
				}
			}
		}

		// Token: 0x060072C3 RID: 29379 RVA: 0x00281FD8 File Offset: 0x002801D8
		private void SpawnBatch(SpawnRequest request, int spawnsLeft)
		{
			for (int i = Mathf.Min(request.batchSize, spawnsLeft) - 1; i >= 0; i--)
			{
				Thing thing = request.unspawnedThings[i];
				IntVec3 loc = request.spawnPositions[i];
				Thing thing2 = GenSpawn.Spawn(thing, loc, this.map, Rot4.Random, WipeMode.Vanish, false, false);
				request.unspawnedThings.RemoveAt(i);
				request.spawnPositions.RemoveAt(i);
				request.spawnedThings.Add(thing);
				Pawn pawn;
				if ((pawn = (thing2 as Pawn)) == null)
				{
					IThingHolder thingHolder = thing2 as IThingHolder;
					pawn = ((thingHolder != null && thingHolder.GetDirectlyHeldThings().Count > 0) ? (thingHolder.GetDirectlyHeldThings()[0] as Pawn) : null);
				}
				Pawn pawn2 = pawn;
				if (pawn2 != null)
				{
					if (request.lord != null)
					{
						Lord lord = pawn2.GetLord();
						if (lord != null)
						{
							lord.RemovePawn(pawn2);
						}
						Lord lord2 = request.lord;
						if (lord2 != null)
						{
							lord2.AddPawn(pawn2);
						}
					}
					Pawn_RotationTracker rotationTracker = pawn2.rotationTracker;
					if (rotationTracker != null)
					{
						rotationTracker.FaceCell(pawn2.Map.Center);
					}
				}
				DeferredSpawnWorker spawnWorker = request.spawnWorker;
				if (spawnWorker != null)
				{
					spawnWorker.OnSpawn(thing2);
				}
				SoundDef spawnSound = request.spawnSound;
				if (spawnSound != null)
				{
					spawnSound.PlayOneShot(thing2);
				}
				EffecterDef spawnEffect = request.spawnEffect;
				if (spawnEffect != null)
				{
					spawnEffect.SpawnMaintained(thing2, thing2.Map, 1f);
				}
			}
			if (request.unspawnedThings.Count == 0)
			{
				request.done = true;
			}
		}

		// Token: 0x060072C4 RID: 29380 RVA: 0x00282140 File Offset: 0x00280340
		public void Notify_MapRemoved()
		{
			foreach (SpawnRequest spawnRequest in this.requests)
			{
				foreach (Thing thing in spawnRequest.unspawnedThings)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
					IThingHolder thingHolder = thing as IThingHolder;
					ThingOwner thingOwner = (thingHolder != null) ? thingHolder.GetDirectlyHeldThings() : null;
					if (thingOwner != null && thingOwner.Count > 0)
					{
						Pawn pawn2 = thingOwner[0] as Pawn;
						if (pawn2 != null)
						{
							Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Decide);
						}
					}
				}
			}
		}

		// Token: 0x060072C5 RID: 29381 RVA: 0x00282220 File Offset: 0x00280420
		public void ExposeData()
		{
			Scribe_Collections.Look<SpawnRequest>(ref this.requests, "requests", LookMode.Deep);
		}

		// Token: 0x0400488D RID: 18573
		public Map map;

		// Token: 0x0400488E RID: 18574
		private HashSet<SpawnRequest> requests = new HashSet<SpawnRequest>();
	}
}
