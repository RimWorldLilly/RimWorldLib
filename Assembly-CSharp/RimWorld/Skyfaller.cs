using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B55 RID: 6997
	[StaticConstructorOnStartup]
	public class Skyfaller : ThingWithComps, IThingHolder, IRoofCollapseAlert
	{
		// Token: 0x1700207B RID: 8315
		// (get) Token: 0x0600B39D RID: 45981 RVA: 0x003F4D86 File Offset: 0x003F2F86
		public int LeaveMapAfterTicks
		{
			get
			{
				if (this.ticksToDiscard <= 0)
				{
					return 220;
				}
				return this.ticksToDiscard;
			}
		}

		// Token: 0x1700207C RID: 8316
		// (get) Token: 0x0600B39E RID: 45982 RVA: 0x003F4D9D File Offset: 0x003F2F9D
		// (set) Token: 0x0600B39F RID: 45983 RVA: 0x003F4DA5 File Offset: 0x003F2FA5
		public bool? OverrideFlightFlippedHorizontal { get; set; }

		// Token: 0x1700207D RID: 8317
		// (get) Token: 0x0600B3A0 RID: 45984 RVA: 0x003F4DAE File Offset: 0x003F2FAE
		public CompSkyfallerRandomizeDirection RandomizeDirectionComp
		{
			get
			{
				return this.randomizeDirectionComp;
			}
		}

		// Token: 0x0600B3A1 RID: 45985 RVA: 0x003F4DB6 File Offset: 0x003F2FB6
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.randomizeDirectionComp = base.GetComp<CompSkyfallerRandomizeDirection>();
		}

		// Token: 0x1700207E RID: 8318
		// (get) Token: 0x0600B3A2 RID: 45986 RVA: 0x003F4DCC File Offset: 0x003F2FCC
		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				if (this.def.skyfaller.fadeInTicks > 0 || this.def.skyfaller.fadeOutTicks > 0)
				{
					return this.def.graphicData.GraphicColoredFor(thingForGraphic);
				}
				if (thingForGraphic == this)
				{
					return base.Graphic;
				}
				return thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic, null).GetShadowlessGraphic();
			}
		}

		// Token: 0x1700207F RID: 8319
		// (get) Token: 0x0600B3A3 RID: 45987 RVA: 0x003F4E40 File Offset: 0x003F3040
		public override Vector3 DrawPos
		{
			get
			{
				switch (this.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				case SkyfallerMovementType.ConstantSpeed:
					return SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				case SkyfallerMovementType.Decelerate:
					return SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				default:
					Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType.ToString(), this.thingIDNumber ^ 1948576711);
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				}
			}
		}

		// Token: 0x17002080 RID: 8320
		// (get) Token: 0x0600B3A4 RID: 45988 RVA: 0x003F4FE0 File Offset: 0x003F31E0
		// (set) Token: 0x0600B3A5 RID: 45989 RVA: 0x003F50CB File Offset: 0x003F32CB
		public override Color DrawColor
		{
			get
			{
				if (this.def.skyfaller.fadeInTicks > 0 && this.ageTicks < this.def.skyfaller.fadeInTicks)
				{
					Color drawColor = base.DrawColor;
					drawColor.a *= Mathf.Lerp(0f, 1f, Mathf.Min((float)this.ageTicks / (float)this.def.skyfaller.fadeInTicks, 1f));
					return drawColor;
				}
				if (this.FadingOut)
				{
					Color drawColor2 = base.DrawColor;
					drawColor2.a *= Mathf.Lerp(1f, 0f, Mathf.Max((float)this.ageTicks - (float)(this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks), 0f) / (float)this.def.skyfaller.fadeOutTicks);
					return drawColor2;
				}
				return base.DrawColor;
			}
			set
			{
				base.DrawColor = value;
			}
		}

		// Token: 0x17002081 RID: 8321
		// (get) Token: 0x0600B3A6 RID: 45990 RVA: 0x003F50D4 File Offset: 0x003F32D4
		public bool FadingOut
		{
			get
			{
				return this.def.skyfaller.fadeOutTicks > 0 && this.ageTicks >= this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks;
			}
		}

		// Token: 0x17002082 RID: 8322
		// (get) Token: 0x0600B3A7 RID: 45991 RVA: 0x003F5110 File Offset: 0x003F3310
		protected Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x17002083 RID: 8323
		// (get) Token: 0x0600B3A8 RID: 45992 RVA: 0x003F5168 File Offset: 0x003F3368
		protected float TimeInAnimation
		{
			get
			{
				if (this.def.skyfaller.reversed)
				{
					return (float)this.ticksToImpact / (float)this.LeaveMapAfterTicks;
				}
				return 1f - (float)this.ticksToImpact / (float)this.ticksToImpactMax;
			}
		}

		// Token: 0x17002084 RID: 8324
		// (get) Token: 0x0600B3A9 RID: 45993 RVA: 0x003F51A4 File Offset: 0x003F33A4
		private float CurrentSpeed
		{
			get
			{
				if (this.def.skyfaller.speedCurve == null)
				{
					return this.def.skyfaller.speed;
				}
				return this.def.skyfaller.speedCurve.Evaluate(this.TimeInAnimation) * this.def.skyfaller.speed;
			}
		}

		// Token: 0x17002085 RID: 8325
		// (get) Token: 0x0600B3AA RID: 45994 RVA: 0x003F5200 File Offset: 0x003F3400
		private bool SpawnTimedMotes
		{
			get
			{
				return this.def.skyfaller.moteSpawnTime != float.MinValue && Mathf.Approximately(this.def.skyfaller.moteSpawnTime, this.TimeInAnimation);
			}
		}

		// Token: 0x0600B3AB RID: 45995 RVA: 0x003F5236 File Offset: 0x003F3436
		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600B3AC RID: 45996 RVA: 0x003F525C File Offset: 0x003F345C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<Letter>(ref this.impactLetter, "impactLetter", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToDiscard, "ticksToDiscard", 0, false);
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToImpactMax, "ticksToImpactMax", this.LeaveMapAfterTicks, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasHitRoof, "hasHitRoof", false, false);
			Scribe_Values.Look<bool>(ref this.hasImpacted, "hasImpacted", false, false);
			Scribe_Values.Look<bool>(ref this.hasLeftMap, "hasLeftMap", false, false);
			Scribe_Values.Look<bool>(ref this.contentsCanOverlap, "contentsCanOverlap", true, false);
			Scribe_Values.Look<bool>(ref this.moveAside, "moveAside", false, false);
		}

		// Token: 0x0600B3AD RID: 45997 RVA: 0x003F5371 File Offset: 0x003F3571
		public override void PostMake()
		{
			base.PostMake();
			if (this.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		// Token: 0x0600B3AE RID: 45998 RVA: 0x003F53A0 File Offset: 0x003F35A0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.ticksToImpact = (this.ticksToImpactMax = this.def.skyfaller.ticksToImpactRange.RandomInRange);
			this.ticksToDiscard = ((this.def.skyfaller.ticksToDiscardInReverse != IntRange.Zero) ? this.def.skyfaller.ticksToDiscardInReverse.RandomInRange : -1);
			if (this.def.skyfaller.MakesShrapnel)
			{
				float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
				if (num < 270f && num >= 90f)
				{
					this.angle = Rand.Range(0f, 33f);
				}
				else
				{
					this.angle = Rand.Range(-33f, 0f);
				}
			}
			else if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(0f);
			}
			else
			{
				this.angle = -33.7f;
			}
			if (this.def.rotatable && this.innerContainer.Any)
			{
				base.Rotation = this.innerContainer[0].Rotation;
			}
		}

		// Token: 0x0600B3AF RID: 45999 RVA: 0x003F54F0 File Offset: 0x003F36F0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.anticipationSoundPlaying != null)
			{
				this.anticipationSoundPlaying.End();
				this.anticipationSoundPlaying = null;
			}
		}

		// Token: 0x0600B3B0 RID: 46000 RVA: 0x003F5520 File Offset: 0x003F3720
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float extraRotation;
			this.GetDrawPositionAndRotation(ref drawLoc, out extraRotation);
			Thing thingForGraphic = this.GetThingForGraphic();
			if (!WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.Graphic.Draw(drawLoc, flip ? thingForGraphic.Rotation.Opposite : thingForGraphic.Rotation, thingForGraphic, extraRotation);
			}
			this.DrawDropSpotShadow();
		}

		// Token: 0x0600B3B1 RID: 46001 RVA: 0x003F5574 File Offset: 0x003F3774
		protected virtual void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				extraRotation = this.angle;
			}
			if (this.randomizeDirectionComp != null)
			{
				extraRotation += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.rotationCurve != null)
			{
				extraRotation += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.xPositionCurve != null)
			{
				drawLoc.x += this.def.skyfaller.xPositionCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.zPositionCurve != null)
			{
				drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(this.TimeInAnimation);
			}
		}

		// Token: 0x0600B3B2 RID: 46002 RVA: 0x003F5690 File Offset: 0x003F3890
		public virtual float DrawAngle()
		{
			float num = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				num = this.angle;
			}
			num += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			if (this.randomizeDirectionComp != null)
			{
				num += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			return num;
		}

		// Token: 0x0600B3B3 RID: 46003 RVA: 0x003F56F4 File Offset: 0x003F38F4
		protected override void Tick()
		{
			base.Tick();
			if (this.SpawnTimedMotes)
			{
				CellRect cellRect = this.OccupiedRect();
				for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
				{
					FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
				}
			}
			if (this.def.skyfaller.floatingSound != null && (this.floatingSoundPlaying == null || this.floatingSoundPlaying.Ended))
			{
				this.floatingSoundPlaying = this.def.skyfaller.floatingSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this), MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.floatingSoundPlaying;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			if (this.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact > this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					TargetInfo source = new TargetInfo(base.Position, base.Map, false);
					if (this.def.skyfaller.anticipationSound.sustain)
					{
						this.anticipationSoundPlaying = this.def.skyfaller.anticipationSound.TrySpawnSustainer(source);
					}
					else
					{
						this.def.skyfaller.anticipationSound.PlayOneShot(source);
					}
				}
				if (this.ticksToImpact >= this.LeaveMapAfterTicks && !this.hasLeftMap)
				{
					this.LeaveMap();
				}
			}
			else
			{
				this.ticksToImpact--;
				if (this.ticksToImpact <= 15 && !this.hasHitRoof)
				{
					this.HitRoof();
				}
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact < this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					TargetInfo source2 = new TargetInfo(base.Position, base.Map, false);
					if (this.def.skyfaller.anticipationSound.sustain)
					{
						this.anticipationSoundPlaying = this.def.skyfaller.anticipationSound.TrySpawnSustainer(source2);
					}
					else
					{
						this.def.skyfaller.anticipationSound.PlayOneShot(source2);
					}
				}
				Sustainer sustainer2 = this.anticipationSoundPlaying;
				if (sustainer2 != null)
				{
					sustainer2.Maintain();
				}
				if (this.ticksToImpact <= 0 && !this.hasImpacted)
				{
					this.Impact();
				}
			}
			this.ageTicks++;
		}

		// Token: 0x0600B3B4 RID: 46004 RVA: 0x003F59A8 File Offset: 0x003F3BA8
		protected virtual void HitRoof()
		{
			if (!this.def.skyfaller.hitRoof)
			{
				return;
			}
			CellRect cr = this.OccupiedRect();
			this.hasHitRoof = true;
			if (!cr.Cells.Any((IntVec3 x) => x.InBounds(this.Map) && x.Roofed(this.Map)))
			{
				return;
			}
			RoofDef roof = cr.Cells.First((IntVec3 x) => x.InBounds(this.Map) && x.Roofed(this.Map)).GetRoof(base.Map);
			if (!roof.soundPunchThrough.NullOrUndefined())
			{
				roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			CellRect cellRect = cr.ExpandedBy((!this.def.skyfaller.minimalRoofDestruction) ? 1 : 0).ClipInsideMap(base.Map);
			Map map = base.Map;
			RoofCollapserImmediate.DropRoofInCells(cellRect.Cells.Where(delegate(IntVec3 c)
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				if (cr.Contains(c))
				{
					return true;
				}
				if (c.GetFirstPawn(map) != null)
				{
					return false;
				}
				Building edifice = c.GetEdifice(map);
				return edifice == null || !edifice.def.holdsRoof;
			}), map, null);
		}

		// Token: 0x0600B3B5 RID: 46005 RVA: 0x003F5ABC File Offset: 0x003F3CBC
		protected virtual void SpawnThings()
		{
			Skyfaller.usedCells.Clear();
			int i;
			int j;
			for (i = this.innerContainer.Count - 1; i >= 0; i = j - 1)
			{
				GenPlace.TryPlaceThing(this.innerContainer[i], base.Position, base.Map, ThingPlaceMode.Near, delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
					if (thing.def.Fillage == FillCategory.Full && this.def.skyfaller.CausesExplosion && this.def.skyfaller.explosionDamage.isExplosive && thing.Position.InHorDistOf(base.Position, this.def.skyfaller.explosionRadius))
					{
						base.Map.terrainGrid.Notify_TerrainDestroyed(thing.Position);
					}
					if (this.moveAside)
					{
						GenSpawn.CheckMoveItemsAside(thing.Position, thing.Rotation, thing.def, thing.Map);
					}
					if (!this.contentsCanOverlap)
					{
						foreach (IntVec3 item in thing.OccupiedRect())
						{
							Skyfaller.usedCells.Add(item);
						}
					}
				}, delegate(IntVec3 c)
				{
					if (!this.contentsCanOverlap)
					{
						foreach (IntVec3 item in GenAdj.OccupiedRect(c, this.innerContainer[i].def.defaultPlacingRot, this.innerContainer[i].def.size))
						{
							if (Skyfaller.usedCells.Contains(item))
							{
								return false;
							}
						}
						return true;
					}
					return true;
				}, new Rot4?(this.innerContainer[i].Rotation), 1);
				j = i;
			}
		}

		// Token: 0x0600B3B6 RID: 46006 RVA: 0x003F5B68 File Offset: 0x003F3D68
		protected virtual void Impact()
		{
			this.hasImpacted = true;
			if (this.def.skyfaller.CausesExplosion)
			{
				IntVec3 position = base.Position;
				Map map = base.Map;
				float explosionRadius = this.def.skyfaller.explosionRadius;
				DamageDef explosionDamage = this.def.skyfaller.explosionDamage;
				Thing instigator = null;
				int damAmount = GenMath.RoundRandom((float)this.def.skyfaller.explosionDamage.defaultDamage * this.def.skyfaller.explosionDamageFactor);
				float armorPenetration = -1f;
				SoundDef explosionSound = null;
				ThingDef weapon = null;
				ThingDef projectile = null;
				Thing intendedTarget = null;
				ThingDef postExplosionSpawnThingDef = null;
				float postExplosionSpawnChance = 0f;
				int postExplosionSpawnThingCount = 1;
				List<Thing> ignoredThings = (!this.def.skyfaller.damageSpawnedThings) ? this.innerContainer.ToList<Thing>() : null;
				GenExplosion.DoExplosion(position, map, explosionRadius, explosionDamage, instigator, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
			this.SpawnThings();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
			{
				FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (this.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, this.def.skyfaller.shrapnelDistanceFactor, this.def.skyfaller.metalShrapnelCountRange.RandomInRange, this.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (this.def.skyfaller.cameraShake > 0f && base.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(this.def.skyfaller.cameraShake);
			}
			if (this.def.skyfaller.impactSound != null)
			{
				this.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			if (this.impactLetter != null)
			{
				Find.LetterStack.ReceiveLetter(this.impactLetter, null, 0, true);
			}
			Map map2 = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.skyfaller.spawnThing != null)
			{
				Thing thing;
				GenSpawn.TrySpawn(this.def.skyfaller.spawnThing, base.Position, map2, out thing, WipeMode.Vanish, true);
			}
		}

		// Token: 0x0600B3B7 RID: 46007 RVA: 0x003F5E0A File Offset: 0x003F400A
		protected virtual void LeaveMap()
		{
			this.hasLeftMap = true;
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600B3B8 RID: 46008 RVA: 0x003F5E1A File Offset: 0x003F401A
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600B3B9 RID: 46009 RVA: 0x003F5E22 File Offset: 0x003F4022
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B3BA RID: 46010 RVA: 0x003F5E30 File Offset: 0x003F4030
		private Thing GetThingForGraphic()
		{
			if (this.def.graphicData != null || !this.innerContainer.Any)
			{
				return this;
			}
			return this.innerContainer[0];
		}

		// Token: 0x0600B3BB RID: 46011 RVA: 0x003F5E5C File Offset: 0x003F405C
		protected virtual void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x003F5EA4 File Offset: 0x003F40A4
		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = AltitudeLayer.Shadows.AltitudeFor();
			float num = 1f + (float)ticksToImpact / 100f;
			Vector3 s = new Vector3(num * shadowSize.x, 1f, num * shadowSize.y);
			Color white = Color.white;
			if (ticksToImpact > 150)
			{
				white.a = Mathf.InverseLerp(200f, 150f, (float)ticksToImpact);
			}
			Skyfaller.shadowPropertyBlock.SetColor(ShaderPropertyIDs.Color, white);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, rot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10Back, matrix, material, 0, null, 0, Skyfaller.shadowPropertyBlock);
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x003F5F5C File Offset: 0x003F415C
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			return RoofCollapseResponse.None;
		}

		// Token: 0x04006796 RID: 26518
		public ThingOwner innerContainer;

		// Token: 0x04006797 RID: 26519
		public int ticksToImpact;

		// Token: 0x04006798 RID: 26520
		public int ageTicks;

		// Token: 0x04006799 RID: 26521
		public int ticksToDiscard;

		// Token: 0x0400679A RID: 26522
		public float angle;

		// Token: 0x0400679B RID: 26523
		public float shrapnelDirection;

		// Token: 0x0400679C RID: 26524
		private int ticksToImpactMax = 220;

		// Token: 0x0400679D RID: 26525
		public Letter impactLetter;

		// Token: 0x0400679E RID: 26526
		public bool contentsCanOverlap = true;

		// Token: 0x0400679F RID: 26527
		private bool hasHitRoof;

		// Token: 0x040067A0 RID: 26528
		protected bool hasImpacted;

		// Token: 0x040067A1 RID: 26529
		private bool hasLeftMap;

		// Token: 0x040067A2 RID: 26530
		public bool moveAside;

		// Token: 0x040067A3 RID: 26531
		private Material cachedShadowMaterial;

		// Token: 0x040067A4 RID: 26532
		private bool anticipationSoundPlayed;

		// Token: 0x040067A5 RID: 26533
		private Sustainer floatingSoundPlaying;

		// Token: 0x040067A6 RID: 26534
		private Sustainer anticipationSoundPlaying;

		// Token: 0x040067A7 RID: 26535
		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x040067A8 RID: 26536
		public const float DefaultAngle = -33.7f;

		// Token: 0x040067A9 RID: 26537
		private const int RoofHitPreDelay = 15;

		// Token: 0x040067AA RID: 26538
		private const int LeaveMapAfterTicksDefault = 220;

		// Token: 0x040067AC RID: 26540
		protected CompSkyfallerRandomizeDirection randomizeDirectionComp;

		// Token: 0x040067AD RID: 26541
		private static readonly List<IntVec3> usedCells = new List<IntVec3>();
	}
}
