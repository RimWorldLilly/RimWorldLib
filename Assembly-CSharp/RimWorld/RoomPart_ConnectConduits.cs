using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126C RID: 4716
	public class RoomPart_ConnectConduits : RoomPartWorker
	{
		// Token: 0x060074DE RID: 29918 RVA: 0x00290894 File Offset: 0x0028EA94
		public RoomPart_ConnectConduits(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x060074DF RID: 29919 RVA: 0x0029089D File Offset: 0x0028EA9D
		public override bool FillOnPost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x002908A0 File Offset: 0x0028EAA0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			HashSet<Building> hashSet = new HashSet<Building>();
			HashSet<Building> hashSet2 = new HashSet<Building>();
			HashSet<Building> hashSet3 = new HashSet<Building>();
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						Building building = list[i] as Building;
						if (building != null && building.PowerComp != null)
						{
							if (building.PowerComp is CompPowerPlant)
							{
								hashSet.Add(building);
							}
							else if (building.def == ThingDefOf.HiddenConduit)
							{
								hashSet3.Add(building);
							}
							else if (!(building.PowerComp is CompPowerTransmitter))
							{
								hashSet2.Add(building);
							}
						}
					}
				}
			}
			if (hashSet.Any<Building>() && hashSet2.Any<Building>())
			{
				using (HashSet<Building>.Enumerator enumerator3 = hashSet3.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Building conduit = enumerator3.Current;
						Building building2 = (from ps in hashSet
						orderby (ps.Position - conduit.Position).LengthHorizontalSquared
						select ps).FirstOrDefault<Building>();
						if (building2 != null)
						{
							this.CreateHiddenConduitPath(map, building2.Position, conduit.Position);
						}
					}
				}
			}
			if (hashSet.Count > 1)
			{
				List<Building> list2 = hashSet.ToList<Building>();
				Building building3 = list2[0];
				for (int j = 1; j < list2.Count; j++)
				{
					this.CreateHiddenConduitPath(map, building3.Position, list2[j].Position);
				}
			}
			foreach (Building building4 in hashSet2)
			{
				CompPower powerComp = building4.PowerComp;
				PowerConnectionMaker.DisconnectFromPowerNet(powerComp);
				Building transmitter = powerComp.parent.Position.GetTransmitter(map);
				if (((transmitter != null) ? transmitter.PowerComp : null) != null)
				{
					powerComp.ConnectToTransmitter(transmitter.PowerComp, false);
				}
				else
				{
					map.powerNetManager.Notify_ConnectorWantsConnect(powerComp);
				}
			}
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x00290B34 File Offset: 0x0028ED34
		private void CreateHiddenConduitPath(Map map, IntVec3 from, IntVec3 to)
		{
			List<IntVec3> list = this.FindCardinalOnlyPath(map, from, to);
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (IntVec3 intVec in list)
			{
				if (intVec.GetTransmitter(map) == null)
				{
					Thing thing;
					GenSpawn.TrySpawn(ThingDefOf.HiddenConduit, intVec, map, out thing, WipeMode.Vanish, true);
				}
			}
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x00290BAC File Offset: 0x0028EDAC
		private List<IntVec3> FindCardinalOnlyPath(Map map, IntVec3 from, IntVec3 to)
		{
			new List<IntVec3>();
			List<IntVec3> list = this.TryLShapedPath(map, from, to, true);
			if (list != null)
			{
				return list;
			}
			List<IntVec3> list2 = this.TryLShapedPath(map, from, to, false);
			if (list2 != null)
			{
				return list2;
			}
			return null;
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x00290BE0 File Offset: 0x0028EDE0
		private List<IntVec3> TryLShapedPath(Map map, IntVec3 from, IntVec3 to, bool horizontalFirst)
		{
			List<IntVec3> list = new List<IntVec3>();
			IntVec3 intVec = from;
			if (horizontalFirst)
			{
				while (intVec.x != to.x)
				{
					if (intVec.x < to.x)
					{
						intVec.x++;
					}
					else
					{
						intVec.x--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
				while (intVec.z != to.z)
				{
					if (intVec.z < to.z)
					{
						intVec.z++;
					}
					else
					{
						intVec.z--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
			}
			else
			{
				while (intVec.z != to.z)
				{
					if (intVec.z < to.z)
					{
						intVec.z++;
					}
					else
					{
						intVec.z--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
				while (intVec.x != to.x)
				{
					if (intVec.x < to.x)
					{
						intVec.x++;
					}
					else
					{
						intVec.x--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
			}
			return list;
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x00290D74 File Offset: 0x0028EF74
		private bool CanPlaceConduitAt(Map map, IntVec3 cell)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			Building transmitter = cell.GetTransmitter(map);
			return transmitter == null || transmitter.def == ThingDefOf.HiddenConduit;
		}
	}
}
