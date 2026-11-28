using System;

namespace Verse
{
	// Token: 0x02000729 RID: 1833
	public static class ShootTuning
	{
		// Token: 0x0400261F RID: 9759
		public const float DistTouch = 3f;

		// Token: 0x04002620 RID: 9760
		public const float DistShort = 12f;

		// Token: 0x04002621 RID: 9761
		public const float DistMedium = 25f;

		// Token: 0x04002622 RID: 9762
		public const float DistLong = 40f;

		// Token: 0x04002623 RID: 9763
		public const float MeleeRange = 1.42f;

		// Token: 0x04002624 RID: 9764
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x04002625 RID: 9765
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x04002626 RID: 9766
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04002627 RID: 9767
		public const float HitChanceFactorIfLayingDown = 0.5f;

		// Token: 0x04002628 RID: 9768
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04002629 RID: 9769
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x0400262A RID: 9770
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x0400262B RID: 9771
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x0400262C RID: 9772
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x0400262D RID: 9773
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x0400262E RID: 9774
		public static readonly SimpleSurface MissDistanceFromAimOnChanceCurves = new SimpleSurface
		{
			new SurfaceColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new SurfaceColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new SurfaceColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new SurfaceColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new SurfaceColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new SurfaceColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x0400262F RID: 9775
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x04002630 RID: 9776
		public const float InterceptDistMin = 5f;

		// Token: 0x04002631 RID: 9777
		public const float InterceptDistMax = 12f;

		// Token: 0x04002632 RID: 9778
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x04002633 RID: 9779
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x04002634 RID: 9780
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x04002635 RID: 9781
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x04002636 RID: 9782
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x04002637 RID: 9783
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x04002638 RID: 9784
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x04002639 RID: 9785
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x0400263A RID: 9786
		public const float BodySizeClampMax = 2f;
	}
}
