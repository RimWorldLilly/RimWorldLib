using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002398 RID: 9112
	public class Caravan_Tweener
	{
		// Token: 0x170027E1 RID: 10209
		// (get) Token: 0x0600E2BA RID: 58042 RVA: 0x00543218 File Offset: 0x00541418
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x170027E2 RID: 10210
		// (get) Token: 0x0600E2BB RID: 58043 RVA: 0x00543220 File Offset: 0x00541420
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x170027E3 RID: 10211
		// (get) Token: 0x0600E2BC RID: 58044 RVA: 0x00543233 File Offset: 0x00541433
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x0600E2BD RID: 58045 RVA: 0x00543250 File Offset: 0x00541450
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E2BE RID: 58046 RVA: 0x0054326C File Offset: 0x0054146C
		public void TweenerTickInterval(int delta)
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x0600E2BF RID: 58047 RVA: 0x005432B3 File Offset: 0x005414B3
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x040090AF RID: 37039
		private Caravan caravan;

		// Token: 0x040090B0 RID: 37040
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x040090B1 RID: 37041
		private Vector3 lastTickSpringPos;

		// Token: 0x040090B2 RID: 37042
		private const float SpringTightness = 0.09f;
	}
}
