using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCD RID: 3277
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06005CDE RID: 23774 RVA: 0x00221508 File Offset: 0x0021F708
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x00221510 File Offset: 0x0021F710
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x00221520 File Offset: 0x0021F720
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x040034E8 RID: 13544
		public IntVec3 baseCenter;
	}
}
