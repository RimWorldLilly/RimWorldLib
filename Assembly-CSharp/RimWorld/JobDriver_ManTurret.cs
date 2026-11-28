using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A55 RID: 2645
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x001E2218 File Offset: 0x001E0418
		private Building_TurretGun Turret
		{
			get
			{
				return (Building_TurretGun)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x001E2240 File Offset: 0x001E0440
		private Thing Hauling
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x001E2264 File Offset: 0x001E0464
		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
			return compChangeableProjectile != null && !compChangeableProjectile.Loaded;
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x001E2298 File Offset: 0x001E0498
		private static bool GunNeedsRefueling(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompRefuelable compRefuelable = building_TurretGun.TryGetComp<CompRefuelable>();
			return compRefuelable != null && !compRefuelable.HasFuel && compRefuelable.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars;
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001E22E4 File Offset: 0x001E04E4
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			JobDriver_ManTurret.<>c__DisplayClass10_0 CS$<>8__locals1 = new JobDriver_ManTurret.<>c__DisplayClass10_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.allowedShellsSettings = ((CS$<>8__locals1.pawn.IsColonist || CS$<>8__locals1.pawn.IsColonyMech) ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null);
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 40f, new Predicate<Thing>(CS$<>8__locals1.<FindAmmoForTurret>g__ShellValidator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x001E2374 File Offset: 0x001E0574
		public static Thing FindFuelForTurret(Pawn pawn, Building_TurretGun gun)
		{
			JobDriver_ManTurret.<>c__DisplayClass11_0 CS$<>8__locals1 = new JobDriver_ManTurret.<>c__DisplayClass11_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.refuelableComp = gun.TryGetComp<CompRefuelable>();
			if (CS$<>8__locals1.refuelableComp == null)
			{
				return null;
			}
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 40f, new Predicate<Thing>(CS$<>8__locals1.<FindFuelForTurret>g__FuelValidator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x001E23E6 File Offset: 0x001E05E6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x001E2409 File Offset: 0x001E0609
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ManTurret.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_ManTurret.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400310C RID: 12556
		private const float SearchRadius = 40f;

		// Token: 0x0400310D RID: 12557
		private const int MaxPawnReservations = 10;

		// Token: 0x0400310E RID: 12558
		private const TargetIndex TurretInd = TargetIndex.A;

		// Token: 0x0400310F RID: 12559
		private const TargetIndex HaulingInd = TargetIndex.B;
	}
}
