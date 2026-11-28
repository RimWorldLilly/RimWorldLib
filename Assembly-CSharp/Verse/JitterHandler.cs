using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000586 RID: 1414
	public class JitterHandler
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000FF768 File Offset: 0x000FD968
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000FF770 File Offset: 0x000FD970
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			float num = (float)ticksPassed * this.JitterDropPerTick;
			if (this.curOffset.sqrMagnitude < num * num)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * num;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000FF7D4 File Offset: 0x000FD9D4
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000FF7F7 File Offset: 0x000FD9F7
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000FF81C File Offset: 0x000FDA1C
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x04001CEE RID: 7406
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04001CEF RID: 7407
		private float DamageJitterDistance = 0.17f;

		// Token: 0x04001CF0 RID: 7408
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x04001CF1 RID: 7409
		private float JitterDropPerTick = 0.018f;

		// Token: 0x04001CF2 RID: 7410
		private float JitterMax = 0.35f;
	}
}
