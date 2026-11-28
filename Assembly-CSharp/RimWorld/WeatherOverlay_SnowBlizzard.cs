using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B6 RID: 5046
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowBlizzard : SkyOverlay
	{
		// Token: 0x06007D14 RID: 32020 RVA: 0x002CAE65 File Offset: 0x002C9065
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SnowBlizzard.ParticleLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SnowBlizzard.CloudLayer, 0);
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x002CAE7F File Offset: 0x002C907F
		public override void SetOverlayColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x002CAE88 File Offset: 0x002C9088
		public override void TickOverlay(Map map, float lerpFactor)
		{
			float num = 0.5f + 0.5f * GenCelestial.CurCelestialSunGlow(map);
			Color color = new Color(this.color.r * num, this.color.g * num, this.color.b * num, this.color.a);
			WeatherOverlay_SnowBlizzard.CloudLayer.color = color;
			WeatherOverlay_SnowBlizzard.ParticleLayer.color = color;
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x04004EBB RID: 20155
		private Color color;

		// Token: 0x04004EBC RID: 20156
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/SnowBlizzard/CloudLayer", -1);

		// Token: 0x04004EBD RID: 20157
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/SnowBlizzard/ParticleLayer", -1);

		// Token: 0x04004EBE RID: 20158
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.05f),
			new UnityEngine.Keyframe(3f, 1.1f)
		});

		// Token: 0x04004EBF RID: 20159
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.CloudLayer, "_MainTex", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-1f, -0.3f), 0.03f);

		// Token: 0x04004EC0 RID: 20160
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.CloudLayer, "_MainTex2", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-1f, -0.28f), 0.06f);

		// Token: 0x04004EC1 RID: 20161
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.ParticleLayer, "_MainTex", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-0.5f, -0.2f), 0.05f);

		// Token: 0x04004EC2 RID: 20162
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.ParticleLayer, "_MainTex2", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-0.5f, -0.3f), 0.06f);
	}
}
