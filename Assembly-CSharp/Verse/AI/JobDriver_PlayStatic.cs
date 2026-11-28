using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200080B RID: 2059
	public class JobDriver_PlayStatic : JobDriver_BabyPlay
	{
		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060040D3 RID: 16595 RVA: 0x0019C087 File Offset: 0x0019A287
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.GotoBaby;
			}
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x0019C08A File Offset: 0x0019A28A
		protected override IEnumerable<Toil> Play()
		{
			JobDriver_PlayStatic.<Play>d__3 <Play>d__ = new JobDriver_PlayStatic.<Play>d__3(-2);
			<Play>d__.<>4__this = this;
			return <Play>d__;
		}

		// Token: 0x04002AFC RID: 11004
		private const int InteractionIntervalTicks = 1250;
	}
}
