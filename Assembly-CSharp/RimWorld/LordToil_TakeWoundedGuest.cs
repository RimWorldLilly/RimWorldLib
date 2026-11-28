using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFD RID: 3325
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06005DBB RID: 23995 RVA: 0x0022551B File Offset: 0x0022371B
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06005DBC RID: 23996 RVA: 0x0022551E File Offset: 0x0022371E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x00225524 File Offset: 0x00223724
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
