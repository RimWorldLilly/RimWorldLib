using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9F RID: 3743
	public static class ThingDefGenerator_Buildings
	{
		// Token: 0x06006295 RID: 25237 RVA: 0x002379D7 File Offset: 0x00235BD7
		public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs(bool hotReload = false)
		{
			ThingDefGenerator_Buildings.<ImpliedBlueprintAndFrameDefs>d__5 <ImpliedBlueprintAndFrameDefs>d__ = new ThingDefGenerator_Buildings.<ImpliedBlueprintAndFrameDefs>d__5(-2);
			<ImpliedBlueprintAndFrameDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedBlueprintAndFrameDefs>d__;
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x002379E8 File Offset: 0x00235BE8
		private static ThingDef BaseBlueprintDef()
		{
			return new ThingDef
			{
				category = ThingCategory.Ethereal,
				label = "Unspecified blueprint",
				altitudeLayer = AltitudeLayer.Blueprint,
				useHitPoints = false,
				selectable = true,
				seeThroughFog = true,
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				drawerType = DrawerType.MapMeshOnly
			};
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x00237A54 File Offset: 0x00235C54
		private static ThingDef BaseFrameDef()
		{
			return new ThingDef
			{
				isFrameInt = true,
				category = ThingCategory.Building,
				label = "Unspecified building frame",
				thingClass = typeof(Frame),
				altitudeLayer = AltitudeLayer.BuildingOnTop,
				useHitPoints = true,
				selectable = true,
				drawerType = DrawerType.RealtimeOnly,
				building = new BuildingProperties(),
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				scatterableOnMapGen = false,
				leaveResourcesWhenKilled = true
			};
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x00237AE8 File Offset: 0x00235CE8
		private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + def.defName;
			if (isInstallBlueprint)
			{
				defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + ThingDefGenerator_Buildings.InstallBlueprintDefNamePrefix + def.defName;
			}
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseBlueprintDef()) : ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.defName = defName;
			thingDef.label = def.label + "BlueprintLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			thingDef.rotatable = def.rotatable;
			thingDef.replaceTags = def.replaceTags;
			if (!isInstallBlueprint)
			{
				thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
				thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			}
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (isInstallBlueprint && normalBlueprint != null)
			{
				thingDef.graphicData = normalBlueprint.graphicData;
			}
			else
			{
				thingDef.graphicData = new GraphicData();
				if (def.building != null && def.building.blueprintGraphicData != null)
				{
					thingDef.graphicData.CopyFrom(def.building.blueprintGraphicData);
					if (thingDef.graphicData.graphicClass == null)
					{
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					}
					if (thingDef.graphicData.shaderType == null)
					{
						thingDef.graphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (def.graphicData != null)
					{
						if (def.building.blueprintGraphicData.drawSize == Vector2.one)
						{
							thingDef.graphicData.drawSize = def.graphicData.drawSize;
						}
						thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
						thingDef.graphicData.linkType = def.graphicData.linkType;
						thingDef.graphicData.asymmetricLink = def.graphicData.asymmetricLink;
					}
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
				}
				else
				{
					thingDef.graphicData.CopyFrom(def.graphicData);
					thingDef.graphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					thingDef.graphicData.colorTwo = Color.white;
					thingDef.graphicData.shadowData = null;
				}
			}
			thingDef.graphicData.renderQueue = 2950;
			thingDef.defaultPlacingRot = def.defaultPlacingRot;
			if (thingDef.graphicData.shadowData != null)
			{
				Log.Error("Blueprint has shadow: " + ((def != null) ? def.ToString() : null));
			}
			if (isInstallBlueprint)
			{
				thingDef.thingClass = typeof(Blueprint_Install);
			}
			else if (def.building != null)
			{
				thingDef.thingClass = def.building.blueprintClass;
			}
			else
			{
				Log.Error("Tried creating build blueprint for thing that has no blueprint class assigned!");
			}
			if (def.thingClass == typeof(Building_Door))
			{
				thingDef.drawerType = DrawerType.RealtimeOnly;
			}
			else
			{
				thingDef.drawerType = def.drawerType;
			}
			thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
			thingDef.entityDefToBuild = def;
			if (isInstallBlueprint)
			{
				def.installBlueprintDef = thingDef;
			}
			else
			{
				def.blueprintDef = thingDef;
			}
			return thingDef;
		}

		// Token: 0x06006299 RID: 25241 RVA: 0x00237E24 File Offset: 0x00236024
		private static ThingDef NewFrameDef_Thing(ThingDef def, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + def.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseFrameDef()) : ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = defName;
			thingDef.label = def.label + "FrameLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)def.BaseMaxHitPoints * 0.25f);
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
			thingDef.SetStatBaseValue(StatDefOf.Flammability, def.BaseFlammability);
			thingDef.fillPercent = 0.2f;
			thingDef.pathCost = 14;
			thingDef.description = def.description;
			thingDef.passability = def.passability;
			thingDef.altitudeLayer = def.altitudeLayer;
			if (thingDef.passability > Traversability.PassThroughOnly)
			{
				thingDef.passability = Traversability.PassThroughOnly;
			}
			thingDef.selectable = def.selectable;
			thingDef.constructEffect = def.constructEffect;
			thingDef.building.isEdifice = def.building.isEdifice;
			thingDef.building.watchBuildingInSameRoom = def.building.watchBuildingInSameRoom;
			thingDef.building.watchBuildingStandDistanceRange = def.building.watchBuildingStandDistanceRange;
			thingDef.building.watchBuildingStandRectWidth = def.building.watchBuildingStandRectWidth;
			thingDef.building.artificialForMeditationPurposes = def.building.artificialForMeditationPurposes;
			thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = def.modContentPack;
			thingDef.blocksAltitudes = def.blocksAltitudes;
			thingDef.replaceTags = def.replaceTags;
			thingDef.rotatable = def.rotatable;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (def.BuildableByPlayer)
			{
				thingDef.stuffCategories = def.stuffCategories;
				thingDef.costListForDifficulty = def.costListForDifficulty;
			}
			thingDef.entityDefToBuild = def;
			def.frameDef = thingDef;
			return thingDef;
		}

		// Token: 0x0600629A RID: 25242 RVA: 0x0023804C File Offset: 0x0023624C
		private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + terrDef.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseBlueprintDef()) : ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.thingClass = typeof(Blueprint_Build);
			thingDef.defName = defName;
			thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.graphicData = new GraphicData();
			thingDef.graphicData.shaderType = ShaderTypeDefOf.MetaOverlay;
			thingDef.graphicData.texPath = ThingDefGenerator_Buildings.TerrainBlueprintGraphicPath;
			thingDef.graphicData.renderQueue = 2950;
			thingDef.graphicData.graphicClass = typeof(Graphic_Single);
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.entityDefToBuild = terrDef;
			terrDef.blueprintDef = thingDef;
			return thingDef;
		}

		// Token: 0x0600629B RID: 25243 RVA: 0x0023815C File Offset: 0x0023635C
		private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + terrDef.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseFrameDef()) : ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.building.artificialForMeditationPurposes = false;
			thingDef.defName = defName;
			thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.useHitPoints = false;
			thingDef.fillPercent = 0f;
			thingDef.description = "Terrain building in progress.";
			thingDef.passability = Traversability.Standable;
			thingDef.selectable = true;
			thingDef.constructEffect = terrDef.constructEffect;
			thingDef.building.isEdifice = false;
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.category = ThingCategory.Ethereal;
			thingDef.entityDefToBuild = terrDef;
			terrDef.frameDef = thingDef;
			if (!thingDef.IsFrame)
			{
				string str = "Framedef is not frame: ";
				ThingDef thingDef2 = thingDef;
				Log.Error(str + ((thingDef2 != null) ? thingDef2.ToString() : null));
			}
			return thingDef;
		}

		// Token: 0x04003645 RID: 13893
		public static readonly string BlueprintDefNamePrefix = "Blueprint_";

		// Token: 0x04003646 RID: 13894
		public static readonly string InstallBlueprintDefNamePrefix = "Install_";

		// Token: 0x04003647 RID: 13895
		public static readonly string BuildingFrameDefNamePrefix = "Frame_";

		// Token: 0x04003648 RID: 13896
		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		// Token: 0x04003649 RID: 13897
		public static readonly Color BlueprintColor = new Color(0.8235294f, 0.92156863f, 1f, 0.6f);
	}
}
