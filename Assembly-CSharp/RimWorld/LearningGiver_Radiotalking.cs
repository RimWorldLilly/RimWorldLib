using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B92 RID: 2962
	public class LearningGiver_Radiotalking : LearningGiver
	{
		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x060055E0 RID: 21984 RVA: 0x001FF575 File Offset: 0x001FD775
		public override bool CanGiveDesire
		{
			get
			{
				return ResearchProjectDefOf.MicroelectronicsBasics.IsFinished;
			}
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x001FF584 File Offset: 0x001FD784
		private bool TryFindCommsConsole(Pawn pawn, out Thing commsConsole)
		{
			commsConsole = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.CommsConsole), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Building_CommsConsole building_CommsConsole = t as Building_CommsConsole;
				return building_CommsConsole != null && building_CommsConsole.CanUseCommsNow && pawn.CanReserve(building_CommsConsole, 1, -1, null, false) && !building_CommsConsole.IsForbidden(pawn);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
			return commsConsole != null;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x001FF5F8 File Offset: 0x001FD7F8
		public override bool CanDo(Pawn pawn)
		{
			Thing thing;
			return base.CanDo(pawn) && this.TryFindCommsConsole(pawn, out thing);
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x001FF61C File Offset: 0x001FD81C
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing t;
			if (!this.TryFindCommsConsole(pawn, out t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}
	}
}
