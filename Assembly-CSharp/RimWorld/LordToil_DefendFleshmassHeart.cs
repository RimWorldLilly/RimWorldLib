using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCF RID: 3279
	public class LordToil_DefendFleshmassHeart : LordToil
	{
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x00221598 File Offset: 0x0021F798
		private LordToilData_DefendFleshmassHeart Data
		{
			get
			{
				return (LordToilData_DefendFleshmassHeart)this.data;
			}
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x002215A5 File Offset: 0x0021F7A5
		private LordToil_DefendFleshmassHeart()
		{
			this.data = new LordToilData_DefendFleshmassHeart();
		}

		// Token: 0x06005CE5 RID: 23781 RVA: 0x002215B8 File Offset: 0x0021F7B8
		public LordToil_DefendFleshmassHeart(Building_FleshmassHeart heart) : this()
		{
			this.Data.heart = heart;
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x002215CC File Offset: 0x0021F7CC
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				this.UpdateDefendPoint(pawn);
			}
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x00221624 File Offset: 0x0021F824
		public override void LordToilTick()
		{
			if (this.Data.heart.DestroyedOrNull())
			{
				base.Map.lordManager.RemoveLord(this.lord);
				return;
			}
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn.IsHashIntervalTick(3600))
				{
					this.UpdateDefendPoint(pawn);
				}
			}
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x002216B4 File Offset: 0x0021F8B4
		private void UpdateDefendPoint(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return;
			}
			if (pawn.mindState.CombatantRecently)
			{
				return;
			}
			IntVec3 intVec = IntVec3.Invalid;
			int i;
			for (i = 0; i < 100; i++)
			{
				if (!this.Data.heart.Comp.ContiguousFleshmass.TryRandomElement(out intVec))
				{
					intVec = this.Data.heart.Position;
					break;
				}
				bool flag = false;
				foreach (IntVec3 b in GenAdj.CardinalDirections)
				{
					IntVec3 c = intVec + b;
					if (c.InBounds(base.Map) && c.Standable(base.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (i == 100 || intVec == IntVec3.Invalid)
			{
				intVec = this.Data.heart.Position;
			}
			pawn.mindState.duty = new PawnDuty(DutyDefOf.DefendFleshmassHeart, intVec, -1f);
			pawn.mindState.duty.focusSecond = intVec;
			pawn.mindState.duty.radius = 50f;
			pawn.mindState.duty.wanderRadius = new float?(12f);
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x00221814 File Offset: 0x0021FA14
		public override void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (!victim.HostileTo(dinfo.Instigator))
			{
				return;
			}
			this.lord.Notify_PawnLost(victim, PawnLostCondition.LeftVoluntarily, null);
			victim.mindState.enemyTarget = dinfo.Instigator;
			victim.mindState.Notify_EngagedTarget();
			victim.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x040034EA RID: 13546
		private const float DefendRadius = 50f;

		// Token: 0x040034EB RID: 13547
		private const float WanderRadius = 12f;

		// Token: 0x040034EC RID: 13548
		private const int SwitchDefendPointInterval = 3600;
	}
}
