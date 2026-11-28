using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB8 RID: 3512
	public class Thought_Situational_Precept_HighLife : Thought_Situational
	{
		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06006029 RID: 24617 RVA: 0x0022E828 File Offset: 0x0022CA28
		protected override float BaseMoodOffset
		{
			get
			{
				if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
				{
					return 0f;
				}
				float x = (float)(Find.TickManager.TicksGame - this.pawn.mindState.lastTakeRecreationalDrugTick) / 60000f;
				return (float)Mathf.RoundToInt(ThoughtWorker_Precept_HighLife.MoodOffsetFromDaysSinceLastDrugCurve.Evaluate(x));
			}
		}
	}
}
