using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C82 RID: 7298
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x170021CC RID: 8652
		// (get) Token: 0x0600BA56 RID: 47702 RVA: 0x0041AF62 File Offset: 0x00419162
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x170021CD RID: 8653
		// (get) Token: 0x0600BA57 RID: 47703 RVA: 0x0041AF73 File Offset: 0x00419173
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BA58 RID: 47704 RVA: 0x0041AF78 File Offset: 0x00419178
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.tutorTag = "ZoneAdd_Growing";
			this.hotKey = KeyBindingDefOf.Misc2;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Growing;
		}

		// Token: 0x0600BA59 RID: 47705 RVA: 0x0041AFF8 File Offset: 0x004191F8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			float num = ModsConfig.BiotechActive ? 0.5f : ThingDefOf.Plant_Potato.plant.fertilityMin;
			if (ModsConfig.IdeologyActive && BuildCopyCommandUtility.FindAllowedDesignator(TerrainDefOf.FungalGravel, true) != null)
			{
				num = Mathf.Min(num, ThingDefOf.Plant_Nutrifungus.plant.fertilityMin);
			}
			if (c.GetFertility(base.Map) < num)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600BA5A RID: 47706 RVA: 0x0041B081 File Offset: 0x00419281
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}

		// Token: 0x170021CE RID: 8654
		// (get) Token: 0x0600BA5B RID: 47707 RVA: 0x0041B09D File Offset: 0x0041929D
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Designator_ZoneAdd_Growing.<get_RightClickFloatMenuOptions>d__8 <get_RightClickFloatMenuOptions>d__ = new Designator_ZoneAdd_Growing.<get_RightClickFloatMenuOptions>d__8(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}
	}
}
