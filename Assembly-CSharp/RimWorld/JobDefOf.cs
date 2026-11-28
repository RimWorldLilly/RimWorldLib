using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E77 RID: 7799
	[DefOf]
	public static class JobDefOf
	{
		// Token: 0x0600CA32 RID: 51762 RVA: 0x004A5C14 File Offset: 0x004A3E14
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		// Token: 0x0400779E RID: 30622
		public static JobDef IdleWhileDespawned;

		// Token: 0x0400779F RID: 30623
		public static JobDef Goto;

		// Token: 0x040077A0 RID: 30624
		public static JobDef PickupToHold;

		// Token: 0x040077A1 RID: 30625
		public static JobDef Wait;

		// Token: 0x040077A2 RID: 30626
		public static JobDef StandAndStare;

		// Token: 0x040077A3 RID: 30627
		public static JobDef Wait_MaintainPosture;

		// Token: 0x040077A4 RID: 30628
		public static JobDef Wait_Asleep;

		// Token: 0x040077A5 RID: 30629
		public static JobDef Wait_Downed;

		// Token: 0x040077A6 RID: 30630
		public static JobDef Wait_AsleepDormancy;

		// Token: 0x040077A7 RID: 30631
		public static JobDef GotoWander;

		// Token: 0x040077A8 RID: 30632
		public static JobDef Wait_Wander;

		// Token: 0x040077A9 RID: 30633
		public static JobDef GotoSafeTemperature;

		// Token: 0x040077AA RID: 30634
		public static JobDef Wait_SafeTemperature;

		// Token: 0x040077AB RID: 30635
		public static JobDef Wait_Combat;

		// Token: 0x040077AC RID: 30636
		public static JobDef Equip;

		// Token: 0x040077AD RID: 30637
		public static JobDef AttackMelee;

		// Token: 0x040077AE RID: 30638
		public static JobDef AttackStatic;

		// Token: 0x040077AF RID: 30639
		public static JobDef UseVerbOnThing;

		// Token: 0x040077B0 RID: 30640
		public static JobDef UseVerbOnThingStatic;

		// Token: 0x040077B1 RID: 30641
		public static JobDef UseVerbOnThingStaticReserve;

		// Token: 0x040077B2 RID: 30642
		public static JobDef CastJump;

		// Token: 0x040077B3 RID: 30643
		public static JobDef CastAbilityOnThing;

		// Token: 0x040077B4 RID: 30644
		public static JobDef CastAbilityOnWorldTile;

		// Token: 0x040077B5 RID: 30645
		public static JobDef TakeInventory;

		// Token: 0x040077B6 RID: 30646
		public static JobDef Follow;

		// Token: 0x040077B7 RID: 30647
		public static JobDef FollowClose;

		// Token: 0x040077B8 RID: 30648
		public static JobDef Wear;

		// Token: 0x040077B9 RID: 30649
		public static JobDef ForceTargetWear;

		// Token: 0x040077BA RID: 30650
		public static JobDef RemoveApparel;

		// Token: 0x040077BB RID: 30651
		public static JobDef DropEquipment;

		// Token: 0x040077BC RID: 30652
		public static JobDef Strip;

		// Token: 0x040077BD RID: 30653
		public static JobDef Open;

		// Token: 0x040077BE RID: 30654
		public static JobDef EjectFuel;

		// Token: 0x040077BF RID: 30655
		public static JobDef Hunt;

		// Token: 0x040077C0 RID: 30656
		public static JobDef ManTurret;

		// Token: 0x040077C1 RID: 30657
		public static JobDef EnterCryptosleepCasket;

		// Token: 0x040077C2 RID: 30658
		public static JobDef UseNeurotrainer;

		// Token: 0x040077C3 RID: 30659
		public static JobDef ClearSnow;

		// Token: 0x040077C4 RID: 30660
		public static JobDef Vomit;

		// Token: 0x040077C5 RID: 30661
		public static JobDef Flick;

		// Token: 0x040077C6 RID: 30662
		public static JobDef DoBill;

		// Token: 0x040077C7 RID: 30663
		public static JobDef Research;

		// Token: 0x040077C8 RID: 30664
		public static JobDef Mine;

		// Token: 0x040077C9 RID: 30665
		public static JobDef OperateDeepDrill;

		// Token: 0x040077CA RID: 30666
		public static JobDef OperateScanner;

		// Token: 0x040077CB RID: 30667
		public static JobDef Repair;

		// Token: 0x040077CC RID: 30668
		[MayRequireBiotech]
		public static JobDef RepairMech;

		// Token: 0x040077CD RID: 30669
		[MayRequireBiotech]
		public static JobDef ControlMech;

		// Token: 0x040077CE RID: 30670
		public static JobDef FixBrokenDownBuilding;

		// Token: 0x040077CF RID: 30671
		public static JobDef UseCommsConsole;

		// Token: 0x040077D0 RID: 30672
		public static JobDef Clean;

		// Token: 0x040077D1 RID: 30673
		public static JobDef PaintBuilding;

		// Token: 0x040077D2 RID: 30674
		public static JobDef PaintFloor;

		// Token: 0x040077D3 RID: 30675
		public static JobDef RemovePaintBuilding;

		// Token: 0x040077D4 RID: 30676
		public static JobDef RemovePaintFloor;

		// Token: 0x040077D5 RID: 30677
		public static JobDef TradeWithPawn;

		// Token: 0x040077D6 RID: 30678
		public static JobDef DismissTrader;

		// Token: 0x040077D7 RID: 30679
		public static JobDef Flee;

		// Token: 0x040077D8 RID: 30680
		public static JobDef FleeAndCower;

		// Token: 0x040077D9 RID: 30681
		[MayRequireAnomaly]
		public static JobDef FleeAndCowerShort;

		// Token: 0x040077DA RID: 30682
		public static JobDef Lovin;

		// Token: 0x040077DB RID: 30683
		public static JobDef SocialFight;

		// Token: 0x040077DC RID: 30684
		public static JobDef Maintain;

		// Token: 0x040077DD RID: 30685
		public static JobDef GiveToPackAnimal;

		// Token: 0x040077DE RID: 30686
		public static JobDef EnterTransporter;

		// Token: 0x040077DF RID: 30687
		public static JobDef Resurrect;

		// Token: 0x040077E0 RID: 30688
		public static JobDef InstallImplant;

		// Token: 0x040077E1 RID: 30689
		public static JobDef Insult;

		// Token: 0x040077E2 RID: 30690
		public static JobDef HaulCorpseToPublicPlace;

		// Token: 0x040077E3 RID: 30691
		public static JobDef InducePrisonerToEscape;

		// Token: 0x040077E4 RID: 30692
		public static JobDef OfferHelp;

		// Token: 0x040077E5 RID: 30693
		public static JobDef ApplyTechprint;

		// Token: 0x040077E6 RID: 30694
		public static JobDef GotoMindControlled;

		// Token: 0x040077E7 RID: 30695
		public static JobDef EmptyThingContainer;

		// Token: 0x040077E8 RID: 30696
		public static JobDef InteractThing;

		// Token: 0x040077E9 RID: 30697
		public static JobDef TakeFromOtherInventory;

		// Token: 0x040077EA RID: 30698
		public static JobDef Reading;

		// Token: 0x040077EB RID: 30699
		public static JobDef EnterPortal;

		// Token: 0x040077EC RID: 30700
		public static JobDef ExitMapFlying;

		// Token: 0x040077ED RID: 30701
		public static JobDef Hack;

		// Token: 0x040077EE RID: 30702
		[MayRequireIdeology]
		public static JobDef ActivateArchonexusCore;

		// Token: 0x040077EF RID: 30703
		[MayRequireBiotech]
		public static JobDef RemoveMechlink;

		// Token: 0x040077F0 RID: 30704
		[MayRequireBiotech]
		public static JobDef GetReimplanted;

		// Token: 0x040077F1 RID: 30705
		[MayRequireBiotech]
		public static JobDef ClearPollution;

		// Token: 0x040077F2 RID: 30706
		[MayRequireBiotech]
		public static JobDef TryRomance;

		// Token: 0x040077F3 RID: 30707
		[MayRequireAnomaly]
		public static JobDef TalkCreepJoiner;

		// Token: 0x040077F4 RID: 30708
		[MayRequireAnomaly]
		public static JobDef AnalyzeItem;

		// Token: 0x040077F5 RID: 30709
		[MayRequireOdyssey]
		public static JobDef InspectGravEngine;

		// Token: 0x040077F6 RID: 30710
		[MayRequireOdyssey]
		public static JobDef GotoOxygenatedArea;

		// Token: 0x040077F7 RID: 30711
		[MayRequireOdyssey]
		public static JobDef GotoPatrolDest;

		// Token: 0x040077F8 RID: 30712
		[MayRequireOdyssey]
		public static JobDef UseOutfitStand;

		// Token: 0x040077F9 RID: 30713
		[MayRequireOdyssey]
		public static JobDef Forage;

		// Token: 0x040077FA RID: 30714
		[MayRequireOdyssey]
		public static JobDef GoSwimming;

		// Token: 0x040077FB RID: 30715
		[MayRequireOdyssey]
		public static JobDef Seal;

		// Token: 0x040077FC RID: 30716
		public static JobDef MarryAdjacentPawn;

		// Token: 0x040077FD RID: 30717
		public static JobDef SpectateCeremony;

		// Token: 0x040077FE RID: 30718
		public static JobDef StandAndBeSociallyActive;

		// Token: 0x040077FF RID: 30719
		public static JobDef GiveSpeech;

		// Token: 0x04007800 RID: 30720
		[MayRequireIdeology]
		public static JobDef AcceptRole;

		// Token: 0x04007801 RID: 30721
		[MayRequireIdeology]
		public static JobDef Dance;

		// Token: 0x04007802 RID: 30722
		[MayRequireIdeology]
		public static JobDef EatAtCannibalPlatter;

		// Token: 0x04007803 RID: 30723
		[MayRequireAnomaly]
		public static JobDef HateChanting;

		// Token: 0x04007804 RID: 30724
		[MayRequireOdyssey]
		public static JobDef GotoShip;

		// Token: 0x04007805 RID: 30725
		[MayRequireOdyssey]
		public static JobDef LeaveShip;

		// Token: 0x04007806 RID: 30726
		[MayRequireOdyssey]
		public static JobDef PilotConsole;

		// Token: 0x04007807 RID: 30727
		public static JobDef PrepareCaravan_GatherItems;

		// Token: 0x04007808 RID: 30728
		public static JobDef PrepareCaravan_GatherAnimals;

		// Token: 0x04007809 RID: 30729
		public static JobDef PrepareCaravan_CollectAnimals;

		// Token: 0x0400780A RID: 30730
		public static JobDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x0400780B RID: 30731
		public static JobDef ReturnedCaravan_PenAnimals;

		// Token: 0x0400780C RID: 30732
		public static JobDef Ignite;

		// Token: 0x0400780D RID: 30733
		public static JobDef BeatFire;

		// Token: 0x0400780E RID: 30734
		public static JobDef ExtinguishSelf;

		// Token: 0x0400780F RID: 30735
		public static JobDef ExtinguishFiresNearby;

		// Token: 0x04007810 RID: 30736
		public static JobDef LayDown;

		// Token: 0x04007811 RID: 30737
		public static JobDef LayDownAwake;

		// Token: 0x04007812 RID: 30738
		public static JobDef LayDownResting;

		// Token: 0x04007813 RID: 30739
		public static JobDef Ingest;

		// Token: 0x04007814 RID: 30740
		[MayRequireBiotech]
		public static JobDef SelfShutdown;

		// Token: 0x04007815 RID: 30741
		[MayRequireAnomaly]
		public static JobDef ActivityDormant;

		// Token: 0x04007816 RID: 30742
		[MayRequireAnomaly]
		public static JobDef EntityGoPassive;

		// Token: 0x04007817 RID: 30743
		public static JobDef HaulToCell;

		// Token: 0x04007818 RID: 30744
		public static JobDef HaulToContainer;

		// Token: 0x04007819 RID: 30745
		public static JobDef Steal;

		// Token: 0x0400781A RID: 30746
		public static JobDef Reload;

		// Token: 0x0400781B RID: 30747
		public static JobDef Refuel;

		// Token: 0x0400781C RID: 30748
		public static JobDef RefuelAtomic;

		// Token: 0x0400781D RID: 30749
		public static JobDef RearmTurret;

		// Token: 0x0400781E RID: 30750
		public static JobDef RearmTurretAtomic;

		// Token: 0x0400781F RID: 30751
		public static JobDef FillFermentingBarrel;

		// Token: 0x04007820 RID: 30752
		public static JobDef TakeBeerOutOfFermentingBarrel;

		// Token: 0x04007821 RID: 30753
		public static JobDef UnloadInventory;

		// Token: 0x04007822 RID: 30754
		public static JobDef UnloadYourInventory;

		// Token: 0x04007823 RID: 30755
		public static JobDef HaulToTransporter;

		// Token: 0x04007824 RID: 30756
		public static JobDef HaulToPortal;

		// Token: 0x04007825 RID: 30757
		[MayRequireIdeology]
		public static JobDef GiveToPawn;

		// Token: 0x04007826 RID: 30758
		[MayRequireIdeology]
		public static JobDef ExtractRelic;

		// Token: 0x04007827 RID: 30759
		[MayRequireIdeology]
		public static JobDef InstallRelic;

		// Token: 0x04007828 RID: 30760
		[MayRequireIdeology]
		public static JobDef ExtractToInventory;

		// Token: 0x04007829 RID: 30761
		[MayRequireBiotech]
		public static JobDef EmptyWasteContainer;

		// Token: 0x0400782A RID: 30762
		[MayRequireBiotech]
		public static JobDef CarryGenepackToContainer;

		// Token: 0x0400782B RID: 30763
		[MayRequireAnomaly]
		public static JobDef TakeBioferriteOutOfHarvester;

		// Token: 0x0400782C RID: 30764
		public static JobDef Carried;

		// Token: 0x0400782D RID: 30765
		public static JobDef Rescue;

		// Token: 0x0400782E RID: 30766
		public static JobDef Arrest;

		// Token: 0x0400782F RID: 30767
		public static JobDef Capture;

		// Token: 0x04007830 RID: 30768
		public static JobDef TakeWoundedPrisonerToBed;

		// Token: 0x04007831 RID: 30769
		public static JobDef TakeToBedToOperate;

		// Token: 0x04007832 RID: 30770
		public static JobDef TakeDownedPawnToBedDrafted;

		// Token: 0x04007833 RID: 30771
		public static JobDef EscortPrisonerToBed;

		// Token: 0x04007834 RID: 30772
		public static JobDef CarryToCryptosleepCasket;

		// Token: 0x04007835 RID: 30773
		public static JobDef CarryToCryptosleepCasketDrafted;

		// Token: 0x04007836 RID: 30774
		public static JobDef ReleasePrisoner;

		// Token: 0x04007837 RID: 30775
		public static JobDef Kidnap;

		// Token: 0x04007838 RID: 30776
		public static JobDef CarryDownedPawnToExit;

		// Token: 0x04007839 RID: 30777
		public static JobDef CarryDownedPawnToPortal;

		// Token: 0x0400783A RID: 30778
		public static JobDef CarryDownedPawnDrafted;

		// Token: 0x0400783B RID: 30779
		public static JobDef CarryToPrisonerBedDrafted;

		// Token: 0x0400783C RID: 30780
		public static JobDef DeliverToCell;

		// Token: 0x0400783D RID: 30781
		public static JobDef DeliverToBed;

		// Token: 0x0400783E RID: 30782
		[MayRequireIdeology]
		public static JobDef DeliverToAltar;

		// Token: 0x0400783F RID: 30783
		[MayRequireIdeology]
		public static JobDef Sacrifice;

		// Token: 0x04007840 RID: 30784
		[MayRequireIdeology]
		public static JobDef Scarify;

		// Token: 0x04007841 RID: 30785
		[MayRequireIdeology]
		public static JobDef Blind;

		// Token: 0x04007842 RID: 30786
		[MayRequireBiotech]
		public static JobDef HaulMechToCharger;

		// Token: 0x04007843 RID: 30787
		[MayRequireBiotech]
		public static JobDef HaulToAtomizer;

		// Token: 0x04007844 RID: 30788
		[MayRequireAnomaly]
		public static JobDef CarryToEntityHolder;

		// Token: 0x04007845 RID: 30789
		[MayRequireAnomaly]
		public static JobDef TransferBetweenEntityHolders;

		// Token: 0x04007846 RID: 30790
		[MayRequireAnomaly]
		public static JobDef CarryToEntityHolderAlreadyHolding;

		// Token: 0x04007847 RID: 30791
		public static JobDef PlaceNoCostFrame;

		// Token: 0x04007848 RID: 30792
		public static JobDef Replant;

		// Token: 0x04007849 RID: 30793
		public static JobDef FinishFrame;

		// Token: 0x0400784A RID: 30794
		public static JobDef Deconstruct;

		// Token: 0x0400784B RID: 30795
		public static JobDef DeconstructForBlueprint;

		// Token: 0x0400784C RID: 30796
		public static JobDef Uninstall;

		// Token: 0x0400784D RID: 30797
		public static JobDef ExtractTree;

		// Token: 0x0400784E RID: 30798
		public static JobDef SmoothFloor;

		// Token: 0x0400784F RID: 30799
		public static JobDef RemoveFloor;

		// Token: 0x04007850 RID: 30800
		public static JobDef RemoveFoundation;

		// Token: 0x04007851 RID: 30801
		public static JobDef BuildRoof;

		// Token: 0x04007852 RID: 30802
		public static JobDef RemoveRoof;

		// Token: 0x04007853 RID: 30803
		public static JobDef SmoothWall;

		// Token: 0x04007854 RID: 30804
		public static JobDef PrisonerAttemptRecruit;

		// Token: 0x04007855 RID: 30805
		public static JobDef PrisonerExecution;

		// Token: 0x04007856 RID: 30806
		public static JobDef GuiltyColonistExecution;

		// Token: 0x04007857 RID: 30807
		public static JobDef DeliverFood;

		// Token: 0x04007858 RID: 30808
		[MayRequireIdeology]
		public static JobDef PrisonerEnslave;

		// Token: 0x04007859 RID: 30809
		[MayRequireIdeology]
		public static JobDef PrisonerReduceWill;

		// Token: 0x0400785A RID: 30810
		[MayRequireIdeology]
		public static JobDef PrisonerConvert;

		// Token: 0x0400785B RID: 30811
		[MayRequireBiotech]
		public static JobDef PrisonerBloodfeed;

		// Token: 0x0400785C RID: 30812
		[MayRequireAnomaly]
		public static JobDef PrisonerInterrogateIdentity;

		// Token: 0x0400785D RID: 30813
		[MayRequireIdeology]
		public static JobDef SlaveSuppress;

		// Token: 0x0400785E RID: 30814
		[MayRequireIdeology]
		public static JobDef SlaveEmancipation;

		// Token: 0x0400785F RID: 30815
		[MayRequireIdeology]
		public static JobDef SlaveExecution;

		// Token: 0x04007860 RID: 30816
		public static JobDef FeedPatient;

		// Token: 0x04007861 RID: 30817
		public static JobDef TendPatient;

		// Token: 0x04007862 RID: 30818
		public static JobDef VisitSickPawn;

		// Token: 0x04007863 RID: 30819
		public static JobDef Sow;

		// Token: 0x04007864 RID: 30820
		public static JobDef Harvest;

		// Token: 0x04007865 RID: 30821
		public static JobDef CutPlant;

		// Token: 0x04007866 RID: 30822
		public static JobDef HarvestDesignated;

		// Token: 0x04007867 RID: 30823
		public static JobDef CutPlantDesignated;

		// Token: 0x04007868 RID: 30824
		public static JobDef PlantSeed;

		// Token: 0x04007869 RID: 30825
		[MayRequireIdeology]
		public static JobDef PruneGauranlenTree;

		// Token: 0x0400786A RID: 30826
		public static JobDef Slaughter;

		// Token: 0x0400786B RID: 30827
		public static JobDef Milk;

		// Token: 0x0400786C RID: 30828
		public static JobDef Shear;

		// Token: 0x0400786D RID: 30829
		public static JobDef Tame;

		// Token: 0x0400786E RID: 30830
		public static JobDef Train;

		// Token: 0x0400786F RID: 30831
		public static JobDef RopeToPen;

		// Token: 0x04007870 RID: 30832
		public static JobDef RopeRoamerToUnenclosedPen;

		// Token: 0x04007871 RID: 30833
		public static JobDef RopeRoamerToHitchingPost;

		// Token: 0x04007872 RID: 30834
		public static JobDef Unrope;

		// Token: 0x04007873 RID: 30835
		public static JobDef ReleaseAnimalToWild;

		// Token: 0x04007874 RID: 30836
		public static JobDef ExtractSkull;

		// Token: 0x04007875 RID: 30837
		[MayRequireOdyssey]
		public static JobDef Fish;

		// Token: 0x04007876 RID: 30838
		[MayRequireOdyssey]
		public static JobDef FishAnimal;

		// Token: 0x04007877 RID: 30839
		public static JobDef Nuzzle;

		// Token: 0x04007878 RID: 30840
		public static JobDef Mate;

		// Token: 0x04007879 RID: 30841
		public static JobDef LayEgg;

		// Token: 0x0400787A RID: 30842
		public static JobDef PredatorHunt;

		// Token: 0x0400787B RID: 30843
		public static JobDef FollowRoper;

		// Token: 0x0400787C RID: 30844
		[MayRequireRoyalty]
		public static JobDef Reign;

		// Token: 0x0400787D RID: 30845
		[MayRequireRoyalty]
		public static JobDef Meditate;

		// Token: 0x0400787E RID: 30846
		[MayRequireRoyalty]
		public static JobDef Play_MusicalInstrument;

		// Token: 0x0400787F RID: 30847
		[MayRequireRoyalty]
		public static JobDef LinkPsylinkable;

		// Token: 0x04007880 RID: 30848
		[MayRequireRoyalty]
		public static JobDef BestowingCeremony;

		// Token: 0x04007881 RID: 30849
		[MayRequireIdeology]
		public static JobDef InduceSlaveToRebel;

		// Token: 0x04007882 RID: 30850
		[MayRequireIdeology]
		public static JobDef OpenStylingStationDialog;

		// Token: 0x04007883 RID: 30851
		[MayRequireIdeology]
		public static JobDef UseStylingStation;

		// Token: 0x04007884 RID: 30852
		[MayRequireIdeology]
		public static JobDef UseStylingStationAutomatic;

		// Token: 0x04007885 RID: 30853
		[MayRequireIdeology]
		public static JobDef DyeHair;

		// Token: 0x04007886 RID: 30854
		[MayRequireIdeology]
		public static JobDef TakeCountToInventory;

		// Token: 0x04007887 RID: 30855
		[MayRequireIdeology]
		public static JobDef GotoAndBeSociallyActive;

		// Token: 0x04007888 RID: 30856
		[MayRequireIdeology]
		public static JobDef PrepareSkylantern;

		// Token: 0x04007889 RID: 30857
		[MayRequireIdeology]
		public static JobDef MeditatePray;

		// Token: 0x0400788A RID: 30858
		[MayRequireIdeology]
		public static JobDef GetNeuralSupercharge;

		// Token: 0x0400788B RID: 30859
		[MayRequireIdeology]
		public static JobDef CreateAndEnterCocoon;

		// Token: 0x0400788C RID: 30860
		[MayRequireIdeology]
		public static JobDef CreateAndEnterHealingPod;

		// Token: 0x0400788D RID: 30861
		[MayRequireIdeology]
		public static JobDef ReturnToGauranlenTree;

		// Token: 0x0400788E RID: 30862
		[MayRequireIdeology]
		public static JobDef MergeIntoGaumakerPod;

		// Token: 0x0400788F RID: 30863
		[MayRequireIdeology]
		public static JobDef ChangeTreeMode;

		// Token: 0x04007890 RID: 30864
		[MayRequireIdeology]
		public static JobDef RecolorApparel;

		// Token: 0x04007891 RID: 30865
		[MayRequireIdeology]
		public static JobDef EnterBiosculpterPod;

		// Token: 0x04007892 RID: 30866
		[MayRequireIdeology]
		public static JobDef CarryToBiosculpterPod;

		// Token: 0x04007893 RID: 30867
		[MayRequireBiotech]
		public static JobDef MechCharge;

		// Token: 0x04007894 RID: 30868
		[MayRequireBiotech]
		public static JobDef Deathrest;

		// Token: 0x04007895 RID: 30869
		[MayRequireBiotech]
		public static JobDef CreateXenogerm;

		// Token: 0x04007896 RID: 30870
		[MayRequireBiotech]
		public static JobDef EnterBuilding;

		// Token: 0x04007897 RID: 30871
		[MayRequireBiotech]
		public static JobDef CarryToBuilding;

		// Token: 0x04007898 RID: 30872
		[MayRequireBiotech]
		public static JobDef AbsorbXenogerm;

		// Token: 0x04007899 RID: 30873
		[MayRequireBiotech]
		public static JobDef Breastfeed;

		// Token: 0x0400789A RID: 30874
		[MayRequireBiotech]
		public static JobDef BreastfeedCarryToMom;

		// Token: 0x0400789B RID: 30875
		[MayRequireBiotech]
		public static JobDef BringBabyToSafety;

		// Token: 0x0400789C RID: 30876
		[MayRequireBiotech]
		public static JobDef BringBabyToSafetyUnforced;

		// Token: 0x0400789D RID: 30877
		[MayRequireBiotech]
		public static JobDef BottleFeedBaby;

		// Token: 0x0400789E RID: 30878
		[MayRequireBiotech]
		public static JobDef CarryToMomAfterBirth;

		// Token: 0x0400789F RID: 30879
		[MayRequireBiotech]
		public static JobDef Wait_WithSleeping;

		// Token: 0x040078A0 RID: 30880
		[MayRequireBiotech]
		public static JobDef ReleaseMechs;

		// Token: 0x040078A1 RID: 30881
		[MayRequireBiotech]
		public static JobDef Lessontaking;

		// Token: 0x040078A2 RID: 30882
		[MayRequireBiotech]
		public static JobDef Lessongiving;

		// Token: 0x040078A3 RID: 30883
		[MayRequireBiotech]
		public static JobDef FertilizeOvum;

		// Token: 0x040078A4 RID: 30884
		[MayRequireBiotech]
		public static JobDef DisassembleMech;

		// Token: 0x040078A5 RID: 30885
		[MayRequireBiotech]
		public static JobDef BabySuckle;

		// Token: 0x040078A6 RID: 30886
		[MayRequireBiotech]
		public static JobDef BabyPlay;

		// Token: 0x040078A7 RID: 30887
		[MayRequireAnomaly]
		public static JobDef RevenantWander;

		// Token: 0x040078A8 RID: 30888
		[MayRequireAnomaly]
		public static JobDef RevenantAttack;

		// Token: 0x040078A9 RID: 30889
		[MayRequireAnomaly]
		public static JobDef RevenantEscape;

		// Token: 0x040078AA RID: 30890
		[MayRequireAnomaly]
		public static JobDef RevenantSleep;

		// Token: 0x040078AB RID: 30891
		[MayRequireAnomaly]
		public static JobDef StudyInteract;

		// Token: 0x040078AC RID: 30892
		[MayRequireAnomaly]
		public static JobDef ReleaseEntity;

		// Token: 0x040078AD RID: 30893
		[MayRequireAnomaly]
		public static JobDef TendEntity;

		// Token: 0x040078AE RID: 30894
		[MayRequireAnomaly]
		public static JobDef ExecuteEntity;

		// Token: 0x040078AF RID: 30895
		[MayRequireAnomaly]
		public static JobDef ExtractBioferrite;

		// Token: 0x040078B0 RID: 30896
		[MayRequireAnomaly]
		public static JobDef ActivitySuppression;

		// Token: 0x040078B1 RID: 30897
		[MayRequireAnomaly]
		public static JobDef NociosphereDepart;

		// Token: 0x040078B2 RID: 30898
		[MayRequireAnomaly]
		public static JobDef GoldenCubePlay;

		// Token: 0x040078B3 RID: 30899
		[MayRequireAnomaly]
		public static JobDef BuildCubeSculpture;

		// Token: 0x040078B4 RID: 30900
		[MayRequireAnomaly]
		public static JobDef InvestigateMonolith;

		// Token: 0x040078B5 RID: 30901
		[MayRequireAnomaly]
		public static JobDef ActivateMonolith;

		// Token: 0x040078B6 RID: 30902
		[MayRequireAnomaly]
		public static JobDef FillIn;

		// Token: 0x040078B7 RID: 30903
		[MayRequireAnomaly]
		public static JobDef ChimeraSwitchToAttackMode;

		// Token: 0x040078B8 RID: 30904
		[MayRequireAnomaly]
		public static JobDef DevourerDigest;

		// Token: 0x040078B9 RID: 30905
		[MayRequireAnomaly]
		public static JobDef UnnaturalCorpseAttack;

		// Token: 0x040078BA RID: 30906
		[MayRequireOdyssey]
		public static JobDef Deactivated;

		// Token: 0x040078BB RID: 30907
		[MayRequireOdyssey]
		public static JobDef SelfDetonate;

		// Token: 0x040078BC RID: 30908
		[MayRequireOdyssey]
		public static JobDef PutApparelOnOutfitStand;
	}
}
