using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002316 RID: 8982
	public class WorldDrawLayer_Clouds : WorldDrawLayer
	{
		// Token: 0x0600DDDA RID: 56794 RVA: 0x005291B0 File Offset: 0x005273B0
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Clouds.<Regenerate>d__9 <Regenerate>d__ = new WorldDrawLayer_Clouds.<Regenerate>d__9(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DDDB RID: 56795 RVA: 0x005291C0 File Offset: 0x005273C0
		private float GetTargetOpacity()
		{
			if (PlanetLayer.Selected == this.planetLayer && !WorldRendererUtility.WorldBackgroundNow)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x0600DDDC RID: 56796 RVA: 0x005291E4 File Offset: 0x005273E4
		public override void Render()
		{
			this.opacity = ((!this.set) ? this.GetTargetOpacity() : Mathf.SmoothDamp(this.opacity, this.GetTargetOpacity(), ref this.opacityVel, 0.15f));
			this.material.SetFloat(ShaderPropertyIDs.CloudShaderOpacity, this.opacity);
			this.set = true;
			base.Render();
		}

		// Token: 0x04008E70 RID: 36464
		private Material material;

		// Token: 0x04008E71 RID: 36465
		private Texture2D noiseTexture;

		// Token: 0x04008E72 RID: 36466
		private Texture2D sunsetColorRamp;

		// Token: 0x04008E73 RID: 36467
		private Texture2D cloudTexture;

		// Token: 0x04008E74 RID: 36468
		private float opacity = 1f;

		// Token: 0x04008E75 RID: 36469
		private float opacityVel;

		// Token: 0x04008E76 RID: 36470
		private bool set;

		// Token: 0x04008E77 RID: 36471
		private const int SubdivisionsCount = 4;

		// Token: 0x04008E78 RID: 36472
		private const float OpacityChangeTime = 0.15f;
	}
}
