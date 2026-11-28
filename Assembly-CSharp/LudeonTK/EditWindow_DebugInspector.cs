using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Steam;

namespace LudeonTK
{
	// Token: 0x02000054 RID: 84
	public class EditWindow_DebugInspector : EditWindow
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00010704 File Offset: 0x0000E904
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00010715 File Offset: 0x0000E915
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00010718 File Offset: 0x0000E918
		public EditWindow_DebugInspector()
		{
			this.optionalTitle = "Debug inspector";
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00010741 File Offset: 0x0000E941
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (Current.ProgramState == ProgramState.Playing)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00010758 File Offset: 0x0000E958
		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			if (Input.GetMouseButtonDown(1))
			{
				GUIUtility.systemCopyBuffer = this.debugStringBuilder.ToString();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			Text.Font = GameFont.Tiny;
			DevGUI.Label(new Rect(inRect.x, inRect.y, 200f, 20f), "Right click to copy to clipboard");
			Text.Font = GameFont.Small;
			inRect.yMin += 20f;
			float x = inRect.x;
			float y = inRect.y;
			base.DoImageToggle(ref x, y, DevGUI.InspectMode, "Toggle deep inspection mode for things on the map.", ref this.fullMode);
			base.DoImageToggle(ref x, y, DevGUI.InspectMode, "Toggle shallow inspection for things on the map.", ref DebugViewSettings.writeCellContents);
			base.DoRowButton(ref x, y, "Visbility", "Toggle what information should be reported by the inspector.", delegate
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Settings));
			});
			base.DoRowButton(ref x, y, "Column Width +", "Make the columns wider.", delegate
			{
				this.columnWidth += 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			});
			base.DoRowButton(ref x, y, "Column Width -", "Make the columns narrower.", delegate
			{
				this.columnWidth -= 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			});
			inRect.yMin += 30f;
			float num = Mathf.Min(this.columnWidth, inRect.width);
			string[] array = this.debugStringBuilder.ToString().Split('\n', StringSplitOptions.None);
			float num2 = inRect.y;
			float num3 = inRect.x;
			Text.Font = GameFont.Tiny;
			for (int i = 0; i < array.Length; i++)
			{
				float num4 = Text.CalcHeight(array[i], num);
				if (num2 + num4 > inRect.yMax)
				{
					num3 += num;
					num2 = inRect.y;
				}
				DevGUI.Label(new Rect(num3, num2, num, num4), array[i]);
				num2 += num4 - 8f;
			}
			Text.Font = GameFont.Small;
			if (Event.current.type == EventType.repaint)
			{
				this.debugStringBuilder = new StringBuilder();
				this.debugStringBuilder.Append(this.CurrentDebugString());
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0001097B File Offset: 0x0000EB7B
		public void AppendDebugString(string str)
		{
			this.debugStringBuilder.AppendLine(str);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0001098C File Offset: 0x0000EB8C
		private string CurrentDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (DebugViewSettings.writeGame)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine((Current.Game == null) ? "Current.Game = null" : Current.Game.DebugString());
			}
			if (DebugViewSettings.writeMusicManagerPlay)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(Find.MusicManagerPlay.DebugString());
			}
			if (DebugViewSettings.writePlayingSounds)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Sustainers:");
				foreach (Sustainer sustainer in Find.SoundRoot.sustainerManager.AllSustainers)
				{
					stringBuilder.AppendLine(sustainer.DebugString());
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("OneShots:");
				foreach (SampleOneShot sampleOneShot in Find.SoundRoot.oneShotManager.PlayingOneShots)
				{
					stringBuilder.AppendLine(sampleOneShot.ToString());
				}
			}
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Recent sound events:\n       ...");
				stringBuilder.AppendLine(DebugSoundEventsLog.EventsListingDebugString);
			}
			if (DebugViewSettings.writeSteamItems)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(WorkshopItems.DebugOutput());
			}
			if (DebugViewSettings.writeConcepts)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(LessonAutoActivator.DebugString());
			}
			if (DebugViewSettings.writeReservations && Find.CurrentMap != null)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(string.Join("\r\n", (from r in Find.CurrentMap.reservationManager.ReservationsReadOnly
				select r.ToString()).ToArray<string>()));
			}
			if (DebugViewSettings.writeMemoryUsage)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Total allocated: " + Profiler.GetTotalAllocatedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Total reserved: " + Profiler.GetTotalReservedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Total reserved unused: " + Profiler.GetTotalUnusedReservedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Mono heap size: " + Profiler.GetMonoHeapSizeLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Mono used size: " + Profiler.GetMonoUsedSizeLong().ToStringBytes("F2"));
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				stringBuilder.AppendLine("Tick " + Find.TickManager.TicksGame.ToString());
				if (DebugViewSettings.writeStoryteller)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.Storyteller.DebugString());
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null)
			{
				if (DebugViewSettings.writeMapGameConditions)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.gameConditionManager.DebugString());
				}
				if (DebugViewSettings.drawPawnDebug)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.reservationManager.DebugString());
				}
				if (DebugViewSettings.writeMoteSaturation)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Mote count: " + Find.CurrentMap.moteCounter.MoteCount.ToString());
					stringBuilder.AppendLine("Mote saturation: " + Find.CurrentMap.moteCounter.Saturation.ToString());
				}
				if (DebugViewSettings.writeEcosystem)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.wildAnimalSpawner.DebugString());
				}
				if (DebugViewSettings.writeTotalSnowDepth)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Total snow depth: " + Find.CurrentMap.snowGrid.TotalDepth.ToString());
				}
				if (DebugViewSettings.writeWind)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.windManager.DebugString());
				}
				if (DebugViewSettings.writeRecentStrikes)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.mineStrikeManager.DebugStrikeRecords());
				}
				if (DebugViewSettings.writeListRepairableBldgs)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerBuildingsRepairable.DebugString());
				}
				if (DebugViewSettings.writeListFilthInHomeArea)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerFilthInHomeArea.DebugString());
				}
				if (DebugViewSettings.writeListHaulables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerHaulables.DebugString());
				}
				if (DebugViewSettings.writeListMergeables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerMergeables.DebugString());
				}
				if (DebugViewSettings.drawLords)
				{
					foreach (Lord lord in Find.CurrentMap.lordManager.lords)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(lord.DebugString());
					}
				}
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Inspecting ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(intVec);
					defaultInterpolatedStringHandler.AppendLiteral(" (index: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(Find.CurrentMap.cellIndices[intVec]);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
					if (DebugViewSettings.writeTerrain)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.CurrentMap.terrainGrid.DebugStringAt(intVec));
					}
					if (DebugViewSettings.writeAttackTargets)
					{
						foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Potential attack targets for " + pawn.LabelShort + ":");
							List<IAttackTarget> potentialTargetsFor = Find.CurrentMap.attackTargetsCache.GetPotentialTargetsFor(pawn);
							for (int i = 0; i < potentialTargetsFor.Count; i++)
							{
								Thing thing = (Thing)potentialTargetsFor[i];
								StringBuilder stringBuilder3 = stringBuilder;
								string labelShort = thing.LabelShort;
								string str = ", ";
								Faction faction = thing.Faction;
								stringBuilder3.AppendLine(labelShort + str + ((faction != null) ? faction.ToString() : null) + (potentialTargetsFor[i].ThreatDisabled(null) ? " (threat disabled)" : ""));
							}
						}
					}
					if (DebugViewSettings.writeSnowDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Snow depth: " + Find.CurrentMap.snowGrid.GetDepth(intVec).ToString());
					}
					if (DebugViewSettings.writeSandDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Sand depth: " + Find.CurrentMap.sandGrid.GetDepth(intVec).ToString());
					}
					if (DebugViewSettings.drawDeepResources)
					{
						stringBuilder.AppendLine("---");
						StringBuilder stringBuilder4 = stringBuilder;
						string str2 = "Deep resource def: ";
						ThingDef thingDef = Find.CurrentMap.deepResourceGrid.ThingDefAt(intVec);
						stringBuilder4.AppendLine(str2 + ((thingDef != null) ? thingDef.ToString() : null));
						stringBuilder.AppendLine("Deep resource count: " + Find.CurrentMap.deepResourceGrid.CountAt(intVec).ToString());
					}
					if (DebugViewSettings.writeCanReachColony)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("CanReachColony: " + Find.CurrentMap.reachability.CanReachColony(UI.MouseCell()).ToString());
					}
					if (DebugViewSettings.writeMentalStateCalcs)
					{
						stringBuilder.AppendLine("---");
						foreach (Pawn pawn2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							stringBuilder.AppendLine(pawn2.mindState.mentalBreaker.DebugString());
						}
					}
					if (DebugViewSettings.writeWorkSettings)
					{
						foreach (Pawn pawn3 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (pawn3.workSettings != null)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.AppendLine(pawn3.workSettings.DebugString());
							}
						}
					}
					if (DebugViewSettings.writeApparelScore)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Thing thing2 in intVec.GetThingList(Find.CurrentMap))
							{
								Apparel apparel = thing2 as Apparel;
								if (apparel != null)
								{
									stringBuilder.AppendLine(apparel.Label + ": " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.writeCellContents || this.fullMode)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Designation designation in Find.CurrentMap.designationManager.AllDesignationsAt(intVec))
							{
								stringBuilder.AppendLine(designation.ToString());
							}
							foreach (Thing thing3 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
							{
								if (!this.fullMode)
								{
									stringBuilder.AppendLine(thing3.LabelCap + " - " + thing3.ToString());
								}
								else
								{
									stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(thing3));
									stringBuilder.AppendLine();
								}
							}
						}
					}
					if (DebugViewSettings.debugApparelOptimize)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							Apparel apparel2 = thing4 as Apparel;
							if (apparel2 != null)
							{
								stringBuilder.AppendLine(apparel2.LabelCap);
								stringBuilder.AppendLine("   raw: " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel2).ToString("F2"));
								Pawn pawn4 = Find.Selector.SingleSelectedThing as Pawn;
								if (pawn4 != null)
								{
									List<float> list = new List<float>();
									for (int j = 0; j < pawn4.apparel.WornApparel.Count; j++)
									{
										list.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn4, pawn4.apparel.WornApparel[j]));
									}
									StringBuilder stringBuilder5 = stringBuilder;
									string str3 = "  Pawn: ";
									Pawn pawn5 = pawn4;
									stringBuilder5.AppendLine(str3 + ((pawn5 != null) ? pawn5.ToString() : null));
									stringBuilder.AppendLine("  gain: " + JobGiver_OptimizeApparel.ApparelScoreGain(pawn4, apparel2, list).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.drawRegions)
					{
						stringBuilder.AppendLine("---");
						Region regionAt_NoRebuild_InvalidAllowed = Find.CurrentMap.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
						stringBuilder.AppendLine("Region:\n" + ((regionAt_NoRebuild_InvalidAllowed != null) ? regionAt_NoRebuild_InvalidAllowed.DebugString : "null"));
					}
					if (DebugViewSettings.drawDistricts)
					{
						stringBuilder.AppendLine("---");
						District district = intVec.GetDistrict(Find.CurrentMap, RegionType.Set_Passable);
						if (district != null)
						{
							stringBuilder.AppendLine(district.DebugString());
						}
						else
						{
							stringBuilder.AppendLine("(no district)");
						}
					}
					if (DebugViewSettings.drawRooms)
					{
						stringBuilder.AppendLine("---");
						Room room = intVec.GetRoom(Find.CurrentMap);
						if (room != null)
						{
							stringBuilder.AppendLine(room.DebugString());
						}
						else
						{
							stringBuilder.AppendLine("(no room)");
						}
					}
					if (DebugViewSettings.drawGlow)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Game glow: " + Find.CurrentMap.glowGrid.GroundGlowAt(intVec, false, false).ToString());
						stringBuilder.AppendLine("Psych glow: " + Find.CurrentMap.glowGrid.PsychGlowAt(intVec).ToString());
						stringBuilder.AppendLine("Visual Glow: " + Find.CurrentMap.glowGrid.VisualGlowAt(intVec).ToString());
						stringBuilder.AppendLine("GlowReport:\n" + ((SectionLayer_LightingOverlay)Find.CurrentMap.mapDrawer.SectionAt(intVec).GetLayer(typeof(SectionLayer_LightingOverlay))).GlowReportAt(intVec));
						stringBuilder.AppendLine("SkyManager.CurSkyGlow: " + Find.CurrentMap.skyManager.CurSkyGlow.ToString());
					}
					if (DebugViewSettings.writePathCosts)
					{
						stringBuilder.AppendLine("---");
						int num = Find.CurrentMap.pathing.Normal.pathGrid.Cost(intVec);
						int num2 = Find.CurrentMap.pathing.Normal.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid, null);
						stringBuilder.AppendLine("Perceived path cost: " + num.ToString());
						stringBuilder.AppendLine("Real path cost: " + num2.ToString());
						int num3 = Find.CurrentMap.pathing.FenceBlocked.pathGrid.Cost(intVec);
						int num4 = Find.CurrentMap.pathing.FenceBlocked.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid, null);
						if (num3 != num || num4 != num2)
						{
							stringBuilder.AppendLine("Perceived path cost (for fenceblocked): " + num3.ToString());
							stringBuilder.AppendLine("Real path cost (for fenceblocked): " + num4.ToString());
						}
					}
					if (DebugViewSettings.writeFertility)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("\nFertility: " + Find.CurrentMap.fertilityGrid.FertilityAt(intVec).ToString("##0.00"));
					}
					if (DebugViewSettings.writeLinkFlags)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("\nLinkFlags: ");
						foreach (object obj in Enum.GetValues(typeof(LinkFlags)))
						{
							if ((Find.CurrentMap.linkGrid.LinkFlagsAt(intVec) & (LinkFlags)obj) != LinkFlags.None)
							{
								stringBuilder.Append(" " + ((obj != null) ? obj.ToString() : null));
							}
						}
					}
					if (DebugViewSettings.writeSkyManager)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.CurrentMap.skyManager.DebugString());
					}
					if (DebugViewSettings.writeCover)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.Append("Cover: ");
						Thing thing5 = Find.CurrentMap.coverGrid[intVec];
						if (thing5 == null)
						{
							stringBuilder.AppendLine("null");
						}
						else
						{
							stringBuilder.AppendLine(thing5.ToString());
						}
					}
					if (DebugViewSettings.drawPower)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing6 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							ThingWithComps thingWithComps = thing6 as ThingWithComps;
							if (thingWithComps != null && thingWithComps.GetComp<CompPowerTrader>() != null)
							{
								stringBuilder.AppendLine(" " + thingWithComps.GetComp<CompPowerTrader>().DebugString);
							}
						}
						PowerNet powerNet = Find.CurrentMap.powerNetGrid.TransmittedPowerNetAt(intVec);
						if (powerNet != null)
						{
							stringBuilder.AppendLine(powerNet.DebugString() ?? "");
						}
						else
						{
							stringBuilder.AppendLine("(no PowerNet here)");
						}
					}
					if (DebugViewSettings.drawPreyInfo)
					{
						Pawn pawn6 = Find.Selector.SingleSelectedThing as Pawn;
						if (pawn6 != null)
						{
							List<Thing> thingList = intVec.GetThingList(Find.CurrentMap);
							int k = 0;
							while (k < thingList.Count)
							{
								Pawn pawn7 = thingList[k] as Pawn;
								if (pawn7 != null)
								{
									stringBuilder.AppendLine("---");
									if (FoodUtility.IsAcceptablePreyFor(pawn6, pawn7))
									{
										stringBuilder.AppendLine("Prey score: " + FoodUtility.GetPreyScoreFor(pawn6, pawn7).ToString());
										break;
									}
									stringBuilder.AppendLine("Prey score: None");
									break;
								}
								else
								{
									k++;
								}
							}
						}
					}
					if (DebugViewSettings.writeRopesAndPens)
					{
						Building edifice = intVec.GetEdifice(Find.CurrentMap);
						CompAnimalPenMarker compAnimalPenMarker = (edifice != null) ? edifice.TryGetComp<CompAnimalPenMarker>() : null;
						if (compAnimalPenMarker != null)
						{
							stringBuilder.AppendLine("---");
							StringBuilder stringBuilder6 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(14, 2);
							defaultInterpolatedStringHandler2.AppendLiteral("Pen marker ");
							defaultInterpolatedStringHandler2.AppendFormatted<ThingWithComps>(compAnimalPenMarker.parent);
							defaultInterpolatedStringHandler2.AppendLiteral(" - ");
							defaultInterpolatedStringHandler2.AppendFormatted(compAnimalPenMarker.label);
							stringBuilder6.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
							if (compAnimalPenMarker.PenState.Enclosed)
							{
								District district2 = compAnimalPenMarker.parent.GetDistrict(RegionType.Set_Passable);
								float value = new AnimalPenBalanceCalculator(Find.CurrentMap, false).TotalBodySizeIn(district2) / (float)district2.CellCount;
								float value2 = new AnimalPenBalanceCalculator(Find.CurrentMap, true).TotalBodySizeIn(district2) / (float)district2.CellCount;
								StringBuilder stringBuilder7 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(30, 2);
								defaultInterpolatedStringHandler3.AppendLiteral(" animal density: ");
								defaultInterpolatedStringHandler3.AppendFormatted<float>(value);
								defaultInterpolatedStringHandler3.AppendLiteral(" (upcoming: ");
								defaultInterpolatedStringHandler3.AppendFormatted<float>(value2);
								defaultInterpolatedStringHandler3.AppendLiteral(")");
								stringBuilder7.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
							}
						}
						foreach (Pawn pawn8 in intVec.GetThingList(Find.CurrentMap).OfType<Pawn>())
						{
							if (pawn8.roping != null && pawn8.roping.HasAnyRope)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.Append(pawn8.roping.DebugString());
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400012E RID: 302
		private const float CopyTextHeight = 20f;

		// Token: 0x0400012F RID: 303
		private StringBuilder debugStringBuilder = new StringBuilder();

		// Token: 0x04000130 RID: 304
		public bool fullMode;

		// Token: 0x04000131 RID: 305
		private float columnWidth = 360f;
	}
}
