using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000661 RID: 1633
	public class Dialog_MessageBox : Window
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x0014D55D File Offset: 0x0014B75D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x0014D56E File Offset: 0x0014B76E
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x0014D583 File Offset: 0x0014B783
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x0014D598 File Offset: 0x0014B798
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false, string title = null, WindowLayer layer = WindowLayer.Dialog)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive, confirmedAct, delegate()
			{
			}, layer);
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x0014D5F0 File Offset: 0x0014B7F0
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, Action cancelAct, bool destructive = false, string title = null, WindowLayer layer = WindowLayer.Dialog)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), cancelAct, title, destructive, confirmedAct, cancelAct, layer);
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x0014D62C File Offset: 0x0014B82C
		public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null, WindowLayer layer = WindowLayer.Dialog) : base(null)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			this.layer = layer;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x0014D720 File Offset: 0x0014B920
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			if (this.image != null)
			{
				float num2 = (float)this.image.width / (float)this.image.height;
				float num3 = 270f * num2;
				GUI.DrawTexture(new Rect(inRect.x + (inRect.width - num3) / 2f, num, num3, 270f), this.image);
				num += 280f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num4 = this.buttonCText.NullOrEmpty() ? 2 : 3;
			float num5 = inRect.width / (float)num4;
			float width2 = num5 - 10f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = this.InteractionDelayExpired ? this.buttonAText : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
			if (Widgets.ButtonText(new Rect(num5 * (float)(num4 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true, null) && this.InteractionDelayExpired)
			{
				if (this.buttonAAction != null)
				{
					this.buttonAAction();
				}
				this.Close(true);
			}
			GUI.color = Color.white;
			if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true, null))
			{
				if (this.buttonBAction != null)
				{
					this.buttonBAction();
				}
				this.Close(true);
			}
			if (this.buttonCText != null && Widgets.ButtonText(new Rect(num5, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true, null))
			{
				if (this.buttonCAction != null)
				{
					this.buttonCAction();
				}
				if (this.buttonCClose)
				{
					this.Close(true);
				}
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x0014DA27 File Offset: 0x0014BC27
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
				Event.current.Use();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x0014DA54 File Offset: 0x0014BC54
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
				Event.current.Use();
				return;
			}
			base.OnAcceptKeyPressed();
		}

		// Token: 0x040021A0 RID: 8608
		public TaggedString text;

		// Token: 0x040021A1 RID: 8609
		public string title;

		// Token: 0x040021A2 RID: 8610
		public string buttonAText;

		// Token: 0x040021A3 RID: 8611
		public Action buttonAAction;

		// Token: 0x040021A4 RID: 8612
		public bool buttonADestructive;

		// Token: 0x040021A5 RID: 8613
		public string buttonBText;

		// Token: 0x040021A6 RID: 8614
		public Action buttonBAction;

		// Token: 0x040021A7 RID: 8615
		public string buttonCText;

		// Token: 0x040021A8 RID: 8616
		public Action buttonCAction;

		// Token: 0x040021A9 RID: 8617
		public bool buttonCClose = true;

		// Token: 0x040021AA RID: 8618
		public float interactionDelay;

		// Token: 0x040021AB RID: 8619
		public Action acceptAction;

		// Token: 0x040021AC RID: 8620
		public Action cancelAction;

		// Token: 0x040021AD RID: 8621
		public Texture2D image;

		// Token: 0x040021AE RID: 8622
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040021AF RID: 8623
		private float creationRealTime = -1f;

		// Token: 0x040021B0 RID: 8624
		private const float TitleHeight = 42f;

		// Token: 0x040021B1 RID: 8625
		protected const float ButtonHeight = 35f;
	}
}
