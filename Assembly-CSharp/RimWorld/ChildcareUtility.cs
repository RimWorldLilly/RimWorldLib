using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public static class ChildcareUtility
	{
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004B7E RID: 19326 RVA: 0x001D3CE8 File Offset: 0x001D1EE8
		public static List<Pawn> CanBreastfeedPlayerPawns
		{
			get
			{
				int ticksGame = Find.TickManager.TicksGame;
				if (ChildcareUtility.canBreastfeedCacheGameTicks < ticksGame)
				{
					ChildcareUtility.canBreastfeedPlayerPawns.Clear();
					foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
					{
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanBreastfeed(pawn, out breastfeedFailReason))
						{
							ChildcareUtility.canBreastfeedPlayerPawns.Add(pawn);
						}
					}
					ChildcareUtility.canBreastfeedCacheGameTicks = ticksGame;
				}
				return ChildcareUtility.canBreastfeedPlayerPawns;
			}
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x001D3D70 File Offset: 0x001D1F70
		public static List<Pawn> SpawnedColonistChildcarers(Map map)
		{
			ChildcareUtility.childcarers.Clear();
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn.WorkTypeIsDisabled(WorkTypeDefOf.Childcare))
				{
					ChildcareUtility.childcarers.Add(pawn);
				}
			}
			return ChildcareUtility.childcarers;
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001D3DE8 File Offset: 0x001D1FE8
		public static TaggedString Translate(this ChildcareUtility.BreastfeedFailReason reason, Pawn hauler = null, Pawn mom = null, Pawn baby = null)
		{
			NamedArgument arg = hauler.Named("HAULER");
			NamedArgument arg2 = mom.Named("MOM");
			NamedArgument arg3 = baby.Named("BABY");
			string key;
			switch (reason)
			{
			case ChildcareUtility.BreastfeedFailReason.MomNotEnoughMilk:
				key = "BreastfeedFailReason_MomNotEnoughMilk";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomInMentalState:
				key = "BreastfeedFailReason_MomInMentalState";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyTooFull:
				key = "BreastfeedFailReason_BabyTooFull";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyForbiddenToMom:
				key = "BreastfeedFailReason_BabyForbiddenToMom";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyForbiddenToHauler:
				key = "BreastfeedFailReason_BabyForbiddenToHauler";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomForbiddenToHauler:
				key = "BreastfeedFailReason_MomForbiddenToHauler";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomIsMobile:
				key = "BreastfeedFailReason_MomIsMobile";
				break;
			case ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby:
				key = "BreastfeedFailReason_HaulerCannotReachBaby";
				break;
			case ChildcareUtility.BreastfeedFailReason.HaulerCannotReachMom:
				key = "BreastfeedFailReason_HaulerCannotReachMom";
				break;
			case ChildcareUtility.BreastfeedFailReason.NoMomsAssignedToBaby:
				key = "BreastfeedFailReason_NoMomsAssignedToBaby";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomNotAssignedToChildcare:
				key = "BreastfeedFailReason_MomNotAssignedToChildcare";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomInsideContainer:
				key = "BreastfeedFailReason_MomInsideContainer";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomBeingCarried:
				key = "BreastfeedFailReason_MomBeingCarried";
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("BreastfeedFailReason ");
				defaultInterpolatedStringHandler.AppendFormatted<ChildcareUtility.BreastfeedFailReason>(reason);
				defaultInterpolatedStringHandler.AppendLiteral(" should not be player facing.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), (int)(784668349 + reason));
				return reason.ToString();
			}
			}
			return key.Translate(arg, arg2, arg3);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x001D3F2C File Offset: 0x001D212C
		public static bool CanFeed(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (mom == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNull);
			}
			else if (mom.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomDead);
			}
			else if (!mom.RaceProps.Humanlike)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotHumanLike);
			}
			return reason == null;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x001D3F8C File Offset: 0x001D218C
		public static bool CanBreastfeed(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (!ChildcareUtility.CanFeed(mom, out reason))
			{
				return false;
			}
			if (!ModsConfig.BiotechActive)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoBiotechMod);
			}
			else if (!mom.health.hediffSet.HasHediff(HediffDefOf.Lactating, false))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotLactating);
			}
			return reason == null;
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x001D3FF0 File Offset: 0x001D21F0
		public static bool CanBreastfeedNow(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanBreastfeed(mom, out reason))
			{
				return false;
			}
			Hediff firstHediffOfDef = mom.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			bool flag;
			if (firstHediffOfDef == null)
			{
				flag = true;
			}
			else
			{
				HediffComp_Chargeable hediffComp_Chargeable = firstHediffOfDef.TryGetComp<HediffComp_Chargeable>();
				flag = !((hediffComp_Chargeable != null) ? new bool?(hediffComp_Chargeable.CanActivate) : null).GetValueOrDefault();
			}
			if (flag)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotEnoughMilk);
			}
			else if (mom.InMentalState && !mom.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInMentalState);
			}
			return reason == null;
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x001D4084 File Offset: 0x001D2284
		public static bool CanSuckle(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (!ModsConfig.BiotechActive)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoBiotechMod);
			}
			else if (baby == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNull);
			}
			else if (baby.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyDead);
			}
			else if (!baby.RaceProps.Humanlike)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotHumanlike);
			}
			else if (!baby.DevelopmentalStage.Baby())
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyTooOld);
			}
			else if (baby.IsShambler)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyShambler);
			}
			return reason == null;
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x001D412D File Offset: 0x001D232D
		public static bool WantsSuckle(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			if (!FeedPatientUtility.IsHungry(baby))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotHungry);
			}
			return reason == null;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x001D4158 File Offset: 0x001D2358
		public static bool CanSuckleNow(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			Pawn_NeedsTracker needs = baby.needs;
			if (((needs != null) ? needs.food : null) != null && baby.needs.food.CurLevelPercentage >= 0.66f)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyTooFull);
			}
			return reason == null;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x001D41B0 File Offset: 0x001D23B0
		public static bool CanFeedBaby(Pawn feeder, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanFeed(feeder, out reason))
			{
				return false;
			}
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			if (feeder.MapHeld != null && baby.IsForbidden(feeder))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyForbiddenToMom);
			}
			else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(feeder, baby))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyInIncompatibleFactionToMom);
			}
			else
			{
				reason = null;
			}
			return reason == null;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x001D421A File Offset: 0x001D241A
		public static bool CanMomBreastfeedBaby(Pawn mom, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanBreastfeed(mom, out reason) && ChildcareUtility.CanSuckle(baby, out reason) && ChildcareUtility.CanFeedBaby(mom, baby, out reason) && reason == null;
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x001D4247 File Offset: 0x001D2447
		public static bool CanMomBreastfeedBabyNow(Pawn mom, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanBreastfeedNow(mom, out reason) && ChildcareUtility.CanSuckleNow(baby, out reason) && ChildcareUtility.CanMomBreastfeedBaby(mom, baby, out reason) && reason == null;
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x001D4274 File Offset: 0x001D2474
		public static bool CanHaulBaby(Pawn hauler, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (hauler == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNull);
			}
			else if (baby == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNull);
			}
			else if (hauler.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDead);
			}
			else if (hauler.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDowned);
			}
			else if (hauler.Map == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMap);
			}
			else if (baby.MapHeld == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotOnMap);
			}
			else if (hauler.Map != baby.MapHeld)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnBabyMap);
			}
			else if (baby.IsForbidden(hauler))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyForbiddenToHauler);
			}
			else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(hauler, baby))
			{
				if (!ChildcareUtility.BabyHasFeederInCompatibleFaction(hauler.Faction, baby))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyInIncompatibleFactionToHauler);
				}
			}
			else if (!hauler.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerIncapableOfManipulation);
			}
			return reason == null;
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x001D43A0 File Offset: 0x001D25A0
		public static bool CanHaulBabyNow(Pawn hauler, Pawn baby, bool ignoreOtherReservations, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanHaulBaby(hauler, baby, out reason))
			{
				return false;
			}
			if (!hauler.CanReserve(baby, 1, -1, null, ignoreOtherReservations))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReserveBaby);
			}
			else if (!hauler.CanReach(baby, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby);
			}
			return reason == null;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x001D4404 File Offset: 0x001D2604
		public static bool CanHaulToMom(Pawn hauler, Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (hauler == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNull);
			}
			else if (mom == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNull);
			}
			else if (hauler.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDead);
			}
			else if (hauler.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDowned);
			}
			else if (mom.MapHeld == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotOnMap);
			}
			else if (hauler.Map == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMap);
			}
			else if (hauler.Map != mom.MapHeld)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMomMap);
			}
			else if (hauler != mom)
			{
				if (mom.IsForbidden(hauler))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomForbiddenToHauler);
				}
				else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(hauler, mom))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInIncompatibleFactionToHauler);
				}
			}
			return reason == null;
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x001D44F8 File Offset: 0x001D26F8
		public static bool CanHaulBabyToMomNow(Pawn hauler, Pawn mom, Pawn baby, bool ignoreOtherReservations, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out reason))
			{
				return false;
			}
			if (!ChildcareUtility.CanHaulToMom(hauler, mom, out reason))
			{
				return false;
			}
			if (hauler != mom)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (!mom.Downed && !mom.IsPrisoner && ChildcareUtility.CanHaulBabyToMomNow(mom, mom, baby, ignoreOtherReservations, out breastfeedFailReason))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomIsMobile);
				}
				else if (!hauler.CanReserve(mom, 1, -1, null, ignoreOtherReservations))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReserveMom);
				}
				else if (!mom.Spawned)
				{
					if (mom.SpawnedParentOrMe is Pawn)
					{
						reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomBeingCarried);
					}
					else
					{
						reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInsideContainer);
					}
				}
				else if (!hauler.Map.reachability.CanReach(baby.Position, mom.Position, PathEndMode.Touch, TraverseParms.For(hauler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReachMom);
				}
			}
			return reason == null;
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x001D45F4 File Offset: 0x001D27F4
		public static bool CanAutoBreastfeed(Pawn mom, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanMomBreastfeedBaby(mom, baby, out reason))
			{
				return false;
			}
			AutofeedMode autofeedMode = baby.mindState.AutofeedSetting(mom);
			if (autofeedMode == AutofeedMode.Urgent)
			{
				reason = null;
			}
			else if (!mom.workSettings.WorkIsActive(WorkTypeDefOf.Childcare))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotAssignedToChildcare);
			}
			else if (!forced && autofeedMode == AutofeedMode.Never)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotAssignedToBaby);
			}
			return reason == null;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x001D4664 File Offset: 0x001D2864
		public static bool CanMomAutoBreastfeedBabyNow(Pawn mother, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanMomBreastfeedBabyNow(mother, baby, out reason) && ChildcareUtility.CanAutoBreastfeed(mother, baby, forced, out reason) && ChildcareUtility.CanHaulBabyToMomNow(mother, mother, baby, forced, out reason);
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x001D468E File Offset: 0x001D288E
		public static bool CanHaulBabyToDownedMomToBreastfeedNow(Pawn hauler, Pawn mother, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanMomBreastfeedBabyNow(mother, baby, out reason) && ChildcareUtility.CanAutoBreastfeed(mother, baby, forced, out reason) && ChildcareUtility.CanHaulBabyToMomNow(hauler, mother, baby, forced, out reason);
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x001D46BB File Offset: 0x001D28BB
		public static IEnumerable<Pawn> CanBreastfeedMothers(Pawn baby, bool includeAutoBreastfeedOff = false)
		{
			ChildcareUtility.<CanBreastfeedMothers>d__34 <CanBreastfeedMothers>d__ = new ChildcareUtility.<CanBreastfeedMothers>d__34(-2);
			<CanBreastfeedMothers>d__.<>3__baby = baby;
			<CanBreastfeedMothers>d__.<>3__includeAutoBreastfeedOff = includeAutoBreastfeedOff;
			return <CanBreastfeedMothers>d__;
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x001D46D2 File Offset: 0x001D28D2
		public static Job MakeBringBabyToSafetyJob(Pawn hauler, Pawn baby)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafety, baby);
			job.count = 1;
			return job;
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x001D46EB File Offset: 0x001D28EB
		public static Job MakeBreastfeedCarryToMomJob(Pawn baby, Pawn mom)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BreastfeedCarryToMom, baby, mom);
			job.count = 1;
			return job;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x001D470A File Offset: 0x001D290A
		public static Job MakeBreastfeedJob(Pawn baby, Building_Bed bedWithDownedMom = null)
		{
			Job job = (bedWithDownedMom == null) ? JobMaker.MakeJob(JobDefOf.Breastfeed, baby) : JobMaker.MakeJob(JobDefOf.Breastfeed, baby, bedWithDownedMom);
			job.count = 1;
			return job;
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x001D473E File Offset: 0x001D293E
		public static Job MakeBottlefeedJob(Pawn baby, Thing foodSource)
		{
			if (foodSource is Pawn)
			{
				Log.Error("Cannot use a pawn as a food source for bottlefeeding a baby.  Use MakeBreastfeedJob instead.");
			}
			Job job = JobMaker.MakeJob(JobDefOf.BottleFeedBaby);
			job.targetA = baby;
			job.targetB = foodSource;
			job.count = 1;
			return job;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x001D477C File Offset: 0x001D297C
		public static Job MakeAutofeedBabyJob(Pawn feeder, Pawn baby, Thing foodSource)
		{
			if (foodSource == null)
			{
				Log.Error("Cannot MakeFeedBabyJob with null foodSource.");
				return null;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn == null)
			{
				return ChildcareUtility.MakeBottlefeedJob(baby, foodSource);
			}
			if (feeder == pawn)
			{
				return ChildcareUtility.MakeBreastfeedJob(baby, null);
			}
			return ChildcareUtility.MakeBreastfeedCarryToMomJob(baby, pawn);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x001D47BD File Offset: 0x001D29BD
		public static Job MakeBabySuckleJob(Pawn feeder)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BabySuckle, feeder);
			job.count = 1;
			return job;
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x001D47D6 File Offset: 0x001D29D6
		public static Job MakeBabyPlayJob(Pawn feeder)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BabyPlay, feeder);
			job.count = 1;
			return job;
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x001D47F0 File Offset: 0x001D29F0
		public static Toil MakeBabyPlayAsLongAsToilIsActive(Toil toil, TargetIndex babyIndex)
		{
			toil.AddPreInitAction(delegate
			{
				((Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing).jobs.StartJob(ChildcareUtility.MakeBabyPlayJob(toil.actor), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			});
			toil.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing;
				if (pawn.CurJobDef == JobDefOf.BabyPlay)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			});
			return toil;
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x001D4845 File Offset: 0x001D2A45
		public static bool HasBreastfeedCompatibleFactions(Faction faction, Pawn baby)
		{
			return baby.Faction == faction || (faction != null && baby.HostFaction == faction);
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x001D4864 File Offset: 0x001D2A64
		public static bool BabyHasFeederInCompatibleFaction(Faction faction, Pawn baby)
		{
			foreach (Pawn pawn in ChildcareUtility.CanBreastfeedMothers(baby, false))
			{
				if (pawn.Faction == faction)
				{
					return true;
				}
				if (faction != null && pawn.HostFaction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x001D48CC File Offset: 0x001D2ACC
		public static bool HasBreastfeedCompatibleFactions(Pawn mom, Pawn baby)
		{
			return ChildcareUtility.HasBreastfeedCompatibleFactions(mom.Faction, baby);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x001D48DC File Offset: 0x001D2ADC
		public static bool ImmobileBreastfeederAvailable(Pawn hauler, Pawn baby, bool forced, out Pawn feeder, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.cachedHaulerBabyMotherImmobile.ContainsKey(hauler))
			{
				ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler] = new Dictionary<Pawn, Pawn>();
			}
			Pawn pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler].TryGetValue(baby, out pawn) && ChildcareUtility.CanHaulBabyToDownedMomToBreastfeedNow(hauler, pawn, baby, forced, out breastfeedFailReason))
			{
				feeder = pawn;
				reason = null;
				return true;
			}
			reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoMomsAssignedToBaby);
			feeder = null;
			foreach (Pawn pawn2 in ChildcareUtility.CanBreastfeedMothers(baby, forced))
			{
				if (pawn2 == hauler)
				{
					if (feeder == null)
					{
						reason = null;
						feeder = pawn2;
					}
				}
				else
				{
					if (ChildcareUtility.CanHaulBabyToDownedMomToBreastfeedNow(hauler, pawn2, baby, forced, out reason))
					{
						ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler][baby] = pawn2;
						return true;
					}
					feeder = pawn2;
				}
			}
			return false;
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x001D49C4 File Offset: 0x001D2BC4
		public static bool ShouldWakeUpToAutofeedUrgent(Pawn feeder)
		{
			if (feeder.Awake())
			{
				Log.Warning("Should not try check if pawn should wake up to autofeed if pawn already awake.");
				return false;
			}
			if (feeder.mindState.nextSleepingBreastfeedStrictTick > Find.TickManager.TicksGame)
			{
				return false;
			}
			feeder.mindState.nextSleepingBreastfeedStrictTick = Find.TickManager.TicksGame + Pawn_MindState.NextSleepingBreastfeedTickCheck.RandomInRange;
			if (feeder.Downed)
			{
				return false;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanFeed(feeder, out breastfeedFailReason))
			{
				return false;
			}
			Thing thing;
			Pawn pawn = ChildcareUtility.FindAutofeedBaby(feeder, AutofeedMode.Urgent, out thing);
			Pawn pawn2;
			return pawn != null && (thing != null || ChildcareUtility.ImmobileBreastfeederAvailable(feeder, pawn, false, out pawn2, out breastfeedFailReason));
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x001D4A5C File Offset: 0x001D2C5C
		public static Thing FindBabyFoodForBaby(Pawn feeder, Pawn baby)
		{
			Thing result;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(feeder, baby, baby.needs.food.CurCategory == HungerCategory.Starving, out result, out thingDef, false, true, false, false, true, false, false, false, false, false, false, FoodPreferability.RawBad))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x001D4A98 File Offset: 0x001D2C98
		public static Pawn FindAutofeedBaby(Pawn mom, AutofeedMode priorityLevel, out Thing food)
		{
			if (priorityLevel == AutofeedMode.Never)
			{
				Log.Warning("Will never auto breastfeed a baby if priority level is OFF");
				food = null;
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			bool flag = ChildcareUtility.CanBreastfeedNow(mom, out breastfeedFailReason);
			foreach (Pawn pawn in mom.MapHeld.mapPawns.FreeHumanlikesOfFaction(mom.Faction))
			{
				if (!pawn.Suspended && ChildcareUtility.WantsSuckle(pawn, out breastfeedFailReason) && pawn.mindState.AutofeedSetting(mom) == priorityLevel && ChildcareUtility.CanFeedBaby(mom, pawn, out breastfeedFailReason) && ChildcareUtility.CanHaulBabyToMomNow(mom, mom, pawn, false, out breastfeedFailReason))
				{
					if (flag)
					{
						food = mom;
					}
					else
					{
						food = ChildcareUtility.FindBabyFoodForBaby(mom, pawn);
					}
					return pawn;
				}
			}
			food = null;
			return null;
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x001D4B68 File Offset: 0x001D2D68
		public static bool SuckleFromLactatingPawn(Pawn baby, Pawn feeder, int delta)
		{
			Hediff firstHediffOfDef = feeder.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			HediffComp_Chargeable hediffComp_Chargeable = firstHediffOfDef.TryGetComp<HediffComp_Chargeable>();
			firstHediffOfDef.Severity = Mathf.Min(1f, firstHediffOfDef.Severity + 0.0001f * (float)delta);
			float nutritionWanted = baby.needs.food.NutritionWanted;
			float num = Mathf.Min(baby.needs.food.MaxLevel / 5000f * (float)delta, nutritionWanted);
			float num2 = hediffComp_Chargeable.GreedyConsume(num);
			baby.needs.food.CurLevel += num2;
			Caravan caravan = baby.GetCaravan();
			if (caravan != null && feeder.GetCaravan() == caravan)
			{
				feeder.mindState.BreastfeedCaravan(baby, num2 / baby.needs.food.MaxLevel);
			}
			Pawn_IdeoTracker ideo = baby.ideo;
			if (ideo != null)
			{
				ideo.IncreaseIdeoExposureIfBabyTick(feeder.Ideo, 1);
			}
			return !Mathf.Approximately(num2, nutritionWanted) && num2 >= num;
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x001D4C5F File Offset: 0x001D2E5F
		public static void ClearCache()
		{
			ChildcareUtility.canBreastfeedCacheGameTicks = -1;
			ChildcareUtility.canBreastfeedPlayerPawns.Clear();
			ChildcareUtility.cachedHaulerBabyMotherImmobile.Clear();
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x001D4C7B File Offset: 0x001D2E7B
		public static bool SwaddleBaby(this Pawn baby)
		{
			return ModsConfig.BiotechActive && baby.RaceProps.Humanlike && baby.DevelopmentalStage.Baby() && !baby.Dead && !(baby.ParentHolder is Building_GrowthVat);
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x001D4CBC File Offset: 0x001D2EBC
		public static bool BabyNeedsMovingForTemperatureReasons(Pawn baby, Pawn hauler, out Region preferredRegion, IntVec3? positionOverride = null)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				preferredRegion = null;
				return false;
			}
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, false, out breastfeedFailReason))
			{
				preferredRegion = null;
				return false;
			}
			if (Find.TickManager.TicksGame < baby.mindState.lastBroughtToSafeTemperatureTick + 2500)
			{
				preferredRegion = null;
				return false;
			}
			FloatRange tempRange = baby.ComfortableTemperatureRange();
			FloatRange tempRange2 = baby.SafeTemperatureRange();
			IntVec3 root = positionOverride ?? baby.PositionHeld;
			float f;
			if (positionOverride != null)
			{
				f = GenTemperature.GetTemperatureForCell(positionOverride.Value, baby.MapHeld);
			}
			else
			{
				f = baby.AmbientTemperature;
			}
			if (tempRange.Includes(f))
			{
				preferredRegion = null;
				return false;
			}
			Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange, TraverseParms.For(hauler, Danger.Some, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, -1f);
			if (region != null)
			{
				preferredRegion = region;
				return true;
			}
			if (tempRange2.Includes(f))
			{
				preferredRegion = null;
				return false;
			}
			Region region2 = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange2, TraverseParms.For(hauler, Danger.Some, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, -1f);
			if (region2 != null)
			{
				preferredRegion = region2;
				return true;
			}
			preferredRegion = null;
			return false;
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x001D4DD8 File Offset: 0x001D2FD8
		public static LocalTargetInfo SafePlaceForBaby(Pawn baby, Pawn hauler, bool ignoreOtherReservations = false)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				return LocalTargetInfo.Invalid;
			}
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out breastfeedFailReason))
			{
				return LocalTargetInfo.Invalid;
			}
			Building_Bed building_Bed = baby.CurrentBed() ?? RestUtility.FindBedFor(baby, hauler, true, false, baby.GuestStatus);
			float temperatureForCell = GenTemperature.GetTemperatureForCell((building_Bed != null) ? building_Bed.Position : IntVec3.Invalid, baby.MapHeld);
			if (building_Bed != null && building_Bed.Medical && HealthAIUtility.ShouldSeekMedicalRest(baby))
			{
				return building_Bed;
			}
			if (building_Bed != null && baby.ComfortableTemperatureRange().Includes(temperatureForCell))
			{
				return building_Bed;
			}
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			Region targetRegion;
			if (ChildcareUtility.BabyNeedsMovingForTemperatureReasons(baby, hauler, out targetRegion, null))
			{
				result = RCellFinder.SpotToStandDuringJob(hauler, null, targetRegion);
			}
			else if (baby.Spawned)
			{
				result = baby.Position;
			}
			else
			{
				result = RCellFinder.SpotToStandDuringJob(hauler, null, baby.GetRegionHeld(RegionType.Set_Passable));
			}
			if (result.IsValid && baby.ComfortableTemperatureAtCell(result.Cell, baby.MapHeld))
			{
				return result;
			}
			if (building_Bed != null && baby.SafeTemperatureRange().Includes(temperatureForCell))
			{
				return building_Bed;
			}
			if (result.IsValid && baby.SafeTemperatureAtCell(result.Cell, baby.MapHeld))
			{
				return result;
			}
			LocalTargetInfo result2 = LocalTargetInfo.Invalid;
			if (baby.Spawned)
			{
				result2 = baby.Position;
			}
			else
			{
				result2 = RCellFinder.SpotToStandDuringJob(hauler, null, null);
			}
			if (building_Bed != null && GenTemperature.GetTemperatureForCell(result2.Cell, baby.MapHeld) < temperatureForCell + 5f)
			{
				return building_Bed;
			}
			return result2;
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x001D4F7C File Offset: 0x001D317C
		public static Pawn FindUnsafeBaby(Pawn mom, AutofeedMode priorityLevel)
		{
			if (priorityLevel == AutofeedMode.Never)
			{
				return null;
			}
			ChildcareUtility.tmpBabyList.Clear();
			ChildcareUtility.tmpBabyList.AddRange(mom.MapHeld.mapPawns.FreeHumanlikesSpawnedOfFaction(mom.Faction));
			foreach (Pawn pawn in ChildcareUtility.tmpBabyList)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason) && pawn.mindState.AutofeedSetting(mom) == priorityLevel && !CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(pawn))
				{
					LocalTargetInfo localTargetInfo = ChildcareUtility.SafePlaceForBaby(pawn, mom, false);
					if (localTargetInfo.IsValid)
					{
						Building_Bed building_Bed = localTargetInfo.Thing as Building_Bed;
						if (building_Bed != null)
						{
							if (pawn.CurrentBed() == building_Bed)
							{
								continue;
							}
						}
						else if (pawn.Spawned && pawn.Position == localTargetInfo.Cell)
						{
							continue;
						}
						ChildcareUtility.tmpBabyList.Clear();
						return pawn;
					}
				}
			}
			ChildcareUtility.tmpBabyList.Clear();
			return null;
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x001D5084 File Offset: 0x001D3284
		public static bool BabyBeingPlayedWith(Pawn baby)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				return false;
			}
			ChildcareUtility.tmpReserversOut.Clear();
			baby.MapHeld.reservationManager.ReserversOf(baby, ChildcareUtility.tmpReserversOut);
			foreach (Pawn pawn in ChildcareUtility.tmpReserversOut)
			{
				if (baby.MapHeld.reservationManager.ReservedBy(baby, pawn, pawn.CurJob) && pawn.jobs.curDriver is JobDriver_BabyPlay)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002FD1 RID: 12241
		public static readonly IntRange NextCachedDownedMotherTickCheck = new IntRange(200, 300);

		// Token: 0x04002FD2 RID: 12242
		private static Dictionary<Pawn, Dictionary<Pawn, Pawn>> cachedHaulerBabyMotherImmobile = new Dictionary<Pawn, Dictionary<Pawn, Pawn>>();

		// Token: 0x04002FD3 RID: 12243
		private static int canBreastfeedCacheGameTicks = -1;

		// Token: 0x04002FD4 RID: 12244
		private static List<Pawn> canBreastfeedPlayerPawns = new List<Pawn>();

		// Token: 0x04002FD5 RID: 12245
		private const float BreastfeedHungerPercentage = 0.66f;

		// Token: 0x04002FD6 RID: 12246
		private const int FullLactatingSeverityTicks = 10000;

		// Token: 0x04002FD7 RID: 12247
		public const int FullFeedSessionTicks = 5000;

		// Token: 0x04002FD8 RID: 12248
		public const int BabyFullFeedsPerDay = 2;

		// Token: 0x04002FD9 RID: 12249
		private const float PreferBedUnsafeTemperatureEpsilon = 5f;

		// Token: 0x04002FDA RID: 12250
		private const int BabyTemperatureMoveRepeatTicksDelay = 2500;

		// Token: 0x04002FDB RID: 12251
		private static List<Pawn> childcarers = new List<Pawn>();

		// Token: 0x04002FDC RID: 12252
		private static List<Pawn> tmpAutoBreastfeedOpportunisticMoms = new List<Pawn>();

		// Token: 0x04002FDD RID: 12253
		private static List<Pawn> tmpAutoBreastfeedOffMoms = new List<Pawn>();

		// Token: 0x04002FDE RID: 12254
		private static readonly List<Pawn> tmpBabyList = new List<Pawn>();

		// Token: 0x04002FDF RID: 12255
		private static HashSet<Pawn> tmpReserversOut = new HashSet<Pawn>();

		// Token: 0x02002C13 RID: 11283
		public enum BreastfeedFailReason
		{
			// Token: 0x0400B3E5 RID: 46053
			MomNotEnoughMilk,
			// Token: 0x0400B3E6 RID: 46054
			MomInMentalState,
			// Token: 0x0400B3E7 RID: 46055
			BabyTooFull,
			// Token: 0x0400B3E8 RID: 46056
			BabyForbiddenToMom,
			// Token: 0x0400B3E9 RID: 46057
			BabyForbiddenToHauler,
			// Token: 0x0400B3EA RID: 46058
			MomForbiddenToHauler,
			// Token: 0x0400B3EB RID: 46059
			MomIsMobile,
			// Token: 0x0400B3EC RID: 46060
			HaulerCannotReachBaby,
			// Token: 0x0400B3ED RID: 46061
			HaulerCannotReachMom,
			// Token: 0x0400B3EE RID: 46062
			NoMomsAssignedToBaby,
			// Token: 0x0400B3EF RID: 46063
			MomNotAssignedToChildcare,
			// Token: 0x0400B3F0 RID: 46064
			MomInsideContainer,
			// Token: 0x0400B3F1 RID: 46065
			MomBeingCarried,
			// Token: 0x0400B3F2 RID: 46066
			NoBiotechMod,
			// Token: 0x0400B3F3 RID: 46067
			MomNull,
			// Token: 0x0400B3F4 RID: 46068
			BabyNull,
			// Token: 0x0400B3F5 RID: 46069
			HaulerNull,
			// Token: 0x0400B3F6 RID: 46070
			BabyNotHumanlike,
			// Token: 0x0400B3F7 RID: 46071
			MomNotHumanLike,
			// Token: 0x0400B3F8 RID: 46072
			BabyDead,
			// Token: 0x0400B3F9 RID: 46073
			MomDead,
			// Token: 0x0400B3FA RID: 46074
			BabyTooOld,
			// Token: 0x0400B3FB RID: 46075
			HaulerDowned,
			// Token: 0x0400B3FC RID: 46076
			HaulerDead,
			// Token: 0x0400B3FD RID: 46077
			MomNotLactating,
			// Token: 0x0400B3FE RID: 46078
			BabyNotHungry,
			// Token: 0x0400B3FF RID: 46079
			BabyNotOnMap,
			// Token: 0x0400B400 RID: 46080
			MomNotOnMap,
			// Token: 0x0400B401 RID: 46081
			HaulerNotOnMap,
			// Token: 0x0400B402 RID: 46082
			HaulerNotOnBabyMap,
			// Token: 0x0400B403 RID: 46083
			HaulerNotOnMomMap,
			// Token: 0x0400B404 RID: 46084
			MomNotAssignedToBaby,
			// Token: 0x0400B405 RID: 46085
			BabyInIncompatibleFactionToMom,
			// Token: 0x0400B406 RID: 46086
			BabyInIncompatibleFactionToHauler,
			// Token: 0x0400B407 RID: 46087
			MomInIncompatibleFactionToHauler,
			// Token: 0x0400B408 RID: 46088
			HaulerCannotReserveBaby,
			// Token: 0x0400B409 RID: 46089
			HaulerCannotReserveMom,
			// Token: 0x0400B40A RID: 46090
			HaulerIncapableOfManipulation,
			// Token: 0x0400B40B RID: 46091
			BabyShambler
		}
	}
}
