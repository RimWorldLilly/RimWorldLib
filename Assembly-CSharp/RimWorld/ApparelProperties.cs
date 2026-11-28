using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA4 RID: 3748
	public class ApparelProperties : IRenderNodePropertiesParent
	{
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060062A7 RID: 25255 RVA: 0x00238BA8 File Offset: 0x00236DA8
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060062A8 RID: 25256 RVA: 0x00238BB8 File Offset: 0x00236DB8
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060062A9 RID: 25257 RVA: 0x00238BC0 File Offset: 0x00236DC0
		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937);
				return ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060062AA RID: 25258 RVA: 0x00238C00 File Offset: 0x00236E00
		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		// Token: 0x060062AB RID: 25259 RVA: 0x00238C6F File Offset: 0x00236E6F
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || wearerGender == Gender.None || this.gender == wearerGender;
		}

		// Token: 0x060062AC RID: 25260 RVA: 0x00238C87 File Offset: 0x00236E87
		public bool CorrectAgeForWearing(Pawn pawn)
		{
			return !ModsConfig.BiotechActive || this.developmentalStageFilter.Has(pawn.DevelopmentalStage);
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x00238CA6 File Offset: 0x00236EA6
		public bool PawnCanWear(Gender gender = Gender.None, DevelopmentalStage developmentalStage = DevelopmentalStage.Adult)
		{
			return this.CorrectGenderForWearing(gender) && this.developmentalStageFilter.Has(developmentalStage);
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x00238CC4 File Offset: 0x00236EC4
		public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
		{
			return this.PawnCanWear(ignoreGender ? Gender.None : pawn.gender, pawn.DevelopmentalStage);
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x00238CE0 File Offset: 0x00236EE0
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x060062B0 RID: 25264 RVA: 0x00238D49 File Offset: 0x00236F49
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			ApparelProperties.<ConfigErrors>d__56 <ConfigErrors>d__ = new ApparelProperties.<ConfigErrors>d__56(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x060062B1 RID: 25265 RVA: 0x00238D60 File Offset: 0x00236F60
		public void PostLoadSpecial(ThingDef thingDef)
		{
		}

		// Token: 0x060062B2 RID: 25266 RVA: 0x00238D64 File Offset: 0x00236F64
		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062B3 RID: 25267 RVA: 0x00238DA3 File Offset: 0x00236FA3
		public bool CoversBodyPartGroup(BodyPartGroupDef group)
		{
			return this.bodyPartGroups.NotNullAndContains(group);
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x00238DB4 File Offset: 0x00236FB4
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x060062B5 RID: 25269 RVA: 0x00238E0D File Offset: 0x0023700D
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x060062B6 RID: 25270 RVA: 0x00238E48 File Offset: 0x00237048
		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartGroupDef[] array = (from bpgd in (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>().SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		// Token: 0x060062B7 RID: 25271 RVA: 0x00238F18 File Offset: 0x00237118
		public void ResolveReferencesSpecial()
		{
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x04003654 RID: 13908
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04003655 RID: 13909
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x04003656 RID: 13910
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x04003657 RID: 13911
		[NoTranslate]
		public List<string> wornGraphicPaths;

		// Token: 0x04003658 RID: 13912
		public WornGraphicData wornGraphicData;

		// Token: 0x04003659 RID: 13913
		public bool useWornGraphicMask;

		// Token: 0x0400365A RID: 13914
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x0400365B RID: 13915
		public List<RenderSkipFlagDef> renderSkipFlags;

		// Token: 0x0400365C RID: 13916
		public DrawData drawData;

		// Token: 0x0400365D RID: 13917
		public PawnRenderNodeTagDef parentTagDef;

		// Token: 0x0400365E RID: 13918
		public List<int> forceEyesVisibleForRotations = new List<int>();

		// Token: 0x0400365F RID: 13919
		public bool shellRenderedBehindHead;

		// Token: 0x04003660 RID: 13920
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04003661 RID: 13921
		[NoTranslate]
		public List<string> defaultOutfitTags;

		// Token: 0x04003662 RID: 13922
		public bool canBeGeneratedToSatisfyWarmth = true;

		// Token: 0x04003663 RID: 13923
		public bool canBeGeneratedToSatisfyVacuumResistance = true;

		// Token: 0x04003664 RID: 13924
		public bool canBeGeneratedToSatisfyToxicEnvironmentResistance = true;

		// Token: 0x04003665 RID: 13925
		public bool canBeDesiredForIdeo = true;

		// Token: 0x04003666 RID: 13926
		public List<string> ideoDesireAllowedFactionCategoryTags;

		// Token: 0x04003667 RID: 13927
		public List<string> ideoDesireDisallowedFactionCategoryTags;

		// Token: 0x04003668 RID: 13928
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x04003669 RID: 13929
		public float wearPerDay = 0.4f;

		// Token: 0x0400366A RID: 13930
		public bool careIfWornByCorpse = true;

		// Token: 0x0400366B RID: 13931
		public bool careIfDamaged = true;

		// Token: 0x0400366C RID: 13932
		public bool ignoredByNonViolent;

		// Token: 0x0400366D RID: 13933
		public bool ai_pickUpOpportunistically;

		// Token: 0x0400366E RID: 13934
		public bool blocksAddedPartWoundGraphics;

		// Token: 0x0400366F RID: 13935
		public bool blocksVision;

		// Token: 0x04003670 RID: 13936
		public bool immuneToToxGasExposure;

		// Token: 0x04003671 RID: 13937
		public bool slaveApparel;

		// Token: 0x04003672 RID: 13938
		public bool mechanitorApparel;

		// Token: 0x04003673 RID: 13939
		public bool legsNakedUnlessCoveredBySomethingElse;

		// Token: 0x04003674 RID: 13940
		public bool useDeflectMetalEffect;

		// Token: 0x04003675 RID: 13941
		public bool countsAsClothingForNudity = true;

		// Token: 0x04003676 RID: 13942
		public bool anyTechLevelCanUseForWarmth;

		// Token: 0x04003677 RID: 13943
		public Gender gender;

		// Token: 0x04003678 RID: 13944
		public float scoreOffset;

		// Token: 0x04003679 RID: 13945
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Adult;

		// Token: 0x0400367A RID: 13946
		public SoundDef soundWear;

		// Token: 0x0400367B RID: 13947
		public SoundDef soundRemove;

		// Token: 0x0400367C RID: 13948
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x0400367D RID: 13949
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		// Token: 0x0400367E RID: 13950
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
