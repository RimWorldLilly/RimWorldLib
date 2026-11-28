using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC7 RID: 4039
	public class PreceptApparelRequirement : IExposable
	{
		// Token: 0x06006682 RID: 26242 RVA: 0x00246244 File Offset: 0x00244444
		public bool CanAddRequirement(Precept precept, List<PreceptApparelRequirement> currentRequirements, out string cannotAddReason, FactionDef generatingFor = null)
		{
			AcceptanceReport report = this.Compatible(precept.ideo, generatingFor);
			if (!report)
			{
				if (!string.IsNullOrWhiteSpace(report.Reason))
				{
					cannotAddReason = report.Reason;
				}
				else
				{
					cannotAddReason = null;
				}
				return false;
			}
			if (this.requirement.AllRequiredApparel(Gender.None).Any((ThingDef x) => currentRequirements.Any((PreceptApparelRequirement y) => y.requirement.AllRequiredApparel(Gender.None).Contains(x))))
			{
				cannotAddReason = "AlreadyAssigned".Translate();
				return false;
			}
			cannotAddReason = null;
			return true;
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x002462CC File Offset: 0x002444CC
		public bool CanWearTogetherWith(ThingDef apparel)
		{
			foreach (ThingDef thingDef in this.requirement.AllRequiredApparel(Gender.None))
			{
				if (ApparelUtility.CanWearTogether(thingDef, apparel, BodyDefOf.Human) || thingDef == apparel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x00246334 File Offset: 0x00244534
		public bool RequirementOverlapsOther(List<PreceptApparelRequirement> currentRequirements, out string compatibilityReason)
		{
			foreach (IEnumerable<ThingDef> enumerable in from x in currentRequirements.Except(this)
			select x.requirement.AllRequiredApparel(Gender.None))
			{
				foreach (ThingDef a in this.requirement.AllRequiredApparel(Gender.None))
				{
					foreach (ThingDef thingDef in enumerable)
					{
						if (!ApparelUtility.CanWearTogether(a, thingDef, BodyDefOf.Human))
						{
							compatibilityReason = "CannotBeWornWith".Translate(thingDef);
							return true;
						}
					}
				}
			}
			compatibilityReason = null;
			return false;
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x0024644C File Offset: 0x0024464C
		public AcceptanceReport Compatible(Ideo ideo, FactionDef forFaction)
		{
			if (!this.requirement.IsValid)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = this.<Compatible>g__CheckFaction|7_0(forFaction);
			if (!acceptanceReport.Accepted)
			{
				return acceptanceReport;
			}
			if (Find.World != null && Find.FactionManager != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def != forFaction && faction.ideos != null && faction.ideos.PrimaryIdeo == ideo)
					{
						acceptanceReport = this.<Compatible>g__CheckFaction|7_0(faction.def);
						if (!acceptanceReport)
						{
							return acceptanceReport;
						}
					}
				}
			}
			if (this.anyMemeRequired != null && !ideo.memes.Any((MemeDef m) => this.anyMemeRequired.Contains(m)))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleAnyMemeRequired".Translate() + ": " + (from m in this.anyMemeRequired
				select m.label).ToCommaListOr(false).CapitalizeFirst());
			}
			return true;
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x00246588 File Offset: 0x00244788
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.allowedFactionCategoryTags, "allowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.disallowedFactionCategoryTags, "disallowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<MemeDef>(ref this.anyMemeRequired, "anyMemeRequired", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<ApparelRequirement>(ref this.requirement, "requirement", Array.Empty<object>());
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x002465EC File Offset: 0x002447EC
		public virtual void CopyTo(PreceptApparelRequirement other)
		{
			if (this.allowedFactionCategoryTags != null)
			{
				if (other.allowedFactionCategoryTags == null)
				{
					other.allowedFactionCategoryTags = new List<string>();
				}
				other.allowedFactionCategoryTags.Clear();
				other.allowedFactionCategoryTags.AddRange(this.allowedFactionCategoryTags);
			}
			else
			{
				other.allowedFactionCategoryTags = null;
			}
			if (this.disallowedFactionCategoryTags != null)
			{
				if (other.disallowedFactionCategoryTags == null)
				{
					other.disallowedFactionCategoryTags = new List<string>();
				}
				other.disallowedFactionCategoryTags.Clear();
				other.disallowedFactionCategoryTags.AddRange(this.disallowedFactionCategoryTags);
			}
			else
			{
				other.disallowedFactionCategoryTags = null;
			}
			if (this.anyMemeRequired != null)
			{
				if (other.anyMemeRequired == null)
				{
					other.anyMemeRequired = new List<MemeDef>();
				}
				other.anyMemeRequired.Clear();
				other.anyMemeRequired.AddRange(this.anyMemeRequired);
			}
			else
			{
				other.anyMemeRequired = null;
			}
			if (this.requirement != null)
			{
				ApparelRequirement other2 = new ApparelRequirement();
				this.requirement.CopyTo(other2);
				other.requirement = other2;
				return;
			}
			other.requirement = null;
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x002466EC File Offset: 0x002448EC
		[CompilerGenerated]
		private AcceptanceReport <Compatible>g__CheckFaction|7_0(FactionDef faction)
		{
			if (faction != null && this.allowedFactionCategoryTags != null && !this.allowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in this.allowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Allowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			if (faction != null && this.disallowedFactionCategoryTags != null && this.disallowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in this.disallowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Disallowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			return true;
		}

		// Token: 0x04003F0D RID: 16141
		public List<string> allowedFactionCategoryTags;

		// Token: 0x04003F0E RID: 16142
		public List<string> disallowedFactionCategoryTags;

		// Token: 0x04003F0F RID: 16143
		public List<MemeDef> anyMemeRequired;

		// Token: 0x04003F10 RID: 16144
		public ApparelRequirement requirement;
	}
}
