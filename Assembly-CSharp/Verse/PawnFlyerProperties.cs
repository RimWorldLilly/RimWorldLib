using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000144 RID: 324
	public class PawnFlyerProperties
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00038565 File Offset: 0x00036765
		public PawnFlyerWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnFlyerWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00038595 File Offset: 0x00036795
		public AnimationCurve ProgressCurve
		{
			get
			{
				if (this.progressCurveInt == null && this.progressCurve != null)
				{
					this.progressCurveInt = this.progressCurve.ToAnimationCurve();
				}
				return this.progressCurveInt;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x000385BE File Offset: 0x000367BE
		public Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.shadowTexPath.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.shadowTexPath, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x04000626 RID: 1574
		private Type workerClass = typeof(PawnFlyerWorker);

		// Token: 0x04000627 RID: 1575
		private SimpleCurve progressCurve;

		// Token: 0x04000628 RID: 1576
		[NoTranslate]
		private string shadowTexPath = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000629 RID: 1577
		public float flightDurationMin = 0.5f;

		// Token: 0x0400062A RID: 1578
		public float flightSpeed = 12f;

		// Token: 0x0400062B RID: 1579
		public float heightFactor = 2f;

		// Token: 0x0400062C RID: 1580
		public IntRange stunDurationTicksRange = IntRange.Zero;

		// Token: 0x0400062D RID: 1581
		private PawnFlyerWorker workerInt;

		// Token: 0x0400062E RID: 1582
		private AnimationCurve progressCurveInt;

		// Token: 0x0400062F RID: 1583
		private Material cachedShadowMaterial;
	}
}
