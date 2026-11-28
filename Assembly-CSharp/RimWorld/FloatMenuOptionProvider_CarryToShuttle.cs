using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D1E RID: 7454
	public class FloatMenuOptionProvider_CarryToShuttle : FloatMenuOptionProvider
	{
		// Token: 0x17002330 RID: 9008
		// (get) Token: 0x0600C0D5 RID: 49365 RVA: 0x00465170 File Offset: 0x00463370
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002331 RID: 9009
		// (get) Token: 0x0600C0D6 RID: 49366 RVA: 0x00465173 File Offset: 0x00463373
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002332 RID: 9010
		// (get) Token: 0x0600C0D7 RID: 49367 RVA: 0x00465176 File Offset: 0x00463376
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002333 RID: 9011
		// (get) Token: 0x0600C0D8 RID: 49368 RVA: 0x00465179 File Offset: 0x00463379
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0D9 RID: 49369 RVA: 0x0046517C File Offset: 0x0046337C
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.RoyaltyActive;
		}

		// Token: 0x0600C0DA RID: 49370 RVA: 0x00465184 File Offset: 0x00463384
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_CarryToShuttle.<>c__DisplayClass9_0 CS$<>8__locals1 = new FloatMenuOptionProvider_CarryToShuttle.<>c__DisplayClass9_0();
			CS$<>8__locals1.clickedPawn = clickedPawn;
			CS$<>8__locals1.context = context;
			if (!this.CanBeCarriedToShuttle(CS$<>8__locals1.clickedPawn))
			{
				return null;
			}
			if (!CS$<>8__locals1.clickedPawn.Spawned)
			{
				return null;
			}
			if (!CS$<>8__locals1.clickedPawn.CanReserveAndReach(CS$<>8__locals1.clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return null;
			}
			CS$<>8__locals1.shuttleThing = GenClosest.ClosestThingReachable(CS$<>8__locals1.clickedPawn.Position, CS$<>8__locals1.clickedPawn.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.context.FirstSelectedPawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<GetSingleOptionFor>g__IsValidShuttle|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (CS$<>8__locals1.shuttleThing == null)
			{
				return null;
			}
			if (CS$<>8__locals1.context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
			{
				return new FloatMenuOption("CannotLoadIntoShuttle".Translate(CS$<>8__locals1.shuttleThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToShuttle".Translate(CS$<>8__locals1.clickedPawn), new Action(CS$<>8__locals1.<GetSingleOptionFor>g__CarryToShuttleAct|1), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedPawn, "ReservedBy", null);
		}

		// Token: 0x0600C0DB RID: 49371 RVA: 0x00465306 File Offset: 0x00463506
		private bool CanBeCarriedToShuttle(Pawn pawn)
		{
			if (pawn.IsPrisonerOfColony)
			{
				return pawn.guest.PrisonerIsSecure;
			}
			return pawn.AnimalOrWildMan() || pawn.Downed;
		}
	}
}
