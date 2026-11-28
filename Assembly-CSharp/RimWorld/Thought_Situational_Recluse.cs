using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E74 RID: 3700
	public class Thought_Situational_Recluse : Thought_Situational
	{
		// Token: 0x0600620B RID: 25099 RVA: 0x00234B78 File Offset: 0x00232D78
		public override float MoodOffset()
		{
			int freeColonistsAndPrisonersSpawnedCount = this.pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawnedCount;
			return Thought_Situational_Recluse.MoodOffsetCurveFromPopulation.Evaluate((float)freeColonistsAndPrisonersSpawnedCount);
		}

		// Token: 0x04003616 RID: 13846
		public const int NumPawns_Alone = 1;

		// Token: 0x04003617 RID: 13847
		public const int NumPawns_NotCrowded = 4;

		// Token: 0x04003618 RID: 13848
		public const int NumPawns_Inactive = 10;

		// Token: 0x04003619 RID: 13849
		public const int NumPawns_Crowded = 15;

		// Token: 0x0400361A RID: 13850
		private static readonly SimpleCurve MoodOffsetCurveFromPopulation = new SimpleCurve
		{
			{
				new CurvePoint(1f, 12f),
				true
			},
			{
				new CurvePoint(2f, 8f),
				true
			},
			{
				new CurvePoint(3f, 6f),
				true
			},
			{
				new CurvePoint(4f, 4f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0f),
				true
			},
			{
				new CurvePoint(11f, -2f),
				true
			},
			{
				new CurvePoint(12f, -3f),
				true
			},
			{
				new CurvePoint(13f, -4f),
				true
			},
			{
				new CurvePoint(14f, -5f),
				true
			},
			{
				new CurvePoint(15f, -6f),
				true
			},
			{
				new CurvePoint(16f, -8f),
				true
			}
		};
	}
}
