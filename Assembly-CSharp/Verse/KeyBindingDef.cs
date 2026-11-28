using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000181 RID: 385
	public class KeyBindingDef : Def
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0003C998 File Offset: 0x0003AB98
		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.None)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != KeyCode.None)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0003C9D8 File Offset: 0x0003ABD8
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0003C9E8 File Offset: 0x0003ABE8
		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current.type == EventType.keyDown && Event.current.keyCode != KeyCode.None && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (keyBindingData.keyBindingA == KeyCode.LeftApple || keyBindingData.keyBindingA == KeyCode.RightApple || keyBindingData.keyBindingB == KeyCode.LeftApple || keyBindingData.keyBindingB == KeyCode.RightApple || !Event.current.command) && !Find.WindowStack.AnySearchWidgetFocused && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0003CAA0 File Offset: 0x0003ACA0
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftApple || keyBindingData.keyBindingA == KeyCode.RightApple || keyBindingData.keyBindingB == KeyCode.LeftApple || keyBindingData.keyBindingB == KeyCode.RightApple)) || this.IsDown);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0003CBF4 File Offset: 0x0003ADF4
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0003CC40 File Offset: 0x0003AE40
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003CC8B File Offset: 0x0003AE8B
		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return this.defaultKeyCodeA;
			}
			if (slot == KeyPrefs.BindingSlot.B)
			{
				return this.defaultKeyCodeB;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003CCA7 File Offset: 0x0003AEA7
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}

		// Token: 0x04000954 RID: 2388
		public KeyBindingCategoryDef category;

		// Token: 0x04000955 RID: 2389
		public KeyCode defaultKeyCodeA;

		// Token: 0x04000956 RID: 2390
		public KeyCode defaultKeyCodeB;

		// Token: 0x04000957 RID: 2391
		public bool devModeOnly;

		// Token: 0x04000958 RID: 2392
		public List<KeyBindingDef> ignoreConflictsWith;

		// Token: 0x04000959 RID: 2393
		[NoTranslate]
		public List<string> extraConflictTags;
	}
}
