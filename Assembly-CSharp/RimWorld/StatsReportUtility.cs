using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CF0 RID: 7408
	public static class StatsReportUtility
	{
		// Token: 0x170022B0 RID: 8880
		// (get) Token: 0x0600BECD RID: 48845 RVA: 0x0045197E File Offset: 0x0044FB7E
		public static int SelectedStatIndex
		{
			get
			{
				if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>() || StatsReportUtility.selectedEntry == null)
				{
					return -1;
				}
				return StatsReportUtility.cachedDrawEntries.IndexOf(StatsReportUtility.selectedEntry);
			}
		}

		// Token: 0x170022B1 RID: 8881
		// (get) Token: 0x0600BECE RID: 48846 RVA: 0x004519A4 File Offset: 0x0044FBA4
		public static QuickSearchWidget QuickSearchWidget
		{
			get
			{
				return StatsReportUtility.quickSearchWidget;
			}
		}

		// Token: 0x0600BECF RID: 48847 RVA: 0x004519AC File Offset: 0x0044FBAC
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.scrollPositionRightPanel = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.scrollPositioner.Arm(false);
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
			StatsReportUtility.cachedEntryValues.Clear();
			StatsReportUtility.quickSearchWidget.Reset();
			PermitsCardUtility.selectedPermit = null;
			PermitsCardUtility.selectedFaction = ((ModLister.RoyaltyInstalled && Current.ProgramState == ProgramState.Playing) ? Faction.OfEmpire : null);
		}

		// Token: 0x0600BED0 RID: 48848 RVA: 0x00451A28 File Offset: 0x0044FC28
		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				BuildableDef buildableDef = def as BuildableDef;
				StatRequest req = (buildableDef != null) ? StatRequest.For(buildableDef, stuff, QualityCategory.Normal) : StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600BED1 RID: 48849 RVA: 0x00451AB4 File Offset: 0x0044FCB4
		public static void DrawStatsReport(Rect rect, AbilityDef def)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600BED2 RID: 48850 RVA: 0x00451B2C File Offset: 0x0044FD2C
		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats(StatRequest.For(thing)));
				StatsReportUtility.cachedDrawEntries.AddRange(StatsReportUtility.StatsToDraw(thing));
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => (de.stat != null && !de.stat.showNonAbstract) || !de.ShouldDisplay(thing));
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

		// Token: 0x0600BED3 RID: 48851 RVA: 0x00451BBC File Offset: 0x0044FDBC
		public static void DrawStatsReport(Rect rect, Hediff hediff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(hediff.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(hediff.def, null)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600BED4 RID: 48852 RVA: 0x00451C38 File Offset: 0x0044FE38
		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(worldObject)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		// Token: 0x0600BED5 RID: 48853 RVA: 0x00451CE0 File Offset: 0x0044FEE0
		public static void DrawStatsReport(Rect rect, RoyalTitleDef title, Faction faction, Pawn pawn = null)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(title.SpecialDisplayStats(StatRequest.For(title, faction, pawn)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(title, faction)
				where r.ShouldDisplay(pawn)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600BED6 RID: 48854 RVA: 0x00451D58 File Offset: 0x0044FF58
		public static void DrawStatsReport(Rect rect, Faction faction)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(faction.def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(faction)
				where r.ShouldDisplay(null)
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600BED7 RID: 48855 RVA: 0x00451DD3 File Offset: 0x0044FFD3
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			StatsReportUtility.<StatsToDraw>d__22 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__22(-2);
			<StatsToDraw>d__.<>3__def = def;
			<StatsToDraw>d__.<>3__stuff = stuff;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BED8 RID: 48856 RVA: 0x00451DEA File Offset: 0x0044FFEA
		private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
		{
			StatsReportUtility.<StatsToDraw>d__23 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__23(-2);
			<StatsToDraw>d__.<>3__title = title;
			<StatsToDraw>d__.<>3__faction = faction;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BED9 RID: 48857 RVA: 0x00451E01 File Offset: 0x00450001
		private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
		{
			StatsReportUtility.<StatsToDraw>d__24 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__24(-2);
			<StatsToDraw>d__.<>3__faction = faction;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BEDA RID: 48858 RVA: 0x00451E11 File Offset: 0x00450011
		private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
		{
			StatsReportUtility.<StatsToDraw>d__25 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__25(-2);
			<StatsToDraw>d__.<>3__def = def;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BEDB RID: 48859 RVA: 0x00451E21 File Offset: 0x00450021
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			StatsReportUtility.<StatsToDraw>d__26 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__26(-2);
			<StatsToDraw>d__.<>3__thing = thing;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BEDC RID: 48860 RVA: 0x00451E31 File Offset: 0x00450031
		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			StatsReportUtility.<StatsToDraw>d__27 <StatsToDraw>d__ = new StatsReportUtility.<StatsToDraw>d__27(-2);
			<StatsToDraw>d__.<>3__worldObject = worldObject;
			return <StatsToDraw>d__;
		}

		// Token: 0x0600BEDD RID: 48861 RVA: 0x00451E41 File Offset: 0x00450041
		private static IEnumerable<StatDrawEntry> StuffStats(ThingDef stuffDef)
		{
			StatsReportUtility.<StuffStats>d__28 <StuffStats>d__ = new StatsReportUtility.<StuffStats>d__28(-2);
			<StuffStats>d__.<>3__stuffDef = stuffDef;
			return <StuffStats>d__;
		}

		// Token: 0x0600BEDE RID: 48862 RVA: 0x00451E54 File Offset: 0x00450054
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
			StatsReportUtility.quickSearchWidget.noResultsMatched = !StatsReportUtility.cachedDrawEntries.Any<StatDrawEntry>();
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				StatsReportUtility.cachedEntryValues.Add(statDrawEntry.ValueString);
			}
			if (StatsReportUtility.selectedEntry != null)
			{
				StatsReportUtility.selectedEntry = StatsReportUtility.cachedDrawEntries.FirstOrDefault((StatDrawEntry e) => e.Same(StatsReportUtility.selectedEntry));
			}
			if (StatsReportUtility.quickSearchWidget.filter.Active)
			{
				foreach (StatDrawEntry sd2 in StatsReportUtility.cachedDrawEntries)
				{
					if (StatsReportUtility.Matches(sd2))
					{
						StatsReportUtility.selectedEntry = sd2;
						StatsReportUtility.scrollPositioner.Arm(true);
						break;
					}
				}
			}
		}

		// Token: 0x0600BEDF RID: 48863 RVA: 0x00451FD8 File Offset: 0x004501D8
		private static bool Matches(StatDrawEntry sd)
		{
			return StatsReportUtility.quickSearchWidget.filter.Matches(sd.LabelCap);
		}

		// Token: 0x0600BEE0 RID: 48864 RVA: 0x00451FF0 File Offset: 0x004501F0
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks), false, false);
		}

		// Token: 0x0600BEE1 RID: 48865 RVA: 0x00452034 File Offset: 0x00450234
		private static StatDrawEntry DescriptionEntry(Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks), false, false);
		}

		// Token: 0x0600BEE2 RID: 48866 RVA: 0x00452080 File Offset: 0x00450280
		private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)), false, false);
		}

		// Token: 0x0600BEE3 RID: 48867 RVA: 0x004520C8 File Offset: 0x004502C8
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.DescriptionHyperlinks), false, true);
		}

		// Token: 0x0600BEE4 RID: 48868 RVA: 0x0045210C File Offset: 0x0045030C
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999, null, null, false, false);
		}

		// Token: 0x0600BEE5 RID: 48869 RVA: 0x00452148 File Offset: 0x00450348
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999, null, null, false, false);
		}

		// Token: 0x0600BEE6 RID: 48870 RVA: 0x0045219D File Offset: 0x0045039D
		public static void SelectEntry(int index)
		{
			if (index < 0 || index > StatsReportUtility.cachedDrawEntries.Count)
			{
				return;
			}
			StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[index], true);
		}

		// Token: 0x0600BEE7 RID: 48871 RVA: 0x004521C4 File Offset: 0x004503C4
		public static void SelectEntry(StatDef stat, bool playSound = false)
		{
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				if (statDrawEntry.stat == stat)
				{
					StatsReportUtility.SelectEntry(statDrawEntry, playSound);
					return;
				}
			}
			Messages.Message("MessageCannotSelectInvisibleStat".Translate(stat), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600BEE8 RID: 48872 RVA: 0x00452244 File Offset: 0x00450444
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600BEE9 RID: 48873 RVA: 0x0045225C File Offset: 0x0045045C
		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect rect2 = new Rect(rect);
			rect2.width *= 0.5f;
			Rect rect3 = new Rect(rect);
			rect3.x = rect2.xMax;
			rect3.width = rect.xMax - rect3.x;
			StatsReportUtility.scrollPositioner.ClearInterestRects();
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(rect2, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			StatsReportUtility.mousedOverEntry = null;
			for (int i = 0; i < StatsReportUtility.cachedDrawEntries.Count; i++)
			{
				StatDrawEntry ent = StatsReportUtility.cachedDrawEntries[i];
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				bool highlightLabel = false;
				bool lowlightLabel = false;
				bool flag = StatsReportUtility.selectedEntry == ent;
				bool flag2 = false;
				GUI.color = Color.white;
				if (StatsReportUtility.quickSearchWidget.filter.Active)
				{
					if (StatsReportUtility.Matches(ent))
					{
						highlightLabel = true;
						flag2 = true;
					}
					else
					{
						lowlightLabel = true;
					}
				}
				Rect rect4 = new Rect(8f, num, viewRect.width - 8f, 30f);
				num += ent.Draw(rect4.x, rect4.y, rect4.width, flag, highlightLabel, lowlightLabel, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				}, delegate
				{
					StatsReportUtility.mousedOverEntry = ent;
				}, StatsReportUtility.scrollPosition, rect2, StatsReportUtility.cachedEntryValues[i]);
				rect4.yMax = num;
				if (flag || flag2)
				{
					StatsReportUtility.scrollPositioner.RegisterInterestRect(rect4);
				}
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			StatsReportUtility.scrollPositioner.ScrollVertically(ref StatsReportUtility.scrollPosition, rect2.size);
			Rect outRect = rect3.ContractedBy(10f);
			StatDrawEntry statDrawEntry;
			if ((statDrawEntry = StatsReportUtility.selectedEntry) == null)
			{
				statDrawEntry = (StatsReportUtility.mousedOverEntry ?? StatsReportUtility.cachedDrawEntries.FirstOrDefault<StatDrawEntry>());
			}
			StatDrawEntry statDrawEntry2 = statDrawEntry;
			if (statDrawEntry2 != null)
			{
				Rect rect5 = new Rect(0f, 0f, outRect.width - 16f, StatsReportUtility.rightPanelHeight);
				StatRequest statRequest;
				if (statDrawEntry2.hasOptionalReq)
				{
					statRequest = statDrawEntry2.optionalReq;
				}
				else if (optionalThing != null)
				{
					statRequest = StatRequest.For(optionalThing);
				}
				else
				{
					statRequest = StatRequest.ForEmpty();
				}
				string explanationText = statDrawEntry2.GetExplanationText(statRequest);
				float num2 = 0f;
				Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPositionRightPanel, rect5, true);
				Rect rect6 = rect5;
				rect6.width -= 4f;
				Widgets.Label(rect6, explanationText);
				float num3 = Text.CalcHeight(explanationText, rect6.width) + 10f;
				num2 += num3;
				IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = statDrawEntry2.GetHyperlinks(statRequest);
				if (hyperlinks != null)
				{
					Rect rect7 = new Rect(rect6.x, rect6.y + num3, rect6.width, rect6.height - num3);
					Color color = GUI.color;
					GUI.color = Widgets.NormalOptionColor;
					foreach (Dialog_InfoCard.Hyperlink hyperlink in hyperlinks)
					{
						float num4 = Text.CalcHeight(hyperlink.Label, rect7.width);
						Rect rect8 = new Rect(rect7.x, rect7.y, rect7.width, num4);
						string text = hyperlink.HasGeneOwnerThing ? hyperlink.Label : "ViewHyperlink".Translate(hyperlink.Label).ToString();
						Widgets.HyperlinkWithIcon(rect8, hyperlink, text, 2f, 6f, null, false, null);
						rect7.y += num4;
						rect7.height -= num4;
						num2 += num4;
					}
					GUI.color = color;
				}
				StatsReportUtility.rightPanelHeight = num2;
				Widgets.EndScrollView();
			}
		}

		// Token: 0x0600BEEA RID: 48874 RVA: 0x004526C8 File Offset: 0x004508C8
		public static void Notify_QuickSearchChanged()
		{
			StatsReportUtility.cachedDrawEntries.Clear();
			StatsReportUtility.cachedEntryValues.Clear();
		}

		// Token: 0x04006D97 RID: 28055
		private static StatDrawEntry selectedEntry;

		// Token: 0x04006D98 RID: 28056
		private static StatDrawEntry mousedOverEntry;

		// Token: 0x04006D99 RID: 28057
		private static Vector2 scrollPosition;

		// Token: 0x04006D9A RID: 28058
		private static ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x04006D9B RID: 28059
		private static Vector2 scrollPositionRightPanel;

		// Token: 0x04006D9C RID: 28060
		private static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04006D9D RID: 28061
		private static float listHeight;

		// Token: 0x04006D9E RID: 28062
		private static float rightPanelHeight;

		// Token: 0x04006D9F RID: 28063
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

		// Token: 0x04006DA0 RID: 28064
		private static List<string> cachedEntryValues = new List<string>();
	}
}
