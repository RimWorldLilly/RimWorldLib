using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBB RID: 3515
	public class ThoughtWorker_Precept_NoRecentHumanMeat : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06006033 RID: 24627 RVA: 0x0022EA7C File Offset: 0x0022CC7C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (((needs != null) ? needs.food : null) == null)
			{
				return false;
			}
			int num = Mathf.Max(0, p.mindState.lastHumanMeatIngestedTick);
			return Find.TickManager.TicksGame - num > 480000;
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x0022EACE File Offset: 0x0022CCCE
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_Precept_NoRecentHumanMeat.<GetDescriptionArgs>d__2(-2);
		}

		// Token: 0x040035EE RID: 13806
		public const int MinDaysSinceLastHumanMeatForThought = 8;
	}
}
