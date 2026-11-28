using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001517 RID: 5399
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x06008448 RID: 33864 RVA: 0x002F962A File Offset: 0x002F782A
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			return 0.075f * InteractionWorker_DeepTalk.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x040051B0 RID: 20912
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x040051B1 RID: 20913
		private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1.5f, 0f),
				true
			},
			{
				new CurvePoint(-0.5f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.8f),
				true
			},
			{
				new CurvePoint(2f, 3f),
				true
			}
		};
	}
}
