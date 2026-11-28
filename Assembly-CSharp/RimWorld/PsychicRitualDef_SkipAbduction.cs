using System;
using System.Collections.Generic;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9E RID: 3230
	public class PsychicRitualDef_SkipAbduction : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B84 RID: 23428 RVA: 0x0021A3EE File Offset: 0x002185EE
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SkipAbduction(this.InvokerRole));
			return list;
		}
	}
}
