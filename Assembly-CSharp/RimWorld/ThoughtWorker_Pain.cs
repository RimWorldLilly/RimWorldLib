using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E33 RID: 3635
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x0600615A RID: 24922 RVA: 0x00232954 File Offset: 0x00230B54
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 0.0001f)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.4f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.8f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x0600615B RID: 24923 RVA: 0x002329B3 File Offset: 0x00230BB3
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
