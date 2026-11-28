using System;

namespace Verse.AI
{
	// Token: 0x02000839 RID: 2105
	public interface IJobEndable
	{
		// Token: 0x06004238 RID: 16952
		Pawn GetActor();

		// Token: 0x06004239 RID: 16953
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
