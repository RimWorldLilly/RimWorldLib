using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001690 RID: 5776
	public class RitualCancellationTrigger_CannotScarify : RitualCancellationTrigger
	{
		// Token: 0x060090C7 RID: 37063 RVA: 0x00351BD8 File Offset: 0x0034FDD8
		public override IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments)
		{
			RitualCancellationTrigger_CannotScarify.<CancellationTriggers>d__1 <CancellationTriggers>d__ = new RitualCancellationTrigger_CannotScarify.<CancellationTriggers>d__1(-2);
			<CancellationTriggers>d__.<>4__this = this;
			<CancellationTriggers>d__.<>3__assignments = assignments;
			return <CancellationTriggers>d__;
		}

		// Token: 0x04005841 RID: 22593
		[NoTranslate]
		public string roleId;
	}
}
