using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020019CE RID: 6606
	public class CompTurretGun : ThingComp, IAttackTargetSearcher
	{
		// Token: 0x17001CFF RID: 7423
		// (get) Token: 0x0600A61B RID: 42523 RVA: 0x003B408B File Offset: 0x003B228B
		public Thing Thing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001D00 RID: 7424
		// (get) Token: 0x0600A61C RID: 42524 RVA: 0x003B4093 File Offset: 0x003B2293
		public CompProperties_TurretGun Props
		{
			get
			{
				return (CompProperties_TurretGun)this.props;
			}
		}

		// Token: 0x17001D01 RID: 7425
		// (get) Token: 0x0600A61D RID: 42525 RVA: 0x003B40A0 File Offset: 0x003B22A0
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17001D02 RID: 7426
		// (get) Token: 0x0600A61E RID: 42526 RVA: 0x003B40A8 File Offset: 0x003B22A8
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17001D03 RID: 7427
		// (get) Token: 0x0600A61F RID: 42527 RVA: 0x003B40B0 File Offset: 0x003B22B0
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x17001D04 RID: 7428
		// (get) Token: 0x0600A620 RID: 42528 RVA: 0x003B40B8 File Offset: 0x003B22B8
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x0600A621 RID: 42529 RVA: 0x003B40C5 File Offset: 0x003B22C5
		public Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x0600A622 RID: 42530 RVA: 0x003B40D2 File Offset: 0x003B22D2
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x0600A623 RID: 42531 RVA: 0x003B40E0 File Offset: 0x003B22E0
		private bool CanShoot
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (!pawn.Spawned || pawn.Downed || pawn.Dead || !pawn.Awake())
					{
						return false;
					}
					if (pawn.stances.stunner.Stunned)
					{
						return false;
					}
					if (this.TurretDestroyed)
					{
						return false;
					}
					if (pawn.IsColonyMechPlayerControlled && !this.fireAtWill)
					{
						return false;
					}
				}
				CompCanBeDormant compCanBeDormant = this.parent.TryGetComp<CompCanBeDormant>();
				return compCanBeDormant == null || compCanBeDormant.Awake;
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x0600A624 RID: 42532 RVA: 0x003B4168 File Offset: 0x003B2368
		public bool TurretDestroyed
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return pawn != null && this.AttackVerb.verbProps.linkedBodyPartsGroup != null && this.AttackVerb.verbProps.ensureLinkedBodyPartsGroupAlwaysUsable && PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, this.AttackVerb.verbProps.linkedBodyPartsGroup) <= 0f;
			}
		}

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x0600A625 RID: 42533 RVA: 0x003B41D2 File Offset: 0x003B23D2
		public bool AutoAttack
		{
			get
			{
				return this.Props.autoAttack;
			}
		}

		// Token: 0x0600A626 RID: 42534 RVA: 0x003B41DF File Offset: 0x003B23DF
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.MakeGun();
		}

		// Token: 0x0600A627 RID: 42535 RVA: 0x003B41ED File Offset: 0x003B23ED
		private void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.Props.turretDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x0600A628 RID: 42536 RVA: 0x003B420C File Offset: 0x003B240C
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this.parent;
				verb.castCompleteCallback = delegate()
				{
					this.burstCooldownTicksLeft = this.AttackVerb.verbProps.defaultCooldownTime.SecondsToTicks();
				};
			}
		}

		// Token: 0x0600A629 RID: 42537 RVA: 0x003B4260 File Offset: 0x003B2460
		public override void CompTick()
		{
			if (!this.CanShoot)
			{
				return;
			}
			if (this.currentTarget.IsValid)
			{
				this.curRotation = (this.currentTarget.Cell.ToVector3Shifted() - this.parent.DrawPos).AngleFlat() + this.Props.angleOffset;
			}
			this.AttackVerb.VerbTick();
			if (this.AttackVerb.state != VerbState.Bursting)
			{
				if (this.WarmingUp)
				{
					this.burstWarmupTicksLeft--;
					if (this.burstWarmupTicksLeft == 0)
					{
						this.AttackVerb.TryStartCastOn(this.currentTarget, false, true, false, true);
						this.lastAttackTargetTick = Find.TickManager.TicksGame;
						this.lastAttackedTarget = this.currentTarget;
						return;
					}
				}
				else
				{
					if (this.burstCooldownTicksLeft > 0)
					{
						this.burstCooldownTicksLeft--;
					}
					if (this.burstCooldownTicksLeft <= 0 && this.parent.IsHashIntervalTick(10))
					{
						this.currentTarget = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, null, 0f, 9999f);
						if (this.currentTarget.IsValid)
						{
							this.burstWarmupTicksLeft = 1;
							return;
						}
						this.ResetCurrentTarget();
					}
				}
			}
		}

		// Token: 0x0600A62A RID: 42538 RVA: 0x003B439D File Offset: 0x003B259D
		private void ResetCurrentTarget()
		{
			this.currentTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x0600A62B RID: 42539 RVA: 0x003B43B1 File Offset: 0x003B25B1
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTurretGun.<CompGetGizmosExtra>d__37 <CompGetGizmosExtra>d__ = new CompTurretGun.<CompGetGizmosExtra>d__37(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A62C RID: 42540 RVA: 0x003B43C4 File Offset: 0x003B25C4
		public override List<PawnRenderNode> CompRenderNodes()
		{
			if (!this.Props.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>())
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					List<PawnRenderNode> list = new List<PawnRenderNode>();
					foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.Props.renderNodeProperties)
					{
						PawnRenderNode_TurretGun pawnRenderNode_TurretGun = (PawnRenderNode_TurretGun)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
						{
							pawn,
							pawnRenderNodeProperties,
							pawn.Drawer.renderer.renderTree
						});
						pawnRenderNode_TurretGun.turretComp = this;
						list.Add(pawnRenderNode_TurretGun);
					}
					return list;
				}
			}
			return base.CompRenderNodes();
		}

		// Token: 0x0600A62D RID: 42541 RVA: 0x003B448C File Offset: 0x003B268C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompTurretGun.<SpecialDisplayStats>d__39 <SpecialDisplayStats>d__ = new CompTurretGun.<SpecialDisplayStats>d__39(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A62E RID: 42542 RVA: 0x003B449C File Offset: 0x003B269C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.fireAtWill, "fireAtWill", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.gun == null)
				{
					Log.Error("CompTurrentGun had null gun after loading. Recreating.");
					this.MakeGun();
					return;
				}
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x04006207 RID: 25095
		private const int StartShootIntervalTicks = 10;

		// Token: 0x04006208 RID: 25096
		private static readonly CachedTexture ToggleTurretIcon = new CachedTexture("UI/Gizmos/ToggleTurret");

		// Token: 0x04006209 RID: 25097
		public Thing gun;

		// Token: 0x0400620A RID: 25098
		protected int burstCooldownTicksLeft;

		// Token: 0x0400620B RID: 25099
		protected int burstWarmupTicksLeft;

		// Token: 0x0400620C RID: 25100
		protected LocalTargetInfo currentTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400620D RID: 25101
		private bool fireAtWill = true;

		// Token: 0x0400620E RID: 25102
		private LocalTargetInfo lastAttackedTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400620F RID: 25103
		private int lastAttackTargetTick;

		// Token: 0x04006210 RID: 25104
		public float curRotation;
	}
}
