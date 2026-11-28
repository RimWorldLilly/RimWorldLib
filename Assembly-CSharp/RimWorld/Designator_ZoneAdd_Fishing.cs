using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C84 RID: 7300
	public class Designator_ZoneAdd_Fishing : Designator_ZoneAdd
	{
		// Token: 0x170021D0 RID: 8656
		// (get) Token: 0x0600BA5E RID: 47710 RVA: 0x0041B0D8 File Offset: 0x004192D8
		protected override string NewZoneLabel
		{
			get
			{
				return "Zone_Fishing".Translate();
			}
		}

		// Token: 0x170021D1 RID: 8657
		// (get) Token: 0x0600BA5F RID: 47711 RVA: 0x0041B0E9 File Offset: 0x004192E9
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive && (ResearchProjectDefOf.Fishing.IsFinished || DebugSettings.godMode);
			}
		}

		// Token: 0x170021D2 RID: 8658
		// (get) Token: 0x0600BA60 RID: 47712 RVA: 0x0041B107 File Offset: 0x00419307
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BA61 RID: 47713 RVA: 0x0041B10C File Offset: 0x0041930C
		public Designator_ZoneAdd_Fishing()
		{
			this.zoneTypeToPlace = typeof(Zone_Fishing);
			this.defaultLabel = "Zone_Fishing".Translate();
			this.defaultDesc = "DesignatorFishingZoneDesc".Translate();
			this.tutorTag = "ZoneAdd_Fishing";
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Fishing", true);
		}

		// Token: 0x0600BA62 RID: 47714 RVA: 0x0041B198 File Offset: 0x00419398
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.GetWaterBodyType(base.Map) == WaterBodyType.None || c.GetTerrain(base.Map).passability == Traversability.Impassable || !base.Map.waterBodyTracker.AnyFishPopulationAt(c))
			{
				return "CannotPlaceFishingZone".Translate();
			}
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600BA63 RID: 47715 RVA: 0x0041B218 File Offset: 0x00419418
		protected override Zone MakeNewZone()
		{
			if (!ModLister.CheckOdyssey("Fishing"))
			{
				return null;
			}
			return new Zone_Fishing(Find.CurrentMap.zoneManager);
		}

		// Token: 0x170021D3 RID: 8659
		// (get) Token: 0x0600BA64 RID: 47716 RVA: 0x0041B237 File Offset: 0x00419437
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Designator_ZoneAdd_Fishing.<get_RightClickFloatMenuOptions>d__10 <get_RightClickFloatMenuOptions>d__ = new Designator_ZoneAdd_Fishing.<get_RightClickFloatMenuOptions>d__10(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}

		// Token: 0x0600BA65 RID: 47717 RVA: 0x0041B248 File Offset: 0x00419448
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (!ModLister.CheckOdyssey("Fishing"))
			{
				return;
			}
			try
			{
				bool flag = false;
				foreach (IntVec3 intVec in cells)
				{
					if (!flag && this.CanDesignateCell(intVec))
					{
						flag = true;
					}
					WaterBodyType waterBodyType = intVec.GetWaterBodyType(base.Map);
					if (waterBodyType == WaterBodyType.Saltwater)
					{
						this.saltWaterCells.Add(intVec);
					}
					else if (waterBodyType == WaterBodyType.Freshwater)
					{
						this.freshWaterCells.Add(intVec);
					}
				}
				if (!flag)
				{
					base.Finalize(false);
				}
				else
				{
					Zone_Fishing zone_Fishing = Find.Selector.SelectedZone as Zone_Fishing;
					WaterBodyType? waterBodyType2 = (zone_Fishing != null) ? new WaterBodyType?(zone_Fishing.Cells[0].GetWaterBodyType(base.Map)) : null;
					if (zone_Fishing != null)
					{
						this.selectedZones.Add(zone_Fishing);
					}
					if (waterBodyType2.GetValueOrDefault() != WaterBodyType.Saltwater)
					{
						Find.Selector.ClearSelection();
					}
					if (this.saltWaterCells.Count > 0)
					{
						base.DesignateMultiCell(this.saltWaterCells);
					}
					Zone_Fishing zone_Fishing2 = Find.Selector.SelectedZone as Zone_Fishing;
					if (zone_Fishing2 != null && zone_Fishing2 != zone_Fishing)
					{
						this.selectedZones.Add(zone_Fishing2);
					}
					if (waterBodyType2.GetValueOrDefault() == WaterBodyType.Freshwater)
					{
						Find.Selector.Select(zone_Fishing, false, false);
					}
					else
					{
						Find.Selector.ClearSelection();
					}
					if (this.freshWaterCells.Count > 0)
					{
						base.DesignateMultiCell(this.freshWaterCells);
					}
					Zone_Fishing zone_Fishing3 = Find.Selector.SelectedZone as Zone_Fishing;
					if (zone_Fishing3 != null && zone_Fishing3 != zone_Fishing)
					{
						this.selectedZones.Add(zone_Fishing3);
					}
					Find.Selector.ClearSelection();
					foreach (Zone obj in this.selectedZones)
					{
						Find.Selector.Select(obj, false, false);
					}
				}
			}
			finally
			{
				this.saltWaterCells.Clear();
				this.freshWaterCells.Clear();
				this.selectedZones.Clear();
			}
		}

		// Token: 0x0600BA66 RID: 47718 RVA: 0x0041B4A0 File Offset: 0x004196A0
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("CannotPlaceFishingZone".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x040069F2 RID: 27122
		private readonly List<IntVec3> freshWaterCells = new List<IntVec3>();

		// Token: 0x040069F3 RID: 27123
		private readonly List<IntVec3> saltWaterCells = new List<IntVec3>();

		// Token: 0x040069F4 RID: 27124
		private readonly List<Zone> selectedZones = new List<Zone>();
	}
}
