using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Gilzoide.ManagedJobs;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000362 RID: 866
	public class PathFinder : IDisposable
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x000A1CF3 File Offset: 0x0009FEF3
		public PathFinderMapData MapData
		{
			get
			{
				return this.mapData;
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000A1CFC File Offset: 0x0009FEFC
		public PathFinder(Map map)
		{
			this.map = map;
			this.mapData = new PathFinderMapData(map);
			int numGridCells = map.cellIndices.NumGridCells;
			int maxJobWorkerCount = UnityData.MaxJobWorkerCount;
			for (int i = 0; i < maxJobWorkerCount; i++)
			{
				this.batches.Add(PathFinder.JobBatchState.AllocateNew(numGridCells));
			}
			this.immediateBatch = PathFinder.JobBatchState.AllocateNew(numGridCells);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000A1DE4 File Offset: 0x0009FFE4
		public void PathFinderTick()
		{
			if (this.disposed)
			{
				return;
			}
			this.ForceCompleteScheduledJobs();
			this.FinalizeRecyclePathJobData();
			this.ComputeWorkThisTick();
			if (this.tmpCurrentWork.Count == 0)
			{
				return;
			}
			if (this.mapData.GatherData(this.tmpCurrentWork))
			{
				this.RecycleGridJobData();
			}
			JobHandle lastGridHandle = this.ScheduleGridJobs();
			this.ScheduleBatchedPathJobs(lastGridHandle);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A1E44 File Offset: 0x000A0044
		private void ComputeWorkThisTick()
		{
			if (this.tmpCurrentWork.Count != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Pathfinder current work queue is not empty, count: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.tmpCurrentWork.Count);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.tmpCurrentWork.Clear();
			int ticksGame = GenTicks.TicksGame;
			for (int i = this.workQueue.Count - 1; i >= 0; i--)
			{
				PathRequest pathRequest = this.workQueue[i];
				if (pathRequest.Cancelled)
				{
					this.workQueue.RemoveAt(i);
				}
				else if (!pathRequest.Validate())
				{
					this.workQueue.RemoveAt(i);
				}
				else if (pathRequest.TickStart <= ticksGame)
				{
					this.tmpCurrentWork.Add(pathRequest);
					this.workQueue.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000A1F18 File Offset: 0x000A0118
		private void FinalizeRecyclePathJobData()
		{
			foreach (PathFinder.ScheduledPathJob scheduledPathJob in this.scheduledPathJobs)
			{
				if (scheduledPathJob.request.Cancelled)
				{
					goto IL_87;
				}
				NativeList<IntVec3> path = scheduledPathJob.output.path;
				if (path.Length != 0)
				{
					goto IL_87;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Resolved path returned no nodes, request: ");
				defaultInterpolatedStringHandler.AppendFormatted<PathRequest>(scheduledPathJob.request);
				Log.WarningOnce(defaultInterpolatedStringHandler.ToStringAndClear(), HashCode.Combine<PathRequest, int>(scheduledPathJob.request, 163511));
				scheduledPathJob.request.Resolve(null);
				IL_CF:
				this.pathUniqueStates.Enqueue(scheduledPathJob.state);
				this.pathJobOutputs.Enqueue(scheduledPathJob.output);
				continue;
				IL_87:
				if (!scheduledPathJob.request.Cancelled && (scheduledPathJob.request.pawn == null || scheduledPathJob.request.pawn.Spawned))
				{
					PawnPath p = this.EmitPath(scheduledPathJob.output);
					scheduledPathJob.request.Resolve(p);
					goto IL_CF;
				}
				goto IL_CF;
			}
			this.scheduledPathJobs.Clear();
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000A2050 File Offset: 0x000A0250
		private void RecycleGridJobData()
		{
			foreach (PathFinder.ScheduledGridJob scheduledGridJob in this.scheduledGridJobs)
			{
				this.gridJobOutputs.Enqueue(scheduledGridJob.output);
			}
			this.scheduledGridJobs.Clear();
			this.gridJobLookup.Clear();
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000A20C4 File Offset: 0x000A02C4
		private void ScheduleBatchedPathJobs(JobHandle lastGridHandle = default(JobHandle))
		{
			int count = this.batches.Count;
			int count2 = this.tmpCurrentWork.Count;
			int num2;
			int num = Math.DivRem(count2, count, out num2);
			if (num2 > 0)
			{
				num++;
			}
			int i = 0;
			int num3 = 0;
			while (i < count)
			{
				JobHandle jobHandle = lastGridHandle;
				PathFinder.JobBatchState jobBatchState = this.batches[i];
				int num4 = 0;
				while (num4 < num && num3 < count2)
				{
					PathRequest request = this.tmpCurrentWork.Pop<PathRequest>();
					PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
					PathFinder.ScheduledGridJob scheduledGridJob = this.gridJobLookup[key];
					PathFinderJob jobData = default(PathFinderJob);
					PathFinder.PathUniqueState state;
					this.GetOrCreatePathJobUniqueState(out state);
					PathFinder.PathJobOutput output;
					this.GetOrCreatePathJobOutput(out output);
					jobHandle = this.GetDoorBlockedJob(request, ref state).Schedule(jobHandle);
					this.ParameterizePathJob(ref jobData, ref key, ref output, ref state, ref jobBatchState, ref scheduledGridJob, request);
					this.mapData.ParameterizePathJob(ref jobData);
					jobHandle = jobData.Schedule(jobHandle);
					this.scheduledPathJobs.Add(new PathFinder.ScheduledPathJob
					{
						request = request,
						handle = jobHandle,
						state = state,
						output = output
					});
					num4++;
					num3++;
				}
				i++;
			}
			JobHandle.ScheduleBatchedJobs();
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000A2208 File Offset: 0x000A0408
		private ManagedJob GetDoorBlockedJob(PathRequest request, ref PathFinder.PathUniqueState jobState)
		{
			PathGridDoorsBlockedJob pathGridDoorsBlockedJob = new PathGridDoorsBlockedJob();
			this.ParameterizeDoorBlockedJob(pathGridDoorsBlockedJob, request, ref jobState);
			return new ManagedJob(pathGridDoorsBlockedJob);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000A222C File Offset: 0x000A042C
		private JobHandle ScheduleGridJobs()
		{
			JobHandle result;
			using (ProfilerBlock.Scope("PathFinder.ScheduleGridJobs()"))
			{
				JobHandle jobHandle = default(JobHandle);
				foreach (PathRequest request in this.tmpCurrentWork)
				{
					PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
					if (!this.gridJobLookup.ContainsKey(key))
					{
						jobHandle = this.ScheduleGridJob(request, jobHandle).handle;
					}
				}
				result = jobHandle;
			}
			return result;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000A22D4 File Offset: 0x000A04D4
		private PathFinder.ScheduledGridJob ScheduleGridJob(PathRequest request, JobHandle handle = default(JobHandle))
		{
			PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
			PathFinder.ScheduledGridJob scheduledGridJob;
			scheduledGridJob.lastAccessed = (scheduledGridJob.computed = GenTicks.TicksGame);
			this.GetOrCreateGridJobOutput(out scheduledGridJob.output);
			PathGridJob jobData = default(PathGridJob);
			this.mapData.ParameterizeGridJob(request, ref key, ref jobData, ref scheduledGridJob.output);
			int numGridCells = this.map.cellIndices.NumGridCells;
			int idealBatchCount = UnityData.GetIdealBatchCount(numGridCells);
			scheduledGridJob.handle = (handle.Equals(default(JobHandle)) ? jobData.Schedule(numGridCells, idealBatchCount, default(JobHandle)) : jobData.Schedule(numGridCells, idealBatchCount, handle));
			this.scheduledGridJobs.Add(scheduledGridJob);
			return this.gridJobLookup[key] = scheduledGridJob;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000A239C File Offset: 0x000A059C
		private void ForceCompleteScheduledJobs()
		{
			int num = this.scheduledPathJobs.Count + this.scheduledGridJobs.Count;
			if (num <= 0)
			{
				return;
			}
			NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.scheduledPathJobs.Count; i++)
			{
				jobs[i] = this.scheduledPathJobs[i].handle;
			}
			for (int j = 0; j < this.scheduledGridJobs.Count; j++)
			{
				jobs[this.scheduledPathJobs.Count + j] = this.scheduledGridJobs[j].handle;
			}
			JobHandle.CompleteAll(jobs);
			jobs.Dispose();
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000A2448 File Offset: 0x000A0648
		private PawnPath EmitPath(in PathFinder.PathJobOutput pathJobOutput)
		{
			NativeList<IntVec3> path = pathJobOutput.path;
			NativeReference<PathFinderJob.ResultData> data = pathJobOutput.data;
			PathFinderJob.ResultData value = data.Value;
			PawnPath path2 = this.map.pawnPathPool.GetPath();
			path2.Initialize(path, value.pathCost);
			return path2;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000A2488 File Offset: 0x000A0688
		private void GetOrCreateGridJobOutput(out PathFinder.GridJobOutput output)
		{
			if (!this.gridJobOutputs.TryDequeue(out output))
			{
				output.Initalize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000A24AE File Offset: 0x000A06AE
		private void GetOrCreatePathJobUniqueState(out PathFinder.PathUniqueState state)
		{
			if (!this.pathUniqueStates.TryDequeue(out state))
			{
				state.Initialize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000A24D4 File Offset: 0x000A06D4
		private void GetOrCreatePathJobOutput(out PathFinder.PathJobOutput output)
		{
			if (!this.pathJobOutputs.TryDequeue(out output))
			{
				output.Initialize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000A24FC File Offset: 0x000A06FC
		public PathRequest CreateRequest(IntVec3 start, LocalTargetInfo target, IntVec3? dest, Pawn pawn, PathFinderCostTuning? tuning, PathEndMode peMode = PathEndMode.OnCell, PathRequest.IPathGridCustomizer customizer = null)
		{
			bool canBashDoors;
			bool flag;
			this.ResolveCanBash(pawn, out canBashDoors, out flag);
			Danger maxDanger = Danger.Deadly;
			TraverseMode mode = TraverseMode.ByPawn;
			bool canBashFences = flag;
			return this.CreateRequest(start, target, dest, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true), tuning, peMode, pawn, customizer);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000A2538 File Offset: 0x000A0738
		public PathRequest CreateRequest(IntVec3 start, LocalTargetInfo target, IntVec3? dest, TraverseParms traverseParms, PathFinderCostTuning? mtuning, PathEndMode peMode = PathEndMode.OnCell, Pawn pawn = null, PathRequest.IPathGridCustomizer customizer = null)
		{
			int ticksGame = GenTicks.TicksGame;
			PathFinderCostTuning tuning = mtuning ?? PathFinderCostTuning.DefaultTuning;
			return new PathRequest(this.map, start, target, dest, traverseParms, tuning, peMode, pawn, ticksGame, ticksGame, ticksGame, customizer);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000A2580 File Offset: 0x000A0780
		public void PushRequest(PathRequest request)
		{
			if (this.workQueue.Contains(request))
			{
				Log.Error("Tried to add the same PathRequest twice to PathFinder.");
				return;
			}
			if (request.ResultIsReady)
			{
				Log.Error("Tried to add PathRequest to PathFinder but it already has its result calculated.");
				return;
			}
			this.workQueue.Add(request);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000A25BC File Offset: 0x000A07BC
		private static float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			if (pawn == null)
			{
				return 1f;
			}
			if (pawn.RaceProps.Animal)
			{
				return 1.75f;
			}
			if (!pawn.IsColonist)
			{
				float magnitude = (start - dest.Cell).Magnitude;
				return (float)Mathf.RoundToInt(PathFinder.HeuristicScaleAI.Evaluate(magnitude));
			}
			if (GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(pawn.Map) && GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn))
			{
				return 1.5f;
			}
			return 1f;
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000A2638 File Offset: 0x000A0838
		public PawnPath FindPathNow(IntVec3 start, LocalTargetInfo target, Pawn pawn, PathFinderCostTuning? tuning = null, PathEndMode peMode = PathEndMode.OnCell)
		{
			bool canBashDoors;
			bool flag;
			this.ResolveCanBash(pawn, out canBashDoors, out flag);
			Danger maxDanger = Danger.Deadly;
			TraverseMode mode = TraverseMode.ByPawn;
			bool canBashFences = flag;
			return this.FindPathNow(start, target, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true), tuning, peMode, null);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000A266C File Offset: 0x000A086C
		public PawnPath FindPathNow(IntVec3 start, LocalTargetInfo target, TraverseParms traverseParms, PathFinderCostTuning? tuning = null, PathEndMode peMode = PathEndMode.OnCell, PathRequest.IPathGridCustomizer customizer = null)
		{
			if (this.disposed)
			{
				return PawnPath.NotFound;
			}
			PathRequest pathRequest = this.CreateRequest(start, target, null, traverseParms, tuning, peMode, traverseParms.pawn, customizer);
			if (!pathRequest.Validate())
			{
				return PawnPath.NotFound;
			}
			this.ForceCompleteScheduledJobs();
			if (this.mapData.GatherData(new PathRequest[]
			{
				pathRequest
			}))
			{
				this.RecycleGridJobData();
			}
			PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(pathRequest);
			PathFinder.ScheduledGridJob scheduledGridJob;
			if (!this.gridJobLookup.TryGetValue(key, out scheduledGridJob))
			{
				scheduledGridJob = this.ScheduleGridJob(pathRequest, default(JobHandle));
			}
			PathFinderJob jobData = default(PathFinderJob);
			PathFinder.PathUniqueState item;
			this.GetOrCreatePathJobUniqueState(out item);
			PathFinder.PathJobOutput item2;
			this.GetOrCreatePathJobOutput(out item2);
			scheduledGridJob.handle.Complete();
			this.GetDoorBlockedJob(pathRequest, ref item).Run();
			this.ParameterizePathJob(ref jobData, ref key, ref item2, ref item, ref this.immediateBatch, ref scheduledGridJob, pathRequest);
			this.mapData.ParameterizePathJob(ref jobData);
			jobData.Run<PathFinderJob>();
			if (item2.path.Length == 0)
			{
				this.pathUniqueStates.Enqueue(item);
				this.pathJobOutputs.Enqueue(item2);
				pathRequest.Resolve(null);
				return PawnPath.NotFound;
			}
			PawnPath result = this.EmitPath(item2);
			this.pathUniqueStates.Enqueue(item);
			this.pathJobOutputs.Enqueue(item2);
			return result;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000A27B8 File Offset: 0x000A09B8
		private void ParameterizeDoorBlockedJob(PathGridDoorsBlockedJob job, PathRequest request, ref PathFinder.PathUniqueState state)
		{
			job.start = request.Start;
			job.dest = request.Target.Cell;
			job.map = this.map;
			job.pawn = request.pawn;
			job.traverseParams = request.TraverseParms;
			job.tuning = request.Tuning;
			this.EnsureDoorsPawnsCached();
			job.doors = this.cachedDoors;
			job.providers = this.cachedProviders;
			job.pawns = this.cachedPawns;
			job.providerCost = state.providers;
			job.blocked = state.blocked;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000A2858 File Offset: 0x000A0A58
		private void EnsureDoorsPawnsCached()
		{
			if (this.cachedTick == GenTicks.TicksGame)
			{
				return;
			}
			this.cachedTick = GenTicks.TicksGame;
			this.cachedProviders.Clear();
			this.cachedDoors.Clear();
			this.cachedPawns.Clear();
			foreach (Thing thing in this.map.listerThings.ThingsInGroup(ThingRequestGroup.CostProvider))
			{
				this.cachedProviders.Add((IPathFindCostProvider)thing);
			}
			this.cachedDoors.AddRange(this.map.listerThings.ThingsInGroup(ThingRequestGroup.Door));
			foreach (Thing thing2 in this.cachedDoors)
			{
				Building_Door building_Door = thing2 as Building_Door;
				if (building_Door != null)
				{
					building_Door.SnapshotWillCloseSoonForJobs();
				}
			}
			this.cachedPawns.AddRange(this.map.mapPawns.AllPawnsSpawned);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000A297C File Offset: 0x000A0B7C
		private void ParameterizePathJob(ref PathFinderJob job, ref PathFinder.MapGridRequest gridRequest, ref PathFinder.PathJobOutput output, ref PathFinder.PathUniqueState uniqueState, ref PathFinder.JobBatchState batchState, ref PathFinder.ScheduledGridJob scheduledGridJob, PathRequest request)
		{
			using (ProfilerBlock.Scope("PathFinder.Parameterize"))
			{
				job.indices = this.map.cellIndices;
				CellRect destRect;
				PathFinder.MakeDestination(request.Target, request.EndMode, this.map.pathing.For(request.TraverseParms), out destRect, ref uniqueState.excludedDestIndices);
				job.grid = scheduledGridJob.output.grid.AsReadOnly();
				scheduledGridJob.lastAccessed = GenTicks.TicksGame;
				job.start = request.Start;
				job.destCell = request.Target.Cell;
				job.destRect = destRect;
				job.heuristicStrength = PathFinder.DetermineHeuristicStrength(request.TraverseParms.pawn, request.Start, request.Target);
				job.traverseParams = gridRequest.traverseParams;
				if (request.TraverseParms.pawn != null)
				{
					job.moveTicksCardinal = Mathf.RoundToInt(request.TraverseParms.pawn.TicksPerMoveCardinal);
					job.moveTicksDiagonal = Mathf.RoundToInt(request.TraverseParms.pawn.TicksPerMoveDiagonal);
				}
				else
				{
					job.moveTicksCardinal = 18;
					job.moveTicksDiagonal = 18;
				}
				job.calcGrid = batchState.calcGrid;
				job.frontier = batchState.frontier;
				job.path = output.path;
				job.result = output.data;
				job.providerCost = uniqueState.providers.AsReadOnly();
				job.blocked = uniqueState.blocked.AsReadOnly();
				job.excludedRectIndices = uniqueState.excludedDestIndices;
			}
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000A2B3C File Offset: 0x000A0D3C
		private static void MakeDestination(LocalTargetInfo target, PathEndMode peMode, PathingContext context, out CellRect rect, ref NativeList<int> excluded)
		{
			excluded.Clear();
			Map map = context.map;
			CellIndices cellIndices = map.cellIndices;
			rect = PathFinder.CalculateDestinationRect(target, peMode, context);
			if (peMode == PathEndMode.Touch && rect.Area != 1)
			{
				IntVec3 b = new IntVec3(1, 0, 0);
				IntVec3 b2 = new IntVec3(0, 0, 1);
				IntVec3 intVec;
				IntVec3 intVec2;
				IntVec3 intVec3;
				IntVec3 intVec4;
				rect.GetInternalCorners(out intVec, out intVec2, out intVec3, out intVec4);
				if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec + b2, intVec + b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec);
					excluded.Add(num);
				}
				if (intVec3.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec3 - b2, intVec3 - b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec3);
					excluded.Add(num);
				}
				if (intVec2.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec2 - b2, intVec2 + b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec2);
					excluded.Add(num);
				}
				if (intVec4.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec4 + b2, intVec4 - b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec4);
					excluded.Add(num);
				}
			}
			rect = rect.ClipInsideMap(map);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000A2CA4 File Offset: 0x000A0EA4
		private static CellRect CalculateDestinationRect(LocalTargetInfo dest, PathEndMode peMode, PathingContext context)
		{
			CellRect result;
			if (!dest.HasThing || peMode == PathEndMode.OnCell)
			{
				result = CellRect.SingleCell(dest.Cell);
			}
			else
			{
				result = dest.Thing.OccupiedRect();
			}
			if (peMode == PathEndMode.Touch)
			{
				result = result.ExpandedBy(1);
			}
			return result;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000A2CE8 File Offset: 0x000A0EE8
		private void ResolveCanBash(Pawn pawn, out bool canBashDoors, out bool canBashFences)
		{
			canBashDoors = (pawn != null && pawn.CurJob != null && pawn.CurJob.canBashDoors);
			canBashFences = (pawn != null && pawn.CurJob != null && pawn.CurJob.canBashFences);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000A2D20 File Offset: 0x000A0F20
		public void LogPathfinderState()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pathfinder State");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Avaliable resources:");
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Grid outputs: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gridJobOutputs.Count);
			stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Path states: ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.pathUniqueStates.Count);
			stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			StringBuilder stringBuilder4 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Path outputs: ");
			defaultInterpolatedStringHandler3.AppendFormatted<int>(this.pathJobOutputs.Count);
			stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
			stringBuilder.AppendLine();
			StringBuilder stringBuilder5 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler4.AppendLiteral("Current grid results: ");
			defaultInterpolatedStringHandler4.AppendFormatted<int>(this.gridJobLookup.Count);
			stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
			foreach (KeyValuePair<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> keyValuePair in this.gridJobLookup)
			{
				PathFinder.MapGridRequest mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob;
				keyValuePair.Deconstruct(out mapGridRequest, out scheduledGridJob);
				PathFinder.MapGridRequest value = mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob2 = scheduledGridJob;
				StringBuilder stringBuilder6 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(35, 3);
				defaultInterpolatedStringHandler5.AppendLiteral("   - ");
				defaultInterpolatedStringHandler5.AppendFormatted<PathFinder.MapGridRequest>(value);
				defaultInterpolatedStringHandler5.AppendLiteral(" (");
				defaultInterpolatedStringHandler5.AppendFormatted<float>((GenTicks.TicksGame - scheduledGridJob2.computed).TicksToSeconds(), "F2");
				defaultInterpolatedStringHandler5.AppendLiteral("s old, last accessed: ");
				defaultInterpolatedStringHandler5.AppendFormatted<float>((GenTicks.TicksGame - scheduledGridJob2.lastAccessed).TicksToSeconds(), "F2");
				defaultInterpolatedStringHandler5.AppendLiteral("s ago)");
				stringBuilder6.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000A2F24 File Offset: 0x000A1124
		public void LogGridCellResult(IntVec3 cell)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (KeyValuePair<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> keyValuePair in this.gridJobLookup)
			{
				PathFinder.MapGridRequest mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob;
				keyValuePair.Deconstruct(out mapGridRequest, out scheduledGridJob);
				PathFinder.MapGridRequest value = mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob2 = scheduledGridJob;
				PathFinder.ScheduledGridJob local = scheduledGridJob2;
				List<FloatMenuOption> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
				defaultInterpolatedStringHandler.AppendFormatted<PathFinder.MapGridRequest>(value);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<float>((GenTicks.TicksGame - scheduledGridJob2.computed).TicksToSeconds(), "F2");
				defaultInterpolatedStringHandler.AppendLiteral("s old)");
				list2.Add(new FloatMenuOption(defaultInterpolatedStringHandler.ToStringAndClear(), delegate()
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(cell);
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(local.output.grid[this.map.cellIndices[cell]]);
					Log.Message(defaultInterpolatedStringHandler2.ToStringAndClear());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000A3040 File Offset: 0x000A1240
		public void OnDraw()
		{
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000A3042 File Offset: 0x000A1242
		public void OnGUI()
		{
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000A3044 File Offset: 0x000A1244
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			foreach (PathFinder.ScheduledGridJob scheduledGridJob in this.scheduledGridJobs)
			{
				scheduledGridJob.handle.Complete();
			}
			foreach (PathFinder.ScheduledPathJob scheduledPathJob in this.scheduledPathJobs)
			{
				scheduledPathJob.handle.Complete();
			}
			foreach (PathFinder.ScheduledGridJob scheduledGridJob2 in this.scheduledGridJobs)
			{
				PathFinder.GridJobOutput output = scheduledGridJob2.output;
				output.Dispose();
			}
			foreach (PathFinder.ScheduledPathJob scheduledPathJob2 in this.scheduledPathJobs)
			{
				PathFinder.PathUniqueState state = scheduledPathJob2.state;
				state.Dispose();
				PathFinder.PathJobOutput output2 = scheduledPathJob2.output;
				output2.Dispose();
			}
			this.mapData.Dispose();
			foreach (PathFinder.JobBatchState jobBatchState in this.batches)
			{
				jobBatchState.Dispose();
			}
			this.immediateBatch.Dispose();
			foreach (PathFinder.GridJobOutput gridJobOutput in this.gridJobOutputs)
			{
				gridJobOutput.Dispose();
			}
			foreach (PathFinder.PathUniqueState pathUniqueState in this.pathUniqueStates)
			{
				pathUniqueState.Dispose();
			}
			foreach (PathFinder.PathJobOutput pathJobOutput in this.pathJobOutputs)
			{
				pathJobOutput.Dispose();
			}
		}

		// Token: 0x04001594 RID: 5524
		private readonly Map map;

		// Token: 0x04001595 RID: 5525
		private readonly PathFinderMapData mapData;

		// Token: 0x04001596 RID: 5526
		private readonly List<PathFinder.JobBatchState> batches = new List<PathFinder.JobBatchState>();

		// Token: 0x04001597 RID: 5527
		private PathFinder.JobBatchState immediateBatch;

		// Token: 0x04001598 RID: 5528
		private readonly List<PathRequest> workQueue = new List<PathRequest>();

		// Token: 0x04001599 RID: 5529
		private readonly List<PathFinder.ScheduledPathJob> scheduledPathJobs = new List<PathFinder.ScheduledPathJob>();

		// Token: 0x0400159A RID: 5530
		private readonly List<PathFinder.ScheduledGridJob> scheduledGridJobs = new List<PathFinder.ScheduledGridJob>();

		// Token: 0x0400159B RID: 5531
		private readonly Queue<PathFinder.PathJobOutput> pathJobOutputs = new Queue<PathFinder.PathJobOutput>();

		// Token: 0x0400159C RID: 5532
		private readonly Queue<PathFinder.PathUniqueState> pathUniqueStates = new Queue<PathFinder.PathUniqueState>();

		// Token: 0x0400159D RID: 5533
		private readonly Queue<PathFinder.GridJobOutput> gridJobOutputs = new Queue<PathFinder.GridJobOutput>();

		// Token: 0x0400159E RID: 5534
		private readonly Dictionary<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> gridJobLookup = new Dictionary<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob>();

		// Token: 0x0400159F RID: 5535
		private readonly List<PathRequest> tmpCurrentWork = new List<PathRequest>();

		// Token: 0x040015A0 RID: 5536
		private bool disposed;

		// Token: 0x040015A1 RID: 5537
		public const int DefaultMoveTicksCardinal = 13;

		// Token: 0x040015A2 RID: 5538
		public const int DefaultMoveTicksDiagonal = 18;

		// Token: 0x040015A3 RID: 5539
		public const float DiagonalMoveFactor = 1.41421f;

		// Token: 0x040015A4 RID: 5540
		private const float HeuristicScaleAnimal = 1.75f;

		// Token: 0x040015A5 RID: 5541
		private static readonly SimpleCurve HeuristicScaleAI = new SimpleCurve
		{
			{
				new CurvePoint(40f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x040015A6 RID: 5542
		private readonly List<IPathFindCostProvider> cachedProviders = new List<IPathFindCostProvider>();

		// Token: 0x040015A7 RID: 5543
		private readonly List<Thing> cachedDoors = new List<Thing>();

		// Token: 0x040015A8 RID: 5544
		private readonly List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x040015A9 RID: 5545
		private int cachedTick;

		// Token: 0x02002690 RID: 9872
		private struct ScheduledPathJob
		{
			// Token: 0x04009DED RID: 40429
			public PathRequest request;

			// Token: 0x04009DEE RID: 40430
			public JobHandle handle;

			// Token: 0x04009DEF RID: 40431
			public PathFinder.PathUniqueState state;

			// Token: 0x04009DF0 RID: 40432
			public PathFinder.PathJobOutput output;
		}

		// Token: 0x02002691 RID: 9873
		private struct ScheduledGridJob
		{
			// Token: 0x04009DF1 RID: 40433
			public int computed;

			// Token: 0x04009DF2 RID: 40434
			public int lastAccessed;

			// Token: 0x04009DF3 RID: 40435
			public JobHandle handle;

			// Token: 0x04009DF4 RID: 40436
			public PathFinder.GridJobOutput output;
		}

		// Token: 0x02002692 RID: 9874
		public readonly struct UnmanagedGridTraverseParams : IEquatable<PathFinder.UnmanagedGridTraverseParams>
		{
			// Token: 0x0600F41B RID: 62491 RVA: 0x00592DCE File Offset: 0x00590FCE
			public UnmanagedGridTraverseParams(TraverseMode mode, bool canBashFences, bool fenceBlocked, bool avoidPersistentDanger, bool avoidDarknessDanger, bool avoidFog, CellRect targetBuildable)
			{
				this.mode = mode;
				this.canBashFences = canBashFences;
				this.fenceBlocked = fenceBlocked;
				this.avoidPersistentDanger = avoidPersistentDanger;
				this.avoidDarknessDanger = avoidDarknessDanger;
				this.avoidFog = avoidFog;
				this.targetBuildable = targetBuildable;
			}

			// Token: 0x0600F41C RID: 62492 RVA: 0x00592E05 File Offset: 0x00591005
			public static PathFinder.UnmanagedGridTraverseParams For(TraverseParms parms)
			{
				return new PathFinder.UnmanagedGridTraverseParams(parms.mode, parms.canBashFences, parms.fenceBlocked, parms.avoidPersistentDanger, parms.avoidDarknessDanger, parms.avoidFog, parms.targetBuildable);
			}

			// Token: 0x0600F41D RID: 62493 RVA: 0x00592E38 File Offset: 0x00591038
			public bool Equals(PathFinder.UnmanagedGridTraverseParams other)
			{
				return this.mode == other.mode && this.canBashFences == other.canBashFences && this.fenceBlocked == other.fenceBlocked && this.avoidPersistentDanger == other.avoidPersistentDanger && this.avoidDarknessDanger == other.avoidDarknessDanger && this.avoidFog == other.avoidFog && this.targetBuildable == other.targetBuildable;
			}

			// Token: 0x0600F41E RID: 62494 RVA: 0x00592EAC File Offset: 0x005910AC
			public override bool Equals(object obj)
			{
				if (obj is PathFinder.UnmanagedGridTraverseParams)
				{
					PathFinder.UnmanagedGridTraverseParams other = (PathFinder.UnmanagedGridTraverseParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600F41F RID: 62495 RVA: 0x00592ED1 File Offset: 0x005910D1
			public override int GetHashCode()
			{
				return HashCode.Combine<int, bool, bool, bool, bool, bool, CellRect>((int)this.mode, this.canBashFences, this.fenceBlocked, this.avoidPersistentDanger, this.avoidDarknessDanger, this.avoidFog, this.targetBuildable);
			}

			// Token: 0x04009DF5 RID: 40437
			public readonly TraverseMode mode;

			// Token: 0x04009DF6 RID: 40438
			public readonly bool canBashFences;

			// Token: 0x04009DF7 RID: 40439
			public readonly bool fenceBlocked;

			// Token: 0x04009DF8 RID: 40440
			public readonly bool avoidPersistentDanger;

			// Token: 0x04009DF9 RID: 40441
			public readonly bool avoidDarknessDanger;

			// Token: 0x04009DFA RID: 40442
			public readonly bool avoidFog;

			// Token: 0x04009DFB RID: 40443
			public readonly CellRect targetBuildable;
		}

		// Token: 0x02002693 RID: 9875
		public struct MapGridRequest : IEquatable<PathFinder.MapGridRequest>
		{
			// Token: 0x0600F420 RID: 62496 RVA: 0x00592F04 File Offset: 0x00591104
			public static PathFinder.MapGridRequest For(PathRequest request)
			{
				AvoidGrid avoidGrid = null;
				if (request.pawn != null)
				{
					request.pawn.TryGetAvoidGrid(out avoidGrid, true);
				}
				return new PathFinder.MapGridRequest
				{
					traverseParams = PathFinder.UnmanagedGridTraverseParams.For(request.TraverseParms),
					tuning = request.Tuning,
					customizer = request.customizer,
					allowedArea = request.area,
					avoidGrid = avoidGrid
				};
			}

			// Token: 0x0600F421 RID: 62497 RVA: 0x00592F74 File Offset: 0x00591174
			public bool Equals(PathFinder.MapGridRequest other)
			{
				return this.traverseParams.Equals(other.traverseParams) && this.tuning.Equals(other.tuning) && object.Equals(this.customizer, other.customizer) && object.Equals(this.allowedArea, other.allowedArea) && object.Equals(this.avoidGrid, other.avoidGrid);
			}

			// Token: 0x0600F422 RID: 62498 RVA: 0x00592FE0 File Offset: 0x005911E0
			public override bool Equals(object obj)
			{
				if (obj is PathFinder.MapGridRequest)
				{
					PathFinder.MapGridRequest other = (PathFinder.MapGridRequest)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600F423 RID: 62499 RVA: 0x00593005 File Offset: 0x00591205
			public override int GetHashCode()
			{
				return HashCode.Combine<PathFinder.UnmanagedGridTraverseParams, PathFinderCostTuning, PathRequest.IPathGridCustomizer, Area, AvoidGrid>(this.traverseParams, this.tuning, this.customizer, this.allowedArea, this.avoidGrid);
			}

			// Token: 0x0600F424 RID: 62500 RVA: 0x0059302C File Offset: 0x0059122C
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 5);
				defaultInterpolatedStringHandler.AppendFormatted<TraverseMode>(this.traverseParams.mode);
				defaultInterpolatedStringHandler.AppendFormatted(this.traverseParams.fenceBlocked ? ", fence blocked" : "");
				defaultInterpolatedStringHandler.AppendFormatted(this.traverseParams.avoidFog ? ", avoid fog" : "");
				defaultInterpolatedStringHandler.AppendFormatted((this.allowedArea != null) ? (", area: " + this.allowedArea.RenamableLabel) : "");
				defaultInterpolatedStringHandler.AppendFormatted((this.avoidGrid != null) ? ", avoid grid: true" : "");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}

			// Token: 0x04009DFC RID: 40444
			public PathFinder.UnmanagedGridTraverseParams traverseParams;

			// Token: 0x04009DFD RID: 40445
			public PathFinderCostTuning tuning;

			// Token: 0x04009DFE RID: 40446
			public PathRequest.IPathGridCustomizer customizer;

			// Token: 0x04009DFF RID: 40447
			public Area allowedArea;

			// Token: 0x04009E00 RID: 40448
			public AvoidGrid avoidGrid;
		}

		// Token: 0x02002694 RID: 9876
		private struct JobBatchState : IDisposable
		{
			// Token: 0x0600F425 RID: 62501 RVA: 0x005930E4 File Offset: 0x005912E4
			public static PathFinder.JobBatchState AllocateNew(int cells)
			{
				PathFinder.JobBatchState result;
				result.calcGrid = new NativeArray<PathFinderJob.CalcNode>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				result.frontier = new NativePriorityQueue<int, float, FloatMinComparer>(cells, default(FloatMinComparer), Allocator.Persistent);
				return result;
			}

			// Token: 0x0600F426 RID: 62502 RVA: 0x00593118 File Offset: 0x00591318
			public void Dispose()
			{
				this.calcGrid.Dispose();
				this.frontier.Dispose();
			}

			// Token: 0x04009E01 RID: 40449
			public NativeArray<PathFinderJob.CalcNode> calcGrid;

			// Token: 0x04009E02 RID: 40450
			public NativePriorityQueue<int, float, FloatMinComparer> frontier;
		}

		// Token: 0x02002695 RID: 9877
		public struct GridJobOutput : IDisposable
		{
			// Token: 0x0600F427 RID: 62503 RVA: 0x00593130 File Offset: 0x00591330
			public void Initalize(int cells)
			{
				this.grid = new NativeArray<int>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}

			// Token: 0x0600F428 RID: 62504 RVA: 0x00593140 File Offset: 0x00591340
			public void Dispose()
			{
				this.grid.Dispose();
			}

			// Token: 0x04009E03 RID: 40451
			public NativeArray<int> grid;
		}

		// Token: 0x02002696 RID: 9878
		private struct PathUniqueState : IDisposable
		{
			// Token: 0x0600F429 RID: 62505 RVA: 0x0059314D File Offset: 0x0059134D
			public void Initialize(int cells)
			{
				this.excludedDestIndices = new NativeList<int>(4, Allocator.Persistent);
				this.providers = new NativeArray<ushort>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.blocked = new NativeArray<bool>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}

			// Token: 0x0600F42A RID: 62506 RVA: 0x0059317D File Offset: 0x0059137D
			public void Dispose()
			{
				this.excludedDestIndices.Dispose();
				this.providers.Dispose();
				this.blocked.Dispose();
			}

			// Token: 0x04009E04 RID: 40452
			public NativeList<int> excludedDestIndices;

			// Token: 0x04009E05 RID: 40453
			public NativeArray<ushort> providers;

			// Token: 0x04009E06 RID: 40454
			public NativeArray<bool> blocked;
		}

		// Token: 0x02002697 RID: 9879
		private struct PathJobOutput : IDisposable
		{
			// Token: 0x0600F42B RID: 62507 RVA: 0x005931A0 File Offset: 0x005913A0
			public void Initialize(int cells)
			{
				this.path = new NativeList<IntVec3>(cells, Allocator.Persistent);
				this.data = new NativeReference<PathFinderJob.ResultData>(default(PathFinderJob.ResultData), Allocator.Persistent);
			}

			// Token: 0x0600F42C RID: 62508 RVA: 0x005931D9 File Offset: 0x005913D9
			public void Dispose()
			{
				this.path.Dispose();
				this.data.Dispose();
			}

			// Token: 0x04009E07 RID: 40455
			public NativeList<IntVec3> path;

			// Token: 0x04009E08 RID: 40456
			public NativeReference<PathFinderJob.ResultData> data;
		}
	}
}
