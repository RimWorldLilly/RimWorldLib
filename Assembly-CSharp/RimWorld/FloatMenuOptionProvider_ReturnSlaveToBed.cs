using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D3F RID: 7487
	public class FloatMenuOptionProvider_ReturnSlaveToBed : FloatMenuOptionProvider
	{
		// Token: 0x170023A9 RID: 9129
		// (get) Token: 0x0600C1BA RID: 49594 RVA: 0x00468A79 File Offset: 0x00466C79
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023AA RID: 9130
		// (get) Token: 0x0600C1BB RID: 49595 RVA: 0x00468A7C File Offset: 0x00466C7C
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023AB RID: 9131
		// (get) Token: 0x0600C1BC RID: 49596 RVA: 0x00468A7F File Offset: 0x00466C7F
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023AC RID: 9132
		// (get) Token: 0x0600C1BD RID: 49597 RVA: 0x00468A82 File Offset: 0x00466C82
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1BE RID: 49598 RVA: 0x00468A88 File Offset: 0x00466C88
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsSlaveOfColony || clickedPawn.InMentalState)
			{
				return null;
			}
			if (!clickedPawn.Downed)
			{
				return null;
			}
			if (clickedPawn.InBed())
			{
				return null;
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ReturnToSlaveBed".Translate(), delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Slave));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Slave));
				}
				if (building_Bed == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CannotRescue".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NoSlaveBed".Translate());
					Messages.Message(defaultInterpolatedStringHandler2.ToStringAndClear(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Rescue, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotRescue".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NoSlaveBed".Translate());
			string cannot = defaultInterpolatedStringHandler.ToStringAndClear();
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, new GuestStatus?(GuestStatus.Slave));
			return floatMenuOption;
		}
	}
}
