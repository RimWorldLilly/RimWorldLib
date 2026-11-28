using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119A RID: 4506
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x060070C1 RID: 28865 RVA: 0x002776E0 File Offset: 0x002758E0
		private int RandomCountToDrop
		{
			get
			{
				float x2 = (float)Find.TickManager.TicksGame / 3600000f;
				float timePassedFactor = Mathf.Clamp(GenMath.LerpDouble(0f, 1.2f, 1f, 0.1f, x2), 0.1f, 1f);
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight(delegate(Pair<int, float> x)
				{
					if (x.First == 1)
					{
						return x.Second;
					}
					return x.Second * timePassedFactor;
				}).First;
			}
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x00277754 File Offset: 0x00275954
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x0027778C File Offset: 0x0027598C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				return false;
			}
			this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
			Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageTypeDefOf.NeutralEvent, true);
			return true;
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x002777F4 File Offset: 0x002759F4
		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos;
				if (this.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x0027782C File Offset: 0x00275A2C
		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x00277840 File Offset: 0x00275A40
		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.ShipChunkIncoming, map, ThingDefOf.ShipChunk.terrainAffordanceNeeded, out pos, 10, nearLoc, maxDist, true, false, false, false, true, false, null);
		}

		// Token: 0x040047BC RID: 18364
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};
	}
}
