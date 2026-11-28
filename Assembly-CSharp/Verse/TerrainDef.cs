using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C8 RID: 456
	public class TerrainDef : BuildableDef
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x000414AD File Offset: 0x0003F6AD
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000414B5 File Offset: 0x0003F6B5
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites.NotNullAndContains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000414C7 File Offset: 0x0003F6C7
		public Color DrawColor
		{
			get
			{
				if (this.colorDef != null)
				{
					return this.colorDef.color;
				}
				return this.color;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000414E3 File Offset: 0x0003F6E3
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x000414F0 File Offset: 0x0003F6F0
		public bool IsOcean
		{
			get
			{
				return this.HasTag("Ocean");
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000414FD File Offset: 0x0003F6FD
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004150A File Offset: 0x0003F70A
		public bool IsIce
		{
			get
			{
				return this.HasTag("Ice");
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00041517 File Offset: 0x0003F717
		public bool IsFlood
		{
			get
			{
				return this.HasTag("Flood");
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00041524 File Offset: 0x0003F724
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00041531 File Offset: 0x0003F731
		public bool IsSoil
		{
			get
			{
				return this.HasTag("Soil");
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0004153E File Offset: 0x0003F73E
		public bool IsRoad
		{
			get
			{
				return this.HasTag("Road");
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0004154B File Offset: 0x0003F74B
		public bool IsFloor
		{
			get
			{
				return this.HasTag("Floor");
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00041558 File Offset: 0x0003F758
		public bool IsRock
		{
			get
			{
				return this.HasTag("NaturalRock");
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00041565 File Offset: 0x0003F765
		public bool IsSubstructure
		{
			get
			{
				return ModsConfig.OdysseyActive && this.HasTag("Substructure");
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0004157C File Offset: 0x0003F77C
		public Shader Shader
		{
			get
			{
				if (this.customShader != null)
				{
					return this.customShader.Shader;
				}
				Shader result = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					result = ShaderDatabase.TerrainHard;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					result = ShaderDatabase.TerrainFade;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					result = ShaderDatabase.TerrainFadeRough;
					break;
				case TerrainDef.TerrainEdgeType.Water:
					result = ShaderDatabase.TerrainWater;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x000415E0 File Offset: 0x0003F7E0
		private Shader ShaderPolluted
		{
			get
			{
				if (this.pollutionShaderType != null)
				{
					return this.pollutionShaderType.Shader;
				}
				if (this.customShader != null)
				{
					return this.customShader.Shader;
				}
				Shader result = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					result = ShaderDatabase.TerrainHardPolluted;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					result = ShaderDatabase.TerrainFadePolluted;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					result = ShaderDatabase.TerrainFadeRoughPolluted;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00041649 File Offset: 0x0003F849
		public Material DrawMatPolluted
		{
			get
			{
				if (this.graphicPolluted == BaseContent.BadGraphic)
				{
					return this.graphic.MatSingle;
				}
				return this.graphicPolluted.MatSingle;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00041670 File Offset: 0x0003F870
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.drawStyleCategory == null)
				{
					this.drawStyleCategory = DrawStyleCategoryDefOf.Floors;
				}
				if (this.dontRender)
				{
					return;
				}
				if (this.graphic == BaseContent.BadGraphic)
				{
					this.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, this.Shader, Vector2.one, this.DrawColor, 2000 + this.renderPrecedence);
					if (this.edgeType == TerrainDef.TerrainEdgeType.FadeRough || this.edgeType == TerrainDef.TerrainEdgeType.Water)
					{
						this.graphic.MatSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (this.customShader != null && this.customShaderParameters != null)
					{
						for (int j = 0; j < this.customShaderParameters.Count; j++)
						{
							this.customShaderParameters[j].Apply(this.graphic.MatSingle);
						}
					}
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int k = 0; k < this.waterDepthShaderParameters.Count; k++)
						{
							this.waterDepthShaderParameters[k].Apply(this.waterDepthMaterial);
						}
					}
				}
				if (ModsConfig.BiotechActive && this.graphicPolluted == BaseContent.BadGraphic && (!this.pollutionOverlayTexturePath.NullOrEmpty() || !this.pollutedTexturePath.NullOrEmpty()))
				{
					Texture2D texture2D = null;
					if (!this.pollutionOverlayTexturePath.NullOrEmpty())
					{
						texture2D = ContentFinder<Texture2D>.Get(this.pollutionOverlayTexturePath, true);
					}
					this.graphicPolluted = GraphicDatabase.Get<Graphic_Terrain>(this.pollutedTexturePath ?? this.texturePath, this.ShaderPolluted, Vector2.one, this.DrawColor, 2000 + this.renderPrecedence);
					Material matSingle = this.graphicPolluted.MatSingle;
					if (texture2D != null)
					{
						matSingle.SetTexture(ShaderPropertyIDs.BurnTex, texture2D);
					}
					matSingle.SetColor(ShaderPropertyIDs.BurnColor, this.pollutionColor);
					matSingle.SetVector(ShaderPropertyIDs.BurnScale, this.pollutionOverlayScale);
					matSingle.SetVector(ShaderPropertyIDs.ScrollSpeed, this.pollutionOverlayScrollSpeed);
					matSingle.SetColor(ShaderPropertyIDs.PollutionTintColor, this.pollutionTintColor);
					if (this.edgeType == TerrainDef.TerrainEdgeType.FadeRough)
					{
						matSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (matSingle != this.graphic.MatSingle)
					{
						matSingle.SetFloat(ShaderPropertyIDs.IsPolluted, 1f);
					}
					if ((this.pollutionShaderType != null || this.customShader != null) && this.customShaderParameters != null)
					{
						for (int l = 0; l < this.customShaderParameters.Count; l++)
						{
							this.customShaderParameters[l].Apply(matSingle);
						}
					}
				}
				if (!this.bridgePropsPath.NullOrEmpty() && this.bridgePropsLoopGraphic == BaseContent.BadGraphic)
				{
					this.bridgePropsLoopGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.bridgePropsPath + "_Loop", ShaderDatabase.Transparent);
					this.bridgePropsRightGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.bridgePropsPath + "_Right", ShaderDatabase.Transparent);
				}
				if (ModsConfig.OdysseyActive && !this.spaceBridgePropsPath.NullOrEmpty() && this.spaceBridgePropsLoopGraphic == BaseContent.BadGraphic)
				{
					this.spaceBridgePropsLoopGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.spaceBridgePropsPath + "_Loop", ShaderDatabase.Transparent);
					this.spaceBridgePropsRightGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.spaceBridgePropsPath + "_Right", ShaderDatabase.Transparent);
				}
			});
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			base.PostLoad();
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000416CA File Offset: 0x0003F8CA
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.DrawColor;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000416DE File Offset: 0x0003F8DE
		public override IEnumerable<string> ConfigErrors()
		{
			TerrainDef.<ConfigErrors>d__120 <ConfigErrors>d__ = new TerrainDef.<ConfigErrors>d__120(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000416EE File Offset: 0x0003F8EE
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000416F7 File Offset: 0x0003F8F7
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0004170F File Offset: 0x0003F90F
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			TerrainDef.<SpecialDisplayStats>d__123 <SpecialDisplayStats>d__ = new TerrainDef.<SpecialDisplayStats>d__123(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04000BD5 RID: 3029
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000BD6 RID: 3030
		public TerrainDef.TerrainEdgeType edgeType;

		// Token: 0x04000BD7 RID: 3031
		public ShaderTypeDef customShader;

		// Token: 0x04000BD8 RID: 3032
		public List<ShaderParameter> customShaderParameters;

		// Token: 0x04000BD9 RID: 3033
		[NoTranslate]
		public string waterDepthShader;

		// Token: 0x04000BDA RID: 3034
		public List<ShaderParameter> waterDepthShaderParameters;

		// Token: 0x04000BDB RID: 3035
		public int renderPrecedence;

		// Token: 0x04000BDC RID: 3036
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x04000BDD RID: 3037
		public bool layerable;

		// Token: 0x04000BDE RID: 3038
		[NoTranslate]
		public string scatterType;

		// Token: 0x04000BDF RID: 3039
		public bool takeFootprints;

		// Token: 0x04000BE0 RID: 3040
		public bool natural;

		// Token: 0x04000BE1 RID: 3041
		public bool takeSplashes;

		// Token: 0x04000BE2 RID: 3042
		public bool avoidWander;

		// Token: 0x04000BE3 RID: 3043
		public bool changeable = true;

		// Token: 0x04000BE4 RID: 3044
		public TerrainDef smoothedTerrain;

		// Token: 0x04000BE5 RID: 3045
		public TerrainDef gravshipReplacementTerrain;

		// Token: 0x04000BE6 RID: 3046
		public bool holdSnowOrSand = true;

		// Token: 0x04000BE7 RID: 3047
		public bool isPaintable;

		// Token: 0x04000BE8 RID: 3048
		public bool extinguishesFire;

		// Token: 0x04000BE9 RID: 3049
		public Color color = Color.white;

		// Token: 0x04000BEA RID: 3050
		public ColorDef colorDef;

		// Token: 0x04000BEB RID: 3051
		public TerrainDef driesTo;

		// Token: 0x04000BEC RID: 3052
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04000BED RID: 3053
		public TerrainDef burnedDef;

		// Token: 0x04000BEE RID: 3054
		public List<Tool> tools;

		// Token: 0x04000BEF RID: 3055
		public float extraDeteriorationFactor;

		// Token: 0x04000BF0 RID: 3056
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x04000BF1 RID: 3057
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x04000BF2 RID: 3058
		public ThoughtDef traversedThought;

		// Token: 0x04000BF3 RID: 3059
		public int extraDraftedPerceivedPathCost;

		// Token: 0x04000BF4 RID: 3060
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x04000BF5 RID: 3061
		public EffecterDef destroyEffect;

		// Token: 0x04000BF6 RID: 3062
		public EffecterDef destroyEffectWater;

		// Token: 0x04000BF7 RID: 3063
		public bool autoRebuildable;

		// Token: 0x04000BF8 RID: 3064
		public TerrainDef.TerrainCategoryType categoryType;

		// Token: 0x04000BF9 RID: 3065
		public float meltSnowRadius;

		// Token: 0x04000BFA RID: 3066
		public float heatPerTick;

		// Token: 0x04000BFB RID: 3067
		public float igniteRadius;

		// Token: 0x04000BFC RID: 3068
		public float ignitePawnsIntervalTicks;

		// Token: 0x04000BFD RID: 3069
		public int burnDamage;

		// Token: 0x04000BFE RID: 3070
		public float burnIntervalTicks;

		// Token: 0x04000BFF RID: 3071
		public bool supportsRock = true;

		// Token: 0x04000C00 RID: 3072
		public float toxicBuildupFactor;

		// Token: 0x04000C01 RID: 3073
		public bool dontRender;

		// Token: 0x04000C02 RID: 3074
		public bool exposesToVacuum;

		// Token: 0x04000C03 RID: 3075
		public bool canFreeze;

		// Token: 0x04000C04 RID: 3076
		public bool dangerous;

		// Token: 0x04000C05 RID: 3077
		public bool preventCraters;

		// Token: 0x04000C06 RID: 3078
		public bool canEverTerraform = true;

		// Token: 0x04000C07 RID: 3079
		public WaterBodyType waterBodyType;

		// Token: 0x04000C08 RID: 3080
		public bool cropIcon = true;

		// Token: 0x04000C09 RID: 3081
		public float glowRadius;

		// Token: 0x04000C0A RID: 3082
		public ColorInt glowColor;

		// Token: 0x04000C0B RID: 3083
		public float throwFleckChance;

		// Token: 0x04000C0C RID: 3084
		public TerrainFleckData fleckData;

		// Token: 0x04000C0D RID: 3085
		public bool isFoundation;

		// Token: 0x04000C0E RID: 3086
		public bool bridge;

		// Token: 0x04000C0F RID: 3087
		[NoTranslate]
		public string bridgePropsPath;

		// Token: 0x04000C10 RID: 3088
		[NoTranslate]
		public string spaceBridgePropsPath;

		// Token: 0x04000C11 RID: 3089
		[Unsaved(false)]
		public Graphic bridgePropsLoopGraphic = BaseContent.BadGraphic;

		// Token: 0x04000C12 RID: 3090
		[Unsaved(false)]
		public Graphic bridgePropsRightGraphic = BaseContent.BadGraphic;

		// Token: 0x04000C13 RID: 3091
		[Unsaved(false)]
		public Graphic spaceBridgePropsLoopGraphic = BaseContent.BadGraphic;

		// Token: 0x04000C14 RID: 3092
		[Unsaved(false)]
		public Graphic spaceBridgePropsRightGraphic = BaseContent.BadGraphic;

		// Token: 0x04000C15 RID: 3093
		public bool temporary;

		// Token: 0x04000C16 RID: 3094
		public TempTerrainProps tempTerrain;

		// Token: 0x04000C17 RID: 3095
		public TerrainDef floodTerrain;

		// Token: 0x04000C18 RID: 3096
		public bool canBePolluted = true;

		// Token: 0x04000C19 RID: 3097
		[NoTranslate]
		public string pollutedTexturePath;

		// Token: 0x04000C1A RID: 3098
		[NoTranslate]
		public string pollutionOverlayTexturePath;

		// Token: 0x04000C1B RID: 3099
		public ShaderTypeDef pollutionShaderType;

		// Token: 0x04000C1C RID: 3100
		public Color pollutionColor = Color.white;

		// Token: 0x04000C1D RID: 3101
		public Vector2 pollutionOverlayScrollSpeed = Vector2.zero;

		// Token: 0x04000C1E RID: 3102
		public Vector2 pollutionOverlayScale = Vector2.one;

		// Token: 0x04000C1F RID: 3103
		public Color pollutionCloudColor = Color.white;

		// Token: 0x04000C20 RID: 3104
		public Color pollutionTintColor = Color.white;

		// Token: 0x04000C21 RID: 3105
		public ThingDef generatedFilth;

		// Token: 0x04000C22 RID: 3106
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		// Token: 0x04000C23 RID: 3107
		public TerrainDef.SpaceEdgeGraphicData spaceEdgeGraphicData;

		// Token: 0x04000C24 RID: 3108
		[Unsaved(false)]
		public Material waterDepthMaterial;

		// Token: 0x04000C25 RID: 3109
		[Unsaved(false)]
		public Graphic graphicPolluted = BaseContent.BadGraphic;

		// Token: 0x0200255C RID: 9564
		public enum TerrainEdgeType : byte
		{
			// Token: 0x04009923 RID: 39203
			Hard,
			// Token: 0x04009924 RID: 39204
			Fade,
			// Token: 0x04009925 RID: 39205
			FadeRough,
			// Token: 0x04009926 RID: 39206
			Water
		}

		// Token: 0x0200255D RID: 9565
		public enum TerrainCategoryType : byte
		{
			// Token: 0x04009928 RID: 39208
			Misc,
			// Token: 0x04009929 RID: 39209
			Soil,
			// Token: 0x0400992A RID: 39210
			Stone,
			// Token: 0x0400992B RID: 39211
			Sand
		}

		// Token: 0x0200255E RID: 9566
		public class SpaceEdgeGraphicData
		{
			// Token: 0x17002A33 RID: 10803
			// (get) Token: 0x0600EEB3 RID: 61107 RVA: 0x0057F8FB File Offset: 0x0057DAFB
			public Shader LoopShader
			{
				get
				{
					ShaderTypeDef shaderTypeDef = this.loopShader;
					return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.TerrainEdge;
				}
			}

			// Token: 0x17002A34 RID: 10804
			// (get) Token: 0x0600EEB4 RID: 61108 RVA: 0x0057F918 File Offset: 0x0057DB18
			public Shader CornerShader
			{
				get
				{
					ShaderTypeDef shaderTypeDef = this.cornerShader;
					return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.Transparent;
				}
			}

			// Token: 0x0600EEB5 RID: 61109 RVA: 0x0057F938 File Offset: 0x0057DB38
			public void EnsureInitialized(TerrainDef def)
			{
				if (this.initialized)
				{
					return;
				}
				this.initialized = true;
				Shader shader = this.LoopShader;
				Shader shader2 = this.CornerShader;
				this.oShape = new CachedMaterial(this.OShapeTexPath, shader2, this.overrideColor ?? def.color);
				this.uShape = new CachedMaterial(this.UShapeTexPath, shader2, this.overrideColor ?? def.color);
				this.cornerInner = new CachedMaterial(this.CornerInnerTexPath, shader2, this.overrideColor ?? def.color);
				this.cornerOuter = new CachedMaterial(this.CornerOuterTexPath, shader2, this.overrideColor ?? def.color);
				this.flat = new CachedMaterial(this.FlatTexPath, shader2, this.overrideColor ?? def.color);
				this.loopLeft = new CachedMaterial(this.LoopLeftTexPath, shader, this.overrideColor ?? def.color);
				this.loopRight = new CachedMaterial(this.LoopRightTexPath, shader, this.overrideColor ?? def.color);
				this.loopSingle = new CachedMaterial(this.LoopSingleTexPath, shader, this.overrideColor ?? def.color);
				foreach (string texPath in this.LoopTexPaths)
				{
					this.loops.Add(new CachedMaterial(texPath, shader, this.overrideColor ?? def.color));
				}
			}

			// Token: 0x0600EEB6 RID: 61110 RVA: 0x0057FB60 File Offset: 0x0057DD60
			public Material GetMaterial(TerrainDef terrain, SectionLayer_TerrainEdges.EdgeType edgeType, int listIndexOffset = 0)
			{
				this.EnsureInitialized(terrain);
				switch (edgeType)
				{
				case SectionLayer_TerrainEdges.EdgeType.OShape:
					return this.oShape.Material;
				case SectionLayer_TerrainEdges.EdgeType.UShape:
					return this.uShape.Material;
				case SectionLayer_TerrainEdges.EdgeType.CornerInner:
					return this.cornerInner.Material;
				case SectionLayer_TerrainEdges.EdgeType.CornerOuter:
					return this.cornerOuter.Material;
				case SectionLayer_TerrainEdges.EdgeType.Flat:
					return this.flat.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopLeft:
					return this.loopLeft.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopRight:
					return this.loopRight.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopSingle:
					return this.loopSingle.Material;
				case SectionLayer_TerrainEdges.EdgeType.Loop:
					return this.loops[Mathf.Abs(listIndexOffset) % this.loops.Count].Material;
				default:
					throw new ArgumentOutOfRangeException("edgeType", edgeType, null);
				}
			}

			// Token: 0x0400992C RID: 39212
			[NoTranslate]
			public string OShapeTexPath;

			// Token: 0x0400992D RID: 39213
			[NoTranslate]
			public string UShapeTexPath;

			// Token: 0x0400992E RID: 39214
			[NoTranslate]
			public string CornerInnerTexPath;

			// Token: 0x0400992F RID: 39215
			[NoTranslate]
			public string CornerOuterTexPath;

			// Token: 0x04009930 RID: 39216
			[NoTranslate]
			public string FlatTexPath;

			// Token: 0x04009931 RID: 39217
			[NoTranslate]
			public string LoopLeftTexPath;

			// Token: 0x04009932 RID: 39218
			[NoTranslate]
			public string LoopRightTexPath;

			// Token: 0x04009933 RID: 39219
			[NoTranslate]
			public string LoopSingleTexPath;

			// Token: 0x04009934 RID: 39220
			[NoTranslate]
			public List<string> LoopTexPaths;

			// Token: 0x04009935 RID: 39221
			public Color? overrideColor;

			// Token: 0x04009936 RID: 39222
			private CachedMaterial oShape;

			// Token: 0x04009937 RID: 39223
			private CachedMaterial uShape;

			// Token: 0x04009938 RID: 39224
			private CachedMaterial cornerInner;

			// Token: 0x04009939 RID: 39225
			private CachedMaterial cornerOuter;

			// Token: 0x0400993A RID: 39226
			private CachedMaterial flat;

			// Token: 0x0400993B RID: 39227
			private CachedMaterial loopLeft;

			// Token: 0x0400993C RID: 39228
			private CachedMaterial loopRight;

			// Token: 0x0400993D RID: 39229
			private CachedMaterial loopSingle;

			// Token: 0x0400993E RID: 39230
			private ShaderTypeDef loopShader;

			// Token: 0x0400993F RID: 39231
			private ShaderTypeDef cornerShader;

			// Token: 0x04009940 RID: 39232
			private readonly List<CachedMaterial> loops = new List<CachedMaterial>();

			// Token: 0x04009941 RID: 39233
			private bool initialized;
		}
	}
}
