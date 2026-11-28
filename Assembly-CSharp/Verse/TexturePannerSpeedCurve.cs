using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000324 RID: 804
	public class TexturePannerSpeedCurve : TexturePanner
	{
		// Token: 0x060018AC RID: 6316 RVA: 0x00094488 File Offset: 0x00092688
		public TexturePannerSpeedCurve(Material material, Vector2 direction, ComplexCurve timescaleCurve, float speed) : base(material, Shader.PropertyToID("_MainTex"), direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x000944A5 File Offset: 0x000926A5
		public TexturePannerSpeedCurve(Material material, string property, ComplexCurve timescaleCurve, Vector2 direction, float speed) : base(material, Shader.PropertyToID(property), direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000944BF File Offset: 0x000926BF
		public TexturePannerSpeedCurve(Material material, int propertyID, ComplexCurve timescaleCurve, Vector2 direction, float speed) : base(material, propertyID, direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x000944D4 File Offset: 0x000926D4
		public override void Tick()
		{
			this.pan -= this.direction * this.speed * this.material.GetTextureScale(this.propertyID).x * this.timescaleCurve.Evaluate(Find.TickManager.TickRateMultiplier);
			this.material.SetTextureOffset(this.propertyID, this.pan);
		}

		// Token: 0x04001485 RID: 5253
		public ComplexCurve timescaleCurve;
	}
}
