using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CA8 RID: 7336
	public class Dialog_EditPrecept : Window
	{
		// Token: 0x17002227 RID: 8743
		// (get) Token: 0x0600BBDD RID: 48093 RVA: 0x0042D52C File Offset: 0x0042B72C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, this.windowHeight);
			}
		}

		// Token: 0x0600BBDE RID: 48094 RVA: 0x0042D540 File Offset: 0x0042B740
		public Dialog_EditPrecept(Precept precept) : base(null)
		{
			this.precept = precept;
			this.absorbInputAroundWindow = true;
			this.newPreceptName = precept.Label;
			Precept_Ritual ritual = precept as Precept_Ritual;
			if (ritual != null)
			{
				this.dateTrigger = ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
				if (this.dateTrigger != null)
				{
					this.newTriggerDaysSinceStartOfYear = this.dateTrigger.triggerDaysSinceStartOfYear;
					this.RecalculateQuadrumAndDay();
				}
				this.newCanStartAnytime = ritual.isAnytime;
				this.attachableOutcomeEffects = DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs.ToList<RitualAttachableOutcomeEffectDef>();
				this.attachableUsableOutcomeEffects = (from e in this.attachableOutcomeEffects
				where e.CanAttachToRitual(ritual)
				select e).ToList<RitualAttachableOutcomeEffectDef>();
				this.selectedReward = ritual.attachableOutcomeEffect;
			}
			if (precept.def.leaderRole)
			{
				this.newPreceptNameFemale = precept.ideo.leaderTitleFemale;
			}
			this.apparelRequirements = ((precept.ApparelRequirements != null) ? precept.ApparelRequirements.ToList<PreceptApparelRequirement>() : null);
			Precept_Building precept_Building = precept as Precept_Building;
			if (precept_Building != null)
			{
				this.selectedStyle = precept_Building.ideo.GetStyleAndCategoryFor(precept_Building.ThingDef);
			}
			this.UpdateWindowHeight();
		}

		// Token: 0x0600BBDF RID: 48095 RVA: 0x0042D68C File Offset: 0x0042B88C
		private void UpdateWindowHeight()
		{
			this.windowHeight = 170f;
			if (this.precept.def.leaderRole || this.dateTrigger != null)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
			}
			if (this.apparelRequirements != null)
			{
				this.windowHeight += 39f;
				this.windowHeight += 40f;
				this.windowHeight += 30f;
				if (!this.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					foreach (PreceptApparelRequirement preceptApparelRequirement in this.apparelRequirements)
					{
						int num = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).EnumerableCount();
						this.windowHeight += (float)num * 30f;
					}
					this.windowHeight += (float)this.apparelRequirements.Count * 10f;
				}
			}
			if (this.precept is Precept_Ritual)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				if (this.selectedReward != null)
				{
					float x = Text.CalcSize(this.selectedReward.LabelCap).x;
					this.attachedRewardDescWidth = x + Dialog_EditPrecept.ButSize.x + 4f;
					this.attachedRewardDescHeight = Text.CalcHeight(this.selectedReward.effectDesc, this.attachedRewardDescWidth);
					this.windowHeight += this.attachedRewardDescHeight;
				}
			}
			Precept_Building precept_Building = this.precept as Precept_Building;
			if (precept_Building != null && this.StylesForBuilding(precept_Building).Count > 1)
			{
				int val = this.NumStyleRows(precept_Building);
				this.windowHeight += (float)Math.Min(val, 2) * 74f + 14f;
			}
			Precept_Relic precept_Relic = this.precept as Precept_Relic;
			if (precept_Relic != null && precept_Relic.ThingDef.MadeFromStuff)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
			}
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0600BBE0 RID: 48096 RVA: 0x0042D8F0 File Offset: 0x0042BAF0
		private int NumStyleRows(Precept_Building building)
		{
			int num = (int)(this.InitialSize.x - this.Margin * 2f - this.InitialSize.x / 3f) / 68;
			return this.StylesForBuilding(building).Count / num + 1;
		}

		// Token: 0x0600BBE1 RID: 48097 RVA: 0x0042D93C File Offset: 0x0042BB3C
		public override void OnAcceptKeyPressed()
		{
			this.ApplyChanges();
			Event.current.Use();
		}

		// Token: 0x0600BBE2 RID: 48098 RVA: 0x0042D950 File Offset: 0x0042BB50
		public override void DoWindowContents(Rect rect)
		{
			Dialog_EditPrecept.<>c__DisplayClass34_0 CS$<>8__locals1 = new Dialog_EditPrecept.<>c__DisplayClass34_0();
			CS$<>8__locals1.<>4__this = this;
			float num = rect.x + rect.width / 3f;
			float num2 = rect.xMax - num;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditName".Translate());
			Text.Font = GameFont.Small;
			float num3 = rect.y + 35f + 10f;
			if (this.precept.def.leaderRole)
			{
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "LeaderTitle".Translate() + " (" + Gender.Male.GetLabel(false) + ")");
				this.newPreceptName = Widgets.TextField(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), this.newPreceptName, 99999, Dialog_EditPrecept.ValidSymbolRegex);
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
				if (this.newPreceptName.Length > 32)
				{
					Messages.Message("PreceptNameTooLong".Translate(32), null, MessageTypeDefOf.RejectInput, false);
					this.newPreceptName = this.newPreceptName.Substring(0, 32);
				}
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "LeaderTitle".Translate() + " (" + Gender.Female.GetLabel(false) + ")");
				this.newPreceptNameFemale = Widgets.TextField(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), this.newPreceptNameFemale, 99999, Dialog_EditPrecept.ValidSymbolRegex);
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
				if (this.newPreceptNameFemale.Length > 32)
				{
					Messages.Message("PreceptNameTooLong".Translate(32), null, MessageTypeDefOf.RejectInput, false);
					this.newPreceptNameFemale = this.newPreceptNameFemale.Substring(0, 32);
				}
			}
			else
			{
				Dialog_EditPrecept.<>c__DisplayClass34_1 CS$<>8__locals2 = new Dialog_EditPrecept.<>c__DisplayClass34_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "Name".Translate());
				this.newPreceptName = Widgets.TextField(new Rect(num, num3, num2 - Dialog_EditPrecept.EditFieldHeight - 4f, Dialog_EditPrecept.EditFieldHeight), this.newPreceptName, 99999, Dialog_EditPrecept.ValidSymbolRegex);
				if (this.newPreceptName.Length > 32)
				{
					Messages.Message("PreceptNameTooLong".Translate(32), null, MessageTypeDefOf.RejectInput, false);
					this.newPreceptName = this.newPreceptName.Substring(0, 32);
				}
				Rect rect2 = new Rect(rect.xMax - 35f, num3, Dialog_EditPrecept.EditFieldHeight, Dialog_EditPrecept.EditFieldHeight);
				if (Widgets.ButtonImage(rect2, this.precept.nameLocked ? IdeoUIUtility.LockedTex : IdeoUIUtility.UnlockedTex, true, null))
				{
					this.precept.nameLocked = !this.precept.nameLocked;
					if (this.precept.nameLocked)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					TaggedString str = "LockPreceptNameButtonDesc".Translate() + "\n\n" + (this.precept.nameLocked ? "LockInOn" : "LockInOff").Translate("PreceptName".Translate(), "PreceptNameLower".Translate());
					TooltipHandler.TipRegion(rect2, str);
				}
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
				CS$<>8__locals2.ritual = (this.precept as Precept_Ritual);
				if (CS$<>8__locals2.ritual != null)
				{
					if (CS$<>8__locals2.ritual.canBeAnytime && !CS$<>8__locals2.ritual.sourcePattern.alwaysStartAnytime)
					{
						Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition".Translate());
						bool flag = this.newCanStartAnytime;
						if (Widgets.RadioButton(num, num3, flag, false))
						{
							flag = true;
						}
						Widgets.Label(new Rect(num + 10f + 24f, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition_Anytime".Translate());
						num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						if (Widgets.RadioButton(num, num3, !flag, false))
						{
							flag = false;
						}
						Widgets.Label(new Rect(num + 10f + 24f, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition_Date".Translate());
						num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						if (flag != this.newCanStartAnytime)
						{
							this.newCanStartAnytime = flag;
							this.UpdateWindowHeight();
						}
						if (this.dateTrigger != null && !this.newCanStartAnytime)
						{
							Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "Date".Translate());
							if (Widgets.ButtonText(new Rect(num, num3, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), Find.ActiveLanguageWorker.OrdinalNumber(this.day + 1, Gender.None), true, true, true, null))
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < 15; i++)
								{
									int d = i;
									list.Add(new FloatMenuOption(Find.ActiveLanguageWorker.OrdinalNumber(d + 1, Gender.None), delegate()
									{
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.day = d;
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.newTriggerDaysSinceStartOfYear = (int)(CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum * (Quadrum)15 + (byte)CS$<>8__locals2.CS$<>8__locals1.<>4__this.day);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
							if (Widgets.ButtonText(new Rect(num + num2 / 4f + 10f, num3, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), this.quadrum.Label(), true, true, true, null))
							{
								List<FloatMenuOption> list2 = new List<FloatMenuOption>();
								using (List<Quadrum>.Enumerator enumerator = QuadrumUtility.QuadrumsInChronologicalOrder.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										Quadrum q = enumerator.Current;
										list2.Add(new FloatMenuOption(q.Label(), delegate()
										{
											CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum = q;
											CS$<>8__locals2.CS$<>8__locals1.<>4__this.newTriggerDaysSinceStartOfYear = (int)(CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum * (Quadrum)15 + (byte)CS$<>8__locals2.CS$<>8__locals1.<>4__this.day);
										}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
								}
								Find.WindowStack.Add(new FloatMenu(list2));
							}
							num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						}
					}
					if (CS$<>8__locals2.ritual.SupportsAttachableOutcomeEffect)
					{
						Widgets.Label(new Rect(rect.x, num3, num2 / 2f, Dialog_EditPrecept.EditFieldHeight), "RitualAttachedReward".Translate());
						if (this.selectedReward != null)
						{
							GUI.color = Color.gray;
							string value = this.selectedReward.AppliesToOutcomesString(CS$<>8__locals2.ritual.outcomeEffect.def);
							Widgets.Label(new Rect(rect.x, num3 + Dialog_EditPrecept.EditFieldHeight, num2 / 2f, Dialog_EditPrecept.EditFieldHeight * 2f), (this.selectedReward.AppliesToSeveralOutcomes(CS$<>8__locals2.ritual) ? "RitualAttachedApplliesForOutcomes" : "RitualAttachedApplliesForOutcome").Translate(value));
							Widgets.Label(new Rect(num, num3 + Dialog_EditPrecept.EditFieldHeight, this.attachedRewardDescWidth, this.attachedRewardDescHeight), this.selectedReward.effectDesc.CapitalizeFirst());
							GUI.color = Color.white;
						}
						TaggedString taggedString = (this.selectedReward != null) ? this.selectedReward.LabelCap : "None".Translate();
						Widgets.Label(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), taggedString);
						float num4 = num + Text.CalcSize(taggedString).x + 10f;
						if (this.selectedReward != null)
						{
							TooltipHandler.TipRegion(new Rect(rect.x, num3, num4 - rect.x, Dialog_EditPrecept.EditFieldHeight), new TipSignal(() => CS$<>8__locals2.CS$<>8__locals1.<>4__this.selectedReward.TooltipForRitual(CS$<>8__locals2.ritual), CS$<>8__locals2.ritual.Id));
						}
						if (Widgets.ButtonText(new Rect(num4, num3 - 4f, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), "RitualAttachedRewardChoose".Translate() + "...", true, true, this.attachableOutcomeEffects.Any<RitualAttachableOutcomeEffectDef>(), null))
						{
							List<FloatMenuOption> list3 = new List<FloatMenuOption>();
							if (this.selectedReward != null)
							{
								list3.Add(new FloatMenuOption("None".Translate(), delegate()
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.selectedReward = null;
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.UpdateWindowHeight();
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							using (List<RitualAttachableOutcomeEffectDef>.Enumerator enumerator2 = this.attachableOutcomeEffects.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Dialog_EditPrecept.<>c__DisplayClass34_4 CS$<>8__locals5 = new Dialog_EditPrecept.<>c__DisplayClass34_4();
									CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals2;
									CS$<>8__locals5.eff = enumerator2.Current;
									if (CS$<>8__locals5.eff != this.selectedReward)
									{
										AcceptanceReport report = CS$<>8__locals5.eff.CanAttachToRitual(CS$<>8__locals5.CS$<>8__locals4.ritual);
										if (report)
										{
											list3.Add(new FloatMenuOption(CS$<>8__locals5.eff.LabelCap, delegate()
											{
												CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.<>4__this.selectedReward = CS$<>8__locals5.eff;
												CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.<>4__this.UpdateWindowHeight();
											}, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals5.<DoWindowContents>g__DrawTooltip|4), null, 0f, null, null, true, 0));
										}
										else
										{
											list3.Add(new FloatMenuOption(CS$<>8__locals5.eff.LabelCap + " (" + report.Reason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
										}
									}
								}
							}
							if (list3.Any<FloatMenuOption>())
							{
								Find.WindowStack.Add(new FloatMenu(list3));
							}
						}
					}
				}
			}
			if (this.apparelRequirements != null)
			{
				string text = "EditApparelRequirement".Translate();
				Text.Font = GameFont.Medium;
				Vector2 vector = Text.CalcSize(text);
				Widgets.Label(new Rect(rect.x, num3, rect.width, 35f), text);
				Text.Font = GameFont.Small;
				num3 += 39f;
				text = "NoteRoleApparelRequirementEffects".Translate();
				Rect rect3 = new Rect(rect.x, num3, rect.width, 30f);
				Color color = GUI.color;
				GUI.color = Color.gray;
				Widgets.Label(rect3, text);
				GUI.color = color;
				num3 += 40f;
				int num5 = -1;
				for (int j = 0; j < this.apparelRequirements.Count; j++)
				{
					PreceptApparelRequirement preceptApparelRequirement = this.apparelRequirements[j];
					Rect rect4 = new Rect(rect.x + rect.width - 100f, num3, 100f, 30f);
					if (Widgets.ButtonText(rect4, "Remove".Translate(), true, true, true, null))
					{
						num5 = j;
					}
					string text2;
					if (preceptApparelRequirement.RequirementOverlapsOther(this.apparelRequirements, out text2))
					{
						text2 = text2.CapitalizeFirst().Colorize(ColorLibrary.Yellow);
					}
					foreach (ThingDef thingDef in preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None))
					{
						Rect rect5 = new Rect(rect.x + 26f + 10f + 24f, num3 + 3f, 24f, 30f);
						Rect rect6 = new Rect(rect5.x + 32f, num3, text2.NullOrEmpty() ? (rect.width - rect5.width - rect4.width - 32f) : 100f, 30f);
						text = thingDef.LabelCap;
						vector = Text.CalcSize(text);
						rect6.y += (rect6.height - vector.y) / 2f;
						Widgets.Label(rect6, text.Truncate(rect6.width, null));
						Widgets.DefIcon(rect5, thingDef, null, 1f, null, false, null, null, null, 1f);
						if (!text2.NullOrEmpty())
						{
							Widgets.Label(new Rect(rect6.xMax + 4f, rect6.y, rect.width - rect6.width - rect5.width - rect4.width, 30f), text2);
						}
						num3 += rect6.height;
					}
					num3 += 10f;
				}
				if (num5 != -1)
				{
					this.apparelRequirements.RemoveAt(num5);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					this.UpdateWindowHeight();
				}
				if (Widgets.ButtonText(new Rect(rect.x + 26f, num3, rect.width - 26f, 30f), "Add".Translate().CapitalizeFirst() + "...", true, true, true, null))
				{
					bool flag2 = false;
					foreach (MemeDef memeDef in this.precept.ideo.memes)
					{
						if (memeDef.preventApparelRequirements)
						{
							flag2 = true;
							Messages.Message("CannotNotAddRoleApparelDueToMeme".Translate(memeDef.LabelCap.Named("MEME")), MessageTypeDefOf.RejectInput, false);
							break;
						}
					}
					if (!flag2)
					{
						List<FloatMenuOption> list4 = new List<FloatMenuOption>();
						foreach (PreceptApparelRequirement preceptApparelRequirement2 in Precept_Role.AllPossibleRequirements(this.precept.ideo, this.precept.def, true))
						{
							PreceptApparelRequirement localReq = preceptApparelRequirement2;
							List<ThingDef> list5 = preceptApparelRequirement2.requirement.AllRequiredApparel(Gender.None).ToList<ThingDef>();
							if (list5.Count > 0)
							{
								FloatMenuOption floatMenuOption = new FloatMenuOption(string.Join<TaggedString>(", ", from ap in list5
								select ap.LabelCap), delegate()
								{
									CS$<>8__locals1.<>4__this.apparelRequirements.Add(localReq);
									CS$<>8__locals1.<>4__this.UpdateWindowHeight();
								}, list5[0].uiIcon, list5[0].uiIconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								string str2;
								floatMenuOption.Disabled = !preceptApparelRequirement2.CanAddRequirement(this.precept, this.apparelRequirements, out str2, null);
								if (floatMenuOption.Disabled)
								{
									FloatMenuOption floatMenuOption2 = floatMenuOption;
									floatMenuOption2.Label = floatMenuOption2.Label + " (" + str2 + ")";
								}
								list4.Add(floatMenuOption);
							}
						}
						Find.WindowStack.Add(new FloatMenu(list4));
					}
				}
			}
			Precept precept = this.precept;
			CS$<>8__locals1.building = (precept as Precept_Building);
			if (CS$<>8__locals1.building != null && this.StylesForBuilding(CS$<>8__locals1.building).Count > 1)
			{
				ThingDef thingDef2 = CS$<>8__locals1.building.ThingDef;
				Rect rect7 = new Rect(rect);
				rect7.y += num3;
				float num6 = 8f;
				Widgets.Label(rect7, "Appearance".Translate() + ":");
				List<StyleCategoryPair> elements = this.StylesForBuilding(CS$<>8__locals1.building);
				int val = this.NumStyleRows(CS$<>8__locals1.building);
				Rect outRect = new Rect
				{
					x = rect.x + this.InitialSize.x / 3f,
					y = rect.y + num3,
					width = rect.width - this.InitialSize.x / 3f,
					height = (float)Math.Min(val, 2) * 68f + num6
				};
				Widgets.BeginScrollView(outRect, ref this.styleListScrollPos, new Rect(0f, 0f, outRect.width - 16f, this.styleListLastHeight + num6), true);
				this.styleListLastHeight = GenUI.DrawElementStack<StyleCategoryPair>(new Rect(num6, num6, outRect.width - num6, 99999f), 64f, elements, delegate(Rect r, StyleCategoryPair obj)
				{
					if (CS$<>8__locals1.<>4__this.selectedStyle != null && CS$<>8__locals1.<>4__this.selectedStyle.styleDef == obj.styleDef)
					{
						Widgets.DrawBoxSolid(r, new Color(1f, 0.8f, 0.2f, 0.2f));
					}
					if (Mouse.IsOver(r))
					{
						if (Widgets.ButtonInvisible(r, true))
						{
							CS$<>8__locals1.<>4__this.selectedStyle = obj;
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
						TooltipHandler.TipRegion(r, "Origin".Translate() + " " + obj.category.LabelCap + ".");
						Widgets.DrawHighlight(r);
					}
					Widgets.DefIcon(r, CS$<>8__locals1.building.ThingDef, GenStuff.DefaultStuffFor(CS$<>8__locals1.building.ThingDef), 1f, obj.styleDef, false, null, null, null, 1f);
				}, (StyleCategoryPair obj) => 64f, 6f, 5f, true).height;
				Widgets.EndScrollView();
			}
			precept = this.precept;
			CS$<>8__locals1.relic = (precept as Precept_Relic);
			if (CS$<>8__locals1.relic != null && CS$<>8__locals1.relic.ThingDef.MadeFromStuff)
			{
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "RelicStuff".Translate() + ":");
				Vector2 vector2 = Text.CalcSize(CS$<>8__locals1.relic.stuff.LabelCap);
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(num, num3, vector2.x, Dialog_EditPrecept.EditFieldHeight);
				Widgets.Label(rect8, CS$<>8__locals1.relic.stuff.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				Rect rect9 = new Rect(rect8.xMax + 4f, num3, Dialog_EditPrecept.EditFieldHeight, Dialog_EditPrecept.EditFieldHeight);
				Widgets.DefIcon(rect9, CS$<>8__locals1.relic.stuff, null, 1f, null, false, null, null, null, 1f);
				TaggedString taggedString2 = "ChooseStuffForRelic".Translate() + "...";
				Vector2 vector3 = Text.CalcSize(taggedString2);
				if (Widgets.ButtonText(new Rect(rect9.xMax + 4f, num3, vector3.x + 20f, Dialog_EditPrecept.EditFieldHeight), taggedString2, true, true, true, null))
				{
					IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(CS$<>8__locals1.relic.ThingDef, TechLevel.Undefined, false);
					if (enumerable.Count<ThingDef>() > 0)
					{
						List<FloatMenuOption> list6 = new List<FloatMenuOption>();
						foreach (ThingDef thingDef3 in enumerable)
						{
							ThingDef localStuff = thingDef3;
							list6.Add(new FloatMenuOption(thingDef3.LabelCap, delegate()
							{
								CS$<>8__locals1.relic.stuff = localStuff;
							}, thingDef3, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
						}
						Find.WindowStack.Add(new FloatMenu(list6));
					}
				}
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "Back".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_EditPrecept.ButSize.x / 2f, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "Randomize".Translate(), true, true, true, null))
			{
				this.newPreceptName = (this.newPreceptNameFemale = this.precept.GenerateNewName());
				this.apparelRequirements = this.precept.GenerateNewApparelRequirements(null);
				if (this.dateTrigger != null)
				{
					this.newTriggerDaysSinceStartOfYear = this.dateTrigger.RandomDate();
					this.RecalculateQuadrumAndDay();
				}
				Precept_Ritual precept_Ritual = this.precept as Precept_Ritual;
				if (precept_Ritual != null)
				{
					if (precept_Ritual.canBeAnytime && !precept_Ritual.sourcePattern.alwaysStartAnytime)
					{
						this.newCanStartAnytime = Rand.Bool;
					}
					if (this.attachableUsableOutcomeEffects.Any<RitualAttachableOutcomeEffectDef>() && precept_Ritual.SupportsAttachableOutcomeEffect)
					{
						this.selectedReward = this.attachableUsableOutcomeEffects.RandomElement<RitualAttachableOutcomeEffectDef>();
					}
				}
				Precept_Building precept_Building = this.precept as Precept_Building;
				if (precept_Building != null)
				{
					this.selectedStyle = this.StylesForBuilding(precept_Building).RandomElement<StyleCategoryPair>();
				}
				Precept_Relic precept_Relic = this.precept as Precept_Relic;
				if (precept_Relic != null)
				{
					precept_Relic.SetRandomStuff();
				}
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.UpdateWindowHeight();
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditPrecept.ButSize.x, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x0600BBE3 RID: 48099 RVA: 0x0042EF2C File Offset: 0x0042D12C
		private void RecalculateQuadrumAndDay()
		{
			Map map = (Current.ProgramState == ProgramState.Playing) ? Find.AnyPlayerHomeMap : null;
			float longitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f;
			this.quadrum = GenDate.Quadrum((long)(this.newTriggerDaysSinceStartOfYear * 60000), longitude);
			this.day = GenDate.DayOfQuadrum((long)(this.newTriggerDaysSinceStartOfYear * 60000), longitude);
		}

		// Token: 0x0600BBE4 RID: 48100 RVA: 0x0042EF9C File Offset: 0x0042D19C
		private void ApplyChanges()
		{
			if (this.apparelRequirements != null && this.apparelRequirements.Any(delegate(PreceptApparelRequirement x)
			{
				string text;
				return x.RequirementOverlapsOther(this.apparelRequirements, out text);
			}))
			{
				Messages.Message("OverlappingRoleApparel".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.precept.SetName(this.newPreceptName);
			if (this.precept.def.leaderRole)
			{
				this.precept.ideo.leaderTitleMale = this.newPreceptName;
				this.precept.ideo.leaderTitleFemale = this.newPreceptNameFemale;
			}
			Precept_Ritual precept_Ritual = this.precept as Precept_Ritual;
			if (precept_Ritual != null)
			{
				precept_Ritual.isAnytime = this.newCanStartAnytime;
				precept_Ritual.attachableOutcomeEffect = this.selectedReward;
			}
			if (this.dateTrigger != null && this.newTriggerDaysSinceStartOfYear != this.dateTrigger.triggerDaysSinceStartOfYear)
			{
				this.dateTrigger.triggerDaysSinceStartOfYear = this.newTriggerDaysSinceStartOfYear;
			}
			foreach (Precept precept in this.precept.ideo.PreceptsListForReading)
			{
				precept.ClearTipCache();
			}
			if (this.apparelRequirements != null)
			{
				this.precept.ApparelRequirements = this.apparelRequirements;
			}
			Precept_Building precept_Building = this.precept as Precept_Building;
			if (precept_Building != null && this.selectedStyle != null)
			{
				this.precept.ideo.style.SetStyleForThingDef(precept_Building.ThingDef, this.selectedStyle);
			}
			this.Close(true);
		}

		// Token: 0x0600BBE5 RID: 48101 RVA: 0x0042F12C File Offset: 0x0042D32C
		private List<StyleCategoryPair> StylesForBuilding(Precept_Building building)
		{
			ThingDef thingDef = building.ThingDef;
			if (thingDef != null && thingDef.canEditAnyStyle)
			{
				return Precept_ThingDef.AllPossibleStylesForBuilding(building.ThingDef);
			}
			Dialog_EditPrecept.stylesForBuildingTmp.Clear();
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.precept.ideo.thingStyleCategories)
			{
				foreach (ThingDefStyle thingDefStyle in thingStyleCategoryWithPriority.category.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == building.ThingDef)
					{
						Dialog_EditPrecept.stylesForBuildingTmp.Add(new StyleCategoryPair
						{
							category = thingStyleCategoryWithPriority.category,
							styleDef = thingDefStyle.StyleDef
						});
					}
				}
			}
			return Dialog_EditPrecept.stylesForBuildingTmp;
		}

		// Token: 0x04006B1E RID: 27422
		private Precept precept;

		// Token: 0x04006B1F RID: 27423
		private string newPreceptName;

		// Token: 0x04006B20 RID: 27424
		private string newPreceptNameFemale;

		// Token: 0x04006B21 RID: 27425
		private float windowHeight = 170f;

		// Token: 0x04006B22 RID: 27426
		private float attachedRewardDescWidth;

		// Token: 0x04006B23 RID: 27427
		private float attachedRewardDescHeight;

		// Token: 0x04006B24 RID: 27428
		private int newTriggerDaysSinceStartOfYear = -1;

		// Token: 0x04006B25 RID: 27429
		private bool newCanStartAnytime;

		// Token: 0x04006B26 RID: 27430
		private Quadrum quadrum;

		// Token: 0x04006B27 RID: 27431
		private int day;

		// Token: 0x04006B28 RID: 27432
		private StyleCategoryPair selectedStyle;

		// Token: 0x04006B29 RID: 27433
		private float styleListLastHeight;

		// Token: 0x04006B2A RID: 27434
		private Vector2 styleListScrollPos;

		// Token: 0x04006B2B RID: 27435
		private RitualObligationTrigger_Date dateTrigger;

		// Token: 0x04006B2C RID: 27436
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006B2D RID: 27437
		private static readonly float EditFieldHeight = 30f;

		// Token: 0x04006B2E RID: 27438
		private const float ApparelRequirementThingHeight = 30f;

		// Token: 0x04006B2F RID: 27439
		private const float ApparelRequirementBtnHeight = 30f;

		// Token: 0x04006B30 RID: 27440
		private const float WindowBaseHeight = 170f;

		// Token: 0x04006B31 RID: 27441
		private const float HeaderHeight = 35f;

		// Token: 0x04006B32 RID: 27442
		private const float ApparelRequirementNoteHeight = 30f;

		// Token: 0x04006B33 RID: 27443
		private const float BuildingStyleIconSize = 64f;

		// Token: 0x04006B34 RID: 27444
		private const int MaxPreceptNameLength = 32;

		// Token: 0x04006B35 RID: 27445
		private static readonly Regex ValidSymbolRegex = new Regex("^[\\p{L}0-9 '\\-]*$");

		// Token: 0x04006B36 RID: 27446
		private List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x04006B37 RID: 27447
		private RitualAttachableOutcomeEffectDef selectedReward;

		// Token: 0x04006B38 RID: 27448
		private List<RitualAttachableOutcomeEffectDef> attachableOutcomeEffects;

		// Token: 0x04006B39 RID: 27449
		private List<RitualAttachableOutcomeEffectDef> attachableUsableOutcomeEffects;

		// Token: 0x04006B3A RID: 27450
		private static List<StyleCategoryPair> stylesForBuildingTmp = new List<StyleCategoryPair>();
	}
}
