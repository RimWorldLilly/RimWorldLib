using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001651 RID: 5713
	public abstract class PreceptRequirement : IExposable
	{
		// Token: 0x06008F8A RID: 36746
		public abstract bool Met(List<Precept> precepts);

		// Token: 0x06008F8B RID: 36747
		public abstract Precept MakePrecept(Ideo ideo);

		// Token: 0x06008F8C RID: 36748 RVA: 0x0034B991 File Offset: 0x00349B91
		public virtual void ExposeData()
		{
		}
	}
}
