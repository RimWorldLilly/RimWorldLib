using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001726 RID: 5926
	public class RitualTargetFilterDef : Def
	{
		// Token: 0x060093CF RID: 37839 RVA: 0x0035E89B File Offset: 0x0035CA9B
		public RitualTargetFilter GetInstance()
		{
			return (RitualTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x04005956 RID: 22870
		public Type workerClass;

		// Token: 0x04005957 RID: 22871
		public bool fallBackToGatherSpot;

		// Token: 0x04005958 RID: 22872
		public bool fallbackToRitualSpot;
	}
}
