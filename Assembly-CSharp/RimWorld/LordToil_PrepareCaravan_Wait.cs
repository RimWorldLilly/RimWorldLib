using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC0 RID: 3264
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06005C9C RID: 23708 RVA: 0x0022027C File Offset: 0x0021E47C
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06005C9D RID: 23709 RVA: 0x00220288 File Offset: 0x0021E488
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x0022028B File Offset: 0x0021E48B
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x0022029C File Offset: 0x0021E49C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x040034CC RID: 13516
		private IntVec3 meetingPoint;
	}
}
