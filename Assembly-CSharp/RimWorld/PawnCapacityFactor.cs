using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEA RID: 4074
	public class PawnCapacityFactor
	{
		// Token: 0x0600670B RID: 26379 RVA: 0x002487F8 File Offset: 0x002469F8
		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0f && num < 1f)
			{
				num = Mathf.InverseLerp(0f, 1f - this.allowedDefect, num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				if (Mathf.Abs(num) < 0.001f)
				{
					num = 5f;
				}
				else
				{
					num = Mathf.Min(1f / num, 5f);
				}
			}
			return num;
		}

		// Token: 0x0400409C RID: 16540
		public PawnCapacityDef capacity;

		// Token: 0x0400409D RID: 16541
		public float weight = 1f;

		// Token: 0x0400409E RID: 16542
		public float max = 9999f;

		// Token: 0x0400409F RID: 16543
		public bool useReciprocal;

		// Token: 0x040040A0 RID: 16544
		public float allowedDefect;

		// Token: 0x040040A1 RID: 16545
		private const float MaxReciprocalFactor = 5f;
	}
}
