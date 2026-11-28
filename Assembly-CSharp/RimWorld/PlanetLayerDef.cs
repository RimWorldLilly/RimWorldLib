using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC1 RID: 4033
	public class PlanetLayerDef : Def
	{
		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600666E RID: 26222 RVA: 0x00245E1B File Offset: 0x0024401B
		public bool SurfaceTiles
		{
			get
			{
				return this.usesSurfaceTiles;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x0600666F RID: 26223 RVA: 0x00245E23 File Offset: 0x00244023
		public BiomeDef DefaultBiome
		{
			get
			{
				return this.defaultBiome ?? BiomeDefOf.TemperateForest;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06006670 RID: 26224 RVA: 0x00245E34 File Offset: 0x00244034
		public Texture2D ViewGizmoTexture
		{
			get
			{
				if (!this.cachedGizmoTexture)
				{
					return this.cachedGizmoTexture = ContentFinder<Texture2D>.Get(this.viewGizmoTexPath, true);
				}
				return this.cachedGizmoTexture;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06006671 RID: 26225 RVA: 0x00245E6A File Offset: 0x0024406A
		public WorldObjectDef DefaultWorldObject
		{
			get
			{
				return this.defaultMapWorldObject ?? WorldObjectDefOf.Settlement;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06006672 RID: 26226 RVA: 0x00245E7B File Offset: 0x0024407B
		public WorldObjectDef SettlementWorldObjectDef
		{
			get
			{
				return this.settlementWorldObjectDef ?? WorldObjectDefOf.Settlement;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06006673 RID: 26227 RVA: 0x00245E8C File Offset: 0x0024408C
		public WorldObjectDef DestroyedSettlementWorldObjectDef
		{
			get
			{
				return this.destroyedSettlementWorldObjectDef ?? WorldObjectDefOf.DestroyedSettlement;
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06006674 RID: 26228 RVA: 0x00245E9D File Offset: 0x0024409D
		public Material WorldLineMaterial
		{
			get
			{
				return MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, this.lineColor, 3590);
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06006675 RID: 26229 RVA: 0x00245EB9 File Offset: 0x002440B9
		public Material WorldLineMaterialHighVis
		{
			get
			{
				return MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, this.lineColor, 3590);
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06006676 RID: 26230 RVA: 0x00245ED8 File Offset: 0x002440D8
		public List<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				List<WorldGenStepDef> result;
				if ((result = this.cachedGenSteps) == null)
				{
					result = (this.cachedGenSteps = (from x in this.worldGenSteps
					orderby x.order, x.index
					select x).ToList<WorldGenStepDef>());
				}
				return result;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x00245F4B File Offset: 0x0024414B
		public List<WITab> Tabs
		{
			get
			{
				return this.cachedTabs;
			}
		}

		// Token: 0x06006678 RID: 26232 RVA: 0x00245F53 File Offset: 0x00244153
		public override IEnumerable<string> ConfigErrors()
		{
			PlanetLayerDef.<ConfigErrors>d__57 <ConfigErrors>d__ = new PlanetLayerDef.<ConfigErrors>d__57(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006679 RID: 26233 RVA: 0x00245F64 File Offset: 0x00244164
		public override void PostLoad()
		{
			base.PostLoad();
			this.usesSurfaceTiles = typeof(SurfaceTile).IsAssignableFrom(this.tileType);
			foreach (Type type in this.worldTabs)
			{
				this.cachedTabs.Add((WITab)Activator.CreateInstance(type));
			}
		}

		// Token: 0x04003ED7 RID: 16087
		private BiomeDef defaultBiome;

		// Token: 0x04003ED8 RID: 16088
		private WorldObjectDef defaultMapWorldObject;

		// Token: 0x04003ED9 RID: 16089
		private WorldObjectDef settlementWorldObjectDef;

		// Token: 0x04003EDA RID: 16090
		private WorldObjectDef destroyedSettlementWorldObjectDef;

		// Token: 0x04003EDB RID: 16091
		public bool onlyAllowWhitelistedIncidents;

		// Token: 0x04003EDC RID: 16092
		public bool onlyAllowWhitelistedBiomes;

		// Token: 0x04003EDD RID: 16093
		public bool onlyAllowWhitelistedGameConditions;

		// Token: 0x04003EDE RID: 16094
		public bool onlyAllowWhitelistedQuests;

		// Token: 0x04003EDF RID: 16095
		public bool onlyAllowWhitelistedArrivals;

		// Token: 0x04003EE0 RID: 16096
		public bool onlyAllowWhitelistedArrivalModes;

		// Token: 0x04003EE1 RID: 16097
		public bool alwaysRaycastable = true;

		// Token: 0x04003EE2 RID: 16098
		public bool obstructsExpandingIcons = true;

		// Token: 0x04003EE3 RID: 16099
		public bool canFormCaravans = true;

		// Token: 0x04003EE4 RID: 16100
		public bool isSpace;

		// Token: 0x04003EE5 RID: 16101
		public float rangeDistanceFactor = 1f;

		// Token: 0x04003EE6 RID: 16102
		public float generatedLocationFactor = 1f;

		// Token: 0x04003EE7 RID: 16103
		public float raidPointsFactor = 1f;

		// Token: 0x04003EE8 RID: 16104
		public BiomeDef backgroundBiome;

		// Token: 0x04003EE9 RID: 16105
		public bool ignoreNoBuildArea;

		// Token: 0x04003EEA RID: 16106
		public Color lineColor = Color.white;

		// Token: 0x04003EEB RID: 16107
		public float lineWidthFactor = 1f;

		// Token: 0x04003EEC RID: 16108
		private List<WorldGenStepDef> worldGenSteps = new List<WorldGenStepDef>();

		// Token: 0x04003EED RID: 16109
		public List<Type> worldDrawLayers = new List<Type>();

		// Token: 0x04003EEE RID: 16110
		private List<Type> worldTabs = new List<Type>();

		// Token: 0x04003EEF RID: 16111
		public Type layerType = typeof(PlanetLayer);

		// Token: 0x04003EF0 RID: 16112
		public Type tileType = typeof(Tile);

		// Token: 0x04003EF1 RID: 16113
		[MustTranslate]
		public string elevationString = "{0}m";

		// Token: 0x04003EF2 RID: 16114
		[MustTranslate]
		public string viewGizmoTooltip;

		// Token: 0x04003EF3 RID: 16115
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04003EF4 RID: 16116
		public bool viewGizmoOnlyVisibleWithDirectConnection = true;

		// Token: 0x04003EF5 RID: 16117
		public FloatRange settlementsPer100kTiles = new FloatRange(75f, 85f);

		// Token: 0x04003EF6 RID: 16118
		public SimpleCurve viewAngleSettlementsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04003EF7 RID: 16119
		public string viewGizmoTexPath;

		// Token: 0x04003EF8 RID: 16120
		[Unsaved(false)]
		private List<WorldGenStepDef> cachedGenSteps;

		// Token: 0x04003EF9 RID: 16121
		[Unsaved(false)]
		private readonly List<WITab> cachedTabs = new List<WITab>();

		// Token: 0x04003EFA RID: 16122
		[Unsaved(false)]
		private Texture2D cachedGizmoTexture;

		// Token: 0x04003EFB RID: 16123
		private bool usesSurfaceTiles;
	}
}
