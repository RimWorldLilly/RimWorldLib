using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2A RID: 3882
	public class AbilityGroupDef : Def
	{
		// Token: 0x0400395E RID: 14686
		public int cooldownTicks;

		// Token: 0x0400395F RID: 14687
		public bool sendMessageOnCooldownComplete;

		// Token: 0x04003960 RID: 14688
		public List<string> ritualRoleIds;
	}
}
