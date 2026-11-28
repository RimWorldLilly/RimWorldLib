using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001CFB RID: 7419
	[StaticConstructorOnStartup]
	public static class MainMenuDrawer
	{
		// Token: 0x170022D5 RID: 8917
		// (get) Token: 0x0600BF6B RID: 49003 RVA: 0x00456B16 File Offset: 0x00454D16
		private static UI_BackgroundMain BackgroundMain
		{
			get
			{
				return (UI_BackgroundMain)UIMenuBackgroundManager.background;
			}
		}

		// Token: 0x0600BF6C RID: 49004 RVA: 0x00456B24 File Offset: 0x00454D24
		public static void Init()
		{
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			if (ModsConfig.IdeologyActive)
			{
				ArchonexusCountdown.CancelCountdown();
			}
			MainMenuDrawer.anyMapFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
			if (Prefs.RandomBackgroundImage)
			{
				MainMenuDrawer.BackgroundMain.overrideBGImage = (from exp in ModLister.AllExpansions
				where exp.Status == ExpansionStatus.Active
				select exp).RandomElement<ExpansionDef>().BackgroundImage;
			}
			else
			{
				MainMenuDrawer.BackgroundMain.overrideBGImage = Prefs.BackgroundImageExpansion.BackgroundImage;
			}
			MainMenuDrawer.BackgroundMain.SetupExpansionFadeData();
			MainMenuDrawer.anomalyReleasedTranslated = "MainScreen_AnomalyAvailableNow".Translate();
		}

		// Token: 0x0600BF6D RID: 49005 RVA: 0x00456BD0 File Offset: 0x00454DD0
		public static void MainMenuOnGUI()
		{
			VersionControl.DrawInfoInCorner();
			Rect rect = new Rect((float)UI.screenWidth / 2f - MainMenuDrawer.PaneSize.x / 2f, (float)UI.screenHeight / 2f - MainMenuDrawer.PaneSize.y / 2f + 50f, MainMenuDrawer.PaneSize.x, MainMenuDrawer.PaneSize.y);
			rect.x = (float)UI.screenWidth - rect.width - 30f;
			Rect rect2 = new Rect(0f, rect.y - 30f, (float)UI.screenWidth - 85f, 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperRight;
			string text = "MainPageCredit".Translate();
			if (UI.screenWidth < 990)
			{
				Rect position = rect2;
				position.xMin = position.xMax - Text.CalcSize(text).x;
				position.xMin -= 4f;
				position.xMax += 4f;
				GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
				GUI.DrawTexture(position, BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			Widgets.Label(rect2, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			Vector2 vector = MainMenuDrawer.TitleSize;
			if (vector.x > (float)UI.screenWidth)
			{
				vector *= (float)UI.screenWidth / vector.x;
			}
			vector *= 0.5f;
			GUI.DrawTexture(new Rect((float)UI.screenWidth - vector.x - 50f, rect2.y - vector.y, vector.x, vector.y), MainMenuDrawer.TexTitle, ScaleMode.StretchToFill, true);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUI.DrawTexture(new Rect((float)(UI.screenWidth - 8) - MainMenuDrawer.LudeonLogoSize.x, 8f, MainMenuDrawer.LudeonLogoSize.x, MainMenuDrawer.LudeonLogoSize.y), MainMenuDrawer.TexLudeonLogo, ScaleMode.StretchToFill, true);
			GUI.color = Color.white;
			rect.yMin += 17f;
			MainMenuDrawer.DoMainMenuControls(rect, MainMenuDrawer.anyMapFiles);
			MainMenuDrawer.DoTranslationInfoRect(new Rect(8f, 100f, 300f, 400f));
			MainMenuDrawer.DoExpansionIcons();
			SteamDeck.ShowSteamDeckMainMenuControlsIfNotKnown();
		}

		// Token: 0x0600BF6E RID: 49006 RVA: 0x00456E5C File Offset: 0x0045505C
		public static void DoMainMenuControls(Rect rect, bool anyMapFiles)
		{
			if (MainMenuDrawer.webBackgroundYMax > 0f && Current.ProgramState == ProgramState.Entry)
			{
				GUI.color = MainMenuDrawer.WebBGColor;
				Widgets.DrawAtlas(new Rect(rect.x + MainMenuDrawer.webRect.x, rect.y + MainMenuDrawer.webRect.y, MainMenuDrawer.webRect.width, rect.height)
				{
					yMax = rect.y + MainMenuDrawer.webBackgroundYMax - 4f
				}.ExpandedBy(4f), MainMenuDrawer.WebBGAtlas);
				GUI.color = Color.white;
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 170f, rect.height);
			MainMenuDrawer.webRect = new Rect(rect2.xMax + 17f, 0f, 145f, rect.height);
			Text.Font = GameFont.Small;
			List<ListableOption> list = new List<ListableOption>();
			if (Current.ProgramState == ProgramState.Entry)
			{
				string label;
				if (!"Tutorial".CanTranslate())
				{
					label = "LearnToPlay".Translate();
				}
				else
				{
					label = "Tutorial".Translate();
				}
				list.Add(new ListableOption(label, new Action(MainMenuDrawer.InitLearnToPlay), null));
				list.Add(new ListableOption("NewColony".Translate(), delegate()
				{
					Find.WindowStack.Add(new Page_SelectScenario());
				}, null));
				if (Prefs.DevMode)
				{
					list.Add(new ListableOption("DevQuickTest".Translate(), delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							Root_Play.SetupForQuickTestPlay();
							PageUtility.InitGameStart();
						}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
					}, null));
				}
			}
			if (Current.ProgramState == ProgramState.Playing && !GameDataSaveLoader.SavingIsTemporarilyDisabled && !Current.Game.Info.permadeathMode)
			{
				list.Add(new ListableOption("Save".Translate(), delegate()
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Save());
				}, null));
			}
			ListableOption item;
			if (anyMapFiles && (Current.ProgramState != ProgramState.Playing || !Current.Game.Info.permadeathMode))
			{
				item = new ListableOption("LoadGame".Translate(), delegate()
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Load());
				}, null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				list.Add(new ListableOption("ReviewScenario".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Find.Scenario.GetFullInformationText(), null, null, null, null, Find.Scenario.name, false, null, null, WindowLayer.Dialog)
					{
						layer = WindowLayer.Super
					});
				}, null));
			}
			item = new ListableOption("Options".Translate(), delegate()
			{
				MainMenuDrawer.CloseMainTab();
				Find.WindowStack.Add(new Dialog_Options());
			}, "MenuButton-Options");
			list.Add(item);
			if (Current.ProgramState == ProgramState.Entry)
			{
				item = new ListableOption("Mods".Translate(), delegate()
				{
					Find.WindowStack.Add(new Page_ModsConfig());
				}, null);
				list.Add(item);
				if (Prefs.DevMode && LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage && LanguageDatabase.activeLanguage.anyError)
				{
					item = new ListableOption("SaveTranslationReport".Translate(), new Action(LanguageReportGenerator.SaveTranslationReport), null);
					list.Add(item);
				}
				item = new ListableOption("Credits".Translate(), delegate()
				{
					Find.WindowStack.Add(new Screen_Credits());
				}, null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Current.Game.Info.permadeathMode && !GameDataSaveLoader.SavingIsTemporarilyDisabled)
				{
					item = new ListableOption("SaveAndQuitToMainMenu".Translate(), delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							MemoryUtility.ClearAllMapsAndWorld();
						}, "Entry", "SavingLongEvent", false, null, false, false);
					}, null);
					list.Add(item);
					item = new ListableOption("SaveAndQuitToOS".Translate(), delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							LongEventHandler.ExecuteWhenFinished(new Action(Root.Shutdown));
						}, "SavingLongEvent", false, null, false, false, null);
					}, null);
					list.Add(item);
				}
				else
				{
					Action action = delegate()
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), new Action(GenScene.GoToMainMenu), true, null, WindowLayer.Super));
							return;
						}
						GenScene.GoToMainMenu();
					};
					item = new ListableOption("QuitToMainMenu".Translate(), action, null);
					list.Add(item);
					Action action2 = delegate()
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), new Action(Root.Shutdown), true, null, WindowLayer.Super));
							return;
						}
						Root.Shutdown();
					};
					item = new ListableOption("QuitToOS".Translate(), action2, null);
					list.Add(item);
				}
			}
			else
			{
				item = new ListableOption("QuitToOS".Translate(), new Action(Root.Shutdown), null);
				list.Add(item);
			}
			OptionListingUtility.DrawOptionListing(rect2, list);
			Text.Font = GameFont.Small;
			List<ListableOption> list2 = new List<ListableOption>();
			ListableOption item2 = new ListableOption_WebLink("FictionPrimer".Translate(), "https://rimworldgame.com/backstory", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("LudeonBlog".Translate(), "https://ludeon.com/blog", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("Subreddit".Translate(), "https://www.reddit.com/r/RimWorld/", TexButton.IconReddit);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("OfficialWiki".Translate(), "https://rimworldwiki.com", TexButton.IconWiki);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansX".Translate(), "https://x.com/TynanSylvester", TexButton.IconX);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansDesignBook".Translate(), "https://tynansylvester.com/book", TexButton.IconBook);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("HelpTranslate".Translate(), MainMenuDrawer.TranslationsContributeURL, TexButton.IconForums);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("BuySoundtrack".Translate(), delegate()
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				list4.Add(new FloatMenuOption("BuySoundtrack_Classic".Translate(), delegate()
				{
					Application.OpenURL("https://store.steampowered.com/app/990430/RimWorld_Soundtrack/");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list4.Add(new FloatMenuOption("BuySoundtrack_Royalty".Translate(), delegate()
				{
					Application.OpenURL("https://store.steampowered.com/app/1244270/RimWorld__Royalty_Soundtrack/");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list4.Add(new FloatMenuOption("BuySoundtrack_Anomaly".Translate(), delegate()
				{
					Application.OpenURL("https://store.steampowered.com/app/2914900/RimWorld__Anomaly_Soundtrack/");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list4.Add(new FloatMenuOption("BuySoundtrack_Odyssey".Translate(), delegate()
				{
					Application.OpenURL("https://store.steampowered.com/app/3689230/RimWorld__Odyssey_Soundtrack/");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				List<FloatMenuOption> options = list4;
				Find.WindowStack.Add(new FloatMenu(options));
			}, TexButton.IconSoundtrack);
			list2.Add(item2);
			MainMenuDrawer.webBackgroundYMax = OptionListingUtility.DrawOptionListing(MainMenuDrawer.webRect, list2);
			Widgets.BeginGroup(MainMenuDrawer.webRect);
			if (Current.ProgramState == ProgramState.Entry && Widgets.ButtonText(new Rect(0f, MainMenuDrawer.webBackgroundYMax + 10f, MainMenuDrawer.webRect.width, 50f), LanguageDatabase.activeLanguage.FriendlyNameNative, true, true, true, null))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
				{
					LoadedLanguage localLang = localLang2;
					list3.Add(new FloatMenuOption(localLang.DisplayName, delegate()
					{
						LanguageDatabase.SelectLanguage(localLang);
						Prefs.Save();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			Widgets.EndGroup();
			Widgets.EndGroup();
		}

		// Token: 0x0600BF6F RID: 49007 RVA: 0x004575F4 File Offset: 0x004557F4
		public static void DoExpansionIcons()
		{
			List<ExpansionDef> allExpansions = ModLister.AllExpansions;
			int num = -1;
			int num2 = allExpansions.Count((ExpansionDef e) => !e.isCore);
			int num3 = 32 + 64 * num2 + (num2 - 1) * 8 * 2;
			Rect rect = new Rect(8f, (float)(UI.screenHeight - 96 - 8), (float)num3, 96f);
			Widgets.DrawWindowBackground(rect);
			Widgets.BeginGroup(rect.ContractedBy(16f));
			float num4 = 0f;
			for (int i = 0; i < allExpansions.Count; i++)
			{
				if (!allExpansions[i].isCore)
				{
					Rect rect2 = new Rect(num4, 0f, 64f, 64f);
					num4 += 72f;
					GUI.DrawTexture(rect2.ContractedBy(2f), allExpansions[i].IconFromStatus);
					if (Widgets.ButtonInvisible(rect2, true) && !allExpansions[i].StoreURL.NullOrEmpty())
					{
						SteamUtility.OpenUrl(allExpansions[i].StoreURL);
					}
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
						num = i;
					}
					num4 += 8f;
				}
			}
			Widgets.EndGroup();
			if (num >= 0)
			{
				MainMenuDrawer.BackgroundMain.Notify_Hovered(allExpansions[num]);
				MainMenuDrawer.DoExpansionInfo(num, new Vector2(Mathf.Min((float)(num3 + 16), (float)UI.screenWidth - 350f), rect.yMax));
			}
		}

		// Token: 0x0600BF70 RID: 49008 RVA: 0x00457778 File Offset: 0x00455978
		private static void DoExpansionInfo(int index, Vector2 offset)
		{
			ExpansionDef expansionDef = ModLister.AllExpansions[index];
			List<Texture2D> previewImages = expansionDef.PreviewImages;
			float num = 16f;
			float num2 = 200f;
			float num3 = num2 * 2f + num * 2f;
			float num4 = previewImages.NullOrEmpty<Texture2D>() ? 0f : (num2 * 3f + num * 2.5f);
			Text.Font = GameFont.Medium;
			float num5 = Text.CalcHeight(expansionDef.label, 350f - num * 2f);
			Text.Font = GameFont.Small;
			string text = "ClickForMoreInfo".Translate();
			float num6 = Text.CalcHeight(expansionDef.description, 350f - num * 2f);
			float num7 = Text.CalcHeight(text, 350f - num * 2f);
			num4 = Mathf.Max(num5 + num7 + num + num6 + num * 2f, num4);
			Rect rect = new Rect(offset.x, offset.y - num4, 350f, num4);
			Widgets.DrawWindowBackground(new Rect(rect.x, rect.y, previewImages.NullOrEmpty<Texture2D>() ? rect.width : (rect.width + num3), rect.height));
			Rect rect2 = rect.ContractedBy(num);
			Widgets.BeginGroup(rect2);
			float num8 = 0f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, num8, rect2.width, num5), new GUIContent(" " + expansionDef.label, expansionDef.Icon));
			Text.Font = GameFont.Small;
			num8 += num5;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(0f, num8, rect2.width, num7), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			num8 += num7 + num;
			Widgets.Label(new Rect(0f, num8, rect2.width, rect2.height - num8), expansionDef.description);
			Widgets.EndGroup();
			if (!previewImages.NullOrEmpty<Texture2D>())
			{
				Rect rect3 = new Rect(rect.x + rect.width, rect.y, num3, rect.height).ContractedBy(num);
				Widgets.BeginGroup(rect3);
				float num9 = 0f;
				float num10 = 0f;
				for (int i = 0; i < previewImages.Count; i++)
				{
					float num11 = num2 - num / 2f;
					GUI.DrawTexture(new Rect(num9, num10, num11, num11), previewImages[i]);
					num9 += num2 + num / 2f;
					if (num9 >= rect3.width)
					{
						num9 = 0f;
						num10 += num2 + num / 2f;
					}
				}
				Widgets.EndGroup();
			}
		}

		// Token: 0x0600BF71 RID: 49009 RVA: 0x00457A3C File Offset: 0x00455C3C
		public static void DoTranslationInfoRect(Rect outRect)
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage && !DebugSettings.enableTranslationWindowInEnglish)
			{
				return;
			}
			Widgets.DrawWindowBackground(outRect);
			Rect rect = outRect.ContractedBy(8f);
			Widgets.BeginGroup(rect);
			rect = rect.AtZero();
			Rect rect2 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			Rect rect3 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			Rect rect4 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			string text = "";
			foreach (CreditsEntry creditsEntry in LanguageDatabase.activeLanguage.info.credits)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role != null)
				{
					text = text + creditRecord_Role.creditee + "\n";
				}
			}
			text = text.TrimEndNewlines();
			string label = "TranslationThanks".Translate(text) + "\n\n" + "TranslationHowToContribute".Translate();
			Widgets.LabelScrollable(rect, label, ref MainMenuDrawer.translationInfoScrollbarPos, false, false, false);
			if (Widgets.ButtonText(rect4, "LearnMore".Translate(), true, true, true, null))
			{
				Application.OpenURL(MainMenuDrawer.TranslationsContributeURL);
			}
			if (Widgets.ButtonText(rect3, "SaveTranslationReport".Translate(), true, true, true, null))
			{
				LanguageReportGenerator.SaveTranslationReport();
			}
			if (Widgets.ButtonText(rect2, "CleanupTranslationFiles".Translate(), true, true, true, null))
			{
				TranslationFilesCleaner.CleanupTranslationFiles();
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BF72 RID: 49010 RVA: 0x00457C74 File Offset: 0x00455E74
		private static void DoDevBuildWarningRect(Rect outRect)
		{
			Widgets.DrawWindowBackground(outRect);
			Widgets.Label(outRect.ContractedBy(17f), "DevBuildWarning".Translate());
		}

		// Token: 0x0600BF73 RID: 49011 RVA: 0x00457C98 File Offset: 0x00455E98
		private static void InitLearnToPlay()
		{
			Game.ClearCaches();
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Tutorial.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Tutor, DifficultyDefOf.Easy);
			Find.GameInitData.startedFromEntry = true;
			Page next = Current.Game.Scenario.GetFirstConfigPage().next;
			next.prev = null;
			Find.WindowStack.Add(next);
		}

		// Token: 0x0600BF74 RID: 49012 RVA: 0x00457D2C File Offset: 0x00455F2C
		private static void CloseMainTab()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
			}
		}

		// Token: 0x04006E02 RID: 28162
		private static bool anyMapFiles;

		// Token: 0x04006E03 RID: 28163
		private static Vector2 translationInfoScrollbarPos;

		// Token: 0x04006E04 RID: 28164
		private static float webBackgroundYMax;

		// Token: 0x04006E05 RID: 28165
		private static Rect webRect;

		// Token: 0x04006E06 RID: 28166
		private static string anomalyReleasedTranslated = null;

		// Token: 0x04006E07 RID: 28167
		private const float PlayRectWidth = 170f;

		// Token: 0x04006E08 RID: 28168
		private const float WebRectWidth = 145f;

		// Token: 0x04006E09 RID: 28169
		private const float RightEdgeMargin = 50f;

		// Token: 0x04006E0A RID: 28170
		private const float WebBGExpansion = 4f;

		// Token: 0x04006E0B RID: 28171
		private const float ExpansionDetailsWidth = 350f;

		// Token: 0x04006E0C RID: 28172
		private static readonly Vector2 PaneSize = new Vector2(450f, 550f);

		// Token: 0x04006E0D RID: 28173
		private static readonly Vector2 TitleSize = new Vector2(1032f, 146f);

		// Token: 0x04006E0E RID: 28174
		private static readonly Texture2D TexTitle = ContentFinder<Texture2D>.Get("UI/HeroArt/GameTitle", true);

		// Token: 0x04006E0F RID: 28175
		private const float TitleShift = 50f;

		// Token: 0x04006E10 RID: 28176
		private static readonly Vector2 LudeonLogoSize = new Vector2(200f, 58f);

		// Token: 0x04006E11 RID: 28177
		private static readonly Texture2D TexLudeonLogo = ContentFinder<Texture2D>.Get("UI/HeroArt/LudeonLogoSmall", true);

		// Token: 0x04006E12 RID: 28178
		private static readonly string TranslationsContributeURL = "https://rimworldgame.com/helptranslate";

		// Token: 0x04006E13 RID: 28179
		private static readonly Texture2D WebBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/SubtleGradient", true);

		// Token: 0x04006E14 RID: 28180
		private static readonly Color WebBGColor = new Color(0f, 0f, 0f, 0.3f);

		// Token: 0x04006E15 RID: 28181
		private static readonly Color ExpansionReleaseTextColor = new Color(0.9607843f, 0.96862745f, 0.5058824f);

		// Token: 0x04006E16 RID: 28182
		private const int NumColumns = 2;

		// Token: 0x04006E17 RID: 28183
		private const int NumRows = 3;
	}
}
