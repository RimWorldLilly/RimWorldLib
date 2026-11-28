using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200038B RID: 907
	public class PawnDownedWiggler
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x000AA360 File Offset: 0x000A8560
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000AA392 File Offset: 0x000A8592
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggleOffset = Mathf.Abs(pawn.HashOffset()) % 600;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000AA3C4 File Offset: 0x000A85C4
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon -= ticksPassed;
				if (this.ticksToIncapIcon <= 0)
				{
					this.ticksToIncapIcon = 200 + this.ticksToIncapIcon;
					if (HealthAIUtility.WantsToBeRescued(this.pawn))
					{
						FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.IncapIcon, 0.42f);
					}
				}
				if (this.pawn.Awake())
				{
					if (ModsConfig.BiotechActive)
					{
						Job curJob = this.pawn.CurJob;
						if (((curJob != null) ? curJob.def : null) == JobDefOf.Breastfeed)
						{
							return;
						}
					}
					if (ticksPassed > 600)
					{
						Log.Warning("Too many ticks passed during a single frame for sensical wiggling");
					}
					int num = (Find.TickManager.TicksGame + this.wiggleOffset) % 600;
					this.ProcessWigglePeriod(num, ticksPassed, true);
					this.ProcessWigglePeriod((num + 300) % 600, ticksPassed, false);
				}
			}
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000AA4D8 File Offset: 0x000A86D8
		private void ProcessWigglePeriod(int wigglePeriodTick, int ticksPassed, bool positiveAngleWiggle)
		{
			int a = 0;
			int a2 = 90;
			int b = wigglePeriodTick - ticksPassed;
			int num = Mathf.Min(a2, wigglePeriodTick) - Mathf.Max(a, b);
			if (num > 0)
			{
				this.downedAngle += 0.35f * (float)num * (float)(positiveAngleWiggle ? 1 : -1);
			}
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000AA51E File Offset: 0x000A871E
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000AA530 File Offset: 0x000A8730
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
							return;
						}
						this.downedAngle = 135f;
						return;
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x0400169F RID: 5791
		private Pawn pawn;

		// Token: 0x040016A0 RID: 5792
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x040016A1 RID: 5793
		public int ticksToIncapIcon;

		// Token: 0x040016A2 RID: 5794
		private bool usingCustomRotation;

		// Token: 0x040016A3 RID: 5795
		private int wiggleOffset;

		// Token: 0x040016A4 RID: 5796
		private const float DownedAngleWidth = 45f;

		// Token: 0x040016A5 RID: 5797
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x040016A6 RID: 5798
		private const int IncapWigglePeriod = 300;

		// Token: 0x040016A7 RID: 5799
		private const int IncapWiggleLength = 90;

		// Token: 0x040016A8 RID: 5800
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x040016A9 RID: 5801
		private const int TicksBetweenIncapIcons = 200;
	}
}
