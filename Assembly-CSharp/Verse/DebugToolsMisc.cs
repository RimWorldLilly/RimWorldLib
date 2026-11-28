using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005C1 RID: 1473
	public static class DebugToolsMisc
	{
		// Token: 0x06002D61 RID: 11617 RVA: 0x00114D2C File Offset: 0x00112F2C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AttachFire()
		{
			using (List<Thing>.Enumerator enumerator = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing t = enumerator.Current;
					if (t.CanEverAttachFire())
					{
						t.TryAttachFire(1f, null);
					}
					else
					{
						FireUtility.TryStartFireIn(UI.MouseCell(), Find.CurrentMap, 1.75f, null, null);
					}
				}
			}
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x00114DBC File Offset: 0x00112FBC
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SetStuff()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			using (IEnumerator<ThingDef> enumerator = (from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef stuff = enumerator.Current;
					ThingDef stuffInner = stuff;
					debugActionNode.AddChild(new DebugActionNode(stuff.LabelCap, DebugActionType.ToolMap, delegate()
					{
						foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
						{
							if (thing.Stuff != null)
							{
								float num = (float)thing.HitPoints / (float)thing.MaxHitPoints;
								thing.SetStuffDirect(stuffInner);
								StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(thing);
								thing.HitPoints = Mathf.CeilToInt((float)thing.MaxHitPoints * num);
								thing.Notify_ColorChanged();
								thing.DirtyMapMesh(thing.Map);
								UnfinishedThing unfinishedThing = thing as UnfinishedThing;
								if (unfinishedThing != null)
								{
									int num2 = 0;
									int num3 = 100;
									foreach (Thing thing2 in unfinishedThing.ingredients)
									{
										num2 += thing2.stackCount;
									}
									unfinishedThing.ingredients.Clear();
									while (num2 > 0 && num3-- > 0)
									{
										Thing thing3 = ThingMaker.MakeThing(stuff, null);
										thing3.stackCount = Mathf.Min(num2, stuff.stackLimit);
										unfinishedThing.ingredients.Add(thing3);
										num2 -= thing3.stackCount;
									}
								}
							}
						}
					}, null));
				}
			}
			return debugActionNode;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x00114E6C File Offset: 0x0011306C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SetQuality()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			foreach (object obj in Enum.GetValues(typeof(QualityCategory)))
			{
				QualityCategory qualityInner2 = (QualityCategory)obj;
				QualityCategory qualityInner = qualityInner2;
				debugActionNode.AddChild(new DebugActionNode(qualityInner.ToString(), DebugActionType.ToolMap, delegate()
				{
					foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap))
					{
						CompQuality compQuality = thing.TryGetComp<CompQuality>();
						if (compQuality != null)
						{
							compQuality.SetQuality(qualityInner, new ArtGenerationContext?(ArtGenerationContext.Outsider));
						}
					}
				}, null));
			}
			return debugActionNode;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x00114F08 File Offset: 0x00113108
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void MeasureDrawSize()
		{
			Vector3 first = Vector3.zero;
			Action <>9__1;
			DebugTools.curMeasureTool = new DrawMeasureTool("first corner...", delegate()
			{
				first = UI.MouseMapPosition();
				string label = "second corner...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						Vector3 vector = UI.MouseMapPosition();
						Rect rect = default(Rect);
						rect.xMin = Mathf.Min(first.x, vector.x);
						rect.yMin = Mathf.Min(first.z, vector.z);
						rect.xMax = Mathf.Max(first.x, vector.x);
						rect.yMax = Mathf.Max(first.z, vector.z);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Center: (");
						defaultInterpolatedStringHandler.AppendFormatted<float>(rect.center.x);
						defaultInterpolatedStringHandler.AppendLiteral(",");
						defaultInterpolatedStringHandler.AppendFormatted<float>(rect.center.y);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						string text = defaultInterpolatedStringHandler.ToStringAndClear();
						string str = text;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(10, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("\nSize: (");
						defaultInterpolatedStringHandler2.AppendFormatted<float>(rect.size.x);
						defaultInterpolatedStringHandler2.AppendLiteral(",");
						defaultInterpolatedStringHandler2.AppendFormatted<float>(rect.size.y);
						defaultInterpolatedStringHandler2.AppendLiteral(")");
						text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
						if (Find.Selector.SingleSelectedObject != null)
						{
							Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
							Vector3 drawPos = singleSelectedThing.DrawPos;
							Vector2 vector2 = rect.center - new Vector2(drawPos.x, drawPos.z);
							string str2 = text;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(12, 2);
							defaultInterpolatedStringHandler3.AppendLiteral("\nOffset: (");
							defaultInterpolatedStringHandler3.AppendFormatted<float>(vector2.x);
							defaultInterpolatedStringHandler3.AppendLiteral(",");
							defaultInterpolatedStringHandler3.AppendFormatted<float>(vector2.y);
							defaultInterpolatedStringHandler3.AppendLiteral(")");
							text = str2 + defaultInterpolatedStringHandler3.ToStringAndClear();
							Vector2 vector3 = vector2.RotatedBy(-singleSelectedThing.Rotation.AsAngle);
							string str3 = text;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(22, 2);
							defaultInterpolatedStringHandler4.AppendLiteral("\nUnrotated offset: (");
							defaultInterpolatedStringHandler4.AppendFormatted<float>(vector3.x);
							defaultInterpolatedStringHandler4.AppendLiteral(",");
							defaultInterpolatedStringHandler4.AppendFormatted<float>(vector3.y);
							defaultInterpolatedStringHandler4.AppendLiteral(")");
							text = str3 + defaultInterpolatedStringHandler4.ToStringAndClear();
						}
						Log.Message(text);
						DebugToolsMisc.MeasureDrawSize();
					});
				}
				DebugTools.curMeasureTool = new DrawMeasureTool(label, clickAction, first);
			}, null);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x00114F44 File Offset: 0x00113144
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void MeasureWorldDistance()
		{
			Vector3 first = Vector3.zero;
			Action <>9__1;
			DebugTools.curMeasureTool = new MeasureWorldDistanceTool("First Point...", delegate()
			{
				first = UI.MouseMapPosition();
				string label = "Second Point...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						Vector3 value = UI.MouseMapPosition() - first;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Vector: ");
						defaultInterpolatedStringHandler.AppendFormatted<Vector3>(value);
						defaultInterpolatedStringHandler.AppendLiteral(", Distance:");
						defaultInterpolatedStringHandler.AppendFormatted<float>(value.magnitude);
						Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
						DebugToolsMisc.MeasureWorldDistance();
					});
				}
				DebugTools.curMeasureTool = new MeasureWorldDistanceTool(label, clickAction, first);
			}, null);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x00114F80 File Offset: 0x00113180
		[DebugAction("General", "Draw Attach Points", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void DrawAttachPoints()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				ThingWithComps thingWithComps = thing as ThingWithComps;
				if (thingWithComps != null)
				{
					CompAttachPoints comp = thingWithComps.GetComp<CompAttachPoints>();
					if (comp != null)
					{
						int ttl = 500;
						DebugTools.curTool = new DebugTool("attach point drawer", delegate()
						{
						}, delegate()
						{
							int ttl = ttl;
							ttl--;
							if (ttl <= 0)
							{
								DebugTools.curTool = null;
							}
							foreach (AttachPointType type in comp.points.PointTypes(0, int.MaxValue))
							{
								Vector3 worldPos = comp.points.GetWorldPos(type);
								GenMapUI.DrawText(new Vector2(worldPos.x, worldPos.z), type.ToString(), Color.red);
								GenDraw.DrawCircleOutline(worldPos, 0.4f);
							}
						});
					}
				}
			}
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x00115048 File Offset: 0x00113248
		[DebugAction("General", "Pollution +1%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionSmall()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, 0.01f);
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x00115070 File Offset: 0x00113270
		[DebugAction("General", "Pollution +25%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionLarge()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, 0.25f);
			}
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x00115098 File Offset: 0x00113298
		[DebugAction("General", "Pollution -25%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void DecreasePollutionLarge()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, -0.25f);
			}
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x001150C0 File Offset: 0x001132C0
		[DebugAction("General", "Set biome", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> SetBiome()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (BiomeDef biomeDef in DefDatabase<BiomeDef>.AllDefsListForReading)
			{
				BiomeDef local = biomeDef;
				if (biomeDef.implemented && biomeDef.generatesNaturally)
				{
					list.Add(new DebugActionNode(biomeDef.label, DebugActionType.ToolWorld, delegate()
					{
						PlanetTile planetTile = GenWorld.MouseTile(false);
						if (!planetTile.Valid || planetTile.Tile.PrimaryBiome == local)
						{
							return;
						}
						planetTile.Tile.PrimaryBiome = local;
						Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(PlanetLayer.Selected).RegenerateNow();
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x00115150 File Offset: 0x00113350
		[DebugAction("General", "Set landmark", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresOdyssey = true)]
		private static List<DebugActionNode> SetLandmark()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LandmarkDef landmarkDef in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef local = landmarkDef;
				list.Add(new DebugActionNode(landmarkDef.defName, DebugActionType.ToolWorld, delegate()
				{
					PlanetTile tile = GenWorld.MouseTile(false);
					if (!tile.Valid || !local.IsValidTile(tile, tile.Layer, false))
					{
						return;
					}
					Find.World.landmarks.AddLandmark(local, tile, null, false);
					Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(tile.Layer).RegenerateNow();
					Find.World.renderer.GetLayer<WorldDrawLayer_Landmarks>(Find.WorldGrid.Surface).RegenerateNow();
					Find.World.renderer.GetLayer<WorldDrawLayer_Hills>(Find.WorldGrid.Surface).RegenerateNow();
				}, null));
			}
			return list;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x001151D0 File Offset: 0x001133D0
		[DebugAction("General", "Clear Landmark", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresOdyssey = true)]
		private static void ClearLandmark()
		{
			PlanetTile tile = GenWorld.MouseTile(false);
			if (tile.Valid)
			{
				Find.World.landmarks.RemoveLandmark(tile);
				Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(tile.Layer).RegenerateNow();
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x00115218 File Offset: 0x00113418
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupCooldown()
		{
			Find.BossgroupManager.lastBossgroupCalled = Find.TickManager.TicksGame - 120000;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x00115234 File Offset: 0x00113434
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupKilledPawns()
		{
			Find.BossgroupManager.DebugResetDefeatedPawns();
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x00115240 File Offset: 0x00113440
		[DebugAction("Insect", "Spawn cocoon infestation", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, hideInSubMenu = true, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnCocoonInfestationWithPoints()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(false))
			{
				float localP = localP2;
				DebugActionNode item = new DebugActionNode(localP.ToString() + " points", DebugActionType.ToolMap, delegate()
				{
					CocoonInfestationUtility.SpawnCocoonInfestation(UI.MouseCell(), Find.CurrentMap, localP);
				}, null);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x001152CC File Offset: 0x001134CC
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnPitGate()
		{
			IntVec3 loc = UI.MouseMapPosition().ToIntVec3();
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.PitGateSpawner, null), loc, Find.CurrentMap, WipeMode.Vanish);
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x001152FC File Offset: 0x001134FC
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnPitBurrow()
		{
			FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(UI.MouseMapPosition().ToIntVec3(), Find.CurrentMap, 200f, new IntRange(600, 600), new IntRange(60, 180), true);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x00115334 File Offset: 0x00113534
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnFleshmassHeart()
		{
			IntVec3 loc = UI.MouseMapPosition().ToIntVec3();
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FleshmassHeartSpawner, null), loc, Find.CurrentMap, WipeMode.Vanish);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x00115364 File Offset: 0x00113564
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void DiscoverAllEntities()
		{
			Find.EntityCodex.Debug_DiscoverAll();
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x00115370 File Offset: 0x00113570
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action)]
		private static void BenchmarkPerformance()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Running benchmark, results displayed in ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(30f);
			defaultInterpolatedStringHandler.AppendLiteral(" seconds");
			Messages.Message(defaultInterpolatedStringHandler.ToStringAndClear(), MessageTypeDefOf.NeutralEvent, false);
			PerformanceBenchmarkUtility.StartBenchmark();
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x001153C4 File Offset: 0x001135C4
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CompareLineOfSight(Pawn pawn)
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 50f, true))
			{
				if (c.InBounds(Find.CurrentMap))
				{
					Pawn firstPawn = c.GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						bool flag = pawn.CanSee(firstPawn, null);
						bool flag2 = firstPawn.CanSee(pawn, null);
						if (flag != flag2)
						{
							Find.CurrentMap.debugDrawer.FlashCell(c, 1f, null, 50);
						}
					}
				}
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0011545C File Offset: 0x0011365C
		[DebugAction("General", "Enable wound debug draw", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void WoundDebug()
		{
			IntVec3 c = UI.MouseCell();
			Pawn pawn = c.GetFirstPawn(Find.CurrentMap);
			if (pawn == null || pawn.def.race == null || pawn.def.race.body == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All", DebugMenuOptionMode.Action, delegate()
			{
				pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
				pawn.Drawer.renderer.WoundOverlays.ClearCache();
				PortraitsCache.SetDirty(pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
			}));
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord part = allParts[i];
				list.Add(new DebugMenuOption(part.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					pawn.Drawer.renderer.WoundOverlays.debugDrawPart = part;
					pawn.Drawer.renderer.WoundOverlays.ClearCache();
					PortraitsCache.SetDirty(pawn);
					GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x00115560 File Offset: 0x00113760
		[DebugAction("General", "Wound debug export (non-humanlike)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void WoundDebugExport()
		{
			string text = Application.dataPath + "\\woundDump";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			HashSet<RaceProperties> hashSet = new HashSet<RaceProperties>();
			foreach (PawnKindDef pawnKindDef in from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
			where !pkd.RaceProps.Humanlike
			select pkd)
			{
				if (!hashSet.Contains(pawnKindDef.RaceProps))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null, null);
					for (int i = 0; i < 4; i++)
					{
						Rot4 rot = new Rot4((byte)i);
						RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 32, RenderTextureFormat.ARGB32);
						temporary.name = "WoundDebugExport";
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
						pawn.Drawer.renderer.WoundOverlays.ClearCache();
						Find.PawnCacheRenderer.RenderPawn(pawn, temporary, Vector3.zero, 1f, 0f, rot, true, true, true, false, default(Vector3), null, null, false);
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = false;
						pawn.Drawer.renderer.WoundOverlays.ClearCache();
						Texture2D texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.ARGB32, 0, false);
						RenderTexture.active = temporary;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0, true);
						RenderTexture.active = null;
						RenderTexture.ReleaseTemporary(temporary);
						string str = text + "\\" + pawn.def.LabelCap + "_";
						Rot4 rot2 = rot;
						File.WriteAllBytes(str + rot2.ToString() + ".png", texture2D.EncodeToPNG());
					}
					pawn.Destroy(DestroyMode.Vanish);
					hashSet.Add(pawnKindDef.RaceProps);
				}
			}
			Log.Message("Dumped to " + text);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x001157C4 File Offset: 0x001139C4
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static List<DebugActionNode> EmergeMetalhorrors()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= 10; i++)
			{
				int delaySeconds = i;
				List<DebugActionNode> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("s delay");
				list2.Add(new DebugActionNode(defaultInterpolatedStringHandler.ToStringAndClear(), DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						int delayTicks = delaySeconds * 60;
						DelayedMetalhorrorEmerger.Spawn(Find.CurrentMap, delayTicks);
					}
				});
			}
			return list;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x00115836 File Offset: 0x00113A36
		[DebugAction("Mechanoid", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresOdyssey = true)]
		private static void DisableMechs()
		{
			if (Faction.OfMechanoids == null)
			{
				return;
			}
			Faction.OfMechanoids.deactivated = true;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x0011584C File Offset: 0x00113A4C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void AddTraitToUniqueWeapon()
		{
			CompUniqueWeapon comp = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.HasComp<CompUniqueWeapon>()).TryGetComp<CompUniqueWeapon>();
			if (comp == null)
			{
				return;
			}
			DebugToolsMisc.tmpTraitDefs.Clear();
			DebugToolsMisc.tmpTraitDefs.AddRange(from trait in DefDatabase<WeaponTraitDef>.AllDefs
			where comp.CanAddTrait(trait)
			select trait);
			if (!DebugToolsMisc.tmpTraitDefs.Any<WeaponTraitDef>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeaponTraitDef weaponTraitDef in DebugToolsMisc.tmpTraitDefs)
			{
				WeaponTraitDef localTrait = weaponTraitDef;
				list.Add(new DebugMenuOption(weaponTraitDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					comp.AddTrait(localTrait);
					comp.Setup(true);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x00115974 File Offset: 0x00113B74
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void RemoveTraitFromUniqueWeapon()
		{
			CompUniqueWeapon comp = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.HasComp<CompUniqueWeapon>()).TryGetComp<CompUniqueWeapon>();
			if (comp == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeaponTraitDef weaponTraitDef in comp.TraitsListForReading)
			{
				WeaponTraitDef localTrait = weaponTraitDef;
				list.Add(new DebugMenuOption(weaponTraitDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					comp.TraitsListForReading.Remove(localTrait);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x04001E36 RID: 7734
		private static List<WeaponTraitDef> tmpTraitDefs = new List<WeaponTraitDef>();
	}
}
