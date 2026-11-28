using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D0E RID: 3342
	public abstract class LordToil_Gathering : LordToil
	{
		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x002268A0 File Offset: 0x00224AA0
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x06005DFA RID: 24058 RVA: 0x002268AD File Offset: 0x00224AAD
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x002268CE File Offset: 0x00224ACE
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		// Token: 0x06005DFC RID: 24060 RVA: 0x002268E0 File Offset: 0x00224AE0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		// Token: 0x06005DFD RID: 24061 RVA: 0x00226944 File Offset: 0x00224B44
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x04003557 RID: 13655
		protected IntVec3 spot;

		// Token: 0x04003558 RID: 13656
		protected GatheringDef gatheringDef;
	}
}
