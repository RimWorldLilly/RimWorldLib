using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179B RID: 6043
	public abstract class StartingPawnCount : IExposable
	{
		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x060096AA RID: 38570
		public abstract string Summary { get; }

		// Token: 0x060096AB RID: 38571 RVA: 0x0036A482 File Offset: 0x00368682
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
			Scribe_Values.Look<bool>(ref this.requiredAtStart, "requiredAtStart", false, false);
		}

		// Token: 0x04005A58 RID: 23128
		public int count = 1;

		// Token: 0x04005A59 RID: 23129
		public bool requiredAtStart;

		// Token: 0x04005A5A RID: 23130
		[Unsaved(false)]
		public string countBuffer;
	}
}
