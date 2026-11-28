using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014ED RID: 5357
	public static class PawnsFinder
	{
		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x060082E4 RID: 33508 RVA: 0x002EFB00 File Offset: 0x002EDD00
		public static List<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.AllMapsWorldAndTemporary_Alive);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.WorldPawns.AllPawnsDead);
				}
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.Temporary_Dead);
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result;
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x060082E5 RID: 33509 RVA: 0x002EFB70 File Offset: 0x002EDD70
		public static List<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.Temporary_Alive);
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsWorldAndTemporary_Alive_Result;
			}
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x060082E6 RID: 33510 RVA: 0x002EFBE0 File Offset: 0x002EDDE0
		public static List<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
				PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsAndWorld_Alive_Result;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x060082E7 RID: 33511 RVA: 0x002EFC44 File Offset: 0x002EDE44
		public static List<Pawn> AllMaps
		{
			get
			{
				PawnsFinder.allMaps_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawns;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Result.AddRange(maps[i].mapPawns.AllPawns);
					}
				}
				return PawnsFinder.allMaps_Result;
			}
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x060082E8 RID: 33512 RVA: 0x002EFCB4 File Offset: 0x002EDEB4
		public static IReadOnlyList<Pawn> AllMaps_Spawned
		{
			get
			{
				PawnsFinder.allMaps_Spawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawnsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Spawned_Result.AddRange(maps[i].mapPawns.AllPawnsSpawned);
					}
				}
				return PawnsFinder.allMaps_Spawned_Result;
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x060082E9 RID: 33513 RVA: 0x002EFD24 File Offset: 0x002EDF24
		public static List<Pawn> All_AliveOrDead
		{
			get
			{
				List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
				List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead = PawnsFinder.AllCaravansAndTravellingTransporters_AliveOrDead;
				if (allCaravansAndTravellingTransporters_AliveOrDead.Count == 0)
				{
					return allMapsWorldAndTemporary_AliveOrDead;
				}
				PawnsFinder.all_AliveOrDead_Result.Clear();
				PawnsFinder.all_AliveOrDead_Result.AddRange(allMapsWorldAndTemporary_AliveOrDead);
				PawnsFinder.all_AliveOrDead_Result.AddRange(allCaravansAndTravellingTransporters_AliveOrDead);
				return PawnsFinder.all_AliveOrDead_Result;
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x060082EA RID: 33514 RVA: 0x002EFD6C File Offset: 0x002EDF6C
		public static List<Pawn> Temporary
		{
			get
			{
				PawnsFinder.temporary_Result.Clear();
				List<List<Pawn>> pawnsBeingGeneratedNow = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int i = 0; i < pawnsBeingGeneratedNow.Count; i++)
				{
					PawnsFinder.temporary_Result.AddRange(pawnsBeingGeneratedNow[i]);
				}
				List<List<Thing>> thingsBeingGeneratedNow = ThingSetMaker.thingsBeingGeneratedNow;
				for (int j = 0; j < thingsBeingGeneratedNow.Count; j++)
				{
					List<Thing> list = thingsBeingGeneratedNow[j];
					for (int k = 0; k < list.Count; k++)
					{
						Pawn pawn = list[k] as Pawn;
						if (pawn != null)
						{
							PawnsFinder.temporary_Result.Add(pawn);
						}
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null)
				{
					List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
					for (int l = 0; l < startingAndOptionalPawns.Count; l++)
					{
						if (startingAndOptionalPawns[l] != null)
						{
							PawnsFinder.temporary_Result.Add(startingAndOptionalPawns[l]);
						}
					}
				}
				if (Find.World != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int m = 0; m < sites.Count; m++)
					{
						for (int n = 0; n < sites[m].parts.Count; n++)
						{
							if (sites[m].parts[n].things != null && sites[m].parts[n].things.contentsLookMode == LookMode.Deep)
							{
								ThingOwner things = sites[m].parts[n].things;
								for (int num = 0; num < things.Count; num++)
								{
									Pawn pawn2 = things[num] as Pawn;
									if (pawn2 != null)
									{
										PawnsFinder.temporary_Result.Add(pawn2);
									}
								}
							}
						}
					}
				}
				if (Find.World != null)
				{
					List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
					for (int num2 = 0; num2 < allWorldObjects.Count; num2++)
					{
						DownedRefugeeComp component = allWorldObjects[num2].GetComponent<DownedRefugeeComp>();
						if (component != null && component.pawn != null && component.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component.pawn[0]);
						}
						PrisonerWillingToJoinComp component2 = allWorldObjects[num2].GetComponent<PrisonerWillingToJoinComp>();
						if (component2 != null && component2.pawn != null && component2.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component2.pawn[0]);
						}
					}
				}
				return PawnsFinder.temporary_Result;
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x060082EB RID: 33515 RVA: 0x002EFFF8 File Offset: 0x002EE1F8
		public static List<Pawn> Temporary_Alive
		{
			get
			{
				PawnsFinder.temporary_Alive_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (!temporary[i].Dead)
					{
						PawnsFinder.temporary_Alive_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Alive_Result;
			}
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x060082EC RID: 33516 RVA: 0x002F004C File Offset: 0x002EE24C
		public static List<Pawn> Temporary_Dead
		{
			get
			{
				PawnsFinder.temporary_Dead_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (temporary[i].Dead)
					{
						PawnsFinder.temporary_Dead_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Dead_Result;
			}
		}

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x060082ED RID: 33517 RVA: 0x002F00A0 File Offset: 0x002EE2A0
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive
		{
			get
			{
				List<Pawn> allMaps = PawnsFinder.AllMaps;
				List<Pawn> allCaravansAndTravellingTransporters_Alive = PawnsFinder.AllCaravansAndTravellingTransporters_Alive;
				if (allCaravansAndTravellingTransporters_Alive.Count == 0)
				{
					return allMaps;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.AddRange(allMaps);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.AddRange(allCaravansAndTravellingTransporters_Alive);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result;
			}
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x060082EE RID: 33518 RVA: 0x002F00E8 File Offset: 0x002EE2E8
		public static IReadOnlyList<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned
		{
			get
			{
				IReadOnlyList<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				List<Pawn> allCaravansAndTravellingTransporters_Alive = PawnsFinder.AllCaravansAndTravellingTransporters_Alive;
				if (allCaravansAndTravellingTransporters_Alive.Count == 0)
				{
					return allMaps_Spawned;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.AddRange(allMaps_Spawned);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.AddRange(allCaravansAndTravellingTransporters_Alive);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result;
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x060082EF RID: 33519 RVA: 0x002F0130 File Offset: 0x002EE330
		public static List<Pawn> AllCaravansAndTravellingTransporters_Alive
		{
			get
			{
				PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Clear();
				List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead = PawnsFinder.AllCaravansAndTravellingTransporters_AliveOrDead;
				for (int i = 0; i < allCaravansAndTravellingTransporters_AliveOrDead.Count; i++)
				{
					if (!allCaravansAndTravellingTransporters_AliveOrDead[i].Dead)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Add(allCaravansAndTravellingTransporters_AliveOrDead[i]);
					}
				}
				return PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result;
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x060082F0 RID: 33520 RVA: 0x002F0184 File Offset: 0x002EE384
		public static List<Pawn> AllCaravansAndTravellingTransporters_AliveOrDead
		{
			get
			{
				PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Clear();
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int i = 0; i < caravans.Count; i++)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.AddRange(caravans[i].PawnsListForReading);
					}
					List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
					for (int j = 0; j < travellingTransporters.Count; j++)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.AddRange(travellingTransporters[j].Pawns);
					}
					if (Find.CurrentGravship != null)
					{
						foreach (Pawn pawn in Find.CurrentGravship.Pawns)
						{
							if (!pawn.SpawnedOrAnyParentSpawned)
							{
								PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Add(pawn);
							}
						}
					}
				}
				return PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x060082F1 RID: 33521 RVA: 0x002F0270 File Offset: 0x002EE470
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result;
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x060082F2 RID: 33522 RVA: 0x002F02C4 File Offset: 0x002EE4C4
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].IsSlave)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result;
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x060082F3 RID: 33523 RVA: 0x002F0324 File Offset: 0x002EE524
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result;
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x060082F4 RID: 33524 RVA: 0x002F0378 File Offset: 0x002EE578
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result;
			}
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x060082F5 RID: 33525 RVA: 0x002F03CC File Offset: 0x002EE5CC
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].IsQuestLodger())
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result;
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x060082F6 RID: 33526 RVA: 0x002F042C File Offset: 0x002EE62C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result;
			}
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x060082F7 RID: 33527 RVA: 0x002F048C File Offset: 0x002EE68C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_AliveSpawned[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result;
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x060082F8 RID: 33528 RVA: 0x002F04EC File Offset: 0x002EE6EC
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].InCryptosleep)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result;
			}
		}

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x060082F9 RID: 33529 RVA: 0x002F054C File Offset: 0x002EE74C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].Faction == ofPlayer)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result;
			}
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x060082FA RID: 33530 RVA: 0x002F05A8 File Offset: 0x002EE7A8
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].Faction == ofPlayer && !allMapsCaravansAndTravellingTransporters_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result;
			}
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x060082FB RID: 33531 RVA: 0x002F0610 File Offset: 0x002EE810
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x060082FC RID: 33532 RVA: 0x002F0664 File Offset: 0x002EE864
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result;
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x060082FD RID: 33533 RVA: 0x002F06B8 File Offset: 0x002EE8B8
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsSlaveOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result;
			}
		}

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x060082FE RID: 33534 RVA: 0x002F070C File Offset: 0x002EE90C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists;
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony;
				if (allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x060082FF RID: 33535 RVA: 0x002F0754 File Offset: 0x002EE954
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony;
				if (allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_Alive_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x06008300 RID: 33536 RVA: 0x002F079C File Offset: 0x002EE99C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x06008301 RID: 33537 RVA: 0x002F07F0 File Offset: 0x002EE9F0
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06008302 RID: 33538 RVA: 0x002F0844 File Offset: 0x002EEA44
		public static List<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColonySpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.AddRange(maps[i].mapPawns.PrisonersOfColonySpawned);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColonySpawned_Result;
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x06008303 RID: 33539 RVA: 0x002F08B4 File Offset: 0x002EEAB4
		public static List<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColony;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColony_Result.AddRange(maps[i].mapPawns.PrisonersOfColony);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x06008304 RID: 33540 RVA: 0x002F0924 File Offset: 0x002EEB24
		public static List<Pawn> AllMaps_FreeColonists
		{
			get
			{
				PawnsFinder.allMaps_FreeColonists_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonists;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonists_Result.AddRange(maps[i].mapPawns.FreeColonists);
					}
				}
				return PawnsFinder.allMaps_FreeColonists_Result;
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x06008305 RID: 33541 RVA: 0x002F0994 File Offset: 0x002EEB94
		public static List<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06008306 RID: 33542 RVA: 0x002F0A04 File Offset: 0x002EEC04
		public static List<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisonersSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisonersSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result;
			}
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x06008307 RID: 33543 RVA: 0x002F0A74 File Offset: 0x002EEC74
		public static List<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisoners);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x06008308 RID: 33544 RVA: 0x002F0AE4 File Offset: 0x002EECE4
		public static List<Pawn> AllMaps_ColonySubhumansSpawnedPlayerControlled
		{
			get
			{
				PawnsFinder.allMaps_ColonySubhumansSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.SpawnedColonySubhumansPlayerControlled;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_ColonySubhumansSpawned_Result.AddRange(maps[i].mapPawns.SpawnedColonySubhumansPlayerControlled);
					}
				}
				return PawnsFinder.allMaps_ColonySubhumansSpawned_Result;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06008309 RID: 33545 RVA: 0x002F0B54 File Offset: 0x002EED54
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i].IsColonySubhuman && !allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result;
			}
		}

		// Token: 0x0600830A RID: 33546 RVA: 0x002F0BB4 File Offset: 0x002EEDB4
		public static List<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Pawn> list;
			if (!PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Add(faction, list);
			}
			list.Clear();
			if (Current.ProgramState != ProgramState.Entry)
			{
				List<Map> maps = Find.Maps;
				if (maps.Count == 1)
				{
					return maps[0].mapPawns.SpawnedPawnsInFaction(faction);
				}
				for (int i = 0; i < maps.Count; i++)
				{
					list.AddRange(maps[i].mapPawns.SpawnedPawnsInFaction(faction));
				}
			}
			return list;
		}

		// Token: 0x0600830B RID: 33547 RVA: 0x002F0C3C File Offset: 0x002EEE3C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype(XenotypeDef def)
		{
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Clear();
			if (!ModsConfig.BiotechActive)
			{
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result;
			}
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_Colonists.Count; i++)
			{
				if (allMapsCaravansAndTravellingTransporters_Alive_Colonists[i].genes != null && allMapsCaravansAndTravellingTransporters_Alive_Colonists[i].genes.Xenotype == def)
				{
					PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_Colonists[i]);
				}
			}
			return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result;
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x0600830C RID: 33548 RVA: 0x002F0CB0 File Offset: 0x002EEEB0
		public static List<Pawn> HomeMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						if (!maps[0].IsPlayerHome)
						{
							return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
						}
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					else
					{
						for (int i = 0; i < maps.Count; i++)
						{
							if (maps[i].IsPlayerHome)
							{
								PawnsFinder.homeMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
							}
						}
					}
				}
				return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x0600830D RID: 33549 RVA: 0x002F0D44 File Offset: 0x002EEF44
		public static void Clear()
		{
			PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
			PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
			PawnsFinder.allMaps_Result.Clear();
			PawnsFinder.allMaps_Spawned_Result.Clear();
			PawnsFinder.all_AliveOrDead_Result.Clear();
			PawnsFinder.temporary_Result.Clear();
			PawnsFinder.temporary_Alive_Result.Clear();
			PawnsFinder.temporary_Dead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMaps_FreeColonists_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Clear();
			PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Clear();
		}

		// Token: 0x04005100 RID: 20736
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04005101 RID: 20737
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		// Token: 0x04005102 RID: 20738
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		// Token: 0x04005103 RID: 20739
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		// Token: 0x04005104 RID: 20740
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		// Token: 0x04005105 RID: 20741
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04005106 RID: 20742
		private static List<Pawn> temporary_Result = new List<Pawn>();

		// Token: 0x04005107 RID: 20743
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		// Token: 0x04005108 RID: 20744
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		// Token: 0x04005109 RID: 20745
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Result = new List<Pawn>();

		// Token: 0x0400510A RID: 20746
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_Result = new List<Pawn>();

		// Token: 0x0400510B RID: 20747
		private static List<Pawn> allCaravansAndTravellingTransporters_Alive_Result = new List<Pawn>();

		// Token: 0x0400510C RID: 20748
		private static List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x0400510D RID: 20749
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result = new List<Pawn>();

		// Token: 0x0400510E RID: 20750
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result = new List<Pawn>();

		// Token: 0x0400510F RID: 20751
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result = new List<Pawn>();

		// Token: 0x04005110 RID: 20752
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result = new List<Pawn>();

		// Token: 0x04005111 RID: 20753
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result = new List<Pawn>();

		// Token: 0x04005112 RID: 20754
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result = new List<Pawn>();

		// Token: 0x04005113 RID: 20755
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result = new List<Pawn>();

		// Token: 0x04005114 RID: 20756
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x04005115 RID: 20757
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result = new List<Pawn>();

		// Token: 0x04005116 RID: 20758
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x04005117 RID: 20759
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x04005118 RID: 20760
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x04005119 RID: 20761
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result = new List<Pawn>();

		// Token: 0x0400511A RID: 20762
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x0400511B RID: 20763
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x0400511C RID: 20764
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x0400511D RID: 20765
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x0400511E RID: 20766
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		// Token: 0x0400511F RID: 20767
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x04005120 RID: 20768
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		// Token: 0x04005121 RID: 20769
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		// Token: 0x04005122 RID: 20770
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		// Token: 0x04005123 RID: 20771
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x04005124 RID: 20772
		private static List<Pawn> allMaps_ColonySubhumansSpawned_Result = new List<Pawn>();

		// Token: 0x04005125 RID: 20773
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result = new List<Pawn>();

		// Token: 0x04005126 RID: 20774
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04005127 RID: 20775
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result = new List<Pawn>();

		// Token: 0x04005128 RID: 20776
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
