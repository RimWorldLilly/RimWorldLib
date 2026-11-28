using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037A RID: 890
	public class PawnLeaner
	{
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x000A75B8 File Offset: 0x000A57B8
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000A75DA File Offset: 0x000A57DA
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000A75F8 File Offset: 0x000A57F8
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
					return;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A7668 File Offset: 0x000A5868
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000A769B File Offset: 0x000A589B
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 shootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04001649 RID: 5705
		private readonly Pawn pawn;

		// Token: 0x0400164A RID: 5706
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x0400164B RID: 5707
		private float leanOffsetCurPct;

		// Token: 0x0400164C RID: 5708
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x0400164D RID: 5709
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
