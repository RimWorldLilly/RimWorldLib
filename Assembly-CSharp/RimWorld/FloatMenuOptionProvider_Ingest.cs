using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D31 RID: 7473
	public class FloatMenuOptionProvider_Ingest : FloatMenuOptionProvider
	{
		// Token: 0x1700237A RID: 9082
		// (get) Token: 0x0600C162 RID: 49506 RVA: 0x004673FF File Offset: 0x004655FF
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700237B RID: 9083
		// (get) Token: 0x0600C163 RID: 49507 RVA: 0x00467402 File Offset: 0x00465602
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700237C RID: 9084
		// (get) Token: 0x0600C164 RID: 49508 RVA: 0x00467405 File Offset: 0x00465605
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C165 RID: 49509 RVA: 0x00467408 File Offset: 0x00465608
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (clickedThing.def.ingestible == null || !clickedThing.def.ingestible.showIngestFloatOption)
			{
				return null;
			}
			if (!clickedThing.IngestibleNow || !context.FirstSelectedPawn.RaceProps.CanEverEat(clickedThing.def))
			{
				return null;
			}
			string text;
			if (clickedThing.def.ingestible.ingestCommandString.NullOrEmpty())
			{
				text = "ConsumeThing".Translate(clickedThing.LabelShort, clickedThing);
			}
			else
			{
				text = clickedThing.def.ingestible.ingestCommandString.Formatted(clickedThing.LabelShort);
			}
			if (!clickedThing.IsSociallyProper(context.FirstSelectedPawn))
			{
				text = text + ": " + "ReservedForPrisoners".Translate().CapitalizeFirst();
			}
			else if (FoodUtility.MoodFromIngesting(context.FirstSelectedPawn, clickedThing, clickedThing.def) < 0f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("WarningFoodDisliked".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (!clickedThing.def.IsDrug && !clickedThing.def.ingestible.nonDrugIngestibleWithoutFoodNeed && !context.FirstSelectedPawn.FoodIsSuitable(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + "FoodNotSuitable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedThing.def.IsDrug && !context.FirstSelectedPawn.DrugIsSuitable(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + "DrugNotSuitable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (ModsConfig.IdeologyActive)
			{
				FloatMenuOption result;
				if (clickedThing.def.IsDrug && !new HistoryEvent(HistoryEventDefOf.IngestedDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text) && !PawnUtility.CanTakeDrugForDependency(context.FirstSelectedPawn, clickedThing.def))
				{
					return result;
				}
				if (clickedThing.def.IsNonMedicalDrug && !new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text) && !PawnUtility.CanTakeDrugForDependency(context.FirstSelectedPawn, clickedThing.def))
				{
					return result;
				}
				if (clickedThing.def.IsDrug && clickedThing.def.ingestible.drugCategory == DrugCategory.Hard && !new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text))
				{
					return result;
				}
			}
			if (clickedThing.def.IsNonMedicalDrug && !context.FirstSelectedPawn.CanTakeDrug(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + TraitDefOf.DrugDesire.DataAtDegree(-1).GetLabelCapFor(context.FirstSelectedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (FoodUtility.InappropriateForTitle(clickedThing.def, context.FirstSelectedPawn, true))
			{
				return new FloatMenuOption(text + ": " + "FoodBelowTitleRequirements".Translate(context.FirstSelectedPawn.royalty.MostSeniorTitle.def.GetLabelFor(context.FirstSelectedPawn).CapitalizeFirst()).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			bool maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(clickedThing, context.FirstSelectedPawn, FoodUtility.WillIngestStackCountOf(context.FirstSelectedPawn, clickedThing.def, FoodUtility.NutritionForEater(context.FirstSelectedPawn, clickedThing))) != 0;
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				int maxAmountToPickup2 = FoodUtility.GetMaxAmountToPickup(clickedThing, context.FirstSelectedPawn, FoodUtility.WillIngestStackCountOf(context.FirstSelectedPawn, clickedThing.def, FoodUtility.NutritionForEater(context.FirstSelectedPawn, clickedThing)));
				if (maxAmountToPickup2 == 0)
				{
					return;
				}
				clickedThing.SetForbidden(false, true);
				Job job = JobMaker.MakeJob(JobDefOf.Ingest, clickedThing);
				job.count = maxAmountToPickup2;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, (clickedThing is Corpse) ? MenuOptionPriority.Low : MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			if (!maxAmountToPickup)
			{
				floatMenuOption.action = null;
			}
			return floatMenuOption;
		}
	}
}
