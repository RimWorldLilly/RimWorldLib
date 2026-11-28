using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D7 RID: 1751
	public static class EloUtility
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x00161E16 File Offset: 0x00160016
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x00161E30 File Offset: 0x00160030
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00161E74 File Offset: 0x00160074
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x00161E8B File Offset: 0x0016008B
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04002442 RID: 9282
		private const float TenFactorRating = 400f;
	}
}
