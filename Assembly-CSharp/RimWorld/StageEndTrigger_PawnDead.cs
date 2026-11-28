using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166D RID: 5741
	public class StageEndTrigger_PawnDead : StageEndTrigger
	{
		// Token: 0x06009020 RID: 36896 RVA: 0x0034F160 File Offset: 0x0034D360
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
			return new Trigger_TickCondition(() => pawn.Dead, 1);
		}

		// Token: 0x06009021 RID: 36897 RVA: 0x0034F1D7 File Offset: 0x0034D3D7
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x040057F7 RID: 22519
		[NoTranslate]
		public string roleId;
	}
}
