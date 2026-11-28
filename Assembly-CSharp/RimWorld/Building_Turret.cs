using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A70 RID: 6768
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		// Token: 0x17001DEC RID: 7660
		// (get) Token: 0x0600AA1B RID: 43547
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17001DED RID: 7661
		// (get) Token: 0x0600AA1C RID: 43548
		public abstract Verb AttackVerb { get; }

		// Token: 0x17001DEE RID: 7662
		// (get) Token: 0x0600AA1D RID: 43549 RVA: 0x003C4442 File Offset: 0x003C2642
		public virtual Material TurretTopMaterial
		{
			get
			{
				return this.def.building.turretTopMat;
			}
		}

		// Token: 0x17001DEF RID: 7663
		// (get) Token: 0x0600AA1E RID: 43550 RVA: 0x003C4454 File Offset: 0x003C2654
		protected bool IsStunned
		{
			get
			{
				if (!this.triedGettingStunner)
				{
					CompStunnable comp = base.GetComp<CompStunnable>();
					this.stunner = ((comp != null) ? comp.StunHandler : null);
					this.triedGettingStunner = true;
				}
				return this.stunner != null && this.stunner.Stunned;
			}
		}

		// Token: 0x17001DF0 RID: 7664
		// (get) Token: 0x0600AA1F RID: 43551 RVA: 0x003C4492 File Offset: 0x003C2692
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001DF1 RID: 7665
		// (get) Token: 0x0600AA20 RID: 43552 RVA: 0x003C4495 File Offset: 0x003C2695
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x17001DF2 RID: 7666
		// (get) Token: 0x0600AA21 RID: 43553 RVA: 0x003C449D File Offset: 0x003C269D
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001DF3 RID: 7667
		// (get) Token: 0x0600AA22 RID: 43554 RVA: 0x003C44A0 File Offset: 0x003C26A0
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17001DF4 RID: 7668
		// (get) Token: 0x0600AA23 RID: 43555 RVA: 0x003C44A8 File Offset: 0x003C26A8
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17001DF5 RID: 7669
		// (get) Token: 0x0600AA24 RID: 43556 RVA: 0x003C44B0 File Offset: 0x003C26B0
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x17001DF6 RID: 7670
		// (get) Token: 0x0600AA25 RID: 43557 RVA: 0x003C44B8 File Offset: 0x003C26B8
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001DF7 RID: 7671
		// (get) Token: 0x0600AA26 RID: 43558 RVA: 0x003C44BF File Offset: 0x003C26BF
		public LocalTargetInfo ForcedTarget
		{
			get
			{
				return this.forcedTarget;
			}
		}

		// Token: 0x17001DF8 RID: 7672
		// (get) Token: 0x0600AA27 RID: 43559 RVA: 0x003C44C7 File Offset: 0x003C26C7
		public virtual bool IsEverThreat
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AA28 RID: 43560 RVA: 0x003C44CC File Offset: 0x003C26CC
		protected override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x0600AA29 RID: 43561 RVA: 0x003C4529 File Offset: 0x003C2729
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_TargetInfo.Look(ref this.forcedTarget, "forcedTarget");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
		}

		// Token: 0x0600AA2A RID: 43562 RVA: 0x003C4563 File Offset: 0x003C2763
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			absorbed = false;
		}

		// Token: 0x0600AA2B RID: 43563
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x0600AA2C RID: 43564 RVA: 0x003C4578 File Offset: 0x003C2778
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!this.IsEverThreat)
			{
				return true;
			}
			CompPowerTrader comp = base.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return true;
			}
			CompMannable comp2 = base.GetComp<CompMannable>();
			if (comp2 != null && !comp2.MannedNow)
			{
				return true;
			}
			CompCanBeDormant comp3 = base.GetComp<CompCanBeDormant>();
			if (comp3 != null && !comp3.Awake)
			{
				return true;
			}
			CompInitiatable comp4 = base.GetComp<CompInitiatable>();
			if (comp4 != null && !comp4.Initiated)
			{
				return true;
			}
			CompMechPowerCell comp5 = base.GetComp<CompMechPowerCell>();
			if (comp5 != null && comp5.depleted)
			{
				return true;
			}
			CompHackable comp6 = base.GetComp<CompHackable>();
			return comp6 != null && comp6.IsHacked;
		}

		// Token: 0x0600AA2D RID: 43565 RVA: 0x003C460E File Offset: 0x003C280E
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x04006378 RID: 25464
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04006379 RID: 25465
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x0400637A RID: 25466
		private int lastAttackTargetTick;

		// Token: 0x0400637B RID: 25467
		private StunHandler stunner;

		// Token: 0x0400637C RID: 25468
		private bool triedGettingStunner;

		// Token: 0x0400637D RID: 25469
		private const float SightRadiusTurret = 13.4f;
	}
}
