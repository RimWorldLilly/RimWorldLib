using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EC RID: 1260
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x0600256E RID: 9582 RVA: 0x000E3AED File Offset: 0x000E1CED
		public Stance_Cooldown()
		{
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000E3AF5 File Offset: 0x000E1CF5
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000E3B00 File Offset: 0x000E1D00
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04001B06 RID: 6918
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04001B07 RID: 6919
		private const float MaxRadius = 0.5f;
	}
}
