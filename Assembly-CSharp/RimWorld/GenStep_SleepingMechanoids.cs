using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012FB RID: 4859
	public class GenStep_SleepingMechanoids : GenStep
	{
		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x06007882 RID: 30850 RVA: 0x002AA5A6 File Offset: 0x002A87A6
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x002AA5B0 File Offset: 0x002A87B0
		public static void SendMechanoidsToSleepImmediately(List<Pawn> spawnedMechanoids)
		{
			for (int i = 0; i < spawnedMechanoids.Count; i++)
			{
				spawnedMechanoids[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				JobDriver curDriver = spawnedMechanoids[i].jobs.curDriver;
				if (curDriver != null)
				{
					curDriver.asleep = true;
				}
				CompCanBeDormant comp = spawnedMechanoids[i].GetComp<CompCanBeDormant>();
				if (comp != null)
				{
					comp.ToSleep();
				}
				else
				{
					string str = "Tried spawning sleeping mechanoid ";
					Pawn pawn = spawnedMechanoids[i];
					Log.ErrorOnce(str + ((pawn != null) ? pawn.ToString() : null) + " without CompCanBeDormant!", 317364857 ^ spawnedMechanoids[i].def.defName.GetHashCode());
				}
			}
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x002AA660 File Offset: 0x002A8860
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect around;
			IntVec3 near;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out around, out near, map))
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in this.GeneratePawns(parms, map))
			{
				IntVec3 loc;
				if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(around, near, map, out loc))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					break;
				}
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				list.Add(pawn);
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			bool @bool = Rand.Bool;
			foreach (Pawn pawn2 in list)
			{
				CompWakeUpDormant comp = pawn2.GetComp<CompWakeUpDormant>();
				if (comp != null)
				{
					comp.wakeUpIfTargetClose = @bool;
				}
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, true), map, list);
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list);
		}

		// Token: 0x06007885 RID: 30853 RVA: 0x002AA768 File Offset: 0x002A8968
		private IEnumerable<Pawn> GeneratePawns(GenStepParams parms, Map map)
		{
			float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Faction.OfMechanoids,
				points = points
			};
			if (parms.sitePart != null)
			{
				pawnGroupMakerParms.seed = new int?(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms));
			}
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
		}

		// Token: 0x04004B90 RID: 19344
		public FloatRange defaultPointsRange = new FloatRange(340f, 1000f);
	}
}
