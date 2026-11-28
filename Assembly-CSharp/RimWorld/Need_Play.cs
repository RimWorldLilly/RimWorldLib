using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001580 RID: 5504
	public class Need_Play : Need
	{
		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x060086EF RID: 34543 RVA: 0x0030C58C File Offset: 0x0030A78C
		public bool IsLow
		{
			get
			{
				return base.CurLevelPercentage <= 0.15f;
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x060086F0 RID: 34544 RVA: 0x0030C5A0 File Offset: 0x0030A7A0
		public PlayCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return PlayCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return PlayCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return PlayCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return PlayCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return PlayCategory.High;
				}
				return PlayCategory.Extreme;
			}
		}

		// Token: 0x060086F1 RID: 34545 RVA: 0x0030C5F9 File Offset: 0x0030A7F9
		public Need_Play(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.15f,
				0.3f,
				0.7f,
				0.85f
			};
		}

		// Token: 0x060086F2 RID: 34546 RVA: 0x0030C639 File Offset: 0x0030A839
		public void Play(float amount)
		{
			base.CurLevelPercentage += amount;
			base.CurLevelPercentage = Mathf.Clamp01(base.CurLevelPercentage);
		}

		// Token: 0x060086F3 RID: 34547 RVA: 0x0030C65A File Offset: 0x0030A85A
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.00025f;
			}
		}

		// Token: 0x0400535E RID: 21342
		public const float BaseFallPerInterval = 0.00025f;

		// Token: 0x0400535F RID: 21343
		public const float ThresholdEmpty = 0.01f;

		// Token: 0x04005360 RID: 21344
		public const float ThresholdLow = 0.15f;

		// Token: 0x04005361 RID: 21345
		public const float ThresholdSatisfied = 0.3f;

		// Token: 0x04005362 RID: 21346
		public const float ThresholdHigh = 0.7f;

		// Token: 0x04005363 RID: 21347
		public const float ThresholdVeryHigh = 0.85f;
	}
}
