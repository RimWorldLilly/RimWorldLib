using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002397 RID: 9111
	public class Caravan_PathFollower : IExposable
	{
		// Token: 0x170027DC RID: 10204
		// (get) Token: 0x0600E29D RID: 58013 RVA: 0x00542575 File Offset: 0x00540775
		public PlanetTile Destination
		{
			get
			{
				return this.destTile;
			}
		}

		// Token: 0x170027DD RID: 10205
		// (get) Token: 0x0600E29E RID: 58014 RVA: 0x0054257D File Offset: 0x0054077D
		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		// Token: 0x170027DE RID: 10206
		// (get) Token: 0x0600E29F RID: 58015 RVA: 0x00542594 File Offset: 0x00540794
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.Paused && !this.caravan.CantMove;
			}
		}

		// Token: 0x170027DF RID: 10207
		// (get) Token: 0x0600E2A0 RID: 58016 RVA: 0x005425B6 File Offset: 0x005407B6
		public CaravanArrivalAction ArrivalAction
		{
			get
			{
				if (!this.Moving)
				{
					return null;
				}
				return this.arrivalAction;
			}
		}

		// Token: 0x170027E0 RID: 10208
		// (get) Token: 0x0600E2A1 RID: 58017 RVA: 0x005425C8 File Offset: 0x005407C8
		// (set) Token: 0x0600E2A2 RID: 58018 RVA: 0x005425DC File Offset: 0x005407DC
		public bool Paused
		{
			get
			{
				return this.Moving && this.paused;
			}
			set
			{
				if (value == this.paused)
				{
					return;
				}
				if (!value)
				{
					this.paused = false;
				}
				else if (!this.Moving)
				{
					Log.Error("Tried to pause caravan movement of " + this.caravan.ToStringSafe<Caravan>() + " but it's not moving.");
				}
				else
				{
					this.paused = true;
				}
				this.caravan.Notify_DestinationOrPauseStatusChanged();
			}
		}

		// Token: 0x0600E2A3 RID: 58019 RVA: 0x0054263A File Offset: 0x0054083A
		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E2A4 RID: 58020 RVA: 0x0054266C File Offset: 0x0054086C
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.nextTile, "nextTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.previousTileForDrawingIfInDoubt, "previousTileForDrawingIfInDoubt", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeft, "nextTileCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextTileCostTotal, "nextTileCostTotal", 0f, false);
			Scribe_Values.Look<PlanetTile>(ref this.destTile, "destTile", default(PlanetTile), false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Current.ProgramState != ProgramState.Entry && this.moving && !this.StartPath(this.destTile, this.arrivalAction, true, false))
			{
				this.StopDead();
			}
		}

		// Token: 0x0600E2A5 RID: 58021 RVA: 0x00542760 File Offset: 0x00540960
		public bool StartPath(PlanetTile destTile, CaravanArrivalAction arrivalAction, bool repathImmediately = false, bool resetPauseStatus = true)
		{
			this.caravan.autoJoinable = false;
			if (resetPauseStatus)
			{
				this.paused = false;
			}
			if (arrivalAction != null && !arrivalAction.StillValid(this.caravan, destTile))
			{
				return false;
			}
			if (!this.IsPassable(this.caravan.Tile) && !this.TryRecoverFromUnwalkablePosition())
			{
				return false;
			}
			if (this.moving && this.curPath != null && this.destTile == destTile)
			{
				this.arrivalAction = arrivalAction;
				return true;
			}
			if (!this.caravan.CanReach(destTile))
			{
				this.PatherFailed();
				return false;
			}
			this.destTile = destTile;
			this.arrivalAction = arrivalAction;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
			if (!this.nextTile.Valid || !this.IsNextTilePassable())
			{
				this.nextTile = this.caravan.Tile;
				this.nextTileCostLeft = 0f;
				this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return true;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			if (repathImmediately && this.TrySetNewPath() && this.nextTileCostLeft <= 0f && this.moving)
			{
				this.TryEnterNextPathTile();
			}
			return true;
		}

		// Token: 0x0600E2A6 RID: 58022 RVA: 0x005428A4 File Offset: 0x00540AA4
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.paused = false;
			this.nextTile = this.caravan.Tile;
			this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			this.arrivalAction = null;
			this.nextTileCostLeft = 0f;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
		}

		// Token: 0x0600E2A7 RID: 58023 RVA: 0x00542914 File Offset: 0x00540B14
		public void PatherTickInterval(int delta)
		{
			if (this.moving && this.arrivalAction != null && !this.arrivalAction.StillValid(this.caravan, this.Destination))
			{
				string failMessage = this.arrivalAction.StillValid(this.caravan, this.Destination).FailMessage;
				Messages.Message("MessageCaravanArrivalActionNoLongerValid".Translate(this.caravan.Name).CapitalizeFirst() + ((failMessage != null) ? (" " + failMessage) : ""), this.caravan, MessageTypeDefOf.NegativeEvent, true);
				this.StopDead();
			}
			if (this.caravan.CantMove || this.paused)
			{
				return;
			}
			if (this.nextTileCostLeft > 0f)
			{
				this.nextTileCostLeft -= this.CostToPayThisTick() * (float)delta;
				return;
			}
			if (this.moving)
			{
				this.TryEnterNextPathTile();
			}
		}

		// Token: 0x0600E2A8 RID: 58024 RVA: 0x00542A1A File Offset: 0x00540C1A
		public void Notify_Teleported_Int()
		{
			this.StopDead();
		}

		// Token: 0x0600E2A9 RID: 58025 RVA: 0x00542A22 File Offset: 0x00540C22
		private bool IsPassable(PlanetTile tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x0600E2AA RID: 58026 RVA: 0x00542A32 File Offset: 0x00540C32
		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		// Token: 0x0600E2AB RID: 58027 RVA: 0x00542A40 File Offset: 0x00540C40
		private bool TryRecoverFromUnwalkablePosition()
		{
			PlanetTile tile;
			if (GenWorldClosest.TryFindClosestTile(this.caravan.Tile, (PlanetTile t) => this.IsPassable(t), out tile, 2147483647, true))
			{
				string[] array = new string[5];
				int num = 0;
				Caravan caravan = this.caravan;
				array[num] = ((caravan != null) ? caravan.ToString() : null);
				array[1] = " on unwalkable tile ";
				array[2] = this.caravan.Tile.ToString();
				array[3] = ". Teleporting to ";
				array[4] = tile.ToString();
				Log.Warning(string.Concat(array));
				this.caravan.Tile = tile;
				this.caravan.Notify_Teleported();
				return true;
			}
			Caravan caravan2 = this.caravan;
			Log.Error(((caravan2 != null) ? caravan2.ToString() : null) + " on unwalkable tile " + this.caravan.Tile.ToString() + ". Could not find walkable position nearby. Removed.");
			this.caravan.Destroy();
			return false;
		}

		// Token: 0x0600E2AC RID: 58028 RVA: 0x00542B38 File Offset: 0x00540D38
		private void PatherArrived()
		{
			CaravanArrivalAction caravanArrivalAction = this.arrivalAction;
			this.StopDead();
			if (caravanArrivalAction != null && caravanArrivalAction.StillValid(this.caravan, this.caravan.Tile))
			{
				caravanArrivalAction.Arrived(this.caravan);
				return;
			}
			if (this.caravan.IsPlayerControlled && !this.caravan.VisibleToCameraNow())
			{
				Messages.Message("MessageCaravanArrivedAtDestination".Translate(this.caravan.Label), this.caravan, MessageTypeDefOf.TaskCompletion, true);
			}
		}

		// Token: 0x0600E2AD RID: 58029 RVA: 0x00542BCF File Offset: 0x00540DCF
		private void PatherFailed()
		{
			this.StopDead();
		}

		// Token: 0x0600E2AE RID: 58030 RVA: 0x00542BD8 File Offset: 0x00540DD8
		private void TryEnterNextPathTile()
		{
			if (!this.IsNextTilePassable())
			{
				this.PatherFailed();
				return;
			}
			this.caravan.Tile = this.nextTile;
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.curPath.NodesLeftCount == 0)
			{
				Caravan caravan = this.caravan;
				Log.Error(((caravan != null) ? caravan.ToString() : null) + " ran out of path nodes. Force-arriving.");
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextTile();
		}

		// Token: 0x0600E2AF RID: 58031 RVA: 0x00542C60 File Offset: 0x00540E60
		private void SetupMoveIntoNextTile()
		{
			if (this.curPath.NodesLeftCount < 2)
			{
				string[] array = new string[6];
				int num = 0;
				Caravan caravan = this.caravan;
				array[num] = ((caravan != null) ? caravan.ToString() : null);
				array[1] = " at ";
				array[2] = this.caravan.Tile.ToString();
				array[3] = " ran out of path nodes while pathing to ";
				array[4] = this.destTile.ToString();
				array[5] = ".";
				Log.Error(string.Concat(array));
				this.PatherFailed();
				return;
			}
			this.nextTile = this.curPath.ConsumeNextNode();
			this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			if (Find.World.Impassable(this.nextTile))
			{
				Caravan caravan2 = this.caravan;
				Log.Error(((caravan2 != null) ? caravan2.ToString() : null) + " entering " + this.nextTile.ToString() + " which is unwalkable.");
			}
			int num2 = this.CostToMove(this.caravan.Tile, this.nextTile);
			this.nextTileCostTotal = (float)num2;
			this.nextTileCostLeft = (float)num2;
		}

		// Token: 0x0600E2B0 RID: 58032 RVA: 0x00542D7C File Offset: 0x00540F7C
		private int CostToMove(PlanetTile start, PlanetTile end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, null);
		}

		// Token: 0x0600E2B1 RID: 58033 RVA: 0x00542D9F File Offset: 0x00540F9F
		public static int CostToMove(Caravan caravan, PlanetTile start, PlanetTile end, int? ticksAbs = null)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, ticksAbs, false, null, null, caravan.ImmobilizedByMass);
		}

		// Token: 0x0600E2B2 RID: 58034 RVA: 0x00542DB8 File Offset: 0x00540FB8
		public static int CostToMove(int caravanTicksPerMove, PlanetTile start, PlanetTile end, int? ticksAbs = null, bool perceivedStatic = false, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
		{
			if (start == end)
			{
				return 0;
			}
			if (explanation != null)
			{
				explanation.Append(caravanTicksPerMoveExplanation);
				explanation.AppendLine();
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float num;
			if (perceivedStatic && explanation == null)
			{
				num = Find.WorldPathGrid.PerceivedMovementDifficultyAt(end);
			}
			else
			{
				num = WorldPathGrid.CalculatedMovementDifficultyAt(end, perceivedStatic, ticksAbs, stringBuilder);
			}
			float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(start, end, stringBuilder);
			if (explanation != null && !immobile)
			{
				explanation.AppendLine();
				explanation.Append("TileMovementDifficulty".Translate() + ":");
				explanation.AppendLine();
				explanation.Append(stringBuilder.ToString().Indented("  "));
				explanation.AppendLine();
				explanation.Append("  = " + (num * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			int num2 = (int)((float)caravanTicksPerMove * num * roadMovementDifficultyMultiplier);
			num2 = Mathf.Clamp(num2, 1, 30000);
			if (explanation != null)
			{
				explanation.AppendLine();
				if (immobile)
				{
					explanation.Append("EncumberedCaravanTilesPerDayTip".Translate());
				}
				else
				{
					explanation.AppendLine();
					explanation.Append("FinalCaravanMovementSpeed".Translate() + ":");
					int num3 = Mathf.CeilToInt((float)num2 / 1f);
					explanation.AppendLine();
					explanation.Append(string.Concat(new string[]
					{
						"  ",
						(60000f / (float)caravanTicksPerMove).ToString("0.#"),
						" / ",
						(num * roadMovementDifficultyMultiplier).ToString("0.#"),
						" = ",
						(60000f / (float)num3).ToString("0.#"),
						" "
					}) + "TilesPerDay".Translate());
				}
			}
			return num2;
		}

		// Token: 0x0600E2B3 RID: 58035 RVA: 0x00542FB4 File Offset: 0x005411B4
		public static bool IsValidFinalPushDestination(PlanetTile tile)
		{
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !(allWorldObjects[i] is Caravan))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E2B4 RID: 58036 RVA: 0x00543004 File Offset: 0x00541204
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (DebugSettings.fastCaravans)
			{
				num = 100f;
			}
			if (num < this.nextTileCostTotal / 30000f)
			{
				num = this.nextTileCostTotal / 30000f;
			}
			return num;
		}

		// Token: 0x0600E2B5 RID: 58037 RVA: 0x00543044 File Offset: 0x00541244
		private bool TrySetNewPath()
		{
			WorldPath worldPath = this.GenerateNewPath();
			if (!worldPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = worldPath;
			return true;
		}

		// Token: 0x0600E2B6 RID: 58038 RVA: 0x00543084 File Offset: 0x00541284
		private WorldPath GenerateNewPath()
		{
			PlanetTile planetTile = (this.moving && this.nextTile.Valid && this.IsNextTilePassable()) ? this.nextTile : this.caravan.Tile;
			this.lastPathedTargetTile = this.destTile;
			WorldPath worldPath = planetTile.Layer.Pather.FindPath(planetTile, this.destTile, this.caravan, null);
			if (worldPath.Found && planetTile != this.caravan.Tile)
			{
				if (worldPath.NodesLeftCount >= 2 && worldPath.Peek(1) == this.caravan.Tile)
				{
					worldPath.ConsumeNextNode();
					if (this.moving)
					{
						this.previousTileForDrawingIfInDoubt = this.nextTile;
						this.nextTile = this.caravan.Tile;
						this.nextTileCostLeft = this.nextTileCostTotal - this.nextTileCostLeft;
					}
				}
				else
				{
					worldPath.AddNodeAtStart(this.caravan.Tile);
				}
			}
			return worldPath;
		}

		// Token: 0x0600E2B7 RID: 58039 RVA: 0x00543181 File Offset: 0x00541381
		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		// Token: 0x0600E2B8 RID: 58040 RVA: 0x0054319C File Offset: 0x0054139C
		private bool NeedNewPath()
		{
			if (!this.moving)
			{
				return false;
			}
			if (this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				PlanetTile tileID = this.curPath.Peek(num);
				if (Find.World.Impassable(tileID))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x0400909E RID: 37022
		private Caravan caravan;

		// Token: 0x0400909F RID: 37023
		private bool moving;

		// Token: 0x040090A0 RID: 37024
		private bool paused;

		// Token: 0x040090A1 RID: 37025
		public PlanetTile nextTile = PlanetTile.Invalid;

		// Token: 0x040090A2 RID: 37026
		public PlanetTile previousTileForDrawingIfInDoubt = PlanetTile.Invalid;

		// Token: 0x040090A3 RID: 37027
		public float nextTileCostLeft;

		// Token: 0x040090A4 RID: 37028
		public float nextTileCostTotal = 1f;

		// Token: 0x040090A5 RID: 37029
		private PlanetTile destTile;

		// Token: 0x040090A6 RID: 37030
		private CaravanArrivalAction arrivalAction;

		// Token: 0x040090A7 RID: 37031
		public WorldPath curPath;

		// Token: 0x040090A8 RID: 37032
		public PlanetTile lastPathedTargetTile;

		// Token: 0x040090A9 RID: 37033
		public const int MaxMoveTicks = 30000;

		// Token: 0x040090AA RID: 37034
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x040090AB RID: 37035
		private const int MinCostWalk = 50;

		// Token: 0x040090AC RID: 37036
		private const int MinCostAmble = 60;

		// Token: 0x040090AD RID: 37037
		public const float DefaultPathCostToPayPerTick = 1f;

		// Token: 0x040090AE RID: 37038
		public const int FinalNoRestPushMaxDurationTicks = 10000;
	}
}
