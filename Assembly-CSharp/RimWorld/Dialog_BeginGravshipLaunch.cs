using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C93 RID: 7315
	public class Dialog_BeginGravshipLaunch : Dialog_BeginRitual
	{
		// Token: 0x0600BABE RID: 47806 RVA: 0x0041F2C8 File Offset: 0x0041D4C8
		public Dialog_BeginGravshipLaunch(string ritualLabel, Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.ActionCallback action, Pawn organizer, RitualObligation obligation, Dialog_BeginRitual.PawnFilter filter = null, string okButtonText = null, List<Pawn> requiredPawns = null, Dictionary<string, Pawn> forcedForRole = null, RitualOutcomeEffectDef outcome = null, List<string> extraInfoText = null, Pawn selectedPawn = null) : base(ritualLabel, ritual, target, map, action, organizer, obligation, filter, okButtonText, requiredPawns, forcedForRole, outcome, extraInfoText, selectedPawn)
		{
		}

		// Token: 0x0600BABF RID: 47807 RVA: 0x0041F30C File Offset: 0x0041D50C
		protected override void Start()
		{
			RitualBehaviorWorker_GravshipLaunch ritualBehaviorWorker_GravshipLaunch = this.ritual.behavior as RitualBehaviorWorker_GravshipLaunch;
			ritualBehaviorWorker_GravshipLaunch.forceVisitorsToLeave = this.forceVisitorsToLeave;
			ritualBehaviorWorker_GravshipLaunch.boardColonyAnimals = this.boardColonyAnimals;
			ritualBehaviorWorker_GravshipLaunch.boardColonyMechs = this.boardColonyMechs;
			Dialog_BeginRitual.ActionCallback action = this.action;
			if (action != null && action(this.assignments))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600BAC0 RID: 47808 RVA: 0x0041F370 File Offset: 0x0041D570
		public override void DoRightColumn(ref RectDivider layout)
		{
			float height = ModsConfig.BiotechActive ? 84f : 56f;
			Rect rect = layout.NewRow(height, VerticalJustification.Top, null).Rect;
			Rect rect2 = new Rect(rect.x, rect.y, 402f, 28f);
			Widgets.CheckboxLabeled(rect2, "GravshipForceVisitorsToLeaveLabel".Translate(), ref this.forceVisitorsToLeave, false, null, null, false, false);
			TooltipHandler.TipRegion(rect2, "GravshipForceVisitorsToLeaveTooltip".Translate());
			rect2.y += 28f;
			Widgets.CheckboxLabeled(rect2, "GravshipBoardColonyAnimalsLabel".Translate(), ref this.boardColonyAnimals, false, null, null, false, false);
			TooltipHandler.TipRegion(rect2, "GravshipBoardColonyAnimalsTooltip".Translate());
			if (ModsConfig.BiotechActive)
			{
				rect2.y += 28f;
				Widgets.CheckboxLabeled(rect2, "GravshipBoardColonyMechsLabel".Translate(), ref this.boardColonyMechs, false, null, null, false, false);
				TooltipHandler.TipRegion(rect2, "GravshipBoardColonyMechsTooltip".Translate());
			}
			layout.NewRow(17f, VerticalJustification.Top, null);
			base.DoRightColumn(ref layout);
		}

		// Token: 0x04006A2C RID: 27180
		private bool forceVisitorsToLeave = true;

		// Token: 0x04006A2D RID: 27181
		private bool boardColonyAnimals = true;

		// Token: 0x04006A2E RID: 27182
		private bool boardColonyMechs = true;

		// Token: 0x04006A2F RID: 27183
		private const float CheckboxHeight = 28f;
	}
}
