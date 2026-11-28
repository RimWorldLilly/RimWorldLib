using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D37 RID: 7479
	public class FloatMenuOptionProvider_OpenThing : FloatMenuOptionProvider
	{
		// Token: 0x1700238E RID: 9102
		// (get) Token: 0x0600C186 RID: 49542 RVA: 0x00467CEB File Offset: 0x00465EEB
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700238F RID: 9103
		// (get) Token: 0x0600C187 RID: 49543 RVA: 0x00467CEE File Offset: 0x00465EEE
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002390 RID: 9104
		// (get) Token: 0x0600C188 RID: 49544 RVA: 0x00467CF1 File Offset: 0x00465EF1
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C189 RID: 49545 RVA: 0x00467CF4 File Offset: 0x00465EF4
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			IOpenable openable = clickedThing as IOpenable;
			if (openable == null || !openable.CanOpen)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotOpen".Translate(clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotOpen".Translate(clickedThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.Drafted && clickedThing.Map.designationManager.DesignationOn(clickedThing, DesignationDefOf.Open) != null)
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Open".Translate(clickedThing), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Open, clickedThing);
				job.ignoreDesignations = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
