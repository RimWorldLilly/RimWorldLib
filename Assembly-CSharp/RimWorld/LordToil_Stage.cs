using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFA RID: 3322
	public class LordToil_Stage : LordToil
	{
		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06005DAF RID: 23983 RVA: 0x002253A8 File Offset: 0x002235A8
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06005DB0 RID: 23984 RVA: 0x002253B5 File Offset: 0x002235B5
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06005DB1 RID: 23985 RVA: 0x002253C2 File Offset: 0x002235C2
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x002253C5 File Offset: 0x002235C5
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x002253E4 File Offset: 0x002235E4
		public override void UpdateAllDuties()
		{
			LordToilData_Stage data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.stagingPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
