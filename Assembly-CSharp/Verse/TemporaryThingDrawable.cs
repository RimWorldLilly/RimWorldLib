using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public class TemporaryThingDrawable : IExposable
	{
		// Token: 0x06001877 RID: 6263 RVA: 0x0009306C File Offset: 0x0009126C
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<Vector3>(ref this.position, "position", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04001464 RID: 5220
		public Thing thing;

		// Token: 0x04001465 RID: 5221
		public Vector3 position;

		// Token: 0x04001466 RID: 5222
		public int ticksLeft;
	}
}
