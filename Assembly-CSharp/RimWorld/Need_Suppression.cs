using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001588 RID: 5512
	public class Need_Suppression : Need
	{
		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x0600870E RID: 34574 RVA: 0x0030CF3A File Offset: 0x0030B13A
		public bool CanBeSuppressedNow
		{
			get
			{
				return this.CurLevel < 0.7f;
			}
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x0600870F RID: 34575 RVA: 0x0030CF49 File Offset: 0x0030B149
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x06008710 RID: 34576 RVA: 0x0030CF58 File Offset: 0x0030B158
		public Need_Suppression(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06008711 RID: 34577 RVA: 0x0030CF61 File Offset: 0x0030B161
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.0025f * this.pawn.GetStatValue(StatDefOf.SlaveSuppressionFallRate, true, -1);
			}
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x0030CF90 File Offset: 0x0030B190
		public void DrawSuppressionBar(Rect rect)
		{
			Widgets.FillableBar(rect, base.CurLevelPercentage, GuestUtility.SlaveSuppressionFillTex);
			base.DrawBarThreshold(rect, 0.3f);
			base.DrawBarThreshold(rect, 0.15f);
		}

		// Token: 0x04005381 RID: 21377
		private const float CanSuppressMaxThreshold = 0.7f;

		// Token: 0x04005382 RID: 21378
		private const float SuppressCriticalThreshold = 0.3f;
	}
}
