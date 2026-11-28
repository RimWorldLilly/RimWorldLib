using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001D8 RID: 472
	public class WeatherDef : Def
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00043E6D File Offset: 0x0004206D
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WeatherWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00043E9D File Offset: 0x0004209D
		public override IEnumerable<string> ConfigErrors()
		{
			WeatherDef.<ConfigErrors>d__38 <ConfigErrors>d__ = new WeatherDef.<ConfigErrors>d__38(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00043EAD File Offset: 0x000420AD
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x04000D31 RID: 3377
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x04000D32 RID: 3378
		public bool repeatable;

		// Token: 0x04000D33 RID: 3379
		public bool isBad;

		// Token: 0x04000D34 RID: 3380
		public bool canOccurAsRandomForcedEvent = true;

		// Token: 0x04000D35 RID: 3381
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04000D36 RID: 3382
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04000D37 RID: 3383
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x04000D38 RID: 3384
		public int transitionTicksOverride = int.MaxValue;

		// Token: 0x04000D39 RID: 3385
		public int minMonolithLevel;

		// Token: 0x04000D3A RID: 3386
		public bool canOccurInAmbientHorror;

		// Token: 0x04000D3B RID: 3387
		[MustTranslate]
		public string letterText;

		// Token: 0x04000D3C RID: 3388
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000D3D RID: 3389
		public LetterDef letterDef;

		// Token: 0x04000D3E RID: 3390
		public float rainRate;

		// Token: 0x04000D3F RID: 3391
		public float snowRate;

		// Token: 0x04000D40 RID: 3392
		public float sandRate;

		// Token: 0x04000D41 RID: 3393
		public float windSpeedFactor = 1f;

		// Token: 0x04000D42 RID: 3394
		public float windSpeedOffset;

		// Token: 0x04000D43 RID: 3395
		public float moveSpeedMultiplier = 1f;

		// Token: 0x04000D44 RID: 3396
		public float accuracyMultiplier = 1f;

		// Token: 0x04000D45 RID: 3397
		public float maxRangeCap = -1f;

		// Token: 0x04000D46 RID: 3398
		public float perceivePriority;

		// Token: 0x04000D47 RID: 3399
		public bool doToxicBuildup;

		// Token: 0x04000D48 RID: 3400
		public ThoughtDef weatherThought;

		// Token: 0x04000D49 RID: 3401
		public float maxGlow = 1f;

		// Token: 0x04000D4A RID: 3402
		public bool preventSkygaze;

		// Token: 0x04000D4B RID: 3403
		public bool preventsShuttleLaunch;

		// Token: 0x04000D4C RID: 3404
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04000D4D RID: 3405
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04000D4E RID: 3406
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x04000D4F RID: 3407
		public SkyColorSet skyColorsNightMid;

		// Token: 0x04000D50 RID: 3408
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x04000D51 RID: 3409
		public SkyColorSet skyColorsDay;

		// Token: 0x04000D52 RID: 3410
		public SkyColorSet skyColorsDusk;

		// Token: 0x04000D53 RID: 3411
		public Type workerClass = typeof(WeatherWorker);

		// Token: 0x04000D54 RID: 3412
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
