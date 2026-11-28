using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD7 RID: 3287
	public class LordToil_EscortPawn : LordToil
	{
		// Token: 0x06005D0A RID: 23818 RVA: 0x0022217B File Offset: 0x0022037B
		public LordToil_EscortPawn(Pawn escortee, float followRadius = 7f)
		{
			this.escortee = escortee;
			this.followRadius = followRadius;
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x0022219C File Offset: 0x0022039C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Escort, this.escortee, this.followRadius);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x040034F9 RID: 13561
		public Pawn escortee;

		// Token: 0x040034FA RID: 13562
		public float followRadius = 7f;
	}
}
