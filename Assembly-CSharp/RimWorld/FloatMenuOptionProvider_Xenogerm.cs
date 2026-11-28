using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D47 RID: 7495
	public class FloatMenuOptionProvider_Xenogerm : FloatMenuOptionProvider
	{
		// Token: 0x170023C6 RID: 9158
		// (get) Token: 0x0600C1F1 RID: 49649 RVA: 0x00469AC0 File Offset: 0x00467CC0
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023C7 RID: 9159
		// (get) Token: 0x0600C1F2 RID: 49650 RVA: 0x00469AC3 File Offset: 0x00467CC3
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023C8 RID: 9160
		// (get) Token: 0x0600C1F3 RID: 49651 RVA: 0x00469AC6 File Offset: 0x00467CC6
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C1F4 RID: 49652 RVA: 0x00469AC9 File Offset: 0x00467CC9
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && GeneUtility.CanAbsorbXenogerm(context.FirstSelectedPawn);
		}

		// Token: 0x0600C1F5 RID: 49653 RVA: 0x00469AE4 File Offset: 0x00467CE4
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.IsQuestLodger())
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "TemporaryFactionMember".Translate(context.FirstSelectedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (GeneUtility.SameXenotype(context.FirstSelectedPawn, clickedPawn))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "SameXenotype".Translate(context.FirstSelectedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedPawn.health.hediffSet.HasHediff(HediffDefOf.XenogermLossShock, false))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "XenogermLossShockPresent".Translate(clickedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant(clickedPawn, context.FirstSelectedPawn))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "IdeoligionForbids".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("AbsorbXenogerm".Translate(clickedPawn.Named("PAWN")), delegate()
			{
				if (clickedPawn.IsPrisonerOfColony && !clickedPawn.Downed)
				{
					Messages.Message("MessageTargetMustBeDownedToForceReimplant".Translate(clickedPawn.Named("PAWN")), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (GeneUtility.PawnWouldDieFromReimplanting(clickedPawn))
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text = "WarningPawnWillDieFromReimplanting".Translate(clickedPawn.Named("PAWN"));
					Action confirmedAct;
					if ((confirmedAct = <>9__1) == null)
					{
						confirmedAct = (<>9__1 = delegate()
						{
							GeneUtility.GiveReimplantJob(context.FirstSelectedPawn, clickedPawn);
						});
					}
					windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
					return;
				}
				GeneUtility.GiveReimplantJob(context.FirstSelectedPawn, clickedPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
