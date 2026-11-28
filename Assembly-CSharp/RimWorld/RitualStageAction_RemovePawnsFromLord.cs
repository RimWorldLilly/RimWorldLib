using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001720 RID: 5920
	public class RitualStageAction_RemovePawnsFromLord : RitualStageAction
	{
		// Token: 0x060093BF RID: 37823 RVA: 0x0035E594 File Offset: 0x0035C794
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				foreach (Pawn p in ritual.assignments.AssignedPawns(roleId))
				{
					ritual.lord.RemovePawn(p);
				}
			}
		}

		// Token: 0x060093C0 RID: 37824 RVA: 0x0035E628 File Offset: 0x0035C828
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0400594E RID: 22862
		public List<string> roleIds = new List<string>();
	}
}
