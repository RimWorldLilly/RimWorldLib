using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D42 RID: 7490
	public class FloatMenuOptionProvider_Strip : FloatMenuOptionProvider
	{
		// Token: 0x170023B3 RID: 9139
		// (get) Token: 0x0600C1CD RID: 49613 RVA: 0x00468C59 File Offset: 0x00466E59
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B4 RID: 9140
		// (get) Token: 0x0600C1CE RID: 49614 RVA: 0x00468C5C File Offset: 0x00466E5C
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B5 RID: 9141
		// (get) Token: 0x0600C1CF RID: 49615 RVA: 0x00468C5F File Offset: 0x00466E5F
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023B6 RID: 9142
		// (get) Token: 0x0600C1D0 RID: 49616 RVA: 0x00468C62 File Offset: 0x00466E62
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1D1 RID: 49617 RVA: 0x00468C68 File Offset: 0x00466E68
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Pawn pawn = clickedThing as Pawn;
			if (!StrippableUtility.CanBeStrippedByColony(clickedThing))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotStrip".Translate(clickedThing.LabelCap, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (pawn != null && pawn.HasExtraHomeFaction(null))
			{
				return new FloatMenuOption("CannotStrip".Translate(pawn.LabelCap, pawn) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(clickedThing.LabelCap, clickedThing), delegate()
			{
				clickedThing.SetForbidden(false, false);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Strip, clickedThing), new JobTag?(JobTag.Misc), false);
				StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(clickedThing);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
