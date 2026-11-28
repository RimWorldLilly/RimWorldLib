using System;

namespace Verse.AI.Group
{
	// Token: 0x02000984 RID: 2436
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x060049B4 RID: 18868 RVA: 0x001CB246 File Offset: 0x001C9446
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x001CB268 File Offset: 0x001C9468
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x04002F46 RID: 12102
		private Thing thing;

		// Token: 0x04002F47 RID: 12103
		private float damageFraction = 0.5f;
	}
}
