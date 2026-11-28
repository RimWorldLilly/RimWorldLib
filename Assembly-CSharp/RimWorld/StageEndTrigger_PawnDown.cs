using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166E RID: 5742
	public class StageEndTrigger_PawnDown : StageEndTrigger
	{
		// Token: 0x06009023 RID: 36899 RVA: 0x0034F1F4 File Offset: 0x0034D3F4
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			if (ritual.Ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == this.roleId) == null)
			{
				return null;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			return new Trigger_TickCondition(() => pawn.Downed, 1);
		}

		// Token: 0x06009024 RID: 36900 RVA: 0x0034F26B File Offset: 0x0034D46B
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x040057F8 RID: 22520
		[NoTranslate]
		public string roleId;
	}
}
