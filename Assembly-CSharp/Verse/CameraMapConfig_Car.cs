using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F8 RID: 248
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x00032D56 File Offset: 0x00030F56
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00032D8C File Offset: 0x00030F8C
		public override void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.autoPanTargetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.autoPanTargetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.autoPanSpeed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.autoPanSpeed -= 1.2f * num;
				if (this.autoPanSpeed < 0f)
				{
					this.autoPanSpeed = 0f;
				}
			}
			this.autoPanAngle = Mathf.Lerp(this.autoPanAngle, this.autoPanTargetAngle, 0.02f);
			base.ConfigFixedUpdate_60(ref rootPos, ref velocity);
		}

		// Token: 0x04000421 RID: 1057
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000422 RID: 1058
		private const float AngleChangeSpeed = 0.72f;
	}
}
