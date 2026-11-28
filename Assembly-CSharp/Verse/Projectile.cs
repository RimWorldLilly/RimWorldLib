using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000593 RID: 1427
	[StaticConstructorOnStartup]
	public abstract class Projectile : ThingWithComps
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002A14 RID: 10772 RVA: 0x00100FD1 File Offset: 0x000FF1D1
		public DamageDef DamageDef
		{
			get
			{
				return this.damageDefOverride ?? this.def.projectile.damageDef;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x00100FF0 File Offset: 0x000FF1F0
		public IEnumerable<ExtraDamage> ExtraDamages
		{
			get
			{
				IEnumerable<ExtraDamage> first = this.extraDamages;
				IEnumerable<ExtraDamage> enumerable = this.def.projectile.extraDamages;
				return first.Concat(enumerable ?? Enumerable.Empty<ExtraDamage>());
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x00101023 File Offset: 0x000FF223
		// (set) Token: 0x06002A17 RID: 10775 RVA: 0x00101053 File Offset: 0x000FF253
		public ProjectileHitFlags HitFlags
		{
			get
			{
				if (this.def.projectile.alwaysFreeIntercept)
				{
					return ProjectileHitFlags.All;
				}
				if (this.def.projectile.flyOverhead)
				{
					return ProjectileHitFlags.None;
				}
				return this.desiredHitFlags;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002A18 RID: 10776 RVA: 0x0010105C File Offset: 0x000FF25C
		protected float StartingTicksToImpact
		{
			get
			{
				float num = (this.origin - this.destination).magnitude / this.def.projectile.SpeedTilesPerTick;
				if (num <= 0f)
				{
					num = 0.001f;
				}
				return num;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002A19 RID: 10777 RVA: 0x001010A3 File Offset: 0x000FF2A3
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002A1A RID: 10778 RVA: 0x001010B0 File Offset: 0x000FF2B0
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin).Yto0() * this.DistanceCoveredFraction;
				return this.origin.Yto0() + b + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x0010110A File Offset: 0x000FF30A
		protected float DistanceCoveredFraction
		{
			get
			{
				return Mathf.Clamp01(1f - (float)this.ticksToImpact / this.StartingTicksToImpact);
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x00101125 File Offset: 0x000FF325
		protected float DistanceCoveredFractionArc
		{
			get
			{
				return Mathf.Clamp01(1f - (float)(this.landed ? this.lifetime : this.ticksToImpact) / this.StartingTicksToImpact);
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x00101150 File Offset: 0x000FF350
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation((this.destination - this.origin).Yto0());
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002A1E RID: 10782 RVA: 0x0010116D File Offset: 0x000FF36D
		public virtual bool AnimalsFleeImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x00101170 File Offset: 0x000FF370
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x00101178 File Offset: 0x000FF378
		public virtual Material DrawMat
		{
			get
			{
				return this.def.graphic.MatSingleFor(this);
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x0010118B File Offset: 0x000FF38B
		public virtual int DamageAmount
		{
			get
			{
				return this.def.projectile.GetDamageAmount(this.equipment, null);
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x001011A4 File Offset: 0x000FF3A4
		public virtual float ArmorPenetration
		{
			get
			{
				return this.def.projectile.GetArmorPenetration(this.equipment, null);
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x001011BD File Offset: 0x000FF3BD
		public ThingDef EquipmentDef
		{
			get
			{
				return this.equipmentDef;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002A24 RID: 10788 RVA: 0x001011C5 File Offset: 0x000FF3C5
		public Thing Launcher
		{
			get
			{
				return this.launcher;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x001011CD File Offset: 0x000FF3CD
		public override int UpdateRateTicks
		{
			get
			{
				if (base.Spawned && Find.CurrentMap == base.Map && Find.CameraDriver.InViewOf(this))
				{
					return 1;
				}
				return 15;
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x001011F8 File Offset: 0x000FF3F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_References.Look<Thing>(ref this.equipment, "equipment", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
			Scribe_Values.Look<int>(ref this.lifetime, "lifetime", 0, false);
			Scribe_Values.Look<QualityCategory>(ref this.equipmentQuality, "equipmentQuality", QualityCategory.Normal, false);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00101310 File Offset: 0x000FF510
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, null);
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x0010133C File Offset: 0x000FF53C
		public virtual void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.preventFriendlyFire = preventFriendlyFire;
			this.HitFlags = hitFlags;
			this.stoppingPower = this.def.projectile.stoppingPower;
			if (this.stoppingPower == 0f && this.def.projectile.damageDef != null)
			{
				this.stoppingPower = this.def.projectile.damageDef.defaultStoppingPower;
			}
			if (equipment != null)
			{
				this.equipment = equipment;
				this.equipmentDef = equipment.def;
				equipment.TryGetQuality(out this.equipmentQuality);
				CompUniqueWeapon compUniqueWeapon;
				if (!equipment.TryGetComp(out compUniqueWeapon))
				{
					goto IL_115;
				}
				using (List<WeaponTraitDef>.Enumerator enumerator = compUniqueWeapon.TraitsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WeaponTraitDef weaponTraitDef = enumerator.Current;
						if (!Mathf.Approximately(weaponTraitDef.additionalStoppingPower, 0f))
						{
							this.stoppingPower += weaponTraitDef.additionalStoppingPower;
						}
					}
					goto IL_115;
				}
			}
			this.equipmentDef = null;
			IL_115:
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = Mathf.CeilToInt(this.StartingTicksToImpact);
			if (this.ticksToImpact < 1)
			{
				this.ticksToImpact = 1;
			}
			this.lifetime = this.ticksToImpact;
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x00101500 File Offset: 0x000FF700
		protected override void Tick()
		{
			base.Tick();
			if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
			{
				this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
			}
			if (this.ambientSustainer != null)
			{
				this.ambientSustainer.Maintain();
			}
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0010156C File Offset: 0x000FF76C
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.lifetime -= delta;
			if (this.landed)
			{
				return;
			}
			Vector3 exactPosition = this.ExactPosition;
			this.ticksToImpact -= delta;
			if (!this.ExactPosition.InBounds(base.Map))
			{
				this.ticksToImpact += delta;
				base.Position = this.ExactPosition.ToIntVec3();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Vector3 exactPosition2 = this.ExactPosition;
			if (this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
			{
				return;
			}
			base.Position = this.ExactPosition.ToIntVec3();
			if (this.ticksToImpact <= 0)
			{
				if (this.DestinationCell.InBounds(base.Map))
				{
					base.Position = this.DestinationCell;
				}
				this.ImpactSomething();
				return;
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x00101638 File Offset: 0x000FF838
		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (lastExactPos == newExactPos)
			{
				return false;
			}
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckIntercept(this, lastExactPos, newExactPos))
				{
					this.Impact(null, true);
					return true;
				}
			}
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			if (intVec2 == intVec)
			{
				return false;
			}
			if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				return false;
			}
			if (intVec2.AdjacentToCardinal(intVec))
			{
				return this.CheckForFreeIntercept(intVec2);
			}
			if (VerbUtility.InterceptChanceFactorFromDistance(this.origin, intVec2) <= 0f)
			{
				return false;
			}
			Vector3 vector = lastExactPos;
			Vector3 v = newExactPos - lastExactPos;
			Vector3 b = v.normalized * 0.2f;
			int num = (int)(v.MagnitudeHorizontal() / 0.2f);
			Projectile.checkedCells.Clear();
			int num2 = 0;
			for (;;)
			{
				vector += b;
				IntVec3 intVec3 = vector.ToIntVec3();
				if (!Projectile.checkedCells.Contains(intVec3))
				{
					if (this.CheckForFreeIntercept(intVec3))
					{
						break;
					}
					Projectile.checkedCells.Add(intVec3);
				}
				num2++;
				if (num2 > num)
				{
					return false;
				}
				if (intVec3 == intVec2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00101784 File Offset: 0x000FF984
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			if (this.destination.ToIntVec3() == c)
			{
				return false;
			}
			float num = VerbUtility.InterceptChanceFactorFromDistance(this.origin, c);
			if (num <= 0f)
			{
				return false;
			}
			bool flag = false;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (this.CanHit(thing))
				{
					bool flag2 = false;
					if (thing.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = thing as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							this.ThrowDebugText("int-wall", c);
							this.Impact(thing, false);
							return true;
						}
						flag2 = true;
					}
					float num2 = 0f;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						num2 = 0.4f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing)
						{
							num2 *= 0.1f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							if (this.preventFriendlyFire)
							{
								num2 = 0f;
								this.ThrowDebugText("ff-miss", c);
							}
							else
							{
								num2 *= Find.Storyteller.difficulty.friendlyFireChanceFactor;
							}
						}
					}
					else if (thing.def.fillPercent > 0.2f)
					{
						if (flag2)
						{
							num2 = 0.05f;
						}
						else if (this.DestinationCell.AdjacentTo8Way(c))
						{
							num2 = thing.def.fillPercent * 1f;
						}
						else
						{
							num2 = thing.def.fillPercent * 0.15f;
						}
					}
					num2 *= num;
					if (num2 > 1E-05f)
					{
						if (Rand.Chance(num2))
						{
							this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
							this.Impact(thing, false);
							return true;
						}
						flag = true;
						this.ThrowDebugText(num2.ToStringPercent(), c);
					}
				}
			}
			if (!flag)
			{
				this.ThrowDebugText("o", c);
			}
			return false;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x001019AE File Offset: 0x000FFBAE
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x001019D0 File Offset: 0x000FFBD0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float num = this.ArcHeightFactor * GenMath.InverseParabola(this.DistanceCoveredFractionArc);
			Vector3 vector = drawLoc + new Vector3(0f, 0f, 1f) * num;
			if (this.def.projectile.shadowSize > 0f)
			{
				this.DrawShadow(drawLoc, num);
			}
			Quaternion rotation = this.ExactRotation;
			if (this.def.projectile.spinRate != 0f)
			{
				float num2 = 60f / this.def.projectile.spinRate;
				rotation = Quaternion.AngleAxis((float)Find.TickManager.TicksGame % num2 / num2 * 360f, Vector3.up);
			}
			if (this.def.projectile.useGraphicClass)
			{
				this.Graphic.Draw(vector, base.Rotation, this, rotation.eulerAngles.y);
			}
			else
			{
				Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), vector, rotation, this.DrawMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x00101AE8 File Offset: 0x000FFCE8
		private float ArcHeightFactor
		{
			get
			{
				float num = this.def.projectile.arcHeightFactor;
				float num2 = (this.destination - this.origin).MagnitudeHorizontalSquared();
				if (num * num > num2 * 0.2f * 0.2f)
				{
					num = Mathf.Sqrt(num2) * 0.2f;
				}
				return num;
			}
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x00101B40 File Offset: 0x000FFD40
		protected bool CanHit(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing == this.launcher)
			{
				return false;
			}
			ProjectileHitFlags hitFlags = this.HitFlags;
			if (hitFlags == ProjectileHitFlags.None)
			{
				return false;
			}
			if (thing.Map != base.Map)
			{
				return false;
			}
			if (CoverUtility.ThingCovered(thing, base.Map))
			{
				return false;
			}
			if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
			{
				return true;
			}
			if (thing != this.intendedTarget)
			{
				if (thing is Pawn)
				{
					if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
					{
						return true;
					}
				}
				else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
				{
					return true;
				}
			}
			return thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x00101BF8 File Offset: 0x000FFDF8
		protected virtual void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						if (!this.def.projectile.soundHitThickRoof.NullOrUndefined())
						{
							this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						}
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !this.CanHit(this.usedTarget.Thing))
			{
				List<Thing> list = VerbUtility.ThingsToHit(base.Position, base.Map, new Func<Thing, bool>(this.CanHit));
				list.Shuffle<Thing>();
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					Pawn pawn = thing as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
						{
							num *= 0.5f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(thing, false);
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null, false);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.5f))
			{
				this.ThrowDebugText("miss-laying", base.Position);
				this.Impact(null, false);
				return;
			}
			this.Impact(this.usedTarget.Thing, false);
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x00101ED0 File Offset: 0x001000D0
		protected virtual void Impact(Thing hitThing, bool blockedByShield = false)
		{
			GenClamor.DoClamor(this, 12f, ClamorDefOf.Impact);
			if (!blockedByShield && this.def.projectile.landedEffecter != null)
			{
				this.def.projectile.landedEffecter.Spawn(this.ExactPosition.ToIntVec3(), base.Map, 1f).Cleanup();
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x00101F3C File Offset: 0x0010013C
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			if (Projectile.shadowMaterial == null)
			{
				return;
			}
			float num = this.def.projectile.shadowSize * Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Vector3 b = new Vector3(0f, -0.01f, 0f);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc + b, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Projectile.shadowMaterial, 0);
		}

		// Token: 0x04001D23 RID: 7459
		private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

		// Token: 0x04001D24 RID: 7460
		protected Vector3 origin;

		// Token: 0x04001D25 RID: 7461
		protected Vector3 destination;

		// Token: 0x04001D26 RID: 7462
		public LocalTargetInfo usedTarget;

		// Token: 0x04001D27 RID: 7463
		public LocalTargetInfo intendedTarget;

		// Token: 0x04001D28 RID: 7464
		protected Thing equipment;

		// Token: 0x04001D29 RID: 7465
		protected ThingDef equipmentDef;

		// Token: 0x04001D2A RID: 7466
		protected Thing launcher;

		// Token: 0x04001D2B RID: 7467
		protected ThingDef targetCoverDef;

		// Token: 0x04001D2C RID: 7468
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		// Token: 0x04001D2D RID: 7469
		protected bool preventFriendlyFire;

		// Token: 0x04001D2E RID: 7470
		protected int lifetime;

		// Token: 0x04001D2F RID: 7471
		protected QualityCategory equipmentQuality = QualityCategory.Normal;

		// Token: 0x04001D30 RID: 7472
		public float stoppingPower;

		// Token: 0x04001D31 RID: 7473
		public DamageDef damageDefOverride;

		// Token: 0x04001D32 RID: 7474
		public List<ExtraDamage> extraDamages = new List<ExtraDamage>();

		// Token: 0x04001D33 RID: 7475
		protected bool landed;

		// Token: 0x04001D34 RID: 7476
		protected int ticksToImpact;

		// Token: 0x04001D35 RID: 7477
		private Sustainer ambientSustainer;

		// Token: 0x04001D36 RID: 7478
		private static List<IntVec3> checkedCells = new List<IntVec3>();
	}
}
