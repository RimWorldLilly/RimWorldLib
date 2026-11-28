using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005C2 RID: 1474
	public static class DebugToolsPawns
	{
		// Token: 0x06002D7D RID: 11645 RVA: 0x00115A78 File Offset: 0x00113C78
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void AddSlave()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Slave);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x00115A80 File Offset: 0x00113C80
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddPrisoner()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Prisoner);
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x00115A88 File Offset: 0x00113C88
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddGuest()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Guest);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x00115A90 File Offset: 0x00113C90
		private static void AddGuest(GuestStatus guestStatus)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if ((!building_Bed.OwnersForReading.Any<Pawn>() || building_Bed.AnyUnownedSleepingSlot) && (guestStatus != GuestStatus.Prisoner || building_Bed.ForPrisoners) && (guestStatus != GuestStatus.Slave || building_Bed.ForSlaves))
				{
					PawnKindDef pawnKindDef;
					if (guestStatus == GuestStatus.Guest)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionDef != null && !pk.defaultFactionDef.isPlayer && pk.RaceProps.Humanlike && pk.mutant == null
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction, null);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, guestStatus);
					break;
				}
			}
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x00115C40 File Offset: 0x00113E40
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true, null, null, null);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x00115C4C File Offset: 0x00113E4C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x00115C55 File Offset: 0x00113E55
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageUntilIncapableOfManipulation(Pawn p)
		{
			HealthUtility.DamageLimbsUntilIncapableOfManipulation(p, true);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x00115C5E File Offset: 0x00113E5E
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p, null, null, null);
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x00115C69 File Offset: 0x00113E69
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn, null, null, null);
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x00115C84 File Offset: 0x00113E84
		[DebugAction("Pawns", "10 damage until dead", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					dinfo.SetIgnoreInstantKillProtection(true);
					thing.TakeDamage(dinfo);
					if (thing.Destroyed)
					{
						string str = "Took " + (i + 1).ToString() + " hits";
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
							{
								str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
							}
							else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
							{
								str += " (core part hp reached 0)";
							}
							else
							{
								PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
								if (pawnCapacityDef != null)
								{
									str = str + " (incapable of " + pawnCapacityDef.defName + ")";
								}
							}
						}
						Log.Message(str + ".");
						break;
					}
				}
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x00115E30 File Offset: 0x00114030
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageHeldPawnToDeath()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing, null, null, null);
				}
			}
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x00115ED0 File Offset: 0x001140D0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p), null));
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00115EE8 File Offset: 0x001140E8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetHeadType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<HeadTypeDef>.Enumerator enumerator = DefDatabase<HeadTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HeadTypeDef headTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(headTypeDef.defName, delegate()
					{
						p.story.headType = headTypeDef;
						p.Drawer.renderer.SetAllGraphicsDirty();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x00115FAC File Offset: 0x001141AC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBodyType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<BodyTypeDef>.Enumerator enumerator = DefDatabase<BodyTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyTypeDef bodyTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(bodyTypeDef.defName, delegate()
					{
						p.story.bodyType = bodyTypeDef;
						p.Drawer.renderer.SetAllGraphicsDirty();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x00116070 File Offset: 0x00114270
		[DebugAction("Pawns", "Apply damage", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ApplyDamage()
		{
			return DebugTools_Health.Options_ApplyDamage();
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x00116078 File Offset: 0x00114278
		[DebugAction("Pawns", "Heal random injury (10)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HealRandomInjury10(Pawn p)
		{
			List<Hediff_Injury> source = new List<Hediff_Injury>();
			p.health.hediffSet.GetHediffs<Hediff_Injury>(ref source, (Hediff_Injury x) => x.CanHealNaturally() || x.CanHealFromTending());
			Hediff_Injury hediff_Injury;
			if (source.TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001160D4 File Offset: 0x001142D4
		[DebugAction("Pawns", "Make injuries permanent", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeInjuryPermanent(Pawn p)
		{
			foreach (Hediff hd in p.health.hediffSet.hediffs)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hd.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0011613C File Offset: 0x0011433C
		[DebugAction("Pawns", "Toggle immunity", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleImmunity(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				Hediff hediff2 = hediff;
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff2.def);
				if (immunityRecord != null)
				{
					Texture2D iconTex = (immunityRecord.immunity < 1f) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
					list.Add(new FloatMenuOption(hediff2.LabelCap, delegate()
					{
						if (immunityRecord.immunity < 1f)
						{
							immunityRecord.immunity = 1f;
							return;
						}
						immunityRecord.immunity = 0f;
					}, iconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x0011623C File Offset: 0x0011443C
		[DebugAction("Pawns", "Activate HediffGiver", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiver(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (p.RaceProps.hediffGiverSets != null)
			{
				foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
				{
					HediffGiver localHdg = localHdg2;
					list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate()
					{
						if (localHdg.TryApply(p, null))
						{
							Messages.Message(localHdg.hediff.defName + " applied to " + p.Label, MessageTypeDefOf.NeutralEvent, false);
							return;
						}
						Messages.Message("failed to apply " + localHdg.hediff.defName + " to " + p.Label, MessageTypeDefOf.NegativeEvent, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00116348 File Offset: 0x00114548
		[DebugAction("Pawns", "Activate HediffGiver World Pawn", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ActivateHediffGiverWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawnLocal2 in from p in Find.WorldPawns.AllPawnsAlive
			where p.RaceProps.Humanlike
			select p)
			{
				Pawn pawnLocal = pawnLocal2;
				list.Add(new DebugMenuOption(pawnLocal.Label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (HediffGiver hediffGiverLocal2 in pawnLocal.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef s) => s.hediffGivers))
					{
						HediffGiver hediffGiverLocal = hediffGiverLocal2;
						list2.Add(new DebugMenuOption(hediffGiverLocal.hediff.defName, DebugMenuOptionMode.Action, delegate()
						{
							if (hediffGiverLocal.TryApply(pawnLocal, null))
							{
								Messages.Message(hediffGiverLocal.hediff.defName + " applied to " + pawnLocal.Label, MessageTypeDefOf.NeutralEvent, false);
								return;
							}
							Messages.Message("failed to apply " + hediffGiverLocal.hediff.defName + " to " + pawnLocal.Label, MessageTypeDefOf.NegativeEvent, false);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x00116400 File Offset: 0x00114600
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DiscoverHediffs(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x001164A4 File Offset: 0x001146A4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrantImmunities(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x00116520 File Offset: 0x00114720
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x00116530 File Offset: 0x00114730
		[DebugAction("Pawns", "Resistance -1", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x00116584 File Offset: 0x00114784
		[DebugAction("Pawns", "Resistance -10", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x001165D8 File Offset: 0x001147D8
		[DebugAction("Pawns", "Add/remove pawn relation", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddRemovePawnRelation(Pawn p)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			Func<Pawn, bool> <>9__5;
			Action<bool> act = delegate(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list4 = new List<DebugMenuOption>();
								IEnumerable<Pawn> source = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh || (x.RaceProps.IsMechanoid && x.Faction == Faction.OfPlayer)
								select x;
								Func<Pawn, bool> keySelector;
								if ((keySelector = <>9__5) == null)
								{
									keySelector = (<>9__5 = ((Pawn x) => x.def == p.def));
								}
								foreach (Pawn pawn in source.OrderByDescending(keySelector).ThenBy((Pawn x) => x.IsWorldPawn()))
								{
									if (p != pawn && (!defLocal.familyByBloodRelation || pawn.def == p.def) && !p.relations.DirectRelationExists(defLocal, pawn))
									{
										Pawn otherLocal = pawn;
										list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
										{
											p.relations.AddDirectRelation(defLocal, otherLocal);
											if (defLocal == PawnRelationDefOf.Fiance)
											{
												otherLocal.relations.nextMarriageNameChange = (p.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(p));
											}
										}));
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4, null));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					return;
				}
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation rel = directRelations[i];
					list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
					{
						p.relations.RemoveDirectRelation(rel);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			if (!p.relations.DirectRelations.NullOrEmpty<DirectPawnRelation>())
			{
				list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
				{
					act(false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x00116680 File Offset: 0x00114880
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void AddOpinionTalksAbout(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			Action<bool> act = delegate(bool good)
			{
				Func<ThoughtDef, bool> <>9__4;
				foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (p != pawn)
					{
						IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
						Func<ThoughtDef, bool> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f))));
						}
						IEnumerable<ThoughtDef> source = allDefs.Where(predicate);
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p, null);
							}
						}
					}
				}
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x00116710 File Offset: 0x00114910
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				for (int i = 0; i <= 20; i++)
				{
					int level = i;
					debugActionNode.AddChild(new DebugActionNode(level.ToString(), DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.skills != null)
							{
								SkillRecord skill = p.skills.GetSkill(localDef);
								skill.Level = level;
								skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x001167E4 File Offset: 0x001149E4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_SkillTracker skills = p.skills;
					if (skills == null)
					{
						return;
					}
					skills.Learn(localDef, 100000000f, false, false);
				}));
			}
			return list;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x00116864 File Offset: 0x00114A64
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllSkills(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
				{
					p.skills.Learn(sDef, 100000000f, false, false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
			if (p.training != null)
			{
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					bool flag;
					if (p.training.CanAssignToTrain(td, out flag).Accepted)
					{
						p.training.Train(td, trainer, false);
					}
				}
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x00116948 File Offset: 0x00114B48
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				using (IEnumerator enumerator2 = Enum.GetValues(typeof(Passion)).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object passion = enumerator2.Current;
						debugActionNode.AddChild(new DebugActionNode(passion.ToString(), DebugActionType.ToolMapForPawns, null, null)
						{
							pawnAction = delegate(Pawn p)
							{
								if (p.skills != null)
								{
									p.skills.GetSkill(localDef).passion = (Passion)passion;
									DebugActionsUtility.DustPuffFrom(p);
								}
							}
						});
					}
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x00116A54 File Offset: 0x00114C54
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					if (p.skills == null)
					{
						return;
					}
					p.skills.GetSkill(localDef).passion = Passion.Major;
				}));
			}
			return list;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00116AD4 File Offset: 0x00114CD4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllPassions(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillRecord skillRecord in p.skills.skills)
				{
					skillRecord.passion = Passion.Major;
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x00116B38 File Offset: 0x00114D38
		[DebugAction("Pawns", "Mental break", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalBreak()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("(log possibles)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.LogPossibleMentalBreaks();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list2.Add(debugActionNode);
			List<DebugActionNode> list3 = list;
			DebugActionNode debugActionNode2 = new DebugActionNode("(natural mood break)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode2.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list3.Add(debugActionNode2);
			foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
			orderby x.intensity descending
			select x)
			{
				MentalBreakDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__5;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						locBrDef.Worker.TryStart(p, null, false);
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						if (Find.CurrentMap != null)
						{
							List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
							Predicate<Pawn> predicate;
							if ((predicate = <>9__5) == null)
							{
								predicate = (<>9__5 = ((Pawn x) => locBrDef.Worker.BreakCanOccur(x)));
							}
							if (!freeColonists.Any(predicate))
							{
								text += " [NO]";
							}
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x00116C64 File Offset: 0x00114E64
		[DebugAction("Pawns", "Mental state...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalState()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
			{
				MentalStateDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__4;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						if (locBrDef != MentalStateDefOf.SocialFighting)
						{
							p.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, false, true, false, null, false, false, false);
							DebugActionsUtility.DustPuffFrom(p);
							return;
						}
						DebugTools.curTool = new DebugTool("...with", delegate()
						{
							Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).FirstOrDefault<Thing>();
							if (pawn != null)
							{
								p.interactions.StartSocialFight(pawn, "MessageSocialFight");
								DebugTools.curTool = null;
							}
						}, null);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						if (Find.CurrentMap == null)
						{
							return text;
						}
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((Pawn x) => locBrDef.Worker.StateCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00116CFC File Offset: 0x00114EFC
		[DebugAction("Pawns", "Stop mental state", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StopMentalState(Pawn p)
		{
			if (p.InMentalState)
			{
				p.MentalState.RecoverFromState();
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00116D20 File Offset: 0x00114F20
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> Inspiration()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
			{
				InspirationDef localDef = localDef2;
				Predicate<Pawn> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						InspirationHandler inspirationHandler = p.mindState.inspirationHandler;
						if (inspirationHandler != null)
						{
							inspirationHandler.TryStartInspiration(localDef, "Debug gain", true);
						}
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = localDef.defName;
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((Pawn x) => localDef.Worker.InspirationCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x00116DB8 File Offset: 0x00114FB8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveTrait()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
			{
				TraitDef trDef = traitDef;
				for (int j = 0; j < traitDef.degreeDatas.Count; j++)
				{
					int i = j;
					list.Add(new DebugActionNode(trDef.degreeDatas[i].label + " (" + trDef.degreeDatas[j].degree.ToString() + ")", DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								p.story.traits.GainTrait(new Trait(trDef, trDef.degreeDatas[i].degree, false), true);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x00116EC4 File Offset: 0x001150C4
		[DebugAction("Pawns", "Remove all traits", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveAllTraits(Pawn p)
		{
			if (p.story != null)
			{
				for (int i = p.story.traits.allTraits.Count - 1; i >= 0; i--)
				{
					p.story.traits.RemoveTrait(p.story.traits.allTraits[i], false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x00116F28 File Offset: 0x00115128
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> SetBackstory()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Adulthood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Adulthood))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Childhood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Childhood))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			return list;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x00116FF4 File Offset: 0x001151F4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveAbility()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*All", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				if (p.abilities != null)
				{
					foreach (AbilityDef def in DefDatabase<AbilityDef>.AllDefs)
					{
						p.abilities.GainAbility(def);
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				AbilityDef localAb = abilityDef;
				list.Add(new DebugActionNode(abilityDef.label, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_AbilityTracker abilities = p.abilities;
					if (abilities == null)
					{
						return;
					}
					abilities.GainAbility(localAb);
				}));
			}
			return list;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x001170A4 File Offset: 0x001152A4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresRoyalty = true)]
		private static List<DebugActionNode> GivePsylink()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			if (!ModsConfig.RoyaltyActive)
			{
				return list;
			}
			for (int i = 1; i <= (int)HediffDefOf.PsychicAmplifier.maxSeverity; i++)
			{
				int level = i;
				list.Add(new DebugActionNode("Level " + i.ToString(), DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Hediff_Level hediff_Level = p.GetMainPsylinkSource();
						if (hediff_Level == null)
						{
							hediff_Level = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, p, p.health.hediffSet.GetBrain()) as Hediff_Level);
							p.health.AddHediff(hediff_Level, null, null, null);
						}
						hediff_Level.ChangeLevel(level - hediff_Level.level);
					}
				});
			}
			return list;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x00117118 File Offset: 0x00115318
		[DebugAction("Pawns", "Play Animation...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlayAnimation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (AnimationDef localDef2 in DefDatabase<AnimationDef>.AllDefs)
			{
				AnimationDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						p.Drawer.renderer.SetAnimation(localDef);
					}
				});
			}
			return list;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0011719C File Offset: 0x0011539C
		[DebugAction("Pawns", "Give good thought", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null, null);
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x001171CC File Offset: 0x001153CC
		[DebugAction("Pawns", "Give bad thought", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null, null);
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x001171FC File Offset: 0x001153FC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ClearBoundUnfinishedThings()
		{
			foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Building_WorkTable
			select t).Cast<Building_WorkTable>())
			{
				foreach (Bill bill in building_WorkTable.BillStack)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						bill_ProductionWithUft.ClearBoundUft();
					}
				}
			}
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x001172BC File Offset: 0x001154BC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x001172EC File Offset: 0x001154EC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, true);
					DebugActionsUtility.DustPuffFrom(p);
					return;
				}
				if (p.RaceProps.IsMechanoid)
				{
					p.SetFaction(Faction.OfPlayer, null);
					DebugActionsUtility.DustPuffFrom(p);
				}
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x00117355 File Offset: 0x00115555
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void Enslave(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				GenGuest.EnslavePrisoner(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x00117394 File Offset: 0x00115594
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleRecruitable(Pawn p)
		{
			if (p.guest != null)
			{
				p.guest.Recruitable = !p.guest.Recruitable;
				DebugActionsUtility.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.MapHeld, "Recruitable:\n" + p.guest.Recruitable.ToStringYesNo(), -1f);
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x001173F8 File Offset: 0x001155F8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowPawnToMaturity()
		{
			Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
			if (firstPawn != null)
			{
				firstPawn.ageTracker.AgeBiologicalTicks += (long)Mathf.FloorToInt(firstPawn.ageTracker.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00117444 File Offset: 0x00115644
		[DebugAction("Pawns", "Wear apparel (selected)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> WearApparel_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove all apparel", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				foreach (object obj in Find.Selector.SelectedObjectsListForReading)
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null)
					{
						Pawn_ApparelTracker apparel = pawn.apparel;
						if (apparel != null)
						{
							apparel.DestroyAll(DestroyMode.Vanish);
						}
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						foreach (object obj in Find.Selector.SelectedObjectsListForReading)
						{
							Pawn pawn = obj as Pawn;
							if (pawn != null && pawn.apparel != null)
							{
								ThingDef stuff = GenStuff.RandomStuffFor(localDef);
								Apparel newApparel = (Apparel)ThingMaker.MakeThing(localDef, stuff);
								pawn.apparel.Wear(newApparel, false, false);
							}
						}
					}
				});
			}
			return list;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x00117548 File Offset: 0x00115748
		[DebugAction("Pawns", "Equip primary (selected)...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EquipPrimary_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove primary", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				foreach (object obj in Find.Selector.SelectedObjectsListForReading)
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null)
					{
						Pawn_EquipmentTracker equipment = pawn.equipment;
						if (((equipment != null) ? equipment.Primary : null) != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
					}
				}
			};
			list2.Add(debugActionNode);
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.equipmentType == EquipmentType.Primary
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					ThingDef def2 = def;
					list.Add(new DebugActionNode(def2.defName, DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							foreach (object obj in Find.Selector.SelectedObjectsListForReading)
							{
								Pawn pawn = obj as Pawn;
								if (pawn != null && pawn.equipment != null)
								{
									if (pawn.equipment.Primary != null)
									{
										pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
									}
									ThingDef stuff = GenStuff.RandomStuffFor(def);
									ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
									pawn.equipment.AddEquipment(newEq);
								}
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x0011764C File Offset: 0x0011584C
		public static List<FloatMenuOption> PawnGearDevOptions(Pawn pawn)
		{
			return new List<FloatMenuOption>
			{
				new FloatMenuOption("Set primary", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_SetPrimary(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Wear", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_Wear(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Add to inventory", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_GiveToInventory(pawn), null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Damage random apparel", delegate()
				{
					pawn.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x0011770C File Offset: 0x0011590C
		private static List<DebugMenuOption> Options_Wear(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove all apparel", DebugMenuOptionMode.Action, delegate()
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}));
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef def) => def.IsApparel && def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage)));
			}
			using (IEnumerator<ThingDef> enumerator = (from d in allDefs.Where(predicate)
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						Apparel newApparel = (Apparel)ThingMaker.MakeThing(def, stuff);
						pawn.apparel.Wear(newApparel, false, false);
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x00117804 File Offset: 0x00115A04
		private static List<DebugMenuOption> Options_SetPrimary(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove primary", DebugMenuOptionMode.Action, delegate()
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
				}
			}));
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						if (pawn.equipment.Primary != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
						pawn.equipment.AddEquipment(newEq);
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x001178F8 File Offset: 0x00115AF8
		private static List<DebugMenuOption> Options_GiveToInventory(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Clear all", DebugMenuOptionMode.Action, delegate()
			{
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
			}));
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate()
					{
						pawn.inventory.TryAddItemNotForSale(ThingMaker.MakeThing(def, null));
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x001179EC File Offset: 0x00115BEC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x00117A28 File Offset: 0x00115C28
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TrainAnimal(Pawn p)
		{
			if (p.IsAnimal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				DebugActionsUtility.DustPuffFrom(p);
				bool flag = false;
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					if (p.training.GetWanted(td))
					{
						p.training.Train(td, null, true);
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.CanAssignToTrain(td2).Accepted)
						{
							p.training.Train(td2, null, true);
						}
					}
				}
			}
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x00117B18 File Offset: 0x00115D18
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryDevelopBoundRelation(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn humanlike;
			if (p.RaceProps.Humanlike)
			{
				Pawn animal;
				if ((from x in p.Map.mapPawns.AllPawnsSpawned
				where x.IsAnimal && x.Faction == p.Faction
				select x).TryRandomElement(out animal))
				{
					RelationsUtility.TryDevelopBondRelation(p, animal, 999999f);
					return;
				}
			}
			else if (p.IsAnimal && (from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.Faction == p.Faction
			select x).TryRandomElement(out humanlike))
			{
				RelationsUtility.TryDevelopBondRelation(humanlike, p, 999999f);
			}
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x00117BE2 File Offset: 0x00115DE2
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.IsAnimal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x00117C14 File Offset: 0x00115E14
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayRelationsInfo(Pawn pawn)
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.LabelCap));
			list.Add(new TableDataGetter<Pawn>("kind label", (Pawn p) => p.KindLabel));
			list.Add(new TableDataGetter<Pawn>("gender", (Pawn p) => p.gender.GetLabel(false)));
			list.Add(new TableDataGetter<Pawn>("age", (Pawn p) => p.ageTracker.AgeBiologicalYears));
			list.Add(new TableDataGetter<Pawn>("my compat", (Pawn p) => pawn.relations.CompatibilityWith(p).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("their compat", (Pawn p) => p.relations.CompatibilityWith(pawn).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("my 2nd\nrom chance", (Pawn p) => pawn.relations.SecondaryRomanceChanceFactor(p).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("their 2nd\nrom chance", (Pawn p) => p.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("lovin mtb", (Pawn p) => LovePartnerRelationUtility.GetLovinMtbHours(pawn, p).ToString("F1") + " h"));
			List<TableDataGetter<Pawn>> list2 = list;
			DebugTables.MakeTablesDialog<Pawn>(from x in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where x != pawn && x.RaceProps.Humanlike
			select x, list2.ToArray());
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x00117DB8 File Offset: 0x00115FB8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayInteractionsInfo(Pawn pawn)
		{
			DebugToolsPawns.<>c__DisplayClass62_0 CS$<>8__locals1 = new DebugToolsPawns.<>c__DisplayClass62_0();
			CS$<>8__locals1.pawn = pawn;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<Pawn> source = CS$<>8__locals1.pawn.Map.mapPawns.SpawnedPawnsInFaction(CS$<>8__locals1.pawn.Faction);
			Func<Pawn, bool> predicate;
			if ((predicate = CS$<>8__locals1.<>9__0) == null)
			{
				predicate = (CS$<>8__locals1.<>9__0 = ((Pawn x) => x != CS$<>8__locals1.pawn && x.RaceProps.Humanlike));
			}
			using (IEnumerator<Pawn> enumerator = source.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugToolsPawns.<>c__DisplayClass62_1 CS$<>8__locals2 = new DebugToolsPawns.<>c__DisplayClass62_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					float totalWeight = DefDatabase<InteractionDef>.AllDefs.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p));
					Func<InteractionDef, string> <>9__5;
					list.Add(new DebugMenuOption(CS$<>8__locals2.p.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<InteractionDef>> list2 = new List<TableDataGetter<InteractionDef>>();
						list2.Add(new TableDataGetter<InteractionDef>("defName", (InteractionDef i) => i.defName));
						string label = "sel weight";
						Func<InteractionDef, float> getter;
						if ((getter = CS$<>8__locals2.<>9__4) == null)
						{
							getter = (CS$<>8__locals2.<>9__4 = ((InteractionDef i) => i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p)));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label, getter));
						string label2 = "sel chance";
						Func<InteractionDef, string> getter2;
						if ((getter2 = <>9__5) == null)
						{
							getter2 = (<>9__5 = ((InteractionDef i) => (i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p) / totalWeight).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label2, getter2));
						string label3 = "fight\nchance";
						Func<InteractionDef, string> getter3;
						if ((getter3 = CS$<>8__locals2.<>9__6) == null)
						{
							getter3 = (CS$<>8__locals2.<>9__6 = ((InteractionDef i) => CS$<>8__locals2.p.interactions.SocialFightChance(i, CS$<>8__locals2.CS$<>8__locals1.pawn).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label3, getter3));
						string label4 = "success\nchance";
						Func<InteractionDef, string> getter4;
						if ((getter4 = CS$<>8__locals2.<>9__7) == null)
						{
							getter4 = (CS$<>8__locals2.<>9__7 = delegate(InteractionDef i)
							{
								if (i == InteractionDefOf.RomanceAttempt)
								{
									return InteractionWorker_RomanceAttempt.SuccessChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p, 0.6f).ToStringPercent();
								}
								if (i == InteractionDefOf.MarriageProposal)
								{
									return InteractionWorker_MarriageProposal.AcceptanceChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p).ToStringPercent();
								}
								return "";
							});
						}
						list2.Add(new TableDataGetter<InteractionDef>(label4, getter4));
						List<TableDataGetter<InteractionDef>> list3 = list2;
						DebugTables.MakeTablesDialog<InteractionDef>(DefDatabase<InteractionDef>.AllDefs, list3.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x00117ED8 File Offset: 0x001160D8
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StartMarriageCeremony(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike
			select x)
			{
				if (p != pawn)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
						{
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
							p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
							Messages.Message("DEV: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
						}
						if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
						{
							Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x00117FF0 File Offset: 0x001161F0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceInteraction(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn != p)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
						{
							InteractionDef interactionLocal = interactionLocal2;
							list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
							{
								p.interactions.TryInteractWith(otherLocal, interactionLocal);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x001180F0 File Offset: 0x001162F0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> StartGathering()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Random", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
				{
					Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
				}
			};
			list2.Add(debugActionNode);
			foreach (GatheringDef gatheringDef2 in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				GatheringDef gatheringDef = gatheringDef2;
				list.Add(new DebugActionNode(gatheringDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						gatheringDef.Worker.TryExecute(Find.CurrentMap, null);
					},
					labelGetter = delegate()
					{
						TaggedString t = gatheringDef.LabelCap + " (";
						World world = Find.World;
						return t + ((((world != null) ? world.factionManager : null) != null && gatheringDef.Worker.CanExecute(Find.CurrentMap, null)) ? "Yes" : "No") + ")";
					}
				});
			}
			return list;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x001181C4 File Offset: 0x001163C4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x001181D5 File Offset: 0x001163D5
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x001181EC File Offset: 0x001163EC
		[DebugAction("Spawning", "Remove world pawn...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void RemoveWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				Pawn pLocal = pawn;
				string text = pawn.LabelShort;
				WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn);
				if (situation != WorldPawnSituation.Free)
				{
					text = text + " [" + situation.ToString() + "]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					Find.WorldPawns.RemovePawn(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x001182B4 File Offset: 0x001164B4
		[DebugAction("Pawns", "Make +1 year older", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			float num = p.ageTracker.BiologicalTicksPerTick;
			if (num == 0f)
			{
				num = 1f;
			}
			p.ageTracker.AgeTickMothballed(Mathf.RoundToInt(3600000f / num));
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x001182F2 File Offset: 0x001164F2
		[DebugAction("Pawns", "Make +1 day older", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1DayOlder(Pawn p)
		{
			p.ageTracker.AgeTickMothballed(60000);
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x00118304 File Offset: 0x00116504
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJobGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
			{
				Type localType = localType2;
				list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate()
				{
					ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
					thinkNode_JobGiver.ResolveReferences();
					ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
					if (thinkResult.Job != null)
					{
						p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
						return;
					}
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x001183BC File Offset: 0x001165BC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJoyGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JoyGiverDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanBeGivenTo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x00118494 File Offset: 0x00116694
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x001184AB File Offset: 0x001166AB
		[DebugAction("Pawns", "CheckForJobOverride", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride(0f, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x001184C4 File Offset: 0x001166C4
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			if (((p != null) ? p.jobs : null) == null)
			{
				return;
			}
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPosHeld.Value, p.MapHeld, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x00118548 File Offset: 0x00116748
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TogglePathDebugging(Pawn p)
		{
			if (((p != null) ? p.jobs : null) == null)
			{
				return;
			}
			p.pather.debugLog = !p.pather.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPosHeld.Value, p.MapHeld, p.LabelShort + "\n" + (p.pather.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x001185CA File Offset: 0x001167CA
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogJobDetails(Pawn p)
		{
			if (p.CurJob != null)
			{
				p.CurJob.LogDetails(p);
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x001185E0 File Offset: 0x001167E0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x00118604 File Offset: 0x00116804
		[DebugAction("Pawns", "Kidnap colonist", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void Kidnap(Pawn p)
		{
			if (p.IsColonist)
			{
				Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				if (faction != null)
				{
					faction.kidnapped.Kidnap(p, faction.leader);
				}
			}
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x00118640 File Offset: 0x00116840
		[DebugAction("Pawns", "Face cell (selected)...", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetFacing()
		{
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null)
				{
					pawn.rotationTracker.FaceTarget(UI.MouseCell());
				}
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x001186B0 File Offset: 0x001168B0
		[DebugAction("Pawns", "Set enemy target for (selected)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetTarget(Pawn p)
		{
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null)
				{
					pawn.mindState.enemyTarget = p;
					pawn.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x0011872C File Offset: 0x0011692C
		[DebugAction("Pawns", "Progress life stage", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ProgressLifeStage(Pawn p)
		{
			int curLifeStageIndex = p.ageTracker.CurLifeStageIndex;
			if (curLifeStageIndex < p.ageTracker.MaxRaceLifeStageIndex)
			{
				float minAge = p.RaceProps.lifeStageAges[curLifeStageIndex + 1].minAge;
				float minAge2 = p.RaceProps.lifeStageAges[p.RaceProps.lifeStageAges.Count - 1].minAge;
				if (p.RaceProps.Humanlike)
				{
					p.ageTracker.DebugSetAge((long)minAge * 3600000L);
				}
				else
				{
					p.ageTracker.DebugSetGrowth(minAge / minAge2);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x001187CB File Offset: 0x001169CB
		[DebugAction("Pawns", "Make guilty", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void MakeGuilty(Pawn p)
		{
			Pawn_GuiltTracker guilt = p.guilt;
			if (guilt != null)
			{
				guilt.Notify_Guilty(60000);
			}
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001187E9 File Offset: 0x001169E9
		[DebugAction("Pawns", "Force age reversal demand now", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ForceAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugForceAgeReversalDemandNow();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x001187FC File Offset: 0x001169FC
		[DebugAction("Pawns", "Reset age reversal demand", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ResetAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugResetAgeReversalDemand();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x00118810 File Offset: 0x00116A10
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Resurrect()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.TryResurrect(corpse.InnerPawn, null);
				}
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x00118880 File Offset: 0x00116A80
		public static List<DebugMenuOption> Options_AddGene(Action<GeneDef> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					callback(localDef);
				}));
			}
			return list;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0011893C File Offset: 0x00116B3C
		public static List<DebugMenuOption> Options_RemoveGene(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (pawn.genes != null)
			{
				foreach (Gene localG2 in pawn.genes.GenesListForReading)
				{
					Gene localG = localG2;
					list.Add(new DebugMenuOption(localG.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						pawn.genes.RemoveGene(localG);
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x001189F0 File Offset: 0x00116BF0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> AddGene()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Xenogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(true, false))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Endogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(false, false))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			DebugActionNode debugActionNode3 = new DebugActionNode("Heritable", DebugActionType.Action, null, null);
			foreach (DebugActionNode child3 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|89_0(false, true))
			{
				debugActionNode3.AddChild(child3);
			}
			list.Add(debugActionNode3);
			return list;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x00118B1C File Offset: 0x00116D1C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static void RemoveGene(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_RemoveGene(p), null));
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x00118B34 File Offset: 0x00116D34
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> SetXenotype()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (XenotypeDef localDef2 in from x in DefDatabase<XenotypeDef>.AllDefs
			orderby x.defName
			select x)
			{
				XenotypeDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Pawn_GeneTracker genes = p.genes;
						if (genes != null)
						{
							genes.SetXenotype(localDef);
						}
						DebugActionsUtility.DustPuffFrom(p);
					}
				});
			}
			return list;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00118BDC File Offset: 0x00116DDC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> AddLearningDesire()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LearningDesireDef defLocal2 in DefDatabase<LearningDesireDef>.AllDefs)
			{
				LearningDesireDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_LearningTracker learning = p.learning;
					if (learning != null)
					{
						learning.Debug_SetLearningDesire(defLocal);
					}
					DebugActionsUtility.DustPuffFrom(p);
				}));
			}
			return list;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00118C5C File Offset: 0x00116E5C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void TryLearningGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<LearningDesireDef>.Enumerator enumerator = DefDatabase<LearningDesireDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LearningDesireDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanDo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x00118D34 File Offset: 0x00116F34
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ListMeleeVerbs(Pawn p)
		{
			List<Verb> allMeleeVerbs = (from x in p.meleeVerbs.GetUpdatedAvailableVerbsList(false)
			select x.verb).ToList<Verb>();
			float highestWeight = 0f;
			foreach (Verb v2 in allMeleeVerbs)
			{
				float num = VerbUtility.InitialVerbWeight(v2, p);
				if (num > highestWeight)
				{
					highestWeight = num;
				}
			}
			float totalSelectionWeight = 0f;
			foreach (Verb verb in allMeleeVerbs)
			{
				totalSelectionWeight += VerbUtility.FinalSelectionWeight(verb, p, allMeleeVerbs, highestWeight);
			}
			allMeleeVerbs.SortBy((Verb x) => -VerbUtility.InitialVerbWeight(x, p));
			List<TableDataGetter<Verb>> list = new List<TableDataGetter<Verb>>();
			list.Add(new TableDataGetter<Verb>("verb", (Verb v) => v.ToString().Split('/', StringSplitOptions.None)[1].TrimEnd(')')));
			list.Add(new TableDataGetter<Verb>("source", delegate(Verb v)
			{
				if (v.HediffSource != null)
				{
					return v.HediffSource.Label;
				}
				if (v.tool != null)
				{
					return v.tool.label;
				}
				return "";
			}));
			list.Add(new TableDataGetter<Verb>("damage", (Verb v) => v.verbProps.AdjustedMeleeDamageAmount(v, p)));
			list.Add(new TableDataGetter<Verb>("cooldown", (Verb v) => v.verbProps.AdjustedCooldown(v, p).ToString() + "s"));
			list.Add(new TableDataGetter<Verb>("dmg/sec", (Verb v) => VerbUtility.DPS(v, p)));
			list.Add(new TableDataGetter<Verb>("armor pen", (Verb v) => v.verbProps.AdjustedArmorPenetration(v, p)));
			list.Add(new TableDataGetter<Verb>("hediff", delegate(Verb v)
			{
				string text = "";
				if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
				{
					foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
					{
						text = text + damageDefAdditionalHediff.hediff.label + " ";
					}
				}
				return text;
			}));
			list.Add(new TableDataGetter<Verb>("weight", (Verb v) => VerbUtility.InitialVerbWeight(v, p)));
			list.Add(new TableDataGetter<Verb>("category", delegate(Verb v)
			{
				VerbSelectionCategory selectionCategory = v.GetSelectionCategory(p, highestWeight);
				if (selectionCategory == VerbSelectionCategory.Best)
				{
					return "Best".Colorize(Color.green);
				}
				if (selectionCategory != VerbSelectionCategory.Worst)
				{
					return "Mid";
				}
				return "Worst".Colorize(Color.grey);
			}));
			list.Add(new TableDataGetter<Verb>("sel %", (Verb v) => base.<ListMeleeVerbs>g__GetSelectionPercent|1(v).ToStringPercent("F2")));
			List<TableDataGetter<Verb>> list2 = list;
			DebugTables.MakeTablesDialog<Verb>(allMeleeVerbs, list2.ToArray());
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x00118FE0 File Offset: 0x001171E0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DoVoiceCall(Pawn p)
		{
			Pawn_CallTracker caller = p.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall(false);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00118FF4 File Offset: 0x001171F4
		[DebugAction("Pawns", "Force vomit", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x00119033 File Offset: 0x00117233
		[DebugAction("Pawns", "Reset pawn render cache", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ResetRenderCache(Pawn p)
		{
			p.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x00119048 File Offset: 0x00117248
		[DebugAction("Anomaly", "Max ghoul upgrades", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void MaxGhoulUpgrades(Pawn p)
		{
			p.health.AddHediff(HediffDefOf.GhoulBarbs, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Torso), null, null);
			p.health.AddHediff(HediffDefOf.GhoulPlating, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Torso), null, null);
			p.health.AddHediff(HediffDefOf.AdrenalHeart, p.health.hediffSet.GetBodyPartRecord(BodyPartDefOf.Heart), null, null);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x001190E8 File Offset: 0x001172E8
		[DebugAction("Anomaly", "Raise corpse as shambler", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void RaiseAsShambler()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					MutantUtility.ResurrectAsShambler(corpse.InnerPawn, -1, null);
					break;
				}
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x00119154 File Offset: 0x00117354
		[DebugAction("Anomaly", "Set mutant...", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetMutant()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<MutantDef> enumerator = DefDatabase<MutantDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MutantDef mutant = enumerator.Current;
					list.Add(new DebugActionNode(mutant.LabelCap, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
					{
						MutantUtility.SetPawnAsMutantInstantly(p, mutant, RotStage.Fresh);
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x001191D4 File Offset: 0x001173D4
		[DebugAction("Anomaly", "Revert mutant", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void RevertMutant(Pawn p)
		{
			if (p.IsMutant)
			{
				p.mutant.Revert(false);
				p.mutant = null;
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x001191F4 File Offset: 0x001173F4
		[DebugAction("Pawns", "Create baby from parents", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void CreateBabyFromParents()
		{
			DebugTool tool = null;
			Pawn parent1 = null;
			Action <>9__2;
			tool = new DebugTool("First parent...", delegate()
			{
				parent1 = DebugToolsPawns.<CreateBabyFromParents>g__PawnAt|102_0(UI.MouseCell());
				if (parent1 != null)
				{
					string label = "Second parent...";
					Action clickAction;
					if ((clickAction = <>9__2) == null)
					{
						clickAction = (<>9__2 = delegate()
						{
							Pawn pawn = DebugToolsPawns.<CreateBabyFromParents>g__PawnAt|102_0(UI.MouseCell());
							if (pawn != null)
							{
								PregnancyUtility.ApplyBirthOutcome(RitualOutcomeEffectDefOf.ChildBirth.BestOutcome, 1f, null, null, parent1, pawn, pawn, null, null, null, false);
							}
							DebugTools.curTool = tool;
						});
					}
					DebugTools.curTool = new DebugTool(label, clickAction, null);
				}
			}, null);
			DebugTools.curTool = tool;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x00119240 File Offset: 0x00117440
		[DebugAction("Pawns", "Infection pathway debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenInfectionPathwayDebugger()
		{
			Dialog_DevInfectionPathways dialog_DevInfectionPathways;
			if (Find.WindowStack.TryGetWindow<Dialog_DevInfectionPathways>(out dialog_DevInfectionPathways))
			{
				dialog_DevInfectionPathways.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevInfectionPathways());
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00119274 File Offset: 0x00117474
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> AddInfectionPathway()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<InfectionPathwayDef> enumerator = DefDatabase<InfectionPathwayDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InfectionPathwayDef def = enumerator.Current;
					string label = (!def.PawnRequired) ? def.defName : (def.defName + " [NO]");
					list.Add(new DebugActionNode(label, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
					{
						if (def.PawnRequired)
						{
							return;
						}
						p.infectionVectors.AddInfectionVector(def, null);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
					}));
				}
			}
			return list;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x00119318 File Offset: 0x00117518
		[DebugAction("Pawns", "Destroy factionless animals", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAnimals()
		{
			List<Pawn> list = new List<Pawn>();
			list.AddRange(Find.CurrentMap.mapPawns.AllPawnsSpawned);
			foreach (Pawn pawn in list)
			{
				if (pawn.RaceProps.Animal && pawn.Faction == null)
				{
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x00119394 File Offset: 0x00117594
		[DebugAction("Pawns", "Destroy player animals", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyPlayerAnimals()
		{
			List<Pawn> list = new List<Pawn>();
			list.AddRange(Find.CurrentMap.mapPawns.SpawnedColonyAnimals);
			foreach (Pawn pawn in list)
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x001193FC File Offset: 0x001175FC
		[DebugAction("Pawns", "Destroy non-colonists", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllNonColonists()
		{
			IReadOnlyList<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = allPawnsSpawned.Count - 1; i >= 0; i--)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (!pawn.IsColonist || !pawn.RaceProps.Humanlike)
				{
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00119450 File Offset: 0x00117650
		[DebugAction("Other", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, hideInSubMenu = true)]
		private static void GarbageCollectWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					foreach (Pawn pawn in Find.WorldPawns.GetPawnsBySituation(worldPawnSituation).OrderBy(delegate(Pawn x)
					{
						Faction faction = x.Faction;
						if (faction == null)
						{
							return -1;
						}
						return faction.loadID;
					}))
					{
						if (pawn.relations != null)
						{
							Pawn local = pawn;
							list.Add(new DebugMenuOption(pawn.LabelShort + " [" + worldPawnSituation.ToString() + "]", DebugMenuOptionMode.Action, delegate()
							{
								local.markedForDiscard = true;
								Find.WorldPawns.RemoveAndDiscardPawnViaGC(local);
								int value = Find.RelationshipRecords.CleanupUnusedRecords();
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
								defaultInterpolatedStringHandler.AppendFormatted<int>(value);
								defaultInterpolatedStringHandler.AppendLiteral(" records found and removed.");
								Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x00119564 File Offset: 0x00117764
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UnlockAllApparel(Pawn p)
		{
			p.apparel.UnlockAll();
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00119574 File Offset: 0x00117774
		[DebugAction("Pawns", "Try make animal fish for food", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryFishForFood(Pawn p)
		{
			if (!p.IsAnimal)
			{
				Messages.Message("Must select animal to fish for food.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job = JobGiver_GetFood.TryFindFishJob(p);
			if (job != null)
			{
				p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				return;
			}
			Messages.Message("Unable to find fish job", MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x001195E0 File Offset: 0x001177E0
		[CompilerGenerated]
		internal static List<DebugActionNode> <SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot slot)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
			Func<BackstoryDef, bool> <>9__1;
			Func<BackstoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((BackstoryDef b) => b.slot == slot));
			}
			using (IEnumerator<BackstoryDef> enumerator = allDefs.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackstoryDef outerBackstory = enumerator.Current;
					list.Add(new DebugActionNode(outerBackstory.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								if (slot == BackstorySlot.Adulthood)
								{
									p.story.Adulthood = outerBackstory;
								}
								else
								{
									p.story.Childhood = outerBackstory;
								}
								MeditationFocusTypeAvailabilityCache.ClearFor(p);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x001196A0 File Offset: 0x001178A0
		[CompilerGenerated]
		internal static List<DebugActionNode> <AddGene>g__GeneOptionNodes|89_0(bool xenogene, bool heritableOnly)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				if ((xenogene || localDef.biostatArc <= 0) && (!heritableOnly || localDef.endogeneCategory != EndogeneCategory.None || !localDef.forcedTraits.NullOrEmpty<GeneticTraitData>() || DefDatabase<XenotypeDef>.AllDefs.Any((XenotypeDef x) => x.genes.Contains(localDef) && x.inheritable)))
				{
					list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							Pawn_GeneTracker genes = p.genes;
							if (genes != null)
							{
								genes.AddGene(localDef, xenogene);
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x001197C4 File Offset: 0x001179C4
		[CompilerGenerated]
		internal static Pawn <CreateBabyFromParents>g__PawnAt|102_0(IntVec3 c)
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
