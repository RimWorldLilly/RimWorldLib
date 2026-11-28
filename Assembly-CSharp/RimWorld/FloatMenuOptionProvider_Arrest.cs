using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D13 RID: 7443
	public class FloatMenuOptionProvider_Arrest : FloatMenuOptionProvider
	{
		// Token: 0x17002304 RID: 8964
		// (get) Token: 0x0600C086 RID: 49286 RVA: 0x004636FC File Offset: 0x004618FC
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002305 RID: 8965
		// (get) Token: 0x0600C087 RID: 49287 RVA: 0x004636FF File Offset: 0x004618FF
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002306 RID: 8966
		// (get) Token: 0x0600C088 RID: 49288 RVA: 0x00463702 File Offset: 0x00461902
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002307 RID: 8967
		// (get) Token: 0x0600C089 RID: 49289 RVA: 0x00463705 File Offset: 0x00461905
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C08A RID: 49290 RVA: 0x00463708 File Offset: 0x00461908
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.CanBeArrestedBy(context.FirstSelectedPawn))
			{
				return null;
			}
			if (clickedPawn.Downed && clickedPawn.guilt.IsGuilty)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.Drafted && (!clickedPawn.IsWildMan() || clickedPawn.IsPrisonerOfColony))
			{
				return null;
			}
			if (context.FirstSelectedPawn.InSameExtraFaction(clickedPawn, ExtraFactionType.HomeFaction, null) || context.FirstSelectedPawn.InSameExtraFaction(clickedPawn, ExtraFactionType.MiniFaction, null))
			{
				return new FloatMenuOption("CannotArrest".Translate() + ": " + "SameFaction".Translate(clickedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotArrest".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action action = delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
				}
				if (building_Bed == null)
				{
					Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Arrest, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				if (clickedPawn.Faction != null && ((clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden) || clickedPawn.IsQuestLodger()))
				{
					TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies, new string[]
					{
						clickedPawn.GetAcceptArrestChance(context.FirstSelectedPawn).ToStringPercent()
					});
				}
			};
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TryToArrest".Translate(clickedPawn.LabelCap, clickedPawn, clickedPawn.GetAcceptArrestChance(context.FirstSelectedPawn).ToStringPercent()), action, MenuOptionPriority.High, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
