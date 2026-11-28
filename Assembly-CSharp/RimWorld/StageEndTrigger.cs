using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200165F RID: 5727
	public abstract class StageEndTrigger : IExposable
	{
		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x06008FF9 RID: 36857 RVA: 0x0034EAB0 File Offset: 0x0034CCB0
		public virtual bool CountsTowardsProgress
		{
			get
			{
				return this.countsTowardsProgress;
			}
		}

		// Token: 0x06008FFA RID: 36858
		public abstract Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage);

		// Token: 0x06008FFB RID: 36859 RVA: 0x0034EAB8 File Offset: 0x0034CCB8
		public virtual void ExposeData()
		{
		}

		// Token: 0x040057EC RID: 22508
		public bool countsTowardsProgress;
	}
}
