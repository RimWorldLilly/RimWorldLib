using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200098D RID: 2445
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x001CB7A2 File Offset: 0x001C99A2
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x060049C7 RID: 18887 RVA: 0x001CB7AF File Offset: 0x001C99AF
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x001CB7C9 File Offset: 0x001C99C9
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x001CB7EC File Offset: 0x001C99EC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_TicksPassed))
				{
					BackCompatibility.TriggerDataTicksPassedNull(this);
				}
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				return data.ticksPassed > this.duration;
			}
			return false;
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x001CB840 File Offset: 0x001C9A40
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x04002F50 RID: 12112
		private int duration = 100;
	}
}
