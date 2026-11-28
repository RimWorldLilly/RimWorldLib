using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000367 RID: 871
	public class PathFinderMapData : IDisposable
	{
		// Token: 0x06001AAF RID: 6831 RVA: 0x000A3A74 File Offset: 0x000A1C74
		public PathFinderMapData(Map map)
		{
			this.map = map;
			this.cellDeltas.Capacity = map.cellIndices.NumGridCells;
			this.cellDeltaSet.EnsureCapacity(map.cellIndices.NumGridCells);
			this.normalCost = new CostSource(map, map.pathing.Normal);
			this.fenceBlockedCost = new CostSource(map, map.pathing.FenceBlocked);
			this.flyingCost = new CostSource(map, map.pathing.Flying);
			this.areas = new AreaSource(map);
			this.perceptual = new PerceptualSource(map);
			this.connectivity = new ConnectivitySource(map);
			this.water = new WaterSource(map);
			this.fences = new FenceSource(map);
			this.buildings = new BuildingSource(map);
			this.factions = new FactionSource(map);
			this.fogged = new FogSource(map);
			this.persistentDanger = new PersistentDangerSource(map);
			this.darknessDanger = new DarknessSource(map);
			this.sources = new List<IPathFinderDataSource>
			{
				this.normalCost,
				this.fenceBlockedCost,
				this.flyingCost,
				this.areas,
				this.perceptual,
				this.connectivity,
				this.water,
				this.fences,
				this.buildings,
				this.factions,
				this.fogged,
				this.persistentDanger,
				this.darknessDanger
			};
			map.events.BuildingSpawned += this.Notify_BuildingChanged;
			map.events.BuildingDespawned += this.Notify_BuildingChanged;
			map.events.BuildingHitPointsChanged += this.Notify_BuildingChanged;
			map.events.ThingSpawned += this.Notify_ThingSpawnedDespawned;
			map.events.ThingDespawned += this.Notify_ThingSpawnedDespawned;
			map.events.ReservationAdded += this.Notify_Reservation;
			map.events.ReservationRemoved += this.Notify_Reservation;
			map.events.HaulEnrouteAdded += this.Notify_HaulEnroute;
			map.events.HaulEnrouteReleased += this.Notify_HaulReleased;
			map.events.FactionRemoved += this.Notify_FactionRemoved;
			map.events.TerrainChanged += this.Notify_CellDelta;
			map.events.PathCostRecalculate += this.Notify_CellDelta;
			map.events.CellFogChanged += this.Event_FogChanged;
			map.events.MapFogged += this.Notify_MapDirtied;
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000A3DB5 File Offset: 0x000A1FB5
		private void Notify_HaulEnroute(Thing enroute, Pawn pawn, ThingDef stuff, int count)
		{
			this.Notify_CellDelta(enroute.OccupiedRect());
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000A3DC3 File Offset: 0x000A1FC3
		private void Notify_HaulReleased(Thing enroute, Pawn pawn)
		{
			this.Notify_CellDelta(enroute.OccupiedRect());
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000A3DD4 File Offset: 0x000A1FD4
		private void Notify_ThingSpawnedDespawned(Thing thing)
		{
			if (!thing.def.pathfinderDangerous)
			{
				return;
			}
			AttachableThing attachableThing = thing as AttachableThing;
			if (attachableThing != null && attachableThing.parent is Pawn)
			{
				return;
			}
			if (thing.def.size.Area == 1)
			{
				this.Notify_CellDelta(thing.Position);
				return;
			}
			this.Notify_CellDelta(thing.OccupiedRect());
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000A3E34 File Offset: 0x000A2034
		public void Dispose()
		{
			foreach (IPathFinderDataSource pathFinderDataSource in this.sources)
			{
				pathFinderDataSource.Dispose();
			}
			this.emptyByteGrid.Dispose();
			this.emptyBoolGrid.Dispose();
			this.emptyUShortGrid.Dispose();
			this.emptyBitGrid.Dispose();
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000A3EB0 File Offset: 0x000A20B0
		public CellConnection CellConnectionsAt(int index)
		{
			return this.connectivity.Data[index];
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000A3ED4 File Offset: 0x000A20D4
		private void Notify_Reservation(ReservationManager.Reservation reservation)
		{
			if (reservation.Target.HasThing)
			{
				this.Notify_CellDelta(reservation.Target.Thing.OccupiedRect());
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000A3F0C File Offset: 0x000A210C
		public void Notify_CellDelta(IntVec3 cell)
		{
			if (cell.InBounds(this.map))
			{
				if (this.cellDeltaSet.Add(cell))
				{
					this.cellDeltas.Add(cell);
					return;
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler.AppendFormatted<PathFinderMapData>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" was notified using an out-of-bounds cell: ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(cell);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000A3F74 File Offset: 0x000A2174
		private void Notify_MapDirtied()
		{
			this.lastGatherTick = -1;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000A3F7D File Offset: 0x000A217D
		private void Notify_BuildingChanged(Building building)
		{
			this.Notify_CellDelta(building.OccupiedRect());
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000A3F8B File Offset: 0x000A218B
		private void Event_FogChanged(IntVec3 cell, bool _)
		{
			this.Notify_CellDelta(cell);
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000A3F94 File Offset: 0x000A2194
		public void Notify_CellDelta(CellRect rect)
		{
			this.cellRectDeltas.Add(rect);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000A3FA2 File Offset: 0x000A21A2
		public void Notify_AreaDelta(Area area, IntVec3 cell)
		{
			this.areas.Notify_AreaDelta(area, cell);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x000A3FB1 File Offset: 0x000A21B1
		public void Notify_FactionRemoved(Faction faction)
		{
			this.factions.Notify_Removed(faction);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000A3FC0 File Offset: 0x000A21C0
		public bool GatherData(IEnumerable<PathRequest> requests)
		{
			bool result;
			using (ProfilerBlock.Scope("PathFinderMapData.GatherData"))
			{
				if (this.lastGatherTick == GenTicks.TicksGame)
				{
					result = false;
				}
				else
				{
					foreach (CellRect cellRect in this.cellRectDeltas)
					{
						foreach (IntVec3 intVec in cellRect.Cells)
						{
							if (intVec.InBounds(this.map) && this.cellDeltaSet.Add(intVec))
							{
								this.cellDeltas.Add(intVec);
							}
						}
					}
					bool anyChanged = this.cellDeltas.Any<IntVec3>();
					this.cellRectDeltas.Clear();
					if (this.lastGatherTick >= 0)
					{
						using (ProfilerBlock.Scope("incremental"))
						{
							Parallel.ForEach<IPathFinderDataSource>(this.sources, delegate(IPathFinderDataSource source)
							{
								if (source.UpdateIncrementally(requests, this.cellDeltas))
								{
									anyChanged = true;
								}
							});
							goto IL_152;
						}
					}
					anyChanged = true;
					using (ProfilerBlock.Scope("recompute"))
					{
						Parallel.ForEach<IPathFinderDataSource>(this.sources, delegate(IPathFinderDataSource source)
						{
							source.ComputeAll(requests);
						});
					}
					IL_152:
					this.cellDeltas.Clear();
					this.cellDeltaSet.Clear();
					this.lastGatherTick = GenTicks.TicksGame;
					result = anyChanged;
				}
			}
			return result;
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x000A41D4 File Offset: 0x000A23D4
		public void ParameterizePathJob(ref PathFinderJob job)
		{
			job.connectivity = this.connectivity.Data;
			job.fences = this.fences.Data;
			job.buildings = this.buildings.Buildings;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000A420C File Offset: 0x000A240C
		public void ParameterizeGridJob(PathRequest request, ref PathFinder.MapGridRequest query, ref PathGridJob job, ref PathFinder.GridJobOutput output)
		{
			Faction requesterFaction = request.RequesterFaction;
			Lord requesterLord = request.RequesterLord;
			job.traverseParams = query.traverseParams;
			job.tuning = query.tuning;
			job.indicies = this.map.cellIndices;
			job.grid = output.grid;
			job.pathGridDirect = (request.TraverseParms.fenceBlocked ? this.fenceBlockedCost.Data : this.normalCost.Data);
			job.building = this.buildings.Buildings;
			job.buildingDestroyable = this.buildings.Destroyable;
			job.player = this.buildings.Player;
			job.water = this.water.Data;
			job.fence = this.fences.Data;
			job.factionCosts = this.factions[requesterFaction];
			job.buildingHitPoints = this.buildings.Hitpoints;
			job.fogged = this.fogged.Data;
			job.darknessDanger = this.darknessDanger.Data;
			job.persistentDanger = this.persistentDanger.Data;
			Pawn pawn = request.pawn;
			if (pawn != null && pawn.Drafted)
			{
				job.perceptualCost = this.perceptual.CostDrafted;
			}
			else
			{
				job.perceptualCost = this.perceptual.CostUndrafted;
			}
			if (requesterLord != null)
			{
				NativeBitArray walkGrid = requesterLord.LordJob.GetWalkGrid(request.pawn);
				job.lordGrid = (walkGrid.IsCreated ? walkGrid.AsReadOnly() : this.emptyBitGrid.AsReadOnly());
			}
			else
			{
				job.lordGrid = this.emptyBitGrid.AsReadOnly();
			}
			job.allowedGrid = ((request.area != null) ? this.areas.DataForArea(request.area).AsReadOnly() : this.emptyBitGrid.AsReadOnly());
			job.custom = ((request.customizer != null) ? request.customizer.GetOffsetGrid().AsReadOnly() : this.emptyUShortGrid.AsReadOnly());
			AvoidGrid avoidGrid = query.avoidGrid;
			job.avoidGrid = ((avoidGrid != null) ? avoidGrid.Grid : this.emptyByteGrid.AsReadOnly());
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000A443C File Offset: 0x000A263C
		public void LogCell(IntVec3 cell)
		{
			int num = this.map.cellIndices.CellToIndex(cell);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 8);
			defaultInterpolatedStringHandler.AppendLiteral("Cell: ");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(cell);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Cost: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.normalCost.Data[num]);
			defaultInterpolatedStringHandler.AppendLiteral(" (fenced: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.fenceBlockedCost.Data[num]);
			defaultInterpolatedStringHandler.AppendLiteral(", flying: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.flyingCost.Data[num]);
			defaultInterpolatedStringHandler.AppendLiteral(")\n");
			defaultInterpolatedStringHandler.AppendLiteral("Connections: ");
			defaultInterpolatedStringHandler.AppendFormatted<CellConnection>(this.CellConnectionsAt(num));
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Building: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.buildings.Buildings.IsSet(num));
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Player: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.buildings.Player.IsSet(num));
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Fence: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.fences.Data.IsSet(num));
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.factions.HasFactionData(faction))
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(9, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("\n    [");
					defaultInterpolatedStringHandler2.AppendFormatted(faction.def.defName);
					defaultInterpolatedStringHandler2.AppendLiteral("]: ");
					defaultInterpolatedStringHandler2.AppendFormatted<ushort>(this.factions[faction][num]);
					text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			Log.Message(text);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000A4680 File Offset: 0x000A2880
		public void RegisterSource(IPathFinderDataSource source)
		{
			if (this.sources.Contains(source))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Source ");
				defaultInterpolatedStringHandler.AppendFormatted<IPathFinderDataSource>(source);
				defaultInterpolatedStringHandler.AppendLiteral(" is already registered in ");
				defaultInterpolatedStringHandler.AppendFormatted<PathFinderMapData>(this);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.sources.Add(source);
		}

		// Token: 0x040015D2 RID: 5586
		private readonly Map map;

		// Token: 0x040015D3 RID: 5587
		private readonly CostSource normalCost;

		// Token: 0x040015D4 RID: 5588
		private readonly CostSource fenceBlockedCost;

		// Token: 0x040015D5 RID: 5589
		private readonly CostSource flyingCost;

		// Token: 0x040015D6 RID: 5590
		private readonly AreaSource areas;

		// Token: 0x040015D7 RID: 5591
		private readonly PerceptualSource perceptual;

		// Token: 0x040015D8 RID: 5592
		private readonly ConnectivitySource connectivity;

		// Token: 0x040015D9 RID: 5593
		private readonly WaterSource water;

		// Token: 0x040015DA RID: 5594
		private readonly FenceSource fences;

		// Token: 0x040015DB RID: 5595
		private readonly BuildingSource buildings;

		// Token: 0x040015DC RID: 5596
		private readonly FactionSource factions;

		// Token: 0x040015DD RID: 5597
		private readonly FogSource fogged;

		// Token: 0x040015DE RID: 5598
		private readonly PersistentDangerSource persistentDanger;

		// Token: 0x040015DF RID: 5599
		private readonly DarknessSource darknessDanger;

		// Token: 0x040015E0 RID: 5600
		private readonly List<IPathFinderDataSource> sources;

		// Token: 0x040015E1 RID: 5601
		private int lastGatherTick = -1;

		// Token: 0x040015E2 RID: 5602
		private readonly List<IntVec3> cellDeltas = new List<IntVec3>();

		// Token: 0x040015E3 RID: 5603
		private readonly HashSet<IntVec3> cellDeltaSet = new HashSet<IntVec3>();

		// Token: 0x040015E4 RID: 5604
		private readonly List<CellRect> cellRectDeltas = new List<CellRect>();

		// Token: 0x040015E5 RID: 5605
		private NativeArray<byte> emptyByteGrid = NativeArrayUtility.EmptyArray<byte>();

		// Token: 0x040015E6 RID: 5606
		private NativeArray<bool> emptyBoolGrid = NativeArrayUtility.EmptyArray<bool>();

		// Token: 0x040015E7 RID: 5607
		private NativeArray<ushort> emptyUShortGrid = NativeArrayUtility.EmptyArray<ushort>();

		// Token: 0x040015E8 RID: 5608
		private NativeBitArray emptyBitGrid = NativeArrayUtility.EmptyBitArray();
	}
}
