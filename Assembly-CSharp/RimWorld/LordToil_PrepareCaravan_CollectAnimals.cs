using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CBD RID: 3261
	public class LordToil_PrepareCaravan_CollectAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06005C93 RID: 23699 RVA: 0x0021FF6D File Offset: 0x0021E16D
		public LordToil_PrepareCaravan_CollectAnimals(IntVec3 destinationPoint) : base(destinationPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x0021FF80 File Offset: 0x0021E180
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_CollectAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x0021FF9C File Offset: 0x0021E19C
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(pawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllAnimalsCollected");
				}
			}
		}
	}
}
