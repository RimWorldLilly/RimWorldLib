using System;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020002CA RID: 714
	public class MapEvents
	{
		// Token: 0x0600159F RID: 5535 RVA: 0x00081707 File Offset: 0x0007F907
		public MapEvents(Map map)
		{
			this.map = map;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060015A0 RID: 5536 RVA: 0x00081718 File Offset: 0x0007F918
		// (remove) Token: 0x060015A1 RID: 5537 RVA: 0x00081750 File Offset: 0x0007F950
		public event Action<Building> BuildingSpawned;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060015A2 RID: 5538 RVA: 0x00081788 File Offset: 0x0007F988
		// (remove) Token: 0x060015A3 RID: 5539 RVA: 0x000817C0 File Offset: 0x0007F9C0
		public event Action<Building> BuildingDespawned;

		// Token: 0x060015A4 RID: 5540 RVA: 0x000817F5 File Offset: 0x0007F9F5
		public void Notify_BuildingSpawned(Building building)
		{
			Action<Building> buildingSpawned = this.BuildingSpawned;
			if (buildingSpawned == null)
			{
				return;
			}
			buildingSpawned(building);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00081808 File Offset: 0x0007FA08
		public void Notify_BuildingDespawned(Building building)
		{
			Action<Building> buildingDespawned = this.BuildingDespawned;
			if (buildingDespawned == null)
			{
				return;
			}
			buildingDespawned(building);
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060015A6 RID: 5542 RVA: 0x0008181C File Offset: 0x0007FA1C
		// (remove) Token: 0x060015A7 RID: 5543 RVA: 0x00081854 File Offset: 0x0007FA54
		public event Action<Thing> ThingSpawned;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060015A8 RID: 5544 RVA: 0x0008188C File Offset: 0x0007FA8C
		// (remove) Token: 0x060015A9 RID: 5545 RVA: 0x000818C4 File Offset: 0x0007FAC4
		public event Action<Thing> ThingDespawned;

		// Token: 0x060015AA RID: 5546 RVA: 0x000818F9 File Offset: 0x0007FAF9
		public void Notify_ThingSpawned(Thing thing)
		{
			Action<Thing> thingSpawned = this.ThingSpawned;
			if (thingSpawned == null)
			{
				return;
			}
			thingSpawned(thing);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0008190C File Offset: 0x0007FB0C
		public void Notify_ThingDespawned(Thing thing)
		{
			Action<Thing> thingDespawned = this.ThingDespawned;
			if (thingDespawned == null)
			{
				return;
			}
			thingDespawned(thing);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060015AC RID: 5548 RVA: 0x00081920 File Offset: 0x0007FB20
		// (remove) Token: 0x060015AD RID: 5549 RVA: 0x00081958 File Offset: 0x0007FB58
		public event Action<GameCondition> GameConditionAdded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060015AE RID: 5550 RVA: 0x00081990 File Offset: 0x0007FB90
		// (remove) Token: 0x060015AF RID: 5551 RVA: 0x000819C8 File Offset: 0x0007FBC8
		public event Action<GameCondition> GameConditionRemoved;

		// Token: 0x060015B0 RID: 5552 RVA: 0x000819FD File Offset: 0x0007FBFD
		public void Notify_GameConditionAdded(GameCondition condition)
		{
			Action<GameCondition> gameConditionAdded = this.GameConditionAdded;
			if (gameConditionAdded == null)
			{
				return;
			}
			gameConditionAdded(condition);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00081A10 File Offset: 0x0007FC10
		public void Notify_GameConditionRemoved(GameCondition condition)
		{
			Action<GameCondition> gameConditionRemoved = this.GameConditionRemoved;
			if (gameConditionRemoved == null)
			{
				return;
			}
			gameConditionRemoved(condition);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060015B2 RID: 5554 RVA: 0x00081A24 File Offset: 0x0007FC24
		// (remove) Token: 0x060015B3 RID: 5555 RVA: 0x00081A5C File Offset: 0x0007FC5C
		public event Action<Lord> LordAdded;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060015B4 RID: 5556 RVA: 0x00081A94 File Offset: 0x0007FC94
		// (remove) Token: 0x060015B5 RID: 5557 RVA: 0x00081ACC File Offset: 0x0007FCCC
		public event Action<Lord> LordRemoved;

		// Token: 0x060015B6 RID: 5558 RVA: 0x00081B01 File Offset: 0x0007FD01
		public void Notify_LordAdded(Lord lord)
		{
			Action<Lord> lordAdded = this.LordAdded;
			if (lordAdded == null)
			{
				return;
			}
			lordAdded(lord);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00081B14 File Offset: 0x0007FD14
		public void Notify_LordRemoved(Lord lord)
		{
			Action<Lord> lordRemoved = this.LordRemoved;
			if (lordRemoved == null)
			{
				return;
			}
			lordRemoved(lord);
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060015B8 RID: 5560 RVA: 0x00081B28 File Offset: 0x0007FD28
		// (remove) Token: 0x060015B9 RID: 5561 RVA: 0x00081B60 File Offset: 0x0007FD60
		public event Action<Faction> FactionAdded;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060015BA RID: 5562 RVA: 0x00081B98 File Offset: 0x0007FD98
		// (remove) Token: 0x060015BB RID: 5563 RVA: 0x00081BD0 File Offset: 0x0007FDD0
		public event Action<Faction> FactionRemoved;

		// Token: 0x060015BC RID: 5564 RVA: 0x00081C05 File Offset: 0x0007FE05
		public void Notify_FactionAdded(Faction faction)
		{
			Action<Faction> factionAdded = this.FactionAdded;
			if (factionAdded == null)
			{
				return;
			}
			factionAdded(faction);
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00081C18 File Offset: 0x0007FE18
		public void Notify_FactionRemoved(Faction faction)
		{
			Action<Faction> factionRemoved = this.FactionRemoved;
			if (factionRemoved == null)
			{
				return;
			}
			factionRemoved(faction);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060015BE RID: 5566 RVA: 0x00081C2C File Offset: 0x0007FE2C
		// (remove) Token: 0x060015BF RID: 5567 RVA: 0x00081C64 File Offset: 0x0007FE64
		public event Action<Faction, Faction> ThingFactionChanged;

		// Token: 0x060015C0 RID: 5568 RVA: 0x00081C99 File Offset: 0x0007FE99
		public void Notify_ThingFactionChanged(Faction previous, Faction faction)
		{
			Action<Faction, Faction> thingFactionChanged = this.ThingFactionChanged;
			if (thingFactionChanged == null)
			{
				return;
			}
			thingFactionChanged(previous, faction);
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060015C1 RID: 5569 RVA: 0x00081CB0 File Offset: 0x0007FEB0
		// (remove) Token: 0x060015C2 RID: 5570 RVA: 0x00081CE8 File Offset: 0x0007FEE8
		public event Action<Building> BuildingHitPointsChanged;

		// Token: 0x060015C3 RID: 5571 RVA: 0x00081D1D File Offset: 0x0007FF1D
		public void Notify_BuildingHitPointsChanged(Building building)
		{
			Action<Building> buildingHitPointsChanged = this.BuildingHitPointsChanged;
			if (buildingHitPointsChanged == null)
			{
				return;
			}
			buildingHitPointsChanged(building);
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060015C4 RID: 5572 RVA: 0x00081D30 File Offset: 0x0007FF30
		// (remove) Token: 0x060015C5 RID: 5573 RVA: 0x00081D68 File Offset: 0x0007FF68
		public event Action<IntVec3> TerrainChanged;

		// Token: 0x060015C6 RID: 5574 RVA: 0x00081D9D File Offset: 0x0007FF9D
		public void Notify_TerrainChanged(IntVec3 cell)
		{
			Action<IntVec3> terrainChanged = this.TerrainChanged;
			if (terrainChanged == null)
			{
				return;
			}
			terrainChanged(cell);
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060015C7 RID: 5575 RVA: 0x00081DB0 File Offset: 0x0007FFB0
		// (remove) Token: 0x060015C8 RID: 5576 RVA: 0x00081DE8 File Offset: 0x0007FFE8
		public event Action<IntVec3> PathCostRecalculate;

		// Token: 0x060015C9 RID: 5577 RVA: 0x00081E1D File Offset: 0x0008001D
		public void Notify_PathCostRecalculated(IntVec3 cell)
		{
			Action<IntVec3> pathCostRecalculate = this.PathCostRecalculate;
			if (pathCostRecalculate == null)
			{
				return;
			}
			pathCostRecalculate(cell);
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060015CA RID: 5578 RVA: 0x00081E30 File Offset: 0x00080030
		// (remove) Token: 0x060015CB RID: 5579 RVA: 0x00081E68 File Offset: 0x00080068
		public event Action<ReservationManager.Reservation> ReservationAdded;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060015CC RID: 5580 RVA: 0x00081EA0 File Offset: 0x000800A0
		// (remove) Token: 0x060015CD RID: 5581 RVA: 0x00081ED8 File Offset: 0x000800D8
		public event Action<ReservationManager.Reservation> ReservationRemoved;

		// Token: 0x060015CE RID: 5582 RVA: 0x00081F0D File Offset: 0x0008010D
		public void Notify_ReservationAdded(ReservationManager.Reservation reservation)
		{
			Action<ReservationManager.Reservation> reservationAdded = this.ReservationAdded;
			if (reservationAdded == null)
			{
				return;
			}
			reservationAdded(reservation);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00081F20 File Offset: 0x00080120
		public void Notify_ReservationRemoved(ReservationManager.Reservation reservation)
		{
			Action<ReservationManager.Reservation> reservationRemoved = this.ReservationRemoved;
			if (reservationRemoved == null)
			{
				return;
			}
			reservationRemoved(reservation);
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060015D0 RID: 5584 RVA: 0x00081F34 File Offset: 0x00080134
		// (remove) Token: 0x060015D1 RID: 5585 RVA: 0x00081F6C File Offset: 0x0008016C
		public event Action<Thing, Pawn, ThingDef, int> HaulEnrouteAdded;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060015D2 RID: 5586 RVA: 0x00081FA4 File Offset: 0x000801A4
		// (remove) Token: 0x060015D3 RID: 5587 RVA: 0x00081FDC File Offset: 0x000801DC
		public event Action<Thing, Pawn> HaulEnrouteReleased;

		// Token: 0x060015D4 RID: 5588 RVA: 0x00082011 File Offset: 0x00080211
		public void Notify_HaulEnrouteAdded(Thing enroute, Pawn pawn, ThingDef stuff, int count)
		{
			Action<Thing, Pawn, ThingDef, int> haulEnrouteAdded = this.HaulEnrouteAdded;
			if (haulEnrouteAdded == null)
			{
				return;
			}
			haulEnrouteAdded(enroute, pawn, stuff, count);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00082028 File Offset: 0x00080228
		public void Notify_HaulEnrouteReleased(Thing enroute, Pawn pawn)
		{
			Action<Thing, Pawn> haulEnrouteReleased = this.HaulEnrouteReleased;
			if (haulEnrouteReleased == null)
			{
				return;
			}
			haulEnrouteReleased(enroute, pawn);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060015D6 RID: 5590 RVA: 0x0008203C File Offset: 0x0008023C
		// (remove) Token: 0x060015D7 RID: 5591 RVA: 0x00082074 File Offset: 0x00080274
		public event Action<IntVec3, bool> CellFogChanged;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060015D8 RID: 5592 RVA: 0x000820AC File Offset: 0x000802AC
		// (remove) Token: 0x060015D9 RID: 5593 RVA: 0x000820E4 File Offset: 0x000802E4
		public event Action MapFogged;

		// Token: 0x060015DA RID: 5594 RVA: 0x00082119 File Offset: 0x00080319
		public void Notify_CellFogChanged(IntVec3 cell, bool fogged)
		{
			Action<IntVec3, bool> cellFogChanged = this.CellFogChanged;
			if (cellFogChanged == null)
			{
				return;
			}
			cellFogChanged(cell, fogged);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0008212D File Offset: 0x0008032D
		public void Notify_MapFogged()
		{
			Action mapFogged = this.MapFogged;
			if (mapFogged == null)
			{
				return;
			}
			mapFogged();
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060015DC RID: 5596 RVA: 0x00082140 File Offset: 0x00080340
		// (remove) Token: 0x060015DD RID: 5597 RVA: 0x00082178 File Offset: 0x00080378
		public event Action RegionsRoomsChanged;

		// Token: 0x060015DE RID: 5598 RVA: 0x000821AD File Offset: 0x000803AD
		public void Notify_RegionsRoomsChanged()
		{
			Action regionsRoomsChanged = this.RegionsRoomsChanged;
			if (regionsRoomsChanged == null)
			{
				return;
			}
			regionsRoomsChanged();
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060015DF RID: 5599 RVA: 0x000821C0 File Offset: 0x000803C0
		// (remove) Token: 0x060015E0 RID: 5600 RVA: 0x000821F8 File Offset: 0x000803F8
		public event Action<Building_Door> DoorOpened;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060015E1 RID: 5601 RVA: 0x00082230 File Offset: 0x00080430
		// (remove) Token: 0x060015E2 RID: 5602 RVA: 0x00082268 File Offset: 0x00080468
		public event Action<Building_Door> DoorClosed;

		// Token: 0x060015E3 RID: 5603 RVA: 0x0008229D File Offset: 0x0008049D
		public void Notify_DoorOpened(Building_Door door)
		{
			Action<Building_Door> doorOpened = this.DoorOpened;
			if (doorOpened == null)
			{
				return;
			}
			doorOpened(door);
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x000822B0 File Offset: 0x000804B0
		public void Notify_DoorClosed(Building_Door door)
		{
			Action<Building_Door> doorClosed = this.DoorClosed;
			if (doorClosed == null)
			{
				return;
			}
			doorClosed(door);
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060015E5 RID: 5605 RVA: 0x000822C4 File Offset: 0x000804C4
		// (remove) Token: 0x060015E6 RID: 5606 RVA: 0x000822FC File Offset: 0x000804FC
		public event Action<IntVec3> RoofChanged;

		// Token: 0x060015E7 RID: 5607 RVA: 0x00082331 File Offset: 0x00080531
		public void Notify_RoofChanged(IntVec3 cell)
		{
			Action<IntVec3> roofChanged = this.RoofChanged;
			if (roofChanged == null)
			{
				return;
			}
			roofChanged(cell);
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060015E8 RID: 5608 RVA: 0x00082344 File Offset: 0x00080544
		// (remove) Token: 0x060015E9 RID: 5609 RVA: 0x0008237C File Offset: 0x0008057C
		public event Action<IntVec3> GlowChanged;

		// Token: 0x060015EA RID: 5610 RVA: 0x000823B1 File Offset: 0x000805B1
		public void Notify_GlowChanged(IntVec3 cell)
		{
			Action<IntVec3> glowChanged = this.GlowChanged;
			if (glowChanged == null)
			{
				return;
			}
			glowChanged(cell);
		}

		// Token: 0x040012CB RID: 4811
		public readonly Map map;
	}
}
