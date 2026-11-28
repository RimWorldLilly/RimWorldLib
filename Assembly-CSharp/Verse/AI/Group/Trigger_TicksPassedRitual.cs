using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200098F RID: 2447
	public class Trigger_TicksPassedRitual : Trigger
	{
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x001CB878 File Offset: 0x001C9A78
		protected TriggerData_TicksPassedRitual Data
		{
			get
			{
				return (TriggerData_TicksPassedRitual)this.data;
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x001CB885 File Offset: 0x001C9A85
		public Trigger_TicksPassedRitual(int tickLimit, RitualStage stage)
		{
			this.data = new TriggerData_TicksPassedRitual();
			this.duration = tickLimit;
			this.stage = stage;
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x001CB8B0 File Offset: 0x001C9AB0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				LordJob_Ritual ritual = lord.LordJob as LordJob_Ritual;
				TriggerData_TicksPassedRitual data = this.Data;
				data.ticksPassed += this.stage.ProgressPerTick(ritual);
				return data.ticksPassed > (float)this.duration;
			}
			return false;
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x001CB901 File Offset: 0x001C9B01
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0f;
			}
		}

		// Token: 0x04002F52 RID: 12114
		private int duration = 100;

		// Token: 0x04002F53 RID: 12115
		private RitualStage stage;
	}
}
