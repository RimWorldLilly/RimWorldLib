using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BC RID: 5052
	[StaticConstructorOnStartup]
	public class WeatherOverlay_TorrentialRain : SkyOverlay
	{
		// Token: 0x06007D23 RID: 32035 RVA: 0x002CB2C5 File Offset: 0x002C94C5
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_TorrentialRain.CloudLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_TorrentialRain.ParticleLayer, 0);
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x002CB2DF File Offset: 0x002C94DF
		public override void SetOverlayColor(Color color)
		{
			WeatherOverlay_TorrentialRain.CloudLayer.color = color;
			WeatherOverlay_TorrentialRain.ParticleLayer.color = color;
		}

		// Token: 0x06007D25 RID: 32037 RVA: 0x002CB2F7 File Offset: 0x002C94F7
		public override void TickOverlay(Map map, float lerpFactor)
		{
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x04004EC8 RID: 20168
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/TorrentialRain/CloudLayer", -1);

		// Token: 0x04004EC9 RID: 20169
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/TorrentialRain/ParticleLayer", -1);

		// Token: 0x04004ECA RID: 20170
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.075f),
			new UnityEngine.Keyframe(3f, 1.15f)
		});

		// Token: 0x04004ECB RID: 20171
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.CloudLayer, "_MainTex", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.25f, -1f), 0.05f);

		// Token: 0x04004ECC RID: 20172
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.CloudLayer, "_MainTex2", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.2f, -1f), 0.06f);

		// Token: 0x04004ECD RID: 20173
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.ParticleLayer, "_MainTex", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.34f, -1f), 0.06f);

		// Token: 0x04004ECE RID: 20174
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.ParticleLayer, "_MainTex2", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.2f, -1f), 0.05f);
	}
}
