using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D2F RID: 7471
	public class FloatMenuOptionProvider_HackAncientTerminal : FloatMenuOptionProvider
	{
		// Token: 0x17002372 RID: 9074
		// (get) Token: 0x0600C156 RID: 49494 RVA: 0x004672E9 File Offset: 0x004654E9
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002373 RID: 9075
		// (get) Token: 0x0600C157 RID: 49495 RVA: 0x004672EC File Offset: 0x004654EC
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002374 RID: 9076
		// (get) Token: 0x0600C158 RID: 49496 RVA: 0x004672EF File Offset: 0x004654EF
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002375 RID: 9077
		// (get) Token: 0x0600C159 RID: 49497 RVA: 0x004672F2 File Offset: 0x004654F2
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C15A RID: 49498 RVA: 0x004672F8 File Offset: 0x004654F8
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!ModsConfig.IdeologyActive || clickedThing.def != ThingDefOf.AncientEnemyTerminal)
			{
				return null;
			}
			CompHackable compHackable;
			if (!clickedThing.TryGetComp(out compHackable) || compHackable.Props.onlyRemotelyHackable)
			{
				return null;
			}
			if (!compHackable.CanHackNow(context.FirstSelectedPawn).Accepted)
			{
				return null;
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Hack".Translate(clickedThing.Label), delegate()
			{
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = "ConfirmHackEnemyTerminal".Translate(ThingDefOf.AncientEnemyTerminal.label);
				Action confirmedAct;
				if ((confirmedAct = <>9__1) == null)
				{
					confirmedAct = (<>9__1 = delegate()
					{
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, clickedThing), new JobTag?(JobTag.Misc), false);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedThing), "ReservedBy", null);
		}
	}
}
