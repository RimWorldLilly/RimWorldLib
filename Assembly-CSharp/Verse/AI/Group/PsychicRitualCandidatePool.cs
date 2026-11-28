using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000919 RID: 2329
	public class PsychicRitualCandidatePool : ILordJobCandidatePool
	{
		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004706 RID: 18182 RVA: 0x001BE3AC File Offset: 0x001BC5AC
		public List<Pawn> NonAssignablePawns
		{
			get
			{
				return this.nonAssignablePawns;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x001BE3B4 File Offset: 0x001BC5B4
		public List<Pawn> AllCandidatePawns
		{
			get
			{
				return this.allCandidatePawns;
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x001BE3BC File Offset: 0x001BC5BC
		public PsychicRitualCandidatePool(List<Pawn> allCandidatePawns, List<Pawn> nonAssignablePawns)
		{
			this.allCandidatePawns = allCandidatePawns;
			this.nonAssignablePawns = nonAssignablePawns;
		}

		// Token: 0x04002E2F RID: 11823
		private List<Pawn> nonAssignablePawns;

		// Token: 0x04002E30 RID: 11824
		private List<Pawn> allCandidatePawns;
	}
}
