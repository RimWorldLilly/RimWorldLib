using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001413 RID: 5139
	public class CompAbilityEffect_GiveMentalState : CompAbilityEffect
	{
		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06007F24 RID: 32548 RVA: 0x002D6E2E File Offset: 0x002D502E
		public new CompProperties_AbilityGiveMentalState Props
		{
			get
			{
				return (CompProperties_AbilityGiveMentalState)this.props;
			}
		}

		// Token: 0x06007F25 RID: 32549 RVA: 0x002D6E3C File Offset: 0x002D503C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = this.Props.applyToSelf ? this.parent.pawn : (target.Thing as Pawn);
			if (pawn != null && !pawn.InMentalState)
			{
				CompAbilityEffect_GiveMentalState.TryGiveMentalState(pawn.RaceProps.IsMechanoid ? (this.Props.stateDefForMechs ?? this.Props.stateDef) : this.Props.stateDef, pawn, this.parent.def, this.Props.durationMultiplier, this.parent.pawn, this.Props.forced);
				RestUtility.WakeUp(pawn, true);
				if (this.Props.casterEffect != null)
				{
					Effecter effecter = this.Props.casterEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
					effecter.Trigger(this.parent.pawn, null, -1);
					effecter.Cleanup();
				}
				if (this.Props.targetEffect != null)
				{
					Effecter effecter2 = this.Props.targetEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
					effecter2.Trigger(pawn, null, -1);
					effecter2.Cleanup();
				}
			}
		}

		// Token: 0x06007F26 RID: 32550 RVA: 0x002D6FAC File Offset: 0x002D51AC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (!base.Valid(target, throwMessages))
			{
				return false;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (this.Props.excludeNPCFactions && pawn.Faction != null && !pawn.Faction.IsPlayer)
				{
					if (throwMessages)
					{
						Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "TargetBelongsToNPCFaction".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007F27 RID: 32551 RVA: 0x002D705C File Offset: 0x002D525C
		public static void TryGiveMentalState(MentalStateDef def, Pawn p, AbilityDef ability, StatDef multiplierStat, Pawn caster, bool forced = false)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, forced, true, false, caster, false, false, ability.IsPsycast))
			{
				float num = ability.GetStatValueAbstract(StatDefOf.Ability_Duration, caster);
				if (multiplierStat != null)
				{
					num *= p.GetStatValue(multiplierStat, true, -1);
				}
				if (num > 0f)
				{
					p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
				}
				p.mindState.mentalStateHandler.CurState.sourceFaction = caster.Faction;
			}
		}
	}
}
