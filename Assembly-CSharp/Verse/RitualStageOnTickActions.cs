using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B5 RID: 181
	public class RitualStageOnTickActions : IExposable
	{
		// Token: 0x06000757 RID: 1879 RVA: 0x0002A16E File Offset: 0x0002836E
		public void ExposeData()
		{
			Scribe_Collections.Look<ActionOnTick>(ref this.actions, "actions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040002C9 RID: 713
		public List<ActionOnTick> actions = new List<ActionOnTick>();
	}
}
