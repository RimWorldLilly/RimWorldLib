using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C80 RID: 7296
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		// Token: 0x0600BA52 RID: 47698 RVA: 0x0041AEC8 File Offset: 0x004190C8
		public Designator_ZoneAddStockpile_Dumping()
		{
			this.preset = StorageSettingsPreset.DumpingStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_DumpingStockpile", true);
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Dumping;
		}

		// Token: 0x0600BA53 RID: 47699 RVA: 0x0041AF24 File Offset: 0x00419124
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
