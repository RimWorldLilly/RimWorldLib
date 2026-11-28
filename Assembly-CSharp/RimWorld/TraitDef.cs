using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001007 RID: 4103
	public class TraitDef : Def
	{
		// Token: 0x06006783 RID: 26499 RVA: 0x00249CD8 File Offset: 0x00247ED8
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x00249CE4 File Offset: 0x00247EE4
		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(this.defName + " found no data at degree " + degree.ToString() + ", returning first defined.");
			return this.degreeDatas[0];
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x00249D55 File Offset: 0x00247F55
		public override IEnumerable<string> ConfigErrors()
		{
			TraitDef.<ConfigErrors>d__17 <ConfigErrors>d__ = new TraitDef.<ConfigErrors>d__17(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x00249D68 File Offset: 0x00247F68
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (TraitDegreeData traitDegreeData in this.degreeDatas)
			{
				traitDegreeData.ResolveReferences();
			}
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00249DC0 File Offset: 0x00247FC0
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x00249DD0 File Offset: 0x00247FD0
		public bool ConflictsWith(TraitDef other)
		{
			if ((other.conflictingTraits != null && other.conflictingTraits.Contains(this)) || (this.conflictingTraits != null && this.conflictingTraits.Contains(other)))
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006789 RID: 26505 RVA: 0x00249E4D File Offset: 0x0024804D
		public bool CanSuppress(Trait other)
		{
			return this.ConflictsWith(other) || other.def == this;
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x00249E63 File Offset: 0x00248063
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x00249E7B File Offset: 0x0024807B
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x00249E93 File Offset: 0x00248093
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		// Token: 0x040041A1 RID: 16801
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x040041A2 RID: 16802
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x040041A3 RID: 16803
		public List<string> exclusionTags = new List<string>();

		// Token: 0x040041A4 RID: 16804
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		// Token: 0x040041A5 RID: 16805
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		// Token: 0x040041A6 RID: 16806
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040041A7 RID: 16807
		public WorkTags requiredWorkTags;

		// Token: 0x040041A8 RID: 16808
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040041A9 RID: 16809
		public WorkTags disabledWorkTags;

		// Token: 0x040041AA RID: 16810
		public AnimalType? disableHostilityFromAnimalType;

		// Token: 0x040041AB RID: 16811
		public FactionDef disableHostilityFromFaction;

		// Token: 0x040041AC RID: 16812
		public bool canBeSuppressed = true;

		// Token: 0x040041AD RID: 16813
		private float commonality = 1f;

		// Token: 0x040041AE RID: 16814
		private float commonalityFemale = -1f;

		// Token: 0x040041AF RID: 16815
		public bool allowOnHostileSpawn = true;
	}
}
