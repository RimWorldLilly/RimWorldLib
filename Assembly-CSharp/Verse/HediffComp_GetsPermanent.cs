using System;

namespace Verse
{
	// Token: 0x02000434 RID: 1076
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000BE01F File Offset: 0x000BC21F
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000BE02C File Offset: 0x000BC22C
		// (set) Token: 0x06001F31 RID: 7985 RVA: 0x000BE034 File Offset: 0x000BC234
		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value == this.isPermanentInt)
				{
					return;
				}
				this.isPermanentInt = value;
				if (this.isPermanentInt)
				{
					this.permanentDamageThreshold = 9999f;
					this.SetPainCategory(HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category);
				}
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x000BE099 File Offset: 0x000BC299
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x000BE0A1 File Offset: 0x000BC2A1
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000BE0AA File Offset: 0x000BC2AA
		public void SetPainCategory(PainCategory category)
		{
			this.painCategory = category;
			if (base.Pawn != null)
			{
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000BE0D4 File Offset: 0x000BC2D4
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000BE124 File Offset: 0x000BC324
		public void PreFinalizeInjury()
		{
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				return;
			}
			if (ModsConfig.BiotechActive && this.parent.pawn.genes != null)
			{
				if (this.parent.pawn.genes.GenesListForReading.Any((Gene x) => x.def.preventPermanentWounds))
				{
					return;
				}
			}
			float num = 0.02f * this.parent.Part.def.permanentInjuryChanceFactor * this.Props.becomePermanentChanceFactor;
			if (!this.parent.Part.def.delicate)
			{
				num *= HealthTuning.BecomePermanentChanceFactorBySeverityCurve.Evaluate(this.parent.Severity);
			}
			if (Rand.Chance(num))
			{
				if (this.parent.Part.def.delicate)
				{
					this.IsPermanent = true;
					return;
				}
				this.permanentDamageThreshold = Rand.Range(1f, this.parent.Severity / 2f);
			}
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000BE248 File Offset: 0x000BC448
		public override void CompPostInjuryHeal(float amount)
		{
			if (this.permanentDamageThreshold >= 9999f || this.IsPermanent)
			{
				return;
			}
			if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
			{
				this.parent.Severity = this.permanentDamageThreshold;
				this.IsPermanent = true;
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000BE2C4 File Offset: 0x000BC4C4
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = other as HediffComp_GetsPermanent;
			if (hediffComp_GetsPermanent != null)
			{
				this.isPermanentInt = hediffComp_GetsPermanent.isPermanentInt;
				this.painCategory = hediffComp_GetsPermanent.painCategory;
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000BE2F4 File Offset: 0x000BC4F4
		public override string CompDebugString()
		{
			return string.Concat(new string[]
			{
				"isPermanent: ",
				this.isPermanentInt.ToString(),
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold.ToString(),
				"\npainCategory: ",
				this.painCategory.ToString()
			});
		}

		// Token: 0x0400185F RID: 6239
		public float permanentDamageThreshold = 9999f;

		// Token: 0x04001860 RID: 6240
		public bool isPermanentInt;

		// Token: 0x04001861 RID: 6241
		private PainCategory painCategory;

		// Token: 0x04001862 RID: 6242
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}
