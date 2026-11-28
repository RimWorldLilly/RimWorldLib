using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005BA RID: 1466
	public static class DebugActionsPrefabs
	{
		// Token: 0x06002D03 RID: 11523 RVA: 0x001116D0 File Offset: 0x0010F8D0
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RotatePrefabSpawn()
		{
			DebugActionsPrefabs.Rotation.Rotate(RotationDirection.Clockwise);
			Messages.Message("Prefab rotation: " + DebugActionsPrefabs.Rotation.ToStringHuman(), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x001116FC File Offset: 0x0010F8FC
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CreatePrefab()
		{
			DebugToolsGeneral.GenericRectTool("Create", delegate(CellRect rect)
			{
				PrefabDef prefabDef = PrefabUtility.CreatePrefab(rect, DebugGenerationSettings.prefabCopyAllThings, DebugGenerationSettings.prefabCopyTerrain);
				StringBuilder stringBuilder = new StringBuilder();
				string text = "  ";
				stringBuilder.AppendLine("\n<PrefabDef>");
				stringBuilder.AppendLine(text + "<defName>NewPrefab</defName> <!-- rename -->");
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 3);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("<size>(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(rect.Size.x);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendFormatted<int>(rect.Size.z);
				defaultInterpolatedStringHandler.AppendLiteral(")</size>");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				if (prefabDef.things.CountAllowNull<PrefabThingData>() > 0)
				{
					stringBuilder.AppendLine(text + "<things>");
					for (int i = 0; i < prefabDef.things.Count; i++)
					{
						PrefabThingData prefabThingData = prefabDef.things[i];
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 3);
						defaultInterpolatedStringHandler2.AppendFormatted(text);
						defaultInterpolatedStringHandler2.AppendFormatted(text);
						defaultInterpolatedStringHandler2.AppendLiteral("<");
						defaultInterpolatedStringHandler2.AppendFormatted(prefabThingData.def.defName);
						defaultInterpolatedStringHandler2.AppendLiteral(">");
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
						if (prefabThingData.rects != null)
						{
							stringBuilder.AppendLine(text + text + text + "<rects>");
							foreach (CellRect value in prefabThingData.rects)
							{
								StringBuilder stringBuilder4 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(9, 5);
								defaultInterpolatedStringHandler3.AppendFormatted(text);
								defaultInterpolatedStringHandler3.AppendFormatted(text);
								defaultInterpolatedStringHandler3.AppendFormatted(text);
								defaultInterpolatedStringHandler3.AppendFormatted(text);
								defaultInterpolatedStringHandler3.AppendLiteral("<li>");
								defaultInterpolatedStringHandler3.AppendFormatted<CellRect>(value);
								defaultInterpolatedStringHandler3.AppendLiteral("</li>");
								stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
							}
							stringBuilder.AppendLine(text + text + text + "</rects>");
						}
						else if (prefabThingData.positions != null)
						{
							stringBuilder.AppendLine(text + text + text + "<positions>");
							foreach (IntVec3 value2 in prefabThingData.positions)
							{
								StringBuilder stringBuilder5 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(9, 5);
								defaultInterpolatedStringHandler4.AppendFormatted(text);
								defaultInterpolatedStringHandler4.AppendFormatted(text);
								defaultInterpolatedStringHandler4.AppendFormatted(text);
								defaultInterpolatedStringHandler4.AppendFormatted(text);
								defaultInterpolatedStringHandler4.AppendLiteral("<li>");
								defaultInterpolatedStringHandler4.AppendFormatted<IntVec3>(value2);
								defaultInterpolatedStringHandler4.AppendLiteral("</li>");
								stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
							}
							stringBuilder.AppendLine(text + text + text + "</positions>");
						}
						else
						{
							StringBuilder stringBuilder6 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(21, 4);
							defaultInterpolatedStringHandler5.AppendFormatted(text);
							defaultInterpolatedStringHandler5.AppendFormatted(text);
							defaultInterpolatedStringHandler5.AppendFormatted(text);
							defaultInterpolatedStringHandler5.AppendLiteral("<position>");
							defaultInterpolatedStringHandler5.AppendFormatted<IntVec3>(prefabThingData.position);
							defaultInterpolatedStringHandler5.AppendLiteral("</position>");
							stringBuilder6.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
						}
						if (prefabThingData.relativeRotation != RotationDirection.None)
						{
							StringBuilder stringBuilder7 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(37, 4);
							defaultInterpolatedStringHandler6.AppendFormatted(text);
							defaultInterpolatedStringHandler6.AppendFormatted(text);
							defaultInterpolatedStringHandler6.AppendFormatted(text);
							defaultInterpolatedStringHandler6.AppendLiteral("<relativeRotation>");
							defaultInterpolatedStringHandler6.AppendFormatted(Enum.GetName(typeof(RotationDirection), prefabThingData.relativeRotation));
							defaultInterpolatedStringHandler6.AppendLiteral("</relativeRotation>");
							stringBuilder7.AppendLine(defaultInterpolatedStringHandler6.ToStringAndClear());
						}
						if (prefabThingData.stuff != null)
						{
							StringBuilder stringBuilder8 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(15, 4);
							defaultInterpolatedStringHandler7.AppendFormatted(text);
							defaultInterpolatedStringHandler7.AppendFormatted(text);
							defaultInterpolatedStringHandler7.AppendFormatted(text);
							defaultInterpolatedStringHandler7.AppendLiteral("<stuff>");
							defaultInterpolatedStringHandler7.AppendFormatted(prefabThingData.stuff.defName);
							defaultInterpolatedStringHandler7.AppendLiteral("</stuff>");
							stringBuilder8.AppendLine(defaultInterpolatedStringHandler7.ToStringAndClear());
						}
						if (prefabThingData.quality != null)
						{
							StringBuilder stringBuilder9 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(19, 4);
							defaultInterpolatedStringHandler8.AppendFormatted(text);
							defaultInterpolatedStringHandler8.AppendFormatted(text);
							defaultInterpolatedStringHandler8.AppendFormatted(text);
							defaultInterpolatedStringHandler8.AppendLiteral("<quality>");
							defaultInterpolatedStringHandler8.AppendFormatted<QualityCategory?>(prefabThingData.quality);
							defaultInterpolatedStringHandler8.AppendLiteral("</quality>");
							stringBuilder9.AppendLine(defaultInterpolatedStringHandler8.ToStringAndClear());
						}
						if (prefabThingData.hp != 0)
						{
							StringBuilder stringBuilder10 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(9, 4);
							defaultInterpolatedStringHandler9.AppendFormatted(text);
							defaultInterpolatedStringHandler9.AppendFormatted(text);
							defaultInterpolatedStringHandler9.AppendFormatted(text);
							defaultInterpolatedStringHandler9.AppendLiteral("<hp>");
							defaultInterpolatedStringHandler9.AppendFormatted<int>(prefabThingData.hp);
							defaultInterpolatedStringHandler9.AppendLiteral("</hp>");
							stringBuilder10.AppendLine(defaultInterpolatedStringHandler9.ToStringAndClear());
						}
						if (prefabThingData.stackCountRange != IntRange.One)
						{
							StringBuilder stringBuilder11 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler10 = new DefaultInterpolatedStringHandler(36, 5);
							defaultInterpolatedStringHandler10.AppendFormatted(text);
							defaultInterpolatedStringHandler10.AppendFormatted(text);
							defaultInterpolatedStringHandler10.AppendFormatted(text);
							defaultInterpolatedStringHandler10.AppendLiteral("<stackCountRange>");
							defaultInterpolatedStringHandler10.AppendFormatted<int>(prefabThingData.stackCountRange.min);
							defaultInterpolatedStringHandler10.AppendLiteral("~");
							defaultInterpolatedStringHandler10.AppendFormatted<int>(prefabThingData.stackCountRange.max);
							defaultInterpolatedStringHandler10.AppendLiteral("</stackCountRange>");
							stringBuilder11.AppendLine(defaultInterpolatedStringHandler10.ToStringAndClear());
						}
						if (prefabThingData.colorDef != null)
						{
							StringBuilder stringBuilder12 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler11 = new DefaultInterpolatedStringHandler(21, 4);
							defaultInterpolatedStringHandler11.AppendFormatted(text);
							defaultInterpolatedStringHandler11.AppendFormatted(text);
							defaultInterpolatedStringHandler11.AppendFormatted(text);
							defaultInterpolatedStringHandler11.AppendLiteral("<colorDef>");
							defaultInterpolatedStringHandler11.AppendFormatted<ColorDef>(prefabThingData.colorDef);
							defaultInterpolatedStringHandler11.AppendLiteral("</colorDef>");
							stringBuilder12.AppendLine(defaultInterpolatedStringHandler11.ToStringAndClear());
						}
						if (prefabThingData.color != default(Color))
						{
							StringBuilder stringBuilder13 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler12 = new DefaultInterpolatedStringHandler(15, 4);
							defaultInterpolatedStringHandler12.AppendFormatted(text);
							defaultInterpolatedStringHandler12.AppendFormatted(text);
							defaultInterpolatedStringHandler12.AppendFormatted(text);
							defaultInterpolatedStringHandler12.AppendLiteral("<color>");
							defaultInterpolatedStringHandler12.AppendFormatted<Color>(prefabThingData.color);
							defaultInterpolatedStringHandler12.AppendLiteral("</color>");
							stringBuilder13.AppendLine(defaultInterpolatedStringHandler12.ToStringAndClear());
						}
						StringBuilder stringBuilder14 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler13 = new DefaultInterpolatedStringHandler(3, 3);
						defaultInterpolatedStringHandler13.AppendFormatted(text);
						defaultInterpolatedStringHandler13.AppendFormatted(text);
						defaultInterpolatedStringHandler13.AppendLiteral("</");
						defaultInterpolatedStringHandler13.AppendFormatted(prefabThingData.def.defName);
						defaultInterpolatedStringHandler13.AppendLiteral(">");
						stringBuilder14.AppendLine(defaultInterpolatedStringHandler13.ToStringAndClear());
					}
					stringBuilder.AppendLine(text + "</things>");
				}
				if (prefabDef.terrain.CountAllowNull<PrefabTerrainData>() > 0)
				{
					stringBuilder.AppendLine(text + "<terrain>");
					foreach (PrefabTerrainData prefabTerrainData in prefabDef.terrain)
					{
						StringBuilder stringBuilder15 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler14 = new DefaultInterpolatedStringHandler(2, 3);
						defaultInterpolatedStringHandler14.AppendFormatted(text);
						defaultInterpolatedStringHandler14.AppendFormatted(text);
						defaultInterpolatedStringHandler14.AppendLiteral("<");
						defaultInterpolatedStringHandler14.AppendFormatted(prefabTerrainData.def.defName);
						defaultInterpolatedStringHandler14.AppendLiteral(">");
						stringBuilder15.AppendLine(defaultInterpolatedStringHandler14.ToStringAndClear());
						if (prefabTerrainData.color != null)
						{
							StringBuilder stringBuilder16 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler15 = new DefaultInterpolatedStringHandler(15, 4);
							defaultInterpolatedStringHandler15.AppendFormatted(text);
							defaultInterpolatedStringHandler15.AppendFormatted(text);
							defaultInterpolatedStringHandler15.AppendFormatted(text);
							defaultInterpolatedStringHandler15.AppendLiteral("<color>");
							defaultInterpolatedStringHandler15.AppendFormatted<ColorDef>(prefabTerrainData.color);
							defaultInterpolatedStringHandler15.AppendLiteral("</color>");
							stringBuilder16.AppendLine(defaultInterpolatedStringHandler15.ToStringAndClear());
						}
						stringBuilder.AppendLine(text + text + text + "<rects>");
						foreach (CellRect value3 in prefabTerrainData.rects)
						{
							StringBuilder stringBuilder17 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler16 = new DefaultInterpolatedStringHandler(9, 5);
							defaultInterpolatedStringHandler16.AppendFormatted(text);
							defaultInterpolatedStringHandler16.AppendFormatted(text);
							defaultInterpolatedStringHandler16.AppendFormatted(text);
							defaultInterpolatedStringHandler16.AppendFormatted(text);
							defaultInterpolatedStringHandler16.AppendLiteral("<li>");
							defaultInterpolatedStringHandler16.AppendFormatted<CellRect>(value3);
							defaultInterpolatedStringHandler16.AppendLiteral("</li>");
							stringBuilder17.AppendLine(defaultInterpolatedStringHandler16.ToStringAndClear());
						}
						stringBuilder.AppendLine(text + text + text + "</rects>");
						StringBuilder stringBuilder18 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler17 = new DefaultInterpolatedStringHandler(3, 3);
						defaultInterpolatedStringHandler17.AppendFormatted(text);
						defaultInterpolatedStringHandler17.AppendFormatted(text);
						defaultInterpolatedStringHandler17.AppendLiteral("</");
						defaultInterpolatedStringHandler17.AppendFormatted(prefabTerrainData.def.defName);
						defaultInterpolatedStringHandler17.AppendLiteral(">");
						stringBuilder18.AppendLine(defaultInterpolatedStringHandler17.ToStringAndClear());
					}
					stringBuilder.AppendLine(text + "</terrain>");
				}
				stringBuilder.AppendLine("</PrefabDef>");
				DebugActionsPrefabs.buffer = prefabDef;
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
				Messages.Message("Copied to clipboard", MessageTypeDefOf.NeutralEvent, false);
			}, true);
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x00111728 File Offset: 0x0010F928
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPlayerPrefab()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, false, Faction.OfPlayer);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, false, Faction.OfPlayer);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x001117F4 File Offset: 0x0010F9F4
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPrefab()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, false, null);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, false, null);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x001118C0 File Offset: 0x0010FAC0
		[DebugAction("Generation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnPrefabBlueprint()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("Buffer", DebugActionType.ToolMap, null, null);
			debugActionNode.action = delegate()
			{
				if (DebugActionsPrefabs.buffer != null)
				{
					DebugActionsPrefabs.SpawnAtMouseCell(DebugActionsPrefabs.buffer, true, null);
				}
			};
			list2.Add(debugActionNode);
			using (List<PrefabDef>.Enumerator enumerator = DefDatabase<PrefabDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName ?? "", DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							DebugActionsPrefabs.SpawnAtMouseCell(def, true, null);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x0011198C File Offset: 0x0010FB8C
		private static void SpawnAtMouseCell(PrefabDef def, bool blueprint = false, Faction faction = null)
		{
			IntVec3 intVec = UI.MouseCell();
			CellRect cellRect = GenAdj.OccupiedRect(intVec, DebugActionsPrefabs.Rotation, def.size);
			if (!intVec.InBounds(Find.CurrentMap))
			{
				return;
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				if (c.InBounds(Find.CurrentMap))
				{
					Find.CurrentMap.terrainGrid.SetTerrain(c, TerrainDefOf.MetalTile);
				}
			}
			PrefabUtility.SpawnPrefab(def, Find.CurrentMap, intVec, DebugActionsPrefabs.Rotation, faction, null, null, null, blueprint);
		}

		// Token: 0x04001E2D RID: 7725
		private static Rot4 Rotation = Rot4.North;

		// Token: 0x04001E2E RID: 7726
		private static PrefabDef buffer;
	}
}
