using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001466 RID: 5222
	public class CompAbilityEffect_Wallraise : CompAbilityEffect
	{
		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06008035 RID: 32821 RVA: 0x002DBBEF File Offset: 0x002D9DEF
		public new CompProperties_AbilityWallraise Props
		{
			get
			{
				return (CompProperties_AbilityWallraise)this.props;
			}
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x002DBBFC File Offset: 0x002D9DFC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			List<Thing> list = new List<Thing>();
			list.AddRange(this.AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
			where t.def.category == ThingCategory.Item
			select t));
			foreach (Thing thing in list)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			foreach (IntVec3 intVec in this.AffectedCells(target, map))
			{
				GenSpawn.Spawn(ThingDefOf.RaisedRocks, intVec, map, WipeMode.Vanish);
				FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Wallraise.DustColor);
				CompAbilityEffect_Teleport.SendSkipUsedSignal(intVec, this.parent.pawn);
			}
			foreach (Thing thing2 in list)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec3 = thing2.Position + GenRadial.RadialPattern[i];
					if (intVec3.InBounds(map) && intVec3.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec3).Count <= 0)
					{
						intVec2 = intVec3;
						break;
					}
				}
				if (intVec2 != IntVec3.Invalid)
				{
					GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
				}
				else
				{
					GenPlace.TryPlaceThing(thing2, thing2.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x002DBE18 File Offset: 0x002DA018
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, true);
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x002DBE24 File Offset: 0x002DA024
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null, null, 2900);
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x002DBE77 File Offset: 0x002DA077
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			CompAbilityEffect_Wallraise.<AffectedCells>d__6 <AffectedCells>d__ = new CompAbilityEffect_Wallraise.<AffectedCells>d__6(-2);
			<AffectedCells>d__.<>4__this = this;
			<AffectedCells>d__.<>3__target = target;
			<AffectedCells>d__.<>3__map = map;
			return <AffectedCells>d__;
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x002DBE98 File Offset: 0x002DA098
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.Filled(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => !c.Standable(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityUnwalkable".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.GetThingList(this.parent.pawn.Map).Any((Thing t) => !t.def.destroyable)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityNotEnoughFreeSpace".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0400500E RID: 20494
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
	}
}
