using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D2A RID: 7466
	public class FloatMenuOptionProvider_ExtinguishFires : FloatMenuOptionProvider
	{
		// Token: 0x17002361 RID: 9057
		// (get) Token: 0x0600C139 RID: 49465 RVA: 0x00466F98 File Offset: 0x00465198
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002362 RID: 9058
		// (get) Token: 0x0600C13A RID: 49466 RVA: 0x00466F9B File Offset: 0x0046519B
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002363 RID: 9059
		// (get) Token: 0x0600C13B RID: 49467 RVA: 0x00466F9E File Offset: 0x0046519E
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002364 RID: 9060
		// (get) Token: 0x0600C13C RID: 49468 RVA: 0x00466FA1 File Offset: 0x004651A1
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C13D RID: 49469 RVA: 0x00466FA4 File Offset: 0x004651A4
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			if (!new TargetInfo(context.ClickedCell, context.FirstSelectedPawn.Map, false).IsBurning())
			{
				return null;
			}
			bool flag = false;
			AcceptanceReport acceptanceReport = null;
			foreach (Pawn pawn in context.ValidSelectedPawns)
			{
				acceptanceReport = this.PawnCanExtinguish(pawn, context.ClickedCell);
				if (acceptanceReport.Accepted)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotGenericWorkCustom".Translate(WorkGiverDefOf.FightFires.label));
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(acceptanceReport.Reason);
				return new FloatMenuOption(defaultInterpolatedStringHandler.ToStringAndClear(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return new FloatMenuOption("ExtinguishFiresNearby".Translate(), delegate()
			{
				foreach (Pawn pawn2 in context.ValidSelectedPawns)
				{
					if (this.PawnCanExtinguish(pawn2, context.ClickedCell).Accepted)
					{
						Job job = JobMaker.MakeJob(JobDefOf.ExtinguishFiresNearby);
						foreach (Fire t in context.ClickedCell.GetFiresNearCell(context.FirstSelectedPawn.Map))
						{
							job.AddQueuedTarget(TargetIndex.A, t);
						}
						pawn2.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}
				}
			}, ThingDefOf.Fire, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
		}

		// Token: 0x0600C13E RID: 49470 RVA: 0x004670F8 File Offset: 0x004652F8
		private AcceptanceReport PawnCanExtinguish(Pawn pawn, IntVec3 cell)
		{
			if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter))
			{
				return "IncapableOf".Translate().CapitalizeFirst() + " " + WorkTypeDefOf.Firefighter.gerundLabel;
			}
			Fire t;
			if (!pawn.CanReach(cell, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (!cell.TryGetFirstThing(pawn.Map, out t) || !pawn.CanReach(t, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			if (!cell.InAllowedArea(pawn))
			{
				return "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " ( " + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")";
			}
			return true;
		}
	}
}
