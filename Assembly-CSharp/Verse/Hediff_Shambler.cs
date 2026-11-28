using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004C9 RID: 1225
	public class Hediff_Shambler : HediffWithComps
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000CA485 File Offset: 0x000C8685
		public bool IsRising
		{
			get
			{
				return this.pawn.health.hediffSet.HasHediff(HediffDefOf.Rising, false);
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000CA4A4 File Offset: 0x000C86A4
		public override void PostMake()
		{
			base.PostMake();
			this.headRotation = Rand.RangeSeeded(-20f, 20f, this.pawn.thingIDNumber);
			if (!this.pawn.Dead)
			{
				this.pawn.timesRaisedAsShambler++;
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000CA4F7 File Offset: 0x000C86F7
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Shambler"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000CA51E File Offset: 0x000C871E
		public override void Notify_Spawned()
		{
			base.Notify_Spawned();
			this.pawn.Map.mapPawns.RegisterShambler(this.pawn);
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000CA544 File Offset: 0x000C8744
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.IsRising)
			{
				if ((float)Find.TickManager.TicksGame > this.resurrectTimer)
				{
					this.FinishRising(true);
				}
				if (this.pawn.Spawned)
				{
					if ((float)Find.TickManager.TicksGame > this.resurrectTimer - 15f)
					{
						Sustainer sustainer = this.riseSustainer;
						if (sustainer == null)
						{
							return;
						}
						sustainer.End();
						return;
					}
					else if (this.IsRising)
					{
						if (this.riseSustainer == null || this.riseSustainer.Ended)
						{
							SoundInfo info = SoundInfo.InMap(this.pawn, MaintenanceType.PerTick);
							this.riseSustainer = SoundDefOf.Pawn_Shambler_Rise.TrySpawnSustainer(info);
						}
						if (this.riseEffecter == null)
						{
							this.riseEffecter = EffecterDefOf.ShamblerRaise.Spawn(this.pawn, this.pawn.Map, 1f);
						}
						if (this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.ShamblerRise)
						{
							this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.ShamblerRise);
						}
						this.riseSustainer.Maintain();
						this.riseEffecter.EffectTick(this.pawn, TargetInfo.Invalid);
					}
				}
				return;
			}
			if (Rand.MTBEventOccurs(1f, 60f, 1f))
			{
				FleckMaker.ThrowShamblerParticles(this.pawn);
			}
			if (this.pawn.IsBurning() && !this.pawn.Downed && Rand.MTBEventOccurs(45f, 60f, 1f))
			{
				((Fire)this.pawn.GetAttachment(ThingDefOf.Fire)).Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			}
			if (this.pawn.Spawned && !this.pawn.mutant.IsPassive && !this.pawn.Drafted)
			{
				if (Find.TickManager.TicksGame > this.nextTargetCheckTick)
				{
					this.nextTargetCheckTick = Find.TickManager.TicksGame + Hediff_Shambler.CheckForTargetTicksInterval.RandomInRange;
					Thing thing = MutantUtility.FindShamblerTarget(this.pawn);
					if (thing != null)
					{
						this.Notify_DelayedAlert(thing);
						MutantUtility.ActivateNearbyShamblers(this.pawn, thing);
					}
				}
				if (this.alertedTarget != null && (float)Find.TickManager.TicksGame > this.alertTimer)
				{
					this.pawn.mindState.enemyTarget = this.alertedTarget;
					this.pawn.mindState.Notify_EngagedTarget();
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					this.alertedTarget = null;
					if (DebugViewSettings.drawShamblerAlertMote)
					{
						MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ShamblerAlert);
					}
					SoundDefOf.Pawn_Shambler_Alert.PlayOneShot(this.pawn);
				}
			}
			if (this.ShouldSelfRaise())
			{
				this.StartRising(-1);
			}
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000CA828 File Offset: 0x000C8A28
		private bool ShouldSelfRaise()
		{
			return this.pawn.DevelopmentalStage != DevelopmentalStage.Baby && (this.pawn.Downed && this.pawn.CarriedBy == null) && (float)Find.TickManager.TicksGame > this.selfRaiseTimer;
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000CA874 File Offset: 0x000C8A74
		public void StartRising(int lifespanTicks = -1)
		{
			if (!this.pawn.Dead && !this.pawn.Downed)
			{
				Log.Error("Tried to raise non dead/downed pawn as shambler");
				this.pawn.mutant.Turn(true);
				return;
			}
			MutantUtility.RestoreBodyParts(this.pawn);
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.Dead && !ResurrectionUtility.TryResurrect(this.pawn, new ResurrectionParams
			{
				noLord = true,
				restoreMissingParts = false,
				removeDiedThoughts = false
			}))
			{
				return;
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (jobs != null)
			{
				jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.resurrectTimer = (float)(Find.TickManager.TicksGame + Hediff_Shambler.ResurrectionSecondsRange.RandomInRange.SecondsToTicks());
			this.pawn.health.AddHediff(HediffDefOf.Rising, null, null, null);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000CA964 File Offset: 0x000C8B64
		private void CancelRising()
		{
			Sustainer sustainer = this.riseSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.resurrectTimer = -99999f;
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
			if (!this.pawn.Dead)
			{
				this.pawn.Kill(null, null);
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000CA9E0 File Offset: 0x000C8BE0
		private void FinishRising(bool stun = true)
		{
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
			Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				pawn_CarryTracker.TryDropCarriedThing(pawn_CarryTracker.pawn.Position, ThingPlaceMode.Near, out thing, null);
				pawn_CarryTracker.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			if (!this.pawn.mutant.HasTurned)
			{
				this.pawn.mutant.Turn(false);
			}
			this.pawn.timesRaisedAsShambler++;
			MutantUtility.RestoreUntilNotDowned(this.pawn);
			if (this.pawn.Spawned && stun)
			{
				this.pawn.Rotation = Rot4.South;
				this.pawn.stances.stunner.StunFor(Hediff_Shambler.StunSecondsRange.RandomInRange.SecondsToTicks(), this.pawn, false, false, false);
			}
			this.pawn.Drawer.renderer.SetAnimation(null);
			this.StartSelfRaiseTimer();
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000CAB08 File Offset: 0x000C8D08
		private void StartSelfRaiseTimer()
		{
			this.selfRaiseTimer = (float)(Find.TickManager.TicksGame + 2500 * Hediff_Shambler.SelfRaiseHoursRange.RandomInRange);
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000CAB3C File Offset: 0x000C8D3C
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Instigator == null || !this.pawn.HostileTo(dinfo.Instigator))
			{
				return;
			}
			if (this.pawn.Spawned && dinfo.Instigator != null && dinfo.Instigator is IAttackTarget && dinfo.Instigator.Spawned && this.pawn.HostileTo(dinfo.Instigator) && this.pawn.CanSee(dinfo.Instigator, null))
			{
				bool flag = this.pawn.mindState.enemyTarget == dinfo.Instigator;
				this.pawn.mindState.enemyTarget = dinfo.Instigator;
				this.pawn.mindState.Notify_EngagedTarget();
				if (!flag)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptOptional, true, true);
				}
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnAcquiredTarget(this.pawn, dinfo.Instigator);
				}
			}
			MutantUtility.ActivateNearbyShamblers(this.pawn, dinfo.Instigator);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000CAC64 File Offset: 0x000C8E64
		public void Notify_DelayedAlert(Thing target)
		{
			if (this.pawn.mutant.IsPassive || this.pawn.Drafted)
			{
				return;
			}
			if (this.pawn.mindState.enemyTarget != null)
			{
				return;
			}
			this.alertTimer = (float)(Find.TickManager.TicksGame + Hediff_Shambler.AlertSecondsRange.RandomInRange.SecondsToTicks());
			this.alertedTarget = target;
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnAcquiredTarget(this.pawn, target);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000CACEB File Offset: 0x000C8EEB
		public override void Notify_Downed()
		{
			this.StartSelfRaiseTimer();
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000CACF3 File Offset: 0x000C8EF3
		public override void Notify_PawnKilled()
		{
			this.corpseDamagePct = this.pawn.health.summaryHealth.SummaryHealthPercent;
			base.Notify_PawnKilled();
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000CAD18 File Offset: 0x000C8F18
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.IsRising)
			{
				this.CancelRising();
			}
			if (this.pawn.timesRaisedAsShambler == 1 && Rand.Chance(0.04f) && this.pawn.SpawnedOrAnyParentSpawned)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite, null);
				thing.stackCount = 10;
				Thing t;
				GenPlace.TryPlaceThing(thing, this.pawn.PositionHeld, this.pawn.MapHeld, ThingPlaceMode.Near, out t, null, null, null, 1);
				t.SetForbidden(true, true);
			}
			if (this.pawn.Corpse != null)
			{
				this.pawn.Corpse.HitPoints = Mathf.Max(Mathf.RoundToInt((float)this.pawn.Corpse.MaxHitPoints * this.corpseDamagePct), 10);
			}
			this.pawn.health.AddHediff(HediffDefOf.ShamblerCorpse, null, null, null);
			base.Notify_PawnDied(dinfo, culprit);
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000CAE0A File Offset: 0x000C900A
		public override void PreRemoved()
		{
			Map mapHeld = this.pawn.MapHeld;
			if (mapHeld != null)
			{
				mapHeld.mapPawns.DeregisterShambler(this.pawn);
			}
			base.PreRemoved();
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000CAE34 File Offset: 0x000C9034
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.IsRising)
			{
				this.CancelRising();
			}
			if (this.pawn.IsMutant)
			{
				if (this.pawn.mutant.HasTurned)
				{
					this.pawn.mutant.Revert(false);
					return;
				}
				this.pawn.mutant = null;
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000CAEA0 File Offset: 0x000C90A0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_Shambler.<GetGizmos>d__36 <GetGizmos>d__ = new Hediff_Shambler.<GetGizmos>d__36(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000CAEB0 File Offset: 0x000C90B0
		public override string GetInspectString()
		{
			if (this.ShouldSelfRaise())
			{
				return "ShamblerRegenerating".Translate();
			}
			if (this.IsRising)
			{
				return "ShamblerRising".Translate();
			}
			if (this.pawn.CurJobDef == JobDefOf.Wait_Wander || this.pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
			{
				return "ShamblerStanding".Translate();
			}
			if (this.pawn.CurJobDef == JobDefOf.GotoWander || this.pawn.CurJobDef == JobDefOf.Goto)
			{
				return "ShamblerShuffling".Translate();
			}
			return "";
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000CAF5C File Offset: 0x000C915C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.alertTimer, "alertTimer", 0f, false);
			Scribe_Values.Look<int>(ref this.nextTargetCheckTick, "nextTargetCheckTick", 0, false);
			Scribe_References.Look<Thing>(ref this.alertedTarget, "alertedTarget", false);
			Scribe_Values.Look<float>(ref this.resurrectTimer, "resurrectTimer", 0f, false);
			Scribe_Values.Look<float>(ref this.selfRaiseTimer, "selfRaiseTimer", 0f, false);
			Scribe_Values.Look<float>(ref this.headRotation, "headRotation", 0f, false);
		}

		// Token: 0x040019A7 RID: 6567
		private static readonly FloatRange ResurrectionSecondsRange = new FloatRange(5f, 10f);

		// Token: 0x040019A8 RID: 6568
		private static readonly FloatRange StunSecondsRange = new FloatRange(1f, 3f);

		// Token: 0x040019A9 RID: 6569
		private static readonly FloatRange AlertSecondsRange = new FloatRange(0.5f, 3f);

		// Token: 0x040019AA RID: 6570
		private static readonly IntRange SelfRaiseHoursRange = new IntRange(3, 4);

		// Token: 0x040019AB RID: 6571
		private const float ParticleSpawnMTBSeconds = 1f;

		// Token: 0x040019AC RID: 6572
		private static readonly IntRange CheckForTargetTicksInterval = new IntRange(900, 1800);

		// Token: 0x040019AD RID: 6573
		private const float ExtinguishFireMTB = 45f;

		// Token: 0x040019AE RID: 6574
		private const float BioferriteOnDeathChance = 0.04f;

		// Token: 0x040019AF RID: 6575
		private const int BioferriteAmountOnDeath = 10;

		// Token: 0x040019B0 RID: 6576
		public float headRotation;

		// Token: 0x040019B1 RID: 6577
		private float resurrectTimer;

		// Token: 0x040019B2 RID: 6578
		private float selfRaiseTimer;

		// Token: 0x040019B3 RID: 6579
		private float alertTimer;

		// Token: 0x040019B4 RID: 6580
		private int nextTargetCheckTick = -99999;

		// Token: 0x040019B5 RID: 6581
		private Thing alertedTarget;

		// Token: 0x040019B6 RID: 6582
		private Effecter riseEffecter;

		// Token: 0x040019B7 RID: 6583
		private Sustainer riseSustainer;

		// Token: 0x040019B8 RID: 6584
		private float corpseDamagePct = 1f;
	}
}
