using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008AA RID: 2218
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x001AD4ED File Offset: 0x001AB6ED
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x001AD4F5 File Offset: 0x001AB6F5
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x001AD4FD File Offset: 0x001AB6FD
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideNextCell;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x001AD512 File Offset: 0x001AB712
		public float MovePercentage
		{
			get
			{
				return this.cachedMovePercentage;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004498 RID: 17560 RVA: 0x001AD51A File Offset: 0x001AB71A
		public int LastMovedTick
		{
			get
			{
				return this.lastMovedTick;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x001AD522 File Offset: 0x001AB722
		public bool WillCollideNextCell
		{
			get
			{
				return this.cachedWillCollideNextCell;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x001AD52C File Offset: 0x001AB72C
		public IntVec3 LastPassableCellInPath
		{
			get
			{
				if (!this.Moving || this.curPath == null)
				{
					return IntVec3.Invalid;
				}
				if (!this.Destination.Cell.Impassable(this.pawn.Map))
				{
					return this.Destination.Cell;
				}
				List<IntVec3> nodesReversed = this.curPath.NodesReversed;
				for (int i = 0; i < nodesReversed.Count; i++)
				{
					if (!nodesReversed[i].Impassable(this.pawn.Map))
					{
						return nodesReversed[i];
					}
				}
				if (!this.pawn.Position.Impassable(this.pawn.Map))
				{
					return this.pawn.Position;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x001AD5EC File Offset: 0x001AB7EC
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x001AD648 File Offset: 0x001AB848
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<IntVec3>(ref this.nextCell, "nextCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.nextCellCostLeft, "nextCellCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextCellCostTotal, "nextCellCostInitial", 0f, false);
			Scribe_Values.Look<PathEndMode>(ref this.peMode, "peMode", PathEndMode.None, false);
			Scribe_Values.Look<int>(ref this.cellsUntilClamor, "cellsUntilClamor", 0, false);
			Scribe_Values.Look<int>(ref this.lastEnteredCellTick, "lastEnteredCellTick", -999999, false);
			Scribe_Values.Look<int>(ref this.lastMovedTick, "lastMovedTick", -999999, false);
			Scribe_Values.Look<bool>(ref this.debugDisabled, "debugDisabled", false, false);
			Scribe_Values.Look<bool>(ref this.curPathJobIsStale, "curPathJobIsStale", false, false);
			if (this.moving)
			{
				Scribe_TargetInfo.Look(ref this.destination, "destination");
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x001AD73C File Offset: 0x001AB93C
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 3);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" pathing to destroyed thing ");
				defaultInterpolatedStringHandler.AppendFormatted<Thing>(dest.Thing);
				defaultInterpolatedStringHandler.AppendLiteral(" curJob=");
				defaultInterpolatedStringHandler.AppendFormatted(this.pawn.CurJob.ToStringSafe<Job>());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this.PatherFailed();
				return;
			}
			if (!this.PawnCanOccupy(this.pawn.Position) && !this.TryRecoverFromUnwalkablePosition(true))
			{
				return;
			}
			if (this.moving && this.curPath != null && this.destination == dest && this.peMode == peMode)
			{
				return;
			}
			if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				this.PatherFailed();
				return;
			}
			this.peMode = peMode;
			this.destination = dest;
			if (!this.IsNextCellWalkable() || this.NextCellDoorToWaitForOrManuallyOpen() != null || Mathf.Approximately(this.nextCellCostLeft, this.nextCellCostTotal) || (this.nextCell != this.pawn.Position && this.WillCollideWithPawnAt(this.nextCell, false, false)))
			{
				this.ResetToCurrentPosition();
			}
			PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation = this.pawn.Map.pawnDestinationReservationManager.MostRecentReservationFor(this.pawn);
			if (pawnDestinationReservation != null && ((this.destination.HasThing && pawnDestinationReservation.target != this.destination.Cell) || (pawnDestinationReservation.job != this.pawn.CurJob && pawnDestinationReservation.target != this.destination.Cell)))
			{
				this.pawn.Map.pawnDestinationReservationManager.ObsoleteAllClaimedBy(this.pawn);
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.pawn.Downed && !this.pawn.health.CanCrawl)
			{
				Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>());
				this.PatherFailed();
				return;
			}
			this.moving = true;
			this.pawn.jobs.posture = PawnPosture.Standing;
			this.cachedMovePercentage = 0f;
			this.cachedWillCollideNextCell = false;
			this.curPathJobIsStale = true;
			this.SetNewPathRequest();
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x001AD9E4 File Offset: 0x001ABBE4
		public void StopDead()
		{
			this.DisposeAndClearCurPathRequest();
			this.DisposeAndClearCurPath();
			this.moving = false;
			this.nextCell = this.pawn.Position;
			this.cachedMovePercentage = 0f;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
			this.cachedWillCollideNextCell = false;
			this.curPathJobIsStale = false;
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x001ADA44 File Offset: 0x001ABC44
		public void PatherTick()
		{
			if (this.pawn.RaceProps.doesntMove || this.debugDisabled)
			{
				return;
			}
			this.TrySetMovePercent();
			if (this.WillCollideWithPawnAt(this.pawn.Position, true, true))
			{
				if (this.FailedToFindCloseUnoccupiedCellRecently())
				{
					return;
				}
				IntVec3 intVec;
				if (CellFinder.TryFindBestPawnStandCell(this.pawn, out intVec, true) && intVec != this.pawn.Position)
				{
					if (DebugViewSettings.drawPatherState)
					{
						MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "Unstuck", -1f);
					}
					this.pawn.Position = intVec;
					this.ResetToCurrentPosition();
					if (this.moving)
					{
						this.SetNewPathRequest();
						return;
					}
				}
				else
				{
					this.failedToFindCloseUnoccupiedCellTicks = Find.TickManager.TicksGame;
				}
				return;
			}
			else
			{
				if (this.pawn.stances.FullBodyBusy)
				{
					return;
				}
				if (this.moving)
				{
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					PawnPath pawnPath;
					if (this.curPathRequest != null && this.curPathRequest.TryGetPath(out pawnPath))
					{
						this.curPathJobIsStale = false;
						bool? found = this.curPathRequest.Found;
						bool flag = false;
						if (found.GetValueOrDefault() == flag & found != null)
						{
							this.PatherFailed();
						}
						else if (!pawnPath.Found)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(97, 1);
							defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
							defaultInterpolatedStringHandler.AppendLiteral(" got an invalid path from path finder (maybe reachability and path finder state are out of sync).");
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							pawnPath.Dispose();
							this.PatherFailed();
						}
						else
						{
							if (this.debugLog)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(20, 2);
								defaultInterpolatedStringHandler2.AppendFormatted<Pawn>(this.pawn);
								defaultInterpolatedStringHandler2.AppendLiteral(": Claimed new path: ");
								defaultInterpolatedStringHandler2.AppendFormatted<PawnPath>(pawnPath);
								Log.Message(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
							this.DisposeAndClearCurPath();
							this.curPath = pawnPath;
							this.curPathRequest.ClaimCalculatedPath();
							this.DisposeAndClearCurPathRequest();
							List<IntVec3> list = this.curPath.PeekNextCells(20, 0);
							for (int i = 0; i < list.Count; i++)
							{
								if (PawnUtility.KnownDangerAt(list[i], this.pawn.Map, this.pawn))
								{
									this.foundPathWithDanger = GenTicks.TicksGame;
									break;
								}
								if (this.WillCollideWithPawnAt(list[i], false, false))
								{
									this.foundPathWhichCollidesWithPawns = GenTicks.TicksGame;
								}
							}
							this.lastMovedTick = GenTicks.TicksGame;
							if (!this.nextCell.IsValid || this.pawn.Position == this.nextCell)
							{
								this.SetupMoveIntoNextCell();
							}
						}
					}
					else if ((this.destination.IsValid && this.curPathRequest == null && this.curPath == null && !this.pawn.CanReachImmediate(this.destination, this.peMode)) || (this.pawn.IsHashIntervalTick(30) && this.NeedNewPath()))
					{
						if (this.debugLog)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(69, 1);
							defaultInterpolatedStringHandler3.AppendFormatted<Pawn>(this.pawn);
							defaultInterpolatedStringHandler3.AppendLiteral(": Needs a new path (maybe path blocked), will create new path request");
							Log.Message(defaultInterpolatedStringHandler3.ToStringAndClear());
						}
						this.SetNewPathRequest();
					}
				}
				if (this.moving && this.curPath == null)
				{
					return;
				}
				this.cachedWillCollideNextCell = this.WillCollideWithPawnAt(this.nextCell, false, false);
				if (this.moving && this.WillCollideNextCell)
				{
					PawnPath pawnPath2 = this.curPath;
					bool flag2 = pawnPath2 != null && pawnPath2.NodesLeftCount < 15;
					if (Find.TickManager.TicksGame - this.lastMovedTick >= 180 || this.lastBlocker != null)
					{
						Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false, false);
						if ((this.lastBlocker == null || this.lastBlocker == pawn) && pawn != null && this.pawn.HostileTo(pawn))
						{
							if (this.pawn.CanAttackWhenPathingBlocked && this.pawn.TryGetAttackVerb(pawn, false, false) != null)
							{
								Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn);
								job.maxNumMeleeAttacks = 1;
								job.expiryInterval = 300;
								this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false, null, false, true, false);
								return;
							}
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
							return;
						}
					}
					this.cachedMovePercentage = 1f - this.nextCellCostLeft / this.nextCellCostTotal;
					if (flag2 && !this.BestPathHadPawnsInTheWayRecently())
					{
						this.lastBlocker = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false, false);
						this.TrySetNewPathRequest();
					}
					return;
				}
				this.lastBlocker = null;
				if (this.curPath != null && (this.nextCellCostLeft > 0f || this.moving))
				{
					this.curPathJobIsStale = false;
					if (this.nextCellCostLeft > 0f)
					{
						this.nextCellCostLeft -= this.CostToPayThisTick();
					}
					if (this.nextCellCostLeft <= 0f)
					{
						this.TryEnterNextPathCell();
					}
					this.lastMovedTick = Find.TickManager.TicksGame;
					this.TrySetMovePercent();
				}
				return;
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x001ADF7C File Offset: 0x001AC17C
		private void TrySetMovePercent()
		{
			if (this.moving && this.pawn.Spawned && this.BuildingBlockingNextPathCell() == null && this.NextCellDoorToWaitForOrManuallyOpen() == null)
			{
				this.cachedMovePercentage = Mathf.Clamp01(1f - this.nextCellCostLeft / this.nextCellCostTotal);
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x001ADFCC File Offset: 0x001AC1CC
		public void DrawDebugGUI()
		{
			Rect adjustedScreenspaceRect = SilhouetteUtility.GetAdjustedScreenspaceRect(this.pawn, 0.025f);
			Color? color = null;
			if (this.WillCollideWithPawnAt(this.pawn.Position, true, false))
			{
				color = new Color?(new Color(0.14f, 0.93f, 1f, 0.5f));
			}
			else if (this.pawn.stances.FullBodyBusy)
			{
				color = new Color?(new Color(0.37f, 1f, 0.19f, 0.5f));
			}
			else if (this.Moving && this.WillCollideNextCell)
			{
				color = new Color?(new Color(1f, 0.71f, 0.22f, 0.5f));
			}
			if (color != null)
			{
				GUI.DrawTexture(adjustedScreenspaceRect, TexUI.DotHighlight, ScaleMode.ScaleToFit, true, 0f, color.Value, 0f, 0f);
			}
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x001AE0B9 File Offset: 0x001AC2B9
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				if (this.destination.HasThing && this.destination.ThingDestroyed)
				{
					this.PatherFailed();
					return;
				}
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x001AE0F6 File Offset: 0x001AC2F6
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x001AE104 File Offset: 0x001AC304
		public void ResetToCurrentPosition()
		{
			this.DisposeAndClearCurPathRequest();
			this.DisposeAndClearCurPath();
			this.nextCell = this.pawn.Position;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
			this.cachedMovePercentage = 0f;
			this.lastEnteredCellTick = GenTicks.TicksGame;
			if (this.moving && this.destination.IsValid)
			{
				this.SetNewPathRequest();
			}
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x001AE178 File Offset: 0x001AC378
		private bool PawnCanOccupy(IntVec3 c)
		{
			if (!c.WalkableBy(this.pawn.Map, this.pawn))
			{
				return false;
			}
			Building_Door door = c.GetDoor(this.pawn.Map);
			return door == null || door.PawnCanOpen(this.pawn) || door.Open;
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x001AE1D0 File Offset: 0x001AC3D0
		private Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x001AE208 File Offset: 0x001AC408
		public void NotifyThingTransformed(Thing from, Thing to)
		{
			if (!this.destination.HasThing || this.destination.Thing != from)
			{
				return;
			}
			this.destination = new LocalTargetInfo(to);
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x001AE232 File Offset: 0x001AC432
		private bool IsNextCellWalkable()
		{
			return this.nextCell.WalkableBy(this.pawn.Map, this.pawn) && !this.WillCollideWithPawnAt(this.nextCell, false, false);
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x001AE268 File Offset: 0x001AC468
		private bool WillCollideWithPawnAt(IntVec3 c, bool forceOnlyStanding = false, bool useId = false)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, forceOnlyStanding || (this.pawn.IsShambler && !this.pawn.mindState.anyCloseHostilesRecently), false, useId);
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x001AE2BC File Offset: 0x001AC4BC
		public Building_Door NextCellDoorToWaitForOrManuallyOpen()
		{
			Building_Door door = this.nextCell.GetDoor(this.pawn.Map);
			if (door != null && door.SlowsPawns && (!door.Open || door.TicksTillFullyOpened > 0) && door.PawnCanOpen(this.pawn))
			{
				return door;
			}
			return null;
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x001AE310 File Offset: 0x001AC510
		private Pawn RopeeWithStretchedRopeAtNextPathCell()
		{
			List<Pawn> ropees = this.pawn.roping.Ropees;
			for (int i = 0; i < ropees.Count; i++)
			{
				Pawn pawn = ropees[i];
				if (!pawn.Position.InHorDistOf(this.nextCell, 8f))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x001AE368 File Offset: 0x001AC568
		public void PatherDraw()
		{
			if (!Find.ScreenshotModeHandler.Active && DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn) && !this.curPathJobIsStale)
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x001AE3BB File Offset: 0x001AC5BB
		public bool ChangedCellRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastEnteredCellTick <= ticks;
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x001AE3D4 File Offset: 0x001AC5D4
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x001AE3F0 File Offset: 0x001AC5F0
		public bool TryRecoverFromUnwalkablePosition(bool error = true)
		{
			bool flag = false;
			int i = 0;
			while (i < GenRadial.RadialPattern.Length)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					if (intVec == this.pawn.Position)
					{
						return true;
					}
					if (error)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 3);
						defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
						defaultInterpolatedStringHandler.AppendLiteral(" on unwalkable cell ");
						defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.pawn.Position);
						defaultInterpolatedStringHandler.AppendLiteral(". Teleporting to ");
						defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(intVec);
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					this.pawn.Position = intVec;
					this.pawn.Notify_Teleported(true, false);
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(73, 2);
				defaultInterpolatedStringHandler2.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
				defaultInterpolatedStringHandler2.AppendLiteral(" on unwalkable cell ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(this.pawn.Position);
				defaultInterpolatedStringHandler2.AppendLiteral(". Could not find walkable position nearby. Destroyed.");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			return flag;
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x001AE52C File Offset: 0x001AC72C
		private void PatherArrived()
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(": Pather arrived");
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.StopDead();
			if (this.pawn.jobs.curJob != null && !this.curPathJobIsStale)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x001AE5A8 File Offset: 0x001AC7A8
		private void PatherFailed()
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(": Pather failed");
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.StopDead();
			if (this.pawn.jobs.curJob != null && !this.curPathJobIsStale)
			{
				this.pawn.jobs.curDriver.Notify_PatherFailed();
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x001AE624 File Offset: 0x001AC824
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBashDoors) || this.pawn.HostileTo(building))
					{
						this.MakeBashBlockerJob(building);
						return;
					}
					this.ResetToCurrentPosition();
					return;
				}
				else
				{
					if (building.def.IsFence && this.pawn.FenceBlocked && this.pawn.CurJob != null && this.pawn.CurJob.canBashFences)
					{
						this.MakeBashBlockerJob(building);
						return;
					}
					this.ResetToCurrentPosition();
					return;
				}
			}
			else
			{
				if (this.WillCollideNextCell)
				{
					return;
				}
				Building_Door building_Door2 = this.NextCellDoorToWaitForOrManuallyOpen();
				if (building_Door2 != null)
				{
					if (building_Door2.IsForbiddenToPass(this.pawn))
					{
						this.TrySetNewPathRequest();
						return;
					}
					if (!building_Door2.Open)
					{
						building_Door2.StartManualOpenBy(this.pawn);
					}
					Stance_Cooldown stance = new Stance_Cooldown(building_Door2.TicksTillFullyOpened, building_Door2, null)
					{
						neverAimWeapon = true
					};
					this.pawn.stances.SetStance(stance);
					building_Door2.CheckFriendlyTouched(this.pawn);
					return;
				}
				else
				{
					this.lastCell = this.pawn.Position;
					this.pawn.Position = this.nextCell;
					this.lastMoveDirection = (this.nextCell - this.lastCell).AngleFlat;
					this.lastEnteredCellTick = GenTicks.TicksGame;
					if (this.pawn.RaceProps.Humanlike)
					{
						this.cellsUntilClamor--;
						if (this.cellsUntilClamor <= 0)
						{
							GenClamor.DoClamor(this.pawn, 7f, ClamorDefOf.Movement);
							this.cellsUntilClamor = 12;
						}
					}
					this.pawn.filth.Notify_EnteredNewCell();
					if (this.pawn.BodySize > 0.9f)
					{
						this.pawn.Map.snowGrid.AddDepth(this.pawn.Position, -0.001f);
					}
					Building_Door door = this.lastCell.GetDoor(this.pawn.Map);
					if (door != null && !this.pawn.HostileTo(door))
					{
						door.CheckFriendlyTouched(this.pawn);
						if (!door.BlockedOpenMomentary && !door.HoldOpen && door.SlowsPawns && door.PawnCanOpen(this.pawn))
						{
							door.StartManualCloseBy(this.pawn);
							return;
						}
					}
					Pawn pawn = this.RopeeWithStretchedRopeAtNextPathCell();
					if (pawn != null)
					{
						Stance_Cooldown stance_Cooldown = new Stance_Cooldown(60, pawn, null);
						stance_Cooldown.neverAimWeapon = true;
						this.pawn.stances.SetStance(stance_Cooldown);
						return;
					}
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					if (this.curPath.NodesLeftCount <= 1)
					{
						this.TrySetNewPathRequest();
						return;
					}
					this.SetupMoveIntoNextCell();
					return;
				}
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x001AE8F0 File Offset: 0x001ACAF0
		private void MakeBashBlockerJob(Building blocker)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.expiryInterval = 300;
			this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x001AE948 File Offset: 0x001ACB48
		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.pawn.Position);
				defaultInterpolatedStringHandler.AppendLiteral(" ran out of path nodes while pathing to ");
				defaultInterpolatedStringHandler.AppendFormatted<LocalTargetInfo>(this.destination);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this.PatherFailed();
				return;
			}
			IntVec3 a = this.nextCell;
			this.nextCell = this.curPath.ConsumeNextNode();
			if (a == this.nextCell)
			{
				if (this.curPath.NodesLeftCount <= 1)
				{
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					this.SetNewPathRequest();
					return;
				}
				else
				{
					this.nextCell = this.curPath.ConsumeNextNode();
				}
			}
			if (!this.nextCell.WalkableBy(this.pawn.Map, this.pawn))
			{
				this.TrySetNewPathRequest();
				return;
			}
			float num = this.CostToMoveIntoCell(this.nextCell);
			this.nextCellCostTotal = num;
			this.nextCellCostLeft = Mathf.Max(num + Mathf.Min(this.nextCellCostLeft, 0f), 1f);
			this.cachedWillCollideNextCell = this.WillCollideWithPawnAt(this.nextCell, false, false);
			this.cachedMovePercentage = Mathf.Clamp01(1f - this.nextCellCostLeft / this.nextCellCostTotal);
			Building_Door door = this.nextCell.GetDoor(this.pawn.Map);
			if (door == null)
			{
				return;
			}
			door.Notify_PawnApproaching(this.pawn, num);
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x001AEADD File Offset: 0x001ACCDD
		private float CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x001AEAEC File Offset: 0x001ACCEC
		public static int? GetPawnCellBaseCostOverride(Pawn pawn, IntVec3 c)
		{
			if (c.GetTerrain(pawn.Map).IsWater && pawn.WaterCellCost != null)
			{
				return pawn.WaterCellCost;
			}
			return null;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x001AEB2C File Offset: 0x001ACD2C
		private static float CostToMoveIntoCell(Pawn pawn, IntVec3 c)
		{
			float num;
			if (c.x == pawn.Position.x || c.z == pawn.Position.z)
			{
				num = pawn.TicksPerMoveCardinal;
			}
			else
			{
				num = pawn.TicksPerMoveDiagonal;
			}
			int? pawnCellBaseCostOverride = Pawn_PathFollower.GetPawnCellBaseCostOverride(pawn, c);
			num += (float)pawn.Map.pathing.For(pawn).pathGrid.CalculatedCostAt(c, false, pawn.Position, pawnCellBaseCostOverride);
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null)
			{
				num += (float)edifice.PathWalkCostFor(pawn);
			}
			TerrainDef terrain = c.GetTerrain(pawn.Map);
			if (terrain.tags != null)
			{
				foreach (string key in terrain.tags)
				{
					float num2;
					if (pawn.kindDef.moveSpeedFactorByTerrainTag.TryGetValue(key, out num2))
					{
						num /= num2;
					}
				}
			}
			if (num > 450f)
			{
				num = 450f;
			}
			if (pawn.CurJob != null)
			{
				Pawn locomotionUrgencySameAs = pawn.jobs.curDriver.locomotionUrgencySameAs;
				if (locomotionUrgencySameAs != null && locomotionUrgencySameAs != pawn && locomotionUrgencySameAs.Spawned)
				{
					float num3 = Pawn_PathFollower.CostToMoveIntoCell(locomotionUrgencySameAs, c);
					if (num < num3)
					{
						num = num3;
					}
				}
				else
				{
					switch (pawn.jobs.curJob.locomotionUrgency)
					{
					case LocomotionUrgency.Amble:
						num *= 3f;
						if (num < 60f)
						{
							num = 60f;
						}
						break;
					case LocomotionUrgency.Walk:
						num *= 2f;
						if (num < 50f)
						{
							num = 50f;
						}
						break;
					case LocomotionUrgency.Jog:
						num *= 1f;
						break;
					case LocomotionUrgency.Sprint:
						num = (float)Mathf.RoundToInt(num * 0.75f);
						break;
					}
				}
			}
			return Mathf.Max(num, 1f);
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x001AECFC File Offset: 0x001ACEFC
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.stagger.Staggered)
			{
				num *= this.pawn.stances.stagger.StaggerMoveSpeedFactor;
			}
			if (this.pawn.Flying)
			{
				num *= this.pawn.RaceProps.flightSpeedFactor;
			}
			if (num < this.nextCellCostTotal / 450f)
			{
				num = this.nextCellCostTotal / 450f;
			}
			return num;
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x001AED7B File Offset: 0x001ACF7B
		private void TrySetNewPathRequest()
		{
			if (this.curPathRequest == null)
			{
				this.SetNewPathRequest();
			}
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x001AED8B File Offset: 0x001ACF8B
		private void SetNewPathRequest()
		{
			this.DisposeAndClearCurPathRequest();
			this.curPathRequest = this.GenerateNewPathRequest();
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x001AEDA0 File Offset: 0x001ACFA0
		private PathRequest GenerateNewPathRequest()
		{
			if (this.debugLog)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 4);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(": Created request for a new path to ");
				defaultInterpolatedStringHandler.AppendFormatted<LocalTargetInfo>(this.destination);
				defaultInterpolatedStringHandler.AppendLiteral(" with ");
				defaultInterpolatedStringHandler.AppendFormatted<PathEndMode>(this.peMode);
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.nextCell);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.cachedReturningToCell = GuestUtility.PrisonerCanReturnToCell(this.pawn);
			this.lastPathedTargetPosition = this.destination.Cell;
			PathFinder pathFinder = this.pawn.Map.pathFinder;
			PathFinderCostTuning value = PathFinderCostTuning.For(this.pawn);
			PathRequest pathRequest = pathFinder.CreateRequest(this.nextCell, this.destination, null, this.pawn, new PathFinderCostTuning?(value), this.peMode, null);
			pathFinder.PushRequest(pathRequest);
			return pathRequest;
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x001AEE97 File Offset: 0x001AD097
		public void DisposeAndClearCurPathRequest()
		{
			if (this.curPathRequest != null)
			{
				this.curPathRequest.Dispose();
			}
			this.curPathRequest = null;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x001AEEB3 File Offset: 0x001AD0B3
		public void DisposeAndClearCurPath()
		{
			if (this.curPath != null)
			{
				this.curPath.Dispose();
			}
			this.curPath = null;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x001AEECF File Offset: 0x001AD0CF
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x001AEEE8 File Offset: 0x001AD0E8
		private bool NeedNewPath()
		{
			if (this.destination.IsValid && this.pawn.CanReachImmediate(this.destination, this.peMode))
			{
				return false;
			}
			if (!this.destination.IsValid || this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			if (this.destination.HasThing && this.destination.Thing.Map != this.pawn.Map)
			{
				return true;
			}
			if ((this.pawn.Position.InHorDistOf(this.curPath.LastNode, 15f) || this.pawn.Position.InHorDistOf(this.destination.Cell, 15f)) && !ReachabilityImmediate.CanReachImmediate(this.curPath.LastNode, this.destination, this.pawn.Map, this.peMode, this.pawn))
			{
				return true;
			}
			if (this.curPath.UsedRegionHeuristics && this.curPath.NodesConsumedCount >= 75)
			{
				return true;
			}
			if (this.lastPathedTargetPosition != this.destination.Cell)
			{
				float num = (float)(this.pawn.Position - this.destination.Cell).LengthHorizontalSquared;
				float num2;
				if (num > 900f)
				{
					num2 = 10f;
				}
				else if (num > 289f)
				{
					num2 = 5f;
				}
				else if (num > 100f)
				{
					num2 = 3f;
				}
				else if (num > 49f)
				{
					num2 = 2f;
				}
				else
				{
					num2 = 0.5f;
				}
				if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
				{
					return true;
				}
			}
			PathingContext pc = this.pawn.Map.pathing.For(this.pawn);
			bool canBashFences = this.pawn.CurJob != null && this.pawn.CurJob.canBashFences;
			IntVec3 intVec = IntVec3.Invalid;
			int num3 = 0;
			while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
			{
				IntVec3 intVec2 = this.curPath.Peek(num3);
				if (!intVec2.WalkableBy(this.pawn.Map, this.pawn))
				{
					return true;
				}
				if (!this.BestPathHadPawnsInTheWayRecently() && this.WillCollideWithPawnAt(intVec2, false, false))
				{
					if (!this.ChangedCellRecently((int)this.nextCellCostTotal + 10))
					{
						this.ResetToCurrentPosition();
					}
					this.foundPathWhichCollidesWithPawns = GenTicks.TicksGame;
					return true;
				}
				if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec2, this.pawn.Map, this.pawn))
				{
					return true;
				}
				Building_Door building_Door = intVec2.GetEdifice(this.pawn.Map) as Building_Door;
				if (building_Door != null)
				{
					if (building_Door.BlocksPawn(this.pawn) && !building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
					{
						return true;
					}
					if (building_Door.IsForbiddenToPass(this.pawn))
					{
						return true;
					}
				}
				if (num3 != 0 && intVec2.AdjacentToDiagonal(intVec) && (PathUtility.BlocksDiagonalMovement(intVec2.x, intVec.z, pc, canBashFences) || PathUtility.BlocksDiagonalMovement(intVec.x, intVec2.z, pc, canBashFences)))
				{
					return true;
				}
				intVec = intVec2;
				num3++;
			}
			return false;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x001AF25C File Offset: 0x001AD45C
		public bool BestPathHadPawnsInTheWayRecently()
		{
			int num = this.pawn.IsPlayerControlled ? 15 : 100;
			return this.foundPathWhichCollidesWithPawns + num > Find.TickManager.TicksGame;
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x001AF291 File Offset: 0x001AD491
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x001AF2AB File Offset: 0x001AD4AB
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x04002C68 RID: 11368
		protected Pawn pawn;

		// Token: 0x04002C69 RID: 11369
		private bool moving;

		// Token: 0x04002C6A RID: 11370
		public IntVec3 nextCell;

		// Token: 0x04002C6B RID: 11371
		private IntVec3 lastCell;

		// Token: 0x04002C6C RID: 11372
		public float lastMoveDirection;

		// Token: 0x04002C6D RID: 11373
		public float nextCellCostLeft;

		// Token: 0x04002C6E RID: 11374
		public float nextCellCostTotal = 1f;

		// Token: 0x04002C6F RID: 11375
		private int cellsUntilClamor;

		// Token: 0x04002C70 RID: 11376
		private int lastEnteredCellTick = -999999;

		// Token: 0x04002C71 RID: 11377
		private int lastMovedTick = -999999;

		// Token: 0x04002C72 RID: 11378
		public bool debugDisabled;

		// Token: 0x04002C73 RID: 11379
		public bool curPathJobIsStale;

		// Token: 0x04002C74 RID: 11380
		private LocalTargetInfo destination;

		// Token: 0x04002C75 RID: 11381
		private PathEndMode peMode;

		// Token: 0x04002C76 RID: 11382
		public PathRequest curPathRequest;

		// Token: 0x04002C77 RID: 11383
		public PawnPath curPath;

		// Token: 0x04002C78 RID: 11384
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x04002C79 RID: 11385
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x04002C7A RID: 11386
		private int foundPathWithDanger = -999999;

		// Token: 0x04002C7B RID: 11387
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x04002C7C RID: 11388
		private float cachedMovePercentage;

		// Token: 0x04002C7D RID: 11389
		private bool cachedWillCollideNextCell;

		// Token: 0x04002C7E RID: 11390
		public bool debugLog;

		// Token: 0x04002C7F RID: 11391
		public bool cachedReturningToCell;

		// Token: 0x04002C80 RID: 11392
		private Pawn lastBlocker;

		// Token: 0x04002C81 RID: 11393
		private const int MaxMoveTicks = 450;

		// Token: 0x04002C82 RID: 11394
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04002C83 RID: 11395
		private const int CheckNeedNewPathIntervalTicks = 30;

		// Token: 0x04002C84 RID: 11396
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x04002C85 RID: 11397
		private const int ClamorCellsInterval = 12;

		// Token: 0x04002C86 RID: 11398
		private const int MinCostWalk = 50;

		// Token: 0x04002C87 RID: 11399
		private const int MinCostAmble = 60;

		// Token: 0x04002C88 RID: 11400
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 15;

		// Token: 0x04002C89 RID: 11401
		private const int AttackBlockingHostilePawnAfterTicks = 180;

		// Token: 0x04002C8A RID: 11402
		private const int WaitForRopeeTicks = 60;

		// Token: 0x04002C8B RID: 11403
		private const float RopeLength = 8f;
	}
}
