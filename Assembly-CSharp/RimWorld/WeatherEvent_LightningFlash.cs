using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013A6 RID: 5030
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06007CC5 RID: 31941 RVA: 0x002C99A9 File Offset: 0x002C7BA9
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06007CC6 RID: 31942 RVA: 0x002C99B9 File Offset: 0x002C7BB9
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06007CC7 RID: 31943 RVA: 0x002C99D4 File Offset: 0x002C7BD4
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06007CC8 RID: 31944 RVA: 0x002C99E1 File Offset: 0x002C7BE1
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06007CC9 RID: 31945 RVA: 0x002C99E9 File Offset: 0x002C7BE9
		protected virtual float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x002C9A18 File Offset: 0x002C7C18
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x002C9A64 File Offset: 0x002C7C64
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x002C9A76 File Offset: 0x002C7C76
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x04004E8E RID: 20110
		protected int duration;

		// Token: 0x04004E8F RID: 20111
		protected Vector2 shadowVector;

		// Token: 0x04004E90 RID: 20112
		protected int age;

		// Token: 0x04004E91 RID: 20113
		protected const int FlashFadeInTicks = 3;

		// Token: 0x04004E92 RID: 20114
		private const int MinFlashDuration = 15;

		// Token: 0x04004E93 RID: 20115
		private const int MaxFlashDuration = 60;

		// Token: 0x04004E94 RID: 20116
		protected const float FlashShadowDistance = 5f;

		// Token: 0x04004E95 RID: 20117
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.78431374f, 0.8235294f, 0.84705883f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
