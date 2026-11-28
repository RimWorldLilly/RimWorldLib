using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000922 RID: 2338
	public class PsychicRitualToil_CarryAndGoto : PsychicRitualToil_Goto
	{
		// Token: 0x0600477F RID: 18303 RVA: 0x001C0172 File Offset: 0x001BE372
		protected PsychicRitualToil_CarryAndGoto()
		{
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x001C017A File Offset: 0x001BE37A
		public PsychicRitualToil_CarryAndGoto(PsychicRitualRoleDef carrierRole, PsychicRitualRoleDef payloadRole, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions) : base(rolePositions.Slice(new PsychicRitualRoleDef[]
		{
			carrierRole
		}))
		{
			this.payloadPositions = new List<IntVec3>(rolePositions[payloadRole]);
			this.carrierRole = carrierRole;
			this.payloadRole = payloadRole;
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x001C01B4 File Offset: 0x001BE3B4
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.UpdateAllDuties(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.payloadRole);
			if (pawn == null)
			{
				return;
			}
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.carrierRole);
			if (pawn2 == null)
			{
				return;
			}
			if (pawn.IsPrisoner)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
				pawn.mindState.duty.focus = psychicRitual.assignments.Target.Cell;
			}
			this.SetPawnDuty(pawn2, psychicRitual, parent, DutyDefOf.DeliverPawnToPsychicRitualCell, new LocalTargetInfo?(pawn2.mindState.duty.focus), new LocalTargetInfo?(pawn), new LocalTargetInfo?(this.payloadPositions[0]), base.FinalGatherPhase ? "final" : "initial", 0f, null);
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x001C02C4 File Offset: 0x001BE4C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.carrierRole, "carrierRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.payloadRole, "payloadRole");
			Scribe_Collections.Look<IntVec3>(ref this.payloadPositions, "payloadPositions", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04002E4D RID: 11853
		public PsychicRitualRoleDef carrierRole;

		// Token: 0x04002E4E RID: 11854
		public PsychicRitualRoleDef payloadRole;

		// Token: 0x04002E4F RID: 11855
		protected List<IntVec3> payloadPositions;
	}
}
