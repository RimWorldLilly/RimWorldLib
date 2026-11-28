using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000933 RID: 2355
	public class PsychicRitualToil_SummonFleshbeastsPlayer : PsychicRitualToil
	{
		// Token: 0x0600481E RID: 18462 RVA: 0x001C4FEE File Offset: 0x001C31EE
		protected PsychicRitualToil_SummonFleshbeastsPlayer()
		{
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x001C4FF6 File Offset: 0x001C31F6
		public PsychicRitualToil_SummonFleshbeastsPlayer(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x001C5008 File Offset: 0x001C3208
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn == null)
			{
				return;
			}
			IntVec3 intVec = PsychicRitualToil_SummonFleshbeastsPlayer.FindSpawnCenter(psychicRitual, pawn);
			this.SpawnFleshbeasts(psychicRitual, intVec, pawn);
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "SummonFleshbeastsPlayerCompleteText".Translate(pawn, psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, new TargetInfo(intVec, psychicRitual.Map, false), null, null, null, null, 0, true);
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x001C50A0 File Offset: 0x001C32A0
		private static IntVec3 FindSpawnCenter(PsychicRitual psychicRitual, Pawn invoker)
		{
			IntVec3 intVec = IntVec3.Invalid;
			List<Pawn> list = (from t in psychicRitual.Map.attackTargetsCache.TargetsHostileToColony.Where(delegate(IAttackTarget t)
			{
				if (!t.Thing.Fogged())
				{
					Pawn pawn = t.Thing as Pawn;
					if (pawn != null && !t.ThreatDisabled(invoker))
					{
						return !pawn.IsOnHoldingPlatform;
					}
				}
				return false;
			})
			select t.Thing as Pawn).ToList<Pawn>();
			IEnumerable<IAttackTarget> source = from h in list
			where h.RaceProps.Humanlike && !h.IsSubhuman
			select h;
			if (!source.Any<IAttackTarget>())
			{
				source = from h in list
				where !h.IsSubhuman && !h.RaceProps.IsAnomalyEntity
				select h;
				if (!source.Any<IAttackTarget>())
				{
					source = list;
				}
			}
			IAttackTarget[] array = source.ToArray<IAttackTarget>();
			array.Shuffle<IAttackTarget>();
			intVec = PsychicRitualToil_SummonFleshbeastsPlayer.TryFindingValidHostileSpawnCenter(psychicRitual, array);
			if (intVec != IntVec3.Invalid)
			{
				return intVec;
			}
			IntVec3 result;
			CellFinder.TryFindRandomCell(psychicRitual.Map, (IntVec3 cell) => cell.Walkable(psychicRitual.Map) && !cell.Fogged(psychicRitual.Map) && !psychicRitual.Map.thingGrid.ThingsListAtFast(cell).Any<Thing>(), out result);
			return result;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x001C51C4 File Offset: 0x001C33C4
		private static IntVec3 TryFindingValidHostileSpawnCenter(PsychicRitual psychicRitual, IAttackTarget[] hostiles)
		{
			int num = 0;
			while (num < hostiles.Length && num <= 4)
			{
				int num2 = 0;
				IntVec3 positionHeld = hostiles[num].Thing.PositionHeld;
				Map map = psychicRitual.Map;
				CellRect cellRect = CellRect.CenteredOn(positionHeld, 10);
				cellRect.ClipInsideMap(map);
				foreach (IntVec3 intVec in cellRect)
				{
					if (intVec != positionHeld && intVec.Walkable(map) && !intVec.Fogged(map))
					{
						num2++;
					}
				}
				if (num2 >= 5)
				{
					return positionHeld;
				}
				num++;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x001C5280 File Offset: 0x001C3480
		private void SpawnFleshbeasts(PsychicRitual psychicRitual, IntVec3 spawnCenter, Pawn invoker)
		{
			float num = ((PsychicRitualDef_SummonFleshbeastsPlayer)psychicRitual.def).fleshbeastCombatPointsFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			int num2 = 0;
			while (num > 0f)
			{
				IntVec3 intVec = IntVec3.Invalid;
				IntVec3 intVec2;
				if (LargeBuildingCellFinder.TryFindCellNear(spawnCenter, psychicRitual.Map, 10, PsychicRitualToil_SummonFleshbeastsPlayer.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow), out intVec2, false) && intVec2 != spawnCenter)
				{
					intVec = intVec2;
				}
				if (intVec != IntVec3.Invalid)
				{
					float num3 = Mathf.Min(num, 500f);
					num -= num3;
					FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(intVec, psychicRitual.Map, num3, PsychicRitualToil_SummonFleshbeastsPlayer.PitBurrowEmergenceDelayRangeTicks, PsychicRitualToil_SummonFleshbeastsPlayer.FleshbeastSpawnDelayTicks, false);
				}
				num2++;
				if (num2 > 100)
				{
					break;
				}
			}
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x001C5331 File Offset: 0x001C3531
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E98 RID: 11928
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E99 RID: 11929
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x04002E9A RID: 11930
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x04002E9B RID: 11931
		private const int MaxIterations = 100;

		// Token: 0x04002E9C RID: 11932
		private const int SpawnRadius = 10;

		// Token: 0x04002E9D RID: 11933
		private const int MinWalkableCells = 5;

		// Token: 0x04002E9E RID: 11934
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
