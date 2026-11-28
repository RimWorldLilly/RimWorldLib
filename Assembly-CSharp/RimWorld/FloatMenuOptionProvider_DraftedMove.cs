using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D23 RID: 7459
	public class FloatMenuOptionProvider_DraftedMove : FloatMenuOptionProvider
	{
		// Token: 0x17002343 RID: 9027
		// (get) Token: 0x0600C102 RID: 49410 RVA: 0x00465DCF File Offset: 0x00463FCF
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002344 RID: 9028
		// (get) Token: 0x0600C103 RID: 49411 RVA: 0x00465DD2 File Offset: 0x00463FD2
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002345 RID: 9029
		// (get) Token: 0x0600C104 RID: 49412 RVA: 0x00465DD5 File Offset: 0x00463FD5
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002346 RID: 9030
		// (get) Token: 0x0600C105 RID: 49413 RVA: 0x00465DD8 File Offset: 0x00463FD8
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002347 RID: 9031
		// (get) Token: 0x0600C106 RID: 49414 RVA: 0x00465DDB File Offset: 0x00463FDB
		protected override bool IgnoreFogged
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C107 RID: 49415 RVA: 0x00465DE0 File Offset: 0x00463FE0
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			IntVec3 curLoc = CellFinder.StandableCellNear(context.ClickedCell, context.map, 2.9f, null);
			if (!curLoc.IsValid)
			{
				return null;
			}
			FloatMenuOption floatMenuOption;
			if (!context.IsMultiselect)
			{
				if (curLoc == context.FirstSelectedPawn.Position)
				{
					return null;
				}
				AcceptanceReport acceptanceReport = FloatMenuOptionProvider_DraftedMove.PawnCanGoto(context.FirstSelectedPawn, curLoc);
				if (!acceptanceReport.Accepted)
				{
					return new FloatMenuOption(acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				floatMenuOption = new FloatMenuOption("GoHere".Translate(), delegate()
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(context.ClickedCell, context.FirstSelectedPawn, RCellFinder.BestOrderedGotoDestNear(curLoc, context.FirstSelectedPawn, null, true));
				}, MenuOptionPriority.GoHere, null, null, 0f, null, null, true, 0);
			}
			else
			{
				FloatMenuOptionProvider_DraftedMove.tmpPawns.Clear();
				foreach (Pawn pawn in context.ValidSelectedPawns)
				{
					if (FloatMenuOptionProvider_DraftedMove.PawnCanGoto(pawn, curLoc).Accepted)
					{
						FloatMenuOptionProvider_DraftedMove.tmpPawns.Add(pawn);
					}
				}
				if (FloatMenuOptionProvider_DraftedMove.tmpPawns.Count == 0)
				{
					return null;
				}
				floatMenuOption = new FloatMenuOption("GoHere".Translate(), delegate()
				{
					Find.Selector.gotoController.StartInteraction(curLoc);
					foreach (Pawn pawn2 in FloatMenuOptionProvider_DraftedMove.tmpPawns)
					{
						Find.Selector.gotoController.AddPawn(pawn2);
					}
					Find.Selector.gotoController.FinalizeInteraction();
				}, MenuOptionPriority.GoHere, null, null, 0f, null, null, true, 0);
			}
			floatMenuOption.isGoto = true;
			floatMenuOption.autoTakeable = true;
			floatMenuOption.autoTakeablePriority = 10f;
			return floatMenuOption;
		}

		// Token: 0x0600C108 RID: 49416 RVA: 0x00465F90 File Offset: 0x00464190
		public static AcceptanceReport PawnCanGoto(Pawn pawn, IntVec3 gotoLoc)
		{
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, gotoLoc))
			{
				return "CannotGoOutOfRange".Translate() + ": " + "OutOfCommandRange".Translate();
			}
			if (!pawn.CanReach(gotoLoc, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "CannotGoNoPath".Translate();
			}
			return true;
		}

		// Token: 0x0600C109 RID: 49417 RVA: 0x0046600C File Offset: 0x0046420C
		public static void PawnGotoAction(IntVec3 clickCell, Pawn pawn, IntVec3 gotoLoc)
		{
			bool flag;
			if (pawn.Position == gotoLoc)
			{
				flag = true;
				if (pawn.CurJobDef == JobDefOf.Goto)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			}
			else if (pawn.CurJobDef == JobDefOf.Goto && pawn.CurJob.targetA.Cell == gotoLoc)
			{
				flag = true;
			}
			else
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, gotoLoc);
				if (pawn.Map.exitMapGrid.IsExitCell(clickCell))
				{
					job.exitMapOnArrival = !pawn.IsColonyMech;
				}
				else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(clickCell, 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID.ToString(), 60f))
				{
					Messages.Message(pawn.Map.Parent.GetComponent<FormCaravanComp>().CanFormOrReformCaravanNow ? "MessagePlayerTriedToLeaveMapViaExitGrid_CanReform".Translate() : "MessagePlayerTriedToLeaveMapViaExitGrid_CantReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
				}
				flag = pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}
			if (flag)
			{
				FleckMaker.Static(gotoLoc, pawn.Map, FleckDefOf.FeedbackGoto, 1f);
			}
		}

		// Token: 0x04006ED8 RID: 28376
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
