using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D46 RID: 7494
	public class FloatMenuOptionProvider_WorkGivers : FloatMenuOptionProvider
	{
		// Token: 0x170023C1 RID: 9153
		// (get) Token: 0x0600C1E5 RID: 49637 RVA: 0x004691F8 File Offset: 0x004673F8
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023C2 RID: 9154
		// (get) Token: 0x0600C1E6 RID: 49638 RVA: 0x004691FB File Offset: 0x004673FB
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023C3 RID: 9155
		// (get) Token: 0x0600C1E7 RID: 49639 RVA: 0x004691FE File Offset: 0x004673FE
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023C4 RID: 9156
		// (get) Token: 0x0600C1E8 RID: 49640 RVA: 0x00469201 File Offset: 0x00467401
		protected override bool MechanoidCanDo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023C5 RID: 9157
		// (get) Token: 0x0600C1E9 RID: 49641 RVA: 0x00469204 File Offset: 0x00467404
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1EA RID: 49642 RVA: 0x00469207 File Offset: 0x00467407
		public override IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			FloatMenuOptionProvider_WorkGivers.<GetOptions>d__13 <GetOptions>d__ = new FloatMenuOptionProvider_WorkGivers.<GetOptions>d__13(-2);
			<GetOptions>d__.<>4__this = this;
			<GetOptions>d__.<>3__context = context;
			return <GetOptions>d__;
		}

		// Token: 0x0600C1EB RID: 49643 RVA: 0x0046921E File Offset: 0x0046741E
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_WorkGivers.<GetOptionsFor>d__14 <GetOptionsFor>d__ = new FloatMenuOptionProvider_WorkGivers.<GetOptionsFor>d__14(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C1EC RID: 49644 RVA: 0x0046923C File Offset: 0x0046743C
		private IEnumerable<FloatMenuOption> GetWorkGiversOptionsFor(Pawn pawn, LocalTargetInfo target, FloatMenuContext context)
		{
			FloatMenuOptionProvider_WorkGivers.<GetWorkGiversOptionsFor>d__16 <GetWorkGiversOptionsFor>d__ = new FloatMenuOptionProvider_WorkGivers.<GetWorkGiversOptionsFor>d__16(-2);
			<GetWorkGiversOptionsFor>d__.<>4__this = this;
			<GetWorkGiversOptionsFor>d__.<>3__pawn = pawn;
			<GetWorkGiversOptionsFor>d__.<>3__target = target;
			<GetWorkGiversOptionsFor>d__.<>3__context = context;
			return <GetWorkGiversOptionsFor>d__;
		}

		// Token: 0x0600C1ED RID: 49645 RVA: 0x00469264 File Offset: 0x00467464
		private FloatMenuOption GetWorkGiverOption(Pawn pawn, WorkGiverDef workGiver, LocalTargetInfo target, FloatMenuContext context)
		{
			if (pawn.Drafted && !workGiver.canBeDoneWhileDrafted)
			{
				return null;
			}
			WorkGiver_Scanner workGiver_Scanner = workGiver.Worker as WorkGiver_Scanner;
			if (workGiver_Scanner == null || !workGiver_Scanner.def.directOrderable)
			{
				return null;
			}
			JobFailReason.Clear();
			if (target.HasThing)
			{
				if (FloatMenuOptionProvider_WorkGivers.ScannerShouldSkip(pawn, workGiver_Scanner, target.Thing))
				{
					return null;
				}
			}
			else if (!workGiver_Scanner.PotentialWorkCellsGlobal(pawn).Contains(target.Cell) || workGiver_Scanner.ShouldSkip(pawn, true))
			{
				return null;
			}
			Action action = null;
			Job job;
			if (target.HasThing)
			{
				if (!workGiver_Scanner.HasJobOnThing(pawn, target.Thing, true))
				{
					job = null;
				}
				else
				{
					job = workGiver_Scanner.JobOnThing(pawn, target.Thing, true);
				}
			}
			else if (!workGiver_Scanner.HasJobOnCell(pawn, target.Cell, true))
			{
				job = null;
			}
			else
			{
				job = workGiver_Scanner.JobOnCell(pawn, target.Cell, true);
			}
			string text;
			if (!JobFailReason.CustomJobString.NullOrEmpty())
			{
				text = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString).CapitalizeFirst();
			}
			else if (target.HasThing)
			{
				text = "CannotGenericWork".Translate(workGiver.verb, target.Thing.Label);
			}
			else
			{
				text = "CannotGenericWork".Translate(workGiver.verb, "AreaLower".Translate());
			}
			if (job == null)
			{
				if (!JobFailReason.HaveReason)
				{
					return null;
				}
				text = text + ": " + JobFailReason.Reason.CapitalizeFirst();
			}
			else
			{
				WorkTypeDef workType = workGiver_Scanner.def.workType;
				PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
				if (pawnCapacityDef != null)
				{
					text += ": " + "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
				}
				else if (pawn.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
				{
					text += ": " + "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
				}
				else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(pawn, job))
				{
					if (target.HasThing)
					{
						text = "CannotGenericAlreadyAm".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.LabelShort, target.Thing);
					}
					else
					{
						text = "CannotGenericAlreadyAmCustom".Translate(workGiver_Scanner.PostProcessedGerund(job));
					}
				}
				else if (pawn.workSettings.GetPriority(workType) == 0)
				{
					if (pawn.WorkTypeIsDisabled(workType))
					{
						text += ": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel);
					}
					else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
					{
						text += ": " + "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel);
					}
					else
					{
						text += ": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.pawnLabel);
					}
				}
				else if (job.def == JobDefOf.Research && target.Thing is Building_ResearchBench)
				{
					text += ": " + "CannotPrioritizeResearch".Translate();
				}
				else if (target.HasThing && target.Thing.IsForbidden(pawn))
				{
					if (!target.Thing.Position.InAllowedArea(pawn))
					{
						text += ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")");
					}
					else
					{
						text += ": " + "CannotPrioritizeForbidden".Translate(target.Thing.Label, target.Thing);
					}
				}
				else if (!target.HasThing && target.Cell.IsForbidden(pawn))
				{
					if (!target.Cell.InAllowedArea(pawn))
					{
						text += ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")");
					}
					else
					{
						text += ": " + "CannotPrioritizeCellForbidden".Translate();
					}
				}
				else if ((target.HasThing && !pawn.CanReach(target.Thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, false, TraverseMode.ByPawn)) || (!target.HasThing && !pawn.CanReach(target.Cell, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
				{
					text += ": " + "NoPath".Translate().CapitalizeFirst();
				}
				else
				{
					if (target.HasThing)
					{
						text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.Label).CapitalizeFirst();
					}
					else if (job.def.displayAsAreaInFloatMenu)
					{
						text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), "AreaLower".Translate()).CapitalizeFirst();
					}
					else
					{
						text = "PrioritizeGenericSimple".Translate(workGiver_Scanner.PostProcessedGerund(job)).CapitalizeFirst();
					}
					Job localJob = job;
					WorkGiver_Scanner localScanner = workGiver_Scanner;
					job.workGiverDef = workGiver_Scanner.def;
					WorkGiverDef giver = workGiver;
					action = delegate()
					{
						if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, context.ClickedCell))
						{
							if (giver.forceMote != null)
							{
								MoteMaker.MakeStaticMote(context.ClickedCell, pawn.Map, giver.forceMote, 1f);
							}
							if (giver.forceFleck != null)
							{
								FleckMaker.Static(context.ClickedCell, pawn.Map, giver.forceFleck, 1f);
							}
						}
					};
				}
			}
			if (DebugViewSettings.showFloatMenuWorkGivers)
			{
				text += string.Format(" (from {0})", workGiver.defName);
			}
			FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, target, "ReservedBy", null);
			if (FloatMenuOptionProvider_WorkGivers.tmpUsedLabels.Any((string used) => used == menuOption.Label))
			{
				return null;
			}
			if (pawn.Drafted && workGiver.autoTakeablePriorityDrafted != -1)
			{
				menuOption.autoTakeable = true;
				menuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
			}
			if (target.HasThing && workGiver.equivalenceGroup != null)
			{
				if (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] == null || (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
				{
					FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] = menuOption;
					FloatMenuOptionProvider_WorkGivers.shouldProcessEquivalenceGroups = true;
				}
				return null;
			}
			return menuOption;
		}

		// Token: 0x0600C1EE RID: 49646 RVA: 0x00469A5C File Offset: 0x00467C5C
		private static bool ScannerShouldSkip(Pawn pawn, WorkGiver_Scanner scanner, Thing t)
		{
			return (!scanner.PotentialWorkThingRequest.Accepts(t) && (scanner.PotentialWorkThingsGlobal(pawn) == null || !scanner.PotentialWorkThingsGlobal(pawn).Contains(t))) || scanner.ShouldSkip(pawn, true);
		}

		// Token: 0x04006EDB RID: 28379
		private static bool shouldProcessEquivalenceGroups = false;

		// Token: 0x04006EDC RID: 28380
		private static FloatMenuOption[] equivalenceGroupTempStorage;

		// Token: 0x04006EDD RID: 28381
		private static HashSet<string> tmpUsedLabels = new HashSet<string>();

		// Token: 0x04006EDE RID: 28382
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
