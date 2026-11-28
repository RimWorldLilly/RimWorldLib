using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B5 RID: 5045
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SandHard : SkyOverlay
	{
		// Token: 0x06007D0F RID: 32015 RVA: 0x002CAC3E File Offset: 0x002C8E3E
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SandHard.ParticleLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SandHard.CloudLayer, 0);
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x002CAC58 File Offset: 0x002C8E58
		public override void SetOverlayColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x002CAC64 File Offset: 0x002C8E64
		public override void TickOverlay(Map map, float lerpFactor)
		{
			float num = 0.5f + 0.5f * GenCelestial.CurCelestialSunGlow(map);
			Color color = new Color(this.color.r * num * num, this.color.g * num * num, this.color.b * num, this.color.a);
			WeatherOverlay_SandHard.CloudLayer.color = color;
			WeatherOverlay_SandHard.ParticleLayer.color = color;
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x04004EB3 RID: 20147
		private Color color;

		// Token: 0x04004EB4 RID: 20148
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/Sandstorm/CloudLayer", -1);

		// Token: 0x04004EB5 RID: 20149
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/Sandstorm/ParticleLayer", -1);

		// Token: 0x04004EB6 RID: 20150
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.075f),
			new UnityEngine.Keyframe(3f, 1.15f)
		});

		// Token: 0x04004EB7 RID: 20151
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.CloudLayer, "_MainTex", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.3f), 0.04f);

		// Token: 0x04004EB8 RID: 20152
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.CloudLayer, "_MainTex2", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.28f), 0.08f);

		// Token: 0x04004EB9 RID: 20153
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.ParticleLayer, "_MainTex", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.3f), 0.03f);

		// Token: 0x04004EBA RID: 20154
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.ParticleLayer, "_MainTex2", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.28f), 0.06f);
	}
}
