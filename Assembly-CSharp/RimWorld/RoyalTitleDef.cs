using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFF RID: 8191
	public class RoyalTitleDef : Def
	{
		// Token: 0x1700255F RID: 9567
		// (get) Token: 0x0600CF72 RID: 53106 RVA: 0x004C6181 File Offset: 0x004C4381
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		// Token: 0x17002560 RID: 9568
		// (get) Token: 0x0600CF73 RID: 53107 RVA: 0x004C618C File Offset: 0x004C438C
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				RoyalTitleDef.<get_DisabledWorkTypes>d__39 <get_DisabledWorkTypes>d__ = new RoyalTitleDef.<get_DisabledWorkTypes>d__39(-2);
				<get_DisabledWorkTypes>d__.<>4__this = this;
				return <get_DisabledWorkTypes>d__;
			}
		}

		// Token: 0x17002561 RID: 9569
		// (get) Token: 0x0600CF74 RID: 53108 RVA: 0x004C619C File Offset: 0x004C439C
		public RoyalTitleInheritanceWorker InheritanceWorkerOverride
		{
			get
			{
				if (this.inheritanceWorkerOverride == null && this.inheritanceWorkerOverrideClass != null)
				{
					this.inheritanceWorkerOverride = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.inheritanceWorkerOverrideClass);
				}
				return this.inheritanceWorkerOverride;
			}
		}

		// Token: 0x0600CF75 RID: 53109 RVA: 0x004C61D0 File Offset: 0x004C43D0
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		// Token: 0x17002562 RID: 9570
		// (get) Token: 0x0600CF76 RID: 53110 RVA: 0x004C61F4 File Offset: 0x004C43F4
		public float MinThroneRoomImpressiveness
		{
			get
			{
				if (this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return 0f;
				}
				RoomRequirement_Impressiveness roomRequirement_Impressiveness = this.throneRoomRequirements.OfType<RoomRequirement_Impressiveness>().FirstOrDefault<RoomRequirement_Impressiveness>();
				if (roomRequirement_Impressiveness == null)
				{
					return 0f;
				}
				return (float)roomRequirement_Impressiveness.impressiveness;
			}
		}

		// Token: 0x17002563 RID: 9571
		// (get) Token: 0x0600CF77 RID: 53111 RVA: 0x004C6235 File Offset: 0x004C4435
		public RoyalTitleAwardWorker AwardWorker
		{
			get
			{
				if (this.awardWorker == null)
				{
					this.awardWorker = (RoyalTitleAwardWorker)Activator.CreateInstance(this.awardWorkerClass);
					this.awardWorker.def = this;
				}
				return this.awardWorker;
			}
		}

		// Token: 0x0600CF78 RID: 53112 RVA: 0x004C6267 File Offset: 0x004C4467
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		// Token: 0x0600CF79 RID: 53113 RVA: 0x004C627F File Offset: 0x004C447F
		public string GetLabelFor(Gender g)
		{
			if (g != Gender.Female)
			{
				return this.label;
			}
			if (string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label;
			}
			return this.labelFemale;
		}

		// Token: 0x0600CF7A RID: 53114 RVA: 0x004C62A6 File Offset: 0x004C44A6
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x0600CF7B RID: 53115 RVA: 0x004C62D2 File Offset: 0x004C44D2
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return this.LabelCap;
		}

		// Token: 0x0600CF7C RID: 53116 RVA: 0x004C6312 File Offset: 0x004C4512
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		// Token: 0x0600CF7D RID: 53117 RVA: 0x004C6321 File Offset: 0x004C4521
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		// Token: 0x0600CF7E RID: 53118 RVA: 0x004C6342 File Offset: 0x004C4542
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		// Token: 0x0600CF7F RID: 53119 RVA: 0x004C635B File Offset: 0x004C455B
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		// Token: 0x0600CF80 RID: 53120 RVA: 0x004C6374 File Offset: 0x004C4574
		private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
		{
			if (list == null)
			{
				return false;
			}
			using (List<RoomRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SameOrSubsetOf(otherReq))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CF81 RID: 53121 RVA: 0x004C63D0 File Offset: 0x004C45D0
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		// Token: 0x0600CF82 RID: 53122 RVA: 0x004C63DF File Offset: 0x004C45DF
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		// Token: 0x0600CF83 RID: 53123 RVA: 0x004C63F0 File Offset: 0x004C45F0
		public int MaxAllowedPsylinkLevel(FactionDef faction)
		{
			int result = 0;
			for (int i = 0; i < faction.royalImplantRules.Count; i++)
			{
				RoyalImplantRule royalImplantRule = faction.royalImplantRules[i];
				if (royalImplantRule.implantHediff == HediffDefOf.PsychicAmplifier && royalImplantRule.minTitle.Awardable && royalImplantRule.minTitle.seniority <= this.seniority)
				{
					result = royalImplantRule.maxLevel;
				}
			}
			return result;
		}

		// Token: 0x0600CF84 RID: 53124 RVA: 0x004C6458 File Offset: 0x004C4658
		public IEnumerable<ThingDef> SatisfyingMeals(bool includeDrugs = true)
		{
			if (includeDrugs)
			{
				if (this.satisfyingMealsCached == null)
				{
					this.satisfyingMealsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
					where this.foodRequirement.Acceptable(t)
					orderby t.ingestible.preferability descending
					select t).ToList<ThingDef>();
				}
			}
			else if (this.satisfyingMealsNoDrugsCached == null)
			{
				this.satisfyingMealsNoDrugsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
				where this.foodRequirement.Acceptable(t) && !t.IsDrug
				orderby t.ingestible.preferability descending
				select t).ToList<ThingDef>();
			}
			if (!includeDrugs)
			{
				return this.satisfyingMealsNoDrugsCached;
			}
			return this.satisfyingMealsCached;
		}

		// Token: 0x0600CF85 RID: 53125 RVA: 0x004C6517 File Offset: 0x004C4717
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			RoyalTitleDef.<SpecialDisplayStats>d__61 <SpecialDisplayStats>d__ = new RoyalTitleDef.<SpecialDisplayStats>d__61(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600CF86 RID: 53126 RVA: 0x004C652E File Offset: 0x004C472E
		private IEnumerable<string> RequiredApparelListForGender(Gender g, Pawn forPawn = null)
		{
			RoyalTitleDef.<RequiredApparelListForGender>d__62 <RequiredApparelListForGender>d__ = new RoyalTitleDef.<RequiredApparelListForGender>d__62(-2);
			<RequiredApparelListForGender>d__.<>4__this = this;
			<RequiredApparelListForGender>d__.<>3__g = g;
			<RequiredApparelListForGender>d__.<>3__forPawn = forPawn;
			return <RequiredApparelListForGender>d__;
		}

		// Token: 0x0600CF87 RID: 53127 RVA: 0x004C654C File Offset: 0x004C474C
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		// Token: 0x0600CF88 RID: 53128 RVA: 0x004C65AB File Offset: 0x004C47AB
		public override IEnumerable<string> ConfigErrors()
		{
			RoyalTitleDef.<ConfigErrors>d__64 <ConfigErrors>d__ = new RoyalTitleDef.<ConfigErrors>d__64(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040083F3 RID: 33779
		public int seniority;

		// Token: 0x040083F4 RID: 33780
		public int favorCost;

		// Token: 0x040083F5 RID: 33781
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040083F6 RID: 33782
		public int changeHeirQuestPoints = -1;

		// Token: 0x040083F7 RID: 33783
		public float commonality = 1f;

		// Token: 0x040083F8 RID: 33784
		public WorkTags disabledWorkTags;

		// Token: 0x040083F9 RID: 33785
		public Type inheritanceWorkerOverrideClass;

		// Token: 0x040083FA RID: 33786
		public QualityCategory requiredMinimumApparelQuality;

		// Token: 0x040083FB RID: 33787
		public List<ApparelRequirement> requiredApparel;

		// Token: 0x040083FC RID: 33788
		public List<RoyalTitlePermitDef> permits;

		// Token: 0x040083FD RID: 33789
		public ExpectationDef minExpectation;

		// Token: 0x040083FE RID: 33790
		public List<JoyKindDef> disabledJoyKinds;

		// Token: 0x040083FF RID: 33791
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04008400 RID: 33792
		public List<ThingDefCountClass> rewards;

		// Token: 0x04008401 RID: 33793
		public bool suppressIdleAlert;

		// Token: 0x04008402 RID: 33794
		public bool canBeInherited;

		// Token: 0x04008403 RID: 33795
		public bool allowDignifiedMeditationFocus = true;

		// Token: 0x04008404 RID: 33796
		public int permitPointsAwarded;

		// Token: 0x04008405 RID: 33797
		public Type awardWorkerClass = typeof(RoyalTitleAwardWorker);

		// Token: 0x04008406 RID: 33798
		public ThoughtDef awardThought;

		// Token: 0x04008407 RID: 33799
		public ThoughtDef lostThought;

		// Token: 0x04008408 RID: 33800
		public List<RoomRequirement> throneRoomRequirements;

		// Token: 0x04008409 RID: 33801
		public List<RoomRequirement> bedroomRequirements;

		// Token: 0x0400840A RID: 33802
		public float recruitmentResistanceOffset;

		// Token: 0x0400840B RID: 33803
		public RoyalTitleFoodRequirement foodRequirement;

		// Token: 0x0400840C RID: 33804
		public RoyalTitleDef replaceOnRecruited;

		// Token: 0x0400840D RID: 33805
		public float decreeMtbDays = -1f;

		// Token: 0x0400840E RID: 33806
		public float decreeMinIntervalDays = 2f;

		// Token: 0x0400840F RID: 33807
		public float decreeMentalBreakCommonality;

		// Token: 0x04008410 RID: 33808
		public List<string> decreeTags;

		// Token: 0x04008411 RID: 33809
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		// Token: 0x04008412 RID: 33810
		public IntRange speechCooldown;

		// Token: 0x04008413 RID: 33811
		public int maxPsylinkLevel;

		// Token: 0x04008414 RID: 33812
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		// Token: 0x04008415 RID: 33813
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		// Token: 0x04008416 RID: 33814
		private RoyalTitleAwardWorker awardWorker;

		// Token: 0x04008417 RID: 33815
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;
	}
}
