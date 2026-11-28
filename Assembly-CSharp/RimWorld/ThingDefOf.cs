using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E6B RID: 7787
	[DefOf]
	public static class ThingDefOf
	{
		// Token: 0x0600CA27 RID: 51751 RVA: 0x004A5B59 File Offset: 0x004A3D59
		static ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		// Token: 0x040072F6 RID: 29430
		public static ThingDef Silver;

		// Token: 0x040072F7 RID: 29431
		public static ThingDef Gold;

		// Token: 0x040072F8 RID: 29432
		public static ThingDef Steel;

		// Token: 0x040072F9 RID: 29433
		public static ThingDef WoodLog;

		// Token: 0x040072FA RID: 29434
		public static ThingDef MedicineHerbal;

		// Token: 0x040072FB RID: 29435
		public static ThingDef MedicineIndustrial;

		// Token: 0x040072FC RID: 29436
		public static ThingDef MedicineUltratech;

		// Token: 0x040072FD RID: 29437
		public static ThingDef BlocksGranite;

		// Token: 0x040072FE RID: 29438
		public static ThingDef Plasteel;

		// Token: 0x040072FF RID: 29439
		public static ThingDef Beer;

		// Token: 0x04007300 RID: 29440
		public static ThingDef Chocolate;

		// Token: 0x04007301 RID: 29441
		public static ThingDef ComponentIndustrial;

		// Token: 0x04007302 RID: 29442
		public static ThingDef ComponentSpacer;

		// Token: 0x04007303 RID: 29443
		public static ThingDef InsectJelly;

		// Token: 0x04007304 RID: 29444
		public static ThingDef Cloth;

		// Token: 0x04007305 RID: 29445
		public static ThingDef Leather_Plain;

		// Token: 0x04007306 RID: 29446
		public static ThingDef RawPotatoes;

		// Token: 0x04007307 RID: 29447
		public static ThingDef Granite;

		// Token: 0x04007308 RID: 29448
		public static ThingDef Wort;

		// Token: 0x04007309 RID: 29449
		public static ThingDef AIPersonaCore;

		// Token: 0x0400730A RID: 29450
		public static ThingDef Chemfuel;

		// Token: 0x0400730B RID: 29451
		public static ThingDef Uranium;

		// Token: 0x0400730C RID: 29452
		public static ThingDef Jade;

		// Token: 0x0400730D RID: 29453
		public static ThingDef Shell_HighExplosive;

		// Token: 0x0400730E RID: 29454
		public static ThingDef Shell_AntigrainWarhead;

		// Token: 0x0400730F RID: 29455
		public static ThingDef ReinforcedBarrel;

		// Token: 0x04007310 RID: 29456
		[MayRequireAnomaly]
		public static ThingDef Bioferrite;

		// Token: 0x04007311 RID: 29457
		[MayRequireAnomaly]
		public static ThingDef Leather_Dread;

		// Token: 0x04007312 RID: 29458
		[MayRequireOdyssey]
		public static ThingDef GravlitePanel;

		// Token: 0x04007313 RID: 29459
		public static ThingDef MealSurvivalPack;

		// Token: 0x04007314 RID: 29460
		public static ThingDef MealNutrientPaste;

		// Token: 0x04007315 RID: 29461
		public static ThingDef MealSimple;

		// Token: 0x04007316 RID: 29462
		public static ThingDef MealFine;

		// Token: 0x04007317 RID: 29463
		public static ThingDef Pemmican;

		// Token: 0x04007318 RID: 29464
		public static ThingDef Kibble;

		// Token: 0x04007319 RID: 29465
		public static ThingDef Hay;

		// Token: 0x0400731A RID: 29466
		public static ThingDef Meat_Human;

		// Token: 0x0400731B RID: 29467
		[MayRequireBiotech]
		public static ThingDef BabyFood;

		// Token: 0x0400731C RID: 29468
		public static ThingDef SmokeleafJoint;

		// Token: 0x0400731D RID: 29469
		public static ThingDef Flake;

		// Token: 0x0400731E RID: 29470
		public static ThingDef Yayo;

		// Token: 0x0400731F RID: 29471
		public static ThingDef GoJuice;

		// Token: 0x04007320 RID: 29472
		public static ThingDef WakeUp;

		// Token: 0x04007321 RID: 29473
		public static ThingDef Luciferium;

		// Token: 0x04007322 RID: 29474
		public static ThingDef Penoxycyline;

		// Token: 0x04007323 RID: 29475
		public static ThingDef DropPodIncoming;

		// Token: 0x04007324 RID: 29476
		public static ThingDef DropPodLeaving;

		// Token: 0x04007325 RID: 29477
		public static ThingDef ShipChunkIncoming;

		// Token: 0x04007326 RID: 29478
		public static ThingDef ShipChunkIncoming_SmallExplosion;

		// Token: 0x04007327 RID: 29479
		public static ThingDef CrashedShipPartIncoming;

		// Token: 0x04007328 RID: 29480
		public static ThingDef MeteoriteIncoming;

		// Token: 0x04007329 RID: 29481
		public static ThingDef MeteoriteCraterIncoming;

		// Token: 0x0400732A RID: 29482
		public static ThingDef FlyerArrival;

		// Token: 0x0400732B RID: 29483
		public static ThingDef FlyerLeaving;

		// Token: 0x0400732C RID: 29484
		[MayRequireRoyalty]
		public static ThingDef ShuttleIncoming;

		// Token: 0x0400732D RID: 29485
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashing;

		// Token: 0x0400732E RID: 29486
		[MayRequireIdeology]
		public static ThingDef SpacedroneIncoming;

		// Token: 0x0400732F RID: 29487
		[MayRequireAnomaly]
		public static ThingDef WarpedObeliskIncoming;

		// Token: 0x04007330 RID: 29488
		[MayRequireAnomaly]
		public static ThingDef NoctolithIncoming;

		// Token: 0x04007331 RID: 29489
		[MayRequireAnomaly]
		public static ThingDef IncineratorSpray;

		// Token: 0x04007332 RID: 29490
		[MayRequireAnomaly]
		public static ThingDef DyingRevenant;

		// Token: 0x04007333 RID: 29491
		[MayRequireAnomaly]
		public static ThingDef VoidStructureIncoming;

		// Token: 0x04007334 RID: 29492
		[MayRequireAnomaly]
		public static ThingDef MonolithIncoming;

		// Token: 0x04007335 RID: 29493
		[MayRequireOdyssey]
		public static ThingDef LavaRockIncoming;

		// Token: 0x04007336 RID: 29494
		[MayRequireOdyssey]
		public static ThingDef OrbitalDebrisSpawner;

		// Token: 0x04007337 RID: 29495
		public static ThingDef PawnFlyer;

		// Token: 0x04007338 RID: 29496
		[MayRequireAnomaly]
		public static ThingDef PawnFlyer_Stun;

		// Token: 0x04007339 RID: 29497
		[MayRequireAnomaly]
		public static ThingDef PawnFlyer_ConsumeLeap;

		// Token: 0x0400733A RID: 29498
		public static ThingDef ActiveDropPod;

		// Token: 0x0400733B RID: 29499
		public static ThingDef Fire;

		// Token: 0x0400733C RID: 29500
		public static ThingDef ChunkSlagSteel;

		// Token: 0x0400733D RID: 29501
		public static ThingDef SteamGeyser;

		// Token: 0x0400733E RID: 29502
		public static ThingDef Hive;

		// Token: 0x0400733F RID: 29503
		public static ThingDef ShipChunk;

		// Token: 0x04007340 RID: 29504
		public static ThingDef GlowPod;

		// Token: 0x04007341 RID: 29505
		public static ThingDef MinifiedThing;

		// Token: 0x04007342 RID: 29506
		public static ThingDef MinifiedThingRealtimeDraw;

		// Token: 0x04007343 RID: 29507
		public static ThingDef MinifiedTree;

		// Token: 0x04007344 RID: 29508
		public static ThingDef Dye;

		// Token: 0x04007345 RID: 29509
		public static ThingDef Skull;

		// Token: 0x04007346 RID: 29510
		public static ThingDef TextBook;

		// Token: 0x04007347 RID: 29511
		public static ThingDef Schematic;

		// Token: 0x04007348 RID: 29512
		public static ThingDef Novel;

		// Token: 0x04007349 RID: 29513
		public static ThingDef BurningPowerCell;

		// Token: 0x0400734A RID: 29514
		public static ThingDef VitalsMonitor;

		// Token: 0x0400734B RID: 29515
		public static ThingDef CraterSmall;

		// Token: 0x0400734C RID: 29516
		public static ThingDef CraterMedium;

		// Token: 0x0400734D RID: 29517
		public static ThingDef CraterLarge;

		// Token: 0x0400734E RID: 29518
		public static ThingDef Slate;

		// Token: 0x0400734F RID: 29519
		public static ThingDef Gloomlight;

		// Token: 0x04007350 RID: 29520
		[MayRequireOdyssey]
		public static ThingDef MechPylon;

		// Token: 0x04007351 RID: 29521
		public static ThingDef Vent;

		// Token: 0x04007352 RID: 29522
		public static ThingDef MeleeWeapon_Knife;

		// Token: 0x04007353 RID: 29523
		public static ThingDef AncientSpacerCrate;

		// Token: 0x04007354 RID: 29524
		[MayRequireRoyalty]
		public static ThingDef MonumentMarker;

		// Token: 0x04007355 RID: 29525
		[MayRequireRoyalty]
		public static ThingDef PsychicAmplifier;

		// Token: 0x04007356 RID: 29526
		[MayRequireIdeology]
		public static ThingDef Skullspike;

		// Token: 0x04007357 RID: 29527
		[MayRequireBiotech]
		public static ThingDef Mechlink;

		// Token: 0x04007358 RID: 29528
		[MayRequireBiotech]
		public static ThingDef HemogenPack;

		// Token: 0x04007359 RID: 29529
		[MayRequireBiotech]
		public static ThingDef Genepack;

		// Token: 0x0400735A RID: 29530
		[MayRequireBiotech]
		public static ThingDef Xenogerm;

		// Token: 0x0400735B RID: 29531
		[MayRequireBiotech]
		public static ThingDef Wastepack;

		// Token: 0x0400735C RID: 29532
		[MayRequireBiotech]
		public static ThingDef Shell_Toxic;

		// Token: 0x0400735D RID: 29533
		[MayRequireAnomaly]
		public static ThingDef Shard;

		// Token: 0x0400735E RID: 29534
		[MayRequireAnomaly]
		public static ThingDef Meat_Twisted;

		// Token: 0x0400735F RID: 29535
		[MayRequireAnomaly]
		public static ThingDef GoldenCube;

		// Token: 0x04007360 RID: 29536
		[MayRequireAnomaly]
		public static ThingDef Tome;

		// Token: 0x04007361 RID: 29537
		[MayRequireAnomaly]
		public static ThingDef GrayFleshSample;

		// Token: 0x04007362 RID: 29538
		[MayRequireAnomaly]
		public static ThingDef RevenantSpine;

		// Token: 0x04007363 RID: 29539
		[MayRequireAnomaly]
		public static ThingDef FleshmassNeuralLump;

		// Token: 0x04007364 RID: 29540
		[MayRequireAnomaly]
		public static ThingDef RevenantFleshChunk;

		// Token: 0x04007365 RID: 29541
		[MayRequireAnomaly]
		public static ThingDef GrayStatueDeadlifeDust;

		// Token: 0x04007366 RID: 29542
		[MayRequireAnomaly]
		public static ThingDef GrayStatueTeleporter;

		// Token: 0x04007367 RID: 29543
		[MayRequireAnomaly]
		public static ThingDef MonolithFragment;

		// Token: 0x04007368 RID: 29544
		[MayRequireOdyssey]
		public static ThingDef GravEngine;

		// Token: 0x04007369 RID: 29545
		[MayRequireOdyssey]
		public static ThingDef GravFieldExtender;

		// Token: 0x0400736A RID: 29546
		[MayRequireOdyssey]
		public static ThingDef ChemfuelTank;

		// Token: 0x0400736B RID: 29547
		[MayRequireOdyssey]
		public static ThingDef SmallThruster;

		// Token: 0x0400736C RID: 29548
		[MayRequireOdyssey]
		public static ThingDef LargeThruster;

		// Token: 0x0400736D RID: 29549
		[MayRequireOdyssey]
		public static ThingDef PilotConsole;

		// Token: 0x0400736E RID: 29550
		[MayRequireOdyssey]
		public static ThingDef Gravcore;

		// Token: 0x0400736F RID: 29551
		[MayRequireOdyssey]
		public static ThingDef ShipChunk_Mech;

		// Token: 0x04007370 RID: 29552
		[MayRequireOdyssey]
		public static ThingDef RubblePile;

		// Token: 0x04007371 RID: 29553
		[MayRequireOdyssey]
		public static ThingDef MechRelay;

		// Token: 0x04007372 RID: 29554
		[MayRequireOdyssey]
		public static ThingDef MechStabilizer;

		// Token: 0x04007373 RID: 29555
		[MayRequireOdyssey]
		public static ThingDef GeothermalVent;

		// Token: 0x04007374 RID: 29556
		[MayRequireOdyssey]
		public static ThingDef SolidIce;

		// Token: 0x04007375 RID: 29557
		[MayRequireOdyssey]
		public static ThingDef AncientSmokeVent;

		// Token: 0x04007376 RID: 29558
		[MayRequireOdyssey]
		public static ThingDef AncientToxVent;

		// Token: 0x04007377 RID: 29559
		[MayRequireOdyssey]
		public static ThingDef AncientHeatVent;

		// Token: 0x04007378 RID: 29560
		[MayRequireOdyssey]
		public static ThingDef AncientDestroyedConsole;

		// Token: 0x04007379 RID: 29561
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Red;

		// Token: 0x0400737A RID: 29562
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Blue;

		// Token: 0x0400737B RID: 29563
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Green;

		// Token: 0x0400737C RID: 29564
		[MayRequireOdyssey]
		public static ThingDef Turret_AncientArmoredTurret;

		// Token: 0x0400737D RID: 29565
		[MayRequireOdyssey]
		public static ThingDef AncientGravReactor;

		// Token: 0x0400737E RID: 29566
		[MayRequireOdyssey]
		public static ThingDef AncientSecurityTerminal;

		// Token: 0x0400737F RID: 29567
		[MayRequireOdyssey]
		public static ThingDef AncientGravEngine;

		// Token: 0x04007380 RID: 29568
		[MayRequireOdyssey]
		public static ThingDef AncientTransportPod;

		// Token: 0x04007381 RID: 29569
		[MayRequireOdyssey]
		public static ThingDef MalfunctioningTransportPod;

		// Token: 0x04007382 RID: 29570
		[MayRequireOdyssey]
		public static ThingDef LifeSupportUnit;

		// Token: 0x04007383 RID: 29571
		[MayRequireOdyssey]
		public static ThingDef AncientTerraformer;

		// Token: 0x04007384 RID: 29572
		[MayRequireOdyssey]
		public static ThingDef CerebrexCore;

		// Token: 0x04007385 RID: 29573
		[MayRequireOdyssey]
		public static ThingDef CerebrexCore_Destroyed;

		// Token: 0x04007386 RID: 29574
		[MayRequireOdyssey]
		public static ThingDef CerebrexStabilizer;

		// Token: 0x04007387 RID: 29575
		[MayRequireOdyssey]
		public static ThingDef MechanoidProcessor;

		// Token: 0x04007388 RID: 29576
		[MayRequireOdyssey]
		public static ThingDef Vacstone;

		// Token: 0x04007389 RID: 29577
		[MayRequireOdyssey]
		public static ThingDef HunterDroneTrap;

		// Token: 0x0400738A RID: 29578
		[MayRequireOdyssey]
		public static ThingDef WaspDroneTrap;

		// Token: 0x0400738B RID: 29579
		[MayRequireOdyssey]
		public static ThingDef PassengerShuttle;

		// Token: 0x0400738C RID: 29580
		[MayRequireOdyssey]
		public static ThingDef SurveyScanner;

		// Token: 0x0400738D RID: 29581
		[MayRequireOdyssey]
		public static ThingDef Map;

		// Token: 0x0400738E RID: 29582
		[MayRequireOdyssey]
		public static ThingDef SealedCrate;

		// Token: 0x0400738F RID: 29583
		[MayRequireOdyssey]
		public static ThingDef ChunkVacstone;

		// Token: 0x04007390 RID: 29584
		[MayRequire("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ThingDef Fish_Toxfish;

		// Token: 0x04007391 RID: 29585
		public static ThingDef Filth_Blood;

		// Token: 0x04007392 RID: 29586
		public static ThingDef Filth_BloodSmear;

		// Token: 0x04007393 RID: 29587
		public static ThingDef Filth_AmnioticFluid;

		// Token: 0x04007394 RID: 29588
		public static ThingDef Filth_Dirt;

		// Token: 0x04007395 RID: 29589
		public static ThingDef Filth_Vomit;

		// Token: 0x04007396 RID: 29590
		public static ThingDef Filth_AnimalFilth;

		// Token: 0x04007397 RID: 29591
		public static ThingDef Filth_Trash;

		// Token: 0x04007398 RID: 29592
		public static ThingDef Filth_Slime;

		// Token: 0x04007399 RID: 29593
		public static ThingDef Filth_FireFoam;

		// Token: 0x0400739A RID: 29594
		public static ThingDef Filth_Fuel;

		// Token: 0x0400739B RID: 29595
		public static ThingDef Filth_RubbleRock;

		// Token: 0x0400739C RID: 29596
		public static ThingDef Filth_RubbleBuilding;

		// Token: 0x0400739D RID: 29597
		public static ThingDef Filth_CorpseBile;

		// Token: 0x0400739E RID: 29598
		public static ThingDef Filth_BloodInsect;

		// Token: 0x0400739F RID: 29599
		public static ThingDef Filth_Ash;

		// Token: 0x040073A0 RID: 29600
		public static ThingDef Filth_MachineBits;

		// Token: 0x040073A1 RID: 29601
		public static ThingDef Filth_Water;

		// Token: 0x040073A2 RID: 29602
		public static ThingDef Filth_Hair;

		// Token: 0x040073A3 RID: 29603
		public static ThingDef Filth_BlastMark;

		// Token: 0x040073A4 RID: 29604
		public static ThingDef Filth_GestationFluid;

		// Token: 0x040073A5 RID: 29605
		public static ThingDef Filth_LooseGround;

		// Token: 0x040073A6 RID: 29606
		[MayRequireIdeology]
		public static ThingDef Filth_DriedBlood;

		// Token: 0x040073A7 RID: 29607
		[MayRequireIdeology]
		public static ThingDef Filth_ScatteredDocuments;

		// Token: 0x040073A8 RID: 29608
		[MayRequireIdeology]
		public static ThingDef Filth_MoldyUniform;

		// Token: 0x040073A9 RID: 29609
		[MayRequireIdeology]
		public static ThingDef Filth_PodSlime;

		// Token: 0x040073AA RID: 29610
		[MayRequireIdeology]
		public static ThingDef Filth_OilSmear;

		// Token: 0x040073AB RID: 29611
		[MayRequireBiotech]
		public static ThingDef Filth_Floordrawing;

		// Token: 0x040073AC RID: 29612
		[MayRequireBiotech]
		public static ThingDef Filth_FlammableBile;

		// Token: 0x040073AD RID: 29613
		[MayRequireAnomaly]
		public static ThingDef Filth_RevenantSmear;

		// Token: 0x040073AE RID: 29614
		[MayRequireAnomaly]
		public static ThingDef Filth_RevenantBloodPool;

		// Token: 0x040073AF RID: 29615
		[MayRequireAnomaly]
		public static ThingDef Filth_GrayFleshNoticeable;

		// Token: 0x040073B0 RID: 29616
		[MayRequireAnomaly]
		public static ThingDef Filth_GrayFlesh;

		// Token: 0x040073B1 RID: 29617
		[MayRequireAnomaly]
		public static ThingDef Filth_TwistedFlesh;

		// Token: 0x040073B2 RID: 29618
		[MayRequireAnomaly]
		public static ThingDef FloorEtching;

		// Token: 0x040073B3 RID: 29619
		[MayRequireAnomaly]
		public static ThingDef FloorEtchingRambling;

		// Token: 0x040073B4 RID: 29620
		[MayRequireAnomaly]
		public static ThingDef GrayBox;

		// Token: 0x040073B5 RID: 29621
		[MayRequireAnomaly]
		public static ThingDef Filth_MetalhorrorDebris;

		// Token: 0x040073B6 RID: 29622
		public static ThingDef RectTrigger;

		// Token: 0x040073B7 RID: 29623
		public static ThingDef TriggerUnfogged;

		// Token: 0x040073B8 RID: 29624
		public static ThingDef TriggerContainerEmptied;

		// Token: 0x040073B9 RID: 29625
		public static ThingDef Explosion;

		// Token: 0x040073BA RID: 29626
		public static ThingDef Bombardment;

		// Token: 0x040073BB RID: 29627
		public static ThingDef PowerBeam;

		// Token: 0x040073BC RID: 29628
		public static ThingDef SignalAction_Letter;

		// Token: 0x040073BD RID: 29629
		public static ThingDef SignalAction_Ambush;

		// Token: 0x040073BE RID: 29630
		public static ThingDef SignalAction_OpenCasket;

		// Token: 0x040073BF RID: 29631
		public static ThingDef SignalAction_OpenDoor;

		// Token: 0x040073C0 RID: 29632
		public static ThingDef SignalAction_Message;

		// Token: 0x040073C1 RID: 29633
		public static ThingDef SignalAction_Infestation;

		// Token: 0x040073C2 RID: 29634
		public static ThingDef SignalAction_SoundOneShot;

		// Token: 0x040073C3 RID: 29635
		public static ThingDef SignalAction_DormancyWakeUp;

		// Token: 0x040073C4 RID: 29636
		public static ThingDef SignalAction_Incident;

		// Token: 0x040073C5 RID: 29637
		public static ThingDef SignalAction_StartWick;

		// Token: 0x040073C6 RID: 29638
		public static ThingDef Blight;

		// Token: 0x040073C7 RID: 29639
		public static ThingDef TunnelHiveSpawner;

		// Token: 0x040073C8 RID: 29640
		public static ThingDef PawnGroundSpawner;

		// Token: 0x040073C9 RID: 29641
		[MayRequireRoyalty]
		public static ThingDef Flashstorm;

		// Token: 0x040073CA RID: 29642
		public static ThingDef RadialTrigger;

		// Token: 0x040073CB RID: 29643
		[MayRequireIdeology]
		public static ThingDef TunnelJellySpawner;

		// Token: 0x040073CC RID: 29644
		[MayRequireBiotech]
		public static ThingDef MechShield;

		// Token: 0x040073CD RID: 29645
		[MayRequireBiotech]
		public static ThingDef CocoonSpawner;

		// Token: 0x040073CE RID: 29646
		[MayRequireAnomaly]
		public static ThingDef PitGateSpawner;

		// Token: 0x040073CF RID: 29647
		[MayRequireAnomaly]
		public static ThingDef FleshmassHeartSpawner;

		// Token: 0x040073D0 RID: 29648
		[MayRequireAnomaly]
		public static ThingDef PitBurrowSpawner;

		// Token: 0x040073D1 RID: 29649
		[MayRequireAnomaly]
		public static ThingDef MetalHellFloorCracks;

		// Token: 0x040073D2 RID: 29650
		[MayRequireAnomaly]
		public static ThingDef MetalHellFloorMarkings;

		// Token: 0x040073D3 RID: 29651
		[MayRequireAnomaly]
		public static ThingDef MetalHellAmbience;

		// Token: 0x040073D4 RID: 29652
		[MayRequireOdyssey]
		public static ThingDef LavaEmergence;

		// Token: 0x040073D5 RID: 29653
		[MayRequireOdyssey]
		public static ThingDef LavaEmergenceImmediate;

		// Token: 0x040073D6 RID: 29654
		[MayRequireOdyssey]
		public static ThingDef SeasonalFlood;

		// Token: 0x040073D7 RID: 29655
		[MayRequireOdyssey]
		public static ThingDef TorrentialRainFlood;

		// Token: 0x040073D8 RID: 29656
		[MayRequireOdyssey]
		public static ThingDef InsectAmbush;

		// Token: 0x040073D9 RID: 29657
		[MayRequireOdyssey]
		public static ThingDef GravshipLandingMarker;

		// Token: 0x040073DA RID: 29658
		[MayRequireOdyssey]
		public static ThingDef Blueprint_Substructure;

		// Token: 0x040073DB RID: 29659
		[MayRequireOdyssey]
		public static ThingDef Frame_Substructure;

		// Token: 0x040073DC RID: 29660
		public static ThingDef Sandstone;

		// Token: 0x040073DD RID: 29661
		public static ThingDef Ship_Beam;

		// Token: 0x040073DE RID: 29662
		public static ThingDef Ship_Reactor;

		// Token: 0x040073DF RID: 29663
		public static ThingDef Ship_CryptosleepCasket;

		// Token: 0x040073E0 RID: 29664
		public static ThingDef Ship_ComputerCore;

		// Token: 0x040073E1 RID: 29665
		public static ThingDef Ship_Engine;

		// Token: 0x040073E2 RID: 29666
		public static ThingDef Ship_SensorCluster;

		// Token: 0x040073E3 RID: 29667
		public static ThingDef MineableSteel;

		// Token: 0x040073E4 RID: 29668
		public static ThingDef MineableComponentsIndustrial;

		// Token: 0x040073E5 RID: 29669
		public static ThingDef MineableGold;

		// Token: 0x040073E6 RID: 29670
		public static ThingDef RaisedRocks;

		// Token: 0x040073E7 RID: 29671
		[MayRequireOdyssey]
		public static ThingDef BurrowWall;

		// Token: 0x040073E8 RID: 29672
		[MayRequireAnomaly]
		public static ThingDef Fleshmass;

		// Token: 0x040073E9 RID: 29673
		[MayRequireAnomaly]
		public static ThingDef Fleshmass_Active;

		// Token: 0x040073EA RID: 29674
		[MayRequireAnomaly]
		public static ThingDef FleshSack;

		// Token: 0x040073EB RID: 29675
		[MayRequireAnomaly]
		public static ThingDef FleshmassHeart;

		// Token: 0x040073EC RID: 29676
		[MayRequireAnomaly]
		public static ThingDef Fleshbulb;

		// Token: 0x040073ED RID: 29677
		[MayRequireAnomaly]
		public static ThingDef NerveBundle;

		// Token: 0x040073EE RID: 29678
		[MayRequireAnomaly]
		public static ThingDef FleshmassSpitter;

		// Token: 0x040073EF RID: 29679
		[MayRequireOdyssey]
		public static ThingDef MineableObsidian;

		// Token: 0x040073F0 RID: 29680
		public static ThingDef Door;

		// Token: 0x040073F1 RID: 29681
		public static ThingDef Wall;

		// Token: 0x040073F2 RID: 29682
		public static ThingDef Bed;

		// Token: 0x040073F3 RID: 29683
		public static ThingDef Bedroll;

		// Token: 0x040073F4 RID: 29684
		public static ThingDef SleepingSpot;

		// Token: 0x040073F5 RID: 29685
		public static ThingDef Stool;

		// Token: 0x040073F6 RID: 29686
		public static ThingDef OrbitalTradeBeacon;

		// Token: 0x040073F7 RID: 29687
		public static ThingDef NutrientPasteDispenser;

		// Token: 0x040073F8 RID: 29688
		public static ThingDef Grave;

		// Token: 0x040073F9 RID: 29689
		public static ThingDef Sandbags;

		// Token: 0x040073FA RID: 29690
		public static ThingDef Barricade;

		// Token: 0x040073FB RID: 29691
		public static ThingDef AncientCryptosleepCasket;

		// Token: 0x040073FC RID: 29692
		[MayRequireBiotech]
		public static ThingDef DeathrestCasket;

		// Token: 0x040073FD RID: 29693
		public static ThingDef SolarGenerator;

		// Token: 0x040073FE RID: 29694
		public static ThingDef WoodFiredGenerator;

		// Token: 0x040073FF RID: 29695
		public static ThingDef ChemfuelPoweredGenerator;

		// Token: 0x04007400 RID: 29696
		public static ThingDef PowerConduit;

		// Token: 0x04007401 RID: 29697
		public static ThingDef HiddenConduit;

		// Token: 0x04007402 RID: 29698
		public static ThingDef Battery;

		// Token: 0x04007403 RID: 29699
		public static ThingDef GeothermalGenerator;

		// Token: 0x04007404 RID: 29700
		public static ThingDef WatermillGenerator;

		// Token: 0x04007405 RID: 29701
		public static ThingDef Hopper;

		// Token: 0x04007406 RID: 29702
		public static ThingDef BilliardsTable;

		// Token: 0x04007407 RID: 29703
		public static ThingDef MarriageSpot;

		// Token: 0x04007408 RID: 29704
		public static ThingDef PartySpot;

		// Token: 0x04007409 RID: 29705
		public static ThingDef TransportPod;

		// Token: 0x0400740A RID: 29706
		[MayRequireRoyalty]
		public static ThingDef MeditationSpot;

		// Token: 0x0400740B RID: 29707
		[MayRequireIdeology]
		public static ThingDef RitualSpot;

		// Token: 0x0400740C RID: 29708
		[MayRequireIdeology]
		public static ThingDef Ideogram;

		// Token: 0x0400740D RID: 29709
		public static ThingDef RoyalBed;

		// Token: 0x0400740E RID: 29710
		public static ThingDef TrapSpike;

		// Token: 0x0400740F RID: 29711
		public static ThingDef Cooler;

		// Token: 0x04007410 RID: 29712
		public static ThingDef Heater;

		// Token: 0x04007411 RID: 29713
		public static ThingDef Snowman;

		// Token: 0x04007412 RID: 29714
		public static ThingDef WindTurbine;

		// Token: 0x04007413 RID: 29715
		public static ThingDef FermentingBarrel;

		// Token: 0x04007414 RID: 29716
		public static ThingDef DeepDrill;

		// Token: 0x04007415 RID: 29717
		public static ThingDef GroundPenetratingScanner;

		// Token: 0x04007416 RID: 29718
		public static ThingDef CollapsedRocks;

		// Token: 0x04007417 RID: 29719
		public static ThingDef TorchLamp;

		// Token: 0x04007418 RID: 29720
		public static ThingDef StandingLamp;

		// Token: 0x04007419 RID: 29721
		public static ThingDef WallLamp;

		// Token: 0x0400741A RID: 29722
		public static ThingDef Campfire;

		// Token: 0x0400741B RID: 29723
		public static ThingDef FirefoamPopper;

		// Token: 0x0400741C RID: 29724
		public static ThingDef PassiveCooler;

		// Token: 0x0400741D RID: 29725
		public static ThingDef CaravanPackingSpot;

		// Token: 0x0400741E RID: 29726
		public static ThingDef Table1x2c;

		// Token: 0x0400741F RID: 29727
		public static ThingDef Table2x2c;

		// Token: 0x04007420 RID: 29728
		public static ThingDef Table2x4c;

		// Token: 0x04007421 RID: 29729
		public static ThingDef TableButcher;

		// Token: 0x04007422 RID: 29730
		public static ThingDef DiningChair;

		// Token: 0x04007423 RID: 29731
		public static ThingDef Shelf;

		// Token: 0x04007424 RID: 29732
		public static ThingDef ShelfSmall;

		// Token: 0x04007425 RID: 29733
		public static ThingDef PsychicEmanator;

		// Token: 0x04007426 RID: 29734
		public static ThingDef CommsConsole;

		// Token: 0x04007427 RID: 29735
		public static ThingDef Sarcophagus;

		// Token: 0x04007428 RID: 29736
		public static ThingDef Turret_Mortar;

		// Token: 0x04007429 RID: 29737
		public static ThingDef Turret_MiniTurret;

		// Token: 0x0400742A RID: 29738
		public static ThingDef Turret_AutoMiniTurret;

		// Token: 0x0400742B RID: 29739
		[MayRequireRoyalty]
		public static ThingDef MechCapsule;

		// Token: 0x0400742C RID: 29740
		public static ThingDef DefoliatorShipPart;

		// Token: 0x0400742D RID: 29741
		public static ThingDef PsychicDronerShipPart;

		// Token: 0x0400742E RID: 29742
		public static ThingDef Column;

		// Token: 0x0400742F RID: 29743
		public static ThingDef Urn;

		// Token: 0x04007430 RID: 29744
		public static ThingDef SteleLarge;

		// Token: 0x04007431 RID: 29745
		public static ThingDef SteleGrand;

		// Token: 0x04007432 RID: 29746
		public static ThingDef EggBox;

		// Token: 0x04007433 RID: 29747
		[MayRequireRoyalty]
		public static ThingDef Shuttle;

		// Token: 0x04007434 RID: 29748
		[MayRequireRoyalty]
		public static ThingDef Drape;

		// Token: 0x04007435 RID: 29749
		[MayRequireRoyalty]
		public static ThingDef ActivatorProximity;

		// Token: 0x04007436 RID: 29750
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorBullets;

		// Token: 0x04007437 RID: 29751
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorMortar;

		// Token: 0x04007438 RID: 29752
		public static ThingDef Turret_AutoChargeBlaster;

		// Token: 0x04007439 RID: 29753
		public static ThingDef Turret_AutoInferno;

		// Token: 0x0400743A RID: 29754
		[MayRequireRoyalty]
		public static ThingDef BroadshieldProjector;

		// Token: 0x0400743B RID: 29755
		[MayRequireRoyalty]
		public static ThingDef Brazier;

		// Token: 0x0400743C RID: 29756
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashed;

		// Token: 0x0400743D RID: 29757
		[MayRequireBiotech]
		public static ThingDef ShuttleCrashed_Exitable;

		// Token: 0x0400743E RID: 29758
		[MayRequireBiotech]
		public static ThingDef ShuttleCrashed_Exitable_Mechanitor;

		// Token: 0x0400743F RID: 29759
		[MayRequireIdeology]
		public static ThingDef StylingStation;

		// Token: 0x04007440 RID: 29760
		[MayRequireIdeology]
		public static ThingDef Spacedrone;

		// Token: 0x04007441 RID: 29761
		[MayRequireIdeology]
		public static ThingDef Reliquary;

		// Token: 0x04007442 RID: 29762
		[MayRequireIdeology]
		public static ThingDef Lectern;

		// Token: 0x04007443 RID: 29763
		[MayRequireIdeology]
		public static ThingDef AncientTerminal;

		// Token: 0x04007444 RID: 29764
		[MayRequireIdeology]
		public static ThingDef AncientTerminal_Worshipful;

		// Token: 0x04007445 RID: 29765
		public static ThingDef AncientEquipmentBlocks;

		// Token: 0x04007446 RID: 29766
		public static ThingDef AncientMachine;

		// Token: 0x04007447 RID: 29767
		public static ThingDef AncientStorageCylinder;

		// Token: 0x04007448 RID: 29768
		[MayRequireIdeology]
		public static ThingDef AncientCommsConsole;

		// Token: 0x04007449 RID: 29769
		public static ThingDef AncientHermeticCrate;

		// Token: 0x0400744A RID: 29770
		[MayRequireIdeology]
		public static ThingDef LightBall;

		// Token: 0x0400744B RID: 29771
		[MayRequireIdeology]
		public static ThingDef Loudspeaker;

		// Token: 0x0400744C RID: 29772
		[MayRequireIdeology]
		public static ThingDef Drum;

		// Token: 0x0400744D RID: 29773
		public static ThingDef AncientLamp;

		// Token: 0x0400744E RID: 29774
		[MayRequireIdeology]
		public static ThingDef SkyLantern;

		// Token: 0x0400744F RID: 29775
		[MayRequireIdeology]
		public static ThingDef ArchonexusCore;

		// Token: 0x04007450 RID: 29776
		[MayRequireIdeology]
		public static ThingDef MajorArchotechStructure;

		// Token: 0x04007451 RID: 29777
		[MayRequireIdeology]
		public static ThingDef MajorArchotechStructureStudiable;

		// Token: 0x04007452 RID: 29778
		[MayRequireIdeology]
		public static ThingDef ArchotechTower;

		// Token: 0x04007453 RID: 29779
		[MayRequireIdeology]
		public static ThingDef GrandArchotechStructure;

		// Token: 0x04007454 RID: 29780
		public static ThingDef AncientLockerBank;

		// Token: 0x04007455 RID: 29781
		public static ThingDef AncientLargeCrate;

		// Token: 0x04007456 RID: 29782
		public static ThingDef AncientCrate;

		// Token: 0x04007457 RID: 29783
		public static ThingDef AncientFence;

		// Token: 0x04007458 RID: 29784
		public static ThingDef AncientRazorWire;

		// Token: 0x04007459 RID: 29785
		public static ThingDef AncientMegaCannonTripod;

		// Token: 0x0400745A RID: 29786
		public static ThingDef AncientMegaCannonBarrel;

		// Token: 0x0400745B RID: 29787
		public static ThingDef ChunkMechanoidSlag;

		// Token: 0x0400745C RID: 29788
		public static ThingDef AncientBed;

		// Token: 0x0400745D RID: 29789
		public static ThingDef AncientBarrel;

		// Token: 0x0400745E RID: 29790
		public static ThingDef AncientLargeContainer;

		// Token: 0x0400745F RID: 29791
		public static ThingDef AncientSystemRack;

		// Token: 0x04007460 RID: 29792
		public static ThingDef AncientMicrowave;

		// Token: 0x04007461 RID: 29793
		public static ThingDef AncientRefrigerator;

		// Token: 0x04007462 RID: 29794
		public static ThingDef AncientOven;

		// Token: 0x04007463 RID: 29795
		public static ThingDef AncientStove;

		// Token: 0x04007464 RID: 29796
		public static ThingDef AncientKitchenSink;

		// Token: 0x04007465 RID: 29797
		public static ThingDef AncientOperatingTable;

		// Token: 0x04007466 RID: 29798
		public static ThingDef AncientMechDropBeacon;

		// Token: 0x04007467 RID: 29799
		public static ThingDef AncientTankTrap;

		// Token: 0x04007468 RID: 29800
		public static ThingDef AncientRustedCar;

		// Token: 0x04007469 RID: 29801
		public static ThingDef AncientRustedJeep;

		// Token: 0x0400746A RID: 29802
		public static ThingDef AncientGenerator;

		// Token: 0x0400746B RID: 29803
		public static ThingDef AncientDisplayBank;

		// Token: 0x0400746C RID: 29804
		public static ThingDef AncientSecurityTurret;

		// Token: 0x0400746D RID: 29805
		public static ThingDef AncientRustedCarFrame;

		// Token: 0x0400746E RID: 29806
		public static ThingDef AncientRustedEngineBlock;

		// Token: 0x0400746F RID: 29807
		public static ThingDef AncientLargeRustedEngineBlock;

		// Token: 0x04007470 RID: 29808
		[MayRequireIdeology]
		public static ThingDef BiosculpterPod;

		// Token: 0x04007471 RID: 29809
		[MayRequireIdeology]
		public static ThingDef NeuralSupercharger;

		// Token: 0x04007472 RID: 29810
		public static ThingDef AncientWarwalkerTorso;

		// Token: 0x04007473 RID: 29811
		public static ThingDef AncientWarwalkerClaw;

		// Token: 0x04007474 RID: 29812
		public static ThingDef AncientWarwalkerLeg;

		// Token: 0x04007475 RID: 29813
		public static ThingDef AncientMiniWarwalkerRemains;

		// Token: 0x04007476 RID: 29814
		public static ThingDef AncientWarspiderRemains;

		// Token: 0x04007477 RID: 29815
		public static ThingDef AncientShipBeacon;

		// Token: 0x04007478 RID: 29816
		[MayRequireIdeology]
		public static ThingDef AncientSecurityCrate;

		// Token: 0x04007479 RID: 29817
		public static ThingDef AncientCryptosleepPod;

		// Token: 0x0400747A RID: 29818
		[MayRequireIdeology]
		public static ThingDef AncientEnemyTerminal;

		// Token: 0x0400747B RID: 29819
		public static ThingDef AncientFuelNode;

		// Token: 0x0400747C RID: 29820
		public static ThingDef AncientPodCar;

		// Token: 0x0400747D RID: 29821
		public static ThingDef AncientHydrant;

		// Token: 0x0400747E RID: 29822
		public static ThingDef AncientTank;

		// Token: 0x0400747F RID: 29823
		[MayRequireBiotech]
		public static ThingDef AncientExostriderRemains;

		// Token: 0x04007480 RID: 29824
		public static ThingDef AncientWarwalkerFoot;

		// Token: 0x04007481 RID: 29825
		public static ThingDef AncientMechGestatorTank;

		// Token: 0x04007482 RID: 29826
		public static ThingDef AncientLargeMechGestator;

		// Token: 0x04007483 RID: 29827
		public static ThingDef AncientBasicRecharger;

		// Token: 0x04007484 RID: 29828
		public static ThingDef AncientStandardRecharger;

		// Token: 0x04007485 RID: 29829
		[MayRequireBiotech]
		public static ThingDef AncientToxifierGenerator;

		// Token: 0x04007486 RID: 29830
		[MayRequireBiotech]
		public static ThingDef AncientBandNode;

		// Token: 0x04007487 RID: 29831
		[MayRequireBiotech]
		public static ThingDef GeneAssembler;

		// Token: 0x04007488 RID: 29832
		[MayRequireBiotech]
		public static ThingDef GeneExtractor;

		// Token: 0x04007489 RID: 29833
		[MayRequireBiotech]
		public static ThingDef GrowthVat;

		// Token: 0x0400748A RID: 29834
		[MayRequireBiotech]
		public static ThingDef HumanEmbryo;

		// Token: 0x0400748B RID: 29835
		[MayRequireBiotech]
		public static ThingDef HumanOvum;

		// Token: 0x0400748C RID: 29836
		[MayRequireBiotech]
		public static ThingDef ArchiteCapsule;

		// Token: 0x0400748D RID: 29837
		[MayRequireBiotech]
		public static ThingDef ToyBox;

		// Token: 0x0400748E RID: 29838
		[MayRequireBiotech]
		public static ThingDef SchoolDesk;

		// Token: 0x0400748F RID: 29839
		[MayRequireBiotech]
		public static ThingDef Blackboard;

		// Token: 0x04007490 RID: 29840
		[MayRequireBiotech]
		public static ThingDef BandNode;

		// Token: 0x04007491 RID: 29841
		[MayRequireBiotech]
		public static ThingDef BabyDecoration;

		// Token: 0x04007492 RID: 29842
		public static ThingDef SimpleResearchBench;

		// Token: 0x04007493 RID: 29843
		public static ThingDef HydroponicsBasin;

		// Token: 0x04007494 RID: 29844
		[MayRequireBiotech]
		public static ThingDef ToxifierGenerator;

		// Token: 0x04007495 RID: 29845
		[MayRequireBiotech]
		public static ThingDef SanguphageMeetingTorch;

		// Token: 0x04007496 RID: 29846
		[MayRequireBiotech]
		public static ThingDef SubcoreEncoder;

		// Token: 0x04007497 RID: 29847
		[MayRequireBiotech]
		public static ThingDef GeneBank;

		// Token: 0x04007498 RID: 29848
		[MayRequireOdyssey]
		public static ThingDef MechanoidDropPod;

		// Token: 0x04007499 RID: 29849
		[MayRequireAnomaly]
		public static ThingDef PitGate;

		// Token: 0x0400749A RID: 29850
		[MayRequireAnomaly]
		public static ThingDef CaveExit;

		// Token: 0x0400749B RID: 29851
		[MayRequireAnomaly]
		public static ThingDef PitBurrow;

		// Token: 0x0400749C RID: 29852
		[MayRequireAnomaly]
		public static ThingDef Electroharvester;

		// Token: 0x0400749D RID: 29853
		[MayRequireAnomaly]
		public static ThingDef ElectricInhibitor;

		// Token: 0x0400749E RID: 29854
		[MayRequireAnomaly]
		public static ThingDef PsychicRitualSpot;

		// Token: 0x0400749F RID: 29855
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Mutator;

		// Token: 0x040074A0 RID: 29856
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Duplicator;

		// Token: 0x040074A1 RID: 29857
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Abductor;

		// Token: 0x040074A2 RID: 29858
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Labyrinth;

		// Token: 0x040074A3 RID: 29859
		[MayRequireAnomaly]
		public static ThingDef Noctolith;

		// Token: 0x040074A4 RID: 29860
		[MayRequireAnomaly]
		public static ThingDef DirtCubeSculpture;

		// Token: 0x040074A5 RID: 29861
		[MayRequireAnomaly]
		public static ThingDef StoneCubeSculpture;

		// Token: 0x040074A6 RID: 29862
		[MayRequireAnomaly]
		public static ThingDef SandCubeSculpture;

		// Token: 0x040074A7 RID: 29863
		[MayRequireAnomaly]
		public static ThingDef ScrapCubeSculpture;

		// Token: 0x040074A8 RID: 29864
		[MayRequireAnomaly]
		public static ThingDef VoidStructure;

		// Token: 0x040074A9 RID: 29865
		[MayRequireAnomaly]
		public static ThingDef VoidmetalWall;

		// Token: 0x040074AA RID: 29866
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassSmall;

		// Token: 0x040074AB RID: 29867
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassMedium;

		// Token: 0x040074AC RID: 29868
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassLarge;

		// Token: 0x040074AD RID: 29869
		[MayRequireAnomaly]
		public static ThingDef GrayDoor;

		// Token: 0x040074AE RID: 29870
		[MayRequireAnomaly]
		public static ThingDef GrayWall;

		// Token: 0x040074AF RID: 29871
		[MayRequireAnomaly]
		public static ThingDef LabyrinthMatter;

		// Token: 0x040074B0 RID: 29872
		[MayRequireAnomaly]
		public static ThingDef VoidNode;

		// Token: 0x040074B1 RID: 29873
		[MayRequireAnomaly]
		public static ThingDef VoidMonolith;

		// Token: 0x040074B2 RID: 29874
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Small;

		// Token: 0x040074B3 RID: 29875
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Large;

		// Token: 0x040074B4 RID: 29876
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Vertical;

		// Token: 0x040074B5 RID: 29877
		[MayRequireAnomaly]
		public static ThingDef FleshbeastGroundSpawner;

		// Token: 0x040074B6 RID: 29878
		[MayRequireAnomaly]
		public static ThingDef SecurityDoor;

		// Token: 0x040074B7 RID: 29879
		[MayRequireAnomaly]
		public static ThingDef BioferriteHarvester;

		// Token: 0x040074B8 RID: 29880
		[MayRequireAnomaly]
		public static ThingDef AtmosphericHeater;

		// Token: 0x040074B9 RID: 29881
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleA;

		// Token: 0x040074BA RID: 29882
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleB;

		// Token: 0x040074BB RID: 29883
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleC;

		// Token: 0x040074BC RID: 29884
		[MayRequireAnomaly]
		public static ThingDef Mote_IncineratorBurst;

		// Token: 0x040074BD RID: 29885
		[MayRequireAnomaly]
		public static ThingDef SleepSuppressor;

		// Token: 0x040074BE RID: 29886
		[MayRequireAnomaly]
		public static ThingDef BioferriteGenerator;

		// Token: 0x040074BF RID: 29887
		[MayRequireAnomaly]
		public static ThingDef EndlessPit2x2c;

		// Token: 0x040074C0 RID: 29888
		[MayRequireAnomaly]
		public static ThingDef EndlessPit3x2c;

		// Token: 0x040074C1 RID: 29889
		[MayRequireAnomaly]
		public static ThingDef EndlessPit3x3c;

		// Token: 0x040074C2 RID: 29890
		[MayRequireOdyssey]
		public static ThingDef InsectLairEntrance;

		// Token: 0x040074C3 RID: 29891
		[MayRequireOdyssey]
		public static ThingDef AncientFortifiedWall;

		// Token: 0x040074C4 RID: 29892
		[MayRequireOdyssey]
		public static ThingDef OrbitalAncientFortifiedWall;

		// Token: 0x040074C5 RID: 29893
		[MayRequireOdyssey]
		public static ThingDef AncientBlastDoor;

		// Token: 0x040074C6 RID: 29894
		[MayRequireOdyssey]
		public static ThingDef EggSac;

		// Token: 0x040074C7 RID: 29895
		[MayRequireOdyssey]
		public static ThingDef AncientHatch;

		// Token: 0x040074C8 RID: 29896
		[MayRequireOdyssey]
		public static ThingDef AncientHatchExit;

		// Token: 0x040074C9 RID: 29897
		[MayRequireOdyssey]
		public static ThingDef AncientExplosivesCrate;

		// Token: 0x040074CA RID: 29898
		[MayRequireOdyssey]
		public static ThingDef Statue;

		// Token: 0x040074CB RID: 29899
		[MayRequireOdyssey]
		public static ThingDef GaussCannon;

		// Token: 0x040074CC RID: 29900
		[MayRequireOdyssey]
		public static ThingDef AnticraftBeam;

		// Token: 0x040074CD RID: 29901
		[MayRequireOdyssey]
		public static ThingDef GravshipHull;

		// Token: 0x040074CE RID: 29902
		[MayRequireOdyssey]
		public static ThingDef GravAnchor;

		// Token: 0x040074CF RID: 29903
		public static ThingDef Plant_Potato;

		// Token: 0x040074D0 RID: 29904
		public static ThingDef Plant_TreeOak;

		// Token: 0x040074D1 RID: 29905
		public static ThingDef Plant_TreePoplar;

		// Token: 0x040074D2 RID: 29906
		public static ThingDef Plant_TreePine;

		// Token: 0x040074D3 RID: 29907
		public static ThingDef Plant_TreePalm;

		// Token: 0x040074D4 RID: 29908
		public static ThingDef Plant_Alocasia;

		// Token: 0x040074D5 RID: 29909
		public static ThingDef Plant_TreeBirch;

		// Token: 0x040074D6 RID: 29910
		public static ThingDef Plant_Grass;

		// Token: 0x040074D7 RID: 29911
		public static ThingDef Plant_Ambrosia;

		// Token: 0x040074D8 RID: 29912
		public static ThingDef Plant_Dandelion;

		// Token: 0x040074D9 RID: 29913
		public static ThingDef Glowstool;

		// Token: 0x040074DA RID: 29914
		public static ThingDef Bryolux;

		// Token: 0x040074DB RID: 29915
		public static ThingDef Agarilux;

		// Token: 0x040074DC RID: 29916
		public static ThingDef Plant_HealrootWild;

		// Token: 0x040074DD RID: 29917
		[MayRequireRoyalty]
		public static ThingDef Plant_TreeAnima;

		// Token: 0x040074DE RID: 29918
		[MayRequireRoyalty]
		public static ThingDef AnimusStone;

		// Token: 0x040074DF RID: 29919
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Small;

		// Token: 0x040074E0 RID: 29920
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Large;

		// Token: 0x040074E1 RID: 29921
		[MayRequireRoyalty]
		public static ThingDef Plant_GrassAnima;

		// Token: 0x040074E2 RID: 29922
		[MayRequireIdeology]
		public static ThingDef Plant_Nutrifungus;

		// Token: 0x040074E3 RID: 29923
		[MayRequireIdeology]
		public static ThingDef Plant_PodGauranlen;

		// Token: 0x040074E4 RID: 29924
		[MayRequireIdeology]
		public static ThingDef Plant_TreeGauranlen;

		// Token: 0x040074E5 RID: 29925
		[MayRequireIdeology]
		public static ThingDef DryadCocoon;

		// Token: 0x040074E6 RID: 29926
		[MayRequireIdeology]
		public static ThingDef DryadHealingPod;

		// Token: 0x040074E7 RID: 29927
		[MayRequireIdeology]
		public static ThingDef GaumakerCocoon;

		// Token: 0x040074E8 RID: 29928
		[MayRequireIdeology]
		public static ThingDef Plant_Fibercorn;

		// Token: 0x040074E9 RID: 29929
		[MayRequireBiotech]
		public static ThingDef Plant_Toxipotato;

		// Token: 0x040074EA RID: 29930
		[MayRequireBiotech]
		public static ThingDef Plant_RatPalm;

		// Token: 0x040074EB RID: 29931
		[MayRequireBiotech]
		public static ThingDef Plant_GrayGrass;

		// Token: 0x040074EC RID: 29932
		[MayRequireAnomaly]
		public static ThingDef Plant_TreeHarbinger;

		// Token: 0x040074ED RID: 29933
		[MayRequireOdyssey]
		public static ThingDef Boomshroom;

		// Token: 0x040074EE RID: 29934
		[MayRequireOdyssey]
		public static ThingDef Plant_Psilocap;

		// Token: 0x040074EF RID: 29935
		[MayRequireOdyssey]
		public static ThingDef Plant_TreeArchean;

		// Token: 0x040074F0 RID: 29936
		public static ThingDef Human;

		// Token: 0x040074F1 RID: 29937
		public static ThingDef Muffalo;

		// Token: 0x040074F2 RID: 29938
		public static ThingDef Dromedary;

		// Token: 0x040074F3 RID: 29939
		public static ThingDef Cow;

		// Token: 0x040074F4 RID: 29940
		public static ThingDef Goat;

		// Token: 0x040074F5 RID: 29941
		public static ThingDef Chicken;

		// Token: 0x040074F6 RID: 29942
		public static ThingDef Thrumbo;

		// Token: 0x040074F7 RID: 29943
		[MayRequireBiotech]
		public static ThingDef Mech_Apocriton;

		// Token: 0x040074F8 RID: 29944
		[MayRequireBiotech]
		public static ThingDef Mech_Constructoid;

		// Token: 0x040074F9 RID: 29945
		[MayRequireAnomaly]
		public static ThingDef Fingerspike;

		// Token: 0x040074FA RID: 29946
		[MayRequireAnomaly]
		public static ThingDef Metalhorror;

		// Token: 0x040074FB RID: 29947
		[MayRequireAnomaly]
		public static ThingDef CreepJoiner;

		// Token: 0x040074FC RID: 29948
		public static ThingDef Spark;

		// Token: 0x040074FD RID: 29949
		[MayRequireAnomaly]
		public static ThingDef DisruptorFlare;

		// Token: 0x040074FE RID: 29950
		public static ThingDef Apparel_ShieldBelt;

		// Token: 0x040074FF RID: 29951
		public static ThingDef Apparel_Parka;

		// Token: 0x04007500 RID: 29952
		public static ThingDef Apparel_Tuque;

		// Token: 0x04007501 RID: 29953
		[MayRequireIdeology]
		public static ThingDef Apparel_BodyStrap;

		// Token: 0x04007502 RID: 29954
		[MayRequireIdeology]
		public static ThingDef Apparel_Collar;

		// Token: 0x04007503 RID: 29955
		[MayRequireBiotech]
		public static ThingDef Apparel_GasMask;

		// Token: 0x04007504 RID: 29956
		[MayRequireOdyssey]
		public static ThingDef Apparel_VacsuitHelmet;

		// Token: 0x04007505 RID: 29957
		[MayRequireOdyssey]
		public static ThingDef Apparel_Vacsuit;

		// Token: 0x04007506 RID: 29958
		[MayRequireOdyssey]
		public static ThingDef Apparel_CerebrexNode;

		// Token: 0x04007507 RID: 29959
		public static ThingDef Mote_Text;

		// Token: 0x04007508 RID: 29960
		public static ThingDef Mote_TempRoof;

		// Token: 0x04007509 RID: 29961
		public static ThingDef Mote_ColonistFleeing;

		// Token: 0x0400750A RID: 29962
		public static ThingDef Mote_ColonistAttacking;

		// Token: 0x0400750B RID: 29963
		public static ThingDef Mote_Speech;

		// Token: 0x0400750C RID: 29964
		public static ThingDef Mote_ThoughtBad;

		// Token: 0x0400750D RID: 29965
		public static ThingDef Mote_ThoughtGood;

		// Token: 0x0400750E RID: 29966
		public static ThingDef Mote_Stun;

		// Token: 0x0400750F RID: 29967
		public static ThingDef Mote_Bombardment;

		// Token: 0x04007510 RID: 29968
		public static ThingDef Mote_PowerBeam;

		// Token: 0x04007511 RID: 29969
		public static ThingDef Mote_Leaf;

		// Token: 0x04007512 RID: 29970
		public static ThingDef Mote_ForceJob;

		// Token: 0x04007513 RID: 29971
		public static ThingDef Mote_ForceJobMaintained;

		// Token: 0x04007514 RID: 29972
		[MayRequireRoyalty]
		public static ThingDef Mote_CastPsycast;

		// Token: 0x04007515 RID: 29973
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkLine;

		// Token: 0x04007516 RID: 29974
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkPulse;

		// Token: 0x04007517 RID: 29975
		[MayRequireRoyalty]
		public static ThingDef Mote_PsyfocusPulse;

		// Token: 0x04007518 RID: 29976
		[MayRequireRoyalty]
		public static ThingDef Mote_Bestow;

		// Token: 0x04007519 RID: 29977
		[MayRequireIdeology]
		public static ThingDef Mote_LightBall;

		// Token: 0x0400751A RID: 29978
		[MayRequireIdeology]
		public static ThingDef Mote_LightBallLights;

		// Token: 0x0400751B RID: 29979
		[MayRequireIdeology]
		public static ThingDef Mote_LoudspeakerLights;

		// Token: 0x0400751C RID: 29980
		[MayRequireIdeology]
		public static ThingDef Mote_RolePositionHighlight;

		// Token: 0x0400751D RID: 29981
		[MayRequireIdeology]
		public static ThingDef Mote_RolePawnHighlight;

		// Token: 0x0400751E RID: 29982
		[MayRequireIdeology]
		public static ThingDef Mote_GauranlenCasteChanged;

		// Token: 0x0400751F RID: 29983
		[MayRequireBiotech]
		public static ThingDef Mote_ResurrectFlash;

		// Token: 0x04007520 RID: 29984
		[MayRequireBiotech]
		public static ThingDef Mote_PollutionPump;

		// Token: 0x04007521 RID: 29985
		[MayRequireBiotech]
		public static ThingDef Mote_MechCharging;

		// Token: 0x04007522 RID: 29986
		[MayRequireBiotech]
		public static ThingDef Mote_ChargingCablesPulse;

		// Token: 0x04007523 RID: 29987
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_North;

		// Token: 0x04007524 RID: 29988
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_East;

		// Token: 0x04007525 RID: 29989
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_West;

		// Token: 0x04007526 RID: 29990
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_South;

		// Token: 0x04007527 RID: 29991
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_South;

		// Token: 0x04007528 RID: 29992
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_North;

		// Token: 0x04007529 RID: 29993
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_East;

		// Token: 0x0400752A RID: 29994
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_West;

		// Token: 0x0400752B RID: 29995
		[MayRequireBiotech]
		public static ThingDef Mote_XenogermImplantation;

		// Token: 0x0400752C RID: 29996
		[MayRequireBiotech]
		public static ThingDef Mote_MechResurrectWarmupOnTarget;

		// Token: 0x0400752D RID: 29997
		[MayRequireBiotech]
		public static ThingDef Mote_Toy;

		// Token: 0x0400752E RID: 29998
		[MayRequireBiotech]
		public static ThingDef Mote_Deathresting;

		// Token: 0x0400752F RID: 29999
		[MayRequireBiotech]
		public static ThingDef Mote_VatGlowHorizontal;

		// Token: 0x04007530 RID: 30000
		[MayRequireBiotech]
		public static ThingDef Mote_VatGlowVertical;

		// Token: 0x04007531 RID: 30001
		[MayRequireBiotech]
		public static ThingDef Mote_BabyCryingDots;

		// Token: 0x04007532 RID: 30002
		[MayRequireAnomaly]
		public static ThingDef Mote_HateChantCall;

		// Token: 0x04007533 RID: 30003
		[MayRequireAnomaly]
		public static ThingDef Mote_HateChantShadow;

		// Token: 0x04007534 RID: 30004
		[MayRequireAnomaly]
		public static ThingDef Mote_PsychicRitualInvocation;

		// Token: 0x04007535 RID: 30005
		[MayRequireAnomaly]
		public static ThingDef Mote_ChargingElectroCables;

		// Token: 0x04007536 RID: 30006
		[MayRequireAnomaly]
		public static ThingDef Mote_HarbingerTreeRoots;

		// Token: 0x04007537 RID: 30007
		[MayRequireRoyalty]
		public static ThingDef Throne;

		// Token: 0x04007538 RID: 30008
		[MayRequireRoyalty]
		public static ThingDef Harp;

		// Token: 0x04007539 RID: 30009
		[MayRequireRoyalty]
		public static ThingDef Harpsichord;

		// Token: 0x0400753A RID: 30010
		[MayRequireRoyalty]
		public static ThingDef Piano;

		// Token: 0x0400753B RID: 30011
		[MayRequireRoyalty]
		public static ThingDef ShipLandingBeacon;

		// Token: 0x0400753C RID: 30012
		[MayRequireRoyalty]
		public static ThingDef ActivatorCountdown;

		// Token: 0x0400753D RID: 30013
		[MayRequireAnomaly]
		public static ThingDef Mote_ShamblerAlert;

		// Token: 0x0400753E RID: 30014
		[MayRequireAnomaly]
		public static ThingDef Mote_PsychicBanshee;

		// Token: 0x0400753F RID: 30015
		[MayRequireAnomaly]
		public static ThingDef Mote_RevenantHeard;

		// Token: 0x04007540 RID: 30016
		[MayRequireAnyOf("Ludeon.RimWorld.Odyssey,Ludeon.RimWorld.Biotech")]
		public static ThingDef Mote_TerrorHalo;

		// Token: 0x04007541 RID: 30017
		public static ThingDef DelayedEffecterSpawner;

		// Token: 0x04007542 RID: 30018
		[MayRequireAnomaly]
		public static ThingDef DelayedMetalhorrorEmerger;
	}
}
