using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001405 RID: 5125
	public class CompAbilityEffect_Flashstorm : CompAbilityEffect
	{
		// Token: 0x06007EFE RID: 32510 RVA: 0x002D64A4 File Offset: 0x002D46A4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			Thing conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, -1);
			gameCondition_Flashstorm.centerLocation = target.Cell.ToIntVec2;
			gameCondition_Flashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.parent.def.EffectRadius), Mathf.RoundToInt(this.parent.def.EffectRadius));
			gameCondition_Flashstorm.Duration = Mathf.RoundToInt((float)this.parent.def.EffectDuration(this.parent.pawn).SecondsToTicks());
			gameCondition_Flashstorm.suppressEndMessage = true;
			gameCondition_Flashstorm.initialStrikeDelay = new IntRange(60, 180);
			gameCondition_Flashstorm.conditionCauser = conditionCauser;
			gameCondition_Flashstorm.ambientSound = true;
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			this.ApplyGoodwillImpact(target, gameCondition_Flashstorm.AreaRadius);
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x002D65B4 File Offset: 0x002D47B4
		private void ApplyGoodwillImpact(LocalTargetInfo target, int radius)
		{
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.affectedFactionCache.Clear();
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, this.parent.pawn.Map, (float)radius, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && thing.Faction != null && thing.Faction != this.parent.pawn.Faction && !thing.Faction.HostileTo(this.parent.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) && (base.Props.applyGoodwillImpactToLodgers || !pawn.IsQuestLodger()))
				{
					this.affectedFactionCache.Add(thing.Faction);
					Faction.OfPlayer.TryAffectGoodwillWith(thing.Faction, base.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			this.affectedFactionCache.Clear();
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x002D6700 File Offset: 0x002D4900
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Roofed(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityRoofed".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04004F9E RID: 20382
		private HashSet<Faction> affectedFactionCache = new HashSet<Faction>();
	}
}
