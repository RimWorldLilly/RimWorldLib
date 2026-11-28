using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001919 RID: 6425
	public abstract class CompObelisk : ThingComp, IActivity
	{
		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x0600A110 RID: 41232 RVA: 0x0039956C File Offset: 0x0039776C
		protected CompProperties_Obelisk Props
		{
			get
			{
				return (CompProperties_Obelisk)this.props;
			}
		}

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x0600A111 RID: 41233 RVA: 0x00399579 File Offset: 0x00397779
		protected virtual float InteractionEffectChanceMTBDays
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x0600A112 RID: 41234 RVA: 0x00399580 File Offset: 0x00397780
		protected CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyInt) == null)
				{
					result = (this.studyInt = this.parent.TryGetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x0600A113 RID: 41235 RVA: 0x003995AC File Offset: 0x003977AC
		public CompActivity ActivityComp
		{
			get
			{
				CompActivity result;
				if ((result = this.activityInt) == null)
				{
					result = (this.activityInt = this.parent.TryGetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x0600A114 RID: 41236
		public abstract void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false);

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x0600A115 RID: 41237 RVA: 0x003995D7 File Offset: 0x003977D7
		public bool StudyFinished
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x0600A116 RID: 41238 RVA: 0x003995E4 File Offset: 0x003977E4
		public int StudyLevel
		{
			get
			{
				return this.StudyUnlocks.Progress;
			}
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x0600A117 RID: 41239 RVA: 0x003995F1 File Offset: 0x003977F1
		public bool Activated
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x0600A118 RID: 41240 RVA: 0x003995F9 File Offset: 0x003977F9
		public override void Initialize(CompProperties props)
		{
			if (!ModLister.CheckAnomaly("CompObelisk"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Initialize(props);
		}

		// Token: 0x0600A119 RID: 41241 RVA: 0x0039961B File Offset: 0x0039781B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.OnImpact();
			}
		}

		// Token: 0x0600A11A RID: 41242 RVA: 0x0039963C File Offset: 0x0039783C
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (!this.ActivityComp.Deactivated)
			{
				return;
			}
			foreach (Thing thing in leavings)
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			leavings.Clear();
		}

		// Token: 0x0600A11B RID: 41243 RVA: 0x0039969C File Offset: 0x0039789C
		public override void CompTick()
		{
			this.SoundTick();
			if (!this.warmupComplete && GenTicks.TicksGame >= this.delayedActionTick)
			{
				this.warmupComplete = true;
				Effecter effecter = this.obeliskWarmupEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.obeliskWarmupEffecter = null;
			}
			else if (this.activated && this.obeliskWarmupEffecter == null && !this.warmupComplete)
			{
				this.obeliskWarmupEffecter = EffecterDefOf.ObeliskActionWarmup.Spawn(this.parent, this.parent.MapHeld, 1f).Trigger(this.parent, this.parent, -1);
			}
			Effecter effecter2 = this.obeliskWarmupEffecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0600A11C RID: 41244 RVA: 0x00399768 File Offset: 0x00397968
		private void OnImpact()
		{
			EffecterDefOf.ImpactDustCloud.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			for (int i = 0; i < this.Props.rubbleFilthCount; i++)
			{
				IntVec3 c;
				if (CellFinder.TryFindRandomCellNear(this.parent.OccupiedRect().RandomCell, this.parent.Map, 2, null, out c, -1))
				{
					FilthMaker.TryMakeFilth(c, this.parent.Map, Rand.Bool ? ThingDefOf.Filth_Dirt : ThingDefOf.Filth_RubbleRock, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x0600A11D RID: 41245 RVA: 0x00399808 File Offset: 0x00397A08
		public void Notify_InteractedTick(Pawn interactor, int delta)
		{
			if (this.activated)
			{
				return;
			}
			if (this.lastInteractionEffectTick + this.Props.interactionEffectCooldownDays * 60000 > Find.TickManager.TicksGame)
			{
				return;
			}
			if (!Rand.MTBEventOccurs(this.InteractionEffectChanceMTBDays, 60000f, (float)delta))
			{
				return;
			}
			this.TriggerInteractionEffect(interactor, false);
		}

		// Token: 0x0600A11E RID: 41246 RVA: 0x00399860 File Offset: 0x00397A60
		public virtual void OnActivityActivated()
		{
			this.activated = true;
			this.delayedActionTick = GenTicks.TicksGame + 240;
		}

		// Token: 0x0600A11F RID: 41247 RVA: 0x0039987A File Offset: 0x00397A7A
		public void OnPassive()
		{
		}

		// Token: 0x0600A120 RID: 41248 RVA: 0x0039987C File Offset: 0x00397A7C
		public bool ShouldGoPassive()
		{
			return false;
		}

		// Token: 0x0600A121 RID: 41249 RVA: 0x0039987F File Offset: 0x00397A7F
		public bool CanBeSuppressed()
		{
			return true;
		}

		// Token: 0x0600A122 RID: 41250 RVA: 0x00399882 File Offset: 0x00397A82
		public bool CanActivate()
		{
			return true;
		}

		// Token: 0x0600A123 RID: 41251 RVA: 0x00399885 File Offset: 0x00397A85
		public string ActivityTooltipExtra()
		{
			return null;
		}

		// Token: 0x0600A124 RID: 41252 RVA: 0x00399888 File Offset: 0x00397A88
		private void SoundTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.Fogged())
			{
				Sustainer sustainer = this.stageOneSustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
				Sustainer sustainer2 = this.stageTwoSustainer;
				if (sustainer2 != null)
				{
					sustainer2.End();
				}
				Sustainer sustainer3 = this.stageThreeSustainer;
				if (sustainer3 == null)
				{
					return;
				}
				sustainer3.End();
				return;
			}
			else
			{
				float num = CompObelisk.SustainerStudiedStageCurve.Evaluate(this.ActivityComp.ActivityLevel);
				if (num > 0f)
				{
					if (this.stageOneSustainer == null || this.stageOneSustainer.Ended)
					{
						this.stageOneSustainer = SoundDefOf.ObeliskAmbientStageOne.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageOneSustainer.info.volumeFactor = Mathf.Min(num, 1f);
					this.stageOneSustainer.Maintain();
				}
				else
				{
					Sustainer sustainer4 = this.stageOneSustainer;
					if (sustainer4 != null)
					{
						sustainer4.End();
					}
				}
				if (num > 1f)
				{
					if (this.stageTwoSustainer == null || this.stageTwoSustainer.Ended)
					{
						this.stageTwoSustainer = SoundDefOf.ObeliskAmbientStageTwo.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageTwoSustainer.info.volumeFactor = Mathf.Min(num - 1f, 1f);
					this.stageTwoSustainer.Maintain();
				}
				else
				{
					Sustainer sustainer5 = this.stageTwoSustainer;
					if (sustainer5 != null)
					{
						sustainer5.End();
					}
				}
				if (num > 2f)
				{
					if (this.stageThreeSustainer == null || this.stageThreeSustainer.Ended)
					{
						this.stageThreeSustainer = SoundDefOf.ObeliskAmbientStageThree.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageThreeSustainer.info.volumeFactor = Mathf.Min(num - 2f, 1f);
					this.stageThreeSustainer.Maintain();
					return;
				}
				Sustainer sustainer6 = this.stageThreeSustainer;
				if (sustainer6 == null)
				{
					return;
				}
				sustainer6.End();
				return;
			}
		}

		// Token: 0x0600A125 RID: 41253 RVA: 0x00399A68 File Offset: 0x00397C68
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompObelisk.<CompGetGizmosExtra>d__40 <CompGetGizmosExtra>d__ = new CompObelisk.<CompGetGizmosExtra>d__40(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A126 RID: 41254 RVA: 0x00399A78 File Offset: 0x00397C78
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInteractionEffectTick, "lastInteractionEffectTick", 0, false);
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_Values.Look<int>(ref this.delayedActionTick, "delayedActionTick", int.MaxValue, false);
		}

		// Token: 0x04005F25 RID: 24357
		protected Effecter obeliskWarmupEffecter;

		// Token: 0x04005F26 RID: 24358
		protected const int WarmupDelayTicks = 240;

		// Token: 0x04005F27 RID: 24359
		protected bool warmupComplete;

		// Token: 0x04005F28 RID: 24360
		private int delayedActionTick = int.MaxValue;

		// Token: 0x04005F29 RID: 24361
		protected bool activated;

		// Token: 0x04005F2A RID: 24362
		protected int lastInteractionEffectTick = -9999999;

		// Token: 0x04005F2B RID: 24363
		private Sustainer stageOneSustainer;

		// Token: 0x04005F2C RID: 24364
		private Sustainer stageTwoSustainer;

		// Token: 0x04005F2D RID: 24365
		private Sustainer stageThreeSustainer;

		// Token: 0x04005F2E RID: 24366
		private CompStudyUnlocks studyInt;

		// Token: 0x04005F2F RID: 24367
		private CompActivity activityInt;

		// Token: 0x04005F30 RID: 24368
		private static readonly SimpleCurve SustainerStudiedStageCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 2f),
				true
			},
			{
				new CurvePoint(1f, 3f),
				true
			}
		};
	}
}
