using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C73 RID: 3187
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06005A69 RID: 23145 RVA: 0x0021451A File Offset: 0x0021271A
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x0021451D File Offset: 0x0021271D
		public override bool AllowStartNewRituals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06005A6B RID: 23147 RVA: 0x00214520 File Offset: 0x00212720
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x00214523 File Offset: 0x00212723
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x00214532 File Offset: 0x00212732
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x00214548 File Offset: 0x00212748
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x0021455C File Offset: 0x0021275C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x040033DD RID: 13277
		public int transportersGroup = -1;
	}
}
