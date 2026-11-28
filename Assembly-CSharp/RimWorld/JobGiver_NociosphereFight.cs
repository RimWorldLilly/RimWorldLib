using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B53 RID: 2899
	public class JobGiver_NociosphereFight : ThinkNode_JobGiver
	{
		// Token: 0x060054BD RID: 21693 RVA: 0x001F7A90 File Offset: 0x001F5C90
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.IsAnyAttackOnCooldown(pawn))
			{
				return null;
			}
			Thing thing = NociosphereUtility.FindTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			Ability randomPossibleAbility = this.GetRandomPossibleAbility(pawn, thing);
			if (randomPossibleAbility == null)
			{
				return null;
			}
			return randomPossibleAbility.GetJob(thing, null);
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001F7AD4 File Offset: 0x001F5CD4
		private bool IsAnyAttackOnCooldown(Pawn pawn)
		{
			List<CompProperties_Nociosphere.AttackDetails> attacks = pawn.TryGetComp<CompNociosphere>().Props.attacks;
			for (int i = 0; i < attacks.Count; i++)
			{
				Ability ability = pawn.abilities.GetAbility(attacks[i].ability, false);
				if (!ability.CanCast && (float)ability.CooldownTicksRemaining <= attacks[i].maxCooldownTicks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001F7B44 File Offset: 0x001F5D44
		private Ability GetRandomPossibleAbility(Pawn pawn, Thing target)
		{
			List<CompProperties_Nociosphere.AttackDetails> attacks = pawn.TryGetComp<CompNociosphere>().Props.attacks;
			bool flag = false;
			for (int i = 0; i < attacks.Count; i++)
			{
				Ability ability = pawn.abilities.GetAbility(attacks[i].ability, false);
				Verb verb = ability.verb;
				if (ability.CanCast && verb.CanHitTarget(target) && (attacks[i].requiresPawn || target is Pawn))
				{
					if (attacks[i].aiPreferArtificial)
					{
						flag = true;
					}
					this.tmpAttacks.Add(attacks[i]);
				}
			}
			if (this.tmpAttacks.Empty<CompProperties_Nociosphere.AttackDetails>())
			{
				return null;
			}
			bool artificialTarget = target == null || target is Building || target is Plant;
			CompProperties_Nociosphere.AttackDetails attackDetails;
			if (flag)
			{
				attackDetails = this.tmpAttacks.RandomElementByWeight(delegate(CompProperties_Nociosphere.AttackDetails a)
				{
					if (a.aiPreferArtificial)
					{
						return (float)(artificialTarget ? 2 : 1);
					}
					return (float)(artificialTarget ? 1 : 2);
				});
			}
			else
			{
				attackDetails = this.tmpAttacks.RandomElement<CompProperties_Nociosphere.AttackDetails>();
			}
			this.tmpAttacks.Clear();
			return pawn.abilities.GetAbility(attackDetails.ability, false);
		}

		// Token: 0x04003295 RID: 12949
		private readonly List<CompProperties_Nociosphere.AttackDetails> tmpAttacks = new List<CompProperties_Nociosphere.AttackDetails>();
	}
}
