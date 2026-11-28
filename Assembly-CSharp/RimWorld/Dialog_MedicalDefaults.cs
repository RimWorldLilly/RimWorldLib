using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB8 RID: 7352
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x17002260 RID: 8800
		// (get) Token: 0x0600BCC8 RID: 48328 RVA: 0x00439292 File Offset: 0x00437492
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(406f, 640f);
			}
		}

		// Token: 0x17002261 RID: 8801
		// (get) Token: 0x0600BCC9 RID: 48329 RVA: 0x004392A3 File Offset: 0x004374A3
		public override string CloseButtonText
		{
			get
			{
				return "OK".Translate();
			}
		}

		// Token: 0x0600BCCA RID: 48330 RVA: 0x004392B4 File Offset: 0x004374B4
		public Dialog_MedicalDefaults() : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BCCB RID: 48331 RVA: 0x004392E0 File Offset: 0x004374E0
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(inRect, ref num, "DefaultMedicineSettings".Translate(), default(TipSignal));
			}
			Text.Font = GameFont.Small;
			Widgets.Label(inRect, ref num, "DefaultMedicineSettingsDesc".Translate(), default(TipSignal));
			num += 10f;
			Text.Anchor = TextAnchor.MiddleLeft;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForColonist, "MedGroupColonists", "MedGroupColonistsDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForPrisoner, "MedGroupPrisoners", "MedGroupPrisonersDesc");
			if (ModsConfig.IdeologyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForSlave, "MedGroupSlaves", "MedGroupSlavesDesc");
			}
			if (ModsConfig.AnomalyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForGhouls, "MedGroupGhouls", "MedGroupGhoulsDesc");
			}
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForTamedAnimal, "MedGroupTamedAnimals", "MedGroupTamedAnimalsDesc");
			num += 17f;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForFriendlyFaction, "MedGroupFriendlyFaction", "MedGroupFriendlyFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForNeutralFaction, "MedGroupNeutralFaction", "MedGroupNeutralFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForHostileFaction, "MedGroupHostileFaction", "MedGroupHostileFactionDesc");
			num += 17f;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForNoFaction, "MedGroupNoFaction", "MedGroupNoFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForWildlife, "MedGroupWildlife", "MedGroupWildlifeDesc");
			if (ModsConfig.AnomalyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForEntities, "MedGroupEntities", "MedGroupEntitiesDesc");
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600BCCC RID: 48332 RVA: 0x004394DC File Offset: 0x004376DC
		private void DoRow(Rect rect, ref float y, ref MedicalCareCategory category, string labelKey, string tipKey)
		{
			Rect rect2 = new Rect(rect.x, y, rect.width, 28f);
			Rect rect3 = new Rect(rect.x, y, 230f, 28f);
			Rect rect4 = new Rect(230f, y, 140f, 28f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawLightHighlight(rect2);
			}
			TooltipHandler.TipRegionByKey(rect2, tipKey);
			Widgets.LabelFit(rect3, labelKey.Translate());
			MedicalCareUtility.MedicalCareSetter(rect4, ref category);
			y += 34f;
		}

		// Token: 0x04006BEC RID: 27628
		private const float MedicalCareLabelWidth = 230f;

		// Token: 0x04006BED RID: 27629
		private const float VerticalGap = 6f;
	}
}
