using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001668 RID: 5736
	public class StageEndTrigger_MutilatedRole : StageEndTrigger
	{
		// Token: 0x06009011 RID: 36881 RVA: 0x0034EE68 File Offset: 0x0034D068
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			LordJob_Ritual_Mutilation r = ritual as LordJob_Ritual_Mutilation;
			if (r != null)
			{
				return new Trigger_Custom(delegate(TriggerSignal signal)
				{
					foreach (Pawn p in r.mutilatedPawns)
					{
						RitualRole ritualRole = r.RoleFor(p, true);
						if (ritualRole != null && ritualRole.id == this.roleId)
						{
							return true;
						}
					}
					return false;
				});
			}
			Log.Error("Used StageEndTrigger_MutilatedRole on non LordJob_Ritual_Mutilation ritual job");
			return null;
		}

		// Token: 0x06009012 RID: 36882 RVA: 0x0034EEB3 File Offset: 0x0034D0B3
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x040057F2 RID: 22514
		[NoTranslate]
		public string roleId;
	}
}
