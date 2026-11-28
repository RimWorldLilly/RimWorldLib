using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CBE RID: 3262
	public class LordToil_ReturnedCaravan_PenAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06005C96 RID: 23702 RVA: 0x0022000E File Offset: 0x0021E20E
		public LordToil_ReturnedCaravan_PenAnimals(IntVec3 entryPoint) : base(entryPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x0022002C File Offset: 0x0021E22C
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				this.tmpOwnedPawns.Clear();
				this.tmpOwnedPawns.AddRange(this.lord.ownedPawns);
				bool flag = false;
				bool flag2 = false;
				foreach (Pawn pawn in this.tmpOwnedPawns)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						if (this.tmpOwnedPawns.Contains(pawn.roping.RopedByPawn))
						{
							flag2 = true;
						}
						else
						{
							this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
						}
					}
					else if (pawn.Spawned && pawn.IsColonist && !pawn.Downed && !pawn.Dead && pawn.roping.HasAnyRope)
					{
						flag = true;
					}
					else
					{
						this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
					}
				}
				if (!flag || !flag2)
				{
					this.lord.ReceiveMemo("RepenningFinished");
				}
				this.tmpOwnedPawns.Clear();
			}
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x0022015C File Offset: 0x0021E35C
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.ReturnedCaravan_PenAnimals);
		}

		// Token: 0x040034CB RID: 13515
		private List<Pawn> tmpOwnedPawns = new List<Pawn>();
	}
}
