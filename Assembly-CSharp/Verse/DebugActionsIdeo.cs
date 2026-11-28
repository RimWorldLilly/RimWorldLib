using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005B5 RID: 1461
	public static class DebugActionsIdeo
	{
		// Token: 0x06002C60 RID: 11360 RVA: 0x0010C7A8 File Offset: 0x0010A9A8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void SetIdeo()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				list.Add(new DebugMenuOption(ideo.name, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in UI.MouseCell().GetThingList(Find.CurrentMap).OfType<Pawn>().ToList<Pawn>())
					{
						if (!pawn.RaceProps.Humanlike)
						{
							break;
						}
						pawn.ideo.SetIdeo(ideo);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x0010C820 File Offset: 0x0010AA20
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, hideInSubMenu = true)]
		private static void ConvertToIdeo()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				list.Add(new DebugMenuOption(ideo.name, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in UI.MouseCell().GetThingList(Find.CurrentMap).OfType<Pawn>().ToList<Pawn>())
					{
						if (!pawn.RaceProps.Humanlike || pawn.Ideo == ideo)
						{
							break;
						}
						pawn.ideo.IdeoConversionAttempt(1f, ideo, false);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x0010C898 File Offset: 0x0010AA98
		[DebugAction("Ideoligion", "Set ideo role...", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void SetIdeoRole(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (p.Ideo != null)
			{
				Precept_Role currentRole = p.Ideo.GetRole(p);
				using (List<Precept_Role>.Enumerator enumerator = p.Ideo.cachedPossibleRoles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Role role = enumerator.Current;
						if (role != currentRole)
						{
							list.Add(new DebugMenuOption(role.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								role.Assign(p, true);
							}));
						}
					}
				}
				if (currentRole != null)
				{
					list.Add(new DebugMenuOption("None", DebugMenuOptionMode.Action, delegate()
					{
						currentRole.Assign(null, true);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x0010C9AC File Offset: 0x0010ABAC
		[DebugAction("Ideoligion", "Certainty - 20%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, requiresIdeology = true, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetCertaintyNegative20(Pawn p)
		{
			if (p.ideo != null)
			{
				p.ideo.Debug_ReduceCertainty(0.2f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x0010C9CC File Offset: 0x0010ABCC
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		public static void SpawnRelic()
		{
			IntVec3 cell = UI.MouseCell();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_Relic relicPrecept = precept as Precept_Relic;
				if (relicPrecept != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						GenSpawn.Spawn(relicPrecept.GenerateRelic(), cell, Find.CurrentMap, WipeMode.Vanish);
					}));
				}
			}
			if (list.Any<DebugMenuOption>())
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
			}
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x0010CA80 File Offset: 0x0010AC80
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		public static void SetSourcePrecept()
		{
			List<Thing> things = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (things.NullOrEmpty<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_ThingStyle stylePrecept = precept as Precept_ThingStyle;
				if (stylePrecept != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						foreach (Thing thing in things)
						{
							thing.StyleSourcePrecept = stylePrecept;
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x0010CB4C File Offset: 0x0010AD4C
		[DebugAction("Ideoligion", "Remove ritual obligation", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void RemoveRitualObligation()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual = precept as Precept_Ritual;
					if (ritual != null && !ritual.activeObligations.NullOrEmpty<RitualObligation>())
					{
						using (List<RitualObligation>.Enumerator enumerator3 = ritual.activeObligations.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								RitualObligation obligation = enumerator3.Current;
								string text = ritual.LabelCap;
								string text2 = ritual.obligationTargetFilter.LabelExtraPart(obligation);
								if (text2.NullOrEmpty())
								{
									text = text + " " + text2;
								}
								list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									ritual.RemoveObligation(obligation, false);
								}));
							}
						}
					}
				}
			}
			if (list.Any<DebugMenuOption>())
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
				return;
			}
			Messages.Message("No obligations to remove.", LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x0010CD34 File Offset: 0x0010AF34
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static List<DebugActionNode> AddPrecept()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PreceptDef localDef2 in DefDatabase<PreceptDef>.AllDefs)
			{
				PreceptDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.issue.LabelCap + ": " + localDef.LabelCap, DebugActionType.Action, delegate()
				{
					Faction.OfPlayer.ideos.PrimaryIdeo.AddPrecept(PreceptMaker.MakePrecept(localDef), true, null, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x0010CDD8 File Offset: 0x0010AFD8
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresIdeology = true)]
		private static void RemovePrecept()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<Precept>.Enumerator enumerator = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept precept = enumerator.Current;
					list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						Faction.OfPlayer.ideos.PrimaryIdeo.RemovePrecept(precept, false);
					}));
				}
			}
			using (List<Ideo>.Enumerator enumerator2 = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Ideo ideo = enumerator2.Current;
					using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Precept precept = enumerator.Current;
							list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								ideo.RemovePrecept(precept, false);
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x0010CFA8 File Offset: 0x0010B1A8
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void TriggerDateRitual()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual = precept as Precept_Ritual;
					if (ritual != null && ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
					{
						string text = ritual.LabelCap;
						if (!ideo.ObligationsActive && !precept.def.allowOptionalRitualObligations)
						{
							text += "[NO]";
						}
						list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
						{
							ritual.AddObligation(new RitualObligation(ritual, true));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x0010D0DC File Offset: 0x0010B2DC
		[DebugAction("Ideoligion", "Add 5 days to obligation timer", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void Add5DaysToObligationTimer()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null && !precept_Ritual.activeObligations.NullOrEmpty<RitualObligation>())
					{
						using (List<RitualObligation>.Enumerator enumerator3 = precept_Ritual.activeObligations.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								RitualObligation obligation = enumerator3.Current;
								string text = precept_Ritual.LabelCap;
								string text2 = precept_Ritual.obligationTargetFilter.LabelExtraPart(obligation);
								if (text2.NullOrEmpty())
								{
									text = text + " " + text2;
								}
								list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									obligation.DebugOffsetTriggeredTick(-300000);
								}));
							}
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x0010D23C File Offset: 0x0010B43C
		[DebugAction("Pawns", "Suppression +10%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void SuppressionPlus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression;
				p.needs.TryGetNeed<Need_Suppression>(out need_Suppression);
				need_Suppression.CurLevel += need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x0010D288 File Offset: 0x0010B488
		[DebugAction("Pawns", "Suppression -10%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void SuppressionMinus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression;
				p.needs.TryGetNeed<Need_Suppression>(out need_Suppression);
				need_Suppression.CurLevel -= need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x0010D2D2 File Offset: 0x0010B4D2
		[DebugAction("Pawns", "Clear suppression schedule", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, hideInSubMenu = true)]
		private static void ResetSuppresionSchedule(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				p.mindState.lastSlaveSuppressedTick = -99999;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x0010D2FC File Offset: 0x0010B4FC
		[DebugAction("Pawns", "Will +1", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void WillPlus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will + 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x0010D350 File Offset: 0x0010B550
		[DebugAction("Pawns", "Will -1", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void WillMinus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will - 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x0010D3A2 File Offset: 0x0010B5A2
		[DebugAction("Pawns", "Start slave rebellion (random)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void StartSlaveRebellion(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, false))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x0010D3BB File Offset: 0x0010B5BB
		[DebugAction("Pawns", "Start slave rebellion (aggressive)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void StartSlaveRebellionAggressive(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, true))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x0010D3D4 File Offset: 0x0010B5D4
		[DebugAction("Pawns", "Change style", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeStyle(Pawn p)
		{
			if (p.RaceProps.Humanlike && p.story != null)
			{
				Find.WindowStack.Add(new Dialog_StylingStation(p, null));
			}
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0010D3FC File Offset: 0x0010B5FC
		[DebugAction("Pawns", "View render tree", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ViewRenderTree(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugRenderTree(p));
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0010D40E File Offset: 0x0010B60E
		[DebugAction("Pawns", "Request style change", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void RequestStyleChange(Pawn p)
		{
			if (p.style != null && p.style.CanDesireLookChange)
			{
				p.style.RequestLookChange();
			}
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0010D430 File Offset: 0x0010B630
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void DarklightAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (intVec.InBounds(currentMap))
				{
					currentMap.debugDrawer.FlashCell(intVec, DarklightUtility.IsDarklightAt(intVec, currentMap) ? 0.5f : 0f, null, 100);
				}
			}
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x0010D4B4 File Offset: 0x0010B6B4
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void MaxDevelopmentPoints()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.TryAddDevelopmentPoints(Faction.OfPlayer.ideos.FluidIdeo.development.NextReformationDevelopmentPoints);
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0010D505 File Offset: 0x0010B705
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void AddDevelopmentPoint()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.TryAddDevelopmentPoints(1);
			}
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x0010D533 File Offset: 0x0010B733
		[DebugAction("Ideoligion", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void ClearDevelopmentPoints()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.ResetDevelopmentPoints();
			}
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x0010D560 File Offset: 0x0010B760
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnComplex()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LayoutDef localDef2 in DefDatabase<LayoutDef>.AllDefs)
			{
				LayoutDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				for (int i = 0; i < 10000; i += 100)
				{
					int localThreatPoints = i;
					debugActionNode.AddChild(new DebugActionNode(i.ToString() + " threat points", DebugActionType.Action, delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						Action <>9__2;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							string label = "second corner...";
							Action clickAction;
							if ((clickAction = <>9__2) == null)
							{
								clickAction = (<>9__2 = delegate()
								{
									IntVec3 second = UI.MouseCell();
									CellRect cellRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
									StructureGenParams parms = new StructureGenParams
									{
										size = new IntVec2(cellRect.Width, cellRect.Height)
									};
									LayoutStructureSketch layoutStructureSketch = localDef.Worker.GenerateStructureSketch(parms);
									localDef.Worker.Spawn(layoutStructureSketch, Find.CurrentMap, cellRect.Min, new float?((float)localThreatPoints), null, true, false, null);
									DebugTools.curTool = tool;
								});
							}
							DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}, null));
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x0010D638 File Offset: 0x0010B838
		[DebugAction("Ideoligion", "Generate 200 ritual names", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void Generate200RitualNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual = precept as Precept_Ritual;
					if (ritual != null)
					{
						list.Add(new DebugMenuOption(ritual.def.issue.LabelCap + ": " + ritual.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 200; i++)
							{
								stringBuilder.AppendLine(ritual.GenerateNameRaw());
							}
							Log.Message(stringBuilder.ToString());
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}
	}
}
