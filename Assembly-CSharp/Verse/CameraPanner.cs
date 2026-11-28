using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000101 RID: 257
	public struct CameraPanner
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x00033B3E File Offset: 0x00031D3E
		public void PanTo(CameraPanner.Interpolant source, CameraPanner.Interpolant destination, float duration = 0.25f, PanCompletionCallback completion = null)
		{
			this.Moving = true;
			this.TimeSinceStart = 0f;
			this.Source = source;
			this.Destination = destination;
			this.Duration = duration;
			this.Completion = completion;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00033B6F File Offset: 0x00031D6F
		public void JumpOnNextUpdate()
		{
			this.TimeSinceStart = this.Duration;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00033B80 File Offset: 0x00031D80
		public CameraPanner.Interpolant? Update()
		{
			if (!this.Moving)
			{
				return null;
			}
			this.TimeSinceStart += Time.deltaTime;
			if (this.TimeSinceStart >= this.Duration)
			{
				this.Moving = false;
				PanCompletionCallback completion = this.Completion;
				if (completion != null)
				{
					completion();
				}
				this.Completion = null;
			}
			return new CameraPanner.Interpolant?(this.GetCurrentInterpolation());
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00033BEC File Offset: 0x00031DEC
		private CameraPanner.Interpolant GetCurrentInterpolation()
		{
			float t = CameraPanner.SmootherStep(this.TimeSinceStart / this.Duration);
			return new CameraPanner.Interpolant(Vector3.LerpUnclamped(this.Source.Position, this.Destination.Position, t), Mathf.LerpUnclamped(this.Source.Size, this.Destination.Size, t));
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00033C4C File Offset: 0x00031E4C
		public static float SmootherStep(float t)
		{
			float num = Mathf.Clamp01(t);
			return num * num * num * (num * (num * 6f - 15f) + 10f);
		}

		// Token: 0x0400042A RID: 1066
		public bool Moving;

		// Token: 0x0400042B RID: 1067
		private float TimeSinceStart;

		// Token: 0x0400042C RID: 1068
		private CameraPanner.Interpolant Source;

		// Token: 0x0400042D RID: 1069
		private CameraPanner.Interpolant Destination;

		// Token: 0x0400042E RID: 1070
		private float Duration;

		// Token: 0x0400042F RID: 1071
		private PanCompletionCallback Completion;

		// Token: 0x04000430 RID: 1072
		public const float DefaultDuration = 0.25f;

		// Token: 0x0200250D RID: 9485
		public readonly struct Interpolant
		{
			// Token: 0x0600EC8E RID: 60558 RVA: 0x00573B81 File Offset: 0x00571D81
			public Interpolant(Vector3 position, float size)
			{
				this.Position = position;
				this.Size = size;
			}

			// Token: 0x170029CD RID: 10701
			// (get) Token: 0x0600EC8F RID: 60559 RVA: 0x00573B91 File Offset: 0x00571D91
			public Vector3 Position { get; }

			// Token: 0x170029CE RID: 10702
			// (get) Token: 0x0600EC90 RID: 60560 RVA: 0x00573B99 File Offset: 0x00571D99
			public float Size { get; }
		}
	}
}
