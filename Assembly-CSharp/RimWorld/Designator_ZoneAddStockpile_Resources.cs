using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7F RID: 7295
	public class Designator_ZoneAddStockpile_Resources : Designator_ZoneAddStockpile
	{
		// Token: 0x0600BA50 RID: 47696 RVA: 0x0041AE4C File Offset: 0x0041904C
		public Designator_ZoneAddStockpile_Resources()
		{
			this.preset = StorageSettingsPreset.DefaultStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageResourcesDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			this.hotKey = KeyBindingDefOf.Misc1;
			this.tutorTag = "ZoneAddStockpile_Resources";
		}

		// Token: 0x0600BA51 RID: 47697 RVA: 0x0041AEB3 File Offset: 0x004190B3
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
