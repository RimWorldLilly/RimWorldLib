using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A8 RID: 5032
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrikeDelayed : WeatherEvent_LightningFlash
	{
		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06007CD4 RID: 31956 RVA: 0x002C9CB1 File Offset: 0x002C7EB1
		public override bool Expired
		{
			get
			{
				return this.age > this.duration + this.delay;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06007CD5 RID: 31957 RVA: 0x002C9CC8 File Offset: 0x002C7EC8
		protected override float LightningBrightness
		{
			get
			{
				if (this.age < this.delay)
				{
					return -1f;
				}
				if (this.age - this.delay <= 3)
				{
					return (float)(this.age - this.delay) / 3f;
				}
				return 1f - (float)(this.age - this.delay) / (float)this.duration;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06007CD6 RID: 31958 RVA: 0x002C9D2C File Offset: 0x002C7F2C
		public override Vector2? OverrideShadowVector
		{
			get
			{
				if (this.age >= this.delay)
				{
					return new Vector2?(this.shadowVector);
				}
				return null;
			}
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x002C9D5C File Offset: 0x002C7F5C
		public WeatherEvent_LightningStrikeDelayed(Map map) : base(map)
		{
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x002C9D70 File Offset: 0x002C7F70
		public WeatherEvent_LightningStrikeDelayed(Map map, IntVec3 forcedStrikeLoc, int delay) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
			this.delay = delay;
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x002C9D92 File Offset: 0x002C7F92
		public override void FireEvent()
		{
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x002C9D94 File Offset: 0x002C7F94
		public override void WeatherEventTick()
		{
			this.age++;
			if (this.age == this.delay)
			{
				WeatherEvent_LightningStrike.DoStrike(this.strikeLoc, this.map, ref this.boltMesh);
			}
		}

		// Token: 0x06007CDB RID: 31963 RVA: 0x002C9DC9 File Offset: 0x002C7FC9
		public override void WeatherEventDraw()
		{
			if (this.age >= this.delay)
			{
				Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrikeDelayed.LightningMat, this.LightningBrightness), 0);
			}
		}

		// Token: 0x04004E99 RID: 20121
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04004E9A RID: 20122
		private Mesh boltMesh;

		// Token: 0x04004E9B RID: 20123
		private int delay;

		// Token: 0x04004E9C RID: 20124
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}
