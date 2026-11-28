using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001640 RID: 5696
	public class IdeoBuildingPresenceDemand : IExposable, ILoadReferenceable
	{
		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06008EA7 RID: 36519 RVA: 0x00346DC8 File Offset: 0x00344FC8
		public List<string> RoomRequirementsInfo
		{
			get
			{
				if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return null;
				}
				if (this.roomRequirementsInfoCached == null || Find.TickManager.TicksGame - this.roomRequirementsInfoCacheTick >= 20)
				{
					if (this.roomRequirementsInfoCached == null)
					{
						this.roomRequirementsInfoCached = new List<string>();
					}
					else
					{
						this.roomRequirementsInfoCached.Clear();
					}
					Thing building = (Find.CurrentMap != null) ? this.BestBuilding(Find.CurrentMap, false) : null;
					Room effectiveRoom = this.GetEffectiveRoom(building);
					foreach (RoomRequirement roomRequirement in this.roomRequirements)
					{
						if (effectiveRoom == null || !roomRequirement.MetOrDisabled(effectiveRoom, null))
						{
							this.roomRequirementsInfoCached.Add(roomRequirement.LabelCap(effectiveRoom));
						}
					}
					this.roomRequirementsInfoCacheTick = Find.TickManager.TicksGame;
				}
				return this.roomRequirementsInfoCached;
			}
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06008EA8 RID: 36520 RVA: 0x00346EB8 File Offset: 0x003450B8
		public Alert_IdeoBuildingMissing AlertCachedMissingMissing
		{
			get
			{
				if (this.alertCachedMissing == null)
				{
					this.alertCachedMissing = new Alert_IdeoBuildingMissing(this);
				}
				return this.alertCachedMissing;
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06008EA9 RID: 36521 RVA: 0x00346ED4 File Offset: 0x003450D4
		public Alert_IdeoBuildingDisrespected AlertCachedMissingDisrespected
		{
			get
			{
				if (this.alertCachedDisrespected == null)
				{
					this.alertCachedDisrespected = new Alert_IdeoBuildingDisrespected(this);
				}
				return this.alertCachedDisrespected;
			}
		}

		// Token: 0x06008EAA RID: 36522 RVA: 0x00346EF0 File Offset: 0x003450F0
		public IdeoBuildingPresenceDemand()
		{
		}

		// Token: 0x06008EAB RID: 36523 RVA: 0x00346F18 File Offset: 0x00345118
		public IdeoBuildingPresenceDemand(Precept_Building precept)
		{
			this.parent = precept;
			this.ID = Find.UniqueIDsManager.GetNextPresenceDemandID();
		}

		// Token: 0x06008EAC RID: 36524 RVA: 0x00346F58 File Offset: 0x00345158
		public bool AppliesTo(Map map)
		{
			return map.IsPlayerHome && (this.parent.ThingDef.ritualFocus == null || !this.parent.ThingDef.ritualFocus.consumable) && (DebugSettings.activateAllBuildingDemands || ExpectationsUtility.CurrentExpectationFor(map).order >= this.minExpectation.order);
		}

		// Token: 0x06008EAD RID: 36525 RVA: 0x00346FC0 File Offset: 0x003451C0
		public bool BuildingPresent(Map map)
		{
			Faction playerFact = Faction.OfPlayer;
			return map.listerThings.ThingsOfDef(this.parent.ThingDef).Any(delegate(Thing t)
			{
				Building building = t as Building;
				return building != null && building.StyleSourcePrecept == this.parent && building.Faction == playerFact;
			});
		}

		// Token: 0x06008EAE RID: 36526 RVA: 0x0034700C File Offset: 0x0034520C
		public bool RequirementsSatisfied(Map map)
		{
			if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(this.parent.ThingDef))
			{
				Building building = thing as Building;
				if (building != null && building.StyleSourcePrecept == this.parent && building.GetRoom(RegionType.Set_All) != null && this.NumRequirementsMet(building) == this.roomRequirements.Count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008EAF RID: 36527 RVA: 0x003470B4 File Offset: 0x003452B4
		private int NumRequirementsMet(Thing building)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.buildingRequirementsMetCacheTick != ticksGame)
			{
				this.buildingRequirementsMetCache.Clear();
				this.buildingRequirementsMetCacheTick = ticksGame;
			}
			int num;
			if (this.buildingRequirementsMetCache.TryGetValue(building, out num))
			{
				return num;
			}
			num = this.NumRequirementsMetCalc(building);
			this.buildingRequirementsMetCache[building] = num;
			return num;
		}

		// Token: 0x06008EB0 RID: 36528 RVA: 0x00347110 File Offset: 0x00345310
		private int NumRequirementsMetCalc(Thing building)
		{
			if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return 0;
			}
			Room effectiveRoom = this.GetEffectiveRoom(building);
			if (effectiveRoom == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.roomRequirements.Count; i++)
			{
				if (this.roomRequirements[i].MetOrDisabled(effectiveRoom, null))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008EB1 RID: 36529 RVA: 0x0034716C File Offset: 0x0034536C
		public Room GetEffectiveRoom(Thing building)
		{
			Room room = (building != null) ? building.GetRoom(RegionType.Set_All) : null;
			if (room == null || room.PsychologicallyOutdoors)
			{
				return null;
			}
			return room;
		}

		// Token: 0x06008EB2 RID: 36530 RVA: 0x00347198 File Offset: 0x00345398
		public Thing BestBuilding(Map map, bool ignoreFullyMetRequirements = false)
		{
			Thing result = null;
			int num = -1;
			foreach (Thing thing in map.listerThings.ThingsOfDef(this.parent.ThingDef))
			{
				Building building = thing as Building;
				if (building != null && building.StyleSourcePrecept == this.parent)
				{
					int num2 = this.NumRequirementsMet(building);
					if (this.roomRequirements == null || num2 != this.roomRequirements.Count || !ignoreFullyMetRequirements)
					{
						if (this.GetEffectiveRoom(building) != null)
						{
							num2++;
						}
						if (num2 > num)
						{
							result = building;
							num = num2;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06008EB3 RID: 36531 RVA: 0x00347250 File Offset: 0x00345450
		public IEnumerable<Thing> AllBuildings(Map map)
		{
			IdeoBuildingPresenceDemand.<AllBuildings>d__25 <AllBuildings>d__ = new IdeoBuildingPresenceDemand.<AllBuildings>d__25(-2);
			<AllBuildings>d__.<>4__this = this;
			<AllBuildings>d__.<>3__map = map;
			return <AllBuildings>d__;
		}

		// Token: 0x06008EB4 RID: 36532 RVA: 0x00347268 File Offset: 0x00345468
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Defs.Look<ExpectationDef>(ref this.minExpectation, "minExpectation");
			Scribe_Collections.Look<RoomRequirement>(ref this.roomRequirements, "roomRequirements", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
				{
					this.ID = Find.UniqueIDsManager.GetNextPresenceDemandID();
				}
				if (this.roomRequirements != null)
				{
					if (this.roomRequirements.RemoveAll((RoomRequirement x) => x == null) != 0)
					{
						Log.Error("Removing null room requirements.");
					}
				}
			}
		}

		// Token: 0x06008EB5 RID: 36533 RVA: 0x0034730A File Offset: 0x0034550A
		public string GetUniqueLoadID()
		{
			return "IdeoBuildingPresenceDemand_" + this.ID.ToString();
		}

		// Token: 0x06008EB6 RID: 36534 RVA: 0x00347321 File Offset: 0x00345521
		public IdeoBuildingPresenceDemand Copy()
		{
			return new IdeoBuildingPresenceDemand
			{
				ID = this.ID,
				parent = this.parent,
				minExpectation = this.minExpectation,
				roomRequirements = this.roomRequirements
			};
		}

		// Token: 0x0400576C RID: 22380
		public Precept_Building parent;

		// Token: 0x0400576D RID: 22381
		public ExpectationDef minExpectation;

		// Token: 0x0400576E RID: 22382
		public List<RoomRequirement> roomRequirements;

		// Token: 0x0400576F RID: 22383
		public int ID = -1;

		// Token: 0x04005770 RID: 22384
		private List<string> roomRequirementsInfoCached;

		// Token: 0x04005771 RID: 22385
		private int roomRequirementsInfoCacheTick = -1;

		// Token: 0x04005772 RID: 22386
		private Dictionary<Thing, int> buildingRequirementsMetCache = new Dictionary<Thing, int>();

		// Token: 0x04005773 RID: 22387
		private int buildingRequirementsMetCacheTick = -1;

		// Token: 0x04005774 RID: 22388
		private Alert_IdeoBuildingMissing alertCachedMissing;

		// Token: 0x04005775 RID: 22389
		private Alert_IdeoBuildingDisrespected alertCachedDisrespected;
	}
}
