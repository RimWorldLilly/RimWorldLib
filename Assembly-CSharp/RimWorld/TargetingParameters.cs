using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD7 RID: 8151
	public class TargetingParameters
	{
		// Token: 0x0600CED1 RID: 52945 RVA: 0x004C2714 File Offset: 0x004C0914
		public bool CanTarget(TargetInfo targ, ITargetingSource source = null)
		{
			if (this.validator != null && !this.validator(targ))
			{
				return false;
			}
			if (targ.Thing == null)
			{
				return this.canTargetLocations;
			}
			if (this.neverTargetDoors && targ.Thing.def.IsDoor)
			{
				return false;
			}
			if (this.onlyTargetDamagedThings && targ.Thing.HitPoints == targ.Thing.MaxHitPoints)
			{
				return false;
			}
			if (this.onlyTargetFlammables && !targ.Thing.FlammableNow)
			{
				return false;
			}
			if (this.mustBeSelectable && !ThingSelectionUtility.SelectableByMapClick(targ.Thing))
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisoners && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisonersOrSlaves && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.onlyTargetDoors && !targ.Thing.def.IsDoor)
			{
				return false;
			}
			Corpse corpse;
			if ((corpse = (targ.Thing as Corpse)) == null)
			{
				Pawn pawn = targ.Thing as Pawn;
				corpse = ((pawn != null) ? pawn.Corpse : null);
			}
			Corpse corpse2 = corpse;
			if (this.canTargetCorpses && corpse2 != null)
			{
				return (this.canTargetMechs || !corpse2.InnerPawn.RaceProps.IsMechanoid) && (this.canTargetAnimals || !corpse2.InnerPawn.RaceProps.Animal) && (this.canTargetHumans || !corpse2.InnerPawn.RaceProps.Humanlike) && (this.canTargetSubhumans || !corpse2.InnerPawn.IsSubhuman);
			}
			if (this.onlyTargetCorpses)
			{
				return false;
			}
			if (this.targetSpecificThing != null && targ.Thing == this.targetSpecificThing)
			{
				return true;
			}
			if (this.canTargetFires && targ.Thing.def == ThingDefOf.Fire)
			{
				return true;
			}
			if (this.canTargetPawns && targ.Thing.def.category == ThingCategory.Pawn)
			{
				Pawn pawn2 = (Pawn)targ.Thing;
				if (pawn2.Downed)
				{
					if (this.neverTargetIncapacitated)
					{
						return false;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					return false;
				}
				if (this.onlyTargetFactions != null && !this.onlyTargetFactions.Contains(targ.Thing.Faction))
				{
					return false;
				}
				if (pawn2.NonHumanlikeOrWildMan())
				{
					if (pawn2.Faction != null && pawn2.RaceProps.IsMechanoid)
					{
						if (!this.canTargetMechs)
						{
							return false;
						}
						if (this.onlyRepairableMechs && !MechRepairUtility.CanRepair(pawn2))
						{
							return false;
						}
					}
					else if (!this.canTargetAnimals)
					{
						return false;
					}
				}
				if (!pawn2.NonHumanlikeOrWildMan() && !this.canTargetHumans)
				{
					return false;
				}
				if (!this.canTargetEntities && pawn2.IsEntity)
				{
					return false;
				}
				if (!this.canTargetSubhumans && pawn2.IsSubhuman)
				{
					return false;
				}
				if (this.onlyTargetControlledPawns && !pawn2.IsColonistPlayerControlled)
				{
					return false;
				}
				if (this.onlyTargetColonists && (!pawn2.IsColonist || pawn2.HostFaction != null))
				{
					return false;
				}
				if (this.onlyTargetPrisonersOfColony && !pawn2.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisoners && !pawn2.IsColonistPlayerControlled && !pawn2.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlaves && !pawn2.IsColonistPlayerControlled && !pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks)
				{
					if (!pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony && (!pawn2.IsColonist || (pawn2.HostFaction != null && !pawn2.IsSlave)))
					{
						return false;
					}
					MentalStateDef mentalStateDef = pawn2.MentalStateDef;
					if (mentalStateDef != null && mentalStateDef.IsAggro)
					{
						return false;
					}
				}
				if (this.onlyTargetPsychicSensitive && pawn2.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					return false;
				}
				if (this.neverTargetHostileFaction && !pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony)
				{
					Faction homeFaction = pawn2.HomeFaction;
					if (homeFaction != null && homeFaction.HostileTo(Faction.OfPlayer))
					{
						return false;
					}
				}
				if (this.onlyTargetSameIdeo)
				{
					if (source == null)
					{
						Log.Error("Source passed in is null but targeting parameters have onlyTargetSameIdeo set.");
					}
					else
					{
						Verb verb = source as Verb;
						if (verb != null && verb.CasterPawn != null)
						{
							Pawn pawn3 = targ.Thing as Pawn;
							Ideo ideo = (pawn3 != null) ? pawn3.Ideo : null;
							if (verb.CasterPawn.Ideo != ideo)
							{
								return false;
							}
						}
						else
						{
							Log.Error("Source passed in is incompatible type but targeting parameters have onlyTargetSameIdeo set.");
						}
					}
				}
				return this.canTargetBloodfeeders || !ModsConfig.BiotechActive || !pawn2.IsBloodfeeder();
			}
			else
			{
				if (this.canTargetBuildings && targ.Thing.def.category == ThingCategory.Building)
				{
					return (!this.mapObjectTargetsMustBeAutoAttackable || targ.Thing.def.building.isTargetable) && (!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) && (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction));
				}
				if (this.canTargetPlants && targ.Thing.def.category == ThingCategory.Plant)
				{
					return !ModsConfig.RoyaltyActive || !this.onlyTargetAnimaTrees || targ.Thing.def == ThingDefOf.Plant_TreeAnima;
				}
				if (this.canTargetItems)
				{
					if (this.mapObjectTargetsMustBeAutoAttackable && !targ.Thing.def.isAutoAttackableMapObject)
					{
						return false;
					}
					if (this.thingCategory == ThingCategory.None || this.thingCategory == targ.Thing.def.category)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600CED2 RID: 52946 RVA: 0x004C2C76 File Offset: 0x004C0E76
		public static TargetingParameters ForAttackAny()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = true
			};
		}

		// Token: 0x0600CED3 RID: 52947 RVA: 0x004C2C9C File Offset: 0x004C0E9C
		public static TargetingParameters ForForceWear(Pawn selectedPawnForJob)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetAnimals = false,
				canTargetMechs = false,
				canTargetItems = false,
				canTargetBuildings = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					if (pawn == null)
					{
						return ModsConfig.OdysseyActive && targ.Thing is Building_OutfitStand;
					}
					return pawn != selectedPawnForJob && pawn.kindDef.canStrip && ((pawn.Downed && pawn.RaceProps.Humanlike) || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure) || (!pawn.IsQuestLodger() && pawn.IsColonist));
				}
			};
		}

		// Token: 0x0600CED4 RID: 52948 RVA: 0x004C2CF8 File Offset: 0x004C0EF8
		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = ((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true, true));
			return targetingParameters;
		}

		// Token: 0x0600CED5 RID: 52949 RVA: 0x004C2D5C File Offset: 0x004C0F5C
		public static TargetingParameters ForBuilding(ThingDef def = null)
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetItems = false,
				canTargetBuildings = true,
				validator = ((TargetInfo targ) => targ.HasThing && (def == null || targ.Thing.def == def))
			};
		}

		// Token: 0x0600CED6 RID: 52950 RVA: 0x004C2DA2 File Offset: 0x004C0FA2
		public static TargetingParameters ForColonist()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				onlyTargetColonists = true
			};
		}

		// Token: 0x0600CED7 RID: 52951 RVA: 0x004C2DBE File Offset: 0x004C0FBE
		public static TargetingParameters ForPawns()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x0600CED8 RID: 52952 RVA: 0x004C2DD3 File Offset: 0x004C0FD3
		public static TargetingParameters ForThing()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				canTargetPlants = true,
				canTargetFires = true,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x0600CED9 RID: 52953 RVA: 0x004C2E04 File Offset: 0x004C1004
		public static TargetingParameters ForCell()
		{
			return new TargetingParameters
			{
				canTargetLocations = true,
				canTargetBuildings = false,
				canTargetHumans = false,
				canTargetMechs = false,
				canTargetAnimals = false,
				onlyTargetColonists = false
			};
		}

		// Token: 0x04008379 RID: 33657
		public bool canTargetLocations;

		// Token: 0x0400837A RID: 33658
		public bool canTargetSelf;

		// Token: 0x0400837B RID: 33659
		public bool canTargetPawns = true;

		// Token: 0x0400837C RID: 33660
		public bool canTargetFires;

		// Token: 0x0400837D RID: 33661
		public bool canTargetBuildings = true;

		// Token: 0x0400837E RID: 33662
		public bool canTargetItems;

		// Token: 0x0400837F RID: 33663
		public bool canTargetAnimals = true;

		// Token: 0x04008380 RID: 33664
		public bool canTargetHumans = true;

		// Token: 0x04008381 RID: 33665
		public bool canTargetMechs = true;

		// Token: 0x04008382 RID: 33666
		public bool canTargetPlants;

		// Token: 0x04008383 RID: 33667
		public bool canTargetSubhumans = true;

		// Token: 0x04008384 RID: 33668
		public bool canTargetEntities = true;

		// Token: 0x04008385 RID: 33669
		public List<Faction> onlyTargetFactions;

		// Token: 0x04008386 RID: 33670
		public Predicate<TargetInfo> validator;

		// Token: 0x04008387 RID: 33671
		public bool onlyTargetFlammables;

		// Token: 0x04008388 RID: 33672
		public Thing targetSpecificThing;

		// Token: 0x04008389 RID: 33673
		public bool mustBeSelectable;

		// Token: 0x0400838A RID: 33674
		public bool neverTargetDoors;

		// Token: 0x0400838B RID: 33675
		public bool neverTargetIncapacitated;

		// Token: 0x0400838C RID: 33676
		public bool neverTargetHostileFaction;

		// Token: 0x0400838D RID: 33677
		public bool onlyTargetSameIdeo;

		// Token: 0x0400838E RID: 33678
		public bool onlyTargetThingsAffectingRegions;

		// Token: 0x0400838F RID: 33679
		public bool onlyTargetDamagedThings;

		// Token: 0x04008390 RID: 33680
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x04008391 RID: 33681
		public bool onlyTargetIncapacitatedPawns;

		// Token: 0x04008392 RID: 33682
		public bool onlyTargetColonistsOrPrisoners;

		// Token: 0x04008393 RID: 33683
		public bool onlyTargetColonistsOrPrisonersOrSlaves;

		// Token: 0x04008394 RID: 33684
		public bool onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks;

		// Token: 0x04008395 RID: 33685
		public bool onlyTargetControlledPawns;

		// Token: 0x04008396 RID: 33686
		public bool onlyTargetColonists;

		// Token: 0x04008397 RID: 33687
		public bool onlyTargetPrisonersOfColony;

		// Token: 0x04008398 RID: 33688
		public bool onlyTargetPsychicSensitive;

		// Token: 0x04008399 RID: 33689
		public bool onlyTargetAnimaTrees;

		// Token: 0x0400839A RID: 33690
		public bool canTargetBloodfeeders = true;

		// Token: 0x0400839B RID: 33691
		public bool onlyRepairableMechs;

		// Token: 0x0400839C RID: 33692
		public ThingCategory thingCategory;

		// Token: 0x0400839D RID: 33693
		public bool onlyTargetDoors;

		// Token: 0x0400839E RID: 33694
		public bool canTargetCorpses;

		// Token: 0x0400839F RID: 33695
		public bool onlyTargetCorpses;

		// Token: 0x040083A0 RID: 33696
		public int mapBoundsContractedBy;
	}
}
