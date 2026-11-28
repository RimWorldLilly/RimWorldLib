using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000105 RID: 261
	public class CameraSwooper
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x00033F05 File Offset: 0x00032105
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00033F3D File Offset: 0x0003213D
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00033F54 File Offset: 0x00032154
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00033FA4 File Offset: 0x000321A4
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x0400043A RID: 1082
		public bool Swooping;

		// Token: 0x0400043B RID: 1083
		private bool SwoopingTo;

		// Token: 0x0400043C RID: 1084
		private float TimeSinceSwoopStart;

		// Token: 0x0400043D RID: 1085
		private Vector3 FinalOffset;

		// Token: 0x0400043E RID: 1086
		private float FinalOrthoSizeOffset;

		// Token: 0x0400043F RID: 1087
		private float TotalSwoopTime;

		// Token: 0x04000440 RID: 1088
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
