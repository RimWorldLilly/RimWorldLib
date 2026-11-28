using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB7 RID: 3255
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06005C75 RID: 23669 RVA: 0x0021FA52 File Offset: 0x0021DC52
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06005C76 RID: 23670 RVA: 0x0021FA5E File Offset: 0x0021DC5E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x0021FA61 File Offset: 0x0021DC61
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x0021FA78 File Offset: 0x0021DC78
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherDownedPawns, this.meetingPoint, this.exitSpot, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x0021FB14 File Offset: 0x0021DD14
		private void CheckAllPawnsArrived()
		{
			bool flag = true;
			List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)this.lord.LordJob).downedPawns;
			for (int i = 0; i < downedPawns.Count; i++)
			{
				if (!JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(downedPawns[i], this.exitSpot))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.lord.ReceiveMemo("AllDownedPawnsGathered");
			}
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x0021FB75 File Offset: 0x0021DD75
		public override void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
			this.CheckAllPawnsArrived();
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x0021FB7D File Offset: 0x0021DD7D
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.CheckAllPawnsArrived();
		}

		// Token: 0x040034C5 RID: 13509
		private IntVec3 meetingPoint;

		// Token: 0x040034C6 RID: 13510
		private IntVec3 exitSpot;
	}
}
