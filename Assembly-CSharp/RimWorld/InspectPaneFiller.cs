using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5D RID: 7517
	[StaticConstructorOnStartup]
	public class InspectPaneFiller
	{
		// Token: 0x0600C2AC RID: 49836 RVA: 0x0046F054 File Offset: 0x0046D254
		public static void DoPaneContentsFor(ISelectable sel, Rect rect)
		{
			try
			{
				Widgets.BeginGroup(rect);
				float num = 0f;
				Pawn pawn = sel as Pawn;
				Thing thing = sel as Thing;
				if (thing != null && !thing.def.onlyShowInspectString)
				{
					num += 3f;
					WidgetRow row = new WidgetRow(0f, num, UIDirection.RightThenUp, 99999f, 4f);
					InspectPaneFiller.DrawHealth(row, thing);
					if (pawn != null)
					{
						if (pawn.IsGhoul && pawn.needs.food != null)
						{
							InspectPaneFiller.DrawHunger(row, pawn);
						}
						else
						{
							InspectPaneFiller.DrawMood(row, pawn);
						}
						if (pawn.timetable != null && !pawn.IsPrisonerOfColony)
						{
							InspectPaneFiller.DrawTimetableSetting(row, pawn);
						}
						InspectPaneFiller.DrawAreaAllowed(row, pawn);
						Pawn_NeedsTracker needs = pawn.needs;
						if (((needs != null) ? needs.energy : null) != null)
						{
							InspectPaneFiller.DrawMechEnergy(row, pawn);
						}
					}
					num += 18f;
				}
				Rect rect2 = rect.AtZero();
				rect2.yMin = num;
				InspectPaneFiller.DrawInspectStringFor(sel, rect2);
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error in DoPaneContentsFor ");
				defaultInterpolatedStringHandler.AppendFormatted<object>(Find.Selector.FirstSelectedObject);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 754672);
			}
			finally
			{
				Widgets.EndGroup();
			}
		}

		// Token: 0x0600C2AD RID: 49837 RVA: 0x0046F1CC File Offset: 0x0046D3CC
		public static void DoPaneContentsForStorageGroup(StorageGroup group, Rect rect)
		{
			Widgets.BeginGroup(rect);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StorageGroupLabel".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(group.RenamableLabel.CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (group.MemberCount > 1)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("(");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NumBuildings".Translate(group.MemberCount));
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			else
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("(");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("OneBuilding".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(")");
				text = str2 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			InspectPaneFiller.DrawInspectString(text, rect.AtZero());
			Widgets.EndGroup();
		}

		// Token: 0x0600C2AE RID: 49838 RVA: 0x0046F2E0 File Offset: 0x0046D4E0
		public static void DrawHealth(WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float fillPct;
			string label;
			if (pawn == null)
			{
				if (!t.def.useHitPoints)
				{
					return;
				}
				if (t.HitPoints >= t.MaxHitPoints)
				{
					GUI.color = Color.white;
				}
				else if ((float)t.HitPoints > (float)t.MaxHitPoints * 0.5f)
				{
					GUI.color = Color.yellow;
				}
				else if (t.HitPoints > 0)
				{
					GUI.color = Color.red;
				}
				else
				{
					GUI.color = Color.grey;
				}
				fillPct = (float)t.HitPoints / (float)t.MaxHitPoints;
				label = t.HitPoints.ToStringCached() + " / " + t.MaxHitPoints.ToStringCached();
			}
			else
			{
				GUI.color = Color.white;
				fillPct = pawn.health.summaryHealth.SummaryHealthPercent;
				label = HealthUtility.GetGeneralConditionLabel(pawn, true);
			}
			row.FillableBar(93f, 16f, fillPct, label, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.color = Color.white;
		}

		// Token: 0x0600C2AF RID: 49839 RVA: 0x0046F3E0 File Offset: 0x0046D5E0
		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0600C2B0 RID: 49840 RVA: 0x0046F450 File Offset: 0x0046D650
		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		// Token: 0x0600C2B1 RID: 49841 RVA: 0x0046F4A4 File Offset: 0x0046D6A4
		private static void DrawAreaAllowed(WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings == null || !pawn.playerSettings.SupportsAllowedAreas)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.HostFaction != null)
			{
				return;
			}
			if (pawn.IsMutant && !pawn.mutant.Def.respectsAllowedArea)
			{
				return;
			}
			row.Gap(6f);
			Pawn_PlayerSettings playerSettings = pawn.playerSettings;
			bool flag = ((playerSettings != null) ? playerSettings.AreaRestrictionInPawnCurrentMap : null) != null;
			Texture2D fillTex;
			if (flag)
			{
				fillTex = pawn.playerSettings.AreaRestrictionInPawnCurrentMap.ColorTexture;
			}
			else
			{
				fillTex = BaseContent.GreyTex;
			}
			Rect rect = row.FillableBar(93f, 16f, 1f, AreaUtility.AreaAllowedLabel(pawn), fillTex, null);
			if (Mouse.IsOver(rect))
			{
				if (flag)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap.MarkForDraw();
				}
				Widgets.DrawBox(rect.ContractedBy(-1f), 1, null);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap = a;
				}, true, true, pawn.MapHeld);
			}
		}

		// Token: 0x0600C2B2 RID: 49842 RVA: 0x0046F5E8 File Offset: 0x0046D7E8
		private static void DrawMechEnergy(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.energy.CurLevelPercentage, "Energy".Translate(), InspectPaneFiller.EnergyTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0600C2B3 RID: 49843 RVA: 0x0046F63C File Offset: 0x0046D83C
		private static void DrawHunger(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.food.CurLevelPercentage, pawn.needs.food.CurCategory.GetLabel(), InspectPaneFiller.HungerTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x0600C2B4 RID: 49844 RVA: 0x0046F694 File Offset: 0x0046D894
		public static void DrawInspectStringFor(ISelectable sel, Rect rect)
		{
			string text;
			try
			{
				text = sel.GetInspectString();
				Thing thing = sel as Thing;
				if (thing != null)
				{
					string inspectStringLowPriority = thing.GetInspectStringLowPriority();
					if (!inspectStringLowPriority.NullOrEmpty())
					{
						if (!text.NullOrEmpty())
						{
							text = text.TrimEndNewlines() + "\n";
						}
						text += inspectStringLowPriority;
					}
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("GetInspectString exception on ");
				defaultInterpolatedStringHandler.AppendFormatted<ISelectable>(sel);
				defaultInterpolatedStringHandler.AppendLiteral(":\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (!InspectPaneFiller.debug_inspectStringExceptionErrored)
				{
					Log.Error(text);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Inspect string for ");
				defaultInterpolatedStringHandler2.AppendFormatted<ISelectable>(sel);
				defaultInterpolatedStringHandler2.AppendLiteral(" contains empty lines.\n\nSTART\n");
				defaultInterpolatedStringHandler2.AppendFormatted(text);
				defaultInterpolatedStringHandler2.AppendLiteral("\nEND");
				Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), 837163521);
			}
			InspectPaneFiller.DrawInspectString(text, rect);
		}

		// Token: 0x0600C2B5 RID: 49845 RVA: 0x0046F7AC File Offset: 0x0046D9AC
		public static void DrawInspectString(string str, Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.LabelScrollable(rect, str, ref InspectPaneFiller.inspectStringScrollPos, true, true, false);
		}

		// Token: 0x04006F49 RID: 28489
		private const float BarHeight = 16f;

		// Token: 0x04006F4A RID: 28490
		private static readonly Texture2D MoodTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(26, 52, 52).ToColor);

		// Token: 0x04006F4B RID: 28491
		private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

		// Token: 0x04006F4C RID: 28492
		private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(35, 35, 35).ToColor);

		// Token: 0x04006F4D RID: 28493
		private static readonly Texture2D EnergyTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 52, 75).ToColor);

		// Token: 0x04006F4E RID: 28494
		private static readonly Texture2D HungerTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(184, 156, 90).ToColor);

		// Token: 0x04006F4F RID: 28495
		private const float BarWidth = 93f;

		// Token: 0x04006F50 RID: 28496
		private const float BarSpacing = 6f;

		// Token: 0x04006F51 RID: 28497
		private static bool debug_inspectStringExceptionErrored;

		// Token: 0x04006F52 RID: 28498
		private static Vector2 inspectStringScrollPos;
	}
}
