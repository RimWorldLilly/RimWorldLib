using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBA RID: 3770
	public class BuildingProperties
	{
		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06006333 RID: 25395 RVA: 0x0023A72F File Offset: 0x0023892F
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06006334 RID: 25396 RVA: 0x0023A73A File Offset: 0x0023893A
		public int EffectiveMineableYield
		{
			get
			{
				return Mathf.RoundToInt((float)this.mineableYield * Find.Storyteller.difficulty.mineYieldFactor);
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06006335 RID: 25397 RVA: 0x0023A758 File Offset: 0x00238958
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06006336 RID: 25398 RVA: 0x0023A763 File Offset: 0x00238963
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06006337 RID: 25399 RVA: 0x0023A780 File Offset: 0x00238980
		public bool IsMortar
		{
			get
			{
				if (!this.IsTurret)
				{
					return false;
				}
				List<VerbProperties> verbs = this.turretGunDef.Verbs;
				for (int i = 0; i < verbs.Count; i++)
				{
					if (verbs[i].isPrimary && verbs[i].defaultProjectile != null && verbs[i].defaultProjectile.projectile.flyOverhead)
					{
						return true;
					}
				}
				if (this.turretGunDef.HasComp(typeof(CompChangeableProjectile)))
				{
					if (this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive))
					{
						return true;
					}
					foreach (ThingDef thingDef in this.turretGunDef.building.fixedStorageSettings.filter.AllowedThingDefs)
					{
						if (thingDef.projectileWhenLoaded != null && thingDef.projectileWhenLoaded.projectile.flyOverhead)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x0023A894 File Offset: 0x00238A94
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			BuildingProperties.<ConfigErrors>d__215 <ConfigErrors>d__ = new BuildingProperties.<ConfigErrors>d__215(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parent = parent;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x0023A8AB File Offset: 0x00238AAB
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x0600633A RID: 25402 RVA: 0x0023A8B0 File Offset: 0x00238AB0
		public void ResolveReferencesSpecial()
		{
			if (this.soundDoorOpenPowered == null)
			{
				this.soundDoorOpenPowered = SoundDefOf.Door_OpenPowered;
			}
			if (this.soundDoorClosePowered == null)
			{
				this.soundDoorClosePowered = SoundDefOf.Door_ClosePowered;
			}
			if (this.soundDoorOpenManual == null)
			{
				this.soundDoorOpenManual = SoundDefOf.Door_OpenManual;
			}
			if (this.soundDoorCloseManual == null)
			{
				this.soundDoorCloseManual = SoundDefOf.Door_CloseManual;
			}
			if (this.turretGunDef != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretGunDef.graphicData.texPath);
				});
			}
			if (this.turretTopLoadedGraphic != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopLoadedMat = MaterialPool.MatFrom(this.turretTopLoadedGraphic.texPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null && this.fixedStorageSettings != null)
			{
				this.defaultStorageSettings = new StorageSettings();
				this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x0023A998 File Offset: 0x00238B98
		public static void FinalizeInit()
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				BuildingProperties building = thingDef.building;
				if (((building != null) ? building.smoothedThing : null) != null)
				{
					ThingDef thingDef2 = thingDef.building.smoothedThing;
					if (thingDef2.building == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
						defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(thingDef);
						defaultInterpolatedStringHandler.AppendLiteral(" is smoothable to non-building ");
						defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(thingDef2);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
					{
						thingDef2.building.unsmoothedThing = thingDef;
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(21, 3);
						defaultInterpolatedStringHandler2.AppendFormatted<ThingDef>(thingDef);
						defaultInterpolatedStringHandler2.AppendLiteral(" and ");
						defaultInterpolatedStringHandler2.AppendFormatted<ThingDef>(thingDef2.building.unsmoothedThing);
						defaultInterpolatedStringHandler2.AppendLiteral(" both smooth to ");
						defaultInterpolatedStringHandler2.AppendFormatted<ThingDef>(thingDef2);
						Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x0600633C RID: 25404 RVA: 0x0023AAA2 File Offset: 0x00238CA2
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			BuildingProperties.<SpecialDisplayStats>d__220 <SpecialDisplayStats>d__ = new BuildingProperties.<SpecialDisplayStats>d__220(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__parentDef = parentDef;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600633D RID: 25405 RVA: 0x0023AAC0 File Offset: 0x00238CC0
		public GenDraw.FillableBarRequest BarDrawDataFor(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.barDrawData.north;
			case 1:
				return this.barDrawData.east;
			case 2:
				return this.barDrawData.south;
			case 3:
				return this.barDrawData.west;
			default:
				return this.barDrawData.north;
			}
		}

		// Token: 0x040036AA RID: 13994
		public bool isEdifice = true;

		// Token: 0x040036AB RID: 13995
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x040036AC RID: 13996
		public bool isInert;

		// Token: 0x040036AD RID: 13997
		public bool isTargetable = true;

		// Token: 0x040036AE RID: 13998
		private bool deconstructible = true;

		// Token: 0x040036AF RID: 13999
		public bool alwaysDeconstructible;

		// Token: 0x040036B0 RID: 14000
		public bool alwaysUninstallable;

		// Token: 0x040036B1 RID: 14001
		public List<ThingDef> leavingsBlacklist;

		// Token: 0x040036B2 RID: 14002
		public List<ThingDef> forcedCostLeavings;

		// Token: 0x040036B3 RID: 14003
		public bool claimable = true;

		// Token: 0x040036B4 RID: 14004
		public bool isSittable;

		// Token: 0x040036B5 RID: 14005
		public bool multiSittable;

		// Token: 0x040036B6 RID: 14006
		public bool sitIgnoreOrientation;

		// Token: 0x040036B7 RID: 14007
		public ConceptDef spawnedConceptLearnOpportunity;

		// Token: 0x040036B8 RID: 14008
		public ConceptDef boughtConceptLearnOpportunity;

		// Token: 0x040036B9 RID: 14009
		public bool expandHomeArea = true;

		// Token: 0x040036BA RID: 14010
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x040036BB RID: 14011
		public GraphicData blueprintGraphicData;

		// Token: 0x040036BC RID: 14012
		public float uninstallWork = 200f;

		// Token: 0x040036BD RID: 14013
		public bool forceShowRoomStats;

		// Token: 0x040036BE RID: 14014
		public bool neverBuildable;

		// Token: 0x040036BF RID: 14015
		public bool isWall;

		// Token: 0x040036C0 RID: 14016
		public bool alwaysExchangeVacuum;

		// Token: 0x040036C1 RID: 14017
		public bool canExchangeVacuum;

		// Token: 0x040036C2 RID: 14018
		public bool draftAttackNonDeconstructable = true;

		// Token: 0x040036C3 RID: 14019
		public bool wantsHopperAdjacent;

		// Token: 0x040036C4 RID: 14020
		public bool allowWireConnection = true;

		// Token: 0x040036C5 RID: 14021
		public bool shipPart;

		// Token: 0x040036C6 RID: 14022
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x040036C7 RID: 14023
		public float heatPerTickWhileWorking;

		// Token: 0x040036C8 RID: 14024
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x040036C9 RID: 14025
		public bool canPlaceOverWall;

		// Token: 0x040036CA RID: 14026
		public bool isPlaceOverableWall;

		// Token: 0x040036CB RID: 14027
		public bool allowAutoroof = true;

		// Token: 0x040036CC RID: 14028
		public bool preventDeteriorationOnTop;

		// Token: 0x040036CD RID: 14029
		public bool preventDeteriorationInside;

		// Token: 0x040036CE RID: 14030
		public bool isMealSource;

		// Token: 0x040036CF RID: 14031
		public bool isNaturalRock;

		// Token: 0x040036D0 RID: 14032
		public bool isResourceRock;

		// Token: 0x040036D1 RID: 14033
		public bool repairable = true;

		// Token: 0x040036D2 RID: 14034
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x040036D3 RID: 14035
		public bool hasFuelingPort;

		// Token: 0x040036D4 RID: 14036
		public ThingDef smoothedThing;

		// Token: 0x040036D5 RID: 14037
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		// Token: 0x040036D6 RID: 14038
		public TerrainDef naturalTerrain;

		// Token: 0x040036D7 RID: 14039
		public TerrainDef leaveTerrain;

		// Token: 0x040036D8 RID: 14040
		public float combatPower;

		// Token: 0x040036D9 RID: 14041
		public int minMechClusterPoints;

		// Token: 0x040036DA RID: 14042
		public float destroyShakeAmount = -1f;

		// Token: 0x040036DB RID: 14043
		public SoundDef destroySound;

		// Token: 0x040036DC RID: 14044
		public SoundDef soundMeleeHitOverride;

		// Token: 0x040036DD RID: 14045
		public EffecterDef destroyEffecter;

		// Token: 0x040036DE RID: 14046
		public BuildingSizeCategory buildingSizeCategory;

		// Token: 0x040036DF RID: 14047
		public bool isFence;

		// Token: 0x040036E0 RID: 14048
		public List<ThingDef> relatedBuildCommands;

		// Token: 0x040036E1 RID: 14049
		public List<TerrainDef> relatedTerrain;

		// Token: 0x040036E2 RID: 14050
		public bool alwaysShowRelatedBuildCommands;

		// Token: 0x040036E3 RID: 14051
		public bool useIdeoColor;

		// Token: 0x040036E4 RID: 14052
		public bool wakeDormantPawnsOnConstruction = true;

		// Token: 0x040036E5 RID: 14053
		public int maxItemsInCell = 1;

		// Token: 0x040036E6 RID: 14054
		public bool paintable;

		// Token: 0x040036E7 RID: 14055
		public bool canBeDamagedByAttacks = true;

		// Token: 0x040036E8 RID: 14056
		public bool isHopper;

		// Token: 0x040036E9 RID: 14057
		public bool quickTargetable;

		// Token: 0x040036EA RID: 14058
		public bool displayAttackToDestroyOnInspectPane;

		// Token: 0x040036EB RID: 14059
		public bool isEscapableContainer;

		// Token: 0x040036EC RID: 14060
		public bool isPowerConduit;

		// Token: 0x040036ED RID: 14061
		public bool biomeSpecific;

		// Token: 0x040036EE RID: 14062
		public bool crater;

		// Token: 0x040036EF RID: 14063
		public bool canLandGravshipOn;

		// Token: 0x040036F0 RID: 14064
		public bool isAirtight;

		// Token: 0x040036F1 RID: 14065
		public bool isStuffableAirtight;

		// Token: 0x040036F2 RID: 14066
		public bool canConstructInCorner;

		// Token: 0x040036F3 RID: 14067
		public bool isAttachment;

		// Token: 0x040036F4 RID: 14068
		public bool supportsWallAttachments;

		// Token: 0x040036F5 RID: 14069
		public bool isPlayerEjectable;

		// Token: 0x040036F6 RID: 14070
		public SoundDef openingStartedSound;

		// Token: 0x040036F7 RID: 14071
		public GraphicData fullGraveGraphicData;

		// Token: 0x040036F8 RID: 14072
		public float bed_healPerDay;

		// Token: 0x040036F9 RID: 14073
		public bool bed_defaultMedical;

		// Token: 0x040036FA RID: 14074
		public bool bed_showSleeperBody;

		// Token: 0x040036FB RID: 14075
		public bool bed_humanlike = true;

		// Token: 0x040036FC RID: 14076
		public float bed_maxBodySize = 9999f;

		// Token: 0x040036FD RID: 14077
		public bool bed_caravansCanUse;

		// Token: 0x040036FE RID: 14078
		public bool bed_slabBed;

		// Token: 0x040036FF RID: 14079
		public bool bed_crib;

		// Token: 0x04003700 RID: 14080
		public float bed_pawnDrawOffset;

		// Token: 0x04003701 RID: 14081
		public bool bed_canBeMedical = true;

		// Token: 0x04003702 RID: 14082
		public bool bed_DisplayOwnerType = true;

		// Token: 0x04003703 RID: 14083
		public bool bed_DisplayOwnersInInspectString = true;

		// Token: 0x04003704 RID: 14084
		public bool bed_countsForBedroomOrBarracks = true;

		// Token: 0x04003705 RID: 14085
		public bool bed_emptyCountsForBarracks = true;

		// Token: 0x04003706 RID: 14086
		public bool bed_UseSheetColor = true;

		// Token: 0x04003707 RID: 14087
		public float nutritionCostPerDispense;

		// Token: 0x04003708 RID: 14088
		public SoundDef soundDispense;

		// Token: 0x04003709 RID: 14089
		public ThingDef turretGunDef;

		// Token: 0x0400370A RID: 14090
		public FloatRange turretBurstWarmupTime = new FloatRange(0.25f, 0.75f);

		// Token: 0x0400370B RID: 14091
		public float turretBurstCooldownTime = -1f;

		// Token: 0x0400370C RID: 14092
		public float turretInitialCooldownTime;

		// Token: 0x0400370D RID: 14093
		[Unsaved(false)]
		public Material turretTopMat;

		// Token: 0x0400370E RID: 14094
		[Unsaved(false)]
		public Material turretTopLoadedMat;

		// Token: 0x0400370F RID: 14095
		public float turretTopDrawSize = 2f;

		// Token: 0x04003710 RID: 14096
		public Vector2 turretTopOffset;

		// Token: 0x04003711 RID: 14097
		public bool playTargetAcquiredSound = true;

		// Token: 0x04003712 RID: 14098
		public GraphicData turretTopLoadedGraphic;

		// Token: 0x04003713 RID: 14099
		public List<MechWeightClassDef> requiredMechWeightClasses;

		// Token: 0x04003714 RID: 14100
		public bool ai_combatDangerous;

		// Token: 0x04003715 RID: 14101
		public bool ai_chillDestination = true;

		// Token: 0x04003716 RID: 14102
		public bool ai_neverTrashThis;

		// Token: 0x04003717 RID: 14103
		public bool preferConnectingToFences;

		// Token: 0x04003718 RID: 14104
		public bool roamerCanOpen;

		// Token: 0x04003719 RID: 14105
		public SoundDef soundDoorOpenPowered;

		// Token: 0x0400371A RID: 14106
		public SoundDef soundDoorClosePowered;

		// Token: 0x0400371B RID: 14107
		public SoundDef soundDoorOpenManual;

		// Token: 0x0400371C RID: 14108
		public SoundDef soundDoorCloseManual;

		// Token: 0x0400371D RID: 14109
		public SoundDef soundDoorCloseEnd;

		// Token: 0x0400371E RID: 14110
		public float poweredDoorOpenSpeedFactor = 1f;

		// Token: 0x0400371F RID: 14111
		public float poweredDoorCloseSpeedFactor = 1f;

		// Token: 0x04003720 RID: 14112
		public float unpoweredDoorOpenSpeedFactor = 1f;

		// Token: 0x04003721 RID: 14113
		public float unpoweredDoorCloseSpeedFactor = 1f;

		// Token: 0x04003722 RID: 14114
		public int doorTempEqualizeIntervalClosed = 375;

		// Token: 0x04003723 RID: 14115
		public int doorTempEqualizeIntervalOpen = 34;

		// Token: 0x04003724 RID: 14116
		public float doorTempEqualizeRate = 1f;

		// Token: 0x04003725 RID: 14117
		[NoTranslate]
		public string sowTag;

		// Token: 0x04003726 RID: 14118
		public ThingDef defaultPlantToGrow;

		// Token: 0x04003727 RID: 14119
		public ThingDef mineableThing;

		// Token: 0x04003728 RID: 14120
		public int mineableYield = 1;

		// Token: 0x04003729 RID: 14121
		public bool veinMineable;

		// Token: 0x0400372A RID: 14122
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x0400372B RID: 14123
		public float mineableDropChance = 1f;

		// Token: 0x0400372C RID: 14124
		public bool mineableYieldWasteable = true;

		// Token: 0x0400372D RID: 14125
		public float mineableScatterCommonality;

		// Token: 0x0400372E RID: 14126
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x0400372F RID: 14127
		public bool mineablePreventMeteorite;

		// Token: 0x04003730 RID: 14128
		public bool mineablePreventNaturalRockOnSurface;

		// Token: 0x04003731 RID: 14129
		public StorageSettings fixedStorageSettings;

		// Token: 0x04003732 RID: 14130
		public StorageSettings defaultStorageSettings;

		// Token: 0x04003733 RID: 14131
		public bool ignoreStoredThingsBeauty;

		// Token: 0x04003734 RID: 14132
		public string storageGroupTag;

		// Token: 0x04003735 RID: 14133
		public string groupingLabel;

		// Token: 0x04003736 RID: 14134
		public int groupingOrder;

		// Token: 0x04003737 RID: 14135
		public bool isTrap;

		// Token: 0x04003738 RID: 14136
		public bool trapDestroyOnSpring;

		// Token: 0x04003739 RID: 14137
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		// Token: 0x0400373A RID: 14138
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x0400373B RID: 14139
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x0400373C RID: 14140
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		// Token: 0x0400373D RID: 14141
		public RoomRoleDef workTableRoomRole;

		// Token: 0x0400373E RID: 14142
		public float workTableNotInRoomRoleFactor = 1f;

		// Token: 0x0400373F RID: 14143
		public float unpoweredWorkTableWorkSpeedFactor;

		// Token: 0x04003740 RID: 14144
		public SoundDef workTableCompleteSoundDef;

		// Token: 0x04003741 RID: 14145
		public IntRange watchBuildingStandDistanceRange = IntRange.One;

		// Token: 0x04003742 RID: 14146
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x04003743 RID: 14147
		public bool watchBuildingInSameRoom;

		// Token: 0x04003744 RID: 14148
		public JoyKindDef joyKind;

		// Token: 0x04003745 RID: 14149
		public int haulToContainerDuration;

		// Token: 0x04003746 RID: 14150
		public float instrumentRange;

		// Token: 0x04003747 RID: 14151
		public int minDistanceToSameTypeOfBuilding;

		// Token: 0x04003748 RID: 14152
		public bool artificialForMeditationPurposes = true;

		// Token: 0x04003749 RID: 14153
		public EffecterDef effectWatching;

		// Token: 0x0400374A RID: 14154
		public GraphicData gibbetCageTopGraphicData;

		// Token: 0x0400374B RID: 14155
		public Vector3 gibbetCorposeDrawOffset;

		// Token: 0x0400374C RID: 14156
		public EffecterDef gibbetCagePlaceCorpseEffecter;

		// Token: 0x0400374D RID: 14157
		public EffecterDef openingEffect;

		// Token: 0x0400374E RID: 14158
		public FillableBarRequestWithRotation barDrawData;

		// Token: 0x0400374F RID: 14159
		public MoteForRotationData gestatorFormingMote;

		// Token: 0x04003750 RID: 14160
		public MoteForRotationData gestatorCycleCompleteMote;

		// Token: 0x04003751 RID: 14161
		public MoteForRotationData gestatorFormedMote;

		// Token: 0x04003752 RID: 14162
		public GraphicData mechGestatorCylinderGraphic;

		// Token: 0x04003753 RID: 14163
		public GraphicData mechGestatorTopGraphic;

		// Token: 0x04003754 RID: 14164
		public GraphicData formingGraphicData;

		// Token: 0x04003755 RID: 14165
		public float formingMechBobSpeed = 0.001f;

		// Token: 0x04003756 RID: 14166
		public float formingMechYBobDistance = 0.1f;

		// Token: 0x04003757 RID: 14167
		public List<Vector3> formingMechPerRotationOffset = new List<Vector3>();

		// Token: 0x04003758 RID: 14168
		public Vector2 maxFormedMechDrawSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04003759 RID: 14169
		public List<IngredientCount> subcoreScannerFixedIngredients;

		// Token: 0x0400375A RID: 14170
		public int subcoreScannerTicks = 7500;

		// Token: 0x0400375B RID: 14171
		public HediffDef subcoreScannerHediff;

		// Token: 0x0400375C RID: 14172
		public bool destroyBrain;

		// Token: 0x0400375D RID: 14173
		public ThingDef subcoreScannerOutputDef;

		// Token: 0x0400375E RID: 14174
		public EffecterDef subcoreScannerStartEffect;

		// Token: 0x0400375F RID: 14175
		public SoundDef subcoreScannerWorking;

		// Token: 0x04003760 RID: 14176
		public SoundDef subcoreScannerComplete;

		// Token: 0x04003761 RID: 14177
		public bool isInsectCocoon;

		// Token: 0x04003762 RID: 14178
		public GraphicData wastepackAtomizerBottomGraphic;

		// Token: 0x04003763 RID: 14179
		public GraphicData wastepackAtomizerWindowGraphic;

		// Token: 0x04003764 RID: 14180
		public EffecterDef wastepackAtomizerOperationEffecter;

		// Token: 0x04003765 RID: 14181
		public GraphicData bookendGraphicEast;

		// Token: 0x04003766 RID: 14182
		public GraphicData bookendGraphicNorth;

		// Token: 0x04003767 RID: 14183
		public bool isSupportDoor;

		// Token: 0x04003768 RID: 14184
		public GraphicData doorTopGraphic;

		// Token: 0x04003769 RID: 14185
		public GraphicData doorSupportGraphic;

		// Token: 0x0400376A RID: 14186
		public GraphicData upperMoverGraphic;

		// Token: 0x0400376B RID: 14187
		public Vector3 doorTopGraphicOffset;

		// Token: 0x0400376C RID: 14188
		public Vector3 doorSupportGraphicOffset;

		// Token: 0x0400376D RID: 14189
		public Vector3 doorTopHorizontalOffset = new Vector3(0f, 0f, 0.1f);

		// Token: 0x0400376E RID: 14190
		public Vector3 doorTopVerticalOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400376F RID: 14191
		public ThingDef groundSpawnerThingToSpawn;

		// Token: 0x04003770 RID: 14192
		public IntRange groundSpawnerSpawnDelay;

		// Token: 0x04003771 RID: 14193
		public bool groundSpawnerDestroyAdjacent;

		// Token: 0x04003772 RID: 14194
		public SoundDef groundSpawnerSustainerSound;

		// Token: 0x04003773 RID: 14195
		public EffecterDef groundSpawnerSustainedEffecter;

		// Token: 0x04003774 RID: 14196
		public EffecterDef groundSpawnerCompleteEffecter;

		// Token: 0x04003775 RID: 14197
		public string groundSpawnerLetterLabel;

		// Token: 0x04003776 RID: 14198
		public string groundSpawnerLetterText;

		// Token: 0x04003777 RID: 14199
		private static List<string> tmpFenceBlockedAnimals = new List<string>();
	}
}
