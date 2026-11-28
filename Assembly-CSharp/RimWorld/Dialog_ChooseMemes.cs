using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C9C RID: 7324
	public class Dialog_ChooseMemes : Window
	{
		// Token: 0x17002204 RID: 8708
		// (get) Token: 0x0600BB41 RID: 47937 RVA: 0x00424505 File Offset: 0x00422705
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(980f, 724f);
			}
		}

		// Token: 0x17002205 RID: 8709
		// (get) Token: 0x0600BB42 RID: 47938 RVA: 0x00424516 File Offset: 0x00422716
		private bool ConfiguringNewFluidIdeo
		{
			get
			{
				return this.ideo.Fluid && !this.reformingIdeo;
			}
		}

		// Token: 0x17002206 RID: 8710
		// (get) Token: 0x0600BB43 RID: 47939 RVA: 0x00424530 File Offset: 0x00422730
		private bool ReformingFluidIdeo
		{
			get
			{
				return this.ideo.Fluid && this.reformingIdeo;
			}
		}

		// Token: 0x17002207 RID: 8711
		// (get) Token: 0x0600BB44 RID: 47940 RVA: 0x00424548 File Offset: 0x00422748
		private IntRange MemeCountRangeAbsolute
		{
			get
			{
				if (this.ConfiguringNewFluidIdeo)
				{
					return IdeoFoundation.MemeCountRangeFluidAbsolute;
				}
				if (this.ReformingFluidIdeo)
				{
					int num = (from m in this.ideo.memes
					where m.category == MemeCategory.Normal
					select m).Count<MemeDef>();
					return new IntRange(num - 1, Mathf.Min(num + 1, IdeoFoundation.MemeCountRangeAbsolute.max));
				}
				return IdeoFoundation.MemeCountRangeAbsolute;
			}
		}

		// Token: 0x17002208 RID: 8712
		// (get) Token: 0x0600BB45 RID: 47941 RVA: 0x004245C0 File Offset: 0x004227C0
		private int NormalMemesRemoveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.ideo.memes.Count; i++)
				{
					if (this.ideo.memes[i].category == MemeCategory.Normal && !this.newMemes.Contains(this.ideo.memes[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BB46 RID: 47942 RVA: 0x00424628 File Offset: 0x00422828
		public Dialog_ChooseMemes(Ideo ideo, MemeCategory memeCategory, bool initialSelection = false, Action done = null, List<MemeDef> preSelectedMemes = null, bool reformingIdeo = false) : base(null)
		{
			this.ideo = ideo;
			this.memeCategory = memeCategory;
			this.initialSelection = initialSelection;
			this.done = done;
			this.closeOnCancel = ideo.memes.Any((MemeDef m) => m.category == MemeCategory.Normal);
			this.reformingIdeo = reformingIdeo;
			this.absorbInputAroundWindow = true;
			this.newMemes.Clear();
			this.newMemes.AddRange(ideo.memes);
			if (preSelectedMemes != null)
			{
				foreach (MemeDef item in preSelectedMemes)
				{
					if (!this.newMemes.Contains(item))
					{
						this.newMemes.Add(item);
					}
				}
			}
		}

		// Token: 0x0600BB47 RID: 47943 RVA: 0x00424730 File Offset: 0x00422930
		private int RandomGeneratedMemeCount()
		{
			return GenMath.RoundRandom(this.MemeCountRangeAbsolute.Average);
		}

		// Token: 0x0600BB48 RID: 47944 RVA: 0x00424750 File Offset: 0x00422950
		public override void DoWindowContents(Rect rect)
		{
			Rect outRect = rect;
			outRect.height -= Dialog_ChooseMemes.ButSize.y;
			string label;
			if (this.memeCategory == MemeCategory.Structure)
			{
				label = "ChooseStructure".Translate();
			}
			else if (this.ConfiguringNewFluidIdeo)
			{
				label = "ChooseStartingMeme".Translate();
			}
			else
			{
				label = "ChooseMemes".Translate();
			}
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(outRect.x, outRect.y, rect.width, 30f), label);
			Text.Font = GameFont.Small;
			outRect.yMin += 30f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float b = 0f;
			if (this.memeCategory == MemeCategory.Structure)
			{
				this.DoMemeSelector(viewRect, MemeCategory.Structure, ref b);
			}
			else if (this.memeCategory == MemeCategory.Normal)
			{
				this.DoMemeSelector(viewRect, MemeCategory.Normal, ref b);
			}
			this.viewHeight = Mathf.Max(this.viewHeight, b);
			Widgets.EndScrollView();
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y), "Back".Translate(), true, true, true, null))
			{
				this.Close(true);
				if (this.memeCategory == MemeCategory.Structure)
				{
					this.NotifyConfigureIdeoPage();
				}
				else if (this.memeCategory == MemeCategory.Normal)
				{
					if (this.initialSelection)
					{
						Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Structure, this.initialSelection, null, null, false));
					}
					else
					{
						this.NotifyConfigureIdeoPage();
					}
				}
			}
			if (Widgets.ButtonText(new Rect((float)((int)(rect.width - Dialog_ChooseMemes.ButSize.x) / 2), rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y), "Randomize".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionDef forFaction = IdeoUIUtility.FactionForRandomization(this.ideo);
				if (this.memeCategory == MemeCategory.Normal)
				{
					if (this.ReformingFluidIdeo)
					{
						this.newMemes = IdeoUtility.RandomizeNormalMemesForReforming(this.MemeCountRangeAbsolute.max, this.ideo.memes, forFaction);
					}
					else
					{
						this.newMemes = IdeoUtility.RandomizeNormalMemes(this.RandomGeneratedMemeCount(), this.newMemes, forFaction, this.ConfiguringNewFluidIdeo);
					}
				}
				else if (this.memeCategory == MemeCategory.Structure)
				{
					this.newMemes = IdeoUtility.RandomizeStructureMeme(this.newMemes, forFaction);
				}
			}
			Rect rect2 = new Rect(rect.width - Dialog_ChooseMemes.ButSize.x, rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y);
			if (Widgets.ButtonText(rect2, "DoneButton".Translate(), true, true, true, null))
			{
				this.TryAccept();
			}
			string text = null;
			Pair<MemeDef, MemeDef> firstIncompatibleMemePair = this.GetFirstIncompatibleMemePair();
			if (this.GetMemeCount(MemeCategory.Structure) < 1 || (this.memeCategory == MemeCategory.Normal && (this.GetMemeCount(MemeCategory.Normal) < this.MemeCountRangeAbsolute.min || this.GetMemeCount(MemeCategory.Normal) > this.MemeCountRangeAbsolute.max)) || firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
			{
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleRight;
				GUI.color = Color.red;
				if (firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
				{
					text = "IncompatibleMemes".Translate(firstIncompatibleMemePair.First, firstIncompatibleMemePair.Second).CapitalizeFirst();
				}
				else if (this.memeCategory == MemeCategory.Normal)
				{
					if (this.GetMemeCount(MemeCategory.Normal) < this.MemeCountRangeAbsolute.min)
					{
						text = (this.ConfiguringNewFluidIdeo ? "NotEnoughMemesFluidIdeo".Translate(this.MemeCountRangeAbsolute.min) : "NotEnoughMemes".Translate(this.MemeCountRangeAbsolute.min));
					}
					else
					{
						text = "TooManyMemes".Translate(this.MemeCountRangeAbsolute.max);
					}
				}
				else
				{
					text = "ChooseStructureMeme".Translate();
				}
			}
			Rect rect3 = new Rect(rect.xMax - Dialog_ChooseMemes.ButSize.x - 240f - 6f, rect2.y, 240f, Dialog_ChooseMemes.ButSize.y);
			if (text != null)
			{
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				return;
			}
			if (this.memeCategory == MemeCategory.Normal)
			{
				IdeoUIUtility.DrawImpactInfo(rect3, this.newMemes);
			}
		}

		// Token: 0x0600BB49 RID: 47945 RVA: 0x00424C34 File Offset: 0x00422E34
		private void NotifyConfigureIdeoPage()
		{
			Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
			if (page_ConfigureIdeo != null)
			{
				page_ConfigureIdeo.Notify_ClosedChooseMemesDialog();
			}
		}

		// Token: 0x0600BB4A RID: 47946 RVA: 0x00424C58 File Offset: 0x00422E58
		private void TryAccept()
		{
			Pair<MemeDef, MemeDef> firstIncompatibleMemePair = this.GetFirstIncompatibleMemePair();
			if (firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
			{
				Messages.Message("MessageIncompatibleMemes".Translate(firstIncompatibleMemePair.First, firstIncompatibleMemePair.Second), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.memeCategory == MemeCategory.Normal)
			{
				if (this.GetMemeCount(MemeCategory.Normal) < this.MemeCountRangeAbsolute.min)
				{
					Messages.Message("MessageNotEnoughMemes".Translate(this.MemeCountRangeAbsolute.min), MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (this.GetMemeCount(MemeCategory.Normal) > this.MemeCountRangeAbsolute.max)
				{
					Messages.Message("MessageTooManyMemes".Translate(this.MemeCountRangeAbsolute.max), MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			else if (this.memeCategory == MemeCategory.Structure && this.GetMemeCount(MemeCategory.Structure) < 1)
			{
				Messages.Message("MessageNotEnoughStructureMemes".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.initialSelection || this.ideo.PreceptsListForReading.Count <= 0 || this.newMemes.SetsEqual(this.ideo.memes))
			{
				this.DoAcceptChanges();
				return;
			}
			if (this.ReformingFluidIdeo)
			{
				IdeoUIUtility.FactionForRandomization(this.ideo);
				this.tmpPreceptsToRemove.Clear();
				this.tmpPreceptsToRemove.AddRange(this.ideo.foundation.GetPreceptsToRemoveFromMemeChanges(this.ideo.memes, this.newMemes));
				if (this.tmpPreceptsToRemove.Count > 0)
				{
					TaggedString text = "ChangesRemovePrecepts".Translate((from x in this.tmpPreceptsToRemove
					where x.def.visible
					select x into p
					select p.def.issue.LabelCap.Resolve() + ": " + p.def.LabelCap.Resolve()).ToLineList("  - ", false));
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, new Action(this.DoAcceptChanges), false, null, WindowLayer.Dialog));
				}
				else
				{
					this.DoAcceptChanges();
				}
				this.tmpPreceptsToRemove.Clear();
				return;
			}
			TaggedString value = "Changing".Translate((this.memeCategory == MemeCategory.Normal) ? "MemesLower".Translate() : "StructuresLower".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ChangesRandomizePrecepts".Translate(value), new Action(this.DoAcceptChanges), false, null, WindowLayer.Dialog));
		}

		// Token: 0x0600BB4B RID: 47947 RVA: 0x00424F00 File Offset: 0x00423100
		private void DoAcceptChanges()
		{
			if (!this.newMemes.SetsEqual(this.ideo.memes))
			{
				FactionDef forFaction = IdeoUIUtility.FactionForRandomization(this.ideo);
				if (this.ReformingFluidIdeo)
				{
					this.tmpPreviousMemes.Clear();
					this.tmpPreviousMemes.AddRange(this.ideo.memes);
					this.ideo.memes.Clear();
					this.ideo.memes.AddRange(this.newMemes);
					this.ideo.SortMemesInDisplayOrder();
					this.ideo.foundation.EnsurePreceptsCompatibleWithMemes(this.tmpPreviousMemes, this.ideo.memes, new IdeoGenerationParms(forFaction, false, null, null, null, false, false, false, false, "", null, null, false, "", false));
					this.ideo.RecachePrecepts();
					this.tmpPreviousMemes.Clear();
				}
				else
				{
					this.ideo.memes.Clear();
					this.ideo.memes.AddRange(this.newMemes);
					this.ideo.SortMemesInDisplayOrder();
					if (this.ideo.PreceptsListForReading.Any<Precept>())
					{
						this.ideo.foundation.RandomizePrecepts(true, new IdeoGenerationParms(forFaction, false, null, null, null, false, false, false, false, "", null, null, false, "", false));
						IdeoFoundation_Deity ideoFoundation_Deity = this.ideo.foundation as IdeoFoundation_Deity;
						if (ideoFoundation_Deity != null && this.memeCategory == MemeCategory.Structure)
						{
							ideoFoundation_Deity.GenerateDeities();
						}
					}
					else
					{
						this.ideo.foundation.RandomizeCulture(new IdeoGenerationParms(forFaction, false, null, null, null, false, false, false, false, "", null, null, false, "", false));
						this.ideo.foundation.RandomizePlace();
						IdeoFoundation_Deity ideoFoundation_Deity2 = this.ideo.foundation as IdeoFoundation_Deity;
						if (ideoFoundation_Deity2 != null)
						{
							ideoFoundation_Deity2.GenerateDeities();
						}
						this.ideo.foundation.GenerateTextSymbols();
						this.ideo.foundation.RandomizePrecepts(false, new IdeoGenerationParms(forFaction, false, null, null, null, false, false, false, false, "", null, null, false, "", false));
						this.ideo.foundation.GenerateLeaderTitle();
						this.ideo.foundation.RandomizeIcon();
						this.ideo.foundation.InitPrecepts(new IdeoGenerationParms(forFaction, false, null, null, null, false, false, false, false, "", null, null, false, "", false), null);
						this.ideo.RecachePrecepts();
					}
					this.ideo.RegenerateDescription(true);
					this.ideo.foundation.RandomizeStyles();
					this.ideo.style.RecalculateAvailableStyleItems();
				}
			}
			if (this.memeCategory == MemeCategory.Structure)
			{
				if (!this.ideo.memes.Any((MemeDef x) => x.category == MemeCategory.Normal))
				{
					Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Normal, true, this.done, null, false));
					goto IL_2FF;
				}
			}
			if (this.done != null)
			{
				this.done();
			}
			IL_2FF:
			this.Close(true);
		}

		// Token: 0x0600BB4C RID: 47948 RVA: 0x00425213 File Offset: 0x00423413
		public override void OnAcceptKeyPressed()
		{
			this.TryAccept();
		}

		// Token: 0x0600BB4D RID: 47949 RVA: 0x0042521C File Offset: 0x0042341C
		private Pair<MemeDef, MemeDef> GetFirstIncompatibleMemePair()
		{
			for (int i = 0; i < this.newMemes.Count; i++)
			{
				for (int j = i + 1; j < this.newMemes.Count; j++)
				{
					for (int k = 0; k < this.newMemes[i].exclusionTags.Count; k++)
					{
						if (this.newMemes[j].exclusionTags.Contains(this.newMemes[i].exclusionTags[k]))
						{
							return new Pair<MemeDef, MemeDef>(this.newMemes[i], this.newMemes[j]);
						}
					}
				}
			}
			return default(Pair<MemeDef, MemeDef>);
		}

		// Token: 0x0600BB4E RID: 47950 RVA: 0x004252D4 File Offset: 0x004234D4
		private void DoMemeSelector(Rect viewRect, MemeCategory category, ref float curY)
		{
			curY += 17f;
			string text;
			if (category == MemeCategory.Structure)
			{
				text = "ChooseStructureMemesInfo".Translate();
			}
			else if (this.ConfiguringNewFluidIdeo)
			{
				text = "ChooseNormalMemesFluidIdeoInfo".Translate(this.MemeCountRangeAbsolute.min);
			}
			else
			{
				if (this.ReformingFluidIdeo)
				{
					text = "ChooseOrRemoveMeme".Translate();
				}
				else
				{
					text = "ChooseNormalMemesInfo".Translate(this.MemeCountRangeAbsolute.min, this.MemeCountRangeAbsolute.max);
				}
				text += " " + "SomeMemesHaveMoreImpact".Translate();
			}
			Widgets.Label(viewRect.x, ref curY, viewRect.width, text, default(TipSignal));
			curY += 27f;
			Dialog_ChooseMemes.memesInCategory.Clear();
			foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefsListForReading)
			{
				if (memeDef.category == category && this.CanUseMeme(memeDef))
				{
					Dialog_ChooseMemes.memesInCategory.Add(memeDef);
				}
			}
			if (category == MemeCategory.Structure)
			{
				this.DoStructureMemeSelector(viewRect, ref curY, Dialog_ChooseMemes.memesInCategory);
				return;
			}
			this.DoNormalMemeSelector(viewRect, ref curY, Dialog_ChooseMemes.memesInCategory);
		}

		// Token: 0x0600BB4F RID: 47951 RVA: 0x00425444 File Offset: 0x00423644
		private void DoStructureMemeSelector(Rect viewRect, ref float curY, List<MemeDef> memes)
		{
			memes.SortBy((MemeDef x) => x.groupDef != null, (MemeDef x) => x.renderOrder);
			float num = curY;
			int num2 = Mathf.FloorToInt(viewRect.width / (IdeoUIUtility.MemeBoxSize.x + 8f));
			float num3 = 8f;
			int num4 = memes.Count((MemeDef x) => x.groupDef == null);
			if (num4 < num2)
			{
				num3 = (viewRect.width - IdeoUIUtility.MemeBoxSize.x * (float)(num4 + 1)) / (float)num4;
			}
			Dialog_ChooseMemes.memeGroups.Clear();
			for (int i = 0; i < memes.Count; i++)
			{
				MemeDef memeDef = memes[i];
				if (memeDef.groupDef != null)
				{
					if (Dialog_ChooseMemes.memeGroups.ContainsKey(memeDef.groupDef))
					{
						Dialog_ChooseMemes.memeGroups[memeDef.groupDef].Add(memeDef);
					}
					else
					{
						Dialog_ChooseMemes.memeGroups.Add(memeDef.groupDef, new List<MemeDef>
						{
							memeDef
						});
					}
				}
				else
				{
					int num5 = i / num2;
					int num6 = i % num2;
					int num7 = (i >= num4 - num4 % num2) ? (num4 % num2) : num2;
					float num8 = (viewRect.width - (float)num7 * IdeoUIUtility.MemeBoxSize.x - (float)(num7 - 1) * num3) / 2f;
					Rect memeBox = new Rect(num8 + (float)num6 * IdeoUIUtility.MemeBoxSize.x + (float)num6 * num3, curY + (float)num5 * IdeoUIUtility.MemeBoxSize.y + (float)num5 * num3, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y);
					this.DrawMeme(memeDef, memeBox, false);
					num = Mathf.Max(num, memeBox.yMax);
				}
			}
			if (Dialog_ChooseMemes.memeGroups.Any<KeyValuePair<MemeGroupDef, List<MemeDef>>>())
			{
				if (num4 > 0)
				{
					num += num3;
				}
				float num9 = num;
				foreach (KeyValuePair<MemeGroupDef, List<MemeDef>> keyValuePair in Dialog_ChooseMemes.memeGroups)
				{
					num = Mathf.Max(num, this.DrawMemeGroup(keyValuePair.Value, new Vector2(viewRect.x + viewRect.width * keyValuePair.Key.drawOffset.x, viewRect.y + num9 + viewRect.height * keyValuePair.Key.drawOffset.y), keyValuePair.Key.maxRows).yMax);
				}
			}
			Dialog_ChooseMemes.memeGroups.Clear();
			GUI.color = Color.white;
			curY = num;
		}

		// Token: 0x0600BB50 RID: 47952 RVA: 0x00425718 File Offset: 0x00423918
		private void DoNormalMemeSelector(Rect viewRect, ref float curY, List<MemeDef> memes)
		{
			Dialog_ChooseMemes.<>c__DisplayClass39_0 CS$<>8__locals1;
			CS$<>8__locals1.viewRect = viewRect;
			CS$<>8__locals1.<>4__this = this;
			memes.Sort(Dialog_ChooseMemes.NormalMemeSorter);
			CS$<>8__locals1.gapBetweenBoxes = 8f;
			int num = Mathf.FloorToInt(CS$<>8__locals1.viewRect.width / (IdeoUIUtility.MemeBoxSize.x + CS$<>8__locals1.gapBetweenBoxes));
			int impact;
			Func<MemeDef, bool> <>9__1;
			int impact2;
			for (impact = 1; impact <= 3; impact = impact2 + 1)
			{
				int num2 = 0;
				using (List<MemeDef>.Enumerator enumerator = memes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.impact == impact)
						{
							num2++;
						}
					}
				}
				if (num2 != 0)
				{
					int num3 = (num2 <= num) ? num : Mathf.Min(Mathf.CeilToInt((float)num2 / 2f), num);
					Rect rect = new Rect(CS$<>8__locals1.viewRect.x, curY, CS$<>8__locals1.viewRect.width, 30f);
					Widgets.Label(rect, "IdeoImpact".Translate() + ": " + IdeoImpactUtility.MemeImpactLabel(impact).CapitalizeFirst());
					curY = rect.yMax;
					GUI.color = Color.gray;
					Widgets.DrawLineHorizontal(rect.x, curY - 7f, rect.width);
					GUI.color = Color.white;
					Dialog_ChooseMemes.memesCurrentRow.Clear();
					Func<MemeDef, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((MemeDef x) => x.impact == impact));
					}
					foreach (IGrouping<MemeGroupDef, MemeDef> collection in from x in memes.Where(predicate)
					group x by x.groupDef)
					{
						Dialog_ChooseMemes.memesInGroup.Clear();
						Dialog_ChooseMemes.memesInGroup.AddRange(collection);
						if (Dialog_ChooseMemes.memesCurrentRow.Count + Dialog_ChooseMemes.memesInGroup.Count > num3)
						{
							this.<DoNormalMemeSelector>g__DrawRow|39_0(ref curY, ref CS$<>8__locals1);
						}
						foreach (MemeDef item in Dialog_ChooseMemes.memesInGroup)
						{
							if (Dialog_ChooseMemes.memesCurrentRow.Count >= num3)
							{
								this.<DoNormalMemeSelector>g__DrawRow|39_0(ref curY, ref CS$<>8__locals1);
							}
							Dialog_ChooseMemes.memesCurrentRow.Add(item);
						}
					}
					this.<DoNormalMemeSelector>g__DrawRow|39_0(ref curY, ref CS$<>8__locals1);
				}
				impact2 = impact;
			}
		}

		// Token: 0x0600BB51 RID: 47953 RVA: 0x004259D0 File Offset: 0x00423BD0
		private Rect DrawMemeGroup(List<MemeDef> memes, Vector2 offset, int maxRows)
		{
			int num = Mathf.CeilToInt((float)memes.Count / (float)maxRows);
			int num2 = Mathf.CeilToInt((float)memes.Count / (float)num);
			Rect rect = new Rect(offset.x, offset.y, (float)num * IdeoUIUtility.MemeBoxSize.x, (float)num2 * IdeoUIUtility.MemeBoxSize.y);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawHighlight(rect);
			GUI.color = Color.white;
			bool flag = memes.Count % 2 != 0 && maxRows > 1;
			for (int i = 0; i < memes.Count; i++)
			{
				int num3 = i;
				if (flag && (float)i / (float)num > 0f)
				{
					num3++;
				}
				float num4 = offset.x + (float)(num3 % num) * IdeoUIUtility.MemeBoxSize.x;
				if (num3 / num == 0 && flag)
				{
					num4 += IdeoUIUtility.MemeBoxSize.x / 2f;
				}
				this.DrawMeme(memes[i], new Rect(num4, offset.y + (float)(num3 / num) * IdeoUIUtility.MemeBoxSize.y, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y), false);
			}
			return rect;
		}

		// Token: 0x0600BB52 RID: 47954 RVA: 0x00425B1C File Offset: 0x00423D1C
		private void DrawMeme(MemeDef meme, Rect memeBox, bool drawHighlight = true)
		{
			if (this.newMemes.Contains(meme))
			{
				if (!this.CanRemoveMeme(meme).Accepted)
				{
					GUI.color = (this.ReformingFluidIdeo ? Color.gray : Color.red);
				}
				Widgets.DrawBox(memeBox, 1, null);
				GUI.color = Color.white;
			}
			if (!drawHighlight)
			{
				Widgets.DrawLightHighlight(memeBox);
			}
			IdeoUIUtility.DoMeme(memeBox, meme, null, IdeoEditMode.None, drawHighlight, null);
			if (Widgets.ButtonInvisible(memeBox, true))
			{
				if (this.newMemes.Contains(meme))
				{
					if (meme.category != MemeCategory.Structure)
					{
						AcceptanceReport acceptanceReport = this.CanRemoveMeme(meme);
						if (acceptanceReport.Accepted)
						{
							this.newMemes.Remove(meme);
							SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
							return;
						}
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
						return;
					}
				}
				else
				{
					if (meme.category == MemeCategory.Structure)
					{
						for (int i = this.newMemes.Count - 1; i >= 0; i--)
						{
							if (this.newMemes[i].category == MemeCategory.Structure)
							{
								this.newMemes.RemoveAt(i);
							}
						}
					}
					else if (this.ConfiguringNewFluidIdeo)
					{
						this.newMemes.RemoveAll((MemeDef m) => m.category == MemeCategory.Normal);
					}
					else if (this.ReformingFluidIdeo)
					{
						if (this.NormalMemesRemoveCount >= 1 && !this.ideo.memes.Contains(meme))
						{
							return;
						}
						this.newMemes.RemoveAll((MemeDef m) => !this.ideo.memes.Contains(m));
					}
					this.newMemes.Add(meme);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x0600BB53 RID: 47955 RVA: 0x00425CB8 File Offset: 0x00423EB8
		private bool CanUseMeme(MemeDef meme)
		{
			if (meme.hiddenInChooseMemes)
			{
				return false;
			}
			if (IdeoUIUtility.DevEditMode)
			{
				return true;
			}
			if (this.ConfiguringNewFluidIdeo && this.memeCategory == MemeCategory.Normal && !IdeoUtility.IsMemeAllowedForInitialFluidIdeo(meme))
			{
				return false;
			}
			if (Current.Game.World == null)
			{
				return IdeoUtility.IsMemeAllowedFor(meme, Find.Scenario.playerFaction.factionDef);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.def.hidden && !faction.def.isPlayer && faction.ideos != null && (faction.ideos.IsPrimary(this.ideo) || faction.ideos.IsMinor(this.ideo)) && !IdeoUtility.IsMemeAllowedFor(meme, faction.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BB54 RID: 47956 RVA: 0x00425DB0 File Offset: 0x00423FB0
		private AcceptanceReport CanRemoveMeme(MemeDef meme)
		{
			if (IdeoUIUtility.DevEditMode)
			{
				return true;
			}
			if (this.ReformingFluidIdeo)
			{
				if (this.NormalMemesRemoveCount >= 1)
				{
					return false;
				}
				if (this.newMemes.Count > this.ideo.memes.Count && this.ideo.memes.Contains(meme))
				{
					return false;
				}
				if (this.ideo.memes.Count((MemeDef m) => m.category == MemeCategory.Normal) <= 1 && this.ideo.memes.Contains(meme))
				{
					return false;
				}
			}
			if (Current.Game.World == null)
			{
				FactionDef factionDef = Find.Scenario.playerFaction.factionDef;
				if (factionDef.requiredMemes != null && factionDef.requiredMemes.Contains(meme))
				{
					return "CannotRemoveMemeRequiredPlayer".Translate(meme.label.Named("MEME"));
				}
			}
			else
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (!faction.def.hidden && !faction.def.isPlayer && faction.ideos != null && (faction.ideos.IsPrimary(this.ideo) || faction.ideos.IsMinor(this.ideo)) && faction.def.requiredMemes != null && faction.def.requiredMemes.Contains(meme))
					{
						return "CannotRemoveMemeRequired".Translate(meme.label.Named("MEME")) + ": " + "RequiredByFaction".Translate(faction.Named("FACTION"));
					}
				}
			}
			return true;
		}

		// Token: 0x0600BB55 RID: 47957 RVA: 0x00425FC4 File Offset: 0x004241C4
		private int GetMemeCount(MemeCategory category)
		{
			int num = 0;
			for (int i = 0; i < this.newMemes.Count; i++)
			{
				if (this.newMemes[i].category == category)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600BB57 RID: 47959 RVA: 0x00426084 File Offset: 0x00424284
		[CompilerGenerated]
		private void <DoNormalMemeSelector>g__DrawRow|39_0(ref float y, ref Dialog_ChooseMemes.<>c__DisplayClass39_0 A_2)
		{
			if (Dialog_ChooseMemes.memesCurrentRow.Count == 0)
			{
				return;
			}
			float num = (A_2.viewRect.width - (float)Dialog_ChooseMemes.memesCurrentRow.Count * (IdeoUIUtility.MemeBoxSize.x + A_2.gapBetweenBoxes)) / 2f;
			MemeGroupDef memeGroupDef = null;
			for (int i = 0; i < Dialog_ChooseMemes.memesCurrentRow.Count; i++)
			{
				MemeDef memeDef = Dialog_ChooseMemes.memesCurrentRow[i];
				if (i == 0)
				{
					memeGroupDef = memeDef.groupDef;
				}
				else if (memeGroupDef != memeDef.groupDef || (memeDef.groupDef != null && memeDef.groupDef.renderWithGap))
				{
					memeGroupDef = memeDef.groupDef;
					num += A_2.gapBetweenBoxes;
				}
				Rect memeBox = new Rect(A_2.viewRect.x + num, y, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y).Rounded();
				this.DrawMeme(memeDef, memeBox, false);
				num = memeBox.xMax;
			}
			Dialog_ChooseMemes.memesCurrentRow.Clear();
			y += A_2.gapBetweenBoxes + IdeoUIUtility.MemeBoxSize.y;
		}

		// Token: 0x04006A88 RID: 27272
		private Ideo ideo;

		// Token: 0x04006A89 RID: 27273
		private Vector2 scrollPos;

		// Token: 0x04006A8A RID: 27274
		private float viewHeight;

		// Token: 0x04006A8B RID: 27275
		private List<MemeDef> newMemes = new List<MemeDef>();

		// Token: 0x04006A8C RID: 27276
		private bool initialSelection;

		// Token: 0x04006A8D RID: 27277
		private Action done;

		// Token: 0x04006A8E RID: 27278
		private MemeCategory memeCategory;

		// Token: 0x04006A8F RID: 27279
		private bool reformingIdeo;

		// Token: 0x04006A90 RID: 27280
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006A91 RID: 27281
		private const float Width = 980f;

		// Token: 0x04006A92 RID: 27282
		private const float Height = 724f;

		// Token: 0x04006A93 RID: 27283
		private const int MaxMemesToRemoveForFluidIdeo = 1;

		// Token: 0x04006A94 RID: 27284
		private List<Precept> tmpPreceptsToRemove = new List<Precept>();

		// Token: 0x04006A95 RID: 27285
		private List<MemeDef> tmpPreviousMemes = new List<MemeDef>();

		// Token: 0x04006A96 RID: 27286
		private static List<MemeDef> memesInCategory = new List<MemeDef>();

		// Token: 0x04006A97 RID: 27287
		private static Dictionary<MemeGroupDef, List<MemeDef>> memeGroups = new Dictionary<MemeGroupDef, List<MemeDef>>();

		// Token: 0x04006A98 RID: 27288
		private static readonly List<MemeDef> memesCurrentRow = new List<MemeDef>();

		// Token: 0x04006A99 RID: 27289
		private static readonly List<MemeDef> memesInGroup = new List<MemeDef>();

		// Token: 0x04006A9A RID: 27290
		private static readonly IComparer<MemeDef> NormalMemeSorter = GenCollection.CompareBy<MemeDef, int>(delegate(MemeDef x)
		{
			MemeGroupDef groupDef = x.groupDef;
			if (groupDef == null)
			{
				return int.MaxValue;
			}
			return groupDef.renderOrder;
		}).ThenBy(GenCollection.CompareBy<MemeDef, int>((MemeDef x) => x.renderOrder));
	}
}
