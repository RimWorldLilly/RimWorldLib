using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D28 RID: 7464
	public class FloatMenuOptionProvider_EnterMapPortal : FloatMenuOptionProvider
	{
		// Token: 0x1700235A RID: 9050
		// (get) Token: 0x0600C12B RID: 49451 RVA: 0x004668B0 File Offset: 0x00464AB0
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700235B RID: 9051
		// (get) Token: 0x0600C12C RID: 49452 RVA: 0x004668B3 File Offset: 0x00464AB3
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700235C RID: 9052
		// (get) Token: 0x0600C12D RID: 49453 RVA: 0x004668B6 File Offset: 0x00464AB6
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700235D RID: 9053
		// (get) Token: 0x0600C12E RID: 49454 RVA: 0x004668B9 File Offset: 0x00464AB9
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C12F RID: 49455 RVA: 0x004668BC File Offset: 0x00464ABC
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			MapPortal portal = clickedThing as MapPortal;
			if (portal == null)
			{
				return null;
			}
			string t;
			if (!portal.IsEnterable(out t))
			{
				return new FloatMenuOption("CannotEnterPortal".Translate(portal.Label) + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.IsMultiselect)
			{
				AcceptanceReport acceptanceReport = FloatMenuOptionProvider_EnterMapPortal.CanEnterPortal(context.FirstSelectedPawn, portal);
				if (!acceptanceReport.Accepted)
				{
					return new FloatMenuOption("CannotEnterPortal".Translate(portal.Label) + ": " + acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.Clear();
			foreach (Pawn item in context.ValidSelectedPawns)
			{
				if (FloatMenuOptionProvider_EnterMapPortal.CanEnterPortal(context.FirstSelectedPawn, portal))
				{
					FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.Add(item);
				}
			}
			if (FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.NullOrEmpty<Pawn>())
			{
				return null;
			}
			return new FloatMenuOption(portal.EnterString, delegate()
			{
				foreach (Pawn pawn in FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns)
				{
					Job job = JobMaker.MakeJob(JobDefOf.EnterPortal, portal);
					job.playerForced = true;
					pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x0600C130 RID: 49456 RVA: 0x00466A40 File Offset: 0x00464C40
		private static AcceptanceReport CanEnterPortal(Pawn pawn, MapPortal portal)
		{
			if (!pawn.CanReach(portal, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return "Incapable".Translate();
			}
			return true;
		}

		// Token: 0x04006ED9 RID: 28377
		private static List<Pawn> tmpPortalEnteringPawns = new List<Pawn>();
	}
}
