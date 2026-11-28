using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001011 RID: 4113
	public class WeaponTraitDef : Def
	{
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060067A6 RID: 26534 RVA: 0x0024A216 File Offset: 0x00248416
		public WeaponTraitWorker Worker
		{
			get
			{
				if (!ModLister.CheckRoyaltyOrOdyssey("Weapon traits"))
				{
					return null;
				}
				if (this.worker == null)
				{
					this.worker = (WeaponTraitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x0024A258 File Offset: 0x00248458
		public bool Overlaps(WeaponTraitDef other)
		{
			return other == this || (!this.exclusionTags.NullOrEmpty<string>() && !other.exclusionTags.NullOrEmpty<string>() && this.exclusionTags.Any((string x) => other.exclusionTags.Contains(x)));
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x0024A2B5 File Offset: 0x002484B5
		public override IEnumerable<string> ConfigErrors()
		{
			WeaponTraitDef.<ConfigErrors>d__27 <ConfigErrors>d__ = new WeaponTraitDef.<ConfigErrors>d__27(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040041E0 RID: 16864
		public Type workerClass = typeof(WeaponTraitWorker);

		// Token: 0x040041E1 RID: 16865
		public WeaponCategoryDef weaponCategory;

		// Token: 0x040041E2 RID: 16866
		public List<string> exclusionTags;

		// Token: 0x040041E3 RID: 16867
		public float commonality;

		// Token: 0x040041E4 RID: 16868
		public bool canGenerateAlone = true;

		// Token: 0x040041E5 RID: 16869
		public DamageDef damageDefOverride;

		// Token: 0x040041E6 RID: 16870
		public List<ExtraDamage> extraDamages;

		// Token: 0x040041E7 RID: 16871
		public List<StatModifier> statOffsets;

		// Token: 0x040041E8 RID: 16872
		public List<StatModifier> statFactors;

		// Token: 0x040041E9 RID: 16873
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x040041EA RID: 16874
		public float marketValueOffset;

		// Token: 0x040041EB RID: 16875
		public float burstShotSpeedMultiplier = 1f;

		// Token: 0x040041EC RID: 16876
		public float burstShotCountMultiplier = 1f;

		// Token: 0x040041ED RID: 16877
		public float additionalStoppingPower;

		// Token: 0x040041EE RID: 16878
		public bool ignoresAccuracyMaluses;

		// Token: 0x040041EF RID: 16879
		public ColorDef forcedColor;

		// Token: 0x040041F0 RID: 16880
		[MustTranslate]
		public List<string> traitAdjectives = new List<string>();

		// Token: 0x040041F1 RID: 16881
		public List<HediffDef> equippedHediffs;

		// Token: 0x040041F2 RID: 16882
		public List<HediffDef> bondedHediffs;

		// Token: 0x040041F3 RID: 16883
		public ThoughtDef bondedThought;

		// Token: 0x040041F4 RID: 16884
		public ThoughtDef killThought;

		// Token: 0x040041F5 RID: 16885
		public bool neverBond;

		// Token: 0x040041F6 RID: 16886
		public CompProperties_EquippableAbilityReloadable abilityProps;

		// Token: 0x040041F7 RID: 16887
		private WeaponTraitWorker worker;
	}
}
