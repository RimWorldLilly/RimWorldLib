using System;

namespace RimWorld
{
	// Token: 0x02001E9A RID: 7834
	[DefOf]
	public static class ThoughtDefOf
	{
		// Token: 0x0600CA55 RID: 51797 RVA: 0x004A5E67 File Offset: 0x004A4067
		static ThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
		}

		// Token: 0x04007B4D RID: 31565
		public static ThoughtDef AteRawFood;

		// Token: 0x04007B4E RID: 31566
		public static ThoughtDef AteCorpse;

		// Token: 0x04007B4F RID: 31567
		public static ThoughtDef AteRottenFood;

		// Token: 0x04007B50 RID: 31568
		[MayRequireRoyalty]
		public static ThoughtDef AteFoodInappropriateForTitle;

		// Token: 0x04007B51 RID: 31569
		public static ThoughtDef SleptInBedroom;

		// Token: 0x04007B52 RID: 31570
		public static ThoughtDef SleptInBarracks;

		// Token: 0x04007B53 RID: 31571
		public static ThoughtDef SleptOutside;

		// Token: 0x04007B54 RID: 31572
		public static ThoughtDef SleptOnGround;

		// Token: 0x04007B55 RID: 31573
		public static ThoughtDef SleptInCold;

		// Token: 0x04007B56 RID: 31574
		public static ThoughtDef SleptInHeat;

		// Token: 0x04007B57 RID: 31575
		public static ThoughtDef SleepDisturbed;

		// Token: 0x04007B58 RID: 31576
		[MayRequireIdeology]
		public static ThoughtDef SleptInRoomWithSlave;

		// Token: 0x04007B59 RID: 31577
		[MayRequireRoyalty]
		public static ThoughtDef ReignedInThroneroom;

		// Token: 0x04007B5A RID: 31578
		[MayRequireIdeology]
		public static ThoughtDef ObservedTerror;

		// Token: 0x04007B5B RID: 31579
		public static ThoughtDef WasImprisoned;

		// Token: 0x04007B5C RID: 31580
		public static ThoughtDef NewColonyOptimism;

		// Token: 0x04007B5D RID: 31581
		public static ThoughtDef NewColonyHope;

		// Token: 0x04007B5E RID: 31582
		public static ThoughtDef AteWithoutTable;

		// Token: 0x04007B5F RID: 31583
		public static ThoughtDef AteInImpressiveDiningRoom;

		// Token: 0x04007B60 RID: 31584
		public static ThoughtDef JoyActivityInImpressiveRecRoom;

		// Token: 0x04007B61 RID: 31585
		public static ThoughtDef Catharsis;

		// Token: 0x04007B62 RID: 31586
		public static ThoughtDef MyOrganHarvested;

		// Token: 0x04007B63 RID: 31587
		public static ThoughtDef HarvestedOrgan_Bloodlust;

		// Token: 0x04007B64 RID: 31588
		public static ThoughtDef KnowColonistDied;

		// Token: 0x04007B65 RID: 31589
		public static ThoughtDef KnowPrisonerDiedInnocent;

		// Token: 0x04007B66 RID: 31590
		public static ThoughtDef FreedFromSlavery;

		// Token: 0x04007B67 RID: 31591
		public static ThoughtDef ReleasedHealthyPrisoner;

		// Token: 0x04007B68 RID: 31592
		public static ThoughtDef SoakingWet;

		// Token: 0x04007B69 RID: 31593
		public static ThoughtDef SoldMyBondedAnimalMood;

		// Token: 0x04007B6A RID: 31594
		public static ThoughtDef WitnessedDeathNonAlly;

		// Token: 0x04007B6B RID: 31595
		public static ThoughtDef WitnessedDeathAlly;

		// Token: 0x04007B6C RID: 31596
		public static ThoughtDef WitnessedDeathBloodlust;

		// Token: 0x04007B6D RID: 31597
		public static ThoughtDef WitnessedDeathFamily;

		// Token: 0x04007B6E RID: 31598
		public static ThoughtDef KilledHumanlikeBloodlust;

		// Token: 0x04007B6F RID: 31599
		public static ThoughtDef Nuzzled;

		// Token: 0x04007B70 RID: 31600
		public static ThoughtDef ArtifactMoodBoost;

		// Token: 0x04007B71 RID: 31601
		public static ThoughtDef KnowBuriedInSarcophagus;

		// Token: 0x04007B72 RID: 31602
		public static ThoughtDef DebugGood;

		// Token: 0x04007B73 RID: 31603
		public static ThoughtDef DebugBad;

		// Token: 0x04007B74 RID: 31604
		public static ThoughtDef DeniedJoining;

		// Token: 0x04007B75 RID: 31605
		public static ThoughtDef ColonistBanished;

		// Token: 0x04007B76 RID: 31606
		public static ThoughtDef ColonistBanishedToDie;

		// Token: 0x04007B77 RID: 31607
		public static ThoughtDef PrisonerBanishedToDie;

		// Token: 0x04007B78 RID: 31608
		public static ThoughtDef ColonyPrisonerEscaped;

		// Token: 0x04007B79 RID: 31609
		public static ThoughtDef BondedAnimalBanished;

		// Token: 0x04007B7A RID: 31610
		public static ThoughtDef BondedAnimalReleased;

		// Token: 0x04007B7B RID: 31611
		public static ThoughtDef FailedToRescueRelative;

		// Token: 0x04007B7C RID: 31612
		public static ThoughtDef RescuedRelative;

		// Token: 0x04007B7D RID: 31613
		public static ThoughtDef Rescued;

		// Token: 0x04007B7E RID: 31614
		public static ThoughtDef PyroUsed;

		// Token: 0x04007B7F RID: 31615
		[MayRequireRoyalty]
		public static ThoughtDef DecreeMet;

		// Token: 0x04007B80 RID: 31616
		[MayRequireRoyalty]
		public static ThoughtDef JealousRage;

		// Token: 0x04007B81 RID: 31617
		[MayRequireIdeology]
		public static ThoughtDef FailedConvertIdeoAttemptResentment;

		// Token: 0x04007B82 RID: 31618
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingMissing;

		// Token: 0x04007B83 RID: 31619
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingDisrespected;

		// Token: 0x04007B84 RID: 31620
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleLost;

		// Token: 0x04007B85 RID: 31621
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleEmpty;

		// Token: 0x04007B86 RID: 31622
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleApparelRequirementNotMet;

		// Token: 0x04007B87 RID: 31623
		[MayRequireIdeology]
		public static ThoughtDef Counselled;

		// Token: 0x04007B88 RID: 31624
		[MayRequireIdeology]
		public static ThoughtDef Counselled_MoodBoost;

		// Token: 0x04007B89 RID: 31625
		[MayRequireIdeology]
		public static ThoughtDef ObservedGibbetCage;

		// Token: 0x04007B8A RID: 31626
		[MayRequireIdeology]
		public static ThoughtDef ObservedSkullspike;

		// Token: 0x04007B8B RID: 31627
		[MayRequireIdeology]
		public static ThoughtDef RelicDestroyed;

		// Token: 0x04007B8C RID: 31628
		[MayRequireIdeology]
		public static ThoughtDef RelicLost;

		// Token: 0x04007B8D RID: 31629
		[MayRequireIdeology]
		public static ThoughtDef RelicAtRitual;

		// Token: 0x04007B8E RID: 31630
		[MayRequireIdeology]
		public static ThoughtDef WasEnslaved;

		// Token: 0x04007B8F RID: 31631
		[MayRequireIdeology]
		public static ThoughtDef TameVeneratedAnimalDied;

		// Token: 0x04007B90 RID: 31632
		[MayRequireIdeology]
		public static ThoughtDef ConnectedTreeDied;

		// Token: 0x04007B91 RID: 31633
		[MayRequireIdeology]
		public static ThoughtDef RelicsCollected;

		// Token: 0x04007B92 RID: 31634
		[MayRequireIdeology]
		public static ThoughtDef BiosculpterPleasure;

		// Token: 0x04007B93 RID: 31635
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalDemanded;

		// Token: 0x04007B94 RID: 31636
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalReceived;

		// Token: 0x04007B95 RID: 31637
		[MayRequireIdeology]
		public static ThoughtDef DryadDied;

		// Token: 0x04007B96 RID: 31638
		[MayRequireBiotech]
		public static ThoughtDef PregnancyTerminated;

		// Token: 0x04007B97 RID: 31639
		[MayRequireBiotech]
		public static ThoughtDef PregnancyEnded;

		// Token: 0x04007B98 RID: 31640
		[MayRequireBiotech]
		public static ThoughtDef Stillbirth;

		// Token: 0x04007B99 RID: 31641
		[MayRequireBiotech]
		public static ThoughtDef Miscarried;

		// Token: 0x04007B9A RID: 31642
		[MayRequireBiotech]
		public static ThoughtDef PartnerMiscarried;

		// Token: 0x04007B9B RID: 31643
		[MayRequireBiotech]
		public static ThoughtDef BabyBorn;

		// Token: 0x04007B9C RID: 31644
		[MayRequireBiotech]
		public static ThoughtDef CryingBaby;

		// Token: 0x04007B9D RID: 31645
		[MayRequireBiotech]
		public static ThoughtDef MyCryingBaby;

		// Token: 0x04007B9E RID: 31646
		[MayRequireBiotech]
		public static ThoughtDef GigglingBaby;

		// Token: 0x04007B9F RID: 31647
		[MayRequireBiotech]
		public static ThoughtDef MyGigglingBaby;

		// Token: 0x04007BA0 RID: 31648
		[MayRequireBiotech]
		public static ThoughtDef BabyGiggledSocial;

		// Token: 0x04007BA1 RID: 31649
		[MayRequireBiotech]
		public static ThoughtDef BabyCriedSocial;

		// Token: 0x04007BA2 RID: 31650
		[MayRequireBiotech]
		public static ThoughtDef BreastfedMe;

		// Token: 0x04007BA3 RID: 31651
		[MayRequireBiotech]
		public static ThoughtDef BreastfedBaby;

		// Token: 0x04007BA4 RID: 31652
		[MayRequireBiotech]
		public static ThoughtDef FedMe;

		// Token: 0x04007BA5 RID: 31653
		[MayRequireBiotech]
		public static ThoughtDef FedBaby;

		// Token: 0x04007BA6 RID: 31654
		[MayRequireBiotech]
		public static ThoughtDef PlayedWithMe;

		// Token: 0x04007BA7 RID: 31655
		[MayRequireBiotech]
		public static ThoughtDef PsychicBondTorn;

		// Token: 0x04007BA8 RID: 31656
		[MayRequireBiotech]
		public static ThoughtDef XenogermHarvested_Prisoner;

		// Token: 0x04007BA9 RID: 31657
		[MayRequireAnomaly]
		public static ThoughtDef PsychicRitualVictim;

		// Token: 0x04007BAA RID: 31658
		[MayRequireAnomaly]
		public static ThoughtDef DarknessLifted;

		// Token: 0x04007BAB RID: 31659
		[MayRequireAnomaly]
		public static ThoughtDef PlayedWithCube;

		// Token: 0x04007BAC RID: 31660
		[MayRequireAnomaly]
		public static ThoughtDef DestroyedCubeSculpture;

		// Token: 0x04007BAD RID: 31661
		[MayRequireAnomaly]
		public static ThoughtDef UnnaturalCorpseDestroyed;

		// Token: 0x04007BAE RID: 31662
		[MayRequireAnomaly]
		public static ThoughtDef ObeliskAbduction;

		// Token: 0x04007BAF RID: 31663
		[MayRequireAnomaly]
		public static ThoughtDef EmbracedTheVoid;

		// Token: 0x04007BB0 RID: 31664
		[MayRequireAnomaly]
		public static ThoughtDef ClosedTheVoid;

		// Token: 0x04007BB1 RID: 31665
		[MayRequireAnomaly]
		public static ThoughtDef VoidClosed;

		// Token: 0x04007BB2 RID: 31666
		[MayRequireAnomaly]
		public static ThoughtDef ClosedTheVoidOpinion;

		// Token: 0x04007BB3 RID: 31667
		[MayRequireAnomaly]
		public static ThoughtDef PsychicRitualGuilt;

		// Token: 0x04007BB4 RID: 31668
		[MayRequireAnomaly]
		public static ThoughtDef VoidCuriosity;

		// Token: 0x04007BB5 RID: 31669
		[MayRequireAnomaly]
		public static ThoughtDef VoidCuriositySatisfied;

		// Token: 0x04007BB6 RID: 31670
		[MayRequireOdyssey]
		public static ThoughtDef DestroyedMechhive;

		// Token: 0x04007BB7 RID: 31671
		[MayRequireOdyssey]
		public static ThoughtDef DestroyedMechhiveMainPawn;

		// Token: 0x04007BB8 RID: 31672
		public static ThoughtDef ColonistLost;

		// Token: 0x04007BB9 RID: 31673
		public static ThoughtDef PawnWithGoodOpinionLost;

		// Token: 0x04007BBA RID: 31674
		public static ThoughtDef PawnWithBadOpinionLost;

		// Token: 0x04007BBB RID: 31675
		public static ThoughtDef ApparelDamaged;

		// Token: 0x04007BBC RID: 31676
		public static ThoughtDef Naked;

		// Token: 0x04007BBD RID: 31677
		public static ThoughtDef ClothedNudist;

		// Token: 0x04007BBE RID: 31678
		public static ThoughtDef DeadMansApparel;

		// Token: 0x04007BBF RID: 31679
		public static ThoughtDef HumanLeatherApparelSad;

		// Token: 0x04007BC0 RID: 31680
		public static ThoughtDef HumanLeatherApparelHappy;

		// Token: 0x04007BC1 RID: 31681
		[MayRequireRoyalty]
		public static ThoughtDef DecreeUnmet;

		// Token: 0x04007BC2 RID: 31682
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredRavenous;

		// Token: 0x04007BC3 RID: 31683
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredStrong;

		// Token: 0x04007BC4 RID: 31684
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_Preferred;

		// Token: 0x04007BC5 RID: 31685
		[MayRequireIdeology]
		public static ThoughtDef TreesDesired;

		// Token: 0x04007BC6 RID: 31686
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Desired;

		// Token: 0x04007BC7 RID: 31687
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Disapproved;

		// Token: 0x04007BC8 RID: 31688
		[MayRequireAnomaly]
		public static ThoughtDef HeardInhumanRambling;

		// Token: 0x04007BC9 RID: 31689
		[MayRequireAnomaly]
		public static ThoughtDef Chimera;

		// Token: 0x04007BCA RID: 31690
		public static ThoughtDef PawnWithGoodOpinionDied;

		// Token: 0x04007BCB RID: 31691
		public static ThoughtDef PawnWithBadOpinionDied;

		// Token: 0x04007BCC RID: 31692
		public static ThoughtDef HadAngeringFight;

		// Token: 0x04007BCD RID: 31693
		public static ThoughtDef HadCatharticFight;

		// Token: 0x04007BCE RID: 31694
		public static ThoughtDef HarmedMe;

		// Token: 0x04007BCF RID: 31695
		public static ThoughtDef BotchedMySurgery;

		// Token: 0x04007BD0 RID: 31696
		public static ThoughtDef CheatedOnMe;

		// Token: 0x04007BD1 RID: 31697
		public static ThoughtDef RebuffedMyRomanceAttempt;

		// Token: 0x04007BD2 RID: 31698
		public static ThoughtDef FailedRomanceAttemptOnMe;

		// Token: 0x04007BD3 RID: 31699
		public static ThoughtDef FailedRomanceAttemptOnMeLowOpinionMood;

		// Token: 0x04007BD4 RID: 31700
		public static ThoughtDef BrokeUpWithMe;

		// Token: 0x04007BD5 RID: 31701
		public static ThoughtDef DivorcedMe;

		// Token: 0x04007BD6 RID: 31702
		public static ThoughtDef RejectedMyProposal;

		// Token: 0x04007BD7 RID: 31703
		public static ThoughtDef RejectedMyProposalMood;

		// Token: 0x04007BD8 RID: 31704
		public static ThoughtDef IRejectedTheirProposal;

		// Token: 0x04007BD9 RID: 31705
		public static ThoughtDef GotMarried;

		// Token: 0x04007BDA RID: 31706
		public static ThoughtDef HoneymoonPhase;

		// Token: 0x04007BDB RID: 31707
		public static ThoughtDef RescuedMe;

		// Token: 0x04007BDC RID: 31708
		public static ThoughtDef RescuedMeByOfferingHelp;

		// Token: 0x04007BDD RID: 31709
		public static ThoughtDef RecruitedMe;

		// Token: 0x04007BDE RID: 31710
		public static ThoughtDef AttendedWedding;

		// Token: 0x04007BDF RID: 31711
		public static ThoughtDef AttendedParty;

		// Token: 0x04007BE0 RID: 31712
		public static ThoughtDef AttendedConcert;

		// Token: 0x04007BE1 RID: 31713
		public static ThoughtDef HeldConcert;

		// Token: 0x04007BE2 RID: 31714
		public static ThoughtDef EncouragingSpeech;

		// Token: 0x04007BE3 RID: 31715
		public static ThoughtDef InspirationalSpeech;

		// Token: 0x04007BE4 RID: 31716
		public static ThoughtDef CrashedTogether;

		// Token: 0x04007BE5 RID: 31717
		public static ThoughtDef GotSomeLovin;

		// Token: 0x04007BE6 RID: 31718
		public static ThoughtDef KilledMyFriend;

		// Token: 0x04007BE7 RID: 31719
		public static ThoughtDef KilledMyRival;

		// Token: 0x04007BE8 RID: 31720
		public static ThoughtDef DefeatedHostileFactionLeader;

		// Token: 0x04007BE9 RID: 31721
		public static ThoughtDef DefeatedMechCluster;

		// Token: 0x04007BEA RID: 31722
		public static ThoughtDef DefeatedInsectHive;

		// Token: 0x04007BEB RID: 31723
		public static ThoughtDef ForcedMeToTakeDrugs;

		// Token: 0x04007BEC RID: 31724
		public static ThoughtDef ForcedMeToTakeLuciferium;

		// Token: 0x04007BED RID: 31725
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerDied;

		// Token: 0x04007BEE RID: 31726
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerArrested;

		// Token: 0x04007BEF RID: 31727
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerSurgicallyViolated;

		// Token: 0x04007BF0 RID: 31728
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerLeftBehind;

		// Token: 0x04007BF1 RID: 31729
		[MayRequireRoyalty]
		public static ThoughtDef NeuroquakeEcho;

		// Token: 0x04007BF2 RID: 31730
		[MayRequireIdeology]
		public static ThoughtDef TrialExonerated;

		// Token: 0x04007BF3 RID: 31731
		[MayRequireIdeology]
		public static ThoughtDef TrialFailed;

		// Token: 0x04007BF4 RID: 31732
		[MayRequireIdeology]
		public static ThoughtDef TrialConvicted;

		// Token: 0x04007BF5 RID: 31733
		[MayRequireBiotech]
		public static ThoughtDef FedOn;

		// Token: 0x04007BF6 RID: 31734
		[MayRequireBiotech]
		public static ThoughtDef FedOn_Social;

		// Token: 0x04007BF7 RID: 31735
		[MayRequireBiotech]
		public static ThoughtDef WasTaught;

		// Token: 0x04007BF8 RID: 31736
		[MayRequireBiotech]
		public static ThoughtDef GaveLesson;

		// Token: 0x04007BF9 RID: 31737
		[MayRequireBiotech]
		public static ThoughtDef KilledChild;

		// Token: 0x04007BFA RID: 31738
		[MayRequireAnomaly]
		public static ThoughtDef DrainedMySkills;

		// Token: 0x04007BFB RID: 31739
		[MayRequireAnomaly]
		public static ThoughtDef UsedMeForPsychicRitual;

		// Token: 0x04007BFC RID: 31740
		[MayRequireOdyssey]
		public static ThoughtDef MadeStatueOfMe;
	}
}
