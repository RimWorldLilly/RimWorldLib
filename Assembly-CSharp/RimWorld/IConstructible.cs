using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A59 RID: 6745
	public interface IConstructible
	{
		// Token: 0x0600A940 RID: 43328
		List<ThingDefCountClass> TotalMaterialCost();

		// Token: 0x0600A941 RID: 43329
		bool IsCompleted();

		// Token: 0x0600A942 RID: 43330
		int ThingCountNeeded(ThingDef stuff);

		// Token: 0x0600A943 RID: 43331
		ThingDef EntityToBuildStuff();
	}
}
