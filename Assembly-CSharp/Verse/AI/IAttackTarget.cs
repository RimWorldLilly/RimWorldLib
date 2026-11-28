using System;

namespace Verse.AI
{
	// Token: 0x02000913 RID: 2323
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x060046C2 RID: 18114
		Thing Thing { get; }

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060046C3 RID: 18115
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060046C4 RID: 18116
		float TargetPriorityFactor { get; }

		// Token: 0x060046C5 RID: 18117
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
