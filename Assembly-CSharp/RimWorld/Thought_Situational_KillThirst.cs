using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E66 RID: 3686
	public class Thought_Situational_KillThirst : Thought_Situational
	{
		// Token: 0x060061E9 RID: 25065 RVA: 0x002343A0 File Offset: 0x002325A0
		public override float MoodOffset()
		{
			Need_KillThirst need_KillThirst;
			if (this.pawn.needs == null || !this.pawn.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
			{
				return 0f;
			}
			return Thought_Situational_KillThirst.MoodOffsetCurve.Evaluate(need_KillThirst.CurLevel);
		}

		// Token: 0x0400360B RID: 13835
		private static readonly SimpleCurve MoodOffsetCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.301f, 0f),
				true
			},
			{
				new CurvePoint(0.3f, -4f),
				true
			},
			{
				new CurvePoint(0f, -18f),
				true
			}
		};
	}
}
