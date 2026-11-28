using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CFC RID: 7420
	public abstract class Page : Window
	{
		// Token: 0x170022D6 RID: 8918
		// (get) Token: 0x0600BF76 RID: 49014 RVA: 0x00457E04 File Offset: 0x00456004
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x170022D7 RID: 8919
		// (get) Token: 0x0600BF77 RID: 49015 RVA: 0x00457E0B File Offset: 0x0045600B
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600BF78 RID: 49016 RVA: 0x00457E0E File Offset: 0x0045600E
		public Page() : base(null)
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x0600BF79 RID: 49017 RVA: 0x00457E3A File Offset: 0x0045603A
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600BF7A RID: 49018 RVA: 0x00457E70 File Offset: 0x00456070
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x0600BF7B RID: 49019 RVA: 0x00457EB8 File Offset: 0x004560B8
		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true, bool doNextOnKeypress = true)
		{
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			if ((Widgets.ButtonText(new Rect(rect.x, y, Page.BottomButSize.x, Page.BottomButSize.y), label, true, true, true, null) || KeyBindingDefOf.Cancel.KeyDownEvent) && this.CanDoBack())
			{
				this.DoBack();
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				Rect rect2 = new Rect(rect.x + rect.width - Page.BottomButSize.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if ((Widgets.ButtonText(rect2, nextLabel, true, true, true, null) || (doNextOnKeypress && KeyBindingDefOf.Accept.KeyDownEvent)) && this.CanDoNext())
				{
					this.DoNext();
				}
				UIHighlighter.HighlightOpportunity(rect2, "NextPage");
			}
			if (midAct != null && Widgets.ButtonText(new Rect(rect.x + rect.width / 2f - Page.BottomButSize.x / 2f, y, Page.BottomButSize.x, Page.BottomButSize.y), midLabel, true, true, true, null))
			{
				midAct();
			}
		}

		// Token: 0x0600BF7C RID: 49020 RVA: 0x00458030 File Offset: 0x00456230
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x0600BF7D RID: 49021 RVA: 0x0045804A File Offset: 0x0045624A
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x0600BF7E RID: 49022 RVA: 0x00458064 File Offset: 0x00456264
		protected virtual void DoNext()
		{
			if (this.next != null)
			{
				Find.WindowStack.Add(this.next);
			}
			if (this.nextAct != null)
			{
				this.nextAct();
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToNextPage");
			this.Close(true);
		}

		// Token: 0x0600BF7F RID: 49023 RVA: 0x004580C1 File Offset: 0x004562C1
		protected virtual void DoBack()
		{
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToPrevPage");
			if (this.prev != null)
			{
				Find.WindowStack.Add(this.prev);
			}
			this.Close(true);
		}

		// Token: 0x0600BF80 RID: 49024 RVA: 0x00458100 File Offset: 0x00456300
		public override void OnCancelKeyPressed()
		{
			if (!this.closeOnCancel)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoBack())
			{
				this.DoBack();
			}
			else
			{
				this.Close(true);
			}
			Event.current.Use();
			base.OnCancelKeyPressed();
		}

		// Token: 0x0600BF81 RID: 49025 RVA: 0x00458151 File Offset: 0x00456351
		public override void OnAcceptKeyPressed()
		{
			if (!this.closeOnAccept)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoNext())
			{
				this.DoNext();
			}
			Event.current.Use();
		}

		// Token: 0x04006E18 RID: 28184
		public Page prev;

		// Token: 0x04006E19 RID: 28185
		public Page next;

		// Token: 0x04006E1A RID: 28186
		public Action nextAct;

		// Token: 0x04006E1B RID: 28187
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x04006E1C RID: 28188
		public const float TitleAreaHeight = 45f;

		// Token: 0x04006E1D RID: 28189
		public const float BottomButHeight = 38f;

		// Token: 0x04006E1E RID: 28190
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
