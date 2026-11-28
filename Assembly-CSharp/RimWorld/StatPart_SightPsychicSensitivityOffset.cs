using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA7 RID: 8103
	public class StatPart_SightPsychicSensitivityOffset : StatPart
	{
		// Token: 0x0600CDCB RID: 52683 RVA: 0x004B9CBC File Offset: 0x004B7EBC
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetPsychicOffset(req.Thing, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CDCC RID: 52684 RVA: 0x004B9CE0 File Offset: 0x004B7EE0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (req.HasThing && this.TryGetPsychicOffset(req.Thing, out f))
			{
				return "StatsReport_SightPsychicSensitivityOffset".Translate() + (": +" + f.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CDCD RID: 52685 RVA: 0x004B9D30 File Offset: 0x004B7F30
		private bool TryGetPsychicOffset(Thing t, out float offset)
		{
			if (t != null)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					HediffSet hediffSet = pawn.health.hediffSet;
					BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
					float maxValue = float.MaxValue;
					float partEfficiencySpecialWeight = PawnCapacityWorker_Sight.PartEfficiencySpecialWeight;
					float num = PawnCapacityUtility.CalculateTagEfficiency(hediffSet, sightSource, maxValue, default(FloatRange), null, partEfficiencySpecialWeight);
					if (num <= this.startsAt)
					{
						offset = GenMath.LerpDoubleClamped(this.startsAt, this.endsAt, this.minBonus, this.maxBonus, num);
						return offset >= 0.01f;
					}
				}
			}
			offset = 0f;
			return false;
		}

		// Token: 0x04008337 RID: 33591
		public float startsAt = 0.5f;

		// Token: 0x04008338 RID: 33592
		public float minBonus;

		// Token: 0x04008339 RID: 33593
		public float maxBonus = 0.5f;

		// Token: 0x0400833A RID: 33594
		public float endsAt;
	}
}
