using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200098C RID: 2444
	public class Trigger_AcquiredTarget : Trigger
	{
		// Token: 0x060049C4 RID: 18884 RVA: 0x001CB760 File Offset: 0x001C9960
		public Trigger_AcquiredTarget(Faction requireTargetWithSpecificFaction)
		{
			this.faction = requireTargetWithSpecificFaction;
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x001CB76F File Offset: 0x001C996F
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type != TriggerSignalType.AcquiredTarget)
			{
				return false;
			}
			if (this.faction != null)
			{
				Thing otherThing = signal.otherThing;
				if (((otherThing != null) ? otherThing.Faction : null) != this.faction)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002F4F RID: 12111
		private Faction faction;
	}
}
