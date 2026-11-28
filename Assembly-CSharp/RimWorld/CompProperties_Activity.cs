using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EB RID: 6123
	public class CompProperties_Activity : CompProperties
	{
		// Token: 0x060098B8 RID: 39096 RVA: 0x00375498 File Offset: 0x00373698
		public CompProperties_Activity()
		{
			this.compClass = typeof(CompActivity);
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x060098B9 RID: 39097 RVA: 0x00375570 File Offset: 0x00373770
		public ActivityWorker Worker
		{
			get
			{
				ActivityWorker result;
				if ((result = this.workerInt) == null)
				{
					result = (this.workerInt = (ActivityWorker)Activator.CreateInstance(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x04005B3E RID: 23358
		public float changePerDayBase = 0.1f;

		// Token: 0x04005B3F RID: 23359
		public float? changePerDayOutside;

		// Token: 0x04005B40 RID: 23360
		public float changePerDayElectroharvester = 0.15f;

		// Token: 0x04005B41 RID: 23361
		public float changePerDamage = 0.005f;

		// Token: 0x04005B42 RID: 23362
		public float warning = 0.7f;

		// Token: 0x04005B43 RID: 23363
		public bool dirtyGraphicsOnActivityChange;

		// Token: 0x04005B44 RID: 23364
		public FloatRange startingRange = FloatRange.ZeroToOne;

		// Token: 0x04005B45 RID: 23365
		public Type workerClass = typeof(PawnActivityWorker);

		// Token: 0x04005B46 RID: 23366
		public SimpleCurve activityResearchFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0.99f, 2f),
				true
			}
		};

		// Token: 0x04005B47 RID: 23367
		public SimpleCurve damagedActivityMultiplierCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04005B48 RID: 23368
		public bool showLetterOnActivated;

		// Token: 0x04005B49 RID: 23369
		public bool showLetterOnManualActivation;

		// Token: 0x04005B4A RID: 23370
		public bool requiresHoldingPlatform;

		// Token: 0x04005B4B RID: 23371
		public LetterDef letterDef;

		// Token: 0x04005B4C RID: 23372
		[MustTranslate]
		public string letterTitle;

		// Token: 0x04005B4D RID: 23373
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04005B4E RID: 23374
		private ActivityWorker workerInt;
	}
}
