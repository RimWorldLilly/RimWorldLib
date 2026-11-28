using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000535 RID: 1333
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x000F5E4E File Offset: 0x000F404E
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000F5E5C File Offset: 0x000F405C
		public virtual float LerpFactor
		{
			get
			{
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					return Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				return 0f;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000F5EFB File Offset: 0x000F40FB
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x000F5F24 File Offset: 0x000F4124
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x000F5F58 File Offset: 0x000F4158
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000F5F70 File Offset: 0x000F4170
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000F5FE1 File Offset: 0x000F41E1
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x04001C3D RID: 7229
		private int autoAnimationStartTick;

		// Token: 0x04001C3E RID: 7230
		private int fadeInDuration;

		// Token: 0x04001C3F RID: 7231
		private int holdDuration;

		// Token: 0x04001C40 RID: 7232
		private int fadeOutDuration;

		// Token: 0x04001C41 RID: 7233
		private float autoAnimationTarget;
	}
}
