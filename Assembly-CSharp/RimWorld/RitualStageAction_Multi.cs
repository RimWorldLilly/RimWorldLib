using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171B RID: 5915
	public class RitualStageAction_Multi : RitualStageAction
	{
		// Token: 0x060093AE RID: 37806 RVA: 0x0035E258 File Offset: 0x0035C458
		public override void Apply(LordJob_Ritual ritual)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ((ritual != null) ? ritual.ToString() : null));
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.Apply(ritual);
			}
		}

		// Token: 0x060093AF RID: 37807 RVA: 0x0035E2D0 File Offset: 0x0035C4D0
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ((ritual != null) ? ritual.ToString() : null));
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x060093B0 RID: 37808 RVA: 0x0035E348 File Offset: 0x0035C548
		public override void ExposeData()
		{
			Scribe_Collections.Look<RitualStageAction>(ref this.subActions, "subActions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04005946 RID: 22854
		public List<RitualStageAction> subActions;
	}
}
