using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD3 RID: 3283
	public abstract class LordToil_DoOpportunisticTaskOrCover : LordToil
	{
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06005CF3 RID: 23795 RVA: 0x00221B14 File Offset: 0x0021FD14
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06005CF4 RID: 23796
		protected abstract DutyDef DutyDef { get; }

		// Token: 0x06005CF5 RID: 23797
		protected abstract bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets);

		// Token: 0x06005CF6 RID: 23798 RVA: 0x00221B18 File Offset: 0x0021FD18
		public override void UpdateAllDuties()
		{
			List<Thing> list = null;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				Thing item = null;
				if (!this.cover || (this.TryFindGoodOpportunisticTaskTarget(pawn, out item, list) && !GenAI.InDangerousCombat(pawn)))
				{
					if (pawn.mindState.duty == null || pawn.mindState.duty.def != this.DutyDef)
					{
						pawn.mindState.duty = new PawnDuty(this.DutyDef);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					if (list == null)
					{
						list = new List<Thing>();
					}
					list.Add(item);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		// Token: 0x06005CF7 RID: 23799 RVA: 0x00221BEC File Offset: 0x0021FDEC
		public override void LordToilTick()
		{
			if (this.cover && Find.TickManager.TicksGame % 181 == 0)
			{
				List<Thing> list = null;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Downed && pawn.mindState.duty.def == DutyDefOf.AssaultColony)
					{
						Thing thing = null;
						if (this.TryFindGoodOpportunisticTaskTarget(pawn, out thing, list) && !base.Map.reservationManager.IsReservedByAnyoneOf(thing, this.lord.faction) && !GenAI.InDangerousCombat(pawn))
						{
							pawn.mindState.duty = new PawnDuty(this.DutyDef);
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							if (list == null)
							{
								list = new List<Thing>();
							}
							list.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x040034EE RID: 13550
		public bool cover = true;
	}
}
