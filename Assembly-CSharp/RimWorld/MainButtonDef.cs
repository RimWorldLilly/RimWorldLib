using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9D RID: 3997
	public class MainButtonDef : Def
	{
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x060065DD RID: 26077 RVA: 0x00243C86 File Offset: 0x00241E86
		public MainButtonWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (MainButtonWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x060065DE RID: 26078 RVA: 0x00243CB8 File Offset: 0x00241EB8
		public MainTabWindow TabWindow
		{
			get
			{
				if (this.tabWindowInt == null && this.tabWindowClass != null)
				{
					this.tabWindowInt = (MainTabWindow)Activator.CreateInstance(this.tabWindowClass);
					this.tabWindowInt.def = this;
				}
				return this.tabWindowInt;
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x060065DF RID: 26079 RVA: 0x00243CF8 File Offset: 0x00241EF8
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = this.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060065E0 RID: 26080 RVA: 0x00243D2C File Offset: 0x00241F2C
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(this.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060065E1 RID: 26081 RVA: 0x00243D6C File Offset: 0x00241F6C
		public float ShortenedLabelCapWidth
		{
			get
			{
				if (this.cachedShortenedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedShortenedLabelCapWidth = Text.CalcSize(this.ShortenedLabelCap).x;
					Text.Font = font;
				}
				return this.cachedShortenedLabelCapWidth;
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x060065E2 RID: 26082 RVA: 0x00243DA7 File Offset: 0x00241FA7
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.iconPath != null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x00243DD7 File Offset: 0x00241FD7
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x00243DFA File Offset: 0x00241FFA
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x00243E1D File Offset: 0x0024201D
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		// Token: 0x04003D49 RID: 15689
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x04003D4A RID: 15690
		public Type tabWindowClass;

		// Token: 0x04003D4B RID: 15691
		public bool buttonVisible = true;

		// Token: 0x04003D4C RID: 15692
		public int order;

		// Token: 0x04003D4D RID: 15693
		public KeyCode defaultHotKey;

		// Token: 0x04003D4E RID: 15694
		public bool canBeTutorDenied = true;

		// Token: 0x04003D4F RID: 15695
		public bool validWithoutMap;

		// Token: 0x04003D50 RID: 15696
		public bool minimized;

		// Token: 0x04003D51 RID: 15697
		public string iconPath;

		// Token: 0x04003D52 RID: 15698
		public bool closesWorldView;

		// Token: 0x04003D53 RID: 15699
		public bool validWithClassicIdeo = true;

		// Token: 0x04003D54 RID: 15700
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		// Token: 0x04003D55 RID: 15701
		[Unsaved(false)]
		public string cachedTutorTag;

		// Token: 0x04003D56 RID: 15702
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		// Token: 0x04003D57 RID: 15703
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		// Token: 0x04003D58 RID: 15704
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		// Token: 0x04003D59 RID: 15705
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		// Token: 0x04003D5A RID: 15706
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x04003D5B RID: 15707
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x04003D5C RID: 15708
		[Unsaved(false)]
		private Texture2D icon;

		// Token: 0x04003D5D RID: 15709
		public const int ButtonHeight = 35;
	}
}
