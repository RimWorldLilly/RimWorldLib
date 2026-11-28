using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A9 RID: 1193
	public class Hediff_DeathRefusal : HediffWithComps
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002108 RID: 8456 RVA: 0x000C5393 File Offset: 0x000C3593
		public bool PlayerControlled
		{
			get
			{
				return this.pawn.IsColonist && (this.pawn.HostFaction == null || this.pawn.IsSlave);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x000C53BE File Offset: 0x000C35BE
		public bool InProgress
		{
			get
			{
				return !this.resurrectTimer.Finished;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x000C53CE File Offset: 0x000C35CE
		public int UsesLeft
		{
			get
			{
				return this.usesLeft;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600210B RID: 8459 RVA: 0x000C53D6 File Offset: 0x000C35D6
		// (set) Token: 0x0600210C RID: 8460 RVA: 0x000C53DE File Offset: 0x000C35DE
		public bool AIEnabled
		{
			get
			{
				return this.aiEnabled;
			}
			set
			{
				this.aiEnabled = value;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x000C53E7 File Offset: 0x000C35E7
		public virtual int MaxUses
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x000C53EC File Offset: 0x000C35EC
		public override string LabelInBrackets
		{
			get
			{
				return this.UsesLeft.ToString() + " " + ((this.UsesLeft > 1) ? "DeathRefusalUsePlural".Translate() : "DeathRefusalUseSingular".Translate()).ToString();
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000C543E File Offset: 0x000C363E
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Death refusal"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			this.usesLeft = 1;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000C546C File Offset: 0x000C366C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_DeathRefusal.<GetGizmos>d__27 <GetGizmos>d__ = new Hediff_DeathRefusal.<GetGizmos>d__27(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000C547C File Offset: 0x000C367C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.usesLeft, "usesLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.resurrecting, "resurrecting", false, false);
			Scribe_Values.Look<bool>(ref this.aiEnabled, "aiEnabled", false, false);
			Scribe_Deep.Look<TickTimer>(ref this.resurrectTimer, "resurrectTimer", Array.Empty<object>());
			Scribe_Deep.Look<TickTimer>(ref this.warmupTimer, "warmupTimer", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.resurrectTimer.OnFinish = new Action(this.Resurrect);
				this.warmupTimer.OnFinish = new Action(this.Use);
				if (!this.resurrecting && this.pawn.Dead)
				{
					this.TryTriggerAIWarmupResurrection();
				}
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000C5540 File Offset: 0x000C3740
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			if (this.PlayerControlled && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("SelfResurrectText".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			this.TryTriggerAIWarmupResurrection();
			this.TryTriggerReadyEffect();
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000C55AC File Offset: 0x000C37AC
		private void TryTriggerAIWarmupResurrection()
		{
			if (this.PlayerControlled || this.resurrecting || !this.AIEnabled || this.usesLeft <= 0)
			{
				return;
			}
			this.resurrecting = true;
			this.warmupTimer.Start(GenTicks.TicksGame, Hediff_DeathRefusal.AIWarmupSeconds.RandomInRange.SecondsToTicks(), new Action(this.Use));
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000C5610 File Offset: 0x000C3810
		private void TryTriggerReadyEffect()
		{
			if (this.resurrecting || this.usesLeft <= 0)
			{
				return;
			}
			if (this.resurrectAvailableEffecter == null)
			{
				Corpse corpse = this.pawn.ParentHolder as Corpse;
				if (corpse != null)
				{
					this.resurrectAvailableEffecter = EffecterDefOf.DeathRefusalAvailable.Spawn(corpse, corpse.MapHeld, Vector3.zero);
					this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.resurrectAvailableEffecter, corpse, 250);
				}
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000C5688 File Offset: 0x000C3888
		public override void TickInterval(int delta)
		{
			if (!this.resurrectTimer.Finished)
			{
				Corpse corpse = this.pawn.ParentHolder as Corpse;
				if (corpse != null)
				{
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.CellPollution.Spawn(corpse, this.pawn.MapHeld, Vector3.zero);
						this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.effecter, corpse, 250);
					}
					if (this.resurrectSustainer == null)
					{
						SoundInfo info = SoundInfo.InMap(corpse, MaintenanceType.PerTickRare);
						this.resurrectSustainer = SoundDefOf.Pawn_SelfResurrection.TrySpawnSustainer(info);
					}
				}
			}
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000C572C File Offset: 0x000C392C
		public void TickRare()
		{
			if (!this.warmupTimer.Finished)
			{
				this.warmupTimer.TickIntervalDelta();
			}
			this.TryTriggerReadyEffect();
			if (!this.resurrectTimer.Finished)
			{
				this.resurrectTimer.TickIntervalDelta();
				if (this.resurrectSustainer != null && !this.resurrectSustainer.Ended)
				{
					Sustainer sustainer = this.resurrectSustainer;
					if (sustainer != null)
					{
						sustainer.Maintain();
					}
				}
			}
			if (this.effecter != null)
			{
				this.effecter.ticksLeft = (this.resurrectTimer.Finished ? 0 : (this.effecter.ticksLeft + 250));
			}
			if (this.resurrectAvailableEffecter != null)
			{
				this.resurrectAvailableEffecter.ticksLeft += 250;
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000C57E8 File Offset: 0x000C39E8
		private void Resurrect()
		{
			Sustainer sustainer = this.resurrectSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.resurrecting = false;
			this.pawn.Drawer.renderer.SetAnimation(null);
			ResurrectionUtility.TryResurrect(this.pawn, new ResurrectionParams
			{
				gettingScarsChance = 0.1f,
				canKidnap = false,
				canTimeoutOrFlee = false,
				useAvoidGridSmart = true,
				canSteal = false,
				invisibleStun = true
			});
			if (this.usesLeft == 0)
			{
				this.Severity = 0f;
			}
			if (this.pawn.Faction != Faction.OfPlayer && !this.pawn.Downed)
			{
				Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 5f, null, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					Job job = JobGiver_PickupDroppedWeapon.PickupWeaponJob(this.pawn, thing, true);
					if (job != null)
					{
						this.pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					}
				}
			}
			this.pawn.health.AddHediff(HediffDefOf.DeathRefusalSickness, null, null, null);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000C5941 File Offset: 0x000C3B41
		public void SetUseAmountDirect(int amount, bool ignoreLimit = false)
		{
			if (ignoreLimit)
			{
				this.usesLeft = amount;
				return;
			}
			this.usesLeft = Mathf.Clamp(amount, 0, this.MaxUses);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000C5964 File Offset: 0x000C3B64
		private void Use()
		{
			Messages.Message("MessageUsingSelfResurrection".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			this.resurrecting = true;
			this.usesLeft = Mathf.Max(this.usesLeft - 1, 0);
			this.resurrectTimer.Start(GenTicks.TicksGame, Hediff_DeathRefusal.ResurrectDurationSeconds.SecondsToTicks(), new Action(this.Resurrect));
			Effecter effecter = this.resurrectAvailableEffecter;
			if (effecter != null)
			{
				effecter.ForceEnd();
			}
			this.resurrectAvailableEffecter = null;
			Corpse corpse = this.pawn.ParentHolder as Corpse;
			if (corpse != null)
			{
				this.resurrectUsedEffecter = EffecterDefOf.DeathRefusalUse.Spawn(corpse, corpse.MapHeld, Vector3.zero);
				this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.resurrectUsedEffecter, corpse, 1000);
				this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DeathRefusalTwitches);
			}
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000C5A65 File Offset: 0x000C3C65
		public override void Notify_PawnCorpseDestroyed()
		{
			Sustainer sustainer = this.resurrectSustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.End();
		}

		// Token: 0x04001948 RID: 6472
		protected int usesLeft;

		// Token: 0x04001949 RID: 6473
		private TickTimer resurrectTimer = new TickTimer();

		// Token: 0x0400194A RID: 6474
		private TickTimer warmupTimer = new TickTimer();

		// Token: 0x0400194B RID: 6475
		private Sustainer resurrectSustainer;

		// Token: 0x0400194C RID: 6476
		private bool resurrecting;

		// Token: 0x0400194D RID: 6477
		private bool aiEnabled = true;

		// Token: 0x0400194E RID: 6478
		private Effecter effecter;

		// Token: 0x0400194F RID: 6479
		private Effecter resurrectAvailableEffecter;

		// Token: 0x04001950 RID: 6480
		private Effecter resurrectUsedEffecter;

		// Token: 0x04001951 RID: 6481
		private static readonly CachedTexture Icon = new CachedTexture("UI/Abilities/SelfResurrect");

		// Token: 0x04001952 RID: 6482
		private const float ScarredChance = 0.1f;

		// Token: 0x04001953 RID: 6483
		private static readonly float ResurrectDurationSeconds = 3f;

		// Token: 0x04001954 RID: 6484
		private static readonly FloatRange AIWarmupSeconds = new FloatRange(1f, 2f);
	}
}
