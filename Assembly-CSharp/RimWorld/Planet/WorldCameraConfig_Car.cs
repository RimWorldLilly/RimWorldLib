using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002305 RID: 8965
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x0600DD6E RID: 56686 RVA: 0x00526F78 File Offset: 0x00525178
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x0600DD6F RID: 56687 RVA: 0x00526FAC File Offset: 0x005251AC
		public override void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
			base.ConfigFixedUpdate_60(ref rotationVelocity);
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.targetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.targetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.speed += 1.5f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.5f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			rotationVelocity.x = Mathf.Cos(this.angle) * this.speed;
			rotationVelocity.y = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x04008E12 RID: 36370
		private float targetAngle;

		// Token: 0x04008E13 RID: 36371
		private float angle;

		// Token: 0x04008E14 RID: 36372
		private float speed;

		// Token: 0x04008E15 RID: 36373
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x04008E16 RID: 36374
		private const float AngleChangeSpeed = 0.72f;
	}
}
