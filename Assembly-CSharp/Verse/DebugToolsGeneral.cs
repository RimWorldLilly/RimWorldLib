using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005C0 RID: 1472
	public static class DebugToolsGeneral
	{
		// Token: 0x06002D31 RID: 11569 RVA: 0x00113488 File Offset: 0x00111688
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Destroy()
		{
			Thing.allowDestroyNonDestroyable = true;
			try
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			finally
			{
				Thing.allowDestroyNonDestroyable = false;
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x00113508 File Offset: 0x00111708
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Kill()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x00113578 File Offset: 0x00111778
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 999)]
		private static void SetFaction()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<Thing> things = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsInViewOrder)
			{
				Faction localFac = localFac2;
				FloatMenuOption item = new FloatMenuOption(localFac.Name, delegate()
				{
					foreach (Thing thing in things)
					{
						if (thing.def.CanHaveFaction)
						{
							thing.SetFaction(localFac, null);
						}
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			list.Add(new FloatMenuOption("None", delegate()
			{
				foreach (Thing thing in things)
				{
					if (thing.def.CanHaveFaction)
					{
						thing.SetFaction(null, null);
					}
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x00113670 File Offset: 0x00111870
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 999)]
		private static void SetFactionRect()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsInViewOrder)
			{
				Faction localFac = localFac2;
				Action<CellRect> <>9__1;
				FloatMenuOption item = new FloatMenuOption(localFac.Name, delegate()
				{
					string name = localFac.Name;
					Action<CellRect> rectAction;
					if ((rectAction = <>9__1) == null)
					{
						rectAction = (<>9__1 = delegate(CellRect rect)
						{
							foreach (IntVec3 c in rect.Cells)
							{
								foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c).ToList<Thing>())
								{
									if (thing.def.CanHaveFaction)
									{
										thing.SetFaction(localFac, null);
									}
								}
							}
						});
					}
					DebugToolsGeneral.GenericRectTool(name, rectAction, false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x00113710 File Offset: 0x00111910
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Teleport()
		{
			Pawn pawn;
			if (!UI.MouseCell().TryGetFirstThing(Find.CurrentMap, out pawn))
			{
				return;
			}
			MoteMaker.ThrowText(pawn.DrawPos, Find.CurrentMap, "Selected " + pawn.Label, -1f);
			DebugTools.curTool = new DebugTool("Destination", delegate()
			{
				IntVec3 intVec = UI.MouseCell();
				pawn.Position = intVec.ClampInsideMap(Find.CurrentMap);
				pawn.Notify_Teleported(true, true);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x00113788 File Offset: 0x00111988
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PickCopy()
		{
			Thing thing;
			if (!UI.MouseCell().TryGetFirstThing(Find.CurrentMap, out thing))
			{
				return;
			}
			MoteMaker.ThrowText(thing.DrawPos, Find.CurrentMap, "Copied " + thing.Label, -1f);
			DebugTools.curTool = new DebugTool("Place", delegate()
			{
				IntVec3 loc = UI.MouseCell();
				Thing thing = GenSpawn.Spawn(thing.def, loc, Find.CurrentMap, thing.Rotation, WipeMode.Vanish);
				CompQuality compQuality;
				CompQuality compQuality2;
				if (thing.TryGetComp(out compQuality) && thing.TryGetComp(out compQuality2))
				{
					compQuality2.SetQuality(compQuality.Quality, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(thing.Faction);
				}
			}, null);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x00113800 File Offset: 0x00111A00
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Discard()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn);
				}
				else
				{
					thing.Discard(false);
				}
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x00113884 File Offset: 0x00111A84
		[DebugAction("General", "10 damage", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take10Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x0011390C File Offset: 0x00111B0C
		[DebugAction("General", "300 damage", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take300Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 300f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x00113994 File Offset: 0x00111B94
		[DebugAction("General", "5000 damage", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 50000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x00113A1C File Offset: 0x00111C1C
		[DebugAction("General", "Clear area (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void ClearArea()
		{
			DebugToolsGeneral.GenericRectTool("Clear", delegate(CellRect rect)
			{
				GenDebug.ClearArea(rect, Find.CurrentMap);
			}, false);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x00113A48 File Offset: 0x00111C48
		[DebugAction("Spawning", "Spawn fill area (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SpawnFillArea()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				Action<CellRect> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
				{
					string label = "Spawn";
					Action<CellRect> rectAction;
					if ((rectAction = <>9__2) == null)
					{
						rectAction = (<>9__2 = delegate(CellRect rect)
						{
							GenDebug.SpawnArea(rect, Find.CurrentMap, localDef);
						});
					}
					DebugToolsGeneral.GenericRectTool(label, rectAction, false);
				}, null));
			}
			return list;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x00113AEC File Offset: 0x00111CEC
		[DebugAction("General", "Make empty room (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void MakeEmptyRoom()
		{
			DebugToolsGeneral.GenericRectTool("Make room", delegate(CellRect rect)
			{
				GenDebug.ClearArea(rect, Find.CurrentMap);
				IEnumerable<IntVec3> edgeCells = rect.EdgeCells;
				IntVec3 invalid = IntVec3.Invalid;
				(from x in edgeCells
				where !rect.IsCorner(x)
				select x).TryRandomElement(out invalid);
				foreach (IntVec3 intVec in edgeCells)
				{
					Thing thing = ThingMaker.MakeThing((intVec == invalid) ? ThingDefOf.Door : ThingDefOf.Wall, ThingDefOf.WoodLog);
					thing.SetFaction(Faction.OfPlayer, null);
					GenPlace.TryPlaceThing(thing, intVec, Find.CurrentMap, ThingPlaceMode.Direct, null, null, null, 1);
				}
				foreach (IntVec3 c in rect)
				{
					Find.CurrentMap.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					Find.CurrentMap.terrainGrid.SetTerrain(c, TerrainDefOf.WoodPlankFloor);
				}
			}, false);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x00113B18 File Offset: 0x00111D18
		[DebugAction("General", "Edit roof (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> MakeRoof()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("Clear", DebugActionType.Action, delegate()
			{
				DebugToolsGeneral.GenericRectTool("Clear roof", delegate(CellRect rect)
				{
					foreach (IntVec3 c in rect)
					{
						Find.CurrentMap.roofGrid.SetRoof(c, null);
					}
				}, false);
			}, null));
			foreach (RoofDef localDef2 in DefDatabase<RoofDef>.AllDefs)
			{
				RoofDef localDef = localDef2;
				Action<CellRect> <>9__3;
				list.Add(new DebugActionNode(localDef.LabelCap, DebugActionType.Action, delegate()
				{
					string label = "Make roof (" + localDef.label + ")";
					Action<CellRect> rectAction;
					if ((rectAction = <>9__3) == null)
					{
						rectAction = (<>9__3 = delegate(CellRect rect)
						{
							foreach (IntVec3 c in rect)
							{
								Find.CurrentMap.roofGrid.SetRoof(c, localDef);
							}
						});
					}
					DebugToolsGeneral.GenericRectTool(label, rectAction, false);
				}, null));
			}
			return list;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x00113BCC File Offset: 0x00111DCC
		[DebugAction("General", "Fog (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void FogRect()
		{
			DebugToolsGeneral.GenericRectTool("Fog", delegate(CellRect rect)
			{
				Find.CurrentMap.fogGrid.Refog(rect.ClipInsideMap(Find.CurrentMap));
			}, false);
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x00113BF8 File Offset: 0x00111DF8
		[DebugAction("General", "Unfog (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void UnfogRect()
		{
			DebugToolsGeneral.GenericRectTool("Clear", delegate(CellRect rect)
			{
				foreach (IntVec3 c in rect.ClipInsideMap(Find.CurrentMap))
				{
					Find.CurrentMap.fogGrid.Unfog(c);
				}
			}, false);
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x00113C24 File Offset: 0x00111E24
		public static void GenericRectTool(string label, Action<CellRect> rectAction, bool closeOnComplete = false)
		{
			DebugTool tool = null;
			IntVec3 firstCorner;
			Action <>9__1;
			tool = new DebugTool(label + ": First corner...", delegate()
			{
				firstCorner = UI.MouseCell();
				string label2 = label + ": Second corner...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						IntVec3 second = UI.MouseCell();
						CellRect obj = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
						rectAction(obj);
						DebugTools.curTool = (closeOnComplete ? null : tool);
					});
				}
				DebugTools.curTool = new DebugTool(label2, clickAction, firstCorner);
			}, null);
			DebugTools.curTool = tool;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x00113C88 File Offset: 0x00111E88
		[DebugAction("General", "Explosion...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> Explosion()
		{
			if (DebugToolsGeneral.explosionDatas.NullOrEmpty<DebugToolsGeneral.ExplosionData>())
			{
				DebugToolsGeneral.explosionDatas = new List<DebugToolsGeneral.ExplosionData>
				{
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Bomb
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Flame
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Stun
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.EMP
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Extinguish,
						thingDef = ThingDefOf.Filth_FireFoam,
						thingChance = 1f,
						thingCount = 3,
						applyToNeighbors = true
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Smoke,
						gasType = new GasType?(GasType.BlindSmoke)
					}
				};
				if (ModsConfig.BiotechActive)
				{
					DebugToolsGeneral.explosionDatas.Add(new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.ToxGas,
						gasType = new GasType?(GasType.ToxGas)
					});
					DebugToolsGeneral.explosionDatas.Add(new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Vaporize
					});
				}
				if (ModsConfig.OdysseyActive)
				{
					DebugToolsGeneral.explosionDatas.Add(new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.MiningBomb
					});
				}
			}
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugToolsGeneral.explosionDatas.Count; i++)
			{
				DebugToolsGeneral.ExplosionData data = DebugToolsGeneral.explosionDatas[i];
				list.Add(new DebugActionNode(data.damageDef.LabelCap, DebugActionType.ToolMap, delegate()
				{
					IntVec3 center = UI.MouseCell();
					Map currentMap = Find.CurrentMap;
					float radius = 4.9f;
					DamageDef damageDef = data.damageDef;
					Thing instigator = null;
					int damAmount = -1;
					float armorPenetration = -1f;
					SoundDef explosionSound = null;
					ThingDef weapon = null;
					ThingDef projectile = null;
					Thing intendedTarget = null;
					ThingDef thingDef = data.thingDef;
					float thingChance = data.thingChance;
					int thingCount = data.thingCount;
					GasType? gasType = data.gasType;
					bool applyToNeighbors = data.applyToNeighbors;
					GenExplosion.DoExplosion(center, currentMap, radius, damageDef, instigator, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, thingDef, thingChance, thingCount, gasType, null, 255, applyToNeighbors, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x00113E5E File Offset: 0x0011205E
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void LightningStrike()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x00113E84 File Offset: 0x00112084
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> LightningStrikeDelayed()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < 41; i++)
			{
				int delay = i * 30;
				list.Add(new DebugActionNode(((float)delay / 60f).ToString("F1") + " seconds", DebugActionType.ToolMap, delegate()
				{
					Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeDelayed(Find.CurrentMap, UI.MouseCell(), delay));
				}, null));
			}
			return list;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x00113EF4 File Offset: 0x001120F4
		[DebugAction("General", "Add gas...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static List<DebugActionNode> PushGas()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (object obj in Enum.GetValues(typeof(GasType)))
			{
				GasType gasType2 = (GasType)obj;
				GasType gasType = gasType2;
				if ((gasType != GasType.ToxGas || ModsConfig.BiotechActive) && (gasType != GasType.DeadlifeDust || ModsConfig.AnomalyActive))
				{
					list.Add(new DebugActionNode(gasType.GetLabel().CapitalizeFirst(), DebugActionType.ToolMap, delegate()
					{
						GasUtility.AddGas(UI.MouseCell(), Find.CurrentMap, gasType, 5f);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x00113FB0 File Offset: 0x001121B0
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static void ClearAllGas()
		{
			Find.CurrentMap.gasGrid.Debug_ClearAll();
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x00113FC1 File Offset: 0x001121C1
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void FillAllGas()
		{
			Find.CurrentMap.gasGrid.Debug_FillAll();
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x00113FD4 File Offset: 0x001121D4
		[DebugAction("General", "Push heat...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static List<DebugActionNode> PushHeat()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugToolsGeneral.HeatPushOptions.Length; i++)
			{
				int t = DebugToolsGeneral.HeatPushOptions[i];
				list.Add(new DebugActionNode(t.ToString(), DebugActionType.ToolMap, delegate()
				{
					Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
					if (room != null && !room.UsesOutdoorTemperature)
					{
						foreach (IntVec3 c in room.Cells)
						{
							GenTemperature.PushHeat(c, Find.CurrentMap, (float)t);
						}
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x00114034 File Offset: 0x00112234
		[DebugAction("General", "Grow plant 1 day", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Grow1Day()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				if (num >= 60000)
				{
					plant.Age += 60000;
				}
				else if (num > 0)
				{
					plant.Age += num;
				}
				plant.Growth += 1f / plant.def.plant.growDays;
				if ((double)plant.Growth > 1.0)
				{
					plant.Growth = 1f;
				}
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x0011410C File Offset: 0x0011230C
		[DebugAction("General", "Grow plant to maturity", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowPlantToMaturity()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				plant.Age += num;
				plant.Growth = 1f;
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x00114188 File Offset: 0x00112388
		[DebugAction("General", "Make plant leafless", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakePlantLeafless()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				plant.MakeLeafless(Plant.LeaflessCause.Cold, false);
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x001141D4 File Offset: 0x001123D4
		[DebugAction("General", "Rotate", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode Rotate()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			debugActionNode.AddChild(new DebugActionNode("Clockwise", DebugActionType.ToolMap, delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Clockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Counter clockwise", DebugActionType.ToolMap, delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Counterclockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null));
			return debugActionNode;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x00114248 File Offset: 0x00112448
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetColor()
		{
			DebugToolsGeneral.<>c__DisplayClass31_0 CS$<>8__locals1 = new DebugToolsGeneral.<>c__DisplayClass31_0();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			CS$<>8__locals1.cell = UI.MouseCell();
			list.Add(new FloatMenuOption("Random", delegate()
			{
				base.<SetColor>g__SetColor_All|0(GenColor.RandomColorOpaque());
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ideo i = enumerator.Current;
					if (!i.classicMode && i.Icon != BaseContent.BadTex)
					{
						list.Add(new FloatMenuOption(i.name, delegate()
						{
							CS$<>8__locals1.<SetColor>g__SetColor_All|0(i.Color);
						}, i.Icon, i.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
				}
			}
			using (IEnumerator<ColorDef> enumerator2 = DefDatabase<ColorDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ColorDef c = enumerator2.Current;
					list.Add(new FloatMenuOption(c.defName, delegate()
					{
						CS$<>8__locals1.<SetColor>g__SetColor_All|0(c.color);
					}, BaseContent.WhiteTex, c.color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x001143FC File Offset: 0x001125FC
		[DebugAction("General", "Rot 1 day", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rot1Day()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRottable compRottable = thing.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					compRottable.RotProgress += 60000f;
				}
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x0011446C File Offset: 0x0011266C
		[DebugAction("General", "Force sleep", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceSleep()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.ToSleep();
				}
				else
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position), JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
					}
				}
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x00114528 File Offset: 0x00112728
		[DebugAction("General", "Break down...", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BreakDown()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
				if (compBreakdownable != null && !compBreakdownable.BrokenDown)
				{
					compBreakdownable.DoBreakdown();
				}
			}
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x00114594 File Offset: 0x00112794
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void TestFloodUnfog()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x001145A8 File Offset: 0x001127A8
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashClosewalkCell30()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x001145E0 File Offset: 0x001127E0
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashWalkPath()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x001145F0 File Offset: 0x001127F0
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashSkygazeCell()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x00114664 File Offset: 0x00112864
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashDirectFleeDest()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				return;
			}
			IntVec3 c;
			if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn, out c))
			{
				Find.CurrentMap.debugDrawer.FlashCell(c, 0.5f, null, 50);
				return;
			}
			Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x001146F4 File Offset: 0x001128F4
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, hideInSubMenu = true)]
		private static void FlashShuttleDropCellsNear()
		{
			IntVec3 center = UI.MouseCell();
			Map currentMap = Find.CurrentMap;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(center, currentMap, out c, false, false, false, new IntVec2?(ThingDefOf.Shuttle.Size + new IntVec2(2, 2)), false);
				currentMap.debugDrawer.FlashCell(c, 0.2f, null, 50);
			}
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x00114758 File Offset: 0x00112958
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashSpectatorsCells()
		{
			Action<bool> act = delegate(bool bestSideOnly)
			{
				DebugTool tool = null;
				IntVec3 firstCorner;
				Action <>9__4;
				tool = new DebugTool("first watch rect corner...", delegate()
				{
					firstCorner = UI.MouseCell();
					string label = "second watch rect corner...";
					Action clickAction;
					if ((clickAction = <>9__4) == null)
					{
						clickAction = (<>9__4 = delegate()
						{
							IntVec3 second = UI.MouseCell();
							CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
							SpectateRectSide allowedSides = SpectateRectSide.All;
							if (bestSideOnly)
							{
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1, null);
							}
							SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
							DebugTools.curTool = tool;
						});
					}
					DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
				}, null);
				DebugTools.curTool = tool;
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			list.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x001147E4 File Offset: 0x001129E4
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> CheckReachability()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			TraverseMode[] array = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
			for (int i = 0; i < array.Length; i++)
			{
				TraverseMode traverseMode2 = array[i];
				TraverseMode traverseMode = traverseMode2;
				list.Add(new DebugActionNode(traverseMode2.ToString(), DebugActionType.Action, delegate()
				{
					DebugTool tool = null;
					IntVec3 from;
					Pawn fromPawn;
					Action <>9__2;
					Action <>9__3;
					tool = new DebugTool("from...", delegate()
					{
						from = UI.MouseCell();
						fromPawn = from.GetFirstPawn(Find.CurrentMap);
						string text = "to...";
						if (fromPawn != null)
						{
							text = text + " (pawn=" + fromPawn.LabelShort + ")";
						}
						string label = text;
						Action clickAction;
						if ((clickAction = <>9__2) == null)
						{
							clickAction = (<>9__2 = delegate()
							{
								DebugTools.curTool = tool;
							});
						}
						Action onGUIAction;
						if ((onGUIAction = <>9__3) == null)
						{
							onGUIAction = (<>9__3 = delegate()
							{
								IntVec3 c = UI.MouseCell();
								bool flag;
								IntVec3 intVec;
								if (fromPawn != null)
								{
									flag = fromPawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, traverseMode);
									intVec = fromPawn.Position;
								}
								else
								{
									flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, traverseMode, Danger.Deadly);
									intVec = from;
								}
								Color color = flag ? Color.green : Color.red;
								Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
							});
						}
						DebugTools.curTool = new DebugTool(label, clickAction, onGUIAction);
					}, null);
					DebugTools.curTool = tool;
				}, null));
			}
			return list;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x00114858 File Offset: 0x00112A58
		[DebugAction("General", "Flash TryFindRandomPawnExitCell", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashTryFindRandomPawnExitCell(Pawn p)
		{
			IntVec3 intVec;
			if (CellFinder.TryFindRandomPawnExitCell(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x001148C8 File Offset: 0x00112AC8
		[DebugAction("General", "RandomSpotJustOutsideColony", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void RandomSpotJustOutsideColony(Pawn p)
		{
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x00114938 File Offset: 0x00112B38
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void RandomSpotNearThingAvoidingHostiles()
		{
			List<Thing> list = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (list.Count == 0)
			{
				return;
			}
			Thing thing = (from t in list
			where t is Pawn && t.Faction != null
			select t).FirstOrDefault<Thing>();
			if (thing == null)
			{
				thing = list.First<Thing>();
			}
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, thing.Map, (IntVec3 s) => true, out intVec, 100f, 10f, 50f, true))
			{
				thing.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				thing.Map.debugDrawer.FlashLine(thing.Position, intVec, 50, SimpleColor.White);
				return;
			}
			thing.Map.debugDrawer.FlashCell(thing.Position, 0.2f, "no cell", 50);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x00114A32 File Offset: 0x00112C32
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllFog()
		{
			Find.CurrentMap.fogGrid.ClearAllFog();
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x00114A44 File Offset: 0x00112C44
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeThingStyle()
		{
			Thing thing = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.def.CanBeStyled());
			if (thing == null)
			{
				return;
			}
			DebugToolsGeneral.tmpStyleDefs.Clear();
			if (!thing.def.randomStyle.NullOrEmpty<ThingStyleChance>())
			{
				foreach (ThingStyleChance thingStyleChance in thing.def.randomStyle)
				{
					if (thingStyleChance.StyleDef.graphicData != null)
					{
						DebugToolsGeneral.tmpStyleDefs.Add(thingStyleChance.StyleDef);
					}
				}
			}
			IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
			Func<StyleCategoryDef, bool> <>9__1;
			Func<StyleCategoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				Predicate<ThingDefStyle> <>9__2;
				predicate = (<>9__1 = delegate(StyleCategoryDef x)
				{
					List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
					Predicate<ThingDefStyle> predicate2;
					if ((predicate2 = <>9__2) == null)
					{
						predicate2 = (<>9__2 = ((ThingDefStyle y) => y.ThingDef == thing.def));
					}
					return thingDefStyles.Any(predicate2);
				});
			}
			foreach (StyleCategoryDef styleCategoryDef in allDefs.Where(predicate))
			{
				DebugToolsGeneral.tmpStyleDefs.Add(styleCategoryDef.GetStyleForThingDef(thing.def, null));
			}
			if (DebugToolsGeneral.tmpStyleDefs.Any<ThingStyleDef>())
			{
				DebugToolsGeneral.<>c__DisplayClass48_1 CS$<>8__locals2;
				CS$<>8__locals2.opts = new List<DebugMenuOption>();
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|48_3(thing, () => null, "Standard", ref CS$<>8__locals2);
				Func<ThingStyleDef, float> <>9__7;
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|48_3(thing, delegate
				{
					IEnumerable<ThingStyleDef> source = DebugToolsGeneral.tmpStyleDefs;
					Func<ThingStyleDef, float> weightSelector;
					if ((weightSelector = <>9__7) == null)
					{
						weightSelector = (<>9__7 = delegate(ThingStyleDef x)
						{
							if (x != thing.StyleDef)
							{
								return 1f;
							}
							return 0.01f;
						});
					}
					return source.RandomElementByWeight(weightSelector);
				}, "Random", ref CS$<>8__locals2);
				using (List<ThingStyleDef>.Enumerator enumerator3 = DebugToolsGeneral.tmpStyleDefs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ThingStyleDef s = enumerator3.Current;
						DebugToolsGeneral.<ChangeThingStyle>g__AddOption|48_3(thing, () => s, s.defName, ref CS$<>8__locals2);
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals2.opts, null));
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x00114C8C File Offset: 0x00112E8C
		[DebugAction("General", "Destroy fire", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllFire()
		{
			Thing[] array = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Fire).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x00114CEC File Offset: 0x00112EEC
		[CompilerGenerated]
		internal static void <ChangeThingStyle>g__AddOption|48_3(Thing t, Func<ThingStyleDef> styleSelector, string label, ref DebugToolsGeneral.<>c__DisplayClass48_1 A_3)
		{
			A_3.opts.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
			{
				t.StyleDef = styleSelector();
				t.DirtyMapMesh(t.Map);
			}));
		}

		// Token: 0x04001E33 RID: 7731
		private static List<DebugToolsGeneral.ExplosionData> explosionDatas;

		// Token: 0x04001E34 RID: 7732
		private static readonly int[] HeatPushOptions = new int[]
		{
			10,
			50,
			100,
			1000,
			-10,
			-50,
			-1000
		};

		// Token: 0x04001E35 RID: 7733
		private static List<ThingStyleDef> tmpStyleDefs = new List<ThingStyleDef>();

		// Token: 0x02002841 RID: 10305
		private struct ExplosionData
		{
			// Token: 0x0400A398 RID: 41880
			public DamageDef damageDef;

			// Token: 0x0400A399 RID: 41881
			public GasType? gasType;

			// Token: 0x0400A39A RID: 41882
			public ThingDef thingDef;

			// Token: 0x0400A39B RID: 41883
			public float thingChance;

			// Token: 0x0400A39C RID: 41884
			public int thingCount;

			// Token: 0x0400A39D RID: 41885
			public bool applyToNeighbors;
		}
	}
}
