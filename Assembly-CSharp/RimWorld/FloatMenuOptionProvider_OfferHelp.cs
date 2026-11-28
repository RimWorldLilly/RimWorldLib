using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D36 RID: 7478
	public class FloatMenuOptionProvider_OfferHelp : FloatMenuOptionProvider
	{
		// Token: 0x1700238B RID: 9099
		// (get) Token: 0x0600C181 RID: 49537 RVA: 0x00467BD4 File Offset: 0x00465DD4
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700238C RID: 9100
		// (get) Token: 0x0600C182 RID: 49538 RVA: 0x00467BD7 File Offset: 0x00465DD7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700238D RID: 9101
		// (get) Token: 0x0600C183 RID: 49539 RVA: 0x00467BDA File Offset: 0x00465DDA
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C184 RID: 49540 RVA: 0x00467BE0 File Offset: 0x00465DE0
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn.Dead || !clickedPawn.mindState.WillJoinColonyIfRescued)
			{
				return null;
			}
			TaggedString taggedString = clickedPawn.IsPrisoner ? "FreePrisoner".Translate() : "OfferHelp".Translate();
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(taggedString + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OfferHelp, clickedPawn), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
