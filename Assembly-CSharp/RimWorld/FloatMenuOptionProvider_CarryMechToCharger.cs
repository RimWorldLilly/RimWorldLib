using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D19 RID: 7449
	public class FloatMenuOptionProvider_CarryMechToCharger : FloatMenuOptionProvider
	{
		// Token: 0x1700231D RID: 8989
		// (get) Token: 0x0600C0B4 RID: 49332 RVA: 0x00464826 File Offset: 0x00462A26
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700231E RID: 8990
		// (get) Token: 0x0600C0B5 RID: 49333 RVA: 0x00464829 File Offset: 0x00462A29
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700231F RID: 8991
		// (get) Token: 0x0600C0B6 RID: 49334 RVA: 0x0046482C File Offset: 0x00462A2C
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002320 RID: 8992
		// (get) Token: 0x0600C0B7 RID: 49335 RVA: 0x0046482F File Offset: 0x00462A2F
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0B8 RID: 49336 RVA: 0x00464832 File Offset: 0x00462A32
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x0600C0B9 RID: 49337 RVA: 0x0046483C File Offset: 0x00462A3C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn == context.FirstSelectedPawn)
			{
				return null;
			}
			if (!clickedPawn.IsSelfShutdown())
			{
				return null;
			}
			Building_MechCharger charger = JobGiver_GetEnergy_Charger.GetClosestCharger(clickedPawn, context.FirstSelectedPawn, false);
			if (charger == null)
			{
				charger = JobGiver_GetEnergy_Charger.GetClosestCharger(clickedPawn, context.FirstSelectedPawn, true);
			}
			if (charger == null)
			{
				return new FloatMenuOption("CannotCarryToRecharger".Translate(clickedPawn.Named("PAWN")) + ": " + "CannotCarryToRechargerNoneAvailable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(charger, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarryToRecharger".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToRechargerOrdered".Translate(clickedPawn.Named("PAWN")), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.HaulMechToCharger, clickedPawn, charger, charger.InteractionCell);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
		}
	}
}
