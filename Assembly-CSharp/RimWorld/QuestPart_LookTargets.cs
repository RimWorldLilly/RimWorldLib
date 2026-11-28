using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CF RID: 4303
	public class QuestPart_LookTargets : QuestPart
	{
		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06006B97 RID: 27543 RVA: 0x0025BE8E File Offset: 0x0025A08E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.targets;
			}
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x0025BE96 File Offset: 0x0025A096
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x04004463 RID: 17507
		public List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
