using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D24 RID: 7460
	public class FloatMenuOptionProvider_DraftedRepair : FloatMenuOptionProvider
	{
		// Token: 0x17002348 RID: 9032
		// (get) Token: 0x0600C10C RID: 49420 RVA: 0x004661B5 File Offset: 0x004643B5
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002349 RID: 9033
		// (get) Token: 0x0600C10D RID: 49421 RVA: 0x004661B8 File Offset: 0x004643B8
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700234A RID: 9034
		// (get) Token: 0x0600C10E RID: 49422 RVA: 0x004661BB File Offset: 0x004643BB
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700234B RID: 9035
		// (get) Token: 0x0600C10F RID: 49423 RVA: 0x004661BE File Offset: 0x004643BE
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700234C RID: 9036
		// (get) Token: 0x0600C110 RID: 49424 RVA: 0x004661C1 File Offset: 0x004643C1
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C111 RID: 49425 RVA: 0x004661C4 File Offset: 0x004643C4
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.skills != null && !context.FirstSelectedPawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled;
		}

		// Token: 0x0600C112 RID: 49426 RVA: 0x004661F4 File Offset: 0x004643F4
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!RepairUtility.PawnCanRepairNow(context.FirstSelectedPawn, clickedThing))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotRepair".Translate(clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RepairThing".Translate(clickedThing), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Repair, clickedThing), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
