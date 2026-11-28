using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001CF RID: 463
	public class ThingDef : BuildableDef
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00041D68 File Offset: 0x0003FF68
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00041D7A File Offset: 0x0003FF7A
		public bool EverPollutable
		{
			get
			{
				return !this.building.isNaturalRock;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00041D8A File Offset: 0x0003FF8A
		public float VolumePerUnit
		{
			get
			{
				if (this.smallVolume)
				{
					return 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00041D9F File Offset: 0x0003FF9F
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00041DA7 File Offset: 0x0003FFA7
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00041DB2 File Offset: 0x0003FFB2
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x00041DC5 File Offset: 0x0003FFC5
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00041DD3 File Offset: 0x0003FFD3
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00041DE1 File Offset: 0x0003FFE1
		public float BaseMarketValue
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			set
			{
				this.SetStatBaseValue(StatDefOf.MarketValue, value);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00041DEF File Offset: 0x0003FFEF
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00041E00 File Offset: 0x00040000
		public int ArchonexusMaxAllowedCount
		{
			get
			{
				if (this.allowedArchonexusCount == -1)
				{
					return Mathf.Min(new int[]
					{
						this.stackLimit,
						25,
						(this.BaseMass > 0f) ? ((int)(5f / this.BaseMass)) : 0,
						(this.BaseMarketValue > 0f) ? ((int)(2000f / this.BaseMarketValue)) : 0
					});
				}
				return this.allowedArchonexusCount;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00041E78 File Offset: 0x00040078
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop && (this != ThingDefOf.ReinforcedBarrel || Find.Storyteller == null || !Find.Storyteller.difficulty.classicMortars) && (this.requiresFactionToAcquire == null || Find.World == null || Find.World.factionManager == null || Find.FactionManager.FirstFactionOfDef(this.requiresFactionToAcquire) != null);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x00041EE4 File Offset: 0x000400E4
		public bool EverTransmitsPower
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					CompProperties_Power compProperties_Power = this.comps[i] as CompProperties_Power;
					if (compProperties_Power != null && compProperties_Power.transmitsPower)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00041F27 File Offset: 0x00040127
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00041F32 File Offset: 0x00040132
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00041F44 File Offset: 0x00040144
		public List<RecipeDef> AllRecipes
		{
			get
			{
				if (this.allRecipesCached == null)
				{
					this.allRecipesCached = new List<RecipeDef>();
					if (this.recipes != null)
					{
						for (int i = 0; i < this.recipes.Count; i++)
						{
							this.allRecipesCached.Add(this.recipes[i]);
						}
					}
					List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
						{
							this.allRecipesCached.Add(allDefsListForReading[j]);
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00041FF0 File Offset: 0x000401F0
		public bool ConnectToPower
		{
			get
			{
				if (this.EverTransmitsPower)
				{
					return false;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == typeof(CompPowerBattery))
					{
						return true;
					}
					if (this.comps[i].compClass == typeof(CompPowerTrader))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00042066 File Offset: 0x00040266
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0004207B File Offset: 0x0004027B
		public FillCategory Fillage
		{
			get
			{
				if (this.fillPercent < 0.01f)
				{
					return FillCategory.None;
				}
				if (this.fillPercent > 0.99f)
				{
					return FillCategory.Full;
				}
				return FillCategory.Partial;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0004209C File Offset: 0x0004029C
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000420A8 File Offset: 0x000402A8
		public bool CanOverlapZones
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (this.passability == Traversability.Impassable && this.category != ThingCategory.Plant && !this.HasComp(typeof(CompTransporter)))
				{
					return false;
				}
				if (this.surfaceType >= SurfaceType.Item)
				{
					return false;
				}
				if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					return false;
				}
				if (!this.canOverlapZones)
				{
					return false;
				}
				if (this.IsBlueprint || this.IsFrame)
				{
					ThingDef thingDef = this.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						return thingDef.CanOverlapZones;
					}
				}
				return true;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00042147 File Offset: 0x00040347
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority > ResourceCountPriority.Uncounted;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00042152 File Offset: 0x00040352
		public List<VerbProperties> Verbs
		{
			get
			{
				if (this.verbs != null)
				{
					return this.verbs;
				}
				return ThingDef.EmptyVerbPropertiesList;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00042168 File Offset: 0x00040368
		public bool CanHaveFaction
		{
			get
			{
				if (this.IsBlueprint || this.IsFrame)
				{
					return true;
				}
				ThingCategory thingCategory = this.category;
				return thingCategory == ThingCategory.Pawn || thingCategory == ThingCategory.Building;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0004219D File Offset: 0x0004039D
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x000421C4 File Offset: 0x000403C4
		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				if (this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return null;
				}
				return this.thingCategories[0];
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x000421E1 File Offset: 0x000403E1
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00042204 File Offset: 0x00040404
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || (this.plant != null && this.plant.canDeteriorate));
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00042230 File Offset: 0x00040430
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.canInteractThroughCorners || (this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock || this.IsSmoothed));
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0004227C File Offset: 0x0004047C
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor || this.IsFence;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00042297 File Offset: 0x00040497
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x000422C4 File Offset: 0x000404C4
		public string DescriptionDetailed
		{
			get
			{
				if (this.descriptionDetailedCached == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.description);
					if (this.IsApparel)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						StringBuilder stringBuilder2 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Layer".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted(this.apparel.GetLayersString());
						stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Covers".Translate());
						defaultInterpolatedStringHandler2.AppendLiteral(": ");
						defaultInterpolatedStringHandler2.AppendFormatted(this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human));
						stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
						if (this.equippedStatOffsets != null && this.equippedStatOffsets.Count > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
							for (int i = 0; i < this.equippedStatOffsets.Count; i++)
							{
								if (i > 0)
								{
									stringBuilder.AppendLine();
								}
								StatModifier statModifier = this.equippedStatOffsets[i];
								StringBuilder stringBuilder4 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 2);
								defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>(statModifier.stat.LabelCap);
								defaultInterpolatedStringHandler3.AppendLiteral(": ");
								defaultInterpolatedStringHandler3.AppendFormatted(statModifier.ValueToStringAsOffset);
								stringBuilder4.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
							}
						}
					}
					this.descriptionDetailedCached = stringBuilder.ToString();
				}
				return this.descriptionDetailedCached;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00042448 File Offset: 0x00040648
		public bool CanBenefitFromCover
		{
			get
			{
				return this.category == ThingCategory.Pawn || (this.building != null && this.building.IsTurret);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00042470 File Offset: 0x00040670
		public bool PotentiallySmeltable
		{
			get
			{
				if (!this.smeltable)
				{
					return false;
				}
				if (base.MadeFromStuff)
				{
					using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(this, TechLevel.Undefined, false).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.smeltable)
							{
								return true;
							}
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x000424DC File Offset: 0x000406DC
		public bool HasSingleOrMultipleInteractionCells
		{
			get
			{
				return this.hasInteractionCell || !this.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>();
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000424F6 File Offset: 0x000406F6
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00042501 File Offset: 0x00040701
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00042518 File Offset: 0x00040718
		public bool IsWall
		{
			get
			{
				return this.building != null && this.building.isWall;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0004252F File Offset: 0x0004072F
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00042546 File Offset: 0x00040746
		public bool IsFrame
		{
			get
			{
				return this.isFrameInt;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0004254E File Offset: 0x0004074E
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00042564 File Offset: 0x00040764
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0004256F File Offset: 0x0004076F
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00042581 File Offset: 0x00040781
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00042598 File Offset: 0x00040798
		public bool IsFence
		{
			get
			{
				return this.building != null && this.building.isFence;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x000425AF File Offset: 0x000407AF
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000425BA File Offset: 0x000407BA
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x000425C5 File Offset: 0x000407C5
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000425E3 File Offset: 0x000407E3
		public bool IsNutritionGivingIngestibleForHumanlikeBabies
		{
			get
			{
				return this.IsNutritionGivingIngestible && this.ingestible.HumanEdible && this.ingestible.babiesCanIngest;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00042607 File Offset: 0x00040807
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>()) && !this.IsApparel;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00042637 File Offset: 0x00040837
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0004264E File Offset: 0x0004084E
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00042665 File Offset: 0x00040865
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0004267C File Offset: 0x0004087C
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory > DrugCategory.None;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00042696 File Offset: 0x00040896
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x000426B4 File Offset: 0x000408B4
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000426D1 File Offset: 0x000408D1
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x000426EE File Offset: 0x000408EE
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00042705 File Offset: 0x00040905
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00042710 File Offset: 0x00040910
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0004271D File Offset: 0x0004091D
		public bool IsSmoothable
		{
			get
			{
				BuildingProperties buildingProperties = this.building;
				return ((buildingProperties != null) ? buildingProperties.smoothedThing : null) != null;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00042734 File Offset: 0x00040934
		public bool IsSmoothed
		{
			get
			{
				BuildingProperties buildingProperties = this.building;
				return ((buildingProperties != null) ? buildingProperties.unsmoothedThing : null) != null;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0004274B File Offset: 0x0004094B
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0004276C File Offset: 0x0004096C
		public bool IsCryptosleepCasket
		{
			get
			{
				return typeof(Building_CryptosleepCasket).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00042783 File Offset: 0x00040983
		public bool IsGibbetCage
		{
			get
			{
				return typeof(Building_GibbetCage).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0004279A File Offset: 0x0004099A
		public bool IsMechGestator
		{
			get
			{
				return typeof(Building_MechGestator).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000427B1 File Offset: 0x000409B1
		public bool IsMechRecharger
		{
			get
			{
				return typeof(Building_MechCharger).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x000427C8 File Offset: 0x000409C8
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000427EE File Offset: 0x000409EE
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00042813 File Offset: 0x00040A13
		public bool IsEgg
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && (this.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || this.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized));
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0004284C File Offset: 0x00040A4C
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00042871 File Offset: 0x00040A71
		public bool IsWool
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Wools);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00042898 File Offset: 0x00040A98
		public bool IsRangedWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (!this.verbs[i].IsMeleeAttack)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000428E8 File Offset: 0x00040AE8
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00042900 File Offset: 0x00040B00
		public bool IsWeaponUsingProjectiles
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (this.verbs[i].LaunchesProjectile)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00042950 File Offset: 0x00040B50
		public bool IsShieldThatBlocksRanged
		{
			get
			{
				return this.HasComp(typeof(CompShield)) && this.GetCompProperties<CompProperties_Shield>().blocksRangedWeapons;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00042971 File Offset: 0x00040B71
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000429AD File Offset: 0x00040BAD
		public bool IsNonResourceNaturalRock
		{
			get
			{
				return this.category == ThingCategory.Building && this.building.isNaturalRock && !this.building.isResourceRock && !this.building.mineablePreventNaturalRockOnSurface && !this.IsSmoothed;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x000429EA File Offset: 0x00040BEA
		public bool HasSunShadows
		{
			get
			{
				if (this.hasSunShadowsCached == null)
				{
					this.hasSunShadowsCached = new bool?(typeof(Pawn).IsAssignableFrom(this.thingClass));
				}
				return this.hasSunShadowsCached.Value;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00042A24 File Offset: 0x00040C24
		public bool IsNaturalOrgan
		{
			get
			{
				if (this.isNaturalOrganCached == null)
				{
					if (this.category != ThingCategory.Item)
					{
						this.isNaturalOrganCached = new bool?(false);
					}
					else
					{
						List<BodyPartDef> allDefsListForReading = DefDatabase<BodyPartDef>.AllDefsListForReading;
						this.isNaturalOrganCached = new bool?(false);
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (allDefsListForReading[i].spawnThingOnRemoved == this)
							{
								this.isNaturalOrganCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isNaturalOrganCached.Value;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00042AA0 File Offset: 0x00040CA0
		public bool IsFungus
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.Fungus);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00042ACB File Offset: 0x00040CCB
		public bool IsAnimalProduct
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.AnimalProduct);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00042AF3 File Offset: 0x00040CF3
		public bool IsProcessedFood
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.Processed);
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00042B1E File Offset: 0x00040D1E
		public bool CanAffectLinker
		{
			get
			{
				return (this.graphicData != null && this.graphicData.Linked) || this.IsDoor;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00042B40 File Offset: 0x00040D40
		public bool IsNonDeconstructibleAttackableBuilding
		{
			get
			{
				return this.IsBuildingArtificial && !this.building.IsDeconstructible && this.destroyable && !this.mineable && this.building.isTargetable && this.building.draftAttackNonDeconstructable;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00042B8C File Offset: 0x00040D8C
		public bool IsPlant
		{
			get
			{
				return typeof(Plant).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00042BA3 File Offset: 0x00040DA3
		public bool IsDeadPlant
		{
			get
			{
				return typeof(DeadPlant).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00042BBA File Offset: 0x00040DBA
		public bool IsStudiable
		{
			get
			{
				return this.HasAssignableCompFrom(typeof(CompStudiable));
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00042BCC File Offset: 0x00040DCC
		public List<StyleCategoryDef> RelevantStyleCategories
		{
			get
			{
				if (this.cachedRelevantStyleCategories == null)
				{
					this.cachedRelevantStyleCategories = new List<StyleCategoryDef>();
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						if (!styleCategoryDef.thingDefStyles.NullOrEmpty<ThingDefStyle>())
						{
							using (List<ThingDefStyle>.Enumerator enumerator2 = styleCategoryDef.thingDefStyles.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.ThingDef == this)
									{
										this.cachedRelevantStyleCategories.Add(styleCategoryDef);
										break;
									}
								}
							}
						}
					}
				}
				return this.cachedRelevantStyleCategories;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00042C8C File Offset: 0x00040E8C
		public bool BlocksPlanting(bool canWipePlants = false)
		{
			return (this.building == null || !this.building.SupportsPlants) && (this.building == null || !this.building.isAttachment) && (this.blockPlants || (!canWipePlants && this.category == ThingCategory.Plant) || this.Fillage > FillCategory.None || this.IsEdifice());
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00042CF5 File Offset: 0x00040EF5
		public virtual bool CanSpawnAt(IntVec3 pos, Rot4 rot, Map map)
		{
			return true;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00042CF8 File Offset: 0x00040EF8
		public bool EverStorable(bool willMinifyIfPossible)
		{
			if (typeof(MinifiedThing).IsAssignableFrom(this.thingClass))
			{
				return true;
			}
			if (!this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				if (this.category == ThingCategory.Item)
				{
					return true;
				}
				if (willMinifyIfPossible && this.Minifiable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00042D44 File Offset: 0x00040F44
		public Thing GetConcreteExample(ThingDef stuff = null)
		{
			if (this.concreteExamplesInt == null)
			{
				this.concreteExamplesInt = new Dictionary<ThingDef, Thing>();
			}
			if (stuff == null)
			{
				stuff = ThingDefOf.Steel;
			}
			if (!this.concreteExamplesInt.ContainsKey(stuff))
			{
				if (this.race == null)
				{
					this.concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, base.MadeFromStuff ? stuff : null);
				}
				else
				{
					this.concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.AllDefsListForReading.FirstOrDefault((PawnKindDef pkd) => pkd.race == this), null, null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00042DE4 File Offset: 0x00040FE4
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == typeof(T))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00042E38 File Offset: 0x00041038
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (typeof(T).IsAssignableFrom(this.comps[i].compClass))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00042E8C File Offset: 0x0004108C
		public bool HasComp(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == compType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00042ECC File Offset: 0x000410CC
		public bool HasComp<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == typeof(T) || typeof(T).IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00042F38 File Offset: 0x00041138
		public bool HasAssignableCompFrom(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (compType.IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00042F78 File Offset: 0x00041178
		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00042FC8 File Offset: 0x000411C8
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					GraphicData graphicData = this.graphicData;
					if (graphicData.shaderType == null)
					{
						graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					ContentFinderRequester.requester = this;
					try
					{
						this.graphic = this.graphicData.Graphic;
						if (this.drawerType != DrawerType.RealtimeOnly)
						{
							TextureAtlasGroup textureAtlasGroup = this.category.ToAtlasGroup();
							this.graphic.TryInsertIntoAtlas(textureAtlasGroup);
							if (textureAtlasGroup == TextureAtlasGroup.Building && this.Minifiable)
							{
								this.graphic.TryInsertIntoAtlas(TextureAtlasGroup.Item);
							}
						}
					}
					finally
					{
						ContentFinderRequester.requester = null;
					}
				});
			}
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			if (this.verbs != null && this.verbs.Count == 1 && this.verbs[0].label.NullOrEmpty())
			{
				this.verbs[0].label = this.label;
			}
			base.PostLoad();
			if (this.category == ThingCategory.Building && this.building == null)
			{
				this.building = new BuildingProperties();
			}
			BuildingProperties buildingProperties = this.building;
			if (buildingProperties != null)
			{
				buildingProperties.PostLoadSpecial(this);
			}
			ApparelProperties apparelProperties = this.apparel;
			if (apparelProperties != null)
			{
				apparelProperties.PostLoadSpecial(this);
			}
			PlantProperties plantProperties = this.plant;
			if (plantProperties != null)
			{
				plantProperties.PostLoadSpecial(this);
			}
			if (this.comps != null)
			{
				foreach (CompProperties compProperties in this.comps)
				{
					compProperties.PostLoadSpecial(this);
				}
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00043110 File Offset: 0x00041310
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			if (this.category == ThingCategory.Pawn)
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					return;
				}
				if (!this.race.Humanlike)
				{
					PawnKindDef anyPawnKind = this.race.AnyPawnKind;
					if (anyPawnKind != null)
					{
						Material material = ((ModsConfig.BiotechActive && anyPawnKind.RaceProps.IsMechanoid) ? anyPawnKind.lifeStages.First<PawnKindLifeStage>() : anyPawnKind.lifeStages.Last<PawnKindLifeStage>()).bodyGraphicData.Graphic.MatAt(Rot4.East, null);
						this.uiIcon = (Texture2D)material.mainTexture;
						this.uiIconColor = material.color;
						Shader shader;
						MaterialRequest req;
						if (ShaderDatabase.TryGetUIShader(material.shader, out shader) && MaterialPool.TryGetRequestForMat(material, out req))
						{
							req.shader = shader;
							this.uiIconMaterial = MaterialPool.MatFrom(req);
							return;
						}
					}
				}
			}
			else
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(this);
				if (this.colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
				{
					this.uiIconColor = this.colorGenerator.ExemplaryColor;
				}
				else if (thingDef != null)
				{
					this.uiIconColor = base.GetColorForStuff(thingDef);
				}
				else if (this.graphicData != null)
				{
					this.uiIconColor = this.graphicData.color;
				}
				if (this.rotatable && this.graphic != null && this.graphic != BaseContent.BadGraphic && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
				{
					this.uiIconAngle = 180f + this.graphic.DrawRotatedExtraAngleOffset;
				}
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000432BC File Offset: 0x000414BC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.ingestible != null)
			{
				this.ingestible.parent = this;
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.parent = this;
			}
			BuildingProperties buildingProperties = this.building;
			if (buildingProperties != null)
			{
				buildingProperties.ResolveReferencesSpecial();
			}
			GraphicData graphicData = this.graphicData;
			if (graphicData != null)
			{
				graphicData.ResolveReferencesSpecial();
			}
			RaceProperties raceProperties = this.race;
			if (raceProperties != null)
			{
				raceProperties.ResolveReferencesSpecial();
			}
			StuffProperties stuffProperties = this.stuffProps;
			if (stuffProperties != null)
			{
				stuffProperties.ResolveReferencesSpecial();
			}
			ApparelProperties apparelProperties = this.apparel;
			if (apparelProperties != null)
			{
				apparelProperties.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpact_Ground;
			}
			if (this.soundDrop == null)
			{
				this.soundDrop = SoundDefOf.Standard_Drop;
			}
			if (this.soundPickup == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.soundInteract == null)
			{
				this.soundInteract = SoundDefOf.Standard_Pickup;
			}
			if (this.inspectorTabs != null && this.inspectorTabs.Any<Type>())
			{
				this.inspectorTabsResolved = new List<InspectTabBase>();
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate inspector tab of type ";
						Type type = this.inspectorTabs[i];
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					this.comps[j].ResolveReferences(this);
				}
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0004346C File Offset: 0x0004166C
		public override IEnumerable<string> ConfigErrors()
		{
			ThingDef.<ConfigErrors>d__401 <ConfigErrors>d__ = new ThingDef.<ConfigErrors>d__401(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0004347C File Offset: 0x0004167C
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00043485 File Offset: 0x00041685
		public string LabelAsStuff
		{
			get
			{
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					return this.stuffProps.stuffAdjective;
				}
				return this.label;
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000434AC File Offset: 0x000416AC
		public bool IsWithinCategory(ThingCategoryDef category)
		{
			if (this.thingCategories == null)
			{
				return false;
			}
			for (int i = 0; i < this.thingCategories.Count; i++)
			{
				for (ThingCategoryDef thingCategoryDef = this.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
				{
					if (thingCategoryDef == category)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000434FC File Offset: 0x000416FC
		public void Notify_UnlockedByResearch()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_PostUnlockedByResearch(this);
				}
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00043539 File Offset: 0x00041739
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			ThingDef.<SpecialDisplayStats>d__407 <SpecialDisplayStats>d__ = new ThingDef.<SpecialDisplayStats>d__407(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04000C44 RID: 3140
		public Type thingClass;

		// Token: 0x04000C45 RID: 3141
		public ThingCategory category;

		// Token: 0x04000C46 RID: 3142
		public TickerType tickerType;

		// Token: 0x04000C47 RID: 3143
		public int stackLimit = 1;

		// Token: 0x04000C48 RID: 3144
		public IntVec2 size = IntVec2.One;

		// Token: 0x04000C49 RID: 3145
		public bool destroyable = true;

		// Token: 0x04000C4A RID: 3146
		public bool rotatable = true;

		// Token: 0x04000C4B RID: 3147
		public bool smallVolume;

		// Token: 0x04000C4C RID: 3148
		public bool useHitPoints = true;

		// Token: 0x04000C4D RID: 3149
		public bool receivesSignals;

		// Token: 0x04000C4E RID: 3150
		public List<CompProperties> comps = new List<CompProperties>();

		// Token: 0x04000C4F RID: 3151
		public List<ThingDef> virtualDefs = new List<ThingDef>();

		// Token: 0x04000C50 RID: 3152
		public ThingDef virtualDefParent;

		// Token: 0x04000C51 RID: 3153
		[NoTranslate]
		public string devNote;

		// Token: 0x04000C52 RID: 3154
		public List<ThingDefCountRangeClass> killedLeavingsRanges;

		// Token: 0x04000C53 RID: 3155
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x04000C54 RID: 3156
		public List<ThingDefCountClass> killedLeavingsPlayerHostile;

		// Token: 0x04000C55 RID: 3157
		public float killedLeavingsChance = 1f;

		// Token: 0x04000C56 RID: 3158
		public bool forceLeavingsAllowed;

		// Token: 0x04000C57 RID: 3159
		public List<ThingDefCountClass> butcherProducts;

		// Token: 0x04000C58 RID: 3160
		public List<ThingDefCountClass> smeltProducts;

		// Token: 0x04000C59 RID: 3161
		public bool smeltable;

		// Token: 0x04000C5A RID: 3162
		public bool burnableByRecipe;

		// Token: 0x04000C5B RID: 3163
		public bool randomizeRotationOnSpawn;

		// Token: 0x04000C5C RID: 3164
		public List<DamageMultiplier> damageMultipliers;

		// Token: 0x04000C5D RID: 3165
		public bool isTechHediff;

		// Token: 0x04000C5E RID: 3166
		public RecipeMakerProperties recipeMaker;

		// Token: 0x04000C5F RID: 3167
		public ThingDef minifiedDef;

		// Token: 0x04000C60 RID: 3168
		public bool isUnfinishedThing;

		// Token: 0x04000C61 RID: 3169
		public bool leaveResourcesWhenKilled;

		// Token: 0x04000C62 RID: 3170
		public ThingDef slagDef;

		// Token: 0x04000C63 RID: 3171
		public bool isFrameInt;

		// Token: 0x04000C64 RID: 3172
		public List<IntVec3> multipleInteractionCellOffsets;

		// Token: 0x04000C65 RID: 3173
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		// Token: 0x04000C66 RID: 3174
		public bool hasInteractionCell;

		// Token: 0x04000C67 RID: 3175
		public ThingDef interactionCellIcon;

		// Token: 0x04000C68 RID: 3176
		public bool interactionCellIconReverse;

		// Token: 0x04000C69 RID: 3177
		public ThingDef filthLeaving;

		// Token: 0x04000C6A RID: 3178
		public bool forceDebugSpawnable;

		// Token: 0x04000C6B RID: 3179
		public bool intricate;

		// Token: 0x04000C6C RID: 3180
		public bool scatterableOnMapGen = true;

		// Token: 0x04000C6D RID: 3181
		public float deepCommonality;

		// Token: 0x04000C6E RID: 3182
		public int deepCountPerCell = 300;

		// Token: 0x04000C6F RID: 3183
		public int deepCountPerPortion = -1;

		// Token: 0x04000C70 RID: 3184
		public IntRange deepLumpSizeRange = IntRange.Zero;

		// Token: 0x04000C71 RID: 3185
		public float generateCommonality = 1f;

		// Token: 0x04000C72 RID: 3186
		public float generateAllowChance = 1f;

		// Token: 0x04000C73 RID: 3187
		private bool canOverlapZones = true;

		// Token: 0x04000C74 RID: 3188
		public FloatRange startingHpRange = FloatRange.One;

		// Token: 0x04000C75 RID: 3189
		[NoTranslate]
		public List<string> thingSetMakerTags;

		// Token: 0x04000C76 RID: 3190
		public bool alwaysFlee;

		// Token: 0x04000C77 RID: 3191
		public List<RecipeDef> recipes;

		// Token: 0x04000C78 RID: 3192
		public bool messageOnDeteriorateInStorage = true;

		// Token: 0x04000C79 RID: 3193
		public bool deteriorateFromEnvironmentalEffects = true;

		// Token: 0x04000C7A RID: 3194
		public bool canDeteriorateUnspawned;

		// Token: 0x04000C7B RID: 3195
		public bool canLoadIntoCaravan = true;

		// Token: 0x04000C7C RID: 3196
		public bool isMechClusterThreat;

		// Token: 0x04000C7D RID: 3197
		public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

		// Token: 0x04000C7E RID: 3198
		public int minRewardCount = 1;

		// Token: 0x04000C7F RID: 3199
		public bool preventSkyfallersLandingOn;

		// Token: 0x04000C80 RID: 3200
		public FactionDef requiresFactionToAcquire;

		// Token: 0x04000C81 RID: 3201
		public float relicChance;

		// Token: 0x04000C82 RID: 3202
		public OrderedTakeGroupDef orderedTakeGroup;

		// Token: 0x04000C83 RID: 3203
		public int allowedArchonexusCount;

		// Token: 0x04000C84 RID: 3204
		public int possessionCount;

		// Token: 0x04000C85 RID: 3205
		public bool notifyMapRemoved;

		// Token: 0x04000C86 RID: 3206
		public bool canScatterOver = true;

		// Token: 0x04000C87 RID: 3207
		public bool genericMarketSellable = true;

		// Token: 0x04000C88 RID: 3208
		public bool drawHighlight;

		// Token: 0x04000C89 RID: 3209
		public Color? highlightColor;

		// Token: 0x04000C8A RID: 3210
		public bool drawHighlightOnlyForHostile;

		// Token: 0x04000C8B RID: 3211
		public bool autoTargetNearbyIdenticalThings;

		// Token: 0x04000C8C RID: 3212
		public bool preventDroppingThingsOn;

		// Token: 0x04000C8D RID: 3213
		public bool hiddenWhileUndiscovered;

		// Token: 0x04000C8E RID: 3214
		public bool disableImpassableShotOverConfigError;

		// Token: 0x04000C8F RID: 3215
		public bool showInSearch = true;

		// Token: 0x04000C90 RID: 3216
		public bool bringAlongOnGravship = true;

		// Token: 0x04000C91 RID: 3217
		public ThingDef dropPodFaller;

		// Token: 0x04000C92 RID: 3218
		public ThingDef dropPodActive;

		// Token: 0x04000C93 RID: 3219
		public bool preventSpawningInResourcePod;

		// Token: 0x04000C94 RID: 3220
		public bool pathfinderDangerous;

		// Token: 0x04000C95 RID: 3221
		public bool noRightClickDraftAttack;

		// Token: 0x04000C96 RID: 3222
		public int gravshipSpawnPriority = 1;

		// Token: 0x04000C97 RID: 3223
		public List<string> replaceTags;

		// Token: 0x04000C98 RID: 3224
		public bool preventGravshipLandingOn;

		// Token: 0x04000C99 RID: 3225
		public bool canInteractThroughCorners;

		// Token: 0x04000C9A RID: 3226
		public GraphicData graphicData;

		// Token: 0x04000C9B RID: 3227
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		// Token: 0x04000C9C RID: 3228
		public bool drawOffscreen;

		// Token: 0x04000C9D RID: 3229
		public ColorGenerator colorGenerator;

		// Token: 0x04000C9E RID: 3230
		public float hideAtSnowOrSandDepth = 99999f;

		// Token: 0x04000C9F RID: 3231
		public bool drawDamagedOverlay = true;

		// Token: 0x04000CA0 RID: 3232
		public bool castEdgeShadows;

		// Token: 0x04000CA1 RID: 3233
		public float staticSunShadowHeight;

		// Token: 0x04000CA2 RID: 3234
		public bool useSameGraphicForGhost;

		// Token: 0x04000CA3 RID: 3235
		public bool useBlueprintGraphicAsGhost;

		// Token: 0x04000CA4 RID: 3236
		public List<ThingStyleChance> randomStyle;

		// Token: 0x04000CA5 RID: 3237
		public float randomStyleChance;

		// Token: 0x04000CA6 RID: 3238
		public bool canEditAnyStyle;

		// Token: 0x04000CA7 RID: 3239
		public bool dontPrint;

		// Token: 0x04000CA8 RID: 3240
		public ThingDef defaultStuff;

		// Token: 0x04000CA9 RID: 3241
		public int killedLeavingsExpandRect;

		// Token: 0x04000CAA RID: 3242
		public bool minifiedManualDraw;

		// Token: 0x04000CAB RID: 3243
		public float minifiedDrawScale = 1f;

		// Token: 0x04000CAC RID: 3244
		public Rot4 overrideMinifiedRot = Rot4.Invalid;

		// Token: 0x04000CAD RID: 3245
		public Vector3 minifiedDrawOffset = Vector3.zero;

		// Token: 0x04000CAE RID: 3246
		public float deselectedSelectionBracketFactor = 1f;

		// Token: 0x04000CAF RID: 3247
		public bool selectable;

		// Token: 0x04000CB0 RID: 3248
		public bool containedPawnsSelectable;

		// Token: 0x04000CB1 RID: 3249
		public bool containedItemsSelectable;

		// Token: 0x04000CB2 RID: 3250
		public bool neverMultiSelect;

		// Token: 0x04000CB3 RID: 3251
		public bool isAutoAttackableMapObject;

		// Token: 0x04000CB4 RID: 3252
		public bool hasTooltip;

		// Token: 0x04000CB5 RID: 3253
		public List<Type> inspectorTabs;

		// Token: 0x04000CB6 RID: 3254
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04000CB7 RID: 3255
		public bool seeThroughFog;

		// Token: 0x04000CB8 RID: 3256
		public bool drawGUIOverlay;

		// Token: 0x04000CB9 RID: 3257
		public bool drawGUIOverlayQuality = true;

		// Token: 0x04000CBA RID: 3258
		public ResourceCountPriority resourceReadoutPriority;

		// Token: 0x04000CBB RID: 3259
		public bool resourceReadoutAlwaysShow;

		// Token: 0x04000CBC RID: 3260
		public bool drawPlaceWorkersWhileSelected;

		// Token: 0x04000CBD RID: 3261
		public bool drawPlaceWorkersWhileInstallBlueprintSelected;

		// Token: 0x04000CBE RID: 3262
		public ConceptDef storedConceptLearnOpportunity;

		// Token: 0x04000CBF RID: 3263
		public float uiIconScale = 1f;

		// Token: 0x04000CC0 RID: 3264
		public bool hasCustomRectForSelector;

		// Token: 0x04000CC1 RID: 3265
		public bool hideStats;

		// Token: 0x04000CC2 RID: 3266
		public bool hideInspect;

		// Token: 0x04000CC3 RID: 3267
		public bool onlyShowInspectString;

		// Token: 0x04000CC4 RID: 3268
		public bool hideMainDesc;

		// Token: 0x04000CC5 RID: 3269
		public bool alwaysHaulable;

		// Token: 0x04000CC6 RID: 3270
		public bool designateHaulable;

		// Token: 0x04000CC7 RID: 3271
		public List<ThingCategoryDef> thingCategories;

		// Token: 0x04000CC8 RID: 3272
		public bool mineable;

		// Token: 0x04000CC9 RID: 3273
		public bool socialPropernessMatters;

		// Token: 0x04000CCA RID: 3274
		public bool stealable = true;

		// Token: 0x04000CCB RID: 3275
		public SoundDef soundSpawned;

		// Token: 0x04000CCC RID: 3276
		public SoundDef soundDrop;

		// Token: 0x04000CCD RID: 3277
		public SoundDef soundPickup;

		// Token: 0x04000CCE RID: 3278
		public SoundDef soundInteract;

		// Token: 0x04000CCF RID: 3279
		public SoundDef soundImpactDefault;

		// Token: 0x04000CD0 RID: 3280
		public SoundDef soundPlayInstrument;

		// Token: 0x04000CD1 RID: 3281
		public SoundDef soundOpen;

		// Token: 0x04000CD2 RID: 3282
		public bool saveCompressible;

		// Token: 0x04000CD3 RID: 3283
		public bool isSaveable = true;

		// Token: 0x04000CD4 RID: 3284
		public bool holdsRoof;

		// Token: 0x04000CD5 RID: 3285
		public float fillPercent;

		// Token: 0x04000CD6 RID: 3286
		public bool coversFloor;

		// Token: 0x04000CD7 RID: 3287
		public bool neverOverlapFloors;

		// Token: 0x04000CD8 RID: 3288
		public SurfaceType surfaceType;

		// Token: 0x04000CD9 RID: 3289
		public bool wipesPlants;

		// Token: 0x04000CDA RID: 3290
		public bool blockPlants;

		// Token: 0x04000CDB RID: 3291
		public bool blockLight;

		// Token: 0x04000CDC RID: 3292
		public bool blockWind;

		// Token: 0x04000CDD RID: 3293
		public bool blockWeather;

		// Token: 0x04000CDE RID: 3294
		public Tradeability tradeability = Tradeability.All;

		// Token: 0x04000CDF RID: 3295
		[NoTranslate]
		public List<string> tradeTags;

		// Token: 0x04000CE0 RID: 3296
		public bool tradeNeverStack;

		// Token: 0x04000CE1 RID: 3297
		public bool tradeNeverGenerateStacked;

		// Token: 0x04000CE2 RID: 3298
		public bool healthAffectsPrice = true;

		// Token: 0x04000CE3 RID: 3299
		public ColorGenerator colorGeneratorInTraderStock;

		// Token: 0x04000CE4 RID: 3300
		private List<VerbProperties> verbs;

		// Token: 0x04000CE5 RID: 3301
		public List<Tool> tools;

		// Token: 0x04000CE6 RID: 3302
		public float equippedAngleOffset;

		// Token: 0x04000CE7 RID: 3303
		public float equippedDistanceOffset;

		// Token: 0x04000CE8 RID: 3304
		public EquipmentType equipmentType;

		// Token: 0x04000CE9 RID: 3305
		public TechLevel techLevel;

		// Token: 0x04000CEA RID: 3306
		public List<WeaponClassDef> weaponClasses;

		// Token: 0x04000CEB RID: 3307
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000CEC RID: 3308
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000CED RID: 3309
		public bool violentTechHediff;

		// Token: 0x04000CEE RID: 3310
		public bool destroyOnDrop;

		// Token: 0x04000CEF RID: 3311
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04000CF0 RID: 3312
		public SoundDef meleeHitSound;

		// Token: 0x04000CF1 RID: 3313
		public float recoilPower = 1f;

		// Token: 0x04000CF2 RID: 3314
		public float recoilRelaxation = 10f;

		// Token: 0x04000CF3 RID: 3315
		public bool rotateInShelves = true;

		// Token: 0x04000CF4 RID: 3316
		public bool mergeVerbGizmos = true;

		// Token: 0x04000CF5 RID: 3317
		public BuildableDef entityDefToBuild;

		// Token: 0x04000CF6 RID: 3318
		public ThingDef projectileWhenLoaded;

		// Token: 0x04000CF7 RID: 3319
		public RulePackDef ideoBuildingNamerBase;

		// Token: 0x04000CF8 RID: 3320
		public EntityCodexEntryDef entityCodexEntry;

		// Token: 0x04000CF9 RID: 3321
		public IngestibleProperties ingestible;

		// Token: 0x04000CFA RID: 3322
		public FilthProperties filth;

		// Token: 0x04000CFB RID: 3323
		public GasProperties gas;

		// Token: 0x04000CFC RID: 3324
		public BuildingProperties building;

		// Token: 0x04000CFD RID: 3325
		public RaceProperties race;

		// Token: 0x04000CFE RID: 3326
		public ApparelProperties apparel;

		// Token: 0x04000CFF RID: 3327
		public MoteProperties mote;

		// Token: 0x04000D00 RID: 3328
		public PlantProperties plant;

		// Token: 0x04000D01 RID: 3329
		public ProjectileProperties projectile;

		// Token: 0x04000D02 RID: 3330
		public StuffProperties stuffProps;

		// Token: 0x04000D03 RID: 3331
		public SkyfallerProperties skyfaller;

		// Token: 0x04000D04 RID: 3332
		public PawnFlyerProperties pawnFlyer;

		// Token: 0x04000D05 RID: 3333
		public RitualFocusProperties ritualFocus;

		// Token: 0x04000D06 RID: 3334
		public IngredientProperties ingredient;

		// Token: 0x04000D07 RID: 3335
		public MapPortalProperties portal;

		// Token: 0x04000D08 RID: 3336
		public bool canBeUsedUnderRoof = true;

		// Token: 0x04000D09 RID: 3337
		[Unsaved(false)]
		private string descriptionDetailedCached;

		// Token: 0x04000D0A RID: 3338
		[Unsaved(false)]
		public Graphic interactionCellGraphic;

		// Token: 0x04000D0B RID: 3339
		[Unsaved(false)]
		private bool? isNaturalOrganCached;

		// Token: 0x04000D0C RID: 3340
		[Unsaved(false)]
		private bool? hasSunShadowsCached;

		// Token: 0x04000D0D RID: 3341
		[Unsaved(false)]
		private List<StyleCategoryDef> cachedRelevantStyleCategories;

		// Token: 0x04000D0E RID: 3342
		public const int SmallUnitPerVolume = 10;

		// Token: 0x04000D0F RID: 3343
		public const float SmallVolumePerUnit = 0.1f;

		// Token: 0x04000D10 RID: 3344
		public const float ArchonexusMaxItemStackMass = 5f;

		// Token: 0x04000D11 RID: 3345
		public const int ArchonexusMaxItemStackCount = 25;

		// Token: 0x04000D12 RID: 3346
		public const float ArchonexusMaxItemStackValue = 2000f;

		// Token: 0x04000D13 RID: 3347
		public const int ArchonexusAutoCalculateValue = -1;

		// Token: 0x04000D14 RID: 3348
		private List<RecipeDef> allRecipesCached;

		// Token: 0x04000D15 RID: 3349
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		// Token: 0x04000D16 RID: 3350
		private Dictionary<ThingDef, Thing> concreteExamplesInt;
	}
}
