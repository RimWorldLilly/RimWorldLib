using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CC RID: 5068
	public class Ability_RandomEffect : Ability
	{
		// Token: 0x06007E26 RID: 32294 RVA: 0x002D192E File Offset: 0x002CFB2E
		public Ability_RandomEffect(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x002D1937 File Offset: 0x002CFB37
		public Ability_RandomEffect(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x002D1944 File Offset: 0x002CFB44
		public override bool CanApplyOn(LocalTargetInfo target)
		{
			using (List<CompAbilityEffect>.Enumerator enumerator = base.EffectComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanApplyOn(target, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x002D19A4 File Offset: 0x002CFBA4
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			(from x in effects
			where x.Props.weight > 0f && x.CanApplyOn(target, dest)
			select x).RandomElementByWeight((CompAbilityEffect x) => x.Props.weight).Apply(target, dest);
		}
	}
}
