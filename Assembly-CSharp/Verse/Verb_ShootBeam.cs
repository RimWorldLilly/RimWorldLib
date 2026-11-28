using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200073F RID: 1855
	public class Verb_ShootBeam : Verb
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x00176D4C File Offset: 0x00174F4C
		protected override int ShotsPerBurst
		{
			get
			{
				return base.BurstShotCount;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x00176D54 File Offset: 0x00174F54
		public float ShotProgress
		{
			get
			{
				return (float)this.ticksToNextPathStep / (float)base.TicksBetweenBurstShots;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x00176D68 File Offset: 0x00174F68
		public Vector3 InterpolatedPosition
		{
			get
			{
				Vector3 b = base.CurrentTarget.CenterVector3 - this.initialTargetPosition;
				return Vector3.Lerp(this.path[this.burstShotsLeft], this.path[Mathf.Min(this.burstShotsLeft + 1, this.path.Count - 1)], this.ShotProgress) + b;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x00176DD8 File Offset: 0x00174FD8
		public override float? AimAngleOverride
		{
			get
			{
				if (this.state != VerbState.Bursting)
				{
					return null;
				}
				return new float?((this.InterpolatedPosition - this.caster.DrawPos).AngleFlat());
			}
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x00176E18 File Offset: 0x00175018
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlight(target);
			this.CalculatePath(target.CenterVector3, this.tmpPath, this.tmpPathCells, false);
			foreach (IntVec3 targetCell in this.tmpPathCells)
			{
				ShootLine shootLine;
				bool flag = base.TryFindShootLineFromTo(this.caster.Position, target, out shootLine, false);
				IntVec3 intVec;
				if ((!this.verbProps.stopBurstWithoutLos || flag) && this.TryGetHitCell(shootLine.Source, targetCell, out intVec))
				{
					this.tmpHighlightCells.Add(intVec);
					if (this.verbProps.beamHitsNeighborCells)
					{
						foreach (IntVec3 item in this.GetBeamHitNeighbourCells(shootLine.Source, intVec))
						{
							if (!this.tmpHighlightCells.Contains(item))
							{
								this.tmpSecondaryHighlightCells.Add(item);
							}
						}
					}
				}
			}
			this.tmpSecondaryHighlightCells.RemoveWhere((IntVec3 x) => this.tmpHighlightCells.Contains(x));
			if (this.tmpHighlightCells.Any<IntVec3>())
			{
				GenDraw.DrawFieldEdges(this.tmpHighlightCells.ToList<IntVec3>(), this.verbProps.highlightColor ?? Color.white, null, null, 2900);
			}
			if (this.tmpSecondaryHighlightCells.Any<IntVec3>())
			{
				GenDraw.DrawFieldEdges(this.tmpSecondaryHighlightCells.ToList<IntVec3>(), this.verbProps.secondaryHighlightColor ?? Color.white, null, null, 2900);
			}
			this.tmpHighlightCells.Clear();
			this.tmpSecondaryHighlightCells.Clear();
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x00177014 File Offset: 0x00175214
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine, false);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelReloadable comp2 = base.EquipmentSource.GetComp<CompApparelReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.lastShotTick = Find.TickManager.TicksGame;
			this.ticksToNextPathStep = base.TicksBetweenBurstShots;
			IntVec3 targetCell = this.InterpolatedPosition.Yto0().ToIntVec3();
			IntVec3 intVec;
			if (!this.TryGetHitCell(shootLine.Source, targetCell, out intVec))
			{
				return true;
			}
			this.HitCell(intVec, shootLine.Source, 1f);
			if (this.verbProps.beamHitsNeighborCells)
			{
				this.hitCells.Add(intVec);
				foreach (IntVec3 intVec2 in this.GetBeamHitNeighbourCells(shootLine.Source, intVec))
				{
					if (!this.hitCells.Contains(intVec2))
					{
						float damageFactor = this.pathCells.Contains(intVec2) ? 1f : 0.5f;
						this.HitCell(intVec2, shootLine.Source, damageFactor);
						this.hitCells.Add(intVec2);
					}
				}
			}
			return true;
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x001771B4 File Offset: 0x001753B4
		protected bool TryGetHitCell(IntVec3 source, IntVec3 targetCell, out IntVec3 hitCell)
		{
			IntVec3 intVec = GenSight.LastPointOnLineOfSight(source, targetCell, (IntVec3 c) => c.InBounds(this.caster.Map) && c.CanBeSeenOverFast(this.caster.Map), true);
			if (this.verbProps.beamCantHitWithinMinRange && intVec.DistanceTo(source) < this.verbProps.minRange)
			{
				hitCell = default(IntVec3);
				return false;
			}
			hitCell = (intVec.IsValid ? intVec : targetCell);
			return intVec.IsValid;
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0017721C File Offset: 0x0017541C
		protected IntVec3 GetHitCell(IntVec3 source, IntVec3 targetCell)
		{
			IntVec3 result;
			this.TryGetHitCell(source, targetCell, out result);
			return result;
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x00177235 File Offset: 0x00175435
		protected IEnumerable<IntVec3> GetBeamHitNeighbourCells(IntVec3 source, IntVec3 pos)
		{
			Verb_ShootBeam.<GetBeamHitNeighbourCells>d__25 <GetBeamHitNeighbourCells>d__ = new Verb_ShootBeam.<GetBeamHitNeighbourCells>d__25(-2);
			<GetBeamHitNeighbourCells>d__.<>4__this = this;
			<GetBeamHitNeighbourCells>d__.<>3__source = source;
			<GetBeamHitNeighbourCells>d__.<>3__pos = pos;
			return <GetBeamHitNeighbourCells>d__;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00177253 File Offset: 0x00175453
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			return base.TryStartCastOn(this.verbProps.beamTargetsGround ? castTarg.Cell : castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00177280 File Offset: 0x00175480
		public override void BurstingTick()
		{
			this.ticksToNextPathStep--;
			Vector3 vector = this.InterpolatedPosition;
			IntVec3 intVec = vector.ToIntVec3();
			Vector3 vector2 = this.InterpolatedPosition - this.caster.Position.ToVector3Shifted();
			float num = vector2.MagnitudeHorizontal();
			Vector3 normalized = vector2.Yto0().normalized;
			IntVec3 b = GenSight.LastPointOnLineOfSight(this.caster.Position, intVec, (IntVec3 c) => c.CanBeSeenOverFast(this.caster.Map), true);
			if (b.IsValid)
			{
				num -= (intVec - b).LengthHorizontal;
				vector = this.caster.Position.ToVector3Shifted() + normalized * num;
				intVec = vector.ToIntVec3();
			}
			Vector3 offsetA = normalized * this.verbProps.beamStartOffset;
			Vector3 vector3 = vector - intVec.ToVector3Shifted();
			if (this.mote != null)
			{
				this.mote.UpdateTargets(new TargetInfo(this.caster.Position, this.caster.Map, false), new TargetInfo(intVec, this.caster.Map, false), offsetA, vector3);
				this.mote.Maintain();
			}
			if (this.verbProps.beamGroundFleckDef != null && Rand.Chance(this.verbProps.beamFleckChancePerTick))
			{
				FleckMaker.Static(vector, this.caster.Map, this.verbProps.beamGroundFleckDef, 1f);
			}
			if (this.endEffecter == null && this.verbProps.beamEndEffecterDef != null)
			{
				this.endEffecter = this.verbProps.beamEndEffecterDef.Spawn(intVec, this.caster.Map, vector3, 1f);
			}
			if (this.endEffecter != null)
			{
				this.endEffecter.offset = vector3;
				this.endEffecter.EffectTick(new TargetInfo(intVec, this.caster.Map, false), TargetInfo.Invalid);
				this.endEffecter.ticksLeft--;
			}
			if (this.verbProps.beamLineFleckDef != null)
			{
				float num2 = 1f * num;
				int num3 = 0;
				while ((float)num3 < num2)
				{
					if (Rand.Chance(this.verbProps.beamLineFleckChanceCurve.Evaluate((float)num3 / num2)))
					{
						Vector3 b2 = (float)num3 * normalized - normalized * Rand.Value + normalized / 2f;
						FleckMaker.Static(this.caster.Position.ToVector3Shifted() + b2, this.caster.Map, this.verbProps.beamLineFleckDef, 1f);
					}
					num3++;
				}
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x00177544 File Offset: 0x00175744
		public override void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.initialTargetPosition = this.currentTarget.CenterVector3;
			this.CalculatePath(this.currentTarget.CenterVector3, this.path, this.pathCells, true);
			this.hitCells.Clear();
			if (this.verbProps.beamMoteDef != null)
			{
				this.mote = MoteMaker.MakeInteractionOverlay(this.verbProps.beamMoteDef, this.caster, new TargetInfo(this.path[0].ToIntVec3(), this.caster.Map, false));
			}
			base.TryCastNextBurstShot();
			this.ticksToNextPathStep = base.TicksBetweenBurstShots;
			Effecter effecter = this.endEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			if (this.verbProps.soundCastBeam != null)
			{
				this.sustainer = this.verbProps.soundCastBeam.TrySpawnSustainer(SoundInfo.InMap(this.caster, MaintenanceType.PerTick));
			}
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x00177648 File Offset: 0x00175848
		private void CalculatePath(Vector3 target, List<Vector3> pathList, HashSet<IntVec3> pathCellsList, bool addRandomOffset = true)
		{
			pathList.Clear();
			Vector3 vector = (target - this.caster.Position.ToVector3Shifted()).Yto0();
			float magnitude = vector.magnitude;
			Vector3 normalized = vector.normalized;
			Vector3 a = normalized.RotatedBy(-90f);
			float num = (this.verbProps.beamFullWidthRange > 0f) ? Mathf.Min(magnitude / this.verbProps.beamFullWidthRange, 1f) : 1f;
			float d = (this.verbProps.beamWidth + 1f) * num / (float)this.ShotsPerBurst;
			Vector3 vector2 = target.Yto0() - a * this.verbProps.beamWidth / 2f * num;
			pathList.Add(vector2);
			for (int i = 0; i < this.ShotsPerBurst; i++)
			{
				Vector3 a2 = normalized * (Rand.Value * this.verbProps.beamMaxDeviation) - normalized / 2f;
				Vector3 vector3 = Mathf.Sin(((float)i / (float)this.ShotsPerBurst + 0.5f) * 3.1415927f * 57.29578f) * this.verbProps.beamCurvature * -normalized - normalized * this.verbProps.beamMaxDeviation / 2f;
				if (addRandomOffset)
				{
					pathList.Add(vector2 + (a2 + vector3) * num);
				}
				else
				{
					pathList.Add(vector2 + vector3 * num);
				}
				vector2 += a * d;
			}
			pathCellsList.Clear();
			foreach (Vector3 vect in pathList)
			{
				pathCellsList.Add(vect.ToIntVec3());
			}
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x00177858 File Offset: 0x00175A58
		private bool CanHit(Thing thing)
		{
			return thing.Spawned && !CoverUtility.ThingCovered(thing, this.caster.Map);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00177878 File Offset: 0x00175A78
		private void HitCell(IntVec3 cell, IntVec3 sourceCell, float damageFactor = 1f)
		{
			if (!cell.InBounds(this.caster.Map))
			{
				return;
			}
			this.ApplyDamage(VerbUtility.ThingsToHit(cell, this.caster.Map, new Func<Thing, bool>(this.CanHit)).RandomElementWithFallback(null), sourceCell, damageFactor);
			if (this.verbProps.beamSetsGroundOnFire && Rand.Chance(this.verbProps.beamChanceToStartFire))
			{
				FireUtility.TryStartFireIn(cell, this.caster.Map, 1f, this.caster, null);
			}
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x00177904 File Offset: 0x00175B04
		private void ApplyDamage(Thing thing, IntVec3 sourceCell, float damageFactor = 1f)
		{
			IntVec3 intVec = this.InterpolatedPosition.Yto0().ToIntVec3();
			IntVec3 intVec2 = GenSight.LastPointOnLineOfSight(sourceCell, intVec, (IntVec3 c) => c.InBounds(this.caster.Map) && c.CanBeSeenOverFast(this.caster.Map), true);
			if (intVec2.IsValid)
			{
				intVec = intVec2;
			}
			Map map = this.caster.Map;
			if (thing != null && this.verbProps.beamDamageDef != null)
			{
				float angleFlat = (this.currentTarget.Cell - this.caster.Position).AngleFlat;
				BattleLogEntry_RangedImpact log = new BattleLogEntry_RangedImpact(this.caster, thing, this.currentTarget.Thing, base.EquipmentSource.def, null, null);
				DamageInfo dinfo;
				if (this.verbProps.beamTotalDamage > 0f)
				{
					float num = this.verbProps.beamTotalDamage / (float)this.pathCells.Count;
					num *= damageFactor;
					dinfo = new DamageInfo(this.verbProps.beamDamageDef, num, this.verbProps.beamDamageDef.defaultArmorPenetration, angleFlat, this.caster, null, base.EquipmentSource.def, DamageInfo.SourceCategory.ThingOrUnknown, this.currentTarget.Thing, true, true, QualityCategory.Normal, true, false);
				}
				else
				{
					float amount = (float)this.verbProps.beamDamageDef.defaultDamage * damageFactor;
					dinfo = new DamageInfo(this.verbProps.beamDamageDef, amount, this.verbProps.beamDamageDef.defaultArmorPenetration, angleFlat, this.caster, null, base.EquipmentSource.def, DamageInfo.SourceCategory.ThingOrUnknown, this.currentTarget.Thing, true, true, QualityCategory.Normal, true, false);
				}
				thing.TakeDamage(dinfo).AssociateWithLog(log);
				if (thing.CanEverAttachFire())
				{
					float chance;
					if (this.verbProps.flammabilityAttachFireChanceCurve != null)
					{
						chance = this.verbProps.flammabilityAttachFireChanceCurve.Evaluate(thing.GetStatValue(StatDefOf.Flammability, true, -1));
					}
					else
					{
						chance = this.verbProps.beamChanceToAttachFire;
					}
					if (Rand.Chance(chance))
					{
						thing.TryAttachFire(this.verbProps.beamFireSizeRange.RandomInRange, this.caster);
						return;
					}
				}
				else if (Rand.Chance(this.verbProps.beamChanceToStartFire))
				{
					FireUtility.TryStartFireIn(intVec, map, this.verbProps.beamFireSizeRange.RandomInRange, this.caster, this.verbProps.flammabilityAttachFireChanceCurve);
				}
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x00177B3C File Offset: 0x00175D3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Vector3>(ref this.path, "path", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToNextPathStep, "ticksToNextPathStep", 0, false);
			Scribe_Values.Look<Vector3>(ref this.initialTargetPosition, "initialTargetPosition", default(Vector3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.path == null)
			{
				this.path = new List<Vector3>();
			}
		}

		// Token: 0x040026A3 RID: 9891
		private List<Vector3> path = new List<Vector3>();

		// Token: 0x040026A4 RID: 9892
		private List<Vector3> tmpPath = new List<Vector3>();

		// Token: 0x040026A5 RID: 9893
		private int ticksToNextPathStep;

		// Token: 0x040026A6 RID: 9894
		private Vector3 initialTargetPosition;

		// Token: 0x040026A7 RID: 9895
		private MoteDualAttached mote;

		// Token: 0x040026A8 RID: 9896
		private Effecter endEffecter;

		// Token: 0x040026A9 RID: 9897
		private Sustainer sustainer;

		// Token: 0x040026AA RID: 9898
		private HashSet<IntVec3> pathCells = new HashSet<IntVec3>();

		// Token: 0x040026AB RID: 9899
		private HashSet<IntVec3> tmpPathCells = new HashSet<IntVec3>();

		// Token: 0x040026AC RID: 9900
		private HashSet<IntVec3> tmpHighlightCells = new HashSet<IntVec3>();

		// Token: 0x040026AD RID: 9901
		private HashSet<IntVec3> tmpSecondaryHighlightCells = new HashSet<IntVec3>();

		// Token: 0x040026AE RID: 9902
		private HashSet<IntVec3> hitCells = new HashSet<IntVec3>();

		// Token: 0x040026AF RID: 9903
		private const int NumSubdivisionsPerUnitLength = 1;
	}
}
