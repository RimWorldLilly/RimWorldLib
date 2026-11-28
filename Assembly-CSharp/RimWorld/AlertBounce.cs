using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001B9D RID: 7069
	internal class AlertBounce
	{
		// Token: 0x0600B581 RID: 46465 RVA: 0x003FE7F4 File Offset: 0x003FC9F4
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x0600B582 RID: 46466 RVA: 0x003FE820 File Offset: 0x003FCA20
		public float CalculateHorizontalOffset()
		{
			if (this.idle)
			{
				return this.position;
			}
			float num = Mathf.Min(Time.time - this.lastTime, 0.05f);
			this.lastTime = Time.time;
			this.velocity -= 1200f * num;
			this.position += this.velocity * num;
			if (this.position < 0f)
			{
				this.position = 0f;
				this.velocity = Mathf.Max(-this.velocity / 3f - 1f, 0f);
			}
			if (Mathf.Abs(this.velocity) < 0.0001f && this.position < 1f)
			{
				this.velocity = 0f;
				this.position = 0f;
				this.idle = true;
			}
			return this.position;
		}

		// Token: 0x0400685E RID: 26718
		private float position;

		// Token: 0x0400685F RID: 26719
		private float velocity;

		// Token: 0x04006860 RID: 26720
		private float lastTime = Time.time;

		// Token: 0x04006861 RID: 26721
		private bool idle;

		// Token: 0x04006862 RID: 26722
		private const float StartPosition = 300f;

		// Token: 0x04006863 RID: 26723
		private const float StartVelocity = -200f;

		// Token: 0x04006864 RID: 26724
		private const float Acceleration = 1200f;

		// Token: 0x04006865 RID: 26725
		private const float DampingRatio = 3f;

		// Token: 0x04006866 RID: 26726
		private const float DampingConstant = 1f;

		// Token: 0x04006867 RID: 26727
		private const float MaxDelta = 0.05f;
	}
}
