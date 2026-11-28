using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000924 RID: 2340
	public class PsychicRitualToil_GatherForInvocation : PsychicRitualToil_Multiplex
	{
		// Token: 0x0600478B RID: 18315 RVA: 0x001C0B59 File Offset: 0x001BED59
		protected PsychicRitualToil_GatherForInvocation()
		{
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x001C0B64 File Offset: 0x001BED64
		protected PsychicRitualToil_GatherForInvocation(PsychicRitualDef_InvocationCircle def, PsychicRitualToil_Goto fallbackToil, PsychicRitualGraph invokerToil) : base(new Dictionary<PsychicRitualRoleDef, PsychicRitualToil>
		{
			{
				def.InvokerRole,
				invokerToil
			}
		}, fallbackToil)
		{
			this.fallbackToil = fallbackToil;
			this.invokerToil = invokerToil;
			this.invokerFinalToil = (PsychicRitualToil_Goto)invokerToil.GetToil(invokerToil.ToilCount - 1);
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x001C0BB1 File Offset: 0x001BEDB1
		public PsychicRitualToil_GatherForInvocation(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions) : this(def, PsychicRitualToil_GatherForInvocation.FallbackToil(psychicRitual, def, rolePositions), PsychicRitualToil_GatherForInvocation.InvokerToil(def, rolePositions))
		{
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x001C0BC9 File Offset: 0x001BEDC9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PsychicRitualToil_Goto>(ref this.fallbackToil, "fallbackToil", false);
			Scribe_References.Look<PsychicRitualGraph>(ref this.invokerToil, "invokerToil", false);
			Scribe_References.Look<PsychicRitualToil_Goto>(ref this.invokerFinalToil, "invokerFinalToil", false);
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x001C0C04 File Offset: 0x001BEE04
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToil_GatherForInvocation.blockingPawns.Clear();
			PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.fallbackToil.BlockingPawns);
			if (this.invokerToil.CurrentToil == this.invokerFinalToil)
			{
				PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.invokerFinalToil.BlockingPawns);
			}
			else
			{
				PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.invokerFinalToil.ControlledPawns(psychicRitual));
			}
			string str = "PsychicRitualToil_GatherForInvocation_Report".Translate();
			string str2 = (from pawn in PsychicRitualToil_GatherForInvocation.blockingPawns
			select pawn.LabelShortCap).ToCommaList(false, false);
			return str + ": " + str2;
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x001C0CBB File Offset: 0x001BEEBB
		public static PsychicRitualToil_Goto FallbackToil(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			return new PsychicRitualToil_Goto(rolePositions.Slice(rolePositions.Keys.Except(def.InvokerRole)));
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x001C0CD9 File Offset: 0x001BEED9
		public static PsychicRitualGraph InvokerToil(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			return new PsychicRitualGraph(PsychicRitualToil_GatherForInvocation.InvokerGatherPhaseToils(def, rolePositions))
			{
				willAdvancePastLastToil = false
			};
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x001C0CEE File Offset: 0x001BEEEE
		public static IEnumerable<PsychicRitualToil> InvokerGatherPhaseToils(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			PsychicRitualToil_GatherForInvocation.<InvokerGatherPhaseToils>d__11 <InvokerGatherPhaseToils>d__ = new PsychicRitualToil_GatherForInvocation.<InvokerGatherPhaseToils>d__11(-2);
			<InvokerGatherPhaseToils>d__.<>3__def = def;
			<InvokerGatherPhaseToils>d__.<>3__rolePositions = rolePositions;
			return <InvokerGatherPhaseToils>d__;
		}

		// Token: 0x04002E53 RID: 11859
		protected PsychicRitualToil_Goto fallbackToil;

		// Token: 0x04002E54 RID: 11860
		protected PsychicRitualGraph invokerToil;

		// Token: 0x04002E55 RID: 11861
		protected PsychicRitualToil_Goto invokerFinalToil;

		// Token: 0x04002E56 RID: 11862
		private static List<Pawn> blockingPawns = new List<Pawn>(16);
	}
}
