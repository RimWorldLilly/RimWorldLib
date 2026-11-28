using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D3A RID: 7482
	public class FloatMenuOptionProvider_PutOutFireOnPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002397 RID: 9111
		// (get) Token: 0x0600C197 RID: 49559 RVA: 0x004680DB File Offset: 0x004662DB
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002398 RID: 9112
		// (get) Token: 0x0600C198 RID: 49560 RVA: 0x004680DE File Offset: 0x004662DE
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002399 RID: 9113
		// (get) Token: 0x0600C199 RID: 49561 RVA: 0x004680E1 File Offset: 0x004662E1
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C19A RID: 49562 RVA: 0x004680E4 File Offset: 0x004662E4
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn == null || clickedPawn.Dead || !clickedPawn.IsBurning())
			{
				return null;
			}
			if ((clickedPawn.Faction == null || clickedPawn.Faction != context.FirstSelectedPawn.Faction) && (clickedPawn.HostFaction == null || (clickedPawn.HostFaction != context.FirstSelectedPawn.Faction && clickedPawn.HostFaction != context.FirstSelectedPawn.HostFaction)))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotGenericWorkCustom".Translate(WorkGiverDefOf.FightFires.label));
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NoPath".Translate().CapitalizeFirst());
				return new FloatMenuOption(defaultInterpolatedStringHandler.ToStringAndClear(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Thing fire = clickedPawn.GetAttachment(ThingDefOf.Fire);
			if (fire == null)
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PrioritizeGeneric".Translate(WorkGiverDefOf.FightFires.gerund, clickedPawn.Label).CapitalizeFirst(), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.BeatFire, fire);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, fire, "ReservedBy", null);
		}
	}
}
