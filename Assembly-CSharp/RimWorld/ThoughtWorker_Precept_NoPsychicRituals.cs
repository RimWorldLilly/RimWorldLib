using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE4 RID: 3556
	public class ThoughtWorker_Precept_NoPsychicRituals : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x060060A9 RID: 24745 RVA: 0x00230144 File Offset: 0x0022E344
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!p.IsColonist)
			{
				return false;
			}
			return Find.TickManager.TicksGame > Find.IdeoManager.lastPsychicRitualPerformedTick + 360000;
		}

		// Token: 0x060060AA RID: 24746 RVA: 0x00230184 File Offset: 0x0022E384
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_Precept_NoPsychicRituals.<GetDescriptionArgs>d__2(-2);
		}

		// Token: 0x040035FB RID: 13819
		public const int TicksSinceLastPsychicRitualActiveTicks = 360000;
	}
}
