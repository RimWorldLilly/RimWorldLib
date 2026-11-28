using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D16 RID: 7446
	public class FloatMenuOptionProvider_CapturePawn : FloatMenuOptionProvider
	{
		// Token: 0x17002311 RID: 8977
		// (get) Token: 0x0600C09B RID: 49307 RVA: 0x00463A3D File Offset: 0x00461C3D
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002312 RID: 8978
		// (get) Token: 0x0600C09C RID: 49308 RVA: 0x00463A40 File Offset: 0x00461C40
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002313 RID: 8979
		// (get) Token: 0x0600C09D RID: 49309 RVA: 0x00463A43 File Offset: 0x00461C43
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002314 RID: 8980
		// (get) Token: 0x0600C09E RID: 49310 RVA: 0x00463A46 File Offset: 0x00461C46
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C09F RID: 49311 RVA: 0x00463A4C File Offset: 0x00461C4C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.CanBeCaptured())
			{
				return null;
			}
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			TaggedString taggedString = "Capture".Translate(clickedPawn.LabelCap, clickedPawn);
			if (!clickedPawn.guest.Recruitable)
			{
				taggedString += " (" + "Unrecruitable".Translate() + ")";
			}
			if (clickedPawn.Faction != null && clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden && !clickedPawn.Faction.HostileTo(Faction.OfPlayer) && !clickedPawn.IsPrisonerOfColony)
			{
				taggedString += ": " + "AngersFaction".Translate().CapitalizeFirst();
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
				}
				if (building_Bed == null)
				{
					Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Capture, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
				if (clickedPawn.Faction != null && clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden && !clickedPawn.Faction.HostileTo(Faction.OfPlayer) && !clickedPawn.IsPrisonerOfColony)
				{
					Messages.Message("MessageCapturingWillAngerFaction".Translate(clickedPawn.Named("PAWN")).AdjustedFor(clickedPawn, "PAWN", true), clickedPawn, MessageTypeDefOf.CautionInput, false);
				}
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotCapture".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NoPrisonerBed".Translate());
			string cannot = defaultInterpolatedStringHandler.ToStringAndClear();
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, new GuestStatus?(GuestStatus.Prisoner));
			return floatMenuOption;
		}
	}
}
