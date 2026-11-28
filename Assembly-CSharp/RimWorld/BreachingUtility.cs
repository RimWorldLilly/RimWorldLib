using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000ACA RID: 2762
	public static class BreachingUtility
	{
		// Token: 0x060052D4 RID: 21204 RVA: 0x001EDE8C File Offset: 0x001EC08C
		public static bool ShouldBreachBuilding(Thing thing)
		{
			Building building = thing as Building;
			return building != null && TrashUtility.ShouldTrashBuilding(building) && PathUtility.IsDestroyable(building) && building.def.IsEdifice() && !building.def.IsFrame && (building.def.mineable || ((building.Faction == null || building.Faction == Faction.OfPlayer) && BreachingUtility.BlocksBreaching(building.Map, building.Position)));
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x001EDF10 File Offset: 0x001EC110
		public static bool IsWorthBreachingBuilding(BreachingGrid grid, Building b)
		{
			if (b.def.passability != Traversability.Impassable || b.def.IsDoor)
			{
				return true;
			}
			if (BreachingUtility.OnEdgeOfPath(b.Map, grid.BreachGrid, b.Position))
			{
				if (BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.North) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.East) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.South) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.West))
				{
					return false;
				}
			}
			else if (!BreachingUtility.AnyAdjacentImpassibleOnBreachPath(b.Map, grid.BreachGrid, b.Position))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x001EDFC0 File Offset: 0x001EC1C0
		public static int CountReachableAdjacentCells(BreachingGrid grid, Building b)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = b.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(grid.Map) && grid.ReachableGrid[c])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001EE014 File Offset: 0x001EC214
		private static bool OnEdgeOfPath(Map map, BoolGrid grid, IntVec3 p)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = p + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && !grid[c])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001EE054 File Offset: 0x001EC254
		private static bool OnEdgeOfPathAndSufficientlyClear(BreachingGrid breachingGrid, IntVec3 p, IntVec3 right)
		{
			Map map = breachingGrid.Map;
			BoolGrid breachGrid = breachingGrid.BreachGrid;
			int num = breachingGrid.BreachRadius * 2 + 1;
			IntVec3 c = p - right;
			if (!c.InBounds(map) || breachGrid[c])
			{
				return false;
			}
			IntVec3 intVec = p;
			int num2 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
			}
			int num3 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && !BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
				num3++;
			}
			int num4 = Math.Max(2, num - 2);
			return num3 >= num4;
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x001EE124 File Offset: 0x001EC324
		public static bool BlocksBreaching(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			return (edifice == null || !edifice.def.building.ai_neverTrashThis) && (c.Impassable(map) || c.GetDoor(map) != null);
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x001EE168 File Offset: 0x001EC368
		private static bool AnyAdjacentImpassibleOnBreachPath(Map map, BoolGrid grid, IntVec3 position)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = position + GenAdj.AdjacentCellsAround[i];
				if (c.InBounds(map) && grid[c] && BreachingUtility.BlocksBreaching(map, c))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001EE1B1 File Offset: 0x001EC3B1
		public static bool CanSoloAttackTargetBuilding(Thing thing)
		{
			return thing != null && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001EE1C8 File Offset: 0x001EC3C8
		public static bool IsSoloAttackVerb(Verb verb)
		{
			return verb.verbProps.ai_AvoidFriendlyFireRadius > 0f;
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x001EE1DC File Offset: 0x001EC3DC
		public static float EscortRadius(Pawn pawn)
		{
			float randomInRange;
			if (pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
			{
				randomInRange = BreachingUtility.EscortRadiusRanged.RandomInRange;
			}
			else
			{
				randomInRange = BreachingUtility.EscortRadiusMelee.RandomInRange;
			}
			return randomInRange;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x001EE234 File Offset: 0x001EC434
		private static bool UsableVerb(Verb verb)
		{
			return verb != null && verb.Available() && verb.HarmsHealth();
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001EE250 File Offset: 0x001EC450
		public static Verb FindVerbToUseForBreaching(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			CompEquippable compEquippable = (equipment != null) ? equipment.PrimaryEq : null;
			if (compEquippable == null)
			{
				return null;
			}
			Verb primaryVerb = compEquippable.PrimaryVerb;
			if (BreachingUtility.UsableVerb(primaryVerb) && primaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return primaryVerb;
			}
			List<Verb> allVerbs = compEquippable.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				if (BreachingUtility.UsableVerb(verb) && verb.verbProps.ai_IsBuildingDestroyer)
				{
					return verb;
				}
			}
			if (BreachingUtility.UsableVerb(primaryVerb))
			{
				return primaryVerb;
			}
			return null;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001EE2DC File Offset: 0x001EC4DC
		public static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = BreachingUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x001EE30A File Offset: 0x001EC50A
		public static BreachingGrid BreachingGridFor(Pawn pawn)
		{
			return BreachingUtility.BreachingGridFor((pawn != null) ? pawn.GetLord() : null);
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x001EE31D File Offset: 0x001EC51D
		public static BreachingGrid BreachingGridFor(Lord lord)
		{
			LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(lord);
			if (lordToilData_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToilData_AssaultColonyBreaching.breachingGrid;
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x001EE330 File Offset: 0x001EC530
		public static LordToilData_AssaultColonyBreaching LordDataFor(Lord lord)
		{
			LordToil_AssaultColonyBreaching lordToil_AssaultColonyBreaching = ((lord != null) ? lord.CurLordToil : null) as LordToil_AssaultColonyBreaching;
			if (lordToil_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToil_AssaultColonyBreaching.Data;
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x001EE34E File Offset: 0x001EC54E
		public static LordToil_AssaultColonyBreaching LordToilOf(Pawn pawn)
		{
			return ((pawn != null) ? pawn.GetLord().CurLordToil : null) as LordToil_AssaultColonyBreaching;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x001EE366 File Offset: 0x001EC566
		public static bool TryFindCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			if (verb.IsMeleeAttack)
			{
				return BreachingUtility.TryFindMeleeCastPosition(pawn, verb, target, out result);
			}
			return BreachingUtility.cachedRangedCastPositionFinder.TryFindRangedCastPosition(pawn, verb, target, out result);
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x001EE388 File Offset: 0x001EC588
		private static bool TryFindMeleeCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			result = IntVec3.Invalid;
			BreachingGrid breachingGrid = BreachingUtility.BreachingGridFor(pawn);
			if (breachingGrid == null)
			{
				return false;
			}
			if (BreachingUtility.SafeUseableFiringPosition(breachingGrid, pawn.Position) && pawn.CanReachImmediate(target, PathEndMode.Touch))
			{
				result = pawn.Position;
				return true;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = GenAdj.AdjacentCells[i] + target.Position;
				if (intVec.InBounds(target.Map) && BreachingUtility.SafeUseableFiringPosition(breachingGrid, intVec) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					result = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x001EE42E File Offset: 0x001EC62E
		private static bool SafeUseableFiringPosition(BreachingGrid grid, IntVec3 c)
		{
			return grid.ReachableGrid[c] && grid.MarkerGrid[c] != 180 && !c.ContainsStaticFire(grid.Map);
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x001EE468 File Offset: 0x001EC668
		public static Pawn FindPawnToEscort(Pawn follower)
		{
			Lord lord = follower.GetLord();
			if (lord == null)
			{
				return null;
			}
			Pawn result = null;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn != follower)
				{
					PawnDuty duty = pawn.mindState.duty;
					if (((duty != null) ? duty.def : null) == DutyDefOf.Breaching)
					{
						BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
						if (breachingTarget != null && breachingTarget.firingPosition.IsValid)
						{
							return pawn;
						}
						result = pawn;
					}
				}
			}
			return result;
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x001EE4EC File Offset: 0x001EC6EC
		public static bool CanDamageTarget(Verb verb, Thing target)
		{
			return verb.GetDamageDef() != DamageDefOf.Flame || target.FlammableNow;
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x001EE508 File Offset: 0x001EC708
		public static bool IsGoodBreacher(Pawn pawn)
		{
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			return verb != null && verb.verbProps.ai_IsBuildingDestroyer;
		}

		// Token: 0x040031E9 RID: 12777
		public static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(120, 300);

		// Token: 0x040031EA RID: 12778
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		// Token: 0x040031EB RID: 12779
		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);

		// Token: 0x040031EC RID: 12780
		private static BreachingUtility.BreachRangedCastPositionFinder cachedRangedCastPositionFinder = new BreachingUtility.BreachRangedCastPositionFinder();

		// Token: 0x02002DB3 RID: 11699
		private class BreachRangedCastPositionFinder
		{
			// Token: 0x0601162A RID: 71210 RVA: 0x006006C1 File Offset: 0x005FE8C1
			public BreachRangedCastPositionFinder()
			{
				this.visitDangerousCellFunc = new Action<IntVec3>(this.VisitDangerousCell);
				this.safeForRangedCastFunc = new Func<IntVec3, bool>(this.SafeForRangedCast);
			}

			// Token: 0x0601162B RID: 71211 RVA: 0x006006ED File Offset: 0x005FE8ED
			private void VisitDangerousCell(IntVec3 cell)
			{
				if (this.breachingGrid.MarkerGrid[cell] == 180)
				{
					this.wouldPutSomeoneElseInDanger = true;
				}
			}

			// Token: 0x0601162C RID: 71212 RVA: 0x00600710 File Offset: 0x005FE910
			private bool SafeForRangedCast(IntVec3 c)
			{
				if (!BreachingUtility.SafeUseableFiringPosition(this.breachingGrid, c))
				{
					return false;
				}
				this.wouldPutSomeoneElseInDanger = false;
				this.breachingGrid.VisitDangerousCellsOfAttack(c, this.target.Position, this.verb, this.visitDangerousCellFunc);
				return !this.wouldPutSomeoneElseInDanger;
			}

			// Token: 0x0601162D RID: 71213 RVA: 0x00600760 File Offset: 0x005FE960
			public bool TryFindRangedCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
			{
				bool result2;
				try
				{
					result = IntVec3.Invalid;
					LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(pawn.GetLord());
					if (lordToilData_AssaultColonyBreaching == null)
					{
						result2 = false;
					}
					else
					{
						this.breachingGrid = lordToilData_AssaultColonyBreaching.breachingGrid;
						this.verb = verb;
						this.target = target;
						CastPositionRequest castPositionRequest = default(CastPositionRequest);
						castPositionRequest.caster = pawn;
						castPositionRequest.target = target;
						castPositionRequest.verb = verb;
						castPositionRequest.maxRangeFromTarget = verb.EffectiveRange;
						if (lordToilData_AssaultColonyBreaching.soloAttacker == null)
						{
							castPositionRequest.maxRangeFromTarget = Mathf.Min(lordToilData_AssaultColonyBreaching.maxRange, castPositionRequest.maxRangeFromTarget);
						}
						castPositionRequest.validator = this.safeForRangedCastFunc;
						IntVec3 intVec;
						if (CastPositionFinder.TryFindCastPosition(castPositionRequest, out intVec))
						{
							result = intVec;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
					}
				}
				finally
				{
				}
				return result2;
			}

			// Token: 0x0400B9EC RID: 47596
			private BreachingGrid breachingGrid;

			// Token: 0x0400B9ED RID: 47597
			private Verb verb;

			// Token: 0x0400B9EE RID: 47598
			private Thing target;

			// Token: 0x0400B9EF RID: 47599
			private bool wouldPutSomeoneElseInDanger;

			// Token: 0x0400B9F0 RID: 47600
			private Action<IntVec3> visitDangerousCellFunc;

			// Token: 0x0400B9F1 RID: 47601
			private Func<IntVec3, bool> safeForRangedCastFunc;
		}
	}
}
