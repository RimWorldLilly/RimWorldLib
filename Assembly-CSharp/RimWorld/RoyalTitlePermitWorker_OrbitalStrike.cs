using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200150D RID: 5389
	public class RoyalTitlePermitWorker_OrbitalStrike : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x0600840B RID: 33803 RVA: 0x002F7AB4 File Offset: 0x002F5CB4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600840C RID: 33804 RVA: 0x002F7B0C File Offset: 0x002F5D0C
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			GenDraw.DrawRadiusRing(target.Cell, this.def.royalAid.radius + this.def.royalAid.explosionRadiusRange.max, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600840D RID: 33805 RVA: 0x002F7B7C File Offset: 0x002F5D7C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallBombardment(target.Cell);
		}

		// Token: 0x0600840E RID: 33806 RVA: 0x002F7B8B File Offset: 0x002F5D8B
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_OrbitalStrike.<GetRoyalAidOptions>d__4 <GetRoyalAidOptions>d__ = new RoyalTitlePermitWorker_OrbitalStrike.<GetRoyalAidOptions>d__4(-2);
			<GetRoyalAidOptions>d__.<>4__this = this;
			<GetRoyalAidOptions>d__.<>3__map = map;
			<GetRoyalAidOptions>d__.<>3__pawn = pawn;
			<GetRoyalAidOptions>d__.<>3__faction = faction;
			return <GetRoyalAidOptions>d__;
		}

		// Token: 0x0600840F RID: 33807 RVA: 0x002F7BB0 File Offset: 0x002F5DB0
		private void BeginCallBombardment(Pawn caller, Faction faction, Map map, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = true;
			this.targetingParameters.canTargetFires = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = caller;
			this.map = map;
			this.faction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = delegate(TargetInfo target)
			{
				if (rangeActual > 0f && target.Cell.DistanceTo(caller.Position) > rangeActual)
				{
					return false;
				}
				if (target.Cell.Fogged(map))
				{
					return false;
				}
				RoofDef roof = target.Cell.GetRoof(map);
				return roof == null || !roof.isThickRoof;
			};
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x002F7C68 File Offset: 0x002F5E68
		private void CallBombardment(IntVec3 targetCell)
		{
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, targetCell, this.map, WipeMode.Vanish);
			bombardment.impactAreaRadius = this.def.royalAid.radius;
			bombardment.explosionRadiusRange = this.def.royalAid.explosionRadiusRange;
			bombardment.bombIntervalTicks = this.def.royalAid.intervalTicks;
			bombardment.randomFireRadius = 1;
			bombardment.explosionCount = this.def.royalAid.explosionCount;
			bombardment.warmupTicks = this.def.royalAid.warmupTicks;
			bombardment.instigator = this.caller;
			SoundDefOf.OrbitalStrike_Ordered.PlayOneShotOnCamera(null);
			this.caller.royalty.GetPermit(this.def, this.faction).Notify_Used();
			if (!this.free)
			{
				this.caller.royalty.TryRemoveFavor(this.faction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x04005186 RID: 20870
		private Faction faction;
	}
}
