using System;

namespace Verse
{
	// Token: 0x02000291 RID: 657
	public class ThingListChangedCallbacks
	{
		// Token: 0x0400111A RID: 4378
		public Action<Thing> onThingAdded = delegate(Thing t)
		{
		};

		// Token: 0x0400111B RID: 4379
		public Action<Thing> onThingRemoved = delegate(Thing t)
		{
		};
	}
}
