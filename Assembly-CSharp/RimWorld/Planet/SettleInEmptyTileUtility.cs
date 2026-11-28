using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023A1 RID: 9121
	public static class SettleInEmptyTileUtility
	{
		// Token: 0x0600E2F6 RID: 58102 RVA: 0x00546860 File Offset: 0x00544A60
		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.");
				return;
			}
			if (Find.AnyPlayerHomeMap == null)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					MemoryThoughtHandler memoryThoughtHandler;
					if (needs == null)
					{
						memoryThoughtHandler = null;
					}
					else
					{
						Need_Mood mood = needs.mood;
						if (mood == null)
						{
							memoryThoughtHandler = null;
						}
						else
						{
							ThoughtHandler thoughts = mood.thoughts;
							memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
						}
					}
					MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
					if (memoryThoughtHandler2 != null)
					{
						memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
						memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
						if (pawn.IsFreeNonSlaveColonist)
						{
							memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
						}
					}
				}
			}
			Settlement newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null, null, false);
			}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			LongEventHandler.QueueLongEvent(delegate()
			{
				Map map = newHome.Map;
				Thing t = caravan.PawnsListForReading[0];
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map).CellCount >= 600);
				newHome.Notify_MyMapSettled(map);
				CameraJumper.TryJump(t, CameraJumper.MovementMode.Pan);
			}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
		}

		// Token: 0x0600E2F7 RID: 58103 RVA: 0x005469A4 File Offset: 0x00544BA4
		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				PlanetTile tile = caravan.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate()
					{
						SettleInEmptyTileUtility.Settle(caravan);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction, null, null);
			};
			SettleInEmptyTileUtility.tmpSettleFailReason.Length = 0;
			if (!TileFinder.IsValidTileForNewSettlement(caravan.Tile, SettleInEmptyTileUtility.tmpSettleFailReason, false))
			{
				command_Settle.Disable(SettleInEmptyTileUtility.tmpSettleFailReason.ToString());
			}
			else if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			return command_Settle;
		}

		// Token: 0x0600E2F8 RID: 58104 RVA: 0x00546A7C File Offset: 0x00544C7C
		public static Command SetupCamp(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCamp".Translate();
			command_Action.defaultDesc = "CommandCampDesc".Translate();
			command_Action.icon = SettleUtility.CreateCampCommandTex;
			Action <>9__1;
			command_Action.action = delegate()
			{
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						Map map = GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, WorldObjectDefOf.Camp.overrideMapSize ?? Find.World.info.initialMapSize, WorldObjectDefOf.Camp, null, false);
						map.Parent.SetFaction(caravan.Faction);
						Thing t = caravan.PawnsListForReading[0];
						CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DoNotDrop, false, (IntVec3 x) => x.GetRoom(map).CellCount >= 600 && !x.GetTerrain(map).IsWater);
						TimedDetectionRaids component = map.Parent.GetComponent<TimedDetectionRaids>();
						if (component != null)
						{
							component.StartDetectionCountdown(240000, 60000);
						}
						CameraJumper.TryJump(t, CameraJumper.MovementMode.Pan);
					});
				}
				LongEventHandler.QueueLongEvent(action, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			};
			if (!SettleInEmptyTileUtility.CanCreateMapAt(caravan.Tile, false) || Find.WorldObjects.AnyMapParentAt(caravan.Tile))
			{
				command_Action.Disable("CommandCampFailExistingWorldObject".Translate());
			}
			return command_Action;
		}

		// Token: 0x0600E2F9 RID: 58105 RVA: 0x00546B24 File Offset: 0x00544D24
		public static bool CanCreateMapAt(PlanetTile tile, bool forGravship = false)
		{
			using (IEnumerator<WorldObject> enumerator = Find.WorldObjects.ObjectsAt(tile).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.canHaveMap)
					{
						return false;
					}
				}
			}
			return !Find.WorldObjects.AnyMapParentAt(tile) && Current.Game.FindMap(tile) == null;
		}

		// Token: 0x040090DA RID: 37082
		private const int MinStartingLocCellsCount = 600;

		// Token: 0x040090DB RID: 37083
		private static StringBuilder tmpSettleFailReason = new StringBuilder();
	}
}
