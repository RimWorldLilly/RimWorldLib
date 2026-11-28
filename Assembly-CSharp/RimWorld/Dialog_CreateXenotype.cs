using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CA3 RID: 7331
	public class Dialog_CreateXenotype : GeneCreationDialogBase
	{
		// Token: 0x1700221E RID: 8734
		// (get) Token: 0x0600BBAA RID: 48042 RVA: 0x0042A329 File Offset: 0x00428529
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(UI.screenWidth, 1036), (float)(UI.screenHeight - 4));
			}
		}

		// Token: 0x1700221F RID: 8735
		// (get) Token: 0x0600BBAB RID: 48043 RVA: 0x0042A348 File Offset: 0x00428548
		protected override List<GeneDef> SelectedGenes
		{
			get
			{
				return this.selectedGenes;
			}
		}

		// Token: 0x17002220 RID: 8736
		// (get) Token: 0x0600BBAC RID: 48044 RVA: 0x0042A350 File Offset: 0x00428550
		protected override string Header
		{
			get
			{
				return "CreateXenotype".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17002221 RID: 8737
		// (get) Token: 0x0600BBAD RID: 48045 RVA: 0x0042A374 File Offset: 0x00428574
		protected override string AcceptButtonLabel
		{
			get
			{
				return "SaveAndApply".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x0600BBAE RID: 48046 RVA: 0x0042A398 File Offset: 0x00428598
		public Dialog_CreateXenotype(int index, Action callback)
		{
			this.generationRequestIndex = index;
			this.callback = callback;
			this.xenotypeName = string.Empty;
			this.closeOnAccept = false;
			this.absorbInputAroundWindow = true;
			this.alwaysUseFullBiostatsTableHeight = true;
			this.searchWidgetOffsetX = GeneCreationDialogBase.ButSize.x * 2f + 4f;
			foreach (GeneCategoryDef key in DefDatabase<GeneCategoryDef>.AllDefs)
			{
				this.collapsedCategories.Add(key, false);
			}
			this.OnGenesChanged();
		}

		// Token: 0x0600BBAF RID: 48047 RVA: 0x0042A470 File Offset: 0x00428670
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenotype creation"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600BBB0 RID: 48048 RVA: 0x0042A48C File Offset: 0x0042868C
		protected override void DrawGenes(Rect rect)
		{
			this.hoveredAnyGene = false;
			GUI.BeginGroup(rect);
			float num = 0f;
			this.DrawSection(new Rect(0f, 0f, rect.width, this.selectedHeight), this.selectedGenes, "SelectedGenes".Translate(), ref num, ref this.selectedHeight, false, rect, ref this.selectedCollapsed);
			if (!this.selectedCollapsed.Value)
			{
				num += 10f;
			}
			float num2 = num;
			Widgets.Label(0f, ref num, rect.width, "Genes".Translate().CapitalizeFirst(), default(TipSignal));
			num += 10f;
			float height = num - num2 - 4f;
			if (Widgets.ButtonText(new Rect(rect.width - 150f - 16f, num2, 150f, height), "CollapseAllCategories".Translate(), true, true, true, null))
			{
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				foreach (GeneCategoryDef key in DefDatabase<GeneCategoryDef>.AllDefs)
				{
					this.collapsedCategories[key] = true;
				}
			}
			if (Widgets.ButtonText(new Rect(rect.width - 300f - 4f - 16f, num2, 150f, height), "ExpandAllCategories".Translate(), true, true, true, null))
			{
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				foreach (GeneCategoryDef key2 in DefDatabase<GeneCategoryDef>.AllDefs)
				{
					this.collapsedCategories[key2] = false;
				}
			}
			float num3 = num;
			Rect rect2 = new Rect(0f, num, rect.width - 16f, this.scrollHeight);
			Widgets.BeginScrollView(new Rect(0f, num, rect.width, rect.height - num), ref this.scrollPosition, rect2, true);
			Rect containingRect = rect2;
			containingRect.y = num + this.scrollPosition.y;
			containingRect.height = rect.height;
			bool? flag = null;
			this.DrawSection(rect, GeneUtility.GenesInOrder, null, ref num, ref this.unselectedHeight, true, containingRect, ref flag);
			if (Event.current.type == EventType.layout)
			{
				this.scrollHeight = num - num3;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			if (!this.hoveredAnyGene)
			{
				this.hoveredGene = null;
			}
		}

		// Token: 0x0600BBB1 RID: 48049 RVA: 0x0042A748 File Offset: 0x00428948
		private void DrawSection(Rect rect, List<GeneDef> genes, string label, ref float curY, ref float sectionHeight, bool adding, Rect containingRect, ref bool? collapsed)
		{
			float num = 4f;
			if (!label.NullOrEmpty())
			{
				Rect rect2 = new Rect(0f, curY, rect.width, Text.LineHeight);
				rect2.xMax -= (adding ? 16f : (Text.CalcSize("ClickToAddOrRemove".Translate()).x + 4f));
				if (collapsed != null)
				{
					Rect position = new Rect(rect2.x, rect2.y + (rect2.height - 18f) / 2f, 18f, 18f);
					GUI.DrawTexture(position, collapsed.Value ? TexButton.Reveal : TexButton.Collapse);
					if (Widgets.ButtonInvisible(rect2, true))
					{
						collapsed = !collapsed;
						if (collapsed.Value)
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
						else
						{
							SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
						}
					}
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
					}
					rect2.xMin += position.width;
				}
				Widgets.Label(rect2, label);
				if (!adding)
				{
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = ColoredText.SubtleGrayColor;
					Widgets.Label(new Rect(rect2.xMax - 18f, curY, rect.width - rect2.width, Text.LineHeight), "ClickToAddOrRemove".Translate());
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
				}
				curY += Text.LineHeight + 3f;
			}
			if (collapsed.GetValueOrDefault())
			{
				if (Event.current.type == EventType.layout)
				{
					sectionHeight = 0f;
				}
				return;
			}
			float num2 = curY;
			bool flag = false;
			float num3 = 34f + GeneCreationDialogBase.GeneSize.x + 8f;
			float num4 = GeneCreationDialogBase.GeneSize.y + 8f + 4f;
			float num5 = rect.width - 16f;
			float num6 = num3 + 4f;
			float b = (num5 - num6 * Mathf.Floor(num5 / num6)) / 2f;
			Rect rect3 = new Rect(0f, curY, rect.width, sectionHeight);
			if (!adding)
			{
				Widgets.DrawRectFast(rect3, Widgets.MenuSectionBGFillColor, null);
			}
			curY += 4f;
			if (!genes.Any<GeneDef>())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = ColoredText.SubtleGrayColor;
				Widgets.Label(rect3, "(" + "NoneLower".Translate() + ")");
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				curY += num4;
			}
			else
			{
				float num7 = rect.width - 16f;
				int num8 = Mathf.FloorToInt(num7 / (num3 + 1f));
				int num9 = Mathf.CeilToInt((float)genes.Count / (float)num8);
				float num10 = Mathf.Max(new float[]
				{
					num4 * (float)Mathf.Min(num9, 3)
				});
				float num11 = num4 * (float)num9;
				bool flag4;
				if (!adding)
				{
					bool? flag2 = collapsed;
					bool flag3 = false;
					if ((flag2.GetValueOrDefault() == flag3 & flag2 != null) && genes.Count > 0)
					{
						flag4 = (num11 > num10);
						goto IL_35B;
					}
				}
				flag4 = false;
				IL_35B:
				bool flag5 = flag4;
				float num12 = curY;
				if (flag5)
				{
					num12 = 0f;
					Rect outRect = new Rect(0f, curY, rect.width, num10);
					Rect viewRect = new Rect(0f, 0f, num7, num11);
					Widgets.BeginScrollView(outRect, ref this.selectedScrollPosition, viewRect, true);
				}
				GeneCategoryDef geneCategoryDef = null;
				int num13 = 0;
				for (int i = 0; i < genes.Count; i++)
				{
					GeneDef geneDef = genes[i];
					if ((!adding || !this.quickSearchWidget.filter.Active || (this.matchingGenes.Contains(geneDef) && !this.selectedGenes.Contains(geneDef)) || this.matchingCategories.Contains(geneDef.displayCategory)) && (this.ignoreRestrictions || geneDef.biostatArc <= 0))
					{
						bool flag6 = false;
						if (num + num3 > num5)
						{
							num = 4f;
							num12 += GeneCreationDialogBase.GeneSize.y + 8f + 4f;
							flag6 = true;
						}
						bool flag7 = this.quickSearchWidget.filter.Active && (this.matchingGenes.Contains(geneDef) || this.matchingCategories.Contains(geneDef.displayCategory));
						bool flag8 = this.collapsedCategories[geneDef.displayCategory] && !flag7;
						if (adding && geneCategoryDef != geneDef.displayCategory)
						{
							if (!flag6 && flag)
							{
								num = 4f;
								num12 += GeneCreationDialogBase.GeneSize.y + 8f + 4f;
							}
							geneCategoryDef = geneDef.displayCategory;
							Rect rect4 = new Rect(num, num12, rect.width - 8f, Text.LineHeight);
							if (!flag7)
							{
								Rect position2 = new Rect(rect4.x, rect4.y + (rect4.height - 18f) / 2f, 18f, 18f);
								GUI.DrawTexture(position2, flag8 ? TexButton.Reveal : TexButton.Collapse);
								if (Widgets.ButtonInvisible(rect4, true))
								{
									this.collapsedCategories[geneDef.displayCategory] = !this.collapsedCategories[geneDef.displayCategory];
									if (this.collapsedCategories[geneDef.displayCategory])
									{
										SoundDefOf.TabClose.PlayOneShotOnCamera(null);
									}
									else
									{
										SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
									}
								}
								if (num13 % 2 == 1)
								{
									Widgets.DrawLightHighlight(rect4);
								}
								if (Mouse.IsOver(rect4))
								{
									Widgets.DrawHighlight(rect4);
								}
								rect4.xMin += position2.width;
							}
							Widgets.Label(rect4, geneCategoryDef.LabelCap);
							num12 += rect4.height;
							if (!flag8)
							{
								GUI.color = Color.grey;
								Widgets.DrawLineHorizontal(num, num12, rect.width - 8f);
								GUI.color = Color.white;
								num12 += 10f;
							}
							num13++;
						}
						if (adding && flag8)
						{
							flag = false;
							if (Event.current.type == EventType.layout)
							{
								sectionHeight = num12 - num2;
							}
						}
						else
						{
							num = Mathf.Max(num, b);
							flag = true;
							if (this.DrawGene(geneDef, !adding, ref num, num12, num3, containingRect, flag7))
							{
								if (this.selectedGenes.Contains(geneDef))
								{
									SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
									this.selectedGenes.Remove(geneDef);
								}
								else
								{
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
									this.selectedGenes.Add(geneDef);
								}
								if (!this.xenotypeNameLocked)
								{
									this.xenotypeName = GeneUtility.GenerateXenotypeNameFromGenes(this.SelectedGenes);
								}
								this.OnGenesChanged();
								break;
							}
						}
					}
				}
				num12 += num4;
				if (flag5)
				{
					Widgets.EndScrollView();
					curY += num10;
				}
				else
				{
					curY = num12;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				sectionHeight = curY - num2;
			}
		}

		// Token: 0x0600BBB2 RID: 48050 RVA: 0x0042AE8C File Offset: 0x0042908C
		private bool DrawGene(GeneDef geneDef, bool selectedSection, ref float curX, float curY, float packWidth, Rect containingRect, bool isMatch)
		{
			bool result = false;
			Rect rect = new Rect(curX, curY, packWidth, GeneCreationDialogBase.GeneSize.y + 8f);
			if (!containingRect.Overlaps(rect))
			{
				curX = rect.xMax + 4f;
				return false;
			}
			bool selected = !selectedSection && this.selectedGenes.Contains(geneDef);
			bool overridden = this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef));
			Widgets.DrawOptionBackground(rect, selected);
			curX += 4f;
			GeneUIUtility.DrawBiostats(geneDef.biostatCpx, geneDef.biostatMet, geneDef.biostatArc, ref curX, curY, 4f);
			Rect geneRect = new Rect(curX, curY + 4f, GeneCreationDialogBase.GeneSize.x, GeneCreationDialogBase.GeneSize.y);
			GeneUIUtility.DrawGeneDef(geneDef, geneRect, this.inheritable ? GeneType.Endogene : GeneType.Xenogene, () => this.GeneTip(geneDef, selectedSection), false, false, overridden);
			curX += GeneCreationDialogBase.GeneSize.x + 4f;
			if (Mouse.IsOver(rect))
			{
				this.hoveredGene = geneDef;
				this.hoveredAnyGene = true;
			}
			else if (this.hoveredGene != null && geneDef.ConflictsWith(this.hoveredGene))
			{
				Widgets.DrawLightHighlight(rect);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
			}
			curX = Mathf.Max(curX, rect.xMax + 4f);
			return result;
		}

		// Token: 0x0600BBB3 RID: 48051 RVA: 0x0042B028 File Offset: 0x00429228
		private string GeneTip(GeneDef geneDef, bool selectedSection)
		{
			string text = null;
			if (selectedSection)
			{
				if (this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.leftChosen == geneDef))
				{
					text = Dialog_CreateXenotype.<GeneTip>g__GroupInfo|26_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == geneDef));
				}
				else if (this.cachedOverriddenGenes.Contains(geneDef))
				{
					text = Dialog_CreateXenotype.<GeneTip>g__GroupInfo|26_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef)));
				}
				else if (this.randomChosenGroups.ContainsKey(geneDef))
				{
					text = ("GeneWillBeRandomChosen".Translate() + ":\n" + (from x in this.randomChosenGroups[geneDef]
					select x.label).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
				}
			}
			if (this.selectedGenes.Contains(geneDef) && geneDef.prerequisite != null && !this.selectedGenes.Contains(geneDef.prerequisite))
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += ("MessageGeneMissingPrerequisite".Translate(geneDef.label).CapitalizeFirst() + ": " + geneDef.prerequisite.LabelCap).Colorize(ColorLibrary.RedReadable);
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			return text + (this.selectedGenes.Contains(geneDef) ? "ClickToRemove" : "ClickToAdd").Translate().Colorize(ColoredText.SubtleGrayColor);
		}

		// Token: 0x0600BBB4 RID: 48052 RVA: 0x0042B21C File Offset: 0x0042941C
		protected override void PostXenotypeOnGUI(float curX, float curY)
		{
			TaggedString taggedString = "GenesAreInheritable".Translate();
			TaggedString taggedString2 = "IgnoreRestrictions".Translate();
			float width = Mathf.Max(Text.CalcSize(taggedString).x, Text.CalcSize(taggedString2).x) + 4f + 24f;
			Rect rect = new Rect(curX, curY, width, Text.LineHeight);
			Widgets.CheckboxLabeled(rect, taggedString, ref this.inheritable, false, null, null, false, false);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "GenesAreInheritableDesc".Translate());
			}
			rect.y += Text.LineHeight;
			bool ignoreRestrictions = this.ignoreRestrictions;
			Widgets.CheckboxLabeled(rect, taggedString2, ref this.ignoreRestrictions, false, null, null, false, false);
			if (ignoreRestrictions != this.ignoreRestrictions)
			{
				if (this.ignoreRestrictions)
				{
					if (!Dialog_CreateXenotype.ignoreRestrictionsConfirmationSent)
					{
						Dialog_CreateXenotype.ignoreRestrictionsConfirmationSent = true;
						Find.WindowStack.Add(new Dialog_MessageBox("IgnoreRestrictionsConfirmation".Translate(), "Yes".Translate(), delegate()
						{
						}, "No".Translate(), delegate()
						{
							this.ignoreRestrictions = false;
						}, null, false, null, null, WindowLayer.Dialog));
					}
				}
				else
				{
					this.selectedGenes.RemoveAll((GeneDef x) => x.biostatArc > 0);
					this.OnGenesChanged();
				}
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "IgnoreRestrictionsDesc".Translate());
			}
			this.postXenotypeHeight += rect.yMax - curY;
		}

		// Token: 0x0600BBB5 RID: 48053 RVA: 0x0042B3E0 File Offset: 0x004295E0
		protected override void OnGenesChanged()
		{
			this.selectedGenes.SortGeneDefs();
			base.OnGenesChanged();
		}

		// Token: 0x0600BBB6 RID: 48054 RVA: 0x0042B3F4 File Offset: 0x004295F4
		protected override void DrawSearchRect(Rect rect)
		{
			base.DrawSearchRect(rect);
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "LoadCustom".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_XenotypeList_Load(delegate(CustomXenotype xenotype)
				{
					this.xenotypeName = xenotype.name;
					this.xenotypeNameLocked = true;
					this.selectedGenes.Clear();
					this.selectedGenes.AddRange(xenotype.genes);
					this.inheritable = xenotype.inheritable;
					this.iconDef = xenotype.IconDef;
					this.OnGenesChanged();
					this.ignoreRestrictions = (this.selectedGenes.Any((GeneDef x) => x.biostatArc > 0) || !this.WithinAcceptableBiostatLimits(false));
				}));
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x * 2f - 4f, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "LoadPremade".Translate(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (XenotypeDef xenotype2 in from c in DefDatabase<XenotypeDef>.AllDefs
				orderby -c.displayPriority
				select c)
				{
					XenotypeDef xenotype = xenotype2;
					list.Add(new FloatMenuOption(xenotype.LabelCap, delegate()
					{
						this.xenotypeName = xenotype.label;
						this.selectedGenes.Clear();
						this.selectedGenes.AddRange(xenotype.genes);
						this.inheritable = xenotype.inheritable;
						this.OnGenesChanged();
						this.ignoreRestrictions = (this.selectedGenes.Any((GeneDef g) => g.biostatArc > 0) || !this.WithinAcceptableBiostatLimits(false));
					}, xenotype.Icon, XenotypeDef.IconColor, MenuOptionPriority.Default, delegate(Rect r)
					{
						TooltipHandler.TipRegion(r, xenotype.descriptionShort ?? xenotype.description);
					}, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600BBB7 RID: 48055 RVA: 0x0042B5B8 File Offset: 0x004297B8
		protected override void DoBottomButtons(Rect rect)
		{
			base.DoBottomButtons(rect);
			if (this.leftChosenGroups.Any<GeneLeftChosenGroup>())
			{
				int num = this.leftChosenGroups.Sum((GeneLeftChosenGroup x) => x.overriddenGenes.Count);
				GeneLeftChosenGroup geneLeftChosenGroup = this.leftChosenGroups[0];
				string text = "GenesConflict".Translate() + ": " + "GenesConflictDesc".Translate(geneLeftChosenGroup.leftChosen.Named("FIRST"), geneLeftChosenGroup.overriddenGenes[0].Named("SECOND")).CapitalizeFirst() + ((num > 1) ? (" +" + (num - 1).ToString()) : string.Empty);
				float x2 = Text.CalcSize(text).x;
				GUI.color = ColorLibrary.RedReadable;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x - x2 - 4f, rect.y, x2, rect.height), text);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600BBB8 RID: 48056 RVA: 0x0042B6F4 File Offset: 0x004298F4
		protected override bool CanAccept()
		{
			if (!base.CanAccept())
			{
				return false;
			}
			if (!this.selectedGenes.Any<GeneDef>())
			{
				Messages.Message("MessageNoSelectedGenes".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (GenFilePaths.AllCustomXenotypeFiles.EnumerableCount() >= 200)
			{
				Messages.Message("MessageTooManyCustomXenotypes", null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.ignoreRestrictions && this.leftChosenGroups.Any<GeneLeftChosenGroup>())
			{
				Messages.Message("MessageConflictingGenesPresent".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.generationRequestIndex >= 0)
			{
				PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(this.generationRequestIndex);
				if (TutorSystem.TutorialMode && generationRequest.MustBeCapableOfViolence)
				{
					if (this.selectedGenes.Any((GeneDef g) => g.disabledWorkTags.HasFlag(WorkTags.Violent)))
					{
						Messages.Message("MessageStartingPawnCapableOfViolence".Translate(), MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BBB9 RID: 48057 RVA: 0x0042B7FC File Offset: 0x004299FC
		protected override void Accept()
		{
			IEnumerable<string> warnings = this.GetWarnings();
			if (warnings.Any<string>())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("XenotypeBreaksLimits".Translate() + ":\n" + warnings.ToLineList("  - ", true) + "\n\n" + "SaveAnyway".Translate(), new Action(this.AcceptInner), false, null, WindowLayer.Dialog));
				return;
			}
			this.AcceptInner();
		}

		// Token: 0x0600BBBA RID: 48058 RVA: 0x0042B87C File Offset: 0x00429A7C
		private void AcceptInner()
		{
			CustomXenotype customXenotype = new CustomXenotype();
			CustomXenotype customXenotype2 = customXenotype;
			string xenotypeName = this.xenotypeName;
			customXenotype2.name = ((xenotypeName != null) ? xenotypeName.Trim() : null);
			customXenotype.genes.AddRange(this.selectedGenes);
			customXenotype.inheritable = this.inheritable;
			customXenotype.iconDef = this.iconDef;
			string xenotypeName2 = GenFile.SanitizedFileName(customXenotype.name);
			string absPath = GenFilePaths.AbsFilePathForXenotype(xenotypeName2);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveXenotype(customXenotype, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			if (this.generationRequestIndex >= 0)
			{
				PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(this.generationRequestIndex);
				generationRequest.ForcedXenotype = null;
				generationRequest.ForcedCustomXenotype = customXenotype;
				StartingPawnUtility.SetGenerationRequest(this.generationRequestIndex, generationRequest);
			}
			Action action = this.callback;
			if (action != null)
			{
				action();
			}
			this.Close(true);
		}

		// Token: 0x0600BBBB RID: 48059 RVA: 0x0042B975 File Offset: 0x00429B75
		private IEnumerable<string> GetWarnings()
		{
			Dialog_CreateXenotype.<GetWarnings>d__34 <GetWarnings>d__ = new Dialog_CreateXenotype.<GetWarnings>d__34(-2);
			<GetWarnings>d__.<>4__this = this;
			return <GetWarnings>d__;
		}

		// Token: 0x0600BBBC RID: 48060 RVA: 0x0042B988 File Offset: 0x00429B88
		protected override void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingGenes.Clear();
			this.matchingCategories.Clear();
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (GeneDef geneDef in GeneUtility.GenesInOrder)
			{
				if (!this.selectedGenes.Contains(geneDef))
				{
					if (this.quickSearchWidget.filter.Matches(geneDef.label))
					{
						this.matchingGenes.Add(geneDef);
					}
					if (this.quickSearchWidget.filter.Matches(geneDef.displayCategory.label))
					{
						this.matchingCategories.Add(geneDef.displayCategory);
					}
				}
			}
			this.quickSearchWidget.noResultsMatched = (!this.matchingGenes.Any<GeneDef>() && !this.matchingCategories.Any<GeneCategoryDef>());
		}

		// Token: 0x0600BBBE RID: 48062 RVA: 0x0042BAB4 File Offset: 0x00429CB4
		[CompilerGenerated]
		internal static string <GeneTip>g__GroupInfo|26_0(GeneLeftChosenGroup group)
		{
			if (group == null)
			{
				return null;
			}
			return ("GeneLeftmostActive".Translate() + ":\n  - " + group.leftChosen.LabelCap + " (" + "Active".Translate() + ")" + "\n" + (from x in @group.overriddenGenes
			select (x.label + " (" + "Suppressed".Translate() + ")").Colorize(ColorLibrary.RedReadable)).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x04006AEF RID: 27375
		private int generationRequestIndex;

		// Token: 0x04006AF0 RID: 27376
		private Action callback;

		// Token: 0x04006AF1 RID: 27377
		private List<GeneDef> selectedGenes = new List<GeneDef>();

		// Token: 0x04006AF2 RID: 27378
		private bool inheritable;

		// Token: 0x04006AF3 RID: 27379
		private bool? selectedCollapsed = new bool?(false);

		// Token: 0x04006AF4 RID: 27380
		private HashSet<GeneCategoryDef> matchingCategories = new HashSet<GeneCategoryDef>();

		// Token: 0x04006AF5 RID: 27381
		private Dictionary<GeneCategoryDef, bool> collapsedCategories = new Dictionary<GeneCategoryDef, bool>();

		// Token: 0x04006AF6 RID: 27382
		private bool hoveredAnyGene;

		// Token: 0x04006AF7 RID: 27383
		private GeneDef hoveredGene;

		// Token: 0x04006AF8 RID: 27384
		private static bool ignoreRestrictionsConfirmationSent;

		// Token: 0x04006AF9 RID: 27385
		private Vector2 selectedScrollPosition;

		// Token: 0x04006AFA RID: 27386
		private const int MaxCustomXenotypes = 200;

		// Token: 0x04006AFB RID: 27387
		private static readonly Color OutlineColorSelected = new Color(1f, 1f, 0.7f, 1f);
	}
}
