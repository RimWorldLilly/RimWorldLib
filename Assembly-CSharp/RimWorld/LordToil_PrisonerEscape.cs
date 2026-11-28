using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CED RID: 3309
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06005D65 RID: 23909 RVA: 0x002239B0 File Offset: 0x00221BB0
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06005D66 RID: 23910 RVA: 0x002239BD File Offset: 0x00221BBD
		private new LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06005D67 RID: 23911 RVA: 0x002239CA File Offset: 0x00221BCA
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06005D68 RID: 23912 RVA: 0x002239CD File Offset: 0x00221BCD
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x002239D4 File Offset: 0x00221BD4
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x002239E4 File Offset: 0x00221BE4
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, leader, 10f);
				}
			}
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x00223AB0 File Offset: 0x00221CB0
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].guilt.Notify_Guilty(60000);
			}
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x00223B00 File Offset: 0x00221D00
		private Pawn GetLeader()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (!this.lord.ownedPawns[i].Downed && this.IsSapper(this.lord.ownedPawns[i]))
				{
					return this.lord.ownedPawns[i];
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				if (!this.lord.ownedPawns[j].Downed)
				{
					return this.lord.ownedPawns[j];
				}
			}
			return null;
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x00223BB1 File Offset: 0x00221DB1
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}

		// Token: 0x0400350F RID: 13583
		private int sapperThingID;
	}
}
