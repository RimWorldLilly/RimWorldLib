using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000372 RID: 882
	public class FactionSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x1700052B RID: 1323
		public NativeArray<ushort>.ReadOnly this[Faction faction]
		{
			get
			{
				NativeArray<ushort> nativeArray;
				if (faction != null && this.factionCosts.TryGetValue(faction, out nativeArray))
				{
					return nativeArray.AsReadOnly();
				}
				return this.noCost.AsReadOnly();
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000A62B8 File Offset: 0x000A44B8
		public FactionSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.noCost = new NativeArray<ushort>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000A630C File Offset: 0x000A450C
		public void Dispose()
		{
			this.noCost.Dispose();
			foreach (NativeArray<ushort> nativeArray in this.factionCosts.Values)
			{
				nativeArray.Dispose();
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x000A6370 File Offset: 0x000A4570
		public bool HasFactionData(Faction faction)
		{
			return faction == null || this.factionCosts.ContainsKey(faction);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000A6384 File Offset: 0x000A4584
		public void Notify_Removed(Faction faction)
		{
			NativeArray<ushort> nativeArray;
			if (!this.factionCosts.TryGetValue(faction, out nativeArray))
			{
				return;
			}
			nativeArray.Dispose();
			this.factionCosts.Remove(faction);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000A63B8 File Offset: 0x000A45B8
		public void ComputeAll(IEnumerable<PathRequest> requests)
		{
			this.GetFactionsToUpdate(requests);
			foreach (Faction faction in this.tmpToUpdate)
			{
				this.ComputeAllForFaction(faction);
			}
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000A6414 File Offset: 0x000A4614
		public bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas)
		{
			this.GetFactionsToUpdate(requests);
			foreach (Faction faction in this.tmpToUpdate)
			{
				NativeArray<ushort> nativeArray;
				if (!this.factionCosts.TryGetValue(faction, out nativeArray))
				{
					this.ComputeAllForFaction(faction);
				}
				else
				{
					CellIndices cellIndices = this.map.cellIndices;
					List<Blueprint>[] innerArray = this.map.blueprintGrid.InnerArray;
					Building[] innerArray2 = this.map.edificeGrid.InnerArray;
					foreach (IntVec3 c in cellDeltas)
					{
						int num = cellIndices.CellToIndex(c);
						ushort num2 = 0;
						List<Blueprint> list = innerArray[num];
						if (list != null)
						{
							foreach (Blueprint thing in list)
							{
								num2 += this.PathFindCost(thing, faction);
							}
						}
						Frame frame = innerArray2[num] as Frame;
						if (frame != null)
						{
							num2 += this.PathFindCost(frame, faction);
						}
						nativeArray[num] = num2;
					}
				}
			}
			return false;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000A65A8 File Offset: 0x000A47A8
		private void GetFactionsToUpdate(IEnumerable<PathRequest> requests)
		{
			this.tmpToUpdate.Clear();
			if (requests != null)
			{
				foreach (PathRequest pathRequest in requests)
				{
					Faction requesterFaction = pathRequest.RequesterFaction;
					if (requesterFaction != null)
					{
						this.tmpToUpdate.AddUnique(requesterFaction);
					}
				}
			}
			this.tmpToUpdate.AddRangeUnique(this.factionCosts.Keys);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000A6624 File Offset: 0x000A4824
		private void ComputeAllForFaction(Faction faction)
		{
			NativeArray<ushort> nativeArray;
			if (!this.factionCosts.TryGetValue(faction, out nativeArray))
			{
				nativeArray = new NativeArray<ushort>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.factionCosts[faction] = nativeArray;
			}
			else
			{
				nativeArray.Clear<ushort>();
			}
			CellIndices cellIndices = this.map.cellIndices;
			IReadOnlyList<Thing> readOnlyList = this.map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint);
			IReadOnlyList<Thing> readOnlyList2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
			int count = readOnlyList.Count;
			for (int i = 0; i < count; i++)
			{
				Blueprint blueprint = readOnlyList[i] as Blueprint;
				if (blueprint != null)
				{
					ushort num = this.PathFindCost(blueprint, faction);
					foreach (IntVec3 c in blueprint.OccupiedRect())
					{
						int num2 = cellIndices.CellToIndex(c);
						ref NativeArray<ushort> ptr = ref nativeArray;
						int index = num2;
						ptr[index] += num;
					}
				}
			}
			int count2 = readOnlyList2.Count;
			for (int j = 0; j < count2; j++)
			{
				Frame frame = readOnlyList2[j] as Frame;
				if (frame != null)
				{
					ushort num3 = this.PathFindCost(frame, faction);
					foreach (IntVec3 c2 in frame.OccupiedRect())
					{
						int num4 = cellIndices.CellToIndex(c2);
						ref NativeArray<ushort> ptr = ref nativeArray;
						int index = num4;
						ptr[index] += num3;
					}
				}
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000A67E0 File Offset: 0x000A49E0
		private ushort PathFindCost(Thing thing, Faction patherFaction)
		{
			Faction faction = thing.Faction;
			if (faction == null)
			{
				return 0;
			}
			if (thing.def.entityDefToBuild.passability == Traversability.Standable)
			{
				return 0;
			}
			if (!this.map.reservationManager.IsReservedAndRespected(thing, patherFaction))
			{
				return 0;
			}
			if (faction == patherFaction)
			{
				return 400;
			}
			if (faction.AllyOrNeutralTo(patherFaction))
			{
				return 400;
			}
			return 0;
		}

		// Token: 0x04001630 RID: 5680
		public const ushort AvoidUnderConstructionPathFindCost = 400;

		// Token: 0x04001631 RID: 5681
		private readonly Map map;

		// Token: 0x04001632 RID: 5682
		private readonly int cellCount;

		// Token: 0x04001633 RID: 5683
		private NativeArray<ushort> noCost;

		// Token: 0x04001634 RID: 5684
		private readonly Dictionary<Faction, NativeArray<ushort>> factionCosts = new Dictionary<Faction, NativeArray<ushort>>();

		// Token: 0x04001635 RID: 5685
		private readonly List<Faction> tmpToUpdate = new List<Faction>();
	}
}
