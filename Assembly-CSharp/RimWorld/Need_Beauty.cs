using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001568 RID: 5480
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x0600866E RID: 34414 RVA: 0x00309BB8 File Offset: 0x00307DB8
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastInstantBeautyCheckTick != Find.TickManager.TicksGame)
				{
					if (!this.pawn.Spawned)
					{
						this.lastInstantBeauty = 0.5f;
					}
					else if (PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
					{
						this.lastInstantBeauty = 0.5f;
					}
					else
					{
						this.lastInstantBeauty = this.LevelFromBeauty(this.CurrentInstantBeauty());
					}
					this.lastInstantBeautyCheckTick = Find.TickManager.TicksGame;
				}
				return this.lastInstantBeauty;
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x0600866F RID: 34415 RVA: 0x00309C34 File Offset: 0x00307E34
		public BeautyCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.99f)
				{
					return BeautyCategory.Beautiful;
				}
				if (this.CurLevel > 0.85f)
				{
					return BeautyCategory.VeryPretty;
				}
				if (this.CurLevel > 0.65f)
				{
					return BeautyCategory.Pretty;
				}
				if (this.CurLevel > 0.35f)
				{
					return BeautyCategory.Neutral;
				}
				if (this.CurLevel > 0.15f)
				{
					return BeautyCategory.Ugly;
				}
				if (this.CurLevel > 0.01f)
				{
					return BeautyCategory.VeryUgly;
				}
				return BeautyCategory.Hideous;
			}
		}

		// Token: 0x06008670 RID: 34416 RVA: 0x00309C9C File Offset: 0x00307E9C
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x06008671 RID: 34417 RVA: 0x00309D11 File Offset: 0x00307F11
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x06008672 RID: 34418 RVA: 0x00309D2B File Offset: 0x00307F2B
		public float CurrentInstantBeauty()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
		}

		// Token: 0x040052C5 RID: 21189
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x040052C6 RID: 21190
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x040052C7 RID: 21191
		private const float ThreshUgly = 0.15f;

		// Token: 0x040052C8 RID: 21192
		private const float ThreshNeutral = 0.35f;

		// Token: 0x040052C9 RID: 21193
		private const float ThreshPretty = 0.65f;

		// Token: 0x040052CA RID: 21194
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x040052CB RID: 21195
		private const float ThreshBeautiful = 0.99f;

		// Token: 0x040052CC RID: 21196
		private int lastInstantBeautyCheckTick = -9999;

		// Token: 0x040052CD RID: 21197
		private float lastInstantBeauty = -1f;
	}
}
