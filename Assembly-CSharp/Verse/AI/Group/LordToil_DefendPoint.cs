using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000944 RID: 2372
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x001C8EDD File Offset: 0x001C70DD
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06004900 RID: 18688 RVA: 0x001C8EEA File Offset: 0x001C70EA
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06004901 RID: 18689 RVA: 0x001C8EF7 File Offset: 0x001C70F7
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x001C8EFF File Offset: 0x001C70FF
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x001C8F20 File Offset: 0x001C7120
		public LordToil_DefendPoint(IntVec3 defendPoint, float? defendRadius = null, float? wanderRadius = null) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius.GetValueOrDefault(28f);
			this.Data.wanderRadius = wanderRadius;
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x001C8F58 File Offset: 0x001C7158
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (((pawn != null) ? pawn.mindState : null) != null)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
					pawn.mindState.duty.focusSecond = data.defendPoint;
					pawn.mindState.duty.radius = ((pawn.kindDef.defendPointRadius >= 0f) ? pawn.kindDef.defendPointRadius : data.defendRadius);
					pawn.mindState.duty.wanderRadius = data.wanderRadius;
				}
			}
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x001C903B File Offset: 0x001C723B
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x04002EEA RID: 12010
		private bool allowSatisfyLongNeeds = true;
	}
}
