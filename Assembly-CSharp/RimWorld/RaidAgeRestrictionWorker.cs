using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCF RID: 4047
	public class RaidAgeRestrictionWorker
	{
		// Token: 0x060066AD RID: 26285 RVA: 0x00247064 File Offset: 0x00245264
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return GenDate.DaysPassedSinceSettle >= this.def.earliestDay && parms.faction != null && parms.faction.def.humanlikeFaction && !parms.faction.def.disallowedRaidAgeRestrictions.NotNullAndContains(this.def);
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x002470BD File Offset: 0x002452BD
		public bool ShouldApplyToKind(PawnKindDef kind)
		{
			return kind.RaceProps.Humanlike;
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x002470CC File Offset: 0x002452CC
		public bool CanUseKind(PawnKindDef kind)
		{
			if (this.def.ageRange != null)
			{
				FloatRange value = this.def.ageRange.Value;
				if ((float)kind.minGenerationAge > value.max)
				{
					return false;
				}
				if ((float)kind.maxGenerationAge < value.min)
				{
					return false;
				}
			}
			if (kind.apparelRequired != null)
			{
				for (int i = 0; i < kind.apparelRequired.Count; i++)
				{
					if (!kind.apparelRequired[i].apparel.developmentalStageFilter.Has(this.def.developmentStage))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04003FC9 RID: 16329
		public RaidAgeRestrictionDef def;
	}
}
