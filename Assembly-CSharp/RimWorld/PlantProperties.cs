using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F10 RID: 3856
	public class PlantProperties
	{
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x0600640C RID: 25612 RVA: 0x0023D17E File Offset: 0x0023B37E
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600640D RID: 25613 RVA: 0x0023D18E File Offset: 0x0023B38E
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x0600640E RID: 25614 RVA: 0x0023D19D File Offset: 0x0023B39D
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600640F RID: 25615 RVA: 0x0023D1AF File Offset: 0x0023B3AF
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood" || this.forceIsTree;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06006410 RID: 25616 RVA: 0x0023D1CB File Offset: 0x0023B3CB
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06006411 RID: 25617 RVA: 0x0023D1DA File Offset: 0x0023B3DA
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06006412 RID: 25618 RVA: 0x0023D1E9 File Offset: 0x0023B3E9
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06006413 RID: 25619 RVA: 0x0023D1F8 File Offset: 0x0023B3F8
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06006414 RID: 25620 RVA: 0x0023D215 File Offset: 0x0023B415
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06006415 RID: 25621 RVA: 0x0023D220 File Offset: 0x0023B420
		public bool RequiresPollution
		{
			get
			{
				return this.pollution == Pollution.PollutedOnly;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06006416 RID: 25622 RVA: 0x0023D22B File Offset: 0x0023B42B
		public bool RequiresNoPollution
		{
			get
			{
				return this.pollution == Pollution.CleanOnly;
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06006417 RID: 25623 RVA: 0x0023D238 File Offset: 0x0023B438
		public HashSet<string> WildTerrainTags
		{
			get
			{
				HashSet<string> result;
				if ((result = this.wildTerrainTagsSet) == null)
				{
					IEnumerable<string> enumerable = this.wildTerrainTags;
					result = (this.wildTerrainTagsSet = new HashSet<string>(enumerable ?? Enumerable.Empty<string>()));
				}
				return result;
			}
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x0023D270 File Offset: 0x0023B470
		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (ModsConfig.BiotechActive && !this.pollutedGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.pollutedGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.pollutedGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.leaflessImmatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessImmatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessImmatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.snowOverlayGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.snowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.snowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.leaflessSnowOverlayGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessSnowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessSnowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.immatureSnowOverlayGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureSnowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureSnowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x0023D36A File Offset: 0x0023B56A
		public IEnumerable<string> ConfigErrors()
		{
			PlantProperties.<ConfigErrors>d__112 <ConfigErrors>d__ = new PlantProperties.<ConfigErrors>d__112(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x0023D37A File Offset: 0x0023B57A
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			PlantProperties.<GetHarvestYieldHyperlinks>d__113 <GetHarvestYieldHyperlinks>d__ = new PlantProperties.<GetHarvestYieldHyperlinks>d__113(-2);
			<GetHarvestYieldHyperlinks>d__.<>4__this = this;
			return <GetHarvestYieldHyperlinks>d__;
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x0023D38A File Offset: 0x0023B58A
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			PlantProperties.<SpecialDisplayStats>d__114 <SpecialDisplayStats>d__ = new PlantProperties.<SpecialDisplayStats>d__114(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040038E2 RID: 14562
		public List<PlantBiomeRecord> wildBiomes;

		// Token: 0x040038E3 RID: 14563
		public int wildClusterRadius = -1;

		// Token: 0x040038E4 RID: 14564
		public float wildClusterWeight = 15f;

		// Token: 0x040038E5 RID: 14565
		public float wildOrder = 2f;

		// Token: 0x040038E6 RID: 14566
		public bool wildEqualLocalDistribution = true;

		// Token: 0x040038E7 RID: 14567
		public bool cavePlant;

		// Token: 0x040038E8 RID: 14568
		public float cavePlantWeight = 1f;

		// Token: 0x040038E9 RID: 14569
		public List<string> wildTerrainTags;

		// Token: 0x040038EA RID: 14570
		public bool wildPlantUseDistanceToShore;

		// Token: 0x040038EB RID: 14571
		public float plantRespawningCommonalityFactor = 1f;

		// Token: 0x040038EC RID: 14572
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x040038ED RID: 14573
		public float sowWork = 10f;

		// Token: 0x040038EE RID: 14574
		public int sowMinSkill;

		// Token: 0x040038EF RID: 14575
		public bool blockAdjacentSow;

		// Token: 0x040038F0 RID: 14576
		public List<ResearchProjectDef> sowResearchPrerequisites;

		// Token: 0x040038F1 RID: 14577
		public bool mustBeWildToSow;

		// Token: 0x040038F2 RID: 14578
		public bool mustBePermanentDarknessToSow;

		// Token: 0x040038F3 RID: 14579
		public float harvestWork = 10f;

		// Token: 0x040038F4 RID: 14580
		public float harvestYield;

		// Token: 0x040038F5 RID: 14581
		public ThingDef harvestedThingDef;

		// Token: 0x040038F6 RID: 14582
		[NoTranslate]
		public string harvestTag;

		// Token: 0x040038F7 RID: 14583
		public float harvestMinGrowth = 0.65f;

		// Token: 0x040038F8 RID: 14584
		public float harvestAfterGrowth;

		// Token: 0x040038F9 RID: 14585
		public bool harvestFailable = true;

		// Token: 0x040038FA RID: 14586
		public bool harvestYieldAffectedByDifficulty = true;

		// Token: 0x040038FB RID: 14587
		public SoundDef soundHarvesting;

		// Token: 0x040038FC RID: 14588
		public SoundDef soundHarvestFinish;

		// Token: 0x040038FD RID: 14589
		public bool autoHarvestable = true;

		// Token: 0x040038FE RID: 14590
		public bool skipDeteriorationMessage;

		// Token: 0x040038FF RID: 14591
		public float growDays = 2f;

		// Token: 0x04003900 RID: 14592
		public float lifespanDaysPerGrowDays = 8f;

		// Token: 0x04003901 RID: 14593
		public float growMinGlow = 0.51f;

		// Token: 0x04003902 RID: 14594
		public float growOptimalGlow = 1f;

		// Token: 0x04003903 RID: 14595
		public bool diesToLight;

		// Token: 0x04003904 RID: 14596
		public bool vacuumResistant;

		// Token: 0x04003905 RID: 14597
		public bool terraformable;

		// Token: 0x04003906 RID: 14598
		public Pollution pollution;

		// Token: 0x04003907 RID: 14599
		public float minGrowthTemperature;

		// Token: 0x04003908 RID: 14600
		public float minOptimalGrowthTemperature = 6f;

		// Token: 0x04003909 RID: 14601
		public float maxOptimalGrowthTemperature = 42f;

		// Token: 0x0400390A RID: 14602
		public float maxGrowthTemperature = 58f;

		// Token: 0x0400390B RID: 14603
		public float fertilityMin = 0.9f;

		// Token: 0x0400390C RID: 14604
		public float fertilitySensitivity = 0.5f;

		// Token: 0x0400390D RID: 14605
		public bool completelyIgnoreFertility;

		// Token: 0x0400390E RID: 14606
		public List<TerrainDef> terrainBlacklist;

		// Token: 0x0400390F RID: 14607
		public bool dieIfLeafless;

		// Token: 0x04003910 RID: 14608
		public bool neverBlightable;

		// Token: 0x04003911 RID: 14609
		public bool interferesWithRoof;

		// Token: 0x04003912 RID: 14610
		public bool dieIfNoSunlight = true;

		// Token: 0x04003913 RID: 14611
		public bool dieFromToxicFallout = true;

		// Token: 0x04003914 RID: 14612
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x04003915 RID: 14613
		public bool humanFoodPlant;

		// Token: 0x04003916 RID: 14614
		public bool treeLoversCareIfChopped = true;

		// Token: 0x04003917 RID: 14615
		public bool allowAutoCut = true;

		// Token: 0x04003918 RID: 14616
		public bool drugForHarvestPurposes;

		// Token: 0x04003919 RID: 14617
		public TreeCategory treeCategory;

		// Token: 0x0400391A RID: 14618
		public ThingDef burnedThingDef;

		// Token: 0x0400391B RID: 14619
		public ThingDef choppedThingDef;

		// Token: 0x0400391C RID: 14620
		public ThingDef smashedThingDef;

		// Token: 0x0400391D RID: 14621
		public bool canDeteriorate;

		// Token: 0x0400391E RID: 14622
		public bool showGrowthInInspectPane = true;

		// Token: 0x0400391F RID: 14623
		public float minSpacingBetweenSamePlant;

		// Token: 0x04003920 RID: 14624
		public bool forceIsTree;

		// Token: 0x04003921 RID: 14625
		public bool warnIfMarkedForCut;

		// Token: 0x04003922 RID: 14626
		public bool isStump;

		// Token: 0x04003923 RID: 14627
		public float topWindExposure = 0.25f;

		// Token: 0x04003924 RID: 14628
		public int maxMeshCount = 1;

		// Token: 0x04003925 RID: 14629
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x04003926 RID: 14630
		public bool showInFrozenWater = true;

		// Token: 0x04003927 RID: 14631
		public bool destroyedByFlooding = true;

		// Token: 0x04003928 RID: 14632
		[NoTranslate]
		private string leaflessGraphicPath;

		// Token: 0x04003929 RID: 14633
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		// Token: 0x0400392A RID: 14634
		[NoTranslate]
		private string immatureGraphicPath;

		// Token: 0x0400392B RID: 14635
		[Unsaved(false)]
		public Graphic immatureGraphic;

		// Token: 0x0400392C RID: 14636
		public bool dropLeaves;

		// Token: 0x0400392D RID: 14637
		[NoTranslate]
		private string pollutedGraphicPath;

		// Token: 0x0400392E RID: 14638
		[Unsaved(false)]
		public Graphic pollutedGraphic;

		// Token: 0x0400392F RID: 14639
		[NoTranslate]
		private string leaflessImmatureGraphicPath;

		// Token: 0x04003930 RID: 14640
		[Unsaved(false)]
		public Graphic leaflessImmatureGraphic;

		// Token: 0x04003931 RID: 14641
		[NoTranslate]
		private string snowOverlayGraphicPath;

		// Token: 0x04003932 RID: 14642
		[Unsaved(false)]
		public Graphic snowOverlayGraphic;

		// Token: 0x04003933 RID: 14643
		[NoTranslate]
		private string leaflessSnowOverlayGraphicPath;

		// Token: 0x04003934 RID: 14644
		[Unsaved(false)]
		public Graphic leaflessSnowOverlayGraphic;

		// Token: 0x04003935 RID: 14645
		[NoTranslate]
		private string immatureSnowOverlayGraphicPath;

		// Token: 0x04003936 RID: 14646
		[Unsaved(false)]
		public Graphic immatureSnowOverlayGraphic;

		// Token: 0x04003937 RID: 14647
		public const int MaxMaxMeshCount = 25;

		// Token: 0x04003938 RID: 14648
		private HashSet<string> wildTerrainTagsSet;
	}
}
