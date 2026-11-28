using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CA1 RID: 7329
	public class Dialog_CreateXenogerm : GeneCreationDialogBase
	{
		// Token: 0x1700221A RID: 8730
		// (get) Token: 0x0600BB94 RID: 48020 RVA: 0x00429085 File Offset: 0x00427285
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1016f, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700221B RID: 8731
		// (get) Token: 0x0600BB95 RID: 48021 RVA: 0x00429097 File Offset: 0x00427297
		protected override string Header
		{
			get
			{
				return "AssembleGenes".Translate();
			}
		}

		// Token: 0x1700221C RID: 8732
		// (get) Token: 0x0600BB96 RID: 48022 RVA: 0x004290A8 File Offset: 0x004272A8
		protected override string AcceptButtonLabel
		{
			get
			{
				return "StartCombining".Translate();
			}
		}

		// Token: 0x1700221D RID: 8733
		// (get) Token: 0x0600BB97 RID: 48023 RVA: 0x004290BC File Offset: 0x004272BC
		protected override List<GeneDef> SelectedGenes
		{
			get
			{
				this.tmpGenes.Clear();
				foreach (Genepack genepack in this.selectedGenepacks)
				{
					foreach (GeneDef item in genepack.GeneSet.GenesListForReading)
					{
						this.tmpGenes.Add(item);
					}
				}
				return this.tmpGenes;
			}
		}

		// Token: 0x0600BB98 RID: 48024 RVA: 0x00429164 File Offset: 0x00427364
		public Dialog_CreateXenogerm(Building_GeneAssembler geneAssembler)
		{
			this.geneAssembler = geneAssembler;
			this.maxGCX = geneAssembler.MaxComplexity();
			this.libraryGenepacks.AddRange(geneAssembler.GetGenepacks(true, true));
			this.unpoweredGenepacks.AddRange(geneAssembler.GetGenepacks(false, true));
			this.xenotypeName = string.Empty;
			this.closeOnAccept = false;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.searchWidgetOffsetX = GeneCreationDialogBase.ButSize.x * 2f + 4f;
			this.libraryGenepacks.SortGenepacks();
			this.unpoweredGenepacks.SortGenepacks();
		}

		// Token: 0x0600BB99 RID: 48025 RVA: 0x00429258 File Offset: 0x00427458
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("gene assembly"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600BB9A RID: 48026 RVA: 0x00429274 File Offset: 0x00427474
		protected override void Accept()
		{
			if (this.geneAssembler.Working)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmStartNewXenogerm".Translate(this.geneAssembler.xenotypeName.Named("XENOGERMNAME")), new Action(this.StartAssembly), true, null, WindowLayer.Dialog));
				return;
			}
			this.StartAssembly();
		}

		// Token: 0x0600BB9B RID: 48027 RVA: 0x004292D4 File Offset: 0x004274D4
		private void StartAssembly()
		{
			Building_GeneAssembler building_GeneAssembler = this.geneAssembler;
			List<Genepack> packs = this.selectedGenepacks;
			int arc = this.arc;
			string xenotypeName = this.xenotypeName;
			building_GeneAssembler.Start(packs, arc, (xenotypeName != null) ? xenotypeName.Trim() : null, this.iconDef);
			SoundDefOf.StartRecombining.PlayOneShotOnCamera(null);
			this.Close(false);
		}

		// Token: 0x0600BB9C RID: 48028 RVA: 0x00429324 File Offset: 0x00427524
		protected override void DrawGenes(Rect rect)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect.AtZero(), ref this.scrollPosition, rect2, true);
			Rect containingRect = rect2;
			containingRect.y = this.scrollPosition.y;
			containingRect.height = rect.height;
			this.DrawSection(rect, this.selectedGenepacks, "SelectedGenepacks".Translate(), ref num, ref this.selectedHeight, false, containingRect);
			num += 8f;
			this.DrawSection(rect, this.libraryGenepacks, "GenepackLibrary".Translate(), ref num, ref this.unselectedHeight, true, containingRect);
			if (Event.current.type == EventType.layout)
			{
				this.scrollHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x0600BB9D RID: 48029 RVA: 0x00429408 File Offset: 0x00427608
		private void DrawSection(Rect rect, List<Genepack> genepacks, string label, ref float curY, ref float sectionHeight, bool adding, Rect containingRect)
		{
			float num = 4f;
			Rect rect2 = new Rect(10f, curY, rect.width - 16f - 10f, Text.LineHeight);
			Widgets.Label(rect2, label);
			if (!adding)
			{
				Text.Anchor = TextAnchor.UpperRight;
				GUI.color = ColoredText.SubtleGrayColor;
				Widgets.Label(rect2, "ClickToAddOrRemove".Translate());
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			curY += Text.LineHeight + 3f;
			float num2 = curY;
			Rect rect3 = new Rect(0f, curY, rect.width, sectionHeight);
			Widgets.DrawRectFast(rect3, Widgets.MenuSectionBGFillColor, null);
			curY += 4f;
			if (!genepacks.Any<Genepack>())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = ColoredText.SubtleGrayColor;
				Widgets.Label(rect3, "(" + "NoneLower".Translate() + ")");
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else
			{
				for (int i = 0; i < genepacks.Count; i++)
				{
					Genepack genepack = genepacks[i];
					if (!this.quickSearchWidget.filter.Active || (this.matchingGenepacks.Contains(genepack) && (!adding || !this.selectedGenepacks.Contains(genepack))))
					{
						float num3 = 34f + GeneCreationDialogBase.GeneSize.x * (float)genepack.GeneSet.GenesListForReading.Count + 4f * (float)(genepack.GeneSet.GenesListForReading.Count + 2);
						if (num + num3 > rect.width - 16f)
						{
							num = 4f;
							curY += GeneCreationDialogBase.GeneSize.y + 8f + 14f;
						}
						if (adding && this.selectedGenepacks.Contains(genepack))
						{
							Widgets.DrawLightHighlight(new Rect(num, curY, num3, GeneCreationDialogBase.GeneSize.y + 8f));
							num += num3 + 14f;
						}
						else if (this.DrawGenepack(genepack, ref num, curY, num3, containingRect))
						{
							if (adding)
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								this.selectedGenepacks.Add(genepack);
							}
							else
							{
								SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
								this.selectedGenepacks.Remove(genepack);
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
			curY += GeneCreationDialogBase.GeneSize.y + 12f;
			if (Event.current.type == EventType.layout)
			{
				sectionHeight = curY - num2;
			}
		}

		// Token: 0x0600BB9E RID: 48030 RVA: 0x004296C4 File Offset: 0x004278C4
		private bool DrawGenepack(Genepack genepack, ref float curX, float curY, float packWidth, Rect containingRect)
		{
			bool result = false;
			if (genepack.GeneSet == null || genepack.GeneSet.GenesListForReading.NullOrEmpty<GeneDef>())
			{
				return result;
			}
			Rect rect = new Rect(curX, curY, packWidth, GeneCreationDialogBase.GeneSize.y + 8f);
			if (!containingRect.Overlaps(rect))
			{
				curX = rect.xMax + 14f;
				return false;
			}
			Widgets.DrawHighlight(rect);
			GUI.color = GeneCreationDialogBase.OutlineColorUnselected;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
			curX += 4f;
			GeneUIUtility.DrawBiostats(genepack.GeneSet.ComplexityTotal, genepack.GeneSet.MetabolismTotal, genepack.GeneSet.ArchitesTotal, ref curX, curY, 4f);
			List<GeneDef> genesListForReading = genepack.GeneSet.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				GeneDef gene = genesListForReading[i];
				if (this.quickSearchWidget.filter.Active && this.matchingGenes.Contains(gene))
				{
					this.matchingGenepacks.Contains(genepack);
				}
				bool overridden = this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(gene));
				Rect geneRect = new Rect(curX, curY + 4f, GeneCreationDialogBase.GeneSize.x, GeneCreationDialogBase.GeneSize.y);
				string extraTooltip = null;
				if (this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.leftChosen == gene))
				{
					extraTooltip = Dialog_CreateXenogerm.<DrawGenepack>g__GroupInfo|21_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == gene));
				}
				else if (this.cachedOverriddenGenes.Contains(gene))
				{
					extraTooltip = Dialog_CreateXenogerm.<DrawGenepack>g__GroupInfo|21_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(gene)));
				}
				else if (this.randomChosenGroups.ContainsKey(gene))
				{
					extraTooltip = ("GeneWillBeRandomChosen".Translate() + ":\n" + (from x in this.randomChosenGroups[gene]
					select x.label).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
				}
				GeneUIUtility.DrawGeneDef(genesListForReading[i], geneRect, GeneType.Xenogene, () => extraTooltip, false, false, overridden);
				curX += GeneCreationDialogBase.GeneSize.x + 4f;
			}
			Widgets.InfoCardButton(rect.xMax - 24f, rect.y + 2f, genepack);
			if (this.unpoweredGenepacks.Contains(genepack))
			{
				Widgets.DrawBoxSolid(rect, this.UnpoweredColor);
				TooltipHandler.TipRegion(rect, "GenepackUnusableGenebankUnpowered".Translate().Colorize(ColorLibrary.RedReadable));
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect) && Event.current.button == 1)
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("EjectGenepackFromGeneBank".Translate(), delegate()
				{
					CompGenepackContainer geneBankHoldingPack = this.geneAssembler.GetGeneBankHoldingPack(genepack);
					if (geneBankHoldingPack != null)
					{
						ThingWithComps parent = geneBankHoldingPack.parent;
						Thing thing;
						if (geneBankHoldingPack.innerContainer.TryDrop(genepack, parent.def.hasInteractionCell ? parent.InteractionCell : parent.Position, parent.Map, ThingPlaceMode.Near, 1, out thing, null, null))
						{
							if (this.selectedGenepacks.Contains(genepack))
							{
								this.selectedGenepacks.Remove(genepack);
							}
							this.tmpGenes.Clear();
							this.libraryGenepacks.Clear();
							this.unpoweredGenepacks.Clear();
							this.matchingGenepacks.Clear();
							this.libraryGenepacks.AddRange(this.geneAssembler.GetGenepacks(true, true));
							this.unpoweredGenepacks.AddRange(this.geneAssembler.GetGenepacks(false, true));
							this.libraryGenepacks.SortGenepacks();
							this.unpoweredGenepacks.SortGenepacks();
							this.OnGenesChanged();
						}
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Find.WindowStack.Add(new FloatMenu(list));
			}
			else if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
			}
			curX = Mathf.Max(curX, rect.xMax + 14f);
			return result;
		}

		// Token: 0x0600BB9F RID: 48031 RVA: 0x00429AA4 File Offset: 0x00427CA4
		protected override void DrawSearchRect(Rect rect)
		{
			base.DrawSearchRect(rect);
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "LoadXenogermTemplate".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_XenogermList_Load(delegate(CustomXenogerm xenogerm)
				{
					this.xenotypeName = xenogerm.name;
					this.xenotypeNameLocked = true;
					this.iconDef = xenogerm.iconDef;
					IEnumerable<Genepack> collection = CustomXenogermUtility.GetMatchingGenepacks(xenogerm.genesets, this.libraryGenepacks);
					this.selectedGenepacks.Clear();
					this.selectedGenepacks.AddRange(collection);
					this.OnGenesChanged();
					IEnumerable<GeneSet> source = from gp in xenogerm.genesets
					where !this.selectedGenepacks.Any((Genepack g) => g.GeneSet.Matches(gp))
					select gp;
					if (source.Any<GeneSet>())
					{
						int num = source.Count<GeneSet>();
						string text;
						if (num == 1)
						{
							text = "MissingGenepackForXenogerm".Translate(xenogerm.name.Named("NAME"));
							text = text + ": " + (from g in source
							select g.Label).ToCommaList(false, false).CapitalizeFirst();
						}
						else
						{
							text = "MissingGenepacksForXenogerm".Translate(num.Named("COUNT"), xenogerm.name.Named("NAME"));
						}
						Messages.Message(text, null, MessageTypeDefOf.CautionInput, false);
					}
				}));
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x * 2f - 4f, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "SaveXenogermTemplate".Translate(), true, true, true, null))
			{
				AcceptanceReport acceptanceReport = CustomXenogermUtility.SaveXenogermTemplate(this.xenotypeName, this.iconDef, this.selectedGenepacks);
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		// Token: 0x0600BBA0 RID: 48032 RVA: 0x00429BC0 File Offset: 0x00427DC0
		protected override void DoBottomButtons(Rect rect)
		{
			base.DoBottomButtons(rect);
			if (!this.selectedGenepacks.Any<Genepack>())
			{
				return;
			}
			int numTicks = Mathf.RoundToInt((float)Mathf.RoundToInt(GeneTuning.ComplexityToCreationHoursCurve.Evaluate((float)this.gcx) * 2500f) / this.geneAssembler.GetStatValue(StatDefOf.AssemblySpeedFactor, true, -1));
			Rect rect2 = new Rect(rect.center.x, rect.y, rect.width / 2f - GeneCreationDialogBase.ButSize.x - 10f, GeneCreationDialogBase.ButSize.y);
			TaggedString label;
			TaggedString str;
			if (this.arc > 0 && !ResearchProjectDefOf.Archogenetics.IsFinished)
			{
				label = ("MissingRequiredResearch".Translate() + ": " + ResearchProjectDefOf.Archogenetics.LabelCap).Colorize(ColorLibrary.RedReadable);
				str = "MustResearchProject".Translate(ResearchProjectDefOf.Archogenetics);
			}
			else
			{
				label = "RecombineDuration".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true, false);
				str = "RecombineDurationDesc".Translate();
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, str);
			}
		}

		// Token: 0x0600BBA1 RID: 48033 RVA: 0x00429D18 File Offset: 0x00427F18
		protected override bool CanAccept()
		{
			if (!base.CanAccept())
			{
				return false;
			}
			if (!this.selectedGenepacks.Any<Genepack>())
			{
				Messages.Message("MessageNoSelectedGenepacks".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.arc > 0 && !ResearchProjectDefOf.Archogenetics.IsFinished)
			{
				Messages.Message("AssemblingRequiresResearch".Translate(ResearchProjectDefOf.Archogenetics), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.gcx > this.maxGCX)
			{
				Messages.Message("ComplexityTooHighToCreateXenogerm".Translate(this.gcx.Named("AMOUNT"), this.maxGCX.Named("MAX")), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.ColonyHasEnoughArchites())
			{
				Messages.Message("NotEnoughArchites".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600BBA2 RID: 48034 RVA: 0x00429E10 File Offset: 0x00428010
		private bool ColonyHasEnoughArchites()
		{
			if (this.arc == 0 || this.geneAssembler.MapHeld == null)
			{
				return true;
			}
			List<Thing> list = this.geneAssembler.MapHeld.listerThings.ThingsOfDef(ThingDefOf.ArchiteCapsule);
			int num = 0;
			foreach (Thing thing in list)
			{
				if (!thing.Position.Fogged(this.geneAssembler.MapHeld))
				{
					num += thing.stackCount;
					if (num >= this.arc)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BBA3 RID: 48035 RVA: 0x00429EBC File Offset: 0x004280BC
		protected override void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingGenepacks.Clear();
			this.matchingGenes.Clear();
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (Genepack genepack in this.selectedGenepacks)
			{
				List<GeneDef> genesListForReading = genepack.GeneSet.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (this.quickSearchWidget.filter.Matches(genesListForReading[i].label))
					{
						this.matchingGenepacks.Add(genepack);
						this.matchingGenes.Add(genesListForReading[i]);
					}
				}
			}
			foreach (Genepack genepack2 in this.libraryGenepacks)
			{
				if (!this.selectedGenepacks.Contains(genepack2))
				{
					List<GeneDef> genesListForReading2 = genepack2.GeneSet.GenesListForReading;
					for (int j = 0; j < genesListForReading2.Count; j++)
					{
						if (this.quickSearchWidget.filter.Matches(genesListForReading2[j].label))
						{
							this.matchingGenepacks.Add(genepack2);
							this.matchingGenes.Add(genesListForReading2[j]);
						}
					}
				}
			}
			this.quickSearchWidget.noResultsMatched = !this.matchingGenepacks.Any<Genepack>();
		}

		// Token: 0x0600BBA4 RID: 48036 RVA: 0x0042A068 File Offset: 0x00428268
		[CompilerGenerated]
		internal static string <DrawGenepack>g__GroupInfo|21_0(GeneLeftChosenGroup group)
		{
			if (group == null)
			{
				return null;
			}
			return ("GeneOneActive".Translate() + ":\n  - " + group.leftChosen.LabelCap + " (" + "Active".Translate() + ")" + "\n" + (from x in @group.overriddenGenes
			select (x.label + " (" + "Suppressed".Translate() + ")").Colorize(ColorLibrary.RedReadable)).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x04006AE7 RID: 27367
		private Building_GeneAssembler geneAssembler;

		// Token: 0x04006AE8 RID: 27368
		private List<Genepack> libraryGenepacks = new List<Genepack>();

		// Token: 0x04006AE9 RID: 27369
		private List<Genepack> unpoweredGenepacks = new List<Genepack>();

		// Token: 0x04006AEA RID: 27370
		private List<Genepack> selectedGenepacks = new List<Genepack>();

		// Token: 0x04006AEB RID: 27371
		private HashSet<Genepack> matchingGenepacks = new HashSet<Genepack>();

		// Token: 0x04006AEC RID: 27372
		private readonly Color UnpoweredColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		// Token: 0x04006AED RID: 27373
		private List<GeneDef> tmpGenes = new List<GeneDef>();
	}
}
