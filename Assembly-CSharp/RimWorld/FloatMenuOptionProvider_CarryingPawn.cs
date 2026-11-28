using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D18 RID: 7448
	public class FloatMenuOptionProvider_CarryingPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002319 RID: 8985
		// (get) Token: 0x0600C0A8 RID: 49320 RVA: 0x00463E3B File Offset: 0x0046203B
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700231A RID: 8986
		// (get) Token: 0x0600C0A9 RID: 49321 RVA: 0x00463E3E File Offset: 0x0046203E
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700231B RID: 8987
		// (get) Token: 0x0600C0AA RID: 49322 RVA: 0x00463E41 File Offset: 0x00462041
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700231C RID: 8988
		// (get) Token: 0x0600C0AB RID: 49323 RVA: 0x00463E44 File Offset: 0x00462044
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0AC RID: 49324 RVA: 0x00463E47 File Offset: 0x00462047
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.IsCarryingPawn(null);
		}

		// Token: 0x0600C0AD RID: 49325 RVA: 0x00463E55 File Offset: 0x00462055
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_CarryingPawn.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_CarryingPawn.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C0AE RID: 49326 RVA: 0x00463E74 File Offset: 0x00462074
		private bool CarryToBed(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			if (carriedPawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, false, true, true, carriedPawn.GuestStatus))
			{
				return false;
			}
			if (context.FirstSelectedPawn.HostileTo(carriedPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "CarriedPawnHostile".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.TakeDownedPawnToBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x0600C0AF RID: 49327 RVA: 0x00464010 File Offset: 0x00462210
		private bool Capture(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			if (!carriedPawn.CanBeCaptured())
			{
				return false;
			}
			if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, false, true, true, new GuestStatus?(GuestStatus.Prisoner)))
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
			if (!carriedPawn.guest.Recruitable)
			{
				TaggedString t = taggedString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Unrecruitable".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (carriedPawn.Faction != null && carriedPawn.Faction != Faction.OfPlayer && !carriedPawn.Faction.Hidden && !carriedPawn.Faction.HostileTo(Faction.OfPlayer) && !carriedPawn.IsPrisonerOfColony)
			{
				TaggedString t2 = taggedString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("AngersFaction".Translate().CapitalizeFirst());
				taggedString = t2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryToPrisonerBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x0600C0B0 RID: 49328 RVA: 0x00464208 File Offset: 0x00462408
		private bool CarryToTransporter(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			CompTransporter transporter;
			if (!clickedThing.TryGetComp(out transporter))
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (transporter.Shuttle != null)
			{
				if (!transporter.Shuttle.IsAllowedNow(carriedPawn))
				{
					return false;
				}
				if (!transporter.Shuttle.IsPlayerShuttle && !transporter.LeftToLoadContains(carriedPawn))
				{
					option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NotPartOfLaunchGroup".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					return true;
				}
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate()
			{
				if (!transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, carriedPawn, clickedThing);
				job.ignoreForbidden = true;
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x0600C0B1 RID: 49329 RVA: 0x004643D4 File Offset: 0x004625D4
		private bool CarryToCasket(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			Building_CryptosleepCasket casket = clickedThing as Building_CryptosleepCasket;
			if (casket == null)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (casket.HasAnyContents)
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketOccupied".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (carriedPawn.IsQuestLodger())
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestsNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (carriedPawn.GetExtraHostFaction(null) != null)
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestPrisonersNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, casket), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasketDrafted, carriedPawn, casket);
				job.count = 1;
				job.playerForced = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, casket, "ReservedBy", null);
			return true;
		}

		// Token: 0x0600C0B2 RID: 49330 RVA: 0x00464618 File Offset: 0x00462818
		private bool CaptureEntity(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (!carriedPawn.TryGetComp(out compHoldingPlatformTarget) || !compHoldingPlatformTarget.CanBeCaptured || !compHoldingPlatformTarget.StudiedAtHoldingPlatform)
			{
				return false;
			}
			CompEntityHolder compEntityHolder;
			if (!clickedThing.TryGetComp(out compEntityHolder) || !compEntityHolder.Available)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
			if (!clickedThing.SafelyContains(carriedPawn))
			{
				float statValue = carriedPawn.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
				TaggedString t = taggedString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 4);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FloatMenuContainmentStrength".Translate().ToLower());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(compEntityHolder.ContainmentStrength, "F0");
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FloatMenuContainmentRequires".Translate(carriedPawn).ToLower());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(statValue, "F0");
				defaultInterpolatedStringHandler.AppendLiteral(")");
				taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolderAlreadyHolding, clickedThing, context.FirstSelectedPawn.carryTracker.CarriedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}
	}
}
