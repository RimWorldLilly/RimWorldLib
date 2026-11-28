using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000123 RID: 291
	public abstract class BuildableDef : Def, IEquatable<BuildableDef>
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00036344 File Offset: 0x00034544
		public virtual IntVec2 Size
		{
			get
			{
				return IntVec2.One;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0003634B File Offset: 0x0003454B
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0003635B File Offset: 0x0003455B
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00036366 File Offset: 0x00034566
		public Material DrawMatSingle
		{
			get
			{
				Graphic graphic = this.graphic;
				if (graphic == null)
				{
					return null;
				}
				return graphic.MatSingle;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00036379 File Offset: 0x00034579
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00036386 File Offset: 0x00034586
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00036398 File Offset: 0x00034598
		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				if (this.placeWorkersInstantiatedInt == null)
				{
					this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
					foreach (Type type in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
					}
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00036420 File Offset: 0x00034620
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00036461 File Offset: 0x00034661
		public List<ThingDefCountClass> CostList
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costList;
				}
				return this.costList;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0003648A File Offset: 0x0003468A
		public int CostStuffCount
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costStuffCount;
				}
				return this.costStuffCount;
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x000364B4 File Offset: 0x000346B4
		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000364F8 File Offset: 0x000346F8
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else
				{
					this.ResolveIcon();
				}
				if (this.uiIconPathsStuff != null)
				{
					this.stuffUiIcons = new Dictionary<StuffAppearanceDef, Texture2D>();
					foreach (IconForStuffAppearance iconForStuffAppearance in this.uiIconPathsStuff)
					{
						this.stuffUiIcons.Add(iconForStuffAppearance.Appearance, ContentFinder<Texture2D>.Get(iconForStuffAppearance.IconPath, true));
					}
				}
			});
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00036514 File Offset: 0x00034714
		protected virtual void ResolveIcon()
		{
			if (this.graphic == null || this.graphic == BaseContent.BadGraphic)
			{
				return;
			}
			ThingDef thingDef = this as ThingDef;
			if (thingDef != null && thingDef.mote != null)
			{
				return;
			}
			Graphic outerGraphic = this.graphic;
			if (this.uiIconForStackCount >= 1 && this is ThingDef)
			{
				Graphic_StackCount graphic_StackCount = this.graphic as Graphic_StackCount;
				if (graphic_StackCount != null)
				{
					outerGraphic = graphic_StackCount.SubGraphicForStackCount(this.uiIconForStackCount, (ThingDef)this);
				}
			}
			Material material = outerGraphic.ExtractInnerGraphicFor(null, null).MatAt(this.defaultPlacingRot, null);
			Shader shader;
			MaterialRequest materialRequest;
			if (ShaderDatabase.TryGetUIShader(material.shader, out shader) && MaterialPool.TryGetRequestForMat(material, out materialRequest))
			{
				materialRequest.shader = shader;
				if (materialRequest.colorTwo == Color.white)
				{
					materialRequest.colorTwo = this.uiIconColorTwo;
				}
				this.uiIconMaterial = MaterialPool.MatFrom(materialRequest);
			}
			this.uiIcon = (Texture2D)material.mainTexture;
			this.uiIconColor = material.color;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00036610 File Offset: 0x00034810
		public Texture2D GetUIIconForStuff(ThingDef stuff)
		{
			Texture2D result;
			if (this.stuffUiIcons == null || ((stuff != null) ? stuff.stuffProps.appearance : null) == null || !this.stuffUiIcons.TryGetValue(stuff.stuffProps.appearance, out result))
			{
				return this.uiIcon;
			}
			return result;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003665C File Offset: 0x0003485C
		public Color GetColorForStuff(ThingDef stuff)
		{
			if (this.colorPerStuff.NullOrEmpty<ColorForStuff>())
			{
				return stuff.stuffProps.color;
			}
			for (int i = 0; i < this.colorPerStuff.Count; i++)
			{
				ColorForStuff colorForStuff = this.colorPerStuff[i];
				if (colorForStuff.Stuff == stuff)
				{
					return colorForStuff.Color;
				}
			}
			return stuff.stuffProps.color;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000366C0 File Offset: 0x000348C0
		public override IEnumerable<string> ConfigErrors()
		{
			BuildableDef.<ConfigErrors>d__82 <ConfigErrors>d__ = new BuildableDef.<ConfigErrors>d__82(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000366D0 File Offset: 0x000348D0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			BuildableDef.<SpecialDisplayStats>d__85 <SpecialDisplayStats>d__ = new BuildableDef.<SpecialDisplayStats>d__85(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000366E7 File Offset: 0x000348E7
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000366EF File Offset: 0x000348EF
		public bool Equals(BuildableDef other)
		{
			return this.defName == other.defName;
		}

		// Token: 0x040004CC RID: 1228
		public List<StatModifier> statBases;

		// Token: 0x040004CD RID: 1229
		public Traversability passability;

		// Token: 0x040004CE RID: 1230
		public int pathCost;

		// Token: 0x040004CF RID: 1231
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x040004D0 RID: 1232
		public float fertility = -1f;

		// Token: 0x040004D1 RID: 1233
		public List<ThingDefCountClass> costList;

		// Token: 0x040004D2 RID: 1234
		public int costStuffCount;

		// Token: 0x040004D3 RID: 1235
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x040004D4 RID: 1236
		[MustTranslate]
		public string stuffCategorySummary;

		// Token: 0x040004D5 RID: 1237
		public CostListForDifficulty costListForDifficulty;

		// Token: 0x040004D6 RID: 1238
		public DrawStyleCategoryDef drawStyleCategory;

		// Token: 0x040004D7 RID: 1239
		public bool clearBuildingArea = true;

		// Token: 0x040004D8 RID: 1240
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x040004D9 RID: 1241
		public float resourcesFractionWhenDeconstructed = 0.5f;

		// Token: 0x040004DA RID: 1242
		public List<AltitudeLayer> blocksAltitudes;

		// Token: 0x040004DB RID: 1243
		public StyleCategoryDef dominantStyleCategory;

		// Token: 0x040004DC RID: 1244
		public bool forcePassableByFlyingPawns;

		// Token: 0x040004DD RID: 1245
		public bool forceMoveItemsBeforeConstruction;

		// Token: 0x040004DE RID: 1246
		public bool isAltar;

		// Token: 0x040004DF RID: 1247
		public bool useStuffTerrainAffordance;

		// Token: 0x040004E0 RID: 1248
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x040004E1 RID: 1249
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x040004E2 RID: 1250
		public List<ThingDef> discoveryPrerequisites;

		// Token: 0x040004E3 RID: 1251
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040004E4 RID: 1252
		public int minMonolithLevel;

		// Token: 0x040004E5 RID: 1253
		public int constructionSkillPrerequisite;

		// Token: 0x040004E6 RID: 1254
		public int artisticSkillPrerequisite;

		// Token: 0x040004E7 RID: 1255
		public TechLevel minTechLevelToBuild;

		// Token: 0x040004E8 RID: 1256
		public TechLevel maxTechLevelToBuild;

		// Token: 0x040004E9 RID: 1257
		public bool requireInspectedGravEngine;

		// Token: 0x040004EA RID: 1258
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x040004EB RID: 1259
		public EffecterDef repairEffect;

		// Token: 0x040004EC RID: 1260
		public EffecterDef constructEffect;

		// Token: 0x040004ED RID: 1261
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x040004EE RID: 1262
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x040004EF RID: 1263
		public bool ideoBuilding;

		// Token: 0x040004F0 RID: 1264
		public float specialDisplayRadius;

		// Token: 0x040004F1 RID: 1265
		public List<Type> placeWorkers;

		// Token: 0x040004F2 RID: 1266
		public DesignationCategoryDef designationCategory;

		// Token: 0x040004F3 RID: 1267
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x040004F4 RID: 1268
		public KeyBindingDef designationHotKey;

		// Token: 0x040004F5 RID: 1269
		public float uiOrder = 2999f;

		// Token: 0x040004F6 RID: 1270
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x040004F7 RID: 1271
		public List<IconForStuffAppearance> uiIconPathsStuff;

		// Token: 0x040004F8 RID: 1272
		public Vector2 uiIconOffset;

		// Token: 0x040004F9 RID: 1273
		public Color uiIconColor = Color.white;

		// Token: 0x040004FA RID: 1274
		public Color uiIconColorTwo = Color.white;

		// Token: 0x040004FB RID: 1275
		public int uiIconForStackCount = -1;

		// Token: 0x040004FC RID: 1276
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x040004FD RID: 1277
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x040004FE RID: 1278
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x040004FF RID: 1279
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x04000500 RID: 1280
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x04000501 RID: 1281
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04000502 RID: 1282
		[Unsaved(false)]
		public Material uiIconMaterial;

		// Token: 0x04000503 RID: 1283
		[Unsaved(false)]
		public Dictionary<StuffAppearanceDef, Texture2D> stuffUiIcons;

		// Token: 0x04000504 RID: 1284
		[Unsaved(false)]
		public float uiIconAngle;

		// Token: 0x04000505 RID: 1285
		protected static List<string> tmpCostList = new List<string>();

		// Token: 0x04000506 RID: 1286
		protected static List<Dialog_InfoCard.Hyperlink> tmpHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
	}
}
