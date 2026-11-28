using System;

namespace RimWorld
{
	// Token: 0x02001795 RID: 6037
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x06009671 RID: 38513 RVA: 0x00369F6C File Offset: 0x0036816C
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x06009672 RID: 38514
		protected abstract void ApplyRule();
	}
}
