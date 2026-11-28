using System;

namespace RimWorld
{
	// Token: 0x02001ED9 RID: 7897
	[DefOf]
	public static class HistoryEventDefOf
	{
		// Token: 0x0600CA93 RID: 51859 RVA: 0x004A6285 File Offset: 0x004A4485
		static HistoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HistoryEventDefOf));
		}

		// Token: 0x04007FD3 RID: 32723
		public static HistoryEventDef QuestGoodwillReward;

		// Token: 0x04007FD4 RID: 32724
		public static HistoryEventDef DebugGoodwill;

		// Token: 0x04007FD5 RID: 32725
		public static HistoryEventDef RequestedTrader;

		// Token: 0x04007FD6 RID: 32726
		public static HistoryEventDef RequestedOrbitalTrader;

		// Token: 0x04007FD7 RID: 32727
		public static HistoryEventDef RequestedMilitaryAid;

		// Token: 0x04007FD8 RID: 32728
		public static HistoryEventDef PeaceTalksDisaster;

		// Token: 0x04007FD9 RID: 32729
		public static HistoryEventDef PeaceTalksBackfire;

		// Token: 0x04007FDA RID: 32730
		public static HistoryEventDef PeaceTalksSuccess;

		// Token: 0x04007FDB RID: 32731
		public static HistoryEventDef PeaceTalksTriumph;

		// Token: 0x04007FDC RID: 32732
		public static HistoryEventDef QuestPawnLost;

		// Token: 0x04007FDD RID: 32733
		public static HistoryEventDef QuestPawnArrested;

		// Token: 0x04007FDE RID: 32734
		public static HistoryEventDef UsedHarmfulAbility;

		// Token: 0x04007FDF RID: 32735
		public static HistoryEventDef UsedHarmfulItem;

		// Token: 0x04007FE0 RID: 32736
		public static HistoryEventDef GaveGift;

		// Token: 0x04007FE1 RID: 32737
		public static HistoryEventDef SettlementProximity;

		// Token: 0x04007FE2 RID: 32738
		public static HistoryEventDef DestroyedEnemyBase;

		// Token: 0x04007FE3 RID: 32739
		public static HistoryEventDef AttackedSettlement;

		// Token: 0x04007FE4 RID: 32740
		public static HistoryEventDef PerformedHarmfulSurgery;

		// Token: 0x04007FE5 RID: 32741
		public static HistoryEventDef AttackedMember;

		// Token: 0x04007FE6 RID: 32742
		public static HistoryEventDef AttackedBuilding;

		// Token: 0x04007FE7 RID: 32743
		public static HistoryEventDef MemberCrushed;

		// Token: 0x04007FE8 RID: 32744
		public static HistoryEventDef MemberNeutrallyDied;

		// Token: 0x04007FE9 RID: 32745
		public static HistoryEventDef Traded;

		// Token: 0x04007FEA RID: 32746
		public static HistoryEventDef MemberStripped;

		// Token: 0x04007FEB RID: 32747
		public static HistoryEventDef MemberExitedMapHealthy;

		// Token: 0x04007FEC RID: 32748
		public static HistoryEventDef FriendlyExitedMapTended;

		// Token: 0x04007FED RID: 32749
		public static HistoryEventDef UsedForbiddenThing;

		// Token: 0x04007FEE RID: 32750
		public static HistoryEventDef AttackedCaravan;

		// Token: 0x04007FEF RID: 32751
		public static HistoryEventDef MemberCaptured;

		// Token: 0x04007FF0 RID: 32752
		public static HistoryEventDef MemberKilled;

		// Token: 0x04007FF1 RID: 32753
		public static HistoryEventDef MemberSold;

		// Token: 0x04007FF2 RID: 32754
		public static HistoryEventDef ShuttleDestroyed;

		// Token: 0x04007FF3 RID: 32755
		public static HistoryEventDef ReachNaturalGoodwill;

		// Token: 0x04007FF4 RID: 32756
		public static HistoryEventDef ShuttleGuardsMissedShuttle;

		// Token: 0x04007FF5 RID: 32757
		public static HistoryEventDef ShuttleCommanderMissedShuttle;

		// Token: 0x04007FF6 RID: 32758
		public static HistoryEventDef RitualDone;

		// Token: 0x04007FF7 RID: 32759
		[MayRequireBiotech]
		public static HistoryEventDef PollutedNearbySite;

		// Token: 0x04007FF8 RID: 32760
		[MayRequireBiotech]
		public static HistoryEventDef PollutedBase;

		// Token: 0x04007FF9 RID: 32761
		[MayRequireBiotech]
		public static HistoryEventDef AbsorbedXenogerm;

		// Token: 0x04007FFA RID: 32762
		[MayRequireBiotech]
		public static HistoryEventDef ExtractedHemogenPack;

		// Token: 0x04007FFB RID: 32763
		[MayRequireBiotech]
		public static HistoryEventDef PropagateBloodfeederGene;

		// Token: 0x04007FFC RID: 32764
		[MayRequireBiotech]
		public static HistoryEventDef ToxicWasteDumping;

		// Token: 0x04007FFD RID: 32765
		[MayRequireBiotech]
		public static HistoryEventDef BecomeNonPreferredXenotype;

		// Token: 0x04007FFE RID: 32766
		[MayRequireOdyssey]
		public static HistoryEventDef OrbitalPollution;

		// Token: 0x04007FFF RID: 32767
		[MayRequireOdyssey]
		public static HistoryEventDef PawnKidnappedOnGravShip;

		// Token: 0x04008000 RID: 32768
		public static HistoryEventDef ObservedLayingRottingCorpse;

		// Token: 0x04008001 RID: 32769
		public static HistoryEventDef ObservedLayingCorpse;

		// Token: 0x04008002 RID: 32770
		public static HistoryEventDef SoldSlave;

		// Token: 0x04008003 RID: 32771
		public static HistoryEventDef ExecutedPrisoner;

		// Token: 0x04008004 RID: 32772
		public static HistoryEventDef ExecutedGuest;

		// Token: 0x04008005 RID: 32773
		public static HistoryEventDef ExecutedColonist;

		// Token: 0x04008006 RID: 32774
		public static HistoryEventDef ExecutedPrisonerGuilty;

		// Token: 0x04008007 RID: 32775
		public static HistoryEventDef ExecutedPrisonerInnocent;

		// Token: 0x04008008 RID: 32776
		[MayRequireIdeology]
		public static HistoryEventDef HarvestedOrgan;

		// Token: 0x04008009 RID: 32777
		public static HistoryEventDef HarvestedOrganFromColonist;

		// Token: 0x0400800A RID: 32778
		public static HistoryEventDef HarvestedOrganFromGuest;

		// Token: 0x0400800B RID: 32779
		[MayRequireIdeology]
		public static HistoryEventDef TradedOrgan;

		// Token: 0x0400800C RID: 32780
		[MayRequireIdeology]
		public static HistoryEventDef SoldOrgan;

		// Token: 0x0400800D RID: 32781
		[MayRequireIdeology]
		public static HistoryEventDef InstalledOrgan;

		// Token: 0x0400800E RID: 32782
		public static HistoryEventDef IngestedDrug;

		// Token: 0x0400800F RID: 32783
		public static HistoryEventDef IngestedRecreationalDrug;

		// Token: 0x04008010 RID: 32784
		public static HistoryEventDef IngestedHardDrug;

		// Token: 0x04008011 RID: 32785
		public static HistoryEventDef AdministeredDrug;

		// Token: 0x04008012 RID: 32786
		public static HistoryEventDef AdministeredRecreationalDrug;

		// Token: 0x04008013 RID: 32787
		public static HistoryEventDef AdministeredHardDrug;

		// Token: 0x04008014 RID: 32788
		public static HistoryEventDef AteHumanMeat;

		// Token: 0x04008015 RID: 32789
		public static HistoryEventDef AteHumanMeatDirect;

		// Token: 0x04008016 RID: 32790
		public static HistoryEventDef AteHumanMeatAsIngredient;

		// Token: 0x04008017 RID: 32791
		[MayRequireIdeology]
		public static HistoryEventDef AteNonCannibalFood;

		// Token: 0x04008018 RID: 32792
		[MayRequireIdeology]
		public static HistoryEventDef AteVeneratedAnimalMeat;

		// Token: 0x04008019 RID: 32793
		[MayRequireIdeology]
		public static HistoryEventDef BuiltAutomatedTurret;

		// Token: 0x0400801A RID: 32794
		public static HistoryEventDef BuildSpecificDef;

		// Token: 0x0400801B RID: 32795
		public static HistoryEventDef Researching;

		// Token: 0x0400801C RID: 32796
		public static HistoryEventDef InstalledProsthetic;

		// Token: 0x0400801D RID: 32797
		[MayRequireIdeology]
		public static HistoryEventDef SlaughteredAnimal;

		// Token: 0x0400801E RID: 32798
		[MayRequireIdeology]
		public static HistoryEventDef KilledInnocentAnimal;

		// Token: 0x0400801F RID: 32799
		[MayRequireIdeology]
		public static HistoryEventDef SlaughteredVeneratedAnimal;

		// Token: 0x04008020 RID: 32800
		[MayRequireIdeology]
		public static HistoryEventDef HuntedVeneratedAnimal;

		// Token: 0x04008021 RID: 32801
		public static HistoryEventDef CutTree;

		// Token: 0x04008022 RID: 32802
		public static HistoryEventDef MinifiedTreeDied;

		// Token: 0x04008023 RID: 32803
		public static HistoryEventDef Mined;

		// Token: 0x04008024 RID: 32804
		[MayRequireIdeology]
		public static HistoryEventDef AteMeat;

		// Token: 0x04008025 RID: 32805
		[MayRequireIdeology]
		public static HistoryEventDef AteNonMeat;

		// Token: 0x04008026 RID: 32806
		public static HistoryEventDef DestroyedMineable;

		// Token: 0x04008027 RID: 32807
		public static HistoryEventDef Bonded;

		// Token: 0x04008028 RID: 32808
		public static HistoryEventDef InitiatedLovin;

		// Token: 0x04008029 RID: 32809
		public static HistoryEventDef GotLovin;

		// Token: 0x0400802A RID: 32810
		public static HistoryEventDef GotLovin_Spouse;

		// Token: 0x0400802B RID: 32811
		public static HistoryEventDef GotLovin_NonSpouse;

		// Token: 0x0400802C RID: 32812
		public static HistoryEventDef SharedBed;

		// Token: 0x0400802D RID: 32813
		public static HistoryEventDef SharedBed_Spouse;

		// Token: 0x0400802E RID: 32814
		public static HistoryEventDef SharedBed_NonSpouse;

		// Token: 0x0400802F RID: 32815
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Beggars;

		// Token: 0x04008030 RID: 32816
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Beggars_Betrayed;

		// Token: 0x04008031 RID: 32817
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_WandererJoins;

		// Token: 0x04008032 RID: 32818
		public static HistoryEventDef ButcheredHuman;

		// Token: 0x04008033 RID: 32819
		public static HistoryEventDef SowedPlant;

		// Token: 0x04008034 RID: 32820
		public static HistoryEventDef SowedHumanFoodPlant;

		// Token: 0x04008035 RID: 32821
		[MayRequireIdeology]
		public static HistoryEventDef AteFungus;

		// Token: 0x04008036 RID: 32822
		[MayRequireIdeology]
		public static HistoryEventDef AteFungusAsIngredient;

		// Token: 0x04008037 RID: 32823
		[MayRequireIdeology]
		public static HistoryEventDef AteNonFungusPlant;

		// Token: 0x04008038 RID: 32824
		[MayRequireIdeology]
		public static HistoryEventDef AteNonFungusMealWithPlants;

		// Token: 0x04008039 RID: 32825
		[MayRequireIdeology]
		public static HistoryEventDef CharityFulfilled_Beggars;

		// Token: 0x0400803A RID: 32826
		[MayRequireIdeology]
		public static HistoryEventDef CharityFulfilled_WandererJoins;

		// Token: 0x0400803B RID: 32827
		public static HistoryEventDef InnocentPrisonerDied;

		// Token: 0x0400803C RID: 32828
		public static HistoryEventDef GuiltyPrisonerDied;

		// Token: 0x0400803D RID: 32829
		public static HistoryEventDef PrisonerDied;

		// Token: 0x0400803E RID: 32830
		public static HistoryEventDef GotMarried_TookWomansName;

		// Token: 0x0400803F RID: 32831
		public static HistoryEventDef GotMarried_TookMansName;

		// Token: 0x04008040 RID: 32832
		public static HistoryEventDef GotMarried_KeptName;

		// Token: 0x04008041 RID: 32833
		public static HistoryEventDef GotMarried_SpouseCount_OneOrFewer;

		// Token: 0x04008042 RID: 32834
		public static HistoryEventDef GotMarried_SpouseCount_Two;

		// Token: 0x04008043 RID: 32835
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_Three;

		// Token: 0x04008044 RID: 32836
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_Four;

		// Token: 0x04008045 RID: 32837
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_FiveOrMore;

		// Token: 0x04008046 RID: 32838
		[MayRequireIdeology]
		public static HistoryEventDef ChangedIdeo;

		// Token: 0x04008047 RID: 32839
		[MayRequireIdeology]
		public static HistoryEventDef EnslavedPrisoner;

		// Token: 0x04008048 RID: 32840
		[MayRequireIdeology]
		public static HistoryEventDef EnslavedPrisonerNotPreviouslyEnslaved;

		// Token: 0x04008049 RID: 32841
		[MayRequireIdeology]
		public static HistoryEventDef Raided;

		// Token: 0x0400804A RID: 32842
		[MayRequireIdeology]
		public static HistoryEventDef PlayerRaidedSomeone;

		// Token: 0x0400804B RID: 32843
		[MayRequireIdeology]
		public static HistoryEventDef RelicDestroyed;

		// Token: 0x0400804C RID: 32844
		[MayRequireIdeology]
		public static HistoryEventDef RelicLost;

		// Token: 0x0400804D RID: 32845
		[MayRequireIdeology]
		public static HistoryEventDef KillWithNobleWeapon;

		// Token: 0x0400804E RID: 32846
		[MayRequireIdeology]
		public static HistoryEventDef UsedNobleWeapon;

		// Token: 0x0400804F RID: 32847
		[MayRequireIdeology]
		public static HistoryEventDef KillWithDespisedWeapon;

		// Token: 0x04008050 RID: 32848
		[MayRequireIdeology]
		public static HistoryEventDef UsedDespisedWeapon;

		// Token: 0x04008051 RID: 32849
		public static HistoryEventDef AteInsectMeatDirect;

		// Token: 0x04008052 RID: 32850
		public static HistoryEventDef AteInsectMeatAsIngredient;

		// Token: 0x04008053 RID: 32851
		[MayRequireIdeology]
		public static HistoryEventDef GotScarified;

		// Token: 0x04008054 RID: 32852
		[MayRequireIdeology]
		public static HistoryEventDef GotBlinded;

		// Token: 0x04008055 RID: 32853
		[MayRequireIdeology]
		public static HistoryEventDef UsedBiosculpterPod;

		// Token: 0x04008056 RID: 32854
		[MayRequireIdeology]
		public static HistoryEventDef PlantedGauranlenTree;

		// Token: 0x04008057 RID: 32855
		[MayRequireIdeology]
		public static HistoryEventDef ConvertedNewMember;

		// Token: 0x04008058 RID: 32856
		[MayRequireIdeology]
		public static HistoryEventDef ParticipatedInOthersRitual;

		// Token: 0x04008059 RID: 32857
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Pilgrims;

		// Token: 0x0400805A RID: 32858
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Pilgrims_Betrayed;

		// Token: 0x0400805B RID: 32859
		[MayRequireAnomaly]
		public static HistoryEventDef PsychicRitualPerformed;

		// Token: 0x0400805C RID: 32860
		[MayRequireAnomaly]
		public static HistoryEventDef InvolvedInPsychicRitual;

		// Token: 0x0400805D RID: 32861
		[MayRequireAnomaly]
		public static HistoryEventDef WasPsychicRitualTarget;

		// Token: 0x0400805E RID: 32862
		[MayRequireAnomaly]
		public static HistoryEventDef GhoulInfusionPerformed;

		// Token: 0x0400805F RID: 32863
		[MayRequireAnomaly]
		public static HistoryEventDef DestroyedVoidMonolith;

		// Token: 0x04008060 RID: 32864
		[MayRequireOdyssey]
		public static HistoryEventDef SlaughteredFish;

		// Token: 0x04008061 RID: 32865
		[MayRequireOdyssey]
		public static HistoryEventDef DestroyedMechhive;

		// Token: 0x04008062 RID: 32866
		public static HistoryEventDef CharityRefused_ThreatReward_Joiner;
	}
}
