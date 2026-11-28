using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020020DB RID: 8411
	public struct ResolveParams
	{
		// Token: 0x0600D3B2 RID: 54194 RVA: 0x004E1CEB File Offset: 0x004DFEEB
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x0600D3B3 RID: 54195 RVA: 0x004E1CFB File Offset: 0x004DFEFB
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x0600D3B4 RID: 54196 RVA: 0x004E1D09 File Offset: 0x004DFF09
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x0600D3B5 RID: 54197 RVA: 0x004E1D18 File Offset: 0x004DFF18
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0600D3B6 RID: 54198 RVA: 0x004E1D28 File Offset: 0x004DFF28
		public override string ToString()
		{
			string[] array = new string[144];
			array[0] = "rect=";
			int num = 1;
			CellRect cellRect = this.rect;
			array[num] = cellRect.ToString();
			array[2] = ", faction=";
			array[3] = ((this.faction != null) ? this.faction.ToString() : "null");
			array[4] = ", custom=";
			array[5] = ((this.custom != null) ? this.custom.Count.ToString() : "null");
			array[6] = ", combatPoints=";
			array[7] = ((this.threatPoints != null) ? this.threatPoints.ToString() : "null");
			array[8] = ", pawnGroupMakerParams=";
			array[9] = ((this.pawnGroupMakerParams != null) ? this.pawnGroupMakerParams.ToString() : "null");
			array[10] = ", pawnGroupKindDef=";
			array[11] = ((this.pawnGroupKindDef != null) ? this.pawnGroupKindDef.ToString() : "null");
			array[12] = ", roofDef=";
			array[13] = ((this.roofDef != null) ? this.roofDef.ToString() : "null");
			array[14] = ", noRoof=";
			array[15] = ((this.noRoof != null) ? this.noRoof.ToString() : "null");
			array[16] = ", addRoomCenterToRootsToUnfog=";
			array[17] = ((this.addRoomCenterToRootsToUnfog != null) ? this.addRoomCenterToRootsToUnfog.ToString() : "null");
			array[18] = ", singleThingToSpawn=";
			array[19] = ((this.singleThingToSpawn != null) ? this.singleThingToSpawn.ToString() : "null");
			array[20] = ", singleThingDef=";
			array[21] = ((this.singleThingDef != null) ? this.singleThingDef.ToString() : "null");
			array[22] = ", singleThingStuff=";
			array[23] = ((this.singleThingStuff != null) ? this.singleThingStuff.ToString() : "null");
			array[24] = ", singleThingStackCount=";
			array[25] = ((this.singleThingStackCount != null) ? this.singleThingStackCount.ToString() : "null");
			array[26] = ", skipSingleThingIfHasToWipeBuildingOrDoesntFit=";
			array[27] = ((this.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null) ? this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString() : "null");
			array[28] = ", spawnBridgeIfTerrainCantSupportThing=";
			array[29] = ((this.spawnBridgeIfTerrainCantSupportThing != null) ? this.spawnBridgeIfTerrainCantSupportThing.ToString() : "null");
			array[30] = ", singleThingInnerThings=";
			array[31] = ((this.singleThingInnerThings != null) ? this.singleThingInnerThings.ToString() : "null");
			array[32] = ", singlePawnToSpawn=";
			array[33] = ((this.singlePawnToSpawn != null) ? this.singlePawnToSpawn.ToString() : "null");
			array[34] = ", singlePawnKindDef=";
			array[35] = ((this.singlePawnKindDef != null) ? this.singlePawnKindDef.ToString() : "null");
			array[36] = ", disableSinglePawn=";
			array[37] = ((this.disableSinglePawn != null) ? this.disableSinglePawn.ToString() : "null");
			array[38] = ", singlePawnLord=";
			array[39] = ((this.singlePawnLord != null) ? this.singlePawnLord.ToString() : "null");
			array[40] = ", settlementLord=";
			array[41] = ((this.settlementLord != null) ? this.settlementLord.ToString() : "null");
			array[42] = ", singlePawnSpawnCellExtraPredicate=";
			array[43] = ((this.singlePawnSpawnCellExtraPredicate != null) ? this.singlePawnSpawnCellExtraPredicate.ToString() : "null");
			array[44] = ", singlePawnGenerationRequest=";
			array[45] = ((this.singlePawnGenerationRequest != null) ? this.singlePawnGenerationRequest.ToString() : "null");
			array[46] = ", postThingSpawn=";
			array[47] = ((this.postThingSpawn != null) ? this.postThingSpawn.ToString() : "null");
			array[48] = ", postThingGenerate=";
			array[49] = ((this.postThingGenerate != null) ? this.postThingGenerate.ToString() : "null");
			array[50] = ", mechanoidsCount=";
			array[51] = ((this.mechanoidsCount != null) ? this.mechanoidsCount.ToString() : "null");
			array[52] = ", wakeSleepingMechanoidsOnUnfogged=";
			array[53] = ((this.wakeSleepingMechanoidsOnUnfogged != null) ? this.wakeSleepingMechanoidsOnUnfogged.ToString() : "null");
			array[54] = ", hivesCount=";
			array[55] = ((this.hivesCount != null) ? this.hivesCount.ToString() : "null");
			array[56] = ", disableHives=";
			array[57] = ((this.disableHives != null) ? this.disableHives.ToString() : "null");
			array[58] = ", thingRot=";
			array[59] = ((this.thingRot != null) ? this.thingRot.ToString() : "null");
			array[60] = ", wallStuff=";
			array[61] = ((this.wallStuff != null) ? this.wallStuff.ToString() : "null");
			array[62] = ", chanceToSkipWallBlock=";
			array[63] = ((this.chanceToSkipWallBlock != null) ? this.chanceToSkipWallBlock.ToString() : "null");
			array[64] = ", floorDef=";
			array[65] = ((this.floorDef != null) ? this.floorDef.ToString() : "null");
			array[66] = ", chanceToSkipFloor=";
			array[67] = ((this.chanceToSkipFloor != null) ? this.chanceToSkipFloor.ToString() : "null");
			array[68] = ", filthDef=";
			array[69] = ((this.filthDef != null) ? this.filthDef.ToString() : "null");
			array[70] = ", filthDensity=";
			array[71] = ((this.filthDensity != null) ? this.filthDensity.ToString() : "null");
			array[72] = ", floorOnlyIfTerrainSupports=";
			array[73] = ((this.floorOnlyIfTerrainSupports != null) ? this.floorOnlyIfTerrainSupports.ToString() : "null");
			array[74] = ", allowBridgeOnAnyImpassableTerrain=";
			array[75] = ((this.allowBridgeOnAnyImpassableTerrain != null) ? this.allowBridgeOnAnyImpassableTerrain.ToString() : "null");
			array[76] = ", clearEdificeOnly=";
			array[77] = ((this.clearEdificeOnly != null) ? this.clearEdificeOnly.ToString() : "null");
			array[78] = ", clearFillageOnly=";
			array[79] = ((this.clearFillageOnly != null) ? this.clearFillageOnly.ToString() : "null");
			array[80] = ", clearRoof=";
			array[81] = ((this.clearRoof != null) ? this.clearRoof.ToString() : "null");
			array[82] = ", ancientCryptosleepCasketGroupID=";
			array[83] = ((this.ancientCryptosleepCasketGroupID != null) ? this.ancientCryptosleepCasketGroupID.ToString() : "null");
			array[84] = ", podContentsType=";
			array[85] = ((this.podContentsType != null) ? this.podContentsType.ToString() : "null");
			array[86] = ", ancientCryptosleepCasketOpenSignalTag=";
			array[87] = ((this.ancientCryptosleepCasketOpenSignalTag != null) ? this.ancientCryptosleepCasketOpenSignalTag.ToString() : "null");
			array[88] = ", thingSetMakerDef=";
			array[89] = ((this.thingSetMakerDef != null) ? this.thingSetMakerDef.ToString() : "null");
			array[90] = ", thingSetMakerParams=";
			array[91] = ((this.thingSetMakerParams != null) ? this.thingSetMakerParams.ToString() : "null");
			array[92] = ", stockpileConcreteContents=";
			array[93] = ((this.stockpileConcreteContents != null) ? this.stockpileConcreteContents.Count.ToString() : "null");
			array[94] = ", stockpileMarketValue=";
			array[95] = ((this.stockpileMarketValue != null) ? this.stockpileMarketValue.ToString() : "null");
			array[96] = ", innerStockpileSize=";
			array[97] = ((this.innerStockpileSize != null) ? this.innerStockpileSize.ToString() : "null");
			array[98] = ", edgeDefenseWidth=";
			array[99] = ((this.edgeDefenseWidth != null) ? this.edgeDefenseWidth.ToString() : "null");
			array[100] = ", edgeDefenseTurretsCount=";
			array[101] = ((this.edgeDefenseTurretsCount != null) ? this.edgeDefenseTurretsCount.ToString() : "null");
			array[102] = ", edgeDefenseMortarsCount=";
			array[103] = ((this.edgeDefenseMortarsCount != null) ? this.edgeDefenseMortarsCount.ToString() : "null");
			array[104] = ", edgeDefenseGuardsCount=";
			array[105] = ((this.edgeDefenseGuardsCount != null) ? this.edgeDefenseGuardsCount.ToString() : "null");
			array[106] = ", mortarDef=";
			array[107] = ((this.mortarDef != null) ? this.mortarDef.ToString() : "null");
			array[108] = ", pathwayFloorDef=";
			array[109] = ((this.pathwayFloorDef != null) ? this.pathwayFloorDef.ToString() : "null");
			array[110] = ", cultivatedPlantDef=";
			array[111] = ((this.cultivatedPlantDef != null) ? this.cultivatedPlantDef.ToString() : "null");
			array[112] = ", fixedCulativedPlantGrowth=";
			array[113] = ((this.fixedCulativedPlantGrowth != null) ? this.fixedCulativedPlantGrowth.ToString() : "null");
			array[114] = ", fillWithThingsPadding=";
			array[115] = ((this.fillWithThingsPadding != null) ? this.fillWithThingsPadding.ToString() : "null");
			array[116] = ", settlementPawnGroupPoints=";
			array[117] = ((this.settlementPawnGroupPoints != null) ? this.settlementPawnGroupPoints.ToString() : "null");
			array[118] = ", settlementPawnGroupSeed=";
			array[119] = ((this.settlementPawnGroupSeed != null) ? this.settlementPawnGroupSeed.ToString() : "null");
			array[120] = ", settlementDontGeneratePawns=";
			array[121] = ((this.settlementDontGeneratePawns != null) ? this.settlementDontGeneratePawns.ToString() : "null");
			array[122] = ", attackWhenPlayerBecameEnemy=";
			array[123] = ((this.attackWhenPlayerBecameEnemy != null) ? this.attackWhenPlayerBecameEnemy.ToString() : "null");
			array[124] = ", streetHorizontal=";
			array[125] = ((this.streetHorizontal != null) ? this.streetHorizontal.ToString() : "null");
			array[126] = ", edgeThingAvoidOtherEdgeThings=";
			array[127] = ((this.edgeThingAvoidOtherEdgeThings != null) ? this.edgeThingAvoidOtherEdgeThings.ToString() : "null");
			array[128] = ", edgeThingMustReachMapEdge=";
			array[129] = ((this.edgeThingMustReachMapEdge != null) ? this.edgeThingMustReachMapEdge.ToString() : "null");
			array[130] = ", allowPlacementOffEdge=";
			array[131] = ((this.allowPlacementOffEdge != null) ? this.allowPlacementOffEdge.ToString() : "null");
			array[132] = ", thrustAxis=";
			array[133] = ((this.thrustAxis != null) ? this.thrustAxis.ToString() : "null");
			array[134] = ", makeWarningLetter=";
			array[135] = ((this.makeWarningLetter != null) ? this.makeWarningLetter.ToString() : "null");
			array[136] = ", allowedMonumentThings=";
			array[137] = ((this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null");
			array[138] = ", bedCount=";
			array[139] = ((this.bedCount != null) ? this.bedCount.ToString() : string.Concat(new string[]
			{
				"null, workSitePoints=",
				(this.workSitePoints != 0f) ? this.workSitePoints.ToString() : "null",
				", lootConcereteContents=",
				(this.lootConcereteContents != null) ? this.lootConcereteContents.ToString() : "null",
				", lootMarketValue=",
				(this.lootMarketValue != null) ? this.lootMarketValue.ToString() : "null"
			}));
			array[140] = ", extraDoorEdge=";
			array[141] = ((this.extraDoorEdge != null) ? this.extraDoorEdge.ToString() : "null");
			array[142] = ", minLengthAfterSplit=";
			array[143] = ((this.minLengthAfterSplit != null) ? this.minLengthAfterSplit.ToString() : "null");
			return string.Concat(array);
		}

		// Token: 0x0400861E RID: 34334
		public CellRect rect;

		// Token: 0x0400861F RID: 34335
		public Faction faction;

		// Token: 0x04008620 RID: 34336
		public float? threatPoints;

		// Token: 0x04008621 RID: 34337
		private Dictionary<string, object> custom;

		// Token: 0x04008622 RID: 34338
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x04008623 RID: 34339
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04008624 RID: 34340
		public RoofDef roofDef;

		// Token: 0x04008625 RID: 34341
		public bool? noRoof;

		// Token: 0x04008626 RID: 34342
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x04008627 RID: 34343
		public Thing singleThingToSpawn;

		// Token: 0x04008628 RID: 34344
		public ThingDef singleThingDef;

		// Token: 0x04008629 RID: 34345
		public ThingDef singleThingStuff;

		// Token: 0x0400862A RID: 34346
		public int? singleThingStackCount;

		// Token: 0x0400862B RID: 34347
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x0400862C RID: 34348
		public bool? spawnBridgeIfTerrainCantSupportThing;

		// Token: 0x0400862D RID: 34349
		public bool? spawnOutside;

		// Token: 0x0400862E RID: 34350
		public List<Thing> singleThingInnerThings;

		// Token: 0x0400862F RID: 34351
		public Pawn singlePawnToSpawn;

		// Token: 0x04008630 RID: 34352
		public PawnKindDef singlePawnKindDef;

		// Token: 0x04008631 RID: 34353
		public bool? disableSinglePawn;

		// Token: 0x04008632 RID: 34354
		public Lord singlePawnLord;

		// Token: 0x04008633 RID: 34355
		public Lord settlementLord;

		// Token: 0x04008634 RID: 34356
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x04008635 RID: 34357
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x04008636 RID: 34358
		public Action<Thing> postThingSpawn;

		// Token: 0x04008637 RID: 34359
		public Action<Thing> postThingGenerate;

		// Token: 0x04008638 RID: 34360
		public int? mechanoidsCount;

		// Token: 0x04008639 RID: 34361
		public bool? wakeSleepingMechanoidsOnUnfogged;

		// Token: 0x0400863A RID: 34362
		public int? fleshbeastsCount;

		// Token: 0x0400863B RID: 34363
		public int? hivesCount;

		// Token: 0x0400863C RID: 34364
		public bool? disableHives;

		// Token: 0x0400863D RID: 34365
		public Rot4? thingRot;

		// Token: 0x0400863E RID: 34366
		public ThingDef wallStuff;

		// Token: 0x0400863F RID: 34367
		public float? chanceToSkipWallBlock;

		// Token: 0x04008640 RID: 34368
		public ThingDef wallThingDef;

		// Token: 0x04008641 RID: 34369
		public TerrainDef floorDef;

		// Token: 0x04008642 RID: 34370
		public float? chanceToSkipFloor;

		// Token: 0x04008643 RID: 34371
		public ThingDef filthDef;

		// Token: 0x04008644 RID: 34372
		public FloatRange? filthDensity;

		// Token: 0x04008645 RID: 34373
		public bool? floorOnlyIfTerrainSupports;

		// Token: 0x04008646 RID: 34374
		public bool? allowBridgeOnAnyImpassableTerrain;

		// Token: 0x04008647 RID: 34375
		public bool? clearEdificeOnly;

		// Token: 0x04008648 RID: 34376
		public bool? clearFillageOnly;

		// Token: 0x04008649 RID: 34377
		public bool? clearRoof;

		// Token: 0x0400864A RID: 34378
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x0400864B RID: 34379
		public PodContentsType? podContentsType;

		// Token: 0x0400864C RID: 34380
		public string ancientCryptosleepCasketOpenSignalTag;

		// Token: 0x0400864D RID: 34381
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400864E RID: 34382
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x0400864F RID: 34383
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x04008650 RID: 34384
		public float? stockpileMarketValue;

		// Token: 0x04008651 RID: 34385
		public int? innerStockpileSize;

		// Token: 0x04008652 RID: 34386
		public int? edgeDefenseWidth;

		// Token: 0x04008653 RID: 34387
		public int? edgeDefenseTurretsCount;

		// Token: 0x04008654 RID: 34388
		public int? edgeDefenseMortarsCount;

		// Token: 0x04008655 RID: 34389
		public int? edgeDefenseGuardsCount;

		// Token: 0x04008656 RID: 34390
		public ThingDef mortarDef;

		// Token: 0x04008657 RID: 34391
		public TerrainDef pathwayFloorDef;

		// Token: 0x04008658 RID: 34392
		public ThingDef cultivatedPlantDef;

		// Token: 0x04008659 RID: 34393
		public float? fixedCulativedPlantGrowth;

		// Token: 0x0400865A RID: 34394
		public int? fillWithThingsPadding;

		// Token: 0x0400865B RID: 34395
		public float? settlementPawnGroupPoints;

		// Token: 0x0400865C RID: 34396
		public int? settlementPawnGroupSeed;

		// Token: 0x0400865D RID: 34397
		public bool? settlementDontGeneratePawns;

		// Token: 0x0400865E RID: 34398
		public bool? attackWhenPlayerBecameEnemy;

		// Token: 0x0400865F RID: 34399
		public bool? streetHorizontal;

		// Token: 0x04008660 RID: 34400
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x04008661 RID: 34401
		public bool? edgeThingMustReachMapEdge;

		// Token: 0x04008662 RID: 34402
		public bool? allowPlacementOffEdge;

		// Token: 0x04008663 RID: 34403
		public Rot4? thrustAxis;

		// Token: 0x04008664 RID: 34404
		public FloatRange? hpPercentRange;

		// Token: 0x04008665 RID: 34405
		public Thing conditionCauser;

		// Token: 0x04008666 RID: 34406
		public bool? makeWarningLetter;

		// Token: 0x04008667 RID: 34407
		public ThingFilter allowedMonumentThings;

		// Token: 0x04008668 RID: 34408
		public bool? allowGeneratingThronerooms;

		// Token: 0x04008669 RID: 34409
		public int? bedCount;

		// Token: 0x0400866A RID: 34410
		public float workSitePoints;

		// Token: 0x0400866B RID: 34411
		public IList<Thing> lootConcereteContents;

		// Token: 0x0400866C RID: 34412
		public float? lootMarketValue;

		// Token: 0x0400866D RID: 34413
		public Rot4? extraDoorEdge;

		// Token: 0x0400866E RID: 34414
		public string sleepingInsectsWakeupSignalTag;

		// Token: 0x0400866F RID: 34415
		public string sleepingMechanoidsWakeupSignalTag;

		// Token: 0x04008670 RID: 34416
		public string rectTriggerSignalTag;

		// Token: 0x04008671 RID: 34417
		public bool? destroyIfUnfogged;

		// Token: 0x04008672 RID: 34418
		public string messageSignalTag;

		// Token: 0x04008673 RID: 34419
		public string message;

		// Token: 0x04008674 RID: 34420
		public MessageTypeDef messageType;

		// Token: 0x04008675 RID: 34421
		public LookTargets lookTargets;

		// Token: 0x04008676 RID: 34422
		public bool? spawnAnywhereIfNoGoodCell;

		// Token: 0x04008677 RID: 34423
		public bool? ignoreRoofedRequirement;

		// Token: 0x04008678 RID: 34424
		public IntVec3? overrideLoc;

		// Token: 0x04008679 RID: 34425
		public bool? sendStandardLetter;

		// Token: 0x0400867A RID: 34426
		public string infestationSignalTag;

		// Token: 0x0400867B RID: 34427
		public float? insectsPoints;

		// Token: 0x0400867C RID: 34428
		public string ambushSignalTag;

		// Token: 0x0400867D RID: 34429
		public SignalActionAmbushType? ambushType;

		// Token: 0x0400867E RID: 34430
		public IntVec3? spawnNear;

		// Token: 0x0400867F RID: 34431
		public CellRect? spawnAround;

		// Token: 0x04008680 RID: 34432
		public bool? spawnPawnsOnEdge;

		// Token: 0x04008681 RID: 34433
		public float? ambushPoints;

		// Token: 0x04008682 RID: 34434
		public int? cornerRadius;

		// Token: 0x04008683 RID: 34435
		public SoundDef sound;

		// Token: 0x04008684 RID: 34436
		public string soundOneShotActionSignalTag;

		// Token: 0x04008685 RID: 34437
		public string unfoggedSignalTag;

		// Token: 0x04008686 RID: 34438
		public Thing triggerContainerEmptiedThing;

		// Token: 0x04008687 RID: 34439
		public string triggerContainerEmptiedSignalTag;

		// Token: 0x04008688 RID: 34440
		public Building_Door openDoorActionDoor;

		// Token: 0x04008689 RID: 34441
		public string openDoorActionSignalTag;

		// Token: 0x0400868A RID: 34442
		public string triggerSecuritySignal;

		// Token: 0x0400868B RID: 34443
		public Thing relicThing;

		// Token: 0x0400868C RID: 34444
		public float? exteriorThreatPoints;

		// Token: 0x0400868D RID: 34445
		public float? interiorThreatPoints;

		// Token: 0x0400868E RID: 34446
		public SitePart sitePart;

		// Token: 0x0400868F RID: 34447
		public int? minLengthAfterSplit;

		// Token: 0x04008690 RID: 34448
		public bool? sendWokenUpMessage;

		// Token: 0x04008691 RID: 34449
		public LayoutStructureSketch ancientLayoutStructureSketch;

		// Token: 0x04008692 RID: 34450
		public bool? ignoreDoorways;

		// Token: 0x04008693 RID: 34451
		public int? minorBuildingCount;

		// Token: 0x04008694 RID: 34452
		public int? minorBuildingRadialDistance;

		// Token: 0x04008695 RID: 34453
		public ThingDef centralBuilding;

		// Token: 0x04008696 RID: 34454
		public PawnKindDef desiccatedCorpsePawnKind;

		// Token: 0x04008697 RID: 34455
		public IntRange? desiccatedCorpseRandomAgeRange;

		// Token: 0x04008698 RID: 34456
		public FloatRange? dessicatedCorpseDensityRange;

		// Token: 0x04008699 RID: 34457
		public float edgeUnpolluteChance;
	}
}
