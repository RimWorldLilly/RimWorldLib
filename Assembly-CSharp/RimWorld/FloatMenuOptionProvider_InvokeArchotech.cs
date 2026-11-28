using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D32 RID: 7474
	public class FloatMenuOptionProvider_InvokeArchotech : FloatMenuOptionProvider
	{
		// Token: 0x1700237D RID: 9085
		// (get) Token: 0x0600C167 RID: 49511 RVA: 0x004679A2 File Offset: 0x00465BA2
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700237E RID: 9086
		// (get) Token: 0x0600C168 RID: 49512 RVA: 0x004679A5 File Offset: 0x00465BA5
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700237F RID: 9087
		// (get) Token: 0x0600C169 RID: 49513 RVA: 0x004679A8 File Offset: 0x00465BA8
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002380 RID: 9088
		// (get) Token: 0x0600C16A RID: 49514 RVA: 0x004679AB File Offset: 0x00465BAB
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C16B RID: 49515 RVA: 0x004679AE File Offset: 0x00465BAE
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x0600C16C RID: 49516 RVA: 0x004679B8 File Offset: 0x00465BB8
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_ArchonexusCore archCore = clickedThing as Building_ArchonexusCore;
			if (archCore == null)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(archCore, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!archCore.CanActivateNow)
			{
				return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, archCore), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, archCore, "ReservedBy", null);
		}
	}
}
