using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000328 RID: 808
	[StaticConstructorOnStartup]
	public class WeatherOverlayDualPanner : SkyOverlay
	{
		// Token: 0x060018C0 RID: 6336 RVA: 0x000946B1 File Offset: 0x000928B1
		public WeatherOverlayDualPanner()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(Color.clear);
			});
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x000946E0 File Offset: 0x000928E0
		public override void TickOverlay(Map map, float lerpFactor)
		{
			if (this.worldOverlayMat == null)
			{
				return;
			}
			this.worldPan1 -= this.worldPanDir1 * (this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale(WeatherOverlayDualPanner.MainTex).x * Find.TickManager.TickRateMultiplier);
			this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex, this.worldPan1);
			if (this.worldOverlayMat.HasProperty(WeatherOverlayDualPanner.MainTex2))
			{
				this.worldPan2 -= this.worldPanDir2 * (this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale(WeatherOverlayDualPanner.MainTex2).x * Find.TickManager.TickRateMultiplier);
				this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex2, this.worldPan2);
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000947C0 File Offset: 0x000929C0
		public override void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				SkyOverlay.DrawWorldOverlay(map, this.worldOverlayMat, this.GetRenderLayer());
			}
			if (this.screenOverlayMat != null)
			{
				SkyOverlay.DrawScreenOverlay(this.screenOverlayMat, this.GetRenderLayer(), null);
			}
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0009480D File Offset: 0x00092A0D
		public override void SetOverlayColor(Color color)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.color = color;
			}
			if (this.screenOverlayMat != null)
			{
				this.screenOverlayMat.color = color;
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x00094843 File Offset: 0x00092A43
		protected virtual int GetRenderLayer()
		{
			return WeatherOverlayDualPanner.RenderLayer;
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0009484C File Offset: 0x00092A4C
		public override void Reset()
		{
			this.worldPan1 = Vector2.zero;
			this.worldPan2 = Vector2.zero;
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex, this.worldPan1);
				if (this.worldOverlayMat.HasProperty(WeatherOverlayDualPanner.MainTex2))
				{
					this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex2, this.worldPan2);
				}
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000948BB File Offset: 0x00092ABB
		public override string ToString()
		{
			if (this.worldOverlayMat != null)
			{
				return this.worldOverlayMat.name;
			}
			if (this.screenOverlayMat != null)
			{
				return this.screenOverlayMat.name;
			}
			return base.ToString();
		}

		// Token: 0x0400148A RID: 5258
		private Vector2 worldPan1 = Vector2.zero;

		// Token: 0x0400148B RID: 5259
		private Vector2 worldPan2 = Vector2.zero;

		// Token: 0x0400148C RID: 5260
		public Material worldOverlayMat;

		// Token: 0x0400148D RID: 5261
		public Material screenOverlayMat;

		// Token: 0x0400148E RID: 5262
		protected float worldOverlayPanSpeed1;

		// Token: 0x0400148F RID: 5263
		protected float worldOverlayPanSpeed2;

		// Token: 0x04001490 RID: 5264
		protected Vector2 worldPanDir1;

		// Token: 0x04001491 RID: 5265
		protected Vector2 worldPanDir2;

		// Token: 0x04001492 RID: 5266
		private static readonly int RenderLayer = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x04001493 RID: 5267
		private static readonly int MainTex2 = Shader.PropertyToID("_MainTex2");

		// Token: 0x04001494 RID: 5268
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");
	}
}
