using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA8 RID: 2728
	public class Toils_Ingest
	{
		// Token: 0x0600522C RID: 21036 RVA: 0x001E9A7C File Offset: 0x001E7C7C
		public static Toil TakeMealFromDispenser(TargetIndex ind, Pawn eater)
		{
			Toil toil = ToilMaker.MakeToil("TakeMealFromDispenser");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = ((Building_NutrientPasteDispenser)actor.jobs.curJob.GetTarget(ind).Thing).TryDispenseFood();
				if (thing == null)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				actor.carryTracker.TryStartCarry(thing);
				actor.CurJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.FailOnCannotTouch(ind, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = Building_NutrientPasteDispenser.CollectDuration;
			return toil;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x001E9AF4 File Offset: 0x001E7CF4
		public static Toil PickupIngestible(TargetIndex ind, Pawn eater)
		{
			Toil toil = ToilMaker.MakeToil("PickupIngestible");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				if (curJob.count <= 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to do PickupIngestible toil with job.count = ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(curJob.count);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					return;
				}
				int count = Mathf.Min(thing.stackCount, curJob.count);
				actor.carryTracker.TryStartCarry(thing, count, true);
				if (thing != actor.carryTracker.CarriedThing && actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
				{
					actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				actor.jobs.curJob.targetA = actor.carryTracker.CarriedThing;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x001E9B48 File Offset: 0x001E7D48
		public static Toil CarryIngestibleToChewSpot(Pawn pawn, TargetIndex ingestibleInd)
		{
			Toil toil = ToilMaker.MakeToil("CarryIngestibleToChewSpot");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.CurJob.GetTarget(ingestibleInd).Thing;
				IntVec3 intVec;
				if (!Toils_Ingest.TryFindChairOrSpot(actor, thing, out intVec))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Can't find valid chair or spot for ");
					defaultInterpolatedStringHandler.AppendFormatted<Pawn>(actor);
					defaultInterpolatedStringHandler.AppendLiteral(" trying to ingest ");
					defaultInterpolatedStringHandler.AppendFormatted<Thing>(thing);
					Log.WarningOnce(defaultInterpolatedStringHandler.ToStringAndClear(), HashCode.Combine<Pawn, Thing>(actor, thing));
					actor.pather.StartPath(actor.Position, PathEndMode.OnCell);
					return;
				}
				actor.ReserveSittableOrSpot(intVec, actor.CurJob, true);
				actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, intVec);
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x001E9B9C File Offset: 0x001E7D9C
		public static bool TryFindChairOrSpot(Pawn pawn, Thing ingestible, out IntVec3 cell)
		{
			Toils_Ingest.<>c__DisplayClass4_0 CS$<>8__locals1 = new Toils_Ingest.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			cell = IntVec3.Invalid;
			Thing thing = null;
			float num = 0f;
			ThingDef def = ingestible.def;
			IngestibleProperties ingestibleProperties = (def != null) ? def.ingestible : null;
			if (ingestibleProperties != null)
			{
				float chairSearchRadius = ingestibleProperties.chairSearchRadius;
				num = chairSearchRadius;
			}
			else
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = ingestible as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					ThingDef dispensableDef = building_NutrientPasteDispenser.DispensableDef;
					if (dispensableDef != null)
					{
						ingestibleProperties = dispensableDef.ingestible;
						if (ingestibleProperties != null)
						{
							float chairSearchRadius2 = ingestibleProperties.chairSearchRadius;
							num = chairSearchRadius2;
						}
					}
				}
			}
			if (num > 0f)
			{
				thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), num, (Thing t) => base.<TryFindChairOrSpot>g__BaseChairValidator|0(t) && t.Position.GetDangerFor(CS$<>8__locals1.pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false, false);
			}
			if (thing == null)
			{
				cell = RCellFinder.SpotToChewStandingNear(CS$<>8__locals1.pawn, ingestible, (IntVec3 c) => CS$<>8__locals1.pawn.CanReserveSittableOrSpot(c, false));
				Danger chewSpotDanger = cell.GetDangerFor(CS$<>8__locals1.pawn, CS$<>8__locals1.pawn.Map);
				if (chewSpotDanger != Danger.None)
				{
					thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), num, (Thing t) => CS$<>8__locals1.<TryFindChairOrSpot>g__BaseChairValidator|0(t) && t.Position.GetDangerFor(CS$<>8__locals1.pawn, t.Map) <= chewSpotDanger, null, 0, -1, false, RegionType.Set_Passable, false, false);
				}
			}
			if (thing != null && !Toils_Ingest.TryFindFreeSittingSpotOnThing(thing, CS$<>8__locals1.pawn, out cell))
			{
				Log.Error("Could not find sitting spot on chewing chair! This is not supposed to happen - we looked for a free spot in a previous check!");
			}
			return cell.IsValid;
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x001E9D68 File Offset: 0x001E7F68
		public static bool TryFindFreeSittingSpotOnThing(Thing t, Pawn pawn, out IntVec3 cell)
		{
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if (pawn.CanReserveSittableOrSpot(intVec, false))
				{
					cell = intVec;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x001E9DDC File Offset: 0x001E7FDC
		public static Toil ReserveFoodFromStackForIngesting(TargetIndex ind, Pawn ingester = null)
		{
			Toil toil = ToilMaker.MakeToil("ReserveFoodFromStackForIngesting");
			toil.initAction = delegate()
			{
				if (ingester == null)
				{
					ingester = toil.actor;
				}
				int num = -1;
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.IngestibleNow)
				{
					int b = FoodUtility.WillIngestStackCountOf(ingester, target.Thing.def, FoodUtility.NutritionForEater(ingester, target.Thing));
					num = Mathf.Min(target.Thing.stackCount, b);
				}
				if (num == -1 || !target.HasThing || !toil.actor.CanReserve(target, 10, num, null, false))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.Reserve(target, toil.actor.CurJob, 10, num, null, true, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x001E9E44 File Offset: 0x001E8044
		public static bool TryFindAdjacentIngestionPlaceSpot(IntVec3 root, ThingDef ingestibleDef, Pawn pawn, out IntVec3 placeSpot)
		{
			placeSpot = IntVec3.Invalid;
			Func<Thing, bool> <>9__0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirections[i];
				if (intVec.HasEatSurface(pawn.Map))
				{
					IEnumerable<Thing> source = pawn.Map.thingGrid.ThingsAt(intVec);
					Func<Thing, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Thing t) => t.def == ingestibleDef));
					}
					if (!source.Any(predicate) && !intVec.IsForbidden(pawn))
					{
						placeSpot = intVec;
						return true;
					}
				}
			}
			if (!placeSpot.IsValid)
			{
				Toils_Ingest.spotSearchList.Clear();
				Toils_Ingest.cardinals.Shuffle<IntVec3>();
				for (int j = 0; j < 4; j++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.cardinals[j]);
				}
				Toils_Ingest.diagonals.Shuffle<IntVec3>();
				for (int k = 0; k < 4; k++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.diagonals[k]);
				}
				Toils_Ingest.spotSearchList.Add(IntVec3.Zero);
				Func<Thing, bool> <>9__1;
				for (int l = 0; l < Toils_Ingest.spotSearchList.Count; l++)
				{
					IntVec3 intVec2 = root + Toils_Ingest.spotSearchList[l];
					if (intVec2.Walkable(pawn.Map) && !intVec2.IsForbidden(pawn))
					{
						IEnumerable<Thing> source2 = pawn.Map.thingGrid.ThingsAt(intVec2);
						Func<Thing, bool> predicate2;
						if ((predicate2 = <>9__1) == null)
						{
							predicate2 = (<>9__1 = ((Thing t) => t.def == ingestibleDef));
						}
						if (!source2.Any(predicate2))
						{
							placeSpot = intVec2;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001E9FF0 File Offset: 0x001E81F0
		public static Toil FindAdjacentEatSurface(TargetIndex eatSurfaceInd, TargetIndex foodInd)
		{
			Toil toil = ToilMaker.MakeToil("FindAdjacentEatSurface");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 position = actor.Position;
				Map map = actor.Map;
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					IntVec3 intVec = position + rot.FacingCell;
					if (intVec.HasEatSurface(map))
					{
						toil.actor.CurJob.SetTarget(eatSurfaceInd, intVec);
						toil.actor.jobs.curDriver.rotateToFace = eatSurfaceInd;
						Thing thing = toil.actor.CurJob.GetTarget(foodInd).Thing;
						if (thing.def.rotatable)
						{
							thing.Rotation = Rot4.FromIntVec3(intVec - toil.actor.Position);
						}
						return;
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x001EA04C File Offset: 0x001E824C
		public static Toil ChewIngestible(Pawn chewer, float durationMultiplier, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("ChewIngestible");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (!thing.IngestibleNow)
				{
					chewer.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.pather.StopDead();
				actor.jobs.curDriver.ticksLeftThisToil = Mathf.RoundToInt((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
				if (thing.Spawned)
				{
					thing.Map.physicalInteractionReservationManager.Reserve(chewer, actor.CurJob, thing);
				}
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (chewer != toil.actor)
				{
					toil.actor.rotationTracker.FaceCell(chewer.Position);
				}
				else
				{
					Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
					if (thing != null && thing.Spawned)
					{
						toil.actor.rotationTracker.FaceCell(thing.Position);
					}
					else if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
					{
						toil.actor.rotationTracker.FaceCell(toil.actor.CurJob.GetTarget(eatSurfaceInd).Cell);
					}
				}
				toil.actor.GainComfortFromCellIfPossible(delta, false);
			};
			toil.WithProgressBar(ingestibleInd, delegate
			{
				Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (thing == null)
				{
					return 1f;
				}
				return 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / Mathf.Round((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
			}, false, -0.5f, false);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.FailOnDestroyedOrNull(ingestibleInd);
			toil.AddFinishAction(delegate
			{
				Pawn chewer2 = chewer;
				Thing thing;
				if (chewer2 == null)
				{
					thing = null;
				}
				else
				{
					Job curJob = chewer2.CurJob;
					thing = ((curJob != null) ? curJob.GetTarget(ingestibleInd).Thing : null);
				}
				Thing thing2 = thing;
				if (thing2 == null)
				{
					return;
				}
				if (chewer.Map.physicalInteractionReservationManager.IsReservedBy(chewer, thing2))
				{
					chewer.Map.physicalInteractionReservationManager.Release(chewer, toil.actor.CurJob, thing2);
				}
			});
			toil.handlingFacing = true;
			Toils_Ingest.AddIngestionEffects(toil, chewer, ingestibleInd, eatSurfaceInd);
			return toil;
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x001EA144 File Offset: 0x001E8344
		public static Toil AddIngestionEffects(Toil toil, Pawn chewer, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd)
		{
			toil.WithEffect(delegate()
			{
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target.HasThing)
				{
					return null;
				}
				EffecterDef result = target.Thing.def.ingestible.ingestEffect;
				if (chewer.RaceProps.intelligence < Intelligence.ToolUser && target.Thing.def.ingestible.ingestEffectEat != null)
				{
					result = target.Thing.def.ingestible.ingestEffectEat;
				}
				return result;
			}, delegate()
			{
				if (!toil.actor.CurJob.GetTarget(ingestibleInd).HasThing)
				{
					return null;
				}
				Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (chewer != toil.actor)
				{
					return chewer;
				}
				if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
				{
					return toil.actor.CurJob.GetTarget(eatSurfaceInd);
				}
				return thing;
			}, null);
			toil.PlaySustainerOrSound(delegate()
			{
				if (!chewer.RaceProps.Humanlike)
				{
					return chewer.RaceProps.soundEating;
				}
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target.HasThing)
				{
					return null;
				}
				return target.Thing.def.ingestible.ingestSound;
			}, 1f);
			return toil;
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x001EA1C4 File Offset: 0x001E83C4
		public static Toil FinalizeIngest(Pawn ingester, TargetIndex ingestibleInd)
		{
			Toil toil = ToilMaker.MakeToil("FinalizeIngest");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ingestibleInd).Thing;
				if (ingester.needs.mood != null && thing.def.IsNutritionGivingIngestible && thing.def.ingestible.chairSearchRadius > 10f)
				{
					if (!(ingester.Position + ingester.Rotation.FacingCell).HasEatSurface(actor.Map) && ingester.GetPosture() == PawnPosture.Standing && !ingester.IsWildMan() && thing.def.ingestible.tableDesired)
					{
						ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AteWithoutTable, null, null);
					}
					Room room = ingester.GetRoom(RegionType.Set_All);
					if (room != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
						if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
						{
							ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.AteInImpressiveDiningRoom, scoreStageIndex), null);
						}
					}
				}
				Pawn_NeedsTracker needs = ingester.needs;
				float? num;
				if (needs == null)
				{
					num = null;
				}
				else
				{
					Need_Food food = needs.food;
					num = ((food != null) ? new float?(food.NutritionWanted) : null);
				}
				float num2 = num ?? (thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount);
				if (curJob.ingestTotalCount)
				{
					num2 = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
				}
				else if (curJob.overeat)
				{
					num2 = Mathf.Max(num2, 0.75f);
				}
				float num3 = thing.Ingested(ingester, num2);
				if (!ingester.Dead)
				{
					Pawn_NeedsTracker needs2 = ingester.needs;
					if (((needs2 != null) ? needs2.food : null) != null)
					{
						ingester.needs.food.CurLevel += num3;
						ingester.records.AddTo(RecordDefOf.NutritionEaten, num3);
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x040031BD RID: 12733
		public const int MaxPawnReservations = 10;

		// Token: 0x040031BE RID: 12734
		private static List<IntVec3> spotSearchList = new List<IntVec3>();

		// Token: 0x040031BF RID: 12735
		private static List<IntVec3> cardinals = GenAdj.CardinalDirections.ToList<IntVec3>();

		// Token: 0x040031C0 RID: 12736
		private static List<IntVec3> diagonals = GenAdj.DiagonalDirections.ToList<IntVec3>();
	}
}
