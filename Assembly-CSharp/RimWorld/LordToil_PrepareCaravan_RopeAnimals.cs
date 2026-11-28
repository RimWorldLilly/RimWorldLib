using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CBB RID: 3259
	public abstract class LordToil_PrepareCaravan_RopeAnimals : LordToil
	{
		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06005C8A RID: 23690 RVA: 0x0021FE01 File Offset: 0x0021E001
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06005C8B RID: 23691 RVA: 0x0021FE0D File Offset: 0x0021E00D
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x0021FE10 File Offset: 0x0021E010
		public LordToil_PrepareCaravan_RopeAnimals(IntVec3 destinationPoint, int? ropeeLimit)
		{
			this.destinationPoint = destinationPoint;
			this.ropeeLimit = ropeeLimit;
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x0021FE28 File Offset: 0x0021E028
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist || AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					pawn.mindState.duty = this.MakeRopeDuty();
					pawn.mindState.duty.ropeeLimit = this.ropeeLimit;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.destinationPoint, -1f);
				}
			}
		}

		// Token: 0x06005C8E RID: 23694
		protected abstract PawnDuty MakeRopeDuty();

		// Token: 0x040034C9 RID: 13513
		protected IntVec3 destinationPoint;

		// Token: 0x040034CA RID: 13514
		protected int? ropeeLimit;
	}
}
