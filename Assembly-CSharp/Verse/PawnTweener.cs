using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public class PawnTweener
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x000A7BCC File Offset: 0x000A5DCC
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x000A7BD4 File Offset: 0x000A5DD4
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000A7BE8 File Offset: 0x000A5DE8
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x000A7C3C File Offset: 0x000A5E3C
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame == RealTime.frameCount)
			{
				return;
			}
			if (!this.pawn.Spawned)
			{
				this.tweenedPos = this.pawn.Position.ToVector3Shifted();
				return;
			}
			if (this.lastDrawFrame < RealTime.frameCount - 1 && this.lastDrawTick < GenTicks.TicksGame - 1)
			{
				this.ResetTweenedPosToRoot();
			}
			else
			{
				this.lastTickSpringPos = this.tweenedPos;
				float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
				if (tickRateMultiplier < 5f)
				{
					Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
					float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
					if (RealTime.deltaTime > 0.05f)
					{
						num = Mathf.Min(num, 1f);
					}
					this.tweenedPos += a * num;
				}
				else
				{
					this.ResetTweenedPosToRoot();
				}
			}
			this.lastDrawFrame = RealTime.frameCount;
			this.lastDrawTick = GenTicks.TicksGame;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000A7D37 File Offset: 0x000A5F37
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
			this.lastDrawFrame = RealTime.frameCount;
			this.lastDrawTick = GenTicks.TicksGame;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000A7D67 File Offset: 0x000A5F67
		public void Notify_Teleported()
		{
			this.lastDrawFrame = -1;
			this.lastDrawTick = -1;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000A7D78 File Offset: 0x000A5F78
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float z = 0f;
			if (this.pawn.Spawned && this.pawn.ageTracker.CurLifeStage.sittingOffset != null && !this.pawn.pather.MovingNow && this.pawn.GetPosture() == PawnPosture.Standing)
			{
				Building edifice = this.pawn.Position.GetEdifice(this.pawn.Map);
				if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
				{
					z = this.pawn.ageTracker.CurLifeStage.sittingOffset.Value;
				}
			}
			float num = this.pawn.pather.MovePercentage;
			if (this.pawn.Crawling)
			{
				num = num - num % 0.33333334f + Mathf.Pow(num % 0.33333334f * 3f, 3f) * 0.33333334f;
			}
			Vector3 b;
			if (GenTicks.TicksGame == this.lastOffsetTick)
			{
				b = this.lastOffset;
			}
			else
			{
				b = (this.lastOffset = PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn));
				this.lastOffsetTick = GenTicks.TicksGame;
			}
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + new Vector3(0f, 0f, z) + b;
		}

		// Token: 0x0400165C RID: 5724
		private Pawn pawn;

		// Token: 0x0400165D RID: 5725
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x0400165E RID: 5726
		private int lastDrawFrame = -1;

		// Token: 0x0400165F RID: 5727
		private int lastDrawTick = -1;

		// Token: 0x04001660 RID: 5728
		private int lastOffsetTick = -1;

		// Token: 0x04001661 RID: 5729
		private Vector3 lastOffset = Vector3.zero;

		// Token: 0x04001662 RID: 5730
		private Vector3 lastTickSpringPos;

		// Token: 0x04001663 RID: 5731
		private const float SpringTightness = 0.09f;

		// Token: 0x04001664 RID: 5732
		private const float CrawlsPerTile = 3f;

		// Token: 0x04001665 RID: 5733
		private const float CrawlingLurchFactor = 3f;
	}
}
