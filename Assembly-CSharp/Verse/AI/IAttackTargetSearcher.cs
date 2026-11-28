using System;

namespace Verse.AI
{
	// Token: 0x02000908 RID: 2312
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600466B RID: 18027
		Thing Thing { get; }

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x0600466C RID: 18028
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x0600466D RID: 18029
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x0600466E RID: 18030
		int LastAttackTargetTick { get; }
	}
}
