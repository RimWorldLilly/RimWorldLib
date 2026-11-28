using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000985 RID: 2437
	public class Trigger_AnyThingDamageTaken : Trigger
	{
		// Token: 0x060049B6 RID: 18870 RVA: 0x001CB2B6 File Offset: 0x001C94B6
		public Trigger_AnyThingDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x001CB2D8 File Offset: 0x001C94D8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				foreach (Thing thing in this.things)
				{
					if (thing.DestroyedOrNull() || (float)thing.HitPoints < (1f - this.damageFraction) * (float)thing.MaxHitPoints)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04002F48 RID: 12104
		private List<Thing> things;

		// Token: 0x04002F49 RID: 12105
		private float damageFraction = 0.5f;
	}
}
