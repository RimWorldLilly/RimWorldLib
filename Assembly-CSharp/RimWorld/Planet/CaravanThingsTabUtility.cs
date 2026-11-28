using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023EC RID: 9196
	[StaticConstructorOnStartup]
	public static class CaravanThingsTabUtility
	{
		// Token: 0x0600E5B0 RID: 58800 RVA: 0x005556BC File Offset: 0x005538BC
		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true, null))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
			}
		}

		// Token: 0x0600E5B1 RID: 58801 RVA: 0x00555754 File Offset: 0x00553954
		public static void DoAbandonButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true, null))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 8476546));
			}
		}

		// Token: 0x0600E5B2 RID: 58802 RVA: 0x005557EC File Offset: 0x005539EC
		public static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x0600E5B3 RID: 58803 RVA: 0x00555884 File Offset: 0x00553A84
		public static void DoAbandonSpecificCountButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x0600E5B4 RID: 58804 RVA: 0x0055591C File Offset: 0x00553B1C
		public static void DoOpenSpecificTabButton(Rect rowRect, Pawn p, ref Pawn specificTabForPawn)
		{
			Color baseColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonColor : Color.white;
			Color mouseoverColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonMouseoverColor : GenUI.MouseoverColor;
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.SpecificTabButtonTex, baseColor, mouseoverColor, true, null))
			{
				if (p == specificTabForPawn)
				{
					specificTabForPawn = null;
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					specificTabForPawn = p;
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "OpenSpecificTabButtonTip");
			GUI.color = Color.white;
		}

		// Token: 0x0600E5B5 RID: 58805 RVA: 0x005559C4 File Offset: 0x00553BC4
		public static void DoOpenSpecificTabButtonInvisible(Rect rect, Pawn pawn, ref Pawn specificTabForPawn)
		{
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (pawn == specificTabForPawn)
				{
					specificTabForPawn = null;
				}
				else
				{
					specificTabForPawn = pawn;
				}
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600E5B6 RID: 58806 RVA: 0x005559E8 File Offset: 0x00553BE8
		public static void DrawMass(TransferableImmutable transferable, Rect rect)
		{
			float num = 0f;
			for (int i = 0; i < transferable.things.Count; i++)
			{
				num += transferable.things[i].GetStatValue(StatDefOf.Mass, true, -1) * (float)transferable.things[i].stackCount;
			}
			CaravanThingsTabUtility.DrawMass(num, rect);
		}

		// Token: 0x0600E5B7 RID: 58807 RVA: 0x00555A46 File Offset: 0x00553C46
		public static void DrawMass(Thing thing, Rect rect)
		{
			CaravanThingsTabUtility.DrawMass(thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)thing.stackCount, rect);
		}

		// Token: 0x0600E5B8 RID: 58808 RVA: 0x00555A63 File Offset: 0x00553C63
		private static void DrawMass(float mass, Rect rect)
		{
			GUI.color = TransferableOneWayWidget.ItemMassColor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect, mass.ToStringMass());
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x040091F7 RID: 37367
		public const float MassColumnWidth = 60f;

		// Token: 0x040091F8 RID: 37368
		public const float SpaceAroundIcon = 4f;

		// Token: 0x040091F9 RID: 37369
		public const float SpecificTabButtonSize = 24f;

		// Token: 0x040091FA RID: 37370
		public const float AbandonButtonSize = 24f;

		// Token: 0x040091FB RID: 37371
		public const float AbandonSpecificCountButtonSize = 24f;

		// Token: 0x040091FC RID: 37372
		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		// Token: 0x040091FD RID: 37373
		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		// Token: 0x040091FE RID: 37374
		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		// Token: 0x040091FF RID: 37375
		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		// Token: 0x04009200 RID: 37376
		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);
	}
}
