using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBA RID: 3514
	public class ThoughtWorker_Precept_NoRecentExecution : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06006030 RID: 24624 RVA: 0x0022EA1C File Offset: 0x0022CC1C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist || p.IsSlave)
			{
				return false;
			}
			int num = Mathf.Max(0, p.Faction.lastExecutionTick);
			return Find.TickManager.TicksGame - num > 1800000;
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x0022EA6A File Offset: 0x0022CC6A
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_Precept_NoRecentExecution.<GetDescriptionArgs>d__2(-2);
		}

		// Token: 0x040035ED RID: 13805
		private const int MinDaysSinceLastExecutionForThought = 30;
	}
}
