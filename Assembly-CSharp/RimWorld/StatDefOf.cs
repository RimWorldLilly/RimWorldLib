using System;

namespace RimWorld
{
	// Token: 0x02001E9B RID: 7835
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x0600CA56 RID: 51798 RVA: 0x004A5E78 File Offset: 0x004A4078
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x04007BFD RID: 31741
		public static StatDef MaxHitPoints;

		// Token: 0x04007BFE RID: 31742
		public static StatDef MarketValue;

		// Token: 0x04007BFF RID: 31743
		public static StatDef MarketValueIgnoreHp;

		// Token: 0x04007C00 RID: 31744
		public static StatDef RoyalFavorValue;

		// Token: 0x04007C01 RID: 31745
		public static StatDef SellPriceFactor;

		// Token: 0x04007C02 RID: 31746
		public static StatDef Beauty;

		// Token: 0x04007C03 RID: 31747
		public static StatDef BeautyOutdoors;

		// Token: 0x04007C04 RID: 31748
		public static StatDef Cleanliness;

		// Token: 0x04007C05 RID: 31749
		public static StatDef Flammability;

		// Token: 0x04007C06 RID: 31750
		public static StatDef DeteriorationRate;

		// Token: 0x04007C07 RID: 31751
		public static StatDef WorkToMake;

		// Token: 0x04007C08 RID: 31752
		public static StatDef WorkToBuild;

		// Token: 0x04007C09 RID: 31753
		public static StatDef Mass;

		// Token: 0x04007C0A RID: 31754
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x04007C0B RID: 31755
		public static StatDef Nutrition;

		// Token: 0x04007C0C RID: 31756
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x04007C0D RID: 31757
		public static StatDef MoveSpeed;

		// Token: 0x04007C0E RID: 31758
		public static StatDef CrawlSpeed;

		// Token: 0x04007C0F RID: 31759
		public static StatDef GlobalLearningFactor;

		// Token: 0x04007C10 RID: 31760
		public static StatDef BedHungerRateFactor;

		// Token: 0x04007C11 RID: 31761
		public static StatDef RestRateMultiplier;

		// Token: 0x04007C12 RID: 31762
		public static StatDef PsychicSensitivity;

		// Token: 0x04007C13 RID: 31763
		public static StatDef ToxicResistance;

		// Token: 0x04007C14 RID: 31764
		public static StatDef MentalBreakThreshold;

		// Token: 0x04007C15 RID: 31765
		public static StatDef EatingSpeed;

		// Token: 0x04007C16 RID: 31766
		public static StatDef ComfyTemperatureMin;

		// Token: 0x04007C17 RID: 31767
		public static StatDef ComfyTemperatureMax;

		// Token: 0x04007C18 RID: 31768
		public static StatDef Comfort;

		// Token: 0x04007C19 RID: 31769
		public static StatDef MeatAmount;

		// Token: 0x04007C1A RID: 31770
		public static StatDef LeatherAmount;

		// Token: 0x04007C1B RID: 31771
		public static StatDef MinimumHandlingSkill;

		// Token: 0x04007C1C RID: 31772
		public static StatDef MeleeDPS;

		// Token: 0x04007C1D RID: 31773
		public static StatDef PainShockThreshold;

		// Token: 0x04007C1E RID: 31774
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x04007C1F RID: 31775
		public static StatDef FilthRate;

		// Token: 0x04007C20 RID: 31776
		public static StatDef MaxNutrition;

		// Token: 0x04007C21 RID: 31777
		public static StatDef LifespanFactor;

		// Token: 0x04007C22 RID: 31778
		public static StatDef MaxFlightTime;

		// Token: 0x04007C23 RID: 31779
		public static StatDef FlightCooldown;

		// Token: 0x04007C24 RID: 31780
		public static StatDef Wildness;

		// Token: 0x04007C25 RID: 31781
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyMax;

		// Token: 0x04007C26 RID: 31782
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyRecoveryRate;

		// Token: 0x04007C27 RID: 31783
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyGain;

		// Token: 0x04007C28 RID: 31784
		[MayRequireRoyalty]
		public static StatDef MeditationFocusGain;

		// Token: 0x04007C29 RID: 31785
		[MayRequireIdeology]
		public static StatDef CertaintyLossFactor;

		// Token: 0x04007C2A RID: 31786
		[MayRequireIdeology]
		public static StatDef SocialIdeoSpreadFrequencyFactor;

		// Token: 0x04007C2B RID: 31787
		[MayRequireIdeology]
		public static StatDef SuppressionPower;

		// Token: 0x04007C2C RID: 31788
		[MayRequireIdeology]
		public static StatDef SlaveSuppressionFallRate;

		// Token: 0x04007C2D RID: 31789
		public static StatDef AnimalsLearningFactor;

		// Token: 0x04007C2E RID: 31790
		public static StatDef BondAnimalChanceFactor;

		// Token: 0x04007C2F RID: 31791
		public static StatDef CaravanRidingSpeedFactor;

		// Token: 0x04007C30 RID: 31792
		public static StatDef CaravanBonusSpeedFactor;

		// Token: 0x04007C31 RID: 31793
		public static StatDef RestFallRateFactor;

		// Token: 0x04007C32 RID: 31794
		public static StatDef JoyFallRateFactor;

		// Token: 0x04007C33 RID: 31795
		[MayRequireIdeology]
		public static StatDef BiosculpterOccupantSpeed;

		// Token: 0x04007C34 RID: 31796
		[MayRequireBiotech]
		public static StatDef MechEnergyUsageFactor;

		// Token: 0x04007C35 RID: 31797
		public static StatDef MeleeDamageFactor;

		// Token: 0x04007C36 RID: 31798
		public static StatDef MeleeCooldownFactor;

		// Token: 0x04007C37 RID: 31799
		public static StatDef ToxicEnvironmentResistance;

		// Token: 0x04007C38 RID: 31800
		[MayRequireBiotech]
		public static StatDef HemogenGainFactor;

		// Token: 0x04007C39 RID: 31801
		[MayRequireBiotech]
		public static StatDef RawNutritionFactor;

		// Token: 0x04007C3A RID: 31802
		[MayRequireBiotech]
		public static StatDef CancerRate;

		// Token: 0x04007C3B RID: 31803
		[MayRequireBiotech]
		public static StatDef WastepacksPerRecharge;

		// Token: 0x04007C3C RID: 31804
		[MayRequireBiotech]
		public static StatDef MechEnergyLossPerHP;

		// Token: 0x04007C3D RID: 31805
		[MayRequireBiotech]
		public static StatDef LearningRateFactor;

		// Token: 0x04007C3E RID: 31806
		[MayRequireBiotech]
		public static StatDef GrowthVatOccupantSpeed;

		// Token: 0x04007C3F RID: 31807
		[MayRequireAnomaly]
		public static StatDef MinimumContainmentStrength;

		// Token: 0x04007C40 RID: 31808
		[MayRequireAnomaly]
		public static StatDef PsychicRitualQualityOffset;

		// Token: 0x04007C41 RID: 31809
		[MayRequireOdyssey]
		public static StatDef VacuumResistance;

		// Token: 0x04007C42 RID: 31810
		[MayRequireOdyssey]
		public static StatDef PilotingAbility;

		// Token: 0x04007C43 RID: 31811
		public static StatDef WorkSpeedGlobal;

		// Token: 0x04007C44 RID: 31812
		public static StatDef MiningSpeed;

		// Token: 0x04007C45 RID: 31813
		public static StatDef DeepDrillingSpeed;

		// Token: 0x04007C46 RID: 31814
		public static StatDef MiningYield;

		// Token: 0x04007C47 RID: 31815
		public static StatDef ResearchSpeed;

		// Token: 0x04007C48 RID: 31816
		public static StatDef ConstructionSpeed;

		// Token: 0x04007C49 RID: 31817
		public static StatDef HuntingStealth;

		// Token: 0x04007C4A RID: 31818
		public static StatDef PlantWorkSpeed;

		// Token: 0x04007C4B RID: 31819
		public static StatDef SmoothingSpeed;

		// Token: 0x04007C4C RID: 31820
		public static StatDef FoodPoisonChance;

		// Token: 0x04007C4D RID: 31821
		public static StatDef CarryingCapacity;

		// Token: 0x04007C4E RID: 31822
		public static StatDef PlantHarvestYield;

		// Token: 0x04007C4F RID: 31823
		public static StatDef DrugHarvestYield;

		// Token: 0x04007C50 RID: 31824
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x04007C51 RID: 31825
		public static StatDef ConstructSuccessChance;

		// Token: 0x04007C52 RID: 31826
		public static StatDef GeneralLaborSpeed;

		// Token: 0x04007C53 RID: 31827
		public static StatDef CleaningSpeed;

		// Token: 0x04007C54 RID: 31828
		public static StatDef ReadingSpeed;

		// Token: 0x04007C55 RID: 31829
		public static StatDef HackingSpeed;

		// Token: 0x04007C56 RID: 31830
		public static StatDef HackingStealth;

		// Token: 0x04007C57 RID: 31831
		[MayRequireIdeology]
		public static StatDef PruningSpeed;

		// Token: 0x04007C58 RID: 31832
		[MayRequireBiotech]
		public static StatDef MechBandwidth;

		// Token: 0x04007C59 RID: 31833
		[MayRequireBiotech]
		public static StatDef MechRemoteRepairDistance;

		// Token: 0x04007C5A RID: 31834
		[MayRequireBiotech]
		public static StatDef MechRemoteShieldEnergy;

		// Token: 0x04007C5B RID: 31835
		[MayRequireBiotech]
		public static StatDef MechControlGroups;

		// Token: 0x04007C5C RID: 31836
		[MayRequireBiotech]
		public static StatDef BandwidthCost;

		// Token: 0x04007C5D RID: 31837
		[MayRequireBiotech]
		public static StatDef MechRepairSpeed;

		// Token: 0x04007C5E RID: 31838
		[MayRequireBiotech]
		public static StatDef ControlTakingTime;

		// Token: 0x04007C5F RID: 31839
		[MayRequireAnomaly]
		public static StatDef EntityStudyRate;

		// Token: 0x04007C60 RID: 31840
		[MayRequireAnomaly]
		public static StatDef ActivitySuppressionRate;

		// Token: 0x04007C61 RID: 31841
		[MayRequireAnomaly]
		public static StatDef StudyEfficiency;

		// Token: 0x04007C62 RID: 31842
		[MayRequireOdyssey]
		public static StatDef FishingSpeed;

		// Token: 0x04007C63 RID: 31843
		[MayRequireOdyssey]
		public static StatDef FishingYield;

		// Token: 0x04007C64 RID: 31844
		public static StatDef MedicalTendSpeed;

		// Token: 0x04007C65 RID: 31845
		public static StatDef MedicalTendQuality;

		// Token: 0x04007C66 RID: 31846
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x04007C67 RID: 31847
		public static StatDef NegotiationAbility;

		// Token: 0x04007C68 RID: 31848
		public static StatDef ArrestSuccessChance;

		// Token: 0x04007C69 RID: 31849
		public static StatDef TradePriceImprovement;

		// Token: 0x04007C6A RID: 31850
		public static StatDef DrugSellPriceImprovement;

		// Token: 0x04007C6B RID: 31851
		public static StatDef SocialImpact;

		// Token: 0x04007C6C RID: 31852
		public static StatDef PawnBeauty;

		// Token: 0x04007C6D RID: 31853
		[MayRequireIdeology]
		public static StatDef AnimalProductsSellImprovement;

		// Token: 0x04007C6E RID: 31854
		[MayRequireIdeology]
		public static StatDef ConversionPower;

		// Token: 0x04007C6F RID: 31855
		[MayRequireBiotech]
		public static StatDef BabyPlayGainFactor;

		// Token: 0x04007C70 RID: 31856
		[MayRequireBiotech]
		public static StatDef Fertility;

		// Token: 0x04007C71 RID: 31857
		public static StatDef AnimalGatherSpeed;

		// Token: 0x04007C72 RID: 31858
		public static StatDef AnimalGatherYield;

		// Token: 0x04007C73 RID: 31859
		public static StatDef TameAnimalChance;

		// Token: 0x04007C74 RID: 31860
		public static StatDef TrainAnimalChance;

		// Token: 0x04007C75 RID: 31861
		public static StatDef ShootingAccuracyPawn;

		// Token: 0x04007C76 RID: 31862
		public static StatDef ShootingAccuracyTurret;

		// Token: 0x04007C77 RID: 31863
		public static StatDef MortarMissRadiusFactor;

		// Token: 0x04007C78 RID: 31864
		public static StatDef AimingDelayFactor;

		// Token: 0x04007C79 RID: 31865
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsDarkOffset;

		// Token: 0x04007C7A RID: 31866
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsLitOffset;

		// Token: 0x04007C7B RID: 31867
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsDarkOffset;

		// Token: 0x04007C7C RID: 31868
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsLitOffset;

		// Token: 0x04007C7D RID: 31869
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsDarkOffset;

		// Token: 0x04007C7E RID: 31870
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsLitOffset;

		// Token: 0x04007C7F RID: 31871
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsDarkOffset;

		// Token: 0x04007C80 RID: 31872
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsLitOffset;

		// Token: 0x04007C81 RID: 31873
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsDarkOffset;

		// Token: 0x04007C82 RID: 31874
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsLitOffset;

		// Token: 0x04007C83 RID: 31875
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsDarkOffset;

		// Token: 0x04007C84 RID: 31876
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsLitOffset;

		// Token: 0x04007C85 RID: 31877
		[MayRequireBiotech]
		public static StatDef StaggerDurationFactor;

		// Token: 0x04007C86 RID: 31878
		public static StatDef RangedCooldownFactor;

		// Token: 0x04007C87 RID: 31879
		public static StatDef MeleeHitChance;

		// Token: 0x04007C88 RID: 31880
		public static StatDef MeleeDodgeChance;

		// Token: 0x04007C89 RID: 31881
		[MayRequireBiotech]
		public static StatDef MeleeDoorDamageFactor;

		// Token: 0x04007C8A RID: 31882
		public static StatDef PawnTrapSpringChance;

		// Token: 0x04007C8B RID: 31883
		public static StatDef IncomingDamageFactor;

		// Token: 0x04007C8C RID: 31884
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x04007C8D RID: 31885
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x04007C8E RID: 31886
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x04007C8F RID: 31887
		public static StatDef SharpDamageMultiplier;

		// Token: 0x04007C90 RID: 31888
		public static StatDef BluntDamageMultiplier;

		// Token: 0x04007C91 RID: 31889
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x04007C92 RID: 31890
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x04007C93 RID: 31891
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x04007C94 RID: 31892
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x04007C95 RID: 31893
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x04007C96 RID: 31894
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x04007C97 RID: 31895
		public static StatDef RangedWeapon_DamageMultiplier;

		// Token: 0x04007C98 RID: 31896
		public static StatDef RangedWeapon_ArmorPenetrationMultiplier;

		// Token: 0x04007C99 RID: 31897
		public static StatDef RangedWeapon_RangeMultiplier;

		// Token: 0x04007C9A RID: 31898
		public static StatDef RangedWeapon_WarmupMultiplier;

		// Token: 0x04007C9B RID: 31899
		public static StatDef AccuracyTouch;

		// Token: 0x04007C9C RID: 31900
		public static StatDef AccuracyShort;

		// Token: 0x04007C9D RID: 31901
		public static StatDef AccuracyMedium;

		// Token: 0x04007C9E RID: 31902
		public static StatDef AccuracyLong;

		// Token: 0x04007C9F RID: 31903
		public static StatDef ShootingAccuracyFactor_Touch;

		// Token: 0x04007CA0 RID: 31904
		public static StatDef ShootingAccuracyFactor_Short;

		// Token: 0x04007CA1 RID: 31905
		public static StatDef ShootingAccuracyFactor_Medium;

		// Token: 0x04007CA2 RID: 31906
		public static StatDef ShootingAccuracyFactor_Long;

		// Token: 0x04007CA3 RID: 31907
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x04007CA4 RID: 31908
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x04007CA5 RID: 31909
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x04007CA6 RID: 31910
		public static StatDef ArmorRating_Sharp;

		// Token: 0x04007CA7 RID: 31911
		public static StatDef ArmorRating_Blunt;

		// Token: 0x04007CA8 RID: 31912
		public static StatDef ArmorRating_Heat;

		// Token: 0x04007CA9 RID: 31913
		public static StatDef Insulation_Cold;

		// Token: 0x04007CAA RID: 31914
		public static StatDef Insulation_Heat;

		// Token: 0x04007CAB RID: 31915
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x04007CAC RID: 31916
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x04007CAD RID: 31917
		public static StatDef PackRadius;

		// Token: 0x04007CAE RID: 31918
		[MayRequireRoyalty]
		public static StatDef JumpRange;

		// Token: 0x04007CAF RID: 31919
		public static StatDef EquipDelay;

		// Token: 0x04007CB0 RID: 31920
		public static StatDef MedicalPotency;

		// Token: 0x04007CB1 RID: 31921
		public static StatDef MedicalQualityMax;

		// Token: 0x04007CB2 RID: 31922
		public static StatDef ImmunityGainSpeed;

		// Token: 0x04007CB3 RID: 31923
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x04007CB4 RID: 31924
		public static StatDef InjuryHealingFactor;

		// Token: 0x04007CB5 RID: 31925
		public static StatDef DoorOpenSpeed;

		// Token: 0x04007CB6 RID: 31926
		public static StatDef BedRestEffectiveness;

		// Token: 0x04007CB7 RID: 31927
		public static StatDef TrapMeleeDamage;

		// Token: 0x04007CB8 RID: 31928
		public static StatDef TrapSpringChance;

		// Token: 0x04007CB9 RID: 31929
		public static StatDef ResearchSpeedFactor;

		// Token: 0x04007CBA RID: 31930
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x04007CBB RID: 31931
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x04007CBC RID: 31932
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x04007CBD RID: 31933
		public static StatDef JoyGainFactor;

		// Token: 0x04007CBE RID: 31934
		public static StatDef SurgerySuccessChanceFactor;

		// Token: 0x04007CBF RID: 31935
		[MayRequireIdeology]
		public static StatDef StyleDominance;

		// Token: 0x04007CC0 RID: 31936
		[MayRequireIdeology]
		public static StatDef BiosculpterPodSpeedFactor;

		// Token: 0x04007CC1 RID: 31937
		[MayRequireRoyalty]
		public static StatDef MeditationPlantGrowthOffset;

		// Token: 0x04007CC2 RID: 31938
		[MayRequireBiotech]
		public static StatDef GeneticComplexityIncrease;

		// Token: 0x04007CC3 RID: 31939
		[MayRequireBiotech]
		public static StatDef AssemblySpeedFactor;

		// Token: 0x04007CC4 RID: 31940
		[MayRequireAnomaly]
		public static StatDef PsychicRitualQuality;

		// Token: 0x04007CC5 RID: 31941
		[MayRequireAnomaly]
		public static StatDef ContainmentStrength;

		// Token: 0x04007CC6 RID: 31942
		[MayRequireOdyssey]
		public static StatDef GravshipRange;

		// Token: 0x04007CC7 RID: 31943
		[MayRequireOdyssey]
		public static StatDef SubstructureSupport;

		// Token: 0x04007CC8 RID: 31944
		public static StatDef Ability_CastingTime;

		// Token: 0x04007CC9 RID: 31945
		public static StatDef Ability_EntropyGain;

		// Token: 0x04007CCA RID: 31946
		public static StatDef Ability_PsyfocusCost;

		// Token: 0x04007CCB RID: 31947
		public static StatDef Ability_Duration;

		// Token: 0x04007CCC RID: 31948
		public static StatDef Ability_Range;

		// Token: 0x04007CCD RID: 31949
		public static StatDef Ability_EffectRadius;

		// Token: 0x04007CCE RID: 31950
		public static StatDef Ability_RequiredPsylink;

		// Token: 0x04007CCF RID: 31951
		public static StatDef Ability_GoodwillImpact;

		// Token: 0x04007CD0 RID: 31952
		public static StatDef Ability_DetectChancePerEntropy;

		// Token: 0x04007CD1 RID: 31953
		public static StatDef MeditationFocusStrength;

		// Token: 0x04007CD2 RID: 31954
		[MayRequireIdeology]
		public static StatDef TerrorSource;

		// Token: 0x04007CD3 RID: 31955
		[MayRequireIdeology]
		public static StatDef Terror;

		// Token: 0x04007CD4 RID: 31956
		public static StatDef FilthMultiplier;

		// Token: 0x04007CD5 RID: 31957
		public static StatDef CleaningTimeFactor;
	}
}
