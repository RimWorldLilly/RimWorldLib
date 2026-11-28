using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D3D RID: 7485
	public class FloatMenuOptionProvider_RemoveMechlink : FloatMenuOptionProvider
	{
		// Token: 0x170023A1 RID: 9121
		// (get) Token: 0x0600C1AD RID: 49581 RVA: 0x00468755 File Offset: 0x00466955
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023A2 RID: 9122
		// (get) Token: 0x0600C1AE RID: 49582 RVA: 0x00468758 File Offset: 0x00466958
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023A3 RID: 9123
		// (get) Token: 0x0600C1AF RID: 49583 RVA: 0x0046875B File Offset: 0x0046695B
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023A4 RID: 9124
		// (get) Token: 0x0600C1B0 RID: 49584 RVA: 0x0046875E File Offset: 0x0046695E
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1B1 RID: 49585 RVA: 0x00468761 File Offset: 0x00466961
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x0600C1B2 RID: 49586 RVA: 0x00468768 File Offset: 0x00466968
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Corpse corpse = clickedThing as Corpse;
			if (corpse == null)
			{
				return null;
			}
			if (!corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false))
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Extract".Translate() + " " + HediffDefOf.MechlinkImplant.label, delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.RemoveMechlink, corpse);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(corpse), "ReservedBy", null);
		}
	}
}
