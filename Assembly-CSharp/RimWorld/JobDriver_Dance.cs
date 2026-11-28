using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009DF RID: 2527
	public class JobDriver_Dance : JobDriver
	{
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x001D78BC File Offset: 0x001D5ABC
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x001D78D0 File Offset: 0x001D5AD0
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				float num = Mathf.Sin((float)this.AgeTicks / 60f * 8f);
				if (this.jumping)
				{
					float z = Mathf.Max(Mathf.Pow((num + 1f) * 0.5f, 2f) * 0.2f - 0.06f, 0f);
					return new Vector3(0f, 0f, z);
				}
				float num2 = Mathf.Sign(num);
				return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(num) * 0.6f) * 0.09f * num2, 0f, 0f);
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x001D796C File Offset: 0x001D5B6C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x001D796F File Offset: 0x001D5B6F
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.pawn.Rotation = Rot4.Random;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001D7987 File Offset: 0x001D5B87
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Dance.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Dance.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400301B RID: 12315
		private bool jumping;

		// Token: 0x0400301C RID: 12316
		private int moveChangeInterval = 240;
	}
}
