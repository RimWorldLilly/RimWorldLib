using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7E RID: 7294
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x170021C8 RID: 8648
		// (get) Token: 0x0600BA49 RID: 47689 RVA: 0x0041AD5E File Offset: 0x00418F5E
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x170021C9 RID: 8649
		// (get) Token: 0x0600BA4A RID: 47690 RVA: 0x0041AD6B File Offset: 0x00418F6B
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BA4B RID: 47691 RVA: 0x0041AD6E File Offset: 0x00418F6E
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x0600BA4C RID: 47692 RVA: 0x0041AD85 File Offset: 0x00418F85
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Stockpile;
		}

		// Token: 0x0600BA4D RID: 47693 RVA: 0x0041ADA8 File Offset: 0x00418FA8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (c.GetTerrain(base.Map).passability == Traversability.Impassable)
			{
				return false;
			}
			List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.CanOverlapZones)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BA4E RID: 47694 RVA: 0x0041AE26 File Offset: 0x00419026
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}

		// Token: 0x170021CA RID: 8650
		// (get) Token: 0x0600BA4F RID: 47695 RVA: 0x0041AE39 File Offset: 0x00419039
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Designator_ZoneAddStockpile.<get_RightClickFloatMenuOptions>d__10 <get_RightClickFloatMenuOptions>d__ = new Designator_ZoneAddStockpile.<get_RightClickFloatMenuOptions>d__10(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}

		// Token: 0x040069F1 RID: 27121
		protected StorageSettingsPreset preset;
	}
}
