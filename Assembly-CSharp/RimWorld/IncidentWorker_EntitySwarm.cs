using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001174 RID: 4468
	public abstract class IncidentWorker_EntitySwarm : IncidentWorker
	{
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06007024 RID: 28708 RVA: 0x002743DF File Offset: 0x002725DF
		protected virtual FloatRange SwarmSizeVariance { get; } = new FloatRange(0.7f, 1.3f);

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06007025 RID: 28709
		protected abstract PawnGroupKindDef GroupKindDef { get; }

		// Token: 0x06007026 RID: 28710 RVA: 0x002743E7 File Offset: 0x002725E7
		protected virtual float TransformPoints(float points)
		{
			return points;
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x002743EA File Offset: 0x002725EA
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.AnomalyActive && parms.points >= Faction.OfEntities.def.MinPointsToGeneratePawnGroup(this.GroupKindDef, null) && base.CanFireNowSub(parms);
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x0027441C File Offset: 0x0027261C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			int num = 0;
			IntVec3 invalid = IntVec3.Invalid;
			IntVec3 invalid2 = IntVec3.Invalid;
			bool flag = false;
			while (!flag && num < 5)
			{
				RCellFinder.TryFindRandomPawnEntryCell(out invalid, map, CellFinder.EdgeRoadChance_Hostile, false, null);
				flag = RCellFinder.TryFindTravelDestFrom(invalid, map, out invalid2);
				num++;
			}
			if (!flag)
			{
				return false;
			}
			parms.spawnCenter = invalid;
			List<Pawn> list = this.GenerateEntities(parms, this.TransformPoints(parms.points));
			if (list.NullOrEmpty<Pawn>())
			{
				return false;
			}
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			foreach (Pawn pawn in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map, 10, null);
				GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false, false);
				QuestUtility.AddQuestTag(pawn, parms.questTag);
			}
			this.SendLetter(parms, list);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LordMaker.MakeNewLord(parms.faction, this.GenerateLordJob(invalid, invalid2), map, list);
			return true;
		}

		// Token: 0x06007029 RID: 28713
		protected abstract LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest);

		// Token: 0x0600702A RID: 28714 RVA: 0x00274594 File Offset: 0x00272794
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			base.SendStandardLetter(parms, entities, Array.Empty<NamedArgument>());
		}

		// Token: 0x0600702B RID: 28715 RVA: 0x002745A8 File Offset: 0x002727A8
		protected virtual List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			Map map = (Map)parms.target;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = this.GroupKindDef,
				tile = map.Tile,
				faction = Faction.OfEntities,
				points = points * this.SwarmSizeVariance.RandomInRange
			};
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true).ToList<Pawn>();
		}
	}
}
