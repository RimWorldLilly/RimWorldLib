using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001D5E RID: 7518
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x0600C2B8 RID: 49848 RVA: 0x0046F86F File Offset: 0x0046DA6F
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x0600C2B9 RID: 49849 RVA: 0x0046F87C File Offset: 0x0046DA7C
		public static float PaneWidthFor(IInspectPane pane)
		{
			if (pane == null)
			{
				return 432f;
			}
			InspectPaneUtility.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.visible = 0;
			if (pane.CurTabs != null)
			{
				IList list = pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in pane.CurTabs)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			return 72f * (float)Mathf.Max(6, CS$<>8__locals1.visible);
		}

		// Token: 0x0600C2BA RID: 49850 RVA: 0x0046F92C File Offset: 0x0046DB2C
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x0600C2BB RID: 49851 RVA: 0x0046F940 File Offset: 0x0046DB40
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x0600C2BC RID: 49852 RVA: 0x0046F984 File Offset: 0x0046DB84
		public static string AdjustedLabelFor(List<object> selected, Rect rect)
		{
			IRenameable renameable = selected[0] as IRenameable;
			string text;
			if (renameable != null)
			{
				if (selected.Count == 1)
				{
					text = renameable.InspectLabel;
				}
				else
				{
					string baseLabel = renameable.BaseLabel;
					bool flag = true;
					for (int i = 1; i < selected.Count; i++)
					{
						IRenameable renameable2 = selected[i] as IRenameable;
						if (renameable2 != null && renameable2.BaseLabel != baseLabel)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						text = renameable.BaseLabel;
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					text = text + " x" + selected.Count.ToString();
				}
			}
			else
			{
				InspectPaneUtility.selectedThings.Clear();
				for (int j = 0; j < selected.Count; j++)
				{
					Thing thing = selected[j] as Thing;
					if (thing != null)
					{
						InspectPaneUtility.selectedThings.Add(thing.GetInnerIfMinified());
					}
				}
				if (InspectPaneUtility.selectedThings.Count == 1)
				{
					text = InspectPaneUtility.selectedThings[0].LabelCap;
				}
				else if (InspectPaneUtility.selectedThings.Count > 1)
				{
					string b = InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[0]);
					bool flag2 = true;
					for (int k = 1; k < InspectPaneUtility.selectedThings.Count; k++)
					{
						if (InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[k]) != b)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						text = InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[0]).CapitalizeFirst();
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					int num = 0;
					for (int l = 0; l < InspectPaneUtility.selectedThings.Count; l++)
					{
						num += InspectPaneUtility.selectedThings[l].stackCount;
					}
					text = text + " x" + num.ToStringCached();
				}
				else
				{
					text = "?";
				}
				InspectPaneUtility.selectedThings.Clear();
			}
			string result;
			using (new TextBlock(GameFont.Medium))
			{
				result = text.Truncate(rect.width, InspectPaneUtility.truncatedLabelsCached);
			}
			return result;
		}

		// Token: 0x0600C2BD RID: 49853 RVA: 0x0046FBC4 File Offset: 0x0046DDC4
		private static string PluralLabelFor(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsMutant)
			{
				return pawn.mutant.Def.label;
			}
			return thing.def.label;
		}

		// Token: 0x0600C2BE RID: 49854 RVA: 0x0046FBFF File Offset: 0x0046DDFF
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x0600C2BF RID: 49855 RVA: 0x0046FC24 File Offset: 0x0046DE24
		public static void UpdateTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass16_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			CS$<>8__locals1.tabUpdated = false;
			if (CS$<>8__locals1.pane.CurTabs != null)
			{
				IList list = CS$<>8__locals1.pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|16_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|16_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			if (!CS$<>8__locals1.tabUpdated)
			{
				CS$<>8__locals1.pane.CloseOpenTab();
			}
		}

		// Token: 0x0600C2C0 RID: 49856 RVA: 0x0046FCE0 File Offset: 0x0046DEE0
		public static void InspectPaneOnGUI(Rect inRect, IInspectPane pane)
		{
			pane.RecentHeight = 165f;
			if (pane.AnythingSelected)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					rect.yMax += 6f;
					Widgets.BeginGroup(rect);
					float num = 0f;
					if (pane.ShouldShowSelectNextInCellButton)
					{
						Rect rect2 = new Rect(rect.width - 24f, 0f, 24f, 24f);
						MouseoverSounds.DoRegion(rect2);
						if (Widgets.ButtonImage(rect2, TexButton.SelectOverlappingNext, true, null))
						{
							pane.SelectNextInCell();
						}
						num += 24f;
						if (SteamDeck.IsSteamDeckInNonKeyboardMode)
						{
							TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTipController");
						}
						else
						{
							TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTip", KeyBindingDefOf.SelectNextInCell.MainKeyLabel);
						}
					}
					pane.DoInspectPaneButtons(rect, ref num);
					Rect rect3 = new Rect(0f, 0f, rect.width - num, 50f);
					string label = pane.GetLabel(rect3);
					rect3.width += 300f;
					using (new TextBlock(GameFont.Medium, TextAnchor.UpperLeft))
					{
						Widgets.Label(rect3, label);
					}
					if (pane.ShouldShowPaneContents)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						pane.DoPaneContents(rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString());
				}
				finally
				{
					Widgets.EndGroup();
				}
			}
		}

		// Token: 0x0600C2C1 RID: 49857 RVA: 0x0046FEC4 File Offset: 0x0046E0C4
		private static void DoTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			try
			{
				InspectPaneUtility.<>c__DisplayClass18_1 CS$<>8__locals2;
				CS$<>8__locals2.tabsTopY = CS$<>8__locals1.pane.PaneTopY - 30f;
				CS$<>8__locals2.curTabX = InspectPaneUtility.PaneWidthFor(CS$<>8__locals1.pane) - 72f;
				CS$<>8__locals2.leftEdge = 0f;
				CS$<>8__locals2.drewOpen = false;
				if (CS$<>8__locals1.pane.CurTabs != null)
				{
					IList list = CS$<>8__locals1.pane.CurTabs as IList;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							InspectPaneUtility.<DoTabs>g__Do|18_0((InspectTabBase)list[i], ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
					else
					{
						foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
						{
							InspectPaneUtility.<DoTabs>g__Do|18_0(tab, ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
				}
				if (CS$<>8__locals2.drewOpen)
				{
					GUI.DrawTexture(new Rect(0f, CS$<>8__locals2.tabsTopY, CS$<>8__locals2.leftEdge, 30f), InspectPaneUtility.InspectTabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783);
			}
		}

		// Token: 0x0600C2C2 RID: 49858 RVA: 0x00470000 File Offset: 0x0046E200
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x0600C2C3 RID: 49859 RVA: 0x00470014 File Offset: 0x0046E214
		private static void ToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (InspectPaneUtility.IsOpen(tab, pane) || (tab == null && pane.OpenTabType == null))
			{
				pane.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return;
			}
			tab.OnOpen();
			pane.OpenTabType = tab.GetType();
			SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600C2C4 RID: 49860 RVA: 0x0047006C File Offset: 0x0046E26C
		public static InspectTabBase OpenTab(Type inspectTabType)
		{
			InspectPaneUtility.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.inspectTabType = inspectTabType;
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			CS$<>8__locals1.tab = null;
			if (mainTabWindow_Inspect.CurTabs != null)
			{
				IList list = mainTabWindow_Inspect.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (InspectPaneUtility.<OpenTab>g__Find|21_0((InspectTabBase)list[i], ref CS$<>8__locals1))
						{
							break;
						}
					}
				}
				else
				{
					using (IEnumerator<InspectTabBase> enumerator = mainTabWindow_Inspect.CurTabs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InspectPaneUtility.<OpenTab>g__Find|21_0(enumerator.Current, ref CS$<>8__locals1))
							{
								break;
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.tab != null)
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, true);
				}
				if (!InspectPaneUtility.IsOpen(CS$<>8__locals1.tab, mainTabWindow_Inspect))
				{
					InspectPaneUtility.ToggleTab(CS$<>8__locals1.tab, mainTabWindow_Inspect);
				}
			}
			return CS$<>8__locals1.tab;
		}

		// Token: 0x0600C2C5 RID: 49861 RVA: 0x0047016C File Offset: 0x0046E36C
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (TutorSystem.TutorialMode && !InspectPaneUtility.IsOpen(tab, pane) && !TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				return;
			}
			InspectPaneUtility.ToggleTab(tab, pane);
		}

		// Token: 0x0600C2C7 RID: 49863 RVA: 0x004701E0 File Offset: 0x0046E3E0
		[CompilerGenerated]
		internal static void <PaneWidthFor>g__Process|9_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass9_0 A_1)
		{
			if (tab.IsVisible && !tab.Hidden)
			{
				int visible = A_1.visible;
				A_1.visible = visible + 1;
			}
		}

		// Token: 0x0600C2C8 RID: 49864 RVA: 0x0047020D File Offset: 0x0046E40D
		[CompilerGenerated]
		internal static void <UpdateTabs>g__Update|16_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass16_0 A_1)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			if (tab.GetType() == A_1.pane.OpenTabType)
			{
				tab.TabUpdate();
				A_1.tabUpdated = true;
			}
		}

		// Token: 0x0600C2C9 RID: 49865 RVA: 0x00470240 File Offset: 0x0046E440
		[CompilerGenerated]
		internal static void <DoTabs>g__Do|18_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass18_0 A_1, ref InspectPaneUtility.<>c__DisplayClass18_1 A_2)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			bool flag = tab.GetType() == A_1.pane.OpenTabType;
			if (!tab.Hidden)
			{
				Rect rect = new Rect(A_2.curTabX, A_2.tabsTopY, 72f, 30f);
				A_2.leftEdge = A_2.curTabX;
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(rect, tab.labelKey.Translate(), true, true, true, null))
				{
					InspectPaneUtility.InterfaceToggleTab(tab, A_1.pane);
				}
				if (!flag && !tab.TutorHighlightTagClosed.NullOrEmpty())
				{
					UIHighlighter.HighlightOpportunity(rect, tab.TutorHighlightTagClosed);
				}
				A_2.curTabX -= 72f;
			}
			if (flag)
			{
				tab.DoTabGUI();
				A_1.pane.RecentHeight = 700f;
				A_2.drewOpen = true;
			}
		}

		// Token: 0x0600C2CA RID: 49866 RVA: 0x00470325 File Offset: 0x0046E525
		[CompilerGenerated]
		internal static bool <OpenTab>g__Find|21_0(InspectTabBase t, ref InspectPaneUtility.<>c__DisplayClass21_0 A_1)
		{
			if (A_1.inspectTabType.IsAssignableFrom(t.GetType()))
			{
				A_1.tab = t;
				return true;
			}
			return false;
		}

		// Token: 0x04006F53 RID: 28499
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x04006F54 RID: 28500
		public const float TabWidth = 72f;

		// Token: 0x04006F55 RID: 28501
		public const float TabHeight = 30f;

		// Token: 0x04006F56 RID: 28502
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x04006F57 RID: 28503
		public const float CornerButtonsSize = 24f;

		// Token: 0x04006F58 RID: 28504
		public const float PaneInnerMargin = 12f;

		// Token: 0x04006F59 RID: 28505
		public const float PaneHeight = 165f;

		// Token: 0x04006F5A RID: 28506
		private const int TabMinimum = 6;

		// Token: 0x04006F5B RID: 28507
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
