using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CDF RID: 7391
	[StaticConstructorOnStartup]
	public abstract class GeneCreationDialogBase : Window
	{
		// Token: 0x17002290 RID: 8848
		// (get) Token: 0x0600BDE6 RID: 48614
		protected abstract List<GeneDef> SelectedGenes { get; }

		// Token: 0x17002291 RID: 8849
		// (get) Token: 0x0600BDE7 RID: 48615
		protected abstract string Header { get; }

		// Token: 0x17002292 RID: 8850
		// (get) Token: 0x0600BDE8 RID: 48616
		protected abstract string AcceptButtonLabel { get; }

		// Token: 0x0600BDE9 RID: 48617 RVA: 0x004438F5 File Offset: 0x00441AF5
		public override void PreOpen()
		{
			base.PreOpen();
			this.iconDef = XenotypeIconDefOf.Basic;
			this.UpdateSearchResults();
			this.OnGenesChanged();
		}

		// Token: 0x0600BDEA RID: 48618 RVA: 0x00443914 File Offset: 0x00441B14
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMax -= GeneCreationDialogBase.ButSize.y + 4f;
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, 35f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect3, this.Header);
			Text.Font = GameFont.Small;
			this.DrawSearchRect(rect);
			rect2.yMin += 39f;
			float num = rect.width * 0.25f - this.Margin - 10f;
			float num2 = num - 24f - 10f;
			float num3 = Mathf.Max(BiostatsTable.HeightForBiostats(this.alwaysUseFullBiostatsTableHeight ? 1 : this.arc), this.postXenotypeHeight);
			Rect rect4 = new Rect(rect2.x + this.Margin, rect2.y, rect2.width - this.Margin * 2f, rect2.height - num3 - 8f);
			this.DrawGenes(rect4);
			float num4 = rect4.yMax + 4f;
			Rect rect5 = new Rect(rect2.x + this.Margin + 10f, num4, rect.width * 0.75f - this.Margin * 3f - 10f, num3)
			{
				yMax = rect4.yMax + num3 + 4f
			};
			BiostatsTable.Draw(rect5, this.gcx, this.met, this.arc, true, this.ignoreRestrictions, this.maxGCX);
			string text = "XenotypeName".Translate().CapitalizeFirst() + ":";
			Rect rect6 = new Rect(rect5.xMax + this.Margin, num4, Text.CalcSize(text).x, Text.LineHeight);
			Widgets.Label(rect6, text);
			Rect rect7 = new Rect(rect6.xMin, rect6.y + Text.LineHeight, num, Text.LineHeight);
			rect7.xMax = rect2.xMax - this.Margin - 17f - num2 * 0.25f;
			string text2 = this.xenotypeName;
			this.xenotypeName = Widgets.TextField(rect7, this.xenotypeName, 40, GeneCreationDialogBase.ValidSymbolRegex);
			if (text2 != this.xenotypeName)
			{
				if (this.xenotypeName.Length > text2.Length && this.xenotypeName.Length > 3)
				{
					this.xenotypeNameLocked = true;
				}
				else if (this.xenotypeName.Length == 0)
				{
					this.xenotypeNameLocked = false;
				}
			}
			Rect rect8 = new Rect(rect7.xMax + 4f, rect7.yMax - 35f, 35f, 35f);
			this.DrawIconSelector(rect8);
			Rect rect9 = new Rect(rect7.x, rect7.yMax + 4f, num2 * 0.75f - 4f, 24f);
			if (Widgets.ButtonText(rect9, "Randomize".Translate(), true, true, true, null))
			{
				if (this.SelectedGenes.Count == 0)
				{
					Messages.Message("SelectAGeneToRandomizeName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					GUI.FocusControl(null);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.xenotypeName = GeneUtility.GenerateXenotypeNameFromGenes(this.SelectedGenes);
				}
			}
			Rect rect10 = new Rect(rect9.xMax + 4f, rect9.y, num2 * 0.25f, 24f);
			if (Widgets.ButtonText(rect10, "...", true, true, true, null))
			{
				if (this.SelectedGenes.Count > 0)
				{
					List<string> list = new List<string>();
					int num5 = 0;
					while (list.Count < 20)
					{
						string text3 = GeneUtility.GenerateXenotypeNameFromGenes(this.SelectedGenes);
						if (text3.NullOrEmpty())
						{
							break;
						}
						if (list.Contains(text3) || text3 == this.xenotypeName)
						{
							num5++;
							if (num5 >= 1000)
							{
								break;
							}
						}
						else
						{
							list.Add(text3);
						}
					}
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					for (int i = 0; i < list.Count; i++)
					{
						string n = list[i];
						list2.Add(new FloatMenuOption(n, delegate()
						{
							this.xenotypeName = n;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (list2.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list2));
					}
				}
				else
				{
					Messages.Message("SelectAGeneToChooseAName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
			}
			Rect rect11 = new Rect(rect10.xMax + 10f, rect9.y, 24f, 24f);
			if (Widgets.ButtonImage(rect11, this.xenotypeNameLocked ? GeneCreationDialogBase.LockedTex : GeneCreationDialogBase.UnlockedTex, true, null))
			{
				this.xenotypeNameLocked = !this.xenotypeNameLocked;
				if (this.xenotypeNameLocked)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			if (Mouse.IsOver(rect11))
			{
				string str = "LockNameButtonDesc".Translate() + "\n\n" + (this.xenotypeNameLocked ? "LockNameOn" : "LockNameOff").Translate();
				TooltipHandler.TipRegion(rect11, str);
			}
			this.postXenotypeHeight = rect11.yMax - num4;
			this.PostXenotypeOnGUI(rect6.xMin, rect9.y + 24f);
			Rect rect12 = rect;
			rect12.yMin = rect12.yMax - GeneCreationDialogBase.ButSize.y;
			this.DoBottomButtons(rect12);
		}

		// Token: 0x0600BDEB RID: 48619 RVA: 0x00443EEC File Offset: 0x004420EC
		protected virtual void DrawSearchRect(Rect rect)
		{
			Rect rect2 = new Rect(rect.width - 300f - this.searchWidgetOffsetX, 11f, 300f, 24f);
			this.quickSearchWidget.OnGUI(rect2, new Action(this.UpdateSearchResults), null);
		}

		// Token: 0x0600BDEC RID: 48620 RVA: 0x00443F40 File Offset: 0x00442140
		protected virtual bool WithinAcceptableBiostatLimits(bool showMessage)
		{
			if (this.ignoreRestrictions)
			{
				return true;
			}
			if (this.met < GeneTuning.BiostatRange.TrueMin)
			{
				if (showMessage)
				{
					Messages.Message("MetabolismTooLowToCreateXenogerm".Translate(this.met.Named("AMOUNT"), GeneTuning.BiostatRange.TrueMin.Named("MIN")), null, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BDED RID: 48621 RVA: 0x00443FC0 File Offset: 0x004421C0
		protected virtual bool CanAccept()
		{
			string text = this.xenotypeName;
			if (text != null && text.Trim().Length == 0)
			{
				Messages.Message("XenotypeNameCannotBeEmpty".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.WithinAcceptableBiostatLimits(true))
			{
				return false;
			}
			List<GeneDef> selectedGenes = this.SelectedGenes;
			foreach (GeneDef geneDef in this.SelectedGenes)
			{
				if (geneDef.prerequisite != null && !selectedGenes.Contains(geneDef.prerequisite))
				{
					Messages.Message("MessageGeneMissingPrerequisite".Translate(geneDef.label).CapitalizeFirst() + ": " + geneDef.prerequisite.LabelCap, null, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BDEE RID: 48622 RVA: 0x004440C0 File Offset: 0x004422C0
		private void DrawIconSelector(Rect rect)
		{
			Widgets.DrawHighlight(rect);
			if (Widgets.ButtonImage(rect, this.iconDef.Icon, XenotypeDef.IconColor, true, null))
			{
				Find.WindowStack.Add(new Dialog_SelectXenotypeIcon(this.iconDef, delegate(XenotypeIconDef i)
				{
					this.iconDef = i;
				}));
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "SelectIconDesc".Translate() + "\n\n" + "ClickToEdit".Translate().Colorize(ColoredText.SubtleGrayColor));
			}
		}

		// Token: 0x0600BDEF RID: 48623 RVA: 0x00444154 File Offset: 0x00442354
		protected virtual void PostXenotypeOnGUI(float curX, float curY)
		{
		}

		// Token: 0x0600BDF0 RID: 48624
		protected abstract void Accept();

		// Token: 0x0600BDF1 RID: 48625
		protected abstract void DrawGenes(Rect rect);

		// Token: 0x0600BDF2 RID: 48626 RVA: 0x00444158 File Offset: 0x00442358
		protected virtual void OnGenesChanged()
		{
			this.randomChosenGroups.Clear();
			this.leftChosenGroups.Clear();
			this.cachedOverriddenGenes.Clear();
			this.cachedUnoverriddenGenes.Clear();
			this.tmpGenesWithType.Clear();
			this.gcx = 0;
			this.met = 0;
			this.arc = 0;
			List<GeneDef> selectedGenes = this.SelectedGenes;
			for (int i = 0; i < selectedGenes.Count; i++)
			{
				if (selectedGenes[i].RandomChosen)
				{
					for (int j = i + 1; j < selectedGenes.Count; j++)
					{
						if (selectedGenes[i].ConflictsWith(selectedGenes[j]))
						{
							if (!this.randomChosenGroups.ContainsKey(selectedGenes[i]))
							{
								this.randomChosenGroups.Add(selectedGenes[i], new List<GeneDef>
								{
									selectedGenes[i]
								});
							}
							this.randomChosenGroups[selectedGenes[i]].Add(selectedGenes[j]);
						}
					}
				}
			}
			for (int k = 0; k < selectedGenes.Count; k++)
			{
				if (!selectedGenes[k].RandomChosen)
				{
					for (int l = k + 1; l < selectedGenes.Count; l++)
					{
						if (!selectedGenes[l].RandomChosen && selectedGenes[k].ConflictsWith(selectedGenes[l]))
						{
							int num = GeneUtility.GenesInOrder.IndexOf(selectedGenes[k]);
							int num2 = GeneUtility.GenesInOrder.IndexOf(selectedGenes[l]);
							GeneDef leftMost = (num < num2) ? selectedGenes[k] : selectedGenes[l];
							GeneDef rightMost = (num >= num2) ? selectedGenes[k] : selectedGenes[l];
							GeneLeftChosenGroup geneLeftChosenGroup = this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == leftMost);
							GeneLeftChosenGroup geneLeftChosenGroup2 = this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == rightMost);
							if (geneLeftChosenGroup == null)
							{
								geneLeftChosenGroup = new GeneLeftChosenGroup(leftMost);
								this.leftChosenGroups.Add(geneLeftChosenGroup);
							}
							if (geneLeftChosenGroup2 != null)
							{
								foreach (GeneDef item in geneLeftChosenGroup2.overriddenGenes)
								{
									if (!geneLeftChosenGroup.overriddenGenes.Contains(item))
									{
										geneLeftChosenGroup.overriddenGenes.Add(item);
									}
									if (!this.cachedOverriddenGenes.Contains(item))
									{
										this.cachedOverriddenGenes.Add(item);
									}
								}
								this.leftChosenGroups.Remove(geneLeftChosenGroup2);
							}
							if (!geneLeftChosenGroup.overriddenGenes.Contains(rightMost))
							{
								geneLeftChosenGroup.overriddenGenes.Add(rightMost);
							}
							if (!this.cachedOverriddenGenes.Contains(rightMost))
							{
								this.cachedOverriddenGenes.Add(rightMost);
							}
						}
					}
				}
			}
			Func<GeneDef, int> <>9__2;
			foreach (GeneLeftChosenGroup geneLeftChosenGroup3 in this.leftChosenGroups)
			{
				List<GeneDef> overriddenGenes = geneLeftChosenGroup3.overriddenGenes;
				Func<GeneDef, int> selector;
				if ((selector = <>9__2) == null)
				{
					selector = (<>9__2 = ((GeneDef x) => selectedGenes.IndexOf(x)));
				}
				overriddenGenes.SortBy(selector);
			}
			this.cachedUnoverriddenGenes.AddRange(this.SelectedGenes);
			foreach (GeneDef item2 in this.cachedOverriddenGenes)
			{
				this.cachedUnoverriddenGenes.Remove(item2);
			}
			for (int m = 0; m < selectedGenes.Count; m++)
			{
				this.tmpGenesWithType.Add(new GeneDefWithType(selectedGenes[m], true));
			}
			foreach (GeneDef geneDef in this.tmpGenesWithType.NonOverriddenGenes().Distinct<GeneDef>())
			{
				this.gcx += geneDef.biostatCpx;
				this.met += geneDef.biostatMet;
				this.arc += geneDef.biostatArc;
			}
		}

		// Token: 0x0600BDF3 RID: 48627
		protected abstract void UpdateSearchResults();

		// Token: 0x0600BDF4 RID: 48628 RVA: 0x00444664 File Offset: 0x00442864
		protected virtual void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), this.AcceptButtonLabel, true, true, true, null) && this.CanAccept())
			{
				this.Accept();
			}
			if (Widgets.ButtonText(new Rect(rect.x, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600BDF5 RID: 48629 RVA: 0x0044471C File Offset: 0x0044291C
		protected GeneCreationDialogBase() : base(null)
		{
		}

		// Token: 0x04006CBD RID: 27837
		protected int gcx;

		// Token: 0x04006CBE RID: 27838
		protected int met;

		// Token: 0x04006CBF RID: 27839
		protected int arc;

		// Token: 0x04006CC0 RID: 27840
		protected string xenotypeName;

		// Token: 0x04006CC1 RID: 27841
		protected bool xenotypeNameLocked;

		// Token: 0x04006CC2 RID: 27842
		protected float scrollHeight;

		// Token: 0x04006CC3 RID: 27843
		protected Vector2 scrollPosition;

		// Token: 0x04006CC4 RID: 27844
		protected float selectedHeight;

		// Token: 0x04006CC5 RID: 27845
		protected float unselectedHeight;

		// Token: 0x04006CC6 RID: 27846
		protected bool ignoreRestrictions;

		// Token: 0x04006CC7 RID: 27847
		protected float postXenotypeHeight;

		// Token: 0x04006CC8 RID: 27848
		protected bool alwaysUseFullBiostatsTableHeight;

		// Token: 0x04006CC9 RID: 27849
		protected int maxGCX = -1;

		// Token: 0x04006CCA RID: 27850
		protected float searchWidgetOffsetX;

		// Token: 0x04006CCB RID: 27851
		protected QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04006CCC RID: 27852
		protected HashSet<GeneDef> matchingGenes = new HashSet<GeneDef>();

		// Token: 0x04006CCD RID: 27853
		protected Dictionary<GeneDef, List<GeneDef>> randomChosenGroups = new Dictionary<GeneDef, List<GeneDef>>();

		// Token: 0x04006CCE RID: 27854
		protected List<GeneLeftChosenGroup> leftChosenGroups = new List<GeneLeftChosenGroup>();

		// Token: 0x04006CCF RID: 27855
		protected List<GeneDef> cachedOverriddenGenes = new List<GeneDef>();

		// Token: 0x04006CD0 RID: 27856
		protected List<GeneDef> cachedUnoverriddenGenes = new List<GeneDef>();

		// Token: 0x04006CD1 RID: 27857
		protected List<GeneDefWithType> tmpGenesWithType = new List<GeneDefWithType>();

		// Token: 0x04006CD2 RID: 27858
		protected XenotypeIconDef iconDef;

		// Token: 0x04006CD3 RID: 27859
		protected static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006CD4 RID: 27860
		protected const float HeaderHeight = 35f;

		// Token: 0x04006CD5 RID: 27861
		protected const float GeneGap = 4f;

		// Token: 0x04006CD6 RID: 27862
		private const int MaxNameLength = 40;

		// Token: 0x04006CD7 RID: 27863
		private const int NumCharsTypedBeforeAutoLockingName = 3;

		// Token: 0x04006CD8 RID: 27864
		private const int MaxTriesToGenerateUniqueXenotypeNames = 1000;

		// Token: 0x04006CD9 RID: 27865
		private const float TextFieldWidthPct = 0.25f;

		// Token: 0x04006CDA RID: 27866
		private static readonly Regex ValidSymbolRegex = new Regex("^[\\p{L}0-9 '\\-]*$");

		// Token: 0x04006CDB RID: 27867
		public static readonly Texture2D UnlockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/LockedMonochrome", true);

		// Token: 0x04006CDC RID: 27868
		public static readonly Texture2D LockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/Locked", true);

		// Token: 0x04006CDD RID: 27869
		protected const float BiostatsWidth = 38f;

		// Token: 0x04006CDE RID: 27870
		public static readonly Vector2 GeneSize = new Vector2(87f, 68f);

		// Token: 0x04006CDF RID: 27871
		protected static readonly Color OutlineColorUnselected = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x04006CE0 RID: 27872
		protected const float GenepackGap = 14f;

		// Token: 0x04006CE1 RID: 27873
		protected const float QuickSearchFilterWidth = 300f;
	}
}
