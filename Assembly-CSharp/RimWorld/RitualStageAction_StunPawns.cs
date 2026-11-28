using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001721 RID: 5921
	public class RitualStageAction_StunPawns : RitualStageAction
	{
		// Token: 0x060093C2 RID: 37826 RVA: 0x0035E654 File Offset: 0x0035C854
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				foreach (Pawn pawn in ritual.assignments.AssignedPawns(roleId))
				{
					pawn.stances.stunner.StunFor(this.stunDurationTicks, null, false, false, false);
				}
			}
		}

		// Token: 0x060093C3 RID: 37827 RVA: 0x0035E6F4 File Offset: 0x0035C8F4
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.stunDurationTicks, "stunDurationTicks", 0, false);
		}

		// Token: 0x0400594F RID: 22863
		public List<string> roleIds = new List<string>();

		// Token: 0x04005950 RID: 22864
		public int stunDurationTicks = 10;
	}
}
