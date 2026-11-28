using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA3 RID: 3235
	public class PsychicRitualDef_SummonPitGate : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B94 RID: 23444 RVA: 0x0021A5D3 File Offset: 0x002187D3
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonPitGate(this.InvokerRole, this.combatPointMultiplierFromQualityRange));
			return list;
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x0021A5F4 File Offset: 0x002187F4
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_SummonPitGate.<BlockingIssues>d__2 <BlockingIssues>d__ = new PsychicRitualDef_SummonPitGate.<BlockingIssues>d__2(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x0400346C RID: 13420
		private FloatRange combatPointMultiplierFromQualityRange;
	}
}
