using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CCA RID: 7370
	[StaticConstructorOnStartup]
	public class Dialog_StylingStation : Window
	{
		// Token: 0x1700227D RID: 8829
		// (get) Token: 0x0600BD64 RID: 48484 RVA: 0x0043F5C5 File Offset: 0x0043D7C5
		private bool DevMode
		{
			get
			{
				return this.stylingStation == null;
			}
		}

		// Token: 0x1700227E RID: 8830
		// (get) Token: 0x0600BD65 RID: 48485 RVA: 0x0043F5D0 File Offset: 0x0043D7D0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 750f);
			}
		}

		// Token: 0x1700227F RID: 8831
		// (get) Token: 0x0600BD66 RID: 48486 RVA: 0x0043F5E4 File Offset: 0x0043D7E4
		private List<Color> AllColors
		{
			get
			{
				if (this.allColors == null)
				{
					this.allColors = new List<Color>();
					if (this.pawn.Ideo != null && !Find.IdeoManager.classicMode)
					{
						this.allColors.Add(this.pawn.Ideo.ApparelColor);
					}
					using (IEnumerator<ColorDef> enumerator = (from x in DefDatabase<ColorDef>.AllDefs
					where x.colorType == ColorType.Ideo || x.colorType == ColorType.Misc || (this.DevMode && !ModsConfig.IdeologyActive && x.colorType == ColorType.Structure)
					select x).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ColorDef colDef = enumerator.Current;
							if (!this.allColors.Any((Color x) => x.IndistinguishableFrom(colDef.color)))
							{
								this.allColors.Add(colDef.color);
							}
						}
					}
					this.allColors.SortByColor((Color x) => x);
				}
				return this.allColors;
			}
		}

		// Token: 0x17002280 RID: 8832
		// (get) Token: 0x0600BD67 RID: 48487 RVA: 0x0043F6F0 File Offset: 0x0043D8F0
		private List<Color> AllHairColors
		{
			get
			{
				if (this.allHairColors == null)
				{
					this.allHairColors = new List<Color>();
					foreach (ColorDef colorDef in DefDatabase<ColorDef>.AllDefs)
					{
						Color color = colorDef.color;
						if (colorDef.displayInStylingStationUI && !this.allHairColors.Any((Color x) => x.WithinDiffThresholdFrom(color, 0.15f)))
						{
							this.allHairColors.Add(color);
						}
					}
					this.allHairColors.SortByColor((Color x) => x);
				}
				return this.allHairColors;
			}
		}

		// Token: 0x17002281 RID: 8833
		// (get) Token: 0x0600BD68 RID: 48488 RVA: 0x0043F7C0 File Offset: 0x0043D9C0
		private List<Color> AllSkinColors
		{
			get
			{
				if (this.allSkinColors == null)
				{
					this.allSkinColors = new List<Color>();
					foreach (GeneDef geneDef in PawnSkinColors.SkinColorGenesInOrder)
					{
						Color value = geneDef.skinColorBase.Value;
						this.allSkinColors.Add(value);
					}
					this.allSkinColors.SortByColor((Color x) => x);
				}
				return this.allSkinColors;
			}
		}

		// Token: 0x0600BD69 RID: 48489 RVA: 0x0043F864 File Offset: 0x0043DA64
		public Dialog_StylingStation(Pawn pawn, Thing stylingStation) : base(null)
		{
			this.pawn = pawn;
			this.stylingStation = stylingStation;
			this.initialHairDef = pawn.story.hairDef;
			this.desiredHairColor = pawn.story.HairColor;
			this.initialBeardDef = pawn.style.beardDef;
			this.initialFaceTattoo = pawn.style.FaceTattoo;
			this.initialBodyTattoo = pawn.style.BodyTattoo;
			this.initialSkinColor = pawn.story.skinColorOverride;
			this.desiredSkinColor = pawn.story.SkinColor;
			this.initialBodyType = pawn.story.bodyType;
			this.initialHeadType = pawn.story.headType;
			this.forcePause = true;
			this.showClothes = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (apparel.TryGetComp<CompColorable>() != null)
				{
					this.apparelColors.Add(apparel, apparel.DesiredColor ?? apparel.GetColorIgnoringTainted());
				}
			}
		}

		// Token: 0x0600BD6A RID: 48490 RVA: 0x0043F9C8 File Offset: 0x0043DBC8
		public override void PostOpen()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				this.Close(true);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600BD6B RID: 48491 RVA: 0x0043F9E4 File Offset: 0x0043DBE4
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(inRect)
			{
				height = Text.LineHeight * 2f
			};
			Widgets.Label(rect, "StylePawn".Translate().CapitalizeFirst() + ": " + Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.Name.ToStringShort, this.pawn.gender, false, true).ApplyTag(TagType.Name, null));
			Text.Font = GameFont.Small;
			inRect.yMin = rect.yMax + 4f;
			Rect rect2 = inRect;
			rect2.width *= 0.3f;
			rect2.yMax -= Dialog_StylingStation.ButSize.y + 4f;
			this.DrawPawn(rect2);
			Rect rect3 = inRect;
			rect3.xMin = rect2.xMax + 10f;
			rect3.yMax -= Dialog_StylingStation.ButSize.y + 4f;
			this.DrawTabs(rect3);
			this.DrawBottomButtons(inRect);
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(inRect.xMax - 120f, 0f, 120f, 30f), "DEV: Show all", ref this.devEditMode, false, null, null, false, false);
			}
		}

		// Token: 0x0600BD6C RID: 48492 RVA: 0x0043FB3C File Offset: 0x0043DD3C
		private void DrawPawn(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin = rect.yMax - Text.LineHeight * 2f;
			Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f), "ShowHeadgear".Translate(), ref this.showHeadgear, false, null, null, false, false);
			Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y + rect2.height / 2f, rect2.width, rect2.height / 2f), "ShowApparel".Translate(), ref this.showClothes, false, null, null, false, false);
			rect.yMax = rect2.yMin - 4f;
			Widgets.BeginGroup(rect);
			for (int i = 0; i < 3; i++)
			{
				Rect position = new Rect(0f, rect.height / 3f * (float)i, rect.width, rect.height / 3f).ContractedBy(4f);
				RenderTexture image = PortraitsCache.Get(this.pawn, new Vector2(position.width, position.height), new Rot4(2 - i), Dialog_StylingStation.PortraitOffset, 1.1f, true, true, this.showHeadgear, this.showClothes, this.apparelColors, new Color?(this.desiredHairColor), true, null);
				GUI.DrawTexture(position, image);
			}
			Widgets.EndGroup();
			if (this.pawn.style.HasAnyUnwantedStyleItem)
			{
				string t = "PawnUnhappyWithStyleItems".Translate(this.pawn.Named("PAWN")) + ": ";
				Dialog_StylingStation.tmpUnwantedStyleNames.Clear();
				if (this.pawn.style.HasUnwantedHairStyle)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("Hair".Translate());
				}
				if (this.pawn.style.HasUnwantedBeard)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("Beard".Translate());
				}
				if (this.pawn.style.HasUnwantedFaceTattoo)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooFace".Translate());
				}
				if (this.pawn.style.HasUnwantedBodyTattoo)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooBody".Translate());
				}
				GUI.color = ColorLibrary.RedReadable;
				Widgets.Label(new Rect(rect.x, rect.yMin - 30f, rect.width, Text.LineHeight * 2f + 10f), "Warning".Translate() + ": " + t + Dialog_StylingStation.tmpUnwantedStyleNames.ToCommaList(false, false).CapitalizeFirst());
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600BD6D RID: 48493 RVA: 0x0043FE3C File Offset: 0x0043E03C
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("Hair".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.Hair;
			}, this.curTab == Dialog_StylingStation.StylingTab.Hair));
			if (this.pawn.style.CanWantBeard || this.devEditMode)
			{
				this.tabs.Add(new TabRecord("Beard".Translate().CapitalizeFirst(), delegate()
				{
					this.curTab = Dialog_StylingStation.StylingTab.Beard;
				}, this.curTab == Dialog_StylingStation.StylingTab.Beard));
			}
			this.tabs.Add(new TabRecord("TattooFace".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.TattooFace;
			}, this.curTab == Dialog_StylingStation.StylingTab.TattooFace));
			this.tabs.Add(new TabRecord("TattooBody".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.TattooBody;
			}, this.curTab == Dialog_StylingStation.StylingTab.TattooBody));
			this.tabs.Add(new TabRecord("ApparelColor".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.ApparelColor;
			}, this.curTab == Dialog_StylingStation.StylingTab.ApparelColor));
			if (this.devEditMode)
			{
				this.tabs.Add(new TabRecord("Body type", delegate()
				{
					this.curTab = Dialog_StylingStation.StylingTab.BodyType;
				}, this.curTab == Dialog_StylingStation.StylingTab.BodyType));
				this.tabs.Add(new TabRecord("Head type", delegate()
				{
					this.curTab = Dialog_StylingStation.StylingTab.HeadType;
				}, this.curTab == Dialog_StylingStation.StylingTab.HeadType));
			}
			Widgets.DrawMenuSection(rect);
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
			rect = rect.ContractedBy(18f);
			switch (this.curTab)
			{
			case Dialog_StylingStation.StylingTab.Hair:
				rect.yMax -= this.colorsHeight;
				this.DrawStylingItemType<HairDef>(rect, ref this.hairScrollPosition, delegate(Rect r, HairDef h)
				{
					GUI.color = this.desiredHairColor;
					Widgets.DefIcon(r, h, null, 1.25f, null, false, null, null, null, 1f);
					GUI.color = Color.white;
				}, delegate(HairDef h)
				{
					this.pawn.story.hairDef = h;
				}, (StyleItemDef h) => this.pawn.story.hairDef == h, (StyleItemDef h) => this.initialHairDef == h, null, true);
				return;
			case Dialog_StylingStation.StylingTab.Beard:
				this.DrawStylingItemType<BeardDef>(rect, ref this.beardScrollPosition, delegate(Rect r, BeardDef b)
				{
					GUI.color = this.desiredHairColor;
					Widgets.DefIcon(r, b, null, 1.25f, null, false, null, null, null, 1f);
					GUI.color = Color.white;
				}, delegate(BeardDef b)
				{
					this.pawn.style.beardDef = b;
				}, (StyleItemDef b) => this.pawn.style.beardDef == b, (StyleItemDef b) => this.initialBeardDef == b, null, false);
				return;
			case Dialog_StylingStation.StylingTab.TattooFace:
				this.DrawStylingItemType<TattooDef>(rect, ref this.faceTattooScrollPosition, delegate(Rect r, TattooDef t)
				{
					Widgets.DefIcon(r, t, null, 1f, null, false, null, null, null, 1f);
				}, delegate(TattooDef t)
				{
					this.pawn.style.FaceTattoo = t;
				}, (StyleItemDef t) => this.pawn.style.FaceTattoo == t, (StyleItemDef t) => this.initialFaceTattoo == t, (StyleItemDef t) => ((TattooDef)t).tattooType == TattooType.Face, false);
				return;
			case Dialog_StylingStation.StylingTab.TattooBody:
				this.DrawStylingItemType<TattooDef>(rect, ref this.bodyTattooScrollPosition, delegate(Rect r, TattooDef t)
				{
					Widgets.DefIcon(r, t, null, 1f, null, false, null, null, null, 1f);
				}, delegate(TattooDef t)
				{
					this.pawn.style.BodyTattoo = t;
				}, (StyleItemDef t) => this.pawn.style.BodyTattoo == t, (StyleItemDef t) => this.initialBodyTattoo == t, (StyleItemDef t) => ((TattooDef)t).tattooType == TattooType.Body, false);
				return;
			case Dialog_StylingStation.StylingTab.ApparelColor:
				this.DrawApparelColor(rect);
				return;
			case Dialog_StylingStation.StylingTab.BodyType:
				rect.yMax -= this.colorsHeight;
				this.DevDrawBodyType(rect, ref this.bodyTypeScrollPosition, delegate(Rect r, BodyTypeDef b)
				{
					GUI.color = this.pawn.story.SkinColor;
					Widgets.DefIcon(r, b, null, 1.25f, null, false, null, null, null, 1f);
					GUI.color = Color.white;
				}, delegate(BodyTypeDef t)
				{
					this.pawn.story.bodyType = t;
				}, (BodyTypeDef t) => this.pawn.story.bodyType == t);
				return;
			case Dialog_StylingStation.StylingTab.HeadType:
				rect.yMax -= this.colorsHeight;
				this.DevDrawHeadType(rect, ref this.headTypeScrollPosition, delegate(Rect r, HeadTypeDef b)
				{
					GUI.color = this.pawn.story.SkinColor;
					Widgets.DefIcon(r, b, null, 1.25f, null, false, null, null, null, 1f);
					GUI.color = Color.white;
				}, delegate(HeadTypeDef t)
				{
					this.pawn.story.headType = t;
				}, (HeadTypeDef t) => this.pawn.story.headType == t);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600BD6E RID: 48494 RVA: 0x00440250 File Offset: 0x0043E450
		private void DrawDyeRequirement(Rect rect, ref float curY, int requiredDye)
		{
			Widgets.ThingIcon(new Rect(rect.x, curY, Text.LineHeight, Text.LineHeight), ThingDefOf.Dye, null, null, 1.1f, null, null, 1f);
			string text = "Required".Translate() + ": " + requiredDye.ToString() + " " + ThingDefOf.Dye.label;
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(rect.x + Text.LineHeight + 4f, curY, x, Text.LineHeight), text);
			Rect rect2 = new Rect(rect.x, curY, x + Text.LineHeight + 8f, Text.LineHeight);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegionByKey(rect2, "TooltipDyeExplanation");
			}
			curY += Text.LineHeight;
		}

		// Token: 0x0600BD6F RID: 48495 RVA: 0x0044034C File Offset: 0x0043E54C
		private void DrawHairColors(Rect rect)
		{
			float num = rect.y;
			Widgets.ColorSelector(new Rect(rect.x, num, rect.width, this.colorsHeight), ref this.desiredHairColor, this.AllHairColors, out this.colorsHeight, null, 22, 2, null);
			num += this.colorsHeight;
			if (this.pawn.Spawned && this.desiredHairColor != this.pawn.story.HairColor && this.desiredHairColor != this.pawn.style.nextHairColor)
			{
				this.DrawDyeRequirement(rect, ref num, 1);
				if (this.pawn.Map.resourceCounter.GetCount(ThingDefOf.Dye) < 1)
				{
					Rect rect2 = new Rect(rect.x, num, rect.width, Text.LineHeight);
					Color color = GUI.color;
					GUI.color = ColorLibrary.RedReadable;
					Widgets.Label(rect2, "NotEnoughDye".Translate() + " " + "NotEnoughDyeWillRecolorHair".Translate());
					GUI.color = color;
				}
			}
			this.colorsHeight += Text.LineHeight * 2f;
		}

		// Token: 0x0600BD70 RID: 48496 RVA: 0x0044049C File Offset: 0x0043E69C
		private void DrawApparelColor(Rect rect)
		{
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			Widgets.BeginScrollView(rect, ref this.apparelColorScrollPosition, viewRect, true);
			int num = 0;
			float num2 = rect.y;
			foreach (Apparel apparel in this.pawn.apparel.WornApparel)
			{
				Color color;
				if (this.apparelColors.TryGetValue(apparel, out color))
				{
					Rect rect2 = new Rect(rect.x, num2, viewRect.width, 92f);
					num2 += rect2.height + 10f;
					if (!this.pawn.apparel.IsLocked(apparel) || this.DevMode)
					{
						float num3;
						Widgets.ColorSelector(rect2, ref color, this.AllColors, out num3, Widgets.GetIconFor(apparel.def, apparel.Stuff, apparel.StyleDef, null), 22, 2, new Action<Color, Rect>(this.ColorSelecterExtraOnGUI));
						float num4 = rect2.x;
						if (this.pawn.Ideo != null && !Find.IdeoManager.classicMode)
						{
							rect2 = new Rect(num4, num2, 200f, 24f);
							if (Widgets.ButtonText(rect2, "SetIdeoColor".Translate(), true, true, true, null))
							{
								color = this.pawn.Ideo.ApparelColor;
								SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
							}
							num4 += 210f;
						}
						Pawn_StoryTracker story = this.pawn.story;
						if (((story != null) ? story.favoriteColor : null) != null)
						{
							rect2 = new Rect(num4, num2, 200f, 24f);
							if (Widgets.ButtonText(rect2, "SetFavoriteColor".Translate(), true, true, true, null))
							{
								color = this.pawn.story.favoriteColor.color;
								SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
							}
						}
						if (!color.IndistinguishableFrom(apparel.GetColorIgnoringTainted()))
						{
							num++;
						}
						this.apparelColors[apparel] = color;
					}
					else
					{
						Widgets.ColorSelectorIcon(new Rect(rect2.x, rect2.y, 88f, 88f), apparel.def.uiIcon, color, false);
						Text.Anchor = TextAnchor.MiddleLeft;
						Rect rect3 = rect2;
						rect3.x += 100f;
						Widgets.Label(rect3, "ApparelLockedCannotRecolor".Translate(this.pawn.Named("PAWN"), apparel.Named("APPAREL")).Colorize(ColorLibrary.RedReadable));
						Text.Anchor = TextAnchor.UpperLeft;
					}
					num2 += 34f;
				}
			}
			if (this.pawn.Spawned)
			{
				if (num > 0)
				{
					this.DrawDyeRequirement(rect, ref num2, num);
				}
				if (this.pawn.Map.resourceCounter.GetCount(ThingDefOf.Dye) < num)
				{
					Rect rect4 = new Rect(rect.x, num2, rect.width - 16f - 10f, 60f);
					Color color2 = GUI.color;
					GUI.color = ColorLibrary.RedReadable;
					Widgets.Label(rect4, "NotEnoughDye".Translate() + " " + "NotEnoughDyeWillRecolorApparel".Translate());
					GUI.color = color2;
					num2 += rect4.height;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				this.viewRectHeight = num2 - rect.y;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600BD71 RID: 48497 RVA: 0x0044086C File Offset: 0x0043EA6C
		private void ColorSelecterExtraOnGUI(Color color, Rect boxRect)
		{
			Texture2D texture2D = null;
			TaggedString str = null;
			bool flag = Mouse.IsOver(boxRect);
			Pawn_StoryTracker story = this.pawn.story;
			if (((story != null) ? story.favoriteColor : null) != null && color.IndistinguishableFrom(this.pawn.story.favoriteColor.color))
			{
				texture2D = Dialog_StylingStation.FavoriteColorTex;
				if (flag)
				{
					str = "FavoriteColorPickerTip".Translate(this.pawn.Named("PAWN"));
				}
			}
			else if (this.pawn.Ideo != null && !Find.IdeoManager.classicMode && color.IndistinguishableFrom(this.pawn.Ideo.ApparelColor))
			{
				texture2D = Dialog_StylingStation.IdeoColorTex;
				if (flag)
				{
					str = "IdeoColorPickerTip".Translate(this.pawn.Named("PAWN"));
				}
			}
			if (texture2D != null)
			{
				Rect position = boxRect.ContractedBy(4f);
				GUI.color = Color.black.ToTransparent(0.2f);
				GUI.DrawTexture(new Rect(position.x + 2f, position.y + 2f, position.width, position.height), texture2D);
				GUI.color = Color.white.ToTransparent(0.8f);
				GUI.DrawTexture(position, texture2D);
				GUI.color = Color.white;
			}
			if (!str.NullOrEmpty())
			{
				TooltipHandler.TipRegion(boxRect, str);
			}
		}

		// Token: 0x0600BD72 RID: 48498 RVA: 0x004409D0 File Offset: 0x0043EBD0
		private void DrawStylingItemType<T>(Rect rect, ref Vector2 scrollPosition, Action<Rect, T> drawAction, Action<T> selectAction, Func<StyleItemDef, bool> hasStyleItem, Func<StyleItemDef, bool> hadStyleItem, Func<StyleItemDef, bool> extraValidator = null, bool doColors = false) where T : StyleItemDef
		{
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			int num = Mathf.FloorToInt(viewRect.width / 60f) - 1;
			float num2 = (viewRect.width - (float)num * 60f - (float)(num - 1) * 10f) / 2f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Dialog_StylingStation.tmpStyleItems.Clear();
			Dialog_StylingStation.tmpStyleItems.AddRange(from x in DefDatabase<T>.AllDefs
			where (this.devEditMode || PawnStyleItemChooser.WantsToUseStyle(this.pawn, x, null) || hadStyleItem(x)) && (extraValidator == null || extraValidator(x))
			select x);
			Dialog_StylingStation.tmpStyleItems.SortBy((StyleItemDef x) => -PawnStyleItemChooser.FrequencyFromGender(x, this.pawn));
			if (Dialog_StylingStation.tmpStyleItems.NullOrEmpty<StyleItemDef>())
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "NoneUsableForPawn".Translate(this.pawn.Named("PAWN")) + ")");
			}
			else
			{
				Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
				foreach (StyleItemDef styleItemDef in Dialog_StylingStation.tmpStyleItems)
				{
					if (num5 >= num - 1)
					{
						num5 = 0;
						num4++;
					}
					else if (num3 > 0)
					{
						num5++;
					}
					Rect rect2 = new Rect(rect.x + num2 + (float)num5 * 60f + (float)num5 * 10f, rect.y + (float)num4 * 60f + (float)num4 * 10f, 60f, 60f);
					Widgets.DrawHighlight(rect2);
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
						TooltipHandler.TipRegion(rect2, styleItemDef.LabelCap);
					}
					if (drawAction != null)
					{
						drawAction(rect2, styleItemDef as T);
					}
					if (hasStyleItem(styleItemDef))
					{
						Widgets.DrawBox(rect2, 2, null);
					}
					if (Widgets.ButtonInvisible(rect2, true))
					{
						if (selectAction != null)
						{
							selectAction(styleItemDef as T);
						}
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.pawn.Drawer.renderer.SetAllGraphicsDirty();
					}
					num3++;
				}
				if (Event.current.type == EventType.layout)
				{
					this.viewRectHeight = (float)(num4 + 1) * 60f + (float)num4 * 10f + 10f;
				}
				Widgets.EndScrollView();
			}
			if (doColors)
			{
				this.DrawHairColors(new Rect(rect.x, rect.yMax + 10f, rect.width, this.colorsHeight));
			}
		}

		// Token: 0x0600BD73 RID: 48499 RVA: 0x00440CAC File Offset: 0x0043EEAC
		private void DevDrawBodyType(Rect rect, ref Vector2 scrollPosition, Action<Rect, BodyTypeDef> drawAction, Action<BodyTypeDef> selectAction, Func<BodyTypeDef, bool> hasStyleItem)
		{
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			int num = Mathf.FloorToInt(viewRect.width / 60f) - 1;
			float num2 = (viewRect.width - (float)num * 60f - (float)(num - 1) * 10f) / 2f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			foreach (BodyTypeDef bodyTypeDef in DefDatabase<BodyTypeDef>.AllDefs)
			{
				if (bodyTypeDef != BodyTypeDefOf.Child && bodyTypeDef != BodyTypeDefOf.Baby)
				{
					if (num5 >= num - 1)
					{
						num5 = 0;
						num4++;
					}
					else if (num3 > 0)
					{
						num5++;
					}
					Rect rect2 = new Rect(rect.x + num2 + (float)num5 * 60f + (float)num5 * 10f, rect.y + (float)num4 * 60f + (float)num4 * 10f, 60f, 60f);
					Widgets.DrawHighlight(rect2);
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
						TooltipHandler.TipRegion(rect2, bodyTypeDef.LabelCap);
					}
					if (drawAction != null)
					{
						drawAction(rect2, bodyTypeDef);
					}
					if (hasStyleItem(bodyTypeDef))
					{
						Widgets.DrawBox(rect2, 2, null);
					}
					if (Widgets.ButtonInvisible(rect2, true))
					{
						if (selectAction != null)
						{
							selectAction(bodyTypeDef);
						}
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.pawn.Drawer.renderer.SetAllGraphicsDirty();
					}
					num3++;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				this.viewRectHeight = (float)(num4 + 1) * 60f + (float)num4 * 10f + 10f;
			}
			Widgets.EndScrollView();
			Rect rect3 = new Rect(rect.x, rect.yMax + 10f, rect.width, this.colorsHeight);
			float y = rect3.y;
			Widgets.ColorSelector(new Rect(rect3.x, y, rect3.width, this.colorsHeight), ref this.desiredSkinColor, this.AllSkinColors, out this.colorsHeight, null, 22, 2, null);
			this.pawn.story.skinColorOverride = new Color?(this.desiredSkinColor);
			this.colorsHeight += Text.LineHeight * 2f;
		}

		// Token: 0x0600BD74 RID: 48500 RVA: 0x00440F4C File Offset: 0x0043F14C
		private void DevDrawHeadType(Rect rect, ref Vector2 scrollPosition, Action<Rect, HeadTypeDef> drawAction, Action<HeadTypeDef> selectAction, Func<HeadTypeDef, bool> hasStyleItem)
		{
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			int num = Mathf.FloorToInt(viewRect.width / 60f) - 1;
			float num2 = (viewRect.width - (float)num * 60f - (float)(num - 1) * 10f) / 2f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			foreach (HeadTypeDef headTypeDef in DefDatabase<HeadTypeDef>.AllDefs)
			{
				if (headTypeDef != HeadTypeDefOf.Skull && headTypeDef != HeadTypeDefOf.Stump)
				{
					if (num5 >= num - 1)
					{
						num5 = 0;
						num4++;
					}
					else if (num3 > 0)
					{
						num5++;
					}
					Rect rect2 = new Rect(rect.x + num2 + (float)num5 * 60f + (float)num5 * 10f, rect.y + (float)num4 * 60f + (float)num4 * 10f, 60f, 60f);
					Widgets.DrawHighlight(rect2);
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
						TooltipHandler.TipRegion(rect2, headTypeDef.LabelCap);
					}
					if (drawAction != null)
					{
						drawAction(rect2, headTypeDef);
					}
					if (hasStyleItem(headTypeDef))
					{
						Widgets.DrawBox(rect2, 2, null);
					}
					if (Widgets.ButtonInvisible(rect2, true))
					{
						if (selectAction != null)
						{
							selectAction(headTypeDef);
						}
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.pawn.Drawer.renderer.SetAllGraphicsDirty();
					}
					num3++;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				this.viewRectHeight = (float)(num4 + 1) * 60f + (float)num4 * 10f + 10f;
			}
			Widgets.EndScrollView();
			Rect rect3 = new Rect(rect.x, rect.yMax + 10f, rect.width, this.colorsHeight);
			float y = rect3.y;
			Widgets.ColorSelector(new Rect(rect3.x, y, rect3.width, this.colorsHeight), ref this.desiredSkinColor, this.AllSkinColors, out this.colorsHeight, null, 22, 2, null);
			this.pawn.story.skinColorOverride = new Color?(this.desiredSkinColor);
			this.colorsHeight += Text.LineHeight * 2f;
		}

		// Token: 0x0600BD75 RID: 48501 RVA: 0x004411EC File Offset: 0x0043F3EC
		private void DrawBottomButtons(Rect inRect)
		{
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Cancel".Translate(), true, true, true, null))
			{
				this.Reset(true);
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.xMin + inRect.width / 2f - Dialog_StylingStation.ButSize.x / 2f, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Reset".Translate(), true, true, true, null))
			{
				this.Reset(true);
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(inRect.xMax - Dialog_StylingStation.ButSize.x, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Accept".Translate(), true, true, true, null))
			{
				if (this.pawn.story.hairDef != this.initialHairDef || this.pawn.style.beardDef != this.initialBeardDef || this.pawn.style.FaceTattoo != this.initialFaceTattoo || this.pawn.style.BodyTattoo != this.initialBodyTattoo || this.pawn.story.HairColor != this.desiredHairColor || this.pawn.story.skinColorOverride != this.initialSkinColor || this.pawn.story.bodyType != this.initialBodyType || this.pawn.story.headType != this.initialHeadType)
				{
					if (!this.DevMode)
					{
						this.pawn.style.SetupNextLookChangeData(this.pawn.story.hairDef, this.pawn.style.beardDef, this.pawn.style.FaceTattoo, this.pawn.style.BodyTattoo, new Color?(this.desiredHairColor));
						this.Reset(false);
						this.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseStylingStation, this.stylingStation), new JobTag?(JobTag.Misc), false);
					}
					else
					{
						this.pawn.story.HairColor = this.desiredHairColor;
						this.pawn.style.Notify_StyleItemChanged();
					}
				}
				this.ApplyApparelColors();
				this.Close(true);
			}
		}

		// Token: 0x0600BD76 RID: 48502 RVA: 0x0044151C File Offset: 0x0043F71C
		private void ApplyApparelColors()
		{
			foreach (KeyValuePair<Apparel, Color> keyValuePair in this.apparelColors)
			{
				if (!this.DevMode)
				{
					if (keyValuePair.Key.GetColorIgnoringTainted() != keyValuePair.Value)
					{
						keyValuePair.Key.DesiredColor = new Color?(keyValuePair.Value);
					}
				}
				else if (keyValuePair.Key.GetColorIgnoringTainted() != keyValuePair.Value)
				{
					keyValuePair.Key.DrawColor = keyValuePair.Value;
					keyValuePair.Key.Notify_ColorChanged();
					keyValuePair.Key.DesiredColor = null;
				}
			}
			this.pawn.mindState.Notify_OutfitChanged();
		}

		// Token: 0x0600BD77 RID: 48503 RVA: 0x00441608 File Offset: 0x0043F808
		private void Reset(bool resetColors = true)
		{
			if (resetColors)
			{
				this.apparelColors.Clear();
				foreach (Apparel apparel in this.pawn.apparel.WornApparel)
				{
					if (apparel.TryGetComp<CompColorable>() != null)
					{
						this.apparelColors.Add(apparel, apparel.DesiredColor ?? apparel.GetColorIgnoringTainted());
					}
				}
				this.desiredHairColor = this.pawn.story.HairColor;
			}
			this.pawn.story.hairDef = this.initialHairDef;
			this.pawn.style.beardDef = this.initialBeardDef;
			this.pawn.style.FaceTattoo = this.initialFaceTattoo;
			this.pawn.style.BodyTattoo = this.initialBodyTattoo;
			this.pawn.story.bodyType = this.initialBodyType;
			this.pawn.story.headType = this.initialHeadType;
			this.pawn.story.skinColorOverride = this.initialSkinColor;
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x04006C5A RID: 27738
		private Pawn pawn;

		// Token: 0x04006C5B RID: 27739
		private Thing stylingStation;

		// Token: 0x04006C5C RID: 27740
		private HairDef initialHairDef;

		// Token: 0x04006C5D RID: 27741
		private BeardDef initialBeardDef;

		// Token: 0x04006C5E RID: 27742
		private TattooDef initialFaceTattoo;

		// Token: 0x04006C5F RID: 27743
		private TattooDef initialBodyTattoo;

		// Token: 0x04006C60 RID: 27744
		private Color? initialSkinColor;

		// Token: 0x04006C61 RID: 27745
		private Color desiredSkinColor;

		// Token: 0x04006C62 RID: 27746
		private BodyTypeDef initialBodyType;

		// Token: 0x04006C63 RID: 27747
		private HeadTypeDef initialHeadType;

		// Token: 0x04006C64 RID: 27748
		private Color desiredHairColor;

		// Token: 0x04006C65 RID: 27749
		private Dialog_StylingStation.StylingTab curTab;

		// Token: 0x04006C66 RID: 27750
		private Vector2 hairScrollPosition;

		// Token: 0x04006C67 RID: 27751
		private Vector2 beardScrollPosition;

		// Token: 0x04006C68 RID: 27752
		private Vector2 faceTattooScrollPosition;

		// Token: 0x04006C69 RID: 27753
		private Vector2 bodyTattooScrollPosition;

		// Token: 0x04006C6A RID: 27754
		private Vector2 apparelColorScrollPosition;

		// Token: 0x04006C6B RID: 27755
		private Vector2 bodyTypeScrollPosition;

		// Token: 0x04006C6C RID: 27756
		private Vector2 headTypeScrollPosition;

		// Token: 0x04006C6D RID: 27757
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04006C6E RID: 27758
		private Dictionary<Apparel, Color> apparelColors = new Dictionary<Apparel, Color>();

		// Token: 0x04006C6F RID: 27759
		private float viewRectHeight;

		// Token: 0x04006C70 RID: 27760
		private bool showHeadgear;

		// Token: 0x04006C71 RID: 27761
		private bool showClothes;

		// Token: 0x04006C72 RID: 27762
		private bool devEditMode;

		// Token: 0x04006C73 RID: 27763
		private List<Color> allColors;

		// Token: 0x04006C74 RID: 27764
		private List<Color> allHairColors;

		// Token: 0x04006C75 RID: 27765
		private List<Color> allSkinColors;

		// Token: 0x04006C76 RID: 27766
		private float colorsHeight;

		// Token: 0x04006C77 RID: 27767
		private static readonly Vector2 ButSize = new Vector2(200f, 40f);

		// Token: 0x04006C78 RID: 27768
		private static readonly Vector3 PortraitOffset = new Vector3(0f, 0f, 0.15f);

		// Token: 0x04006C79 RID: 27769
		private const float PortraitZoom = 1.1f;

		// Token: 0x04006C7A RID: 27770
		private const float TabMargin = 18f;

		// Token: 0x04006C7B RID: 27771
		private const float IconSize = 60f;

		// Token: 0x04006C7C RID: 27772
		private const float LeftRectPercent = 0.3f;

		// Token: 0x04006C7D RID: 27773
		private const float ApparelRowHeight = 126f;

		// Token: 0x04006C7E RID: 27774
		private const float ApparelRowButtonsHeight = 24f;

		// Token: 0x04006C7F RID: 27775
		private const float SetColorButtonWidth = 200f;

		// Token: 0x04006C80 RID: 27776
		private static readonly Texture2D FavoriteColorTex = ContentFinder<Texture2D>.Get("UI/Icons/ColorSelector/ColorFavourite", true);

		// Token: 0x04006C81 RID: 27777
		private static readonly Texture2D IdeoColorTex = ContentFinder<Texture2D>.Get("UI/Icons/ColorSelector/ColorIdeology", true);

		// Token: 0x04006C82 RID: 27778
		private static List<string> tmpUnwantedStyleNames = new List<string>();

		// Token: 0x04006C83 RID: 27779
		private static List<StyleItemDef> tmpStyleItems = new List<StyleItemDef>();

		// Token: 0x0200397E RID: 14718
		public enum StylingTab
		{
			// Token: 0x0400E50F RID: 58639
			Hair,
			// Token: 0x0400E510 RID: 58640
			Beard,
			// Token: 0x0400E511 RID: 58641
			TattooFace,
			// Token: 0x0400E512 RID: 58642
			TattooBody,
			// Token: 0x0400E513 RID: 58643
			ApparelColor,
			// Token: 0x0400E514 RID: 58644
			BodyType,
			// Token: 0x0400E515 RID: 58645
			HeadType
		}
	}
}
