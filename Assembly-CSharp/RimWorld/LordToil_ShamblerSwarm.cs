using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF3 RID: 3315
	public class LordToil_ShamblerSwarm : LordToil_EntitySwarm
	{
		// Token: 0x06005D8D RID: 23949 RVA: 0x0022425B File Offset: 0x0022245B
		public LordToil_ShamblerSwarm(IntVec3 start, IntVec3 dest) : base(start, dest)
		{
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x00224265 File Offset: 0x00222465
		protected override DutyDef GetDutyDef()
		{
			return DutyDefOf.ShamblerSwarm;
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x0022426C File Offset: 0x0022246C
		public override bool CanAddPawn(Pawn p)
		{
			return p.IsShambler;
		}
	}
}
