using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000297 RID: 663
	public sealed class MapPawns
	{
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x00071490 File Offset: 0x0006F690
		public List<Pawn> AllPawns
		{
			get
			{
				List<Pawn> allPawnsUnspawned = this.AllPawnsUnspawned;
				if (allPawnsUnspawned.Count == 0)
				{
					return this.pawnsSpawned;
				}
				this.allPawnsResult.Clear();
				this.allPawnsResult.AddRange(this.pawnsSpawned);
				this.allPawnsResult.AddRange(allPawnsUnspawned);
				return this.allPawnsResult;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000714E4 File Offset: 0x0006F6E4
		public List<Pawn> AllPawnsUnspawned
		{
			get
			{
				MapPawns.AssertMainThread();
				this.allPawnsUnspawnedResult.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.allPawnsUnspawnedResult, true, null, false);
				for (int i = this.allPawnsUnspawnedResult.Count - 1; i >= 0; i--)
				{
					if (this.allPawnsUnspawnedResult[i].Dead)
					{
						this.allPawnsUnspawnedResult.RemoveAt(i);
					}
				}
				return this.allPawnsUnspawnedResult;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00071559 File Offset: 0x0006F759
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00071568 File Offset: 0x0006F768
		public List<Pawn> PrisonersOfColony
		{
			get
			{
				this.prisonersOfColonyResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsPrisonerOfColony)
					{
						this.prisonersOfColonyResult.Add(allPawns[i]);
					}
				}
				return this.prisonersOfColonyResult;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x000715C0 File Offset: 0x0006F7C0
		public List<Pawn> AllHumanlike
		{
			get
			{
				this.humanlikePawnsResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].RaceProps.Humanlike)
					{
						this.humanlikePawnsResult.Add(allPawns[i]);
					}
				}
				return this.humanlikePawnsResult;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0007161C File Offset: 0x0006F81C
		public List<Pawn> AllHumanlikeSpawned
		{
			get
			{
				this.humanlikeSpawnedPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].RaceProps.Humanlike)
					{
						this.humanlikeSpawnedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.humanlikeSpawnedPawnsResult;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00071678 File Offset: 0x0006F878
		public List<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> freeColonists = this.FreeColonists;
				List<Pawn> prisonersOfColony = this.PrisonersOfColony;
				if (prisonersOfColony.Count == 0)
				{
					return freeColonists;
				}
				this.freeColonistsAndPrisonersResult.Clear();
				this.freeColonistsAndPrisonersResult.AddRange(freeColonists);
				this.freeColonistsAndPrisonersResult.AddRange(prisonersOfColony);
				return this.freeColonistsAndPrisonersResult;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x000716C8 File Offset: 0x0006F8C8
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.");
					return 3;
				}
				int num = 0;
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00071716 File Offset: 0x0006F916
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00071723 File Offset: 0x0006F923
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00071730 File Offset: 0x0006F930
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0007173D File Offset: 0x0006F93D
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0007174A File Offset: 0x0006F94A
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.FreeColonistsCount + this.PrisonersOfColonyCount;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x0007175C File Offset: 0x0006F95C
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				foreach (Pawn pawn in this.pawnsSpawned)
				{
					if (MapPawns.IsValidColonyPawn(pawn))
					{
						return true;
					}
					Pawn_RelationsTracker relations = pawn.relations;
					if (((relations != null) ? relations.relativeInvolvedInRescueQuest : null) != null)
					{
						return true;
					}
					if (pawn.Faction == ofPlayer || pawn.HostFaction == ofPlayer)
					{
						Job curJob = pawn.CurJob;
						if (curJob != null && curJob.exitMapOnArrival)
						{
							return true;
						}
						if (pawn.health.hediffSet.InLabor(true))
						{
							return true;
						}
					}
					if (CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null && !pawn.Downed)
					{
						return true;
					}
					if (ModsConfig.BiotechActive && pawn.IsColonyMechPlayerControlled && !pawn.Downed && pawn.GetOverseer() != null)
					{
						return true;
					}
				}
				foreach (Pawn pawn2 in this.AllPawnsUnspawned)
				{
					if (pawn2.SpawnedOrAnyParentSpawned && MapPawns.IsValidColonyPawn(pawn2))
					{
						return true;
					}
				}
				foreach (Thing thing in this.map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && MapPawns.IsValidColonyPawn(corpse.InnerPawn))
					{
						return true;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int i = 0; i < list.Count; i++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[i], false);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int j = 0; j < this.tmpThings.Count; j++)
						{
							Pawn pawn3 = this.tmpThings[j] as Pawn;
							if (pawn3 != null && !pawn3.Dead && !pawn3.Downed && (pawn3.IsColonist || pawn3.IsColonyMech))
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				using (IEnumerator<Map> enumerator3 = this.map.ChildPocketMaps.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.mapPawns.AnyPawnBlockingMapRemoval)
						{
							return true;
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00071A44 File Offset: 0x0006FC44
		private static bool IsValidColonyPawn(Pawn pawn)
		{
			return ((!pawn.Dead || pawn.HasDeathRefusalOrResurrecting) && pawn.IsColonist) || ((!pawn.Dead || pawn.HasDeathRefusalOrResurrecting) && pawn.IsColonySubhuman && pawn.mutant.Def.canTravelInCaravan);
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x00071A98 File Offset: 0x0006FC98
		public IReadOnlyList<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00071AA0 File Offset: 0x0006FCA0
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00071AB0 File Offset: 0x0006FCB0
		public List<Pawn> FreeAdultColonistsSpawned
		{
			get
			{
				this.freeAdultColonistsSpawnedResult.Clear();
				foreach (Pawn pawn in this.FreeColonistsSpawned)
				{
					if (pawn.DevelopmentalStage.Adult())
					{
						this.freeAdultColonistsSpawnedResult.Add(pawn);
					}
				}
				return this.freeAdultColonistsSpawnedResult;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00071B28 File Offset: 0x0006FD28
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x00071B30 File Offset: 0x0006FD30
		public List<Pawn> SlavesOfColonySpawned
		{
			get
			{
				return this.slavesOfColonySpawned;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x00071B38 File Offset: 0x0006FD38
		public List<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				MapPawns.AssertMainThread();
				List<Pawn> freeColonistsSpawned = this.FreeColonistsSpawned;
				List<Pawn> list = this.PrisonersOfColonySpawned;
				if (list.Count == 0)
				{
					return freeColonistsSpawned;
				}
				this.freeColonistsAndPrisonersSpawnedResult.Clear();
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(freeColonistsSpawned);
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(list);
				return this.freeColonistsAndPrisonersSpawnedResult;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x00071B8C File Offset: 0x0006FD8C
		public List<Pawn> SpawnedPawnsWithAnyHediff
		{
			get
			{
				this.spawnedPawnsWithAnyHediffResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedPawnsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithAnyHediffResult;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x00071BF4 File Offset: 0x0006FDF4
		public List<Pawn> SpawnedHumanlikesWithAnyHediff
		{
			get
			{
				this.spawnedHumanlikesWithAnyHediffResult.Clear();
				List<Pawn> allHumanlikeSpawned = this.AllHumanlikeSpawned;
				for (int i = 0; i < allHumanlikeSpawned.Count; i++)
				{
					if (allHumanlikeSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedHumanlikesWithAnyHediffResult.Add(allHumanlikeSpawned[i]);
					}
				}
				return this.spawnedHumanlikesWithAnyHediffResult;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00071C5C File Offset: 0x0006FE5C
		public List<Pawn> SpawnedAnimalsWithAnyHediff
		{
			get
			{
				this.spawnedAnimalsWithAnyHediffResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].IsAnimal && allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedAnimalsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedAnimalsWithAnyHediffResult;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00071CD0 File Offset: 0x0006FED0
		public List<Pawn> SpawnedHungryPawns
		{
			get
			{
				this.spawnedHungryPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (FeedPatientUtility.IsHungry(allPawnsSpawned[i]))
					{
						this.spawnedHungryPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedHungryPawnsResult;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00071D28 File Offset: 0x0006FF28
		public List<Pawn> SpawnedPawnsWithMiscNeeds
		{
			get
			{
				this.spawnedPawnsWithMiscNeedsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (!allPawnsSpawned[i].needs.MiscNeeds.NullOrEmpty<Need>())
					{
						this.spawnedPawnsWithMiscNeedsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithMiscNeedsResult;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x00071D88 File Offset: 0x0006FF88
		public List<Pawn> ColonyAnimals
		{
			get
			{
				this.colonyAnimalsResult.Clear();
				List<Pawn> list = this.PawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsAnimal)
					{
						this.colonyAnimalsResult.Add(list[i]);
					}
				}
				return this.colonyAnimalsResult;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00071DE4 File Offset: 0x0006FFE4
		public List<Pawn> SpawnedColonyAnimals
		{
			get
			{
				this.spawnedColonyAnimalsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsAnimal)
					{
						this.spawnedColonyAnimalsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyAnimalsResult;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00071E40 File Offset: 0x00070040
		public List<Pawn> SpawnedColonyMechs
		{
			get
			{
				this.spawnedColonyMechsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonyMech)
					{
						this.spawnedColonyMechsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyMechsResult;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00071E9C File Offset: 0x0007009C
		public List<Pawn> ColonySubhumansControllable
		{
			get
			{
				this.colonySubhumansResult.Clear();
				List<Pawn> list = this.PawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonySubhuman && list[i].mutant.Def.canBeDrafted)
					{
						this.colonySubhumansResult.Add(list[i]);
					}
				}
				return this.colonySubhumansResult;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00071F10 File Offset: 0x00070110
		public List<Pawn> SpawnedColonySubhumansPlayerControlled
		{
			get
			{
				this.spawnedColonySubhumansResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonySubhumanPlayerControlled)
					{
						this.spawnedColonySubhumansResult.Add(list[i]);
					}
				}
				return this.spawnedColonySubhumansResult;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00071F6C File Offset: 0x0007016C
		public List<Pawn> SpawnedDownedPawns
		{
			get
			{
				this.spawnedDownedPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Downed)
					{
						this.spawnedDownedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedDownedPawnsResult;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00071FC4 File Offset: 0x000701C4
		public List<Pawn> SpawnedPawnsWhoShouldHaveSurgeryDoneNow
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
					{
						this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0007201C File Offset: 0x0007021C
		public List<Pawn> SpawnedPawnsWhoShouldHaveInventoryUnloaded
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].inventory.UnloadEverything)
					{
						this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00072077 File Offset: 0x00070277
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x00072084 File Offset: 0x00070284
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00072091 File Offset: 0x00070291
		public int FreeAdultColonistsSpawnedCount
		{
			get
			{
				return this.FreeAdultColonistsSpawned.Count;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0007209E File Offset: 0x0007029E
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x000720AB File Offset: 0x000702AB
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x000720B8 File Offset: 0x000702B8
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x000720F8 File Offset: 0x000702F8
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], true);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x000721DC File Offset: 0x000703DC
		private static void AssertMainThread()
		{
			if (!UnityData.IsInMainThread && !LongEventHandler.AnyEventNowOrWaiting)
			{
				throw new Exception("Accessing map pawns off main thread - this is never allowed due to list pooling and will result in modification exceptions elsewhere in code.");
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000721F8 File Offset: 0x000703F8
		private static IThingHolder PlayerEjectablePodHolder(Thing thing, bool includeCryptosleepCaskets = true)
		{
			Building_CryptosleepCasket building_CryptosleepCasket = thing as Building_CryptosleepCasket;
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			if ((includeCryptosleepCaskets && building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || thing is IActiveTransporter || thing is PawnFlyer || thing is Building_Enterable || compTransporter != null || compBiosculpterPod != null)
			{
				IThingHolder thingHolder = compTransporter;
				IThingHolder result;
				if ((result = thingHolder) == null)
				{
					thingHolder = compBiosculpterPod;
					result = (thingHolder ?? (thing as IThingHolder));
				}
				return result;
			}
			return null;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00072266 File Offset: 0x00070466
		public int SlavesAndPrisonersOfColonySpawnedCount
		{
			get
			{
				return this.SlavesAndPrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00072274 File Offset: 0x00070474
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x000722B0 File Offset: 0x000704B0
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x000722EB File Offset: 0x000704EB
		public List<Pawn> SlavesAndPrisonersOfColonySpawned
		{
			get
			{
				this.slavesAndPrisonersOfColonySpawnedResult.Clear();
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.prisonersOfColonySpawned);
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.slavesOfColonySpawned);
				return this.slavesAndPrisonersOfColonySpawnedResult;
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00072320 File Offset: 0x00070520
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000724A8 File Offset: 0x000706A8
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				this.pawnsInFactionSpawned.GetPawnList(allFactionsListForReading[i]);
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x000724E4 File Offset: 0x000706E4
		public List<Faction> FactionsOnMap()
		{
			this.tmpFactionsOnMap.Clear();
			foreach (Faction faction in this.pawnsInFactionResult.KnownFactions())
			{
				if (this.pawnsInFactionResult.GetPawnList(faction).Count > 0)
				{
					this.tmpFactionsOnMap.Add(faction);
				}
			}
			return this.tmpFactionsOnMap;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00072560 File Offset: 0x00070760
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			List<Pawn> pawnList = this.pawnsInFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x000725B5 File Offset: 0x000707B5
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			return this.pawnsInFactionSpawned.GetPawnList(faction);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x000725CC File Offset: 0x000707CC
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if ((!ModsConfig.AnomalyActive || !allPawns[i].IsSubhuman) && allPawns[i].Faction == faction && (allPawns[i].HostFaction == null || allPawns[i].IsSlave) && allPawns[i].RaceProps.Humanlike)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00072668 File Offset: 0x00070868
		public List<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesSpawnedOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> list = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if ((!ModsConfig.AnomalyActive || !list[i].IsSubhuman) && list[i].HostFaction == null && list[i].RaceProps.Humanlike)
				{
					pawnList.Add(list[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x000726E8 File Offset: 0x000708E8
		public List<Pawn> SpawnedBabiesInFaction(Faction faction)
		{
			List<Pawn> pawnList = this.spawnedBabiesInFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> list = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].DevelopmentalStage.Baby())
				{
					pawnList.Add(list[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00072744 File Offset: 0x00070944
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				string[] array = new string[5];
				array[0] = "Tried to register dead pawn ";
				array[1] = ((p != null) ? p.ToString() : null);
				array[2] = " in ";
				int num = 3;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[4] = ".";
				Log.Warning(string.Concat(array));
				return;
			}
			if (!p.Spawned)
			{
				string[] array2 = new string[5];
				array2[0] = "Tried to register despawned pawn ";
				array2[1] = ((p != null) ? p.ToString() : null);
				array2[2] = " in ";
				int num2 = 3;
				Type type2 = base.GetType();
				array2[num2] = ((type2 != null) ? type2.ToString() : null);
				array2[4] = ".";
				Log.Warning(string.Concat(array2));
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + ((p != null) ? p.ToString() : null) + " but his Map is not this one.");
				return;
			}
			if (!p.mindState.Active)
			{
				return;
			}
			this.EnsureFactionsListsInit();
			if (!this.pawnsSpawned.Contains(p))
			{
				this.pawnsSpawned.Add(p);
			}
			if (p.Faction != null)
			{
				List<Pawn> pawnList = this.pawnsInFactionSpawned.GetPawnList(p.Faction);
				if (!pawnList.Contains(p))
				{
					pawnList.Add(p);
					if (p.Faction == Faction.OfPlayer)
					{
						pawnList.InsertionSort(delegate(Pawn a, Pawn b)
						{
							Pawn_PlayerSettings playerSettings = a.playerSettings;
							int num3 = (playerSettings != null) ? playerSettings.joinTick : 0;
							Pawn_PlayerSettings playerSettings2 = b.playerSettings;
							int value = (playerSettings2 != null) ? playerSettings2.joinTick : 0;
							return num3.CompareTo(value);
						});
					}
				}
			}
			if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
			{
				this.prisonersOfColonySpawned.Add(p);
			}
			if (p.IsSlaveOfColony && !this.slavesOfColonySpawned.Contains(p))
			{
				this.slavesOfColonySpawned.Add(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00072904 File Offset: 0x00070B04
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction faction = allFactionsListForReading[i];
				this.pawnsInFactionSpawned.GetPawnList(faction).Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.slavesOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0007297B File Offset: 0x00070B7B
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x000729A7 File Offset: 0x00070BA7
		public List<Pawn> SpawnedShamblers
		{
			get
			{
				return this.shamblersSpawned;
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x000729AF File Offset: 0x00070BAF
		public void RegisterShambler(Pawn p)
		{
			if (!ModLister.CheckAnomaly("Shambler registration"))
			{
				return;
			}
			if (!this.shamblersSpawned.Contains(p))
			{
				this.shamblersSpawned.Add(p);
			}
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x000729D8 File Offset: 0x00070BD8
		public void DeregisterShambler(Pawn p)
		{
			if (!ModLister.CheckAnomaly("Shambler deregistration"))
			{
				return;
			}
			this.shamblersSpawned.Remove(p);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x000729F4 File Offset: 0x00070BF4
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00072A0C File Offset: 0x00070C0C
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "    ";
				Pawn pawn2 = pawn;
				stringBuilder2.AppendLine(str + ((pawn2 != null) ? pawn2.ToString() : null));
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn3 in this.AllPawnsUnspawned)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "    ";
				Pawn pawn4 = pawn3;
				stringBuilder3.AppendLine(str2 + ((pawn4 != null) ? pawn4.ToString() : null));
			}
			foreach (Faction faction in this.pawnsInFactionSpawned.KnownFactions())
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + faction.ToStringSafe<Faction>() + "]");
				foreach (Pawn pawn5 in this.pawnsInFactionSpawned.GetPawnList(faction))
				{
					StringBuilder stringBuilder4 = stringBuilder;
					string str3 = "    ";
					Pawn pawn6 = pawn5;
					stringBuilder4.AppendLine(str3 + ((pawn6 != null) ? pawn6.ToString() : null));
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn7 in this.prisonersOfColonySpawned)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				string str4 = "    ";
				Pawn pawn8 = pawn7;
				stringBuilder5.AppendLine(str4 + ((pawn8 != null) ? pawn8.ToString() : null));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0400117A RID: 4474
		private Map map;

		// Token: 0x0400117B RID: 4475
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x0400117C RID: 4476
		private MapPawns.FactionDictionary pawnsInFactionSpawned = new MapPawns.FactionDictionary();

		// Token: 0x0400117D RID: 4477
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x0400117E RID: 4478
		private List<Pawn> slavesOfColonySpawned = new List<Pawn>();

		// Token: 0x0400117F RID: 4479
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04001180 RID: 4480
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x04001181 RID: 4481
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x04001182 RID: 4482
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x04001183 RID: 4483
		private List<Pawn> humanlikePawnsResult = new List<Pawn>();

		// Token: 0x04001184 RID: 4484
		private List<Pawn> humanlikeSpawnedPawnsResult = new List<Pawn>();

		// Token: 0x04001185 RID: 4485
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x04001186 RID: 4486
		private readonly List<Pawn> freeAdultColonistsSpawnedResult = new List<Pawn>();

		// Token: 0x04001187 RID: 4487
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x04001188 RID: 4488
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x04001189 RID: 4489
		private List<Pawn> spawnedHumanlikesWithAnyHediffResult = new List<Pawn>();

		// Token: 0x0400118A RID: 4490
		private List<Pawn> spawnedAnimalsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x0400118B RID: 4491
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x0400118C RID: 4492
		private List<Pawn> spawnedPawnsWithMiscNeedsResult = new List<Pawn>();

		// Token: 0x0400118D RID: 4493
		private List<Pawn> colonyAnimalsResult = new List<Pawn>();

		// Token: 0x0400118E RID: 4494
		private List<Pawn> spawnedColonyAnimalsResult = new List<Pawn>();

		// Token: 0x0400118F RID: 4495
		private List<Pawn> spawnedColonyMechsResult = new List<Pawn>();

		// Token: 0x04001190 RID: 4496
		private List<Pawn> colonySubhumansResult = new List<Pawn>();

		// Token: 0x04001191 RID: 4497
		private List<Pawn> spawnedColonySubhumansResult = new List<Pawn>();

		// Token: 0x04001192 RID: 4498
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x04001193 RID: 4499
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x04001194 RID: 4500
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x04001195 RID: 4501
		private List<Pawn> slavesAndPrisonersOfColonySpawnedResult = new List<Pawn>();

		// Token: 0x04001196 RID: 4502
		private List<Faction> tmpFactionsOnMap = new List<Faction>(16);

		// Token: 0x04001197 RID: 4503
		private MapPawns.FactionDictionary pawnsInFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x04001198 RID: 4504
		private MapPawns.FactionDictionary freeHumanlikesOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x04001199 RID: 4505
		private MapPawns.FactionDictionary freeHumanlikesSpawnedOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x0400119A RID: 4506
		private MapPawns.FactionDictionary spawnedBabiesInFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x0400119B RID: 4507
		private List<Pawn> shamblersSpawned = new List<Pawn>();

		// Token: 0x020025F1 RID: 9713
		private class FactionDictionary
		{
			// Token: 0x0600F17B RID: 61819 RVA: 0x0058B6E8 File Offset: 0x005898E8
			public List<Pawn> GetPawnList(Faction faction)
			{
				MapPawns.AssertMainThread();
				List<Pawn> list;
				List<Pawn> list2;
				if (faction == null)
				{
					list = this.nullFactionPawns;
				}
				else if (this.pawnList.TryGetValue(faction, out list2))
				{
					list = list2;
				}
				else
				{
					list = (this.pawnList[faction] = new List<Pawn>(32));
				}
				if (list.Count > 0)
				{
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (list[i] == null)
						{
							list.RemoveAt(i);
						}
					}
				}
				return list;
			}

			// Token: 0x0600F17C RID: 61820 RVA: 0x0058B75C File Offset: 0x0058995C
			public IEnumerable<Faction> KnownFactions()
			{
				return this.pawnList.Keys.Concat(null);
			}

			// Token: 0x04009BA4 RID: 39844
			private Dictionary<Faction, List<Pawn>> pawnList = new Dictionary<Faction, List<Pawn>>(16);

			// Token: 0x04009BA5 RID: 39845
			private List<Pawn> nullFactionPawns = new List<Pawn>(32);
		}
	}
}
