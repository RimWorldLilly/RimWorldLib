using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2D RID: 6957
	public class StaggerHandler : IExposable
	{
		// Token: 0x1700202D RID: 8237
		// (get) Token: 0x0600B265 RID: 45669 RVA: 0x003ECCB7 File Offset: 0x003EAEB7
		public bool Staggered
		{
			get
			{
				return this.staggerTicksLeft > 0;
			}
		}

		// Token: 0x1700202E RID: 8238
		// (get) Token: 0x0600B266 RID: 45670 RVA: 0x003ECCC2 File Offset: 0x003EAEC2
		public float StaggerMoveSpeedFactor
		{
			get
			{
				return this.staggerMoveSpeedFactor;
			}
		}

		// Token: 0x1700202F RID: 8239
		// (get) Token: 0x0600B267 RID: 45671 RVA: 0x003ECCCA File Offset: 0x003EAECA
		public int StaggerTicksLeft
		{
			get
			{
				return this.staggerTicksLeft;
			}
		}

		// Token: 0x0600B268 RID: 45672 RVA: 0x003ECCD2 File Offset: 0x003EAED2
		public StaggerHandler(Pawn parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600B269 RID: 45673 RVA: 0x003ECCE4 File Offset: 0x003EAEE4
		public bool StaggerFor(int ticks, float moveSpeedFactor = 0.17f)
		{
			if (ModsConfig.AnomalyActive && this.parent.health.hediffSet.HasHediff(HediffDefOf.AwokenCorpse, false))
			{
				return false;
			}
			ticks = Mathf.RoundToInt((float)ticks * this.parent.GetStatValue(StatDefOf.StaggerDurationFactor, true, -1));
			if (ticks > 0)
			{
				this.staggerTicksLeft = Mathf.Max(this.staggerTicksLeft, ticks);
				this.staggerMoveSpeedFactor = Mathf.Min(this.staggerMoveSpeedFactor, moveSpeedFactor);
				return true;
			}
			return false;
		}

		// Token: 0x0600B26A RID: 45674 RVA: 0x003ECD60 File Offset: 0x003EAF60
		public void Notify_BulletImpact(Bullet bullet)
		{
			if (this.parent.RaceProps.bulletStaggerIgnoreBodySize || this.parent.BodySize <= bullet.stoppingPower + 0.001f)
			{
				int valueOrDefault = this.parent.RaceProps.bulletStaggerDelayTicks.GetValueOrDefault(95);
				if (this.StaggerFor(valueOrDefault, this.parent.RaceProps.bulletStaggerSpeedFactor.GetValueOrDefault(0.17f)) && this.parent.RaceProps.bulletStaggerEffecterDef != null)
				{
					Effecter effecter = this.effecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.effecter = this.parent.RaceProps.bulletStaggerEffecterDef.Spawn();
				}
			}
		}

		// Token: 0x0600B26B RID: 45675 RVA: 0x003ECE14 File Offset: 0x003EB014
		public void StaggerHandlerTick()
		{
			if (this.staggerTicksLeft > 0)
			{
				this.staggerTicksLeft--;
				if (this.staggerTicksLeft <= 0)
				{
					this.staggerMoveSpeedFactor = 0.17f;
				}
			}
			if (this.staggerEffectTicksLeft > 0)
			{
				this.staggerEffectTicksLeft--;
				if (this.staggerEffectTicksLeft <= 0)
				{
					Effecter effecter = this.effecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.effecter = null;
				}
				if (this.effecter != null)
				{
					this.effecter.EffectTick(this.parent, this.parent);
				}
			}
		}

		// Token: 0x0600B26C RID: 45676 RVA: 0x003ECEAD File Offset: 0x003EB0AD
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.staggerTicksLeft, "staggerTicksLeft", 0, false);
			Scribe_Values.Look<float>(ref this.staggerMoveSpeedFactor, "staggerMoveSpeedFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.staggerEffectTicksLeft, "staggerEffectTicksLeft", 0, false);
		}

		// Token: 0x040066F0 RID: 26352
		public const float DefaultStaggerMoveSpeedFactor = 0.17f;

		// Token: 0x040066F1 RID: 26353
		public Pawn parent;

		// Token: 0x040066F2 RID: 26354
		private int staggerTicksLeft;

		// Token: 0x040066F3 RID: 26355
		private float staggerMoveSpeedFactor;

		// Token: 0x040066F4 RID: 26356
		private int staggerEffectTicksLeft;

		// Token: 0x040066F5 RID: 26357
		private Effecter effecter;
	}
}
