using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9C RID: 3740
	public static class TerrainDefGenerator_Carpet
	{
		// Token: 0x06006290 RID: 25232 RVA: 0x0023770C File Offset: 0x0023590C
		public static IEnumerable<TerrainDef> ImpliedTerrainDefs(bool hotReload = false)
		{
			TerrainDefGenerator_Carpet.<ImpliedTerrainDefs>d__0 <ImpliedTerrainDefs>d__ = new TerrainDefGenerator_Carpet.<ImpliedTerrainDefs>d__0(-2);
			<ImpliedTerrainDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedTerrainDefs>d__;
		}

		// Token: 0x06006291 RID: 25233 RVA: 0x0023771C File Offset: 0x0023591C
		public static TerrainDef CarpetFromBlueprint(TerrainTemplateDef tp, ColorDef colorDef, int index, bool hotReload = false)
		{
			string defName = tp.defName + colorDef.defName.Replace("Structure_", "");
			TerrainDef terrainDef = hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName, false) ?? new TerrainDef()) : new TerrainDef();
			terrainDef.defName = defName;
			terrainDef.label = tp.label.Formatted(colorDef.label);
			terrainDef.texturePath = tp.texturePath;
			terrainDef.researchPrerequisites = tp.researchPrerequisites;
			terrainDef.burnedDef = tp.burnedDef;
			terrainDef.costList = tp.costList;
			terrainDef.description = tp.description;
			terrainDef.colorDef = colorDef;
			terrainDef.designatorDropdown = tp.designatorDropdown;
			terrainDef.uiOrder = (float)tp.uiOrder;
			terrainDef.statBases = tp.statBases;
			terrainDef.renderPrecedence = tp.renderPrecedenceStart - index;
			terrainDef.constructionSkillPrerequisite = tp.constructionSkillPrerequisite;
			terrainDef.canGenerateDefaultDesignator = tp.canGenerateDefaultDesignator;
			terrainDef.tags = tp.tags;
			terrainDef.dominantStyleCategory = tp.dominantStyleCategory;
			terrainDef.layerable = true;
			terrainDef.affordances = new List<TerrainAffordanceDef>
			{
				TerrainAffordanceDefOf.Walkable,
				TerrainAffordanceDefOf.Light,
				TerrainAffordanceDefOf.Medium,
				TerrainAffordanceDefOf.Heavy
			};
			terrainDef.designationCategory = DesignationCategoryDefOf.Floors;
			terrainDef.fertility = 0f;
			terrainDef.constructEffect = EffecterDefOf.ConstructDirt;
			terrainDef.pollutionColor = new Color(1f, 1f, 1f, 0.8f);
			terrainDef.pollutionOverlayScale = new Vector2(0.75f, 0.75f);
			terrainDef.pollutionOverlayTexturePath = "Terrain/Surfaces/PollutionFloorSmooth";
			terrainDef.terrainAffordanceNeeded = TerrainAffordanceDefOf.Heavy;
			if (ModsConfig.BiotechActive)
			{
				terrainDef.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearBurn;
			}
			return terrainDef;
		}
	}
}
