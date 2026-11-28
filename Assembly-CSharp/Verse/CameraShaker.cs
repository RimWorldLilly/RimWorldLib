using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000103 RID: 259
	public class CameraShaker
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00033CFD File Offset: 0x00031EFD
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x00033D05 File Offset: 0x00031F05
		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, this.GetMaxShakeMag());
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00033D20 File Offset: 0x00031F20
		public Vector3 ShakeOffset
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return Vector3.zero;
				}
				if (Find.TickManager.Paused && !WorldComponent_GravshipController.CutsceneInProgress)
				{
					return Vector3.zero;
				}
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, 0f, z);
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00033D94 File Offset: 0x00031F94
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00033DAD File Offset: 0x00031FAD
		public void DoShake(float mag, int durationTicks)
		{
			this.extendedShakeRequests.Add(new ExtendedShakeRequest(mag, durationTicks));
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00033DC1 File Offset: 0x00031FC1
		public float GetMaxShakeMag()
		{
			return 0.2f * Prefs.ScreenShakeIntensity;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00033DCE File Offset: 0x00031FCE
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00033DE2 File Offset: 0x00031FE2
		public void StopAllShaking()
		{
			this.CurShakeMag = 0f;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00033DF0 File Offset: 0x00031FF0
		public void Update()
		{
			for (int i = this.extendedShakeRequests.Count - 1; i >= 0; i--)
			{
				ExtendedShakeRequest extendedShakeRequest = this.extendedShakeRequests[i];
				if (Find.TickManager.TicksGame > extendedShakeRequest.StartTick + extendedShakeRequest.Duration)
				{
					this.extendedShakeRequests.RemoveAt(i);
				}
				else
				{
					this.SetMinShake(extendedShakeRequest.Mag);
				}
			}
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00033E88 File Offset: 0x00032088
		public void Expose()
		{
			Scribe_Collections.Look<ExtendedShakeRequest>(ref this.extendedShakeRequests, "extendedShakeRequests", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<ExtendedShakeRequest>(ref this.unpausedExtendedShakeRequests, "unpausedExtendedShakeRequests", LookMode.Deep, Array.Empty<object>());
			if (this.extendedShakeRequests == null)
			{
				this.extendedShakeRequests = new List<ExtendedShakeRequest>();
			}
			if (this.unpausedExtendedShakeRequests == null)
			{
				this.unpausedExtendedShakeRequests = new List<ExtendedShakeRequest>();
			}
		}

		// Token: 0x04000434 RID: 1076
		private float curShakeMag;

		// Token: 0x04000435 RID: 1077
		private List<ExtendedShakeRequest> extendedShakeRequests = new List<ExtendedShakeRequest>();

		// Token: 0x04000436 RID: 1078
		private List<ExtendedShakeRequest> unpausedExtendedShakeRequests = new List<ExtendedShakeRequest>();

		// Token: 0x04000437 RID: 1079
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04000438 RID: 1080
		private const float ShakeFrequency = 24f;

		// Token: 0x04000439 RID: 1081
		private const float MaxShakeMag = 0.2f;
	}
}
