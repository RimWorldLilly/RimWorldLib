using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001483 RID: 5251
	public class CreepJoinerWorker_SightstealerAttack : BaseCreepJoinerWorker
	{
		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x060080E3 RID: 32995 RVA: 0x002DFBFD File Offset: 0x002DDDFD
		public override bool CanOccurOnDeath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060080E4 RID: 32996 RVA: 0x002DFC00 File Offset: 0x002DDE00
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			if (!base.Pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (!base.Pawn.Dead)
			{
				base.Pawn.SetFaction(Find.FactionManager.OfEntities, null);
				base.Pawn.guest.Recruitable = false;
				if (base.Pawn.GetLord() != null)
				{
					base.Pawn.GetLord().Notify_PawnLost(base.Pawn, PawnLostCondition.Undefined, null);
				}
				LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), base.Pawn.MapHeld, null).AddPawn(base.Pawn);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_SightstealerAssault(), base.Pawn.MapHeld, null);
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Pawn.MapHeld) * CreepJoinerWorker_SightstealerAttack.SpawnPointScale.RandomInRange;
			num = Mathf.Max(Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Sightstealers, null), num);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = Faction.OfEntities,
				groupKind = PawnGroupKindDefOf.Sightstealers,
				points = num,
				tile = base.Pawn.MapHeld.Tile
			}, true).ToList<Pawn>();
			foreach (Pair<List<Pawn>, IntVec3> pair in PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(list, base.Pawn.MapHeld, false))
			{
				foreach (Thing newThing in pair.First)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(pair.Second, base.Pawn.MapHeld, 8, null);
					GenSpawn.Spawn(newThing, loc, base.Pawn.MapHeld, WipeMode.Vanish);
				}
			}
			foreach (Pawn p in list)
			{
				lord.AddPawn(p);
			}
			SoundDefOf.Sightstealer_SummonedHowl.PlayOneShot(base.Pawn);
		}

		// Token: 0x04005039 RID: 20537
		private static readonly FloatRange SpawnPointScale = new FloatRange(0.25f, 0.65f);
	}
}
