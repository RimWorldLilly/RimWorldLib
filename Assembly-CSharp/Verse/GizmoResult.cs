using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200060E RID: 1550
	public struct GizmoResult
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x00136BE3 File Offset: 0x00134DE3
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x00136BEB File Offset: 0x00134DEB
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x00136BF3 File Offset: 0x00134DF3
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x00136C03 File Offset: 0x00134E03
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x04001F1B RID: 7963
		private GizmoState stateInt;

		// Token: 0x04001F1C RID: 7964
		private Event interactEventInt;
	}
}
