using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200094E RID: 2382
	public class LordToil_Travel : LordToil
	{
		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x001C9443 File Offset: 0x001C7643
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004922 RID: 18722 RVA: 0x001C9450 File Offset: 0x001C7650
		protected LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004923 RID: 18723 RVA: 0x001C945D File Offset: 0x001C765D
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004924 RID: 18724 RVA: 0x001C9460 File Offset: 0x001C7660
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x001C9467 File Offset: 0x001C7667
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x001C9488 File Offset: 0x001C7688
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.TravelOrLeave, data.dest, -1f);
				pawnDuty.maxDanger = this.maxDanger;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x001C94FC File Offset: 0x001C76FC
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("TravelArrived");
				}
			}
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x001C95A0 File Offset: 0x001C77A0
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x001C95B2 File Offset: 0x001C77B2
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		// Token: 0x04002EF5 RID: 12021
		public Danger maxDanger;
	}
}
