using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C67 RID: 3175
	public class LordJob_DefendCerebrexCore : LordJob
	{
		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06005A17 RID: 23063 RVA: 0x002126DE File Offset: 0x002108DE
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x002126E1 File Offset: 0x002108E1
		public LordJob_DefendCerebrexCore()
		{
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x002126E9 File Offset: 0x002108E9
		public LordJob_DefendCerebrexCore(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x002126F8 File Offset: 0x002108F8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x00212720 File Offset: 0x00210920
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase startingToil = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = startingToil;
			return stateGraph;
		}

		// Token: 0x040033A8 RID: 13224
		private IntVec3 baseCenter;
	}
}
