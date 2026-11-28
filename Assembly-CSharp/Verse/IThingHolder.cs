using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200059F RID: 1439
	public interface IThingHolder
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002B08 RID: 11016
		IThingHolder ParentHolder { get; }

		// Token: 0x06002B09 RID: 11017
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06002B0A RID: 11018
		ThingOwner GetDirectlyHeldThings();
	}
}
