using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D3E RID: 7486
	public class FloatMenuOptionProvider_RescuePawn : FloatMenuOptionProvider
	{
		// Token: 0x170023A5 RID: 9125
		// (get) Token: 0x0600C1B4 RID: 49588 RVA: 0x0046882A File Offset: 0x00466A2A
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023A6 RID: 9126
		// (get) Token: 0x0600C1B5 RID: 49589 RVA: 0x0046882D File Offset: 0x00466A2D
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023A7 RID: 9127
		// (get) Token: 0x0600C1B6 RID: 49590 RVA: 0x00468830 File Offset: 0x00466A30
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023A8 RID: 9128
		// (get) Token: 0x0600C1B7 RID: 49591 RVA: 0x00468833 File Offset: 0x00466A33
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1B8 RID: 49592 RVA: 0x00468838 File Offset: 0x00466A38
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			if (clickedPawn.mindState.WillJoinColonyIfRescued)
			{
				return null;
			}
			if (clickedPawn.IsPrisonerOfColony || clickedPawn.IsSlaveOfColony || clickedPawn.IsColonyMech)
			{
				return null;
			}
			if (clickedPawn.Faction != null && clickedPawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			bool flag = ChildcareUtility.CanSuckle(clickedPawn, out breastfeedFailReason);
			bool flag2 = HealthAIUtility.ShouldSeekMedicalRest(clickedPawn) || !clickedPawn.ageTracker.CurLifeStage.alwaysDowned;
			if (flag || !flag2)
			{
				return null;
			}
			Pawn_PlayerSettings playerSettings = clickedPawn.playerSettings;
			if (playerSettings != null && playerSettings.medCare == MedicalCareCategory.NoCare)
			{
				return new FloatMenuOption("CannotRescuePawn".Translate(clickedPawn.Named("PAWN")) + ": " + "MedicalCareDisabled".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rescue".Translate(clickedPawn.LabelCap, clickedPawn), delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, null);
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, null);
				}
				if (building_Bed == null)
				{
					string t;
					if (clickedPawn.RaceProps.Animal)
					{
						t = "NoAnimalBed".Translate();
					}
					else
					{
						t = "NoNonPrisonerBed".Translate();
					}
					Messages.Message("CannotRescue".Translate() + ": " + t, clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Rescue, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			string key = clickedPawn.RaceProps.Animal ? "NoAnimalBed" : "NoNonPrisonerBed";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotRescue".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(key.Translate().CapitalizeFirst());
			string cannot = defaultInterpolatedStringHandler.ToStringAndClear();
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, null);
			return floatMenuOption;
		}
	}
}
