using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCE RID: 7374
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x17002286 RID: 8838
		// (get) Token: 0x0600BDA9 RID: 48553 RVA: 0x00442247 File Offset: 0x00440447
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 700f);
			}
		}

		// Token: 0x17002287 RID: 8839
		// (get) Token: 0x0600BDAA RID: 48554 RVA: 0x00442258 File Offset: 0x00440458
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002288 RID: 8840
		// (get) Token: 0x0600BDAB RID: 48555 RVA: 0x0044225B File Offset: 0x0044045B
		protected virtual bool FocusSearchField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BDAC RID: 48556 RVA: 0x00442260 File Offset: 0x00440460
		public Dialog_FileList() : base(null)
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x0600BDAD RID: 48557 RVA: 0x004422E0 File Offset: 0x004404E0
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(inRect.width - 16f, 40f);
			float y = vector.y;
			float height = (float)this.FilesMatchingFilter() * y;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			Rect rect = inRect.LeftHalf();
			rect.height = 24f;
			this.search.OnGUI(rect, null, null);
			if (!this.focusedSearch && this.FocusSearchField)
			{
				this.focusedSearch = true;
				this.search.Focus();
			}
			Rect outRect = inRect;
			outRect.yMin = rect.yMax + 10f;
			outRect.yMax -= Window.CloseButSize.y + this.bottomAreaHeight + 10f;
			if (this.ShouldDoTypeInField)
			{
				outRect.yMax -= 53f;
			}
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			int num2 = 0;
			foreach (SaveFileInfo saveFileInfo in this.files)
			{
				if (this.search.filter.Matches(saveFileInfo.FileName))
				{
					if (num + vector.y >= this.scrollPosition.y && num <= this.scrollPosition.y + outRect.height)
					{
						Rect rect2 = new Rect(0f, num, vector.x, vector.y);
						if (num2 % 2 == 1)
						{
							Widgets.DrawAltRect(rect2);
						}
						Widgets.BeginGroup(rect2);
						Rect rect3 = new Rect(rect2.width - 36f, (rect2.height - 36f) / 2f, 36f, 36f);
						if (Widgets.ButtonImage(rect3, TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
						{
							FileInfo localFile = saveFileInfo.FileInfo;
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(localFile.Name), delegate()
							{
								localFile.Delete();
								this.ReloadFiles();
							}, true, null, WindowLayer.Dialog));
						}
						TooltipHandler.TipRegionByKey(rect3, this.deleteTipKey);
						Text.Font = GameFont.Small;
						Rect rect4 = new Rect(rect3.x - 100f, (rect2.height - 36f) / 2f, 100f, 36f);
						if (Widgets.ButtonText(rect4, this.interactButLabel, true, true, true, null))
						{
							this.DoFileInteraction(Path.GetFileNameWithoutExtension(saveFileInfo.FileName));
						}
						Rect rect5 = new Rect(rect4.x - 94f, 0f, 94f, rect2.height);
						Dialog_FileList.DrawDateAndVersion(saveFileInfo, rect5);
						GUI.color = Color.white;
						Text.Anchor = TextAnchor.UpperLeft;
						GUI.color = this.FileNameColor(saveFileInfo);
						Rect rect6 = new Rect(8f, 0f, rect5.x - 8f - 4f, rect2.height);
						Text.Anchor = TextAnchor.MiddleLeft;
						Text.Font = GameFont.Small;
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(saveFileInfo.FileName);
						Widgets.Label(rect6, fileNameWithoutExtension.Truncate(rect6.width * 1.8f, null));
						GUI.color = Color.white;
						Text.Anchor = TextAnchor.UpperLeft;
						Widgets.EndGroup();
					}
					num += vector.y;
					num2++;
				}
			}
			Widgets.EndScrollView();
			if (this.ShouldDoTypeInField)
			{
				this.DoTypeInField(inRect.TopPartPixels(inRect.height - Window.CloseButSize.y - 18f));
			}
		}

		// Token: 0x0600BDAE RID: 48558 RVA: 0x004426D4 File Offset: 0x004408D4
		private int FilesMatchingFilter()
		{
			int num = 0;
			for (int i = 0; i < this.files.Count; i++)
			{
				if (this.search.filter.Matches(this.files[i].FileName))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600BDAF RID: 48559
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x0600BDB0 RID: 48560
		protected abstract void ReloadFiles();

		// Token: 0x0600BDB1 RID: 48561 RVA: 0x00442724 File Offset: 0x00440924
		protected virtual void DoTypeInField(Rect rect)
		{
			Widgets.BeginGroup(rect);
			bool flag = Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Return;
			float y = rect.height - 35f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.SetNextControlName("MapNameField");
			string str = Widgets.TextField(new Rect(5f, y, 400f, 35f), this.typingName);
			if (GenText.IsValidFilename(str))
			{
				this.typingName = str;
			}
			if (!this.focusedNameArea)
			{
				UI.FocusControl("MapNameField", this);
				this.focusedNameArea = true;
			}
			if (Widgets.ButtonText(new Rect(420f, y, rect.width - 400f - 20f, 35f), "SaveGameButton".Translate(), true, true, true, null) || flag)
			{
				if (this.typingName.NullOrEmpty())
				{
					Messages.Message("NeedAName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					string text = this.typingName;
					this.DoFileInteraction((text != null) ? text.Trim() : null);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
		}

		// Token: 0x0600BDB2 RID: 48562 RVA: 0x00442857 File Offset: 0x00440A57
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x0600BDB3 RID: 48563 RVA: 0x00442860 File Offset: 0x00440A60
		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, rect.height / 2f);
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, rect.height / 2f);
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			if (Mouse.IsOver(rect3))
			{
				TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			}
			Widgets.EndGroup();
		}

		// Token: 0x04006C95 RID: 27797
		protected string interactButLabel = "Error";

		// Token: 0x04006C96 RID: 27798
		protected float bottomAreaHeight;

		// Token: 0x04006C97 RID: 27799
		protected readonly List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x04006C98 RID: 27800
		private readonly QuickSearchWidget search = new QuickSearchWidget();

		// Token: 0x04006C99 RID: 27801
		private bool focusedSearch;

		// Token: 0x04006C9A RID: 27802
		private bool focusedNameArea;

		// Token: 0x04006C9B RID: 27803
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006C9C RID: 27804
		protected string typingName = "";

		// Token: 0x04006C9D RID: 27805
		protected string deleteTipKey = "DeleteThisSavegame";

		// Token: 0x04006C9E RID: 27806
		protected const float EntryHeight = 40f;

		// Token: 0x04006C9F RID: 27807
		protected const float FileNameLeftMargin = 8f;

		// Token: 0x04006CA0 RID: 27808
		protected const float FileNameRightMargin = 4f;

		// Token: 0x04006CA1 RID: 27809
		protected const float FileInfoWidth = 94f;

		// Token: 0x04006CA2 RID: 27810
		protected const float InteractButWidth = 100f;

		// Token: 0x04006CA3 RID: 27811
		protected const float InteractButHeight = 36f;

		// Token: 0x04006CA4 RID: 27812
		protected const float DeleteButSize = 36f;

		// Token: 0x04006CA5 RID: 27813
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04006CA6 RID: 27814
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04006CA7 RID: 27815
		protected const float NameTextFieldButtonSpace = 20f;

		// Token: 0x04006CA8 RID: 27816
		protected static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);
	}
}
