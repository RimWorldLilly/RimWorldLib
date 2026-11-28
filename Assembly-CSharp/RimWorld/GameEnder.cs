using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001026 RID: 4134
	public sealed class GameEnder : IExposable
	{
		// Token: 0x06006823 RID: 26659 RVA: 0x0024D25C File Offset: 0x0024B45C
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
			Scribe_Values.Look<int>(ref this.newWanderersCreatedTick, "newWanderersCreatedTick", -99999, false);
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x0024D298 File Offset: 0x0024B498
		public void CheckOrUpdateGameOver()
		{
			if (Find.TickManager.TicksGame < 300)
			{
				return;
			}
			if (ShipCountdown.CountingDown)
			{
				this.gameEnding = false;
				return;
			}
			if (ModsConfig.OdysseyActive && WorldComponent_GravshipController.CutsceneInProgress)
			{
				this.gameEnding = false;
				return;
			}
			if (Find.CurrentGravship != null)
			{
				this.gameEnding = false;
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount >= 1)
				{
					this.gameEnding = false;
					return;
				}
			}
			for (int j = 0; j < maps.Count; j++)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = maps[j].mapPawns.AllPawnsSpawned;
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].carryTracker != null)
					{
						Pawn pawn = allPawnsSpawned[k].carryTracker.CarriedThing as Pawn;
						if (pawn != null && pawn.IsFreeColonist)
						{
							this.gameEnding = false;
							return;
						}
					}
				}
			}
			if (ModsConfig.AnomalyActive && DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal())
			{
				this.gameEnding = false;
				return;
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int l = 0; l < caravans.Count; l++)
			{
				if (this.IsPlayerControlledWithFreeColonist(caravans[l]))
				{
					this.gameEnding = false;
					return;
				}
			}
			List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
			for (int m = 0; m < travellingTransporters.Count; m++)
			{
				if (travellingTransporters[m].PodsHaveAnyFreeColonist)
				{
					this.gameEnding = false;
					return;
				}
			}
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				return;
			}
			if (ModsConfig.AnomalyActive)
			{
				for (int n = 0; n < maps.Count; n++)
				{
					List<Pawn> allPawnsUnspawned = maps[n].mapPawns.AllPawnsUnspawned;
					for (int num = 0; num < allPawnsUnspawned.Count; num++)
					{
						Pawn pawn2 = allPawnsUnspawned[num];
						if (pawn2.IsColonist && pawn2.HostFaction == null && pawn2.ParentHolder is CompDevourer)
						{
							this.gameEnding = false;
							return;
						}
					}
				}
			}
			if (this.gameEnding)
			{
				return;
			}
			this.gameEnding = true;
			this.ticksToGameOver = 400;
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x0024D4C0 File Offset: 0x0024B6C0
		public bool CanSpawnNewWanderers()
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Dead)
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && (DeathRefusalUtility.HasPlayerControlledDeathRefusal(pawn) || DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal()))
				{
					return false;
				}
			}
			return !ModsConfig.AnomalyActive || !Find.Anomaly.VoidAwakeningActive();
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x0024D558 File Offset: 0x0024B758
		public void GameEndTick()
		{
			if (this.gameEnding)
			{
				this.ticksToGameOver--;
				if (this.ticksToGameOver == 0)
				{
					if (Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded) == null)
					{
						Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("GameOver".Translate(), "GameOverEveryoneDead".Translate(), LetterDefOf.GameEnded, null, null), null, 0, true);
					}
				}
				if (Mathf.Abs(this.ticksToGameOver) == 20000 && this.CanSpawnNewWanderers())
				{
					Letter letter3 = Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded);
					ChoiceLetter choiceLetter = LetterMaker.MakeLetter("GameOverCreateNewWanderers".Translate(), "GameOverCreateNewWanderersText".Translate(), LetterDefOf.GameEnded, null, null);
					Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
					Find.LetterStack.RemoveLetter(letter3);
					choiceLetter.arrivalTick = letter3.arrivalTick;
					return;
				}
			}
			else
			{
				Letter letter2 = Find.LetterStack.LettersListForReading.Find((Letter let) => let.def == LetterDefOf.GameEnded);
				if (letter2 != null)
				{
					Find.LetterStack.RemoveLetter(letter2);
				}
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x0024D6B8 File Offset: 0x0024B8B8
		private bool IsPlayerControlledWithFreeColonist(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.IsColonist && pawn.HostFaction == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004256 RID: 16982
		public bool gameEnding;

		// Token: 0x04004257 RID: 16983
		private int ticksToGameOver = -1;

		// Token: 0x04004258 RID: 16984
		public int newWanderersCreatedTick = -99999;

		// Token: 0x04004259 RID: 16985
		private const int GameEndCountdownDuration = 400;

		// Token: 0x0400425A RID: 16986
		public const int NewWanderersDelay = 20000;

		// Token: 0x0400425B RID: 16987
		public const int NewWanderersRespiteDays = 5;
	}
}
