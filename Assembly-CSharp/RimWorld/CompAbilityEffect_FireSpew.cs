using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001401 RID: 5121
	public class CompAbilityEffect_FireSpew : CompAbilityEffect
	{
		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06007EEE RID: 32494 RVA: 0x002D5E19 File Offset: 0x002D4019
		private new CompProperties_AbilityFireSpew Props
		{
			get
			{
				return (CompProperties_AbilityFireSpew)this.props;
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06007EEF RID: 32495 RVA: 0x002D5E26 File Offset: 0x002D4026
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x06007EF0 RID: 32496 RVA: 0x002D5E34 File Offset: 0x002D4034
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IntVec3 cell = target.Cell;
			Map mapHeld = this.parent.pawn.MapHeld;
			float radius = 0f;
			DamageDef flame = DamageDefOf.Flame;
			Thing pawn = this.Pawn;
			ThingDef filthDef = this.Props.filthDef;
			int damAmount = this.Props.damAmount;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = filthDef;
			float postExplosionSpawnChance = 1f;
			int postExplosionSpawnThingCount = 1;
			SimpleCurve flammabilityAttachFireChanceCurve = this.parent.verb.verbProps.flammabilityAttachFireChanceCurve;
			List<IntVec3> overrideCells = this.AffectedCells(target);
			GenExplosion.DoExplosion(cell, mapHeld, radius, flame, pawn, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 1f, false, null, null, null, false, 0.6f, 0f, false, null, 1f, flammabilityAttachFireChanceCurve, overrideCells, null, null);
			base.Apply(target, dest);
		}

		// Token: 0x06007EF1 RID: 32497 RVA: 0x002D5F0B File Offset: 0x002D410B
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_FireSpew.<GetPreCastActions>d__5 <GetPreCastActions>d__ = new CompAbilityEffect_FireSpew.<GetPreCastActions>d__5(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x002D5F1B File Offset: 0x002D411B
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target), 2900);
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x002D5F30 File Offset: 0x002D4130
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			if (this.Pawn.Faction != null)
			{
				foreach (IntVec3 c in this.AffectedCells(target))
				{
					List<Thing> thingList = c.GetThingList(this.Pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].Faction == this.Pawn.Faction)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007EF4 RID: 32500 RVA: 0x002D5FCC File Offset: 0x002D41CC
		private List<IntVec3> AffectedCells(LocalTargetInfo target)
		{
			this.tmpCells.Clear();
			Vector3 b = this.Pawn.Position.ToVector3Shifted().Yto0();
			IntVec3 intVec = target.Cell.ClampInsideMap(this.Pawn.Map);
			if (this.Pawn.Position == intVec)
			{
				return this.tmpCells;
			}
			float lengthHorizontal = (intVec - this.Pawn.Position).LengthHorizontal;
			float num = (float)(intVec.x - this.Pawn.Position.x) / lengthHorizontal;
			float num2 = (float)(intVec.z - this.Pawn.Position.z) / lengthHorizontal;
			intVec.x = Mathf.RoundToInt((float)this.Pawn.Position.x + num * this.Props.range);
			intVec.z = Mathf.RoundToInt((float)this.Pawn.Position.z + num2 * this.Props.range);
			float target2 = Vector3.SignedAngle(intVec.ToVector3Shifted().Yto0() - b, Vector3.right, Vector3.up);
			float num3 = this.Props.lineWidthEnd / 2f;
			float num4 = Mathf.Sqrt(Mathf.Pow((intVec - this.Pawn.Position).LengthHorizontal, 2f) + Mathf.Pow(num3, 2f));
			float num5 = 57.29578f * Mathf.Asin(num3 / num4);
			int num6 = GenRadial.NumCellsInRadius(this.Props.range);
			for (int i = 0; i < num6; i++)
			{
				IntVec3 intVec2 = this.Pawn.Position + GenRadial.RadialPattern[i];
				if (this.<AffectedCells>g__CanUseCell|9_0(intVec2) && Mathf.Abs(Mathf.DeltaAngle(Vector3.SignedAngle(intVec2.ToVector3Shifted().Yto0() - b, Vector3.right, Vector3.up), target2)) <= num5)
				{
					this.tmpCells.Add(intVec2);
				}
			}
			List<IntVec3> list = GenSight.BresenhamCellsBetween(this.Pawn.Position, intVec);
			for (int j = 0; j < list.Count; j++)
			{
				IntVec3 intVec3 = list[j];
				if (!this.tmpCells.Contains(intVec3) && this.<AffectedCells>g__CanUseCell|9_0(intVec3))
				{
					this.tmpCells.Add(intVec3);
				}
			}
			return this.tmpCells;
		}

		// Token: 0x06007EF7 RID: 32503 RVA: 0x002D62D0 File Offset: 0x002D44D0
		[CompilerGenerated]
		private bool <AffectedCells>g__CanUseCell|9_0(IntVec3 c)
		{
			ShootLine shootLine;
			return c.InBounds(this.Pawn.Map) && !(c == this.Pawn.Position) && (this.Props.canHitFilledCells || !c.Filled(this.Pawn.Map)) && c.InHorDistOf(this.Pawn.Position, this.Props.range) && this.parent.verb.TryFindShootLineFromTo(this.parent.pawn.Position, c, out shootLine, false);
		}

		// Token: 0x04004F9D RID: 20381
		private readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
