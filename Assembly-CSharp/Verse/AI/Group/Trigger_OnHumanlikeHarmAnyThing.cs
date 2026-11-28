using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000976 RID: 2422
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		// Token: 0x06004996 RID: 18838 RVA: 0x001CAC72 File Offset: 0x001C8E72
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x001CAC84 File Offset: 0x001C8E84
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null)
			{
				Pawn pawn = signal.dinfo.Instigator as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002F3D RID: 12093
		private List<Thing> things;
	}
}
