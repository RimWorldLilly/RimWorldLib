using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000977 RID: 2423
	public class Trigger_OnPlayerMechHarmAnything : Trigger
	{
		// Token: 0x06004998 RID: 18840 RVA: 0x001CACFE File Offset: 0x001C8EFE
		public Trigger_OnPlayerMechHarmAnything(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x001CAD10 File Offset: 0x001C8F10
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (ModsConfig.BiotechActive && signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null)
			{
				Pawn pawn = signal.dinfo.Instigator as Pawn;
				if (pawn != null && pawn.IsColonyMech && this.things.Contains(signal.thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002F3E RID: 12094
		private List<Thing> things;
	}
}
