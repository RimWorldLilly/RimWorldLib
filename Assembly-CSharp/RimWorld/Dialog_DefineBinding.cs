using System;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001CB4 RID: 7348
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x1700224B RID: 8779
		// (get) Token: 0x0600BC81 RID: 48257 RVA: 0x00436723 File Offset: 0x00434923
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x1700224C RID: 8780
		// (get) Token: 0x0600BC82 RID: 48258 RVA: 0x0043672B File Offset: 0x0043492B
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600BC83 RID: 48259 RVA: 0x00436734 File Offset: 0x00434934
		public Dialog_DefineBinding(KeyPrefsData keyPrefsData, KeyBindingDef keyDef, KeyPrefs.BindingSlot slot) : base(null)
		{
			this.keyDef = keyDef;
			this.slot = slot;
			this.keyPrefsData = keyPrefsData;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC84 RID: 48260 RVA: 0x00436798 File Offset: 0x00434998
		public override void DoWindowContents(Rect inRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				Widgets.Label(inRect, "PressAnyKeyOrEscController".Translate().Resolve().AdjustedForKeys(null, true));
			}
			else
			{
				Widgets.Label(inRect, "PressAnyKeyOrEsc".Translate());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.isKey && Event.current.type == EventType.keyDown && Event.current.keyCode != KeyCode.None)
			{
				if (Event.current.keyCode != KeyCode.Escape)
				{
					this.keyPrefsData.EraseConflictingBindingsForKeyCode(this.keyDef, Event.current.keyCode, delegate(KeyBindingDef oldDef)
					{
						Messages.Message("KeyBindingOverwritten".Translate(oldDef.LabelCap), MessageTypeDefOf.TaskCompletion, false);
					});
					this.keyPrefsData.SetBinding(this.keyDef, this.slot, Event.current.keyCode);
				}
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x04006BC4 RID: 27588
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04006BC5 RID: 27589
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04006BC6 RID: 27590
		protected KeyBindingDef keyDef;

		// Token: 0x04006BC7 RID: 27591
		protected KeyPrefs.BindingSlot slot;
	}
}
