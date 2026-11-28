using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001940 RID: 6464
	[StaticConstructorOnStartup]
	public class CompProjectileInterceptor : ThingComp
	{
		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x0600A20E RID: 41486 RVA: 0x0039E891 File Offset: 0x0039CA91
		private static Material ShieldDotMat
		{
			get
			{
				return MaterialPool.MatFrom("Things/Mote/ShieldDownDot", ShaderDatabase.MoteGlow);
			}
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x0600A20F RID: 41487 RVA: 0x0039E8A2 File Offset: 0x0039CAA2
		protected virtual int NumInactiveDots
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x0600A210 RID: 41488 RVA: 0x0039E8A5 File Offset: 0x0039CAA5
		public CompProperties_ProjectileInterceptor Props
		{
			get
			{
				return (CompProperties_ProjectileInterceptor)this.props;
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x0600A211 RID: 41489 RVA: 0x0039E8B4 File Offset: 0x0039CAB4
		public bool Active
		{
			get
			{
				if (this.OnCooldown || this.Charging || this.stunner.Stunned || this.shutDown || this.currentHitPoints == 0 || !this.parent.Spawned)
				{
					return false;
				}
				if (this.Props.activated && Find.TickManager.TicksGame > this.activatedTick + this.Props.activeDuration)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && (pawn.IsCharging() || pawn.IsSelfShutdown()))
				{
					return false;
				}
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x0600A212 RID: 41490 RVA: 0x0039E965 File Offset: 0x0039CB65
		public bool OnCooldown
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastInterceptTicks + this.Props.cooldownTicks;
			}
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x0600A213 RID: 41491 RVA: 0x0039E985 File Offset: 0x0039CB85
		public bool Charging
		{
			get
			{
				return this.startedChargingTick >= 0 && Find.TickManager.TicksGame > this.startedChargingTick;
			}
		}

		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x0600A214 RID: 41492 RVA: 0x0039E9A4 File Offset: 0x0039CBA4
		public int ChargeCycleStartTick
		{
			get
			{
				if (this.startedChargingTick < 0)
				{
					return 0;
				}
				return this.startedChargingTick;
			}
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x0600A215 RID: 41493 RVA: 0x0039E9B7 File Offset: 0x0039CBB7
		public int ChargingTicksLeft
		{
			get
			{
				if (this.startedChargingTick < 0)
				{
					return 0;
				}
				return Mathf.Max(this.startedChargingTick + this.Props.chargeDurationTicks - Find.TickManager.TicksGame, 0);
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x0600A216 RID: 41494 RVA: 0x0039E9E7 File Offset: 0x0039CBE7
		public int CooldownTicksLeft
		{
			get
			{
				if (!this.OnCooldown)
				{
					return 0;
				}
				return this.Props.cooldownTicks - (Find.TickManager.TicksGame - this.lastInterceptTicks);
			}
		}

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x0600A217 RID: 41495 RVA: 0x0039EA10 File Offset: 0x0039CC10
		public bool ReactivatedThisTick
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastInterceptTicks == this.Props.cooldownTicks;
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x0600A218 RID: 41496 RVA: 0x0039EA30 File Offset: 0x0039CC30
		public bool ShouldDisplayHitpointsGizmo
		{
			get
			{
				if (this.Props.alwaysShowHitpointsGizmo)
				{
					return true;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn != null && (pawn.IsColonistPlayerControlled || pawn.RaceProps.IsMechanoid) && this.HitPointsMax > 0;
			}
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x0600A219 RID: 41497 RVA: 0x0039EA7C File Offset: 0x0039CC7C
		public int HitPointsMax
		{
			get
			{
				int? num = this.maxHitPointsOverride;
				if (num == null)
				{
					return this.Props.hitPoints;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x0600A21A RID: 41498 RVA: 0x0039EAAC File Offset: 0x0039CCAC
		private int RemainingTicks
		{
			get
			{
				return this.activatedTick + this.Props.activeDuration - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x0600A21B RID: 41499 RVA: 0x0039EACB File Offset: 0x0039CCCB
		protected virtual int HitPointsPerInterval
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600A21C RID: 41500 RVA: 0x0039EAD0 File Offset: 0x0039CCD0
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.chargeIntervalTicks > 0)
			{
				this.startedChargingTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
			}
			if (this.HitPointsMax > 0)
			{
				if (this.Props.startWithMaxHitPoints)
				{
					this.currentHitPoints = this.HitPointsMax;
				}
				else
				{
					this.currentHitPoints = 0;
				}
			}
			this.stunner = new StunHandler(this.parent);
		}

		// Token: 0x0600A21D RID: 41501 RVA: 0x0039EB4F File Offset: 0x0039CD4F
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.End();
		}

		// Token: 0x0600A21E RID: 41502 RVA: 0x0039EB64 File Offset: 0x0039CD64
		public bool CheckIntercept(Projectile projectile, Vector3 lastExactPos, Vector3 newExactPos)
		{
			Vector3 vector = this.parent.Position.ToVector3Shifted();
			float num = this.Props.radius + projectile.def.projectile.SpeedTilesPerTick + 0.1f;
			if ((newExactPos.x - vector.x) * (newExactPos.x - vector.x) + (newExactPos.z - vector.z) * (newExactPos.z - vector.z) > num * num)
			{
				return false;
			}
			if (!this.Active)
			{
				return false;
			}
			if (!CompProjectileInterceptor.InterceptsProjectile(this.Props, projectile))
			{
				return false;
			}
			if (projectile.Launcher == null && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			if (this.parent.Faction != null)
			{
				if (projectile.Launcher != null && projectile.Launcher.Spawned && !projectile.Launcher.HostileTo(this.parent.Faction))
				{
					return false;
				}
				if (projectile.Launcher != null && !projectile.Launcher.Spawned && !projectile.Launcher.Faction.HostileTo(this.parent.Faction))
				{
					return false;
				}
			}
			if (!this.Props.interceptOutgoingProjectiles && (new Vector2(vector.x, vector.z) - new Vector2(lastExactPos.x, lastExactPos.z)).sqrMagnitude <= this.Props.radius * this.Props.radius)
			{
				return false;
			}
			if (!GenGeo.IntersectLineCircleOutline(new Vector2(vector.x, vector.z), this.Props.radius, new Vector2(lastExactPos.x, lastExactPos.z), new Vector2(newExactPos.x, newExactPos.z)))
			{
				return false;
			}
			this.lastInterceptAngle = lastExactPos.AngleToFlat(this.parent.TrueCenter());
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = true;
			this.TriggerEffecter(newExactPos.ToIntVec3());
			bool flag = projectile.DamageDef == DamageDefOf.EMP;
			using (IEnumerator<ExtraDamage> enumerator = projectile.ExtraDamages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == DamageDefOf.EMP)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShieldEmp(new DamageInfo(DamageDefOf.EMP, (float)DamageDefOf.EMP.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (this.currentHitPoints > 0)
			{
				this.currentHitPoints -= projectile.DamageAmount;
				if (this.currentHitPoints < 0)
				{
					this.currentHitPoints = 0;
				}
				if (this.currentHitPoints == 0)
				{
					this.startedChargingTick = Find.TickManager.TicksGame;
					this.BreakShieldHitpoints(new DamageInfo(projectile.DamageDef, (float)projectile.DamageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600A21F RID: 41503 RVA: 0x0039EE80 File Offset: 0x0039D080
		public bool CheckBombardmentIntercept(Bombardment bombardment, Bombardment.BombardmentProjectile projectile)
		{
			if (!this.Active || !this.Props.interceptAirProjectiles)
			{
				return false;
			}
			if (!projectile.targetCell.InHorDistOf(this.parent.Position, this.Props.radius))
			{
				return false;
			}
			if ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = false;
			this.TriggerEffecter(projectile.targetCell);
			return true;
		}

		// Token: 0x0600A220 RID: 41504 RVA: 0x0039EF20 File Offset: 0x0039D120
		public bool BombardmentCanStartFireAt(Bombardment bombardment, IntVec3 cell)
		{
			return !this.Active || !this.Props.interceptAirProjectiles || ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles) || !cell.InHorDistOf(this.parent.Position, this.Props.radius);
		}

		// Token: 0x0600A221 RID: 41505 RVA: 0x0039EF96 File Offset: 0x0039D196
		private void TriggerEffecter(IntVec3 pos)
		{
			Effecter effecter = new Effecter(this.Props.interceptEffect ?? EffecterDefOf.Interceptor_BlockedProjectile);
			effecter.Trigger(new TargetInfo(pos, this.parent.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
		}

		// Token: 0x0600A222 RID: 41506 RVA: 0x0039EFD8 File Offset: 0x0039D1D8
		public static bool InterceptsProjectile(CompProperties_ProjectileInterceptor props, Projectile projectile)
		{
			if (props.interceptGroundProjectiles && props.interceptAirProjectiles)
			{
				return true;
			}
			if (props.interceptGroundProjectiles)
			{
				return !projectile.def.projectile.flyOverhead;
			}
			return props.interceptAirProjectiles && projectile.def.projectile.flyOverhead;
		}

		// Token: 0x0600A223 RID: 41507 RVA: 0x0039F030 File Offset: 0x0039D230
		public override void CompTick()
		{
			if (this.HitPointsMax > 0 && this.ChargingTicksLeft == 0)
			{
				if (this.currentHitPoints == 0 && this.Props.hitPointsRestoreInstantlyAfterCharge)
				{
					this.currentHitPoints = this.HitPointsMax;
				}
				if (this.parent.IsHashIntervalTick(this.Props.rechargeHitPointsIntervalTicks))
				{
					this.currentHitPoints = Mathf.Clamp(this.currentHitPoints + this.HitPointsPerInterval, 0, this.HitPointsMax);
				}
				if (this.startedChargingTick > 0)
				{
					this.startedChargingTick = -1;
				}
			}
			if (this.Props.activated && !this.Charging && Find.TickManager.TicksGame == this.activatedTick + this.Props.activeDuration)
			{
				this.startedChargingTick = Find.TickManager.TicksGame;
			}
			if (this.ReactivatedThisTick && this.Props.reactivateEffect != null)
			{
				this.Props.reactivateEffect.Spawn(this.parent, this.parent.MapHeld, 1f).Cleanup();
			}
			if (this.Props.chargeIntervalTicks > 0 && Find.TickManager.TicksGame >= this.startedChargingTick + this.Props.chargeDurationTicks)
			{
				this.startedChargingTick += this.Props.chargeIntervalTicks;
			}
			this.stunner.StunHandlerTick();
			if (!this.Props.activeSound.NullOrUndefined())
			{
				if (this.Active)
				{
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainer.Maintain();
					return;
				}
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
			}
		}

		// Token: 0x0600A224 RID: 41508 RVA: 0x0039F20C File Offset: 0x0039D40C
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (!this.Active && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				for (int i = 0; i < this.NumInactiveDots; i++)
				{
					Vector3 b = new Vector3(0f, 0f, 1f).RotatedBy((float)i / (float)this.NumInactiveDots * 360f) * (this.Props.radius * 0.966f);
					Vector3 vector = this.parent.DrawPos + b;
					Graphics.DrawMesh(MeshPool.plane10, new Vector3(vector.x, AltitudeLayer.MoteOverhead.AltitudeFor(), vector.z), Quaternion.identity, CompProjectileInterceptor.ShieldDotMat, 0);
				}
			}
		}

		// Token: 0x0600A225 RID: 41509 RVA: 0x0039F2CA File Offset: 0x0039D4CA
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			this.shutDown = true;
		}

		// Token: 0x0600A226 RID: 41510 RVA: 0x0039F2DC File Offset: 0x0039D4DC
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float currentAlpha = this.GetCurrentAlpha();
			if (currentAlpha > 0f)
			{
				Color value;
				if (this.Active || !Find.Selector.IsSelected(this.parent))
				{
					value = this.Props.color;
				}
				else
				{
					value = CompProjectileInterceptor.InactiveColor;
				}
				value.a *= currentAlpha;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(drawPos, Quaternion.identity, new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompProjectileInterceptor.ForceFieldMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
			float currentConeAlpha_RecentlyIntercepted = this.GetCurrentConeAlpha_RecentlyIntercepted();
			if (currentConeAlpha_RecentlyIntercepted > 0f)
			{
				Color color = this.Props.color;
				color.a *= currentConeAlpha_RecentlyIntercepted;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(drawPos, Quaternion.Euler(0f, this.lastInterceptAngle - 90f, 0f), new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompProjectileInterceptor.ForceFieldConeMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
		}

		// Token: 0x0600A227 RID: 41511 RVA: 0x0039F483 File Offset: 0x0039D683
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), this.Props.minAlpha);
		}

		// Token: 0x0600A228 RID: 41512 RVA: 0x0039F4BC File Offset: 0x0039D6BC
		private float GetCurrentAlpha_Idle()
		{
			float idlePulseSpeed = this.Props.idlePulseSpeed;
			float minIdleAlpha = this.Props.minIdleAlpha;
			if (!this.Active)
			{
				return 0f;
			}
			if (this.parent.Faction == Faction.OfPlayer && !this.debugInterceptNonHostileProjectiles)
			{
				return 0f;
			}
			if (Find.Selector.IsSelected(this.parent))
			{
				return 0f;
			}
			return Mathf.Lerp(minIdleAlpha, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * idlePulseSpeed) + 1f) / 2f);
		}

		// Token: 0x0600A229 RID: 41513 RVA: 0x0039F564 File Offset: 0x0039D764
		private float GetCurrentAlpha_Selected()
		{
			float num = Mathf.Max(2f, this.Props.idlePulseSpeed);
			if ((!Find.Selector.IsSelected(this.parent) && !this.Props.drawWithNoSelection) || !this.Active)
			{
				return 0f;
			}
			return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * num) + 1f) / 2f);
		}

		// Token: 0x0600A22A RID: 41514 RVA: 0x0039F5F8 File Offset: 0x0039D7F8
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		// Token: 0x0600A22B RID: 41515 RVA: 0x0039F630 File Offset: 0x0039D830
		private float GetCurrentAlpha_RecentlyActivated()
		{
			if (!this.Active)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - (this.lastInterceptTicks + this.Props.cooldownTicks);
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		// Token: 0x0600A22C RID: 41516 RVA: 0x0039F684 File Offset: 0x0039D884
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			if (!this.drawInterceptCone)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		// Token: 0x0600A22D RID: 41517 RVA: 0x0039F6CA File Offset: 0x0039D8CA
		public void Activate()
		{
			this.activatedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A22E RID: 41518 RVA: 0x0039F6DC File Offset: 0x0039D8DC
		public void Deactivate()
		{
			this.activatedTick = Find.TickManager.TicksGame - this.Props.activeDuration;
			this.startedChargingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A22F RID: 41519 RVA: 0x0039F70A File Offset: 0x0039D90A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompProjectileInterceptor.<CompGetGizmosExtra>d__63 <CompGetGizmosExtra>d__ = new CompProjectileInterceptor.<CompGetGizmosExtra>d__63(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A230 RID: 41520 RVA: 0x0039F71C File Offset: 0x0039D91C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.interceptGroundProjectiles || this.Props.interceptAirProjectiles)
			{
				string value;
				if (this.Props.interceptGroundProjectiles && this.Props.interceptAirProjectiles)
				{
					value = "InterceptsProjectiles_BothProjectiles".Translate();
				}
				else if (this.Props.interceptGroundProjectiles)
				{
					value = "InterceptsProjectiles_GroundProjectiles".Translate();
				}
				else
				{
					value = "InterceptsProjectiles_AerialProjectiles".Translate();
				}
				if (this.Props.cooldownTicks > 0)
				{
					stringBuilder.AppendTagged("InterceptsProjectilesEvery".Translate(value, this.Props.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)));
				}
				else
				{
					stringBuilder.Append("InterceptsProjectiles".Translate(value));
				}
			}
			if (this.Props.activated && this.Active)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendTagged("ShieldDuration".Translate() + ": " + this.RemainingTicks.ToStringSecondsFromTicks("F0"));
			}
			if (this.OnCooldown)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendTagged("CooldownTime".Translate() + ": " + this.CooldownTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.stunner.Stunned)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendTagged("DisarmedTime".Translate() + ": " + this.stunner.StunTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.shutDown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ShutDown".Translate());
			}
			else if (this.Props.chargeIntervalTicks > 0)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.Charging)
				{
					stringBuilder.AppendTagged("ChargingTime".Translate() + ": " + this.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
				}
				else
				{
					stringBuilder.AppendTagged("ChargingNext".Translate((this.ChargeCycleStartTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false), this.Props.chargeDurationTicks.ToStringTicksToPeriod(true, false, true, true, false), this.Props.chargeIntervalTicks.ToStringTicksToPeriod(true, false, true, true, false)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A231 RID: 41521 RVA: 0x0039F9D7 File Offset: 0x0039DBD7
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShieldEmp(dinfo);
			}
		}

		// Token: 0x0600A232 RID: 41522 RVA: 0x0039FA08 File Offset: 0x0039DC08
		private void BreakShieldEmp(DamageInfo dinfo)
		{
			if (this.Active)
			{
				EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, this.Props.radius);
				int num = Mathf.CeilToInt(this.Props.radius * 2f);
				CompProjectileInterceptor.<>c__DisplayClass66_0 CS$<>8__locals1;
				CS$<>8__locals1.fTheta = 6.2831855f / (float)num;
				CS$<>8__locals1.center = this.parent.TrueCenter();
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ConnectingLine(this.<BreakShieldEmp>g__PosAtIndex|66_0(i, ref CS$<>8__locals1), this.<BreakShieldEmp>g__PosAtIndex|66_0((i + 1) % num, ref CS$<>8__locals1), FleckDefOf.LineEMP, this.parent.Map, 1.5f);
				}
			}
			dinfo.SetAmount((float)this.Props.disarmedByEmpForTicks / 30f);
			this.stunner.Notify_DamageApplied(dinfo);
		}

		// Token: 0x0600A233 RID: 41523 RVA: 0x0039FAE0 File Offset: 0x0039DCE0
		private void BreakShieldHitpoints(DamageInfo dinfo)
		{
			EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, this.Props.radius);
			this.stunner.Notify_DamageApplied(dinfo);
		}

		// Token: 0x0600A234 RID: 41524 RVA: 0x0039FB18 File Offset: 0x0039DD18
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInterceptTicks, "lastInterceptTicks", -999999, false);
			Scribe_Values.Look<bool>(ref this.shutDown, "shutDown", false, false);
			Scribe_Values.Look<int>(ref this.startedChargingTick, "nextChargeTick", -1, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.parent
			});
			Scribe_Values.Look<int>(ref this.currentHitPoints, "currentHitPoints", -1, false);
			Scribe_Values.Look<int?>(ref this.maxHitPointsOverride, "maxHitPointsOverride", null, false);
			Scribe_Values.Look<int>(ref this.activatedTick, "activatedTick", -999999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.Props.chargeIntervalTicks > 0 && this.startedChargingTick <= 0)
				{
					this.startedChargingTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
				}
				if (this.stunner == null)
				{
					this.stunner = new StunHandler(this.parent);
				}
			}
		}

		// Token: 0x0600A23A RID: 41530 RVA: 0x0039FCDC File Offset: 0x0039DEDC
		[CompilerGenerated]
		private Vector3 <BreakShieldEmp>g__PosAtIndex|66_0(int index, ref CompProjectileInterceptor.<>c__DisplayClass66_0 A_2)
		{
			return new Vector3(this.Props.radius * Mathf.Cos(A_2.fTheta * (float)index) + A_2.center.x, 0f, this.Props.radius * Mathf.Sin(A_2.fTheta * (float)index) + A_2.center.z);
		}

		// Token: 0x04005FCC RID: 24524
		private int lastInterceptTicks = -999999;

		// Token: 0x04005FCD RID: 24525
		private int startedChargingTick = -1;

		// Token: 0x04005FCE RID: 24526
		private bool shutDown;

		// Token: 0x04005FCF RID: 24527
		private StunHandler stunner;

		// Token: 0x04005FD0 RID: 24528
		private Sustainer sustainer;

		// Token: 0x04005FD1 RID: 24529
		public int currentHitPoints = -1;

		// Token: 0x04005FD2 RID: 24530
		public int? maxHitPointsOverride;

		// Token: 0x04005FD3 RID: 24531
		private int activatedTick = -999999;

		// Token: 0x04005FD4 RID: 24532
		private float lastInterceptAngle;

		// Token: 0x04005FD5 RID: 24533
		private bool drawInterceptCone;

		// Token: 0x04005FD6 RID: 24534
		private bool debugInterceptNonHostileProjectiles;

		// Token: 0x04005FD7 RID: 24535
		private static readonly Material ForceFieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x04005FD8 RID: 24536
		private static readonly Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

		// Token: 0x04005FD9 RID: 24537
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04005FDA RID: 24538
		private const float TextureActualRingSizeFactor = 1.1601562f;

		// Token: 0x04005FDB RID: 24539
		private static readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f);
	}
}
