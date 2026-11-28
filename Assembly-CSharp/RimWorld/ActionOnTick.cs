using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001722 RID: 5922
	public abstract class ActionOnTick : IExposable
	{
		// Token: 0x060093C5 RID: 37829
		public abstract void Apply(LordJob_Ritual ritual);

		// Token: 0x060093C6 RID: 37830 RVA: 0x0035E739 File Offset: 0x0035C939
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}

		// Token: 0x04005951 RID: 22865
		public int tick;
	}
}
