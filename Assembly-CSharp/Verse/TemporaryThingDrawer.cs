using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031C RID: 796
	public class TemporaryThingDrawer : IExposable
	{
		// Token: 0x06001879 RID: 6265 RVA: 0x000930C0 File Offset: 0x000912C0
		public void Tick()
		{
			for (int i = this.drawables.Count - 1; i >= 0; i--)
			{
				TemporaryThingDrawable temporaryThingDrawable = this.drawables[i];
				if (temporaryThingDrawable.ticksLeft >= 0 && temporaryThingDrawable.thing != null)
				{
					temporaryThingDrawable.ticksLeft--;
				}
				else
				{
					this.drawables.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00093120 File Offset: 0x00091320
		public void Draw()
		{
			foreach (TemporaryThingDrawable temporaryThingDrawable in this.drawables)
			{
				temporaryThingDrawable.thing.DrawNowAt(temporaryThingDrawable.position, false);
			}
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00093180 File Offset: 0x00091380
		public void AddThing(Thing t, Vector3 position, int ticks)
		{
			this.drawables.Add(new TemporaryThingDrawable
			{
				thing = t,
				position = position,
				ticksLeft = ticks
			});
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000931A7 File Offset: 0x000913A7
		public void ExposeData()
		{
			Scribe_Collections.Look<TemporaryThingDrawable>(ref this.drawables, "drawables", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04001467 RID: 5223
		private List<TemporaryThingDrawable> drawables = new List<TemporaryThingDrawable>();
	}
}
