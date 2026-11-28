using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA2 RID: 8098
	public class StatPart_RevenantSpeed : StatPart
	{
		// Token: 0x0600CDB7 RID: 52663 RVA: 0x004B98BC File Offset: 0x004B7ABC
		protected virtual bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.kindDef == PawnKindDefOf.Revenant && !pawn.IsPsychologicallyInvisible() && Mathf.Max(pawn.mindState.lastBecameVisibleTick, pawn.mindState.lastForcedVisibleTick) > 0;
		}

		// Token: 0x0600CDB8 RID: 52664 RVA: 0x004B9908 File Offset: 0x004B7B08
		private float GetFactor(Pawn p)
		{
			int num = Mathf.Max(p.mindState.lastBecameVisibleTick, p.mindState.lastForcedVisibleTick);
			int numTicks = Find.TickManager.TicksGame - num;
			return RevenantUtility.SpeedRangeFromBecameVisibleCurve.Evaluate(numTicks.TicksToSeconds());
		}

		// Token: 0x0600CDB9 RID: 52665 RVA: 0x004B994E File Offset: 0x004B7B4E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing)
			{
				return;
			}
			if (this.ActiveFor(req.Thing))
			{
				val *= this.GetFactor((Pawn)req.Thing);
			}
		}

		// Token: 0x0600CDBA RID: 52666 RVA: 0x004B9980 File Offset: 0x004B7B80
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
