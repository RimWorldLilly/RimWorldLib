using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200066F RID: 1647
	public abstract class Dialog_Rename<T> : Window where T : class, IRenameable
	{
		// Token: 0x06003412 RID: 13330 RVA: 0x0014FF3C File Offset: 0x0014E13C
		protected Dialog_Rename(T renaming) : base(null)
		{
			this.renaming = renaming;
			T t = renaming;
			this.curName = ((t != null) ? t.RenamableLabel : null);
			this.doCloseX = true;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x0014FF92 File Offset: 0x0014E192
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x0014FFA4 File Offset: 0x0014E1A4
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x0014FFA8 File Offset: 0x0014E1A8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x0014FFB9 File Offset: 0x0014E1B9
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x0014FFC8 File Offset: 0x0014E1C8
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			return name.Length != 0;
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x0014FFD8 File Offset: 0x0014E1D8
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.keyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect = new Rect(inRect);
			Text.Font = GameFont.Medium;
			rect.height = Text.LineHeight + 10f;
			Widgets.Label(rect, "Rename".Translate());
			Text.Font = GameFont.Small;
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, rect.height, inRect.width, 35f), this.curName);
			if (this.AcceptsInput && text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			else if (!this.AcceptsInput)
			{
				((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 10f, inRect.width - 15f - 15f, 35f), "OK", true, true, true, null) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					return;
				}
				else
				{
					if (this.renaming != null)
					{
						this.renaming.RenamableLabel = this.curName;
					}
					this.OnRenamed(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x001501CE File Offset: 0x0014E3CE
		protected virtual void OnRenamed(string name)
		{
		}

		// Token: 0x0400220E RID: 8718
		protected string curName;

		// Token: 0x0400220F RID: 8719
		private bool focusedRenameField;

		// Token: 0x04002210 RID: 8720
		private int startAcceptingInputAtFrame;

		// Token: 0x04002211 RID: 8721
		protected readonly T renaming;
	}
}
