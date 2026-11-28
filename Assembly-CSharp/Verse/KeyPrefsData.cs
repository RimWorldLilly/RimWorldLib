using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000701 RID: 1793
	public class KeyPrefsData
	{
		// Token: 0x060038D7 RID: 14551 RVA: 0x0016CD64 File Offset: 0x0016AF64
		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0016CD78 File Offset: 0x0016AF78
		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(keyBindingDef))
				{
					this.keyPrefs.Add(keyBindingDef, new KeyBindingData(keyBindingDef.defaultKeyCodeA, keyBindingDef.defaultKeyCodeB));
				}
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0016CDE8 File Offset: 0x0016AFE8
		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData;
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				if (slot != KeyPrefs.BindingSlot.A)
				{
					if (slot != KeyPrefs.BindingSlot.B)
					{
						Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString());
						return false;
					}
					keyBindingData.keyBindingB = keyCode;
				}
				else
				{
					keyBindingData.keyBindingA = keyCode;
				}
				return true;
			}
			Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
			return false;
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0016CE80 File Offset: 0x0016B080
		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData;
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
				return KeyCode.None;
			}
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return keyBindingData.keyBindingA;
			}
			if (slot != KeyPrefs.BindingSlot.B)
			{
				throw new InvalidOperationException();
			}
			return keyBindingData.keyBindingB;
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0016CEDF File Offset: 0x0016B0DF
		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			KeyPrefsData.<ConflictingBindings>d__5 <ConflictingBindings>d__ = new KeyPrefsData.<ConflictingBindings>d__5(-2);
			<ConflictingBindings>d__.<>4__this = this;
			<ConflictingBindings>d__.<>3__keyDef = keyDef;
			<ConflictingBindings>d__.<>3__code = code;
			return <ConflictingBindings>d__;
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x0016CF00 File Offset: 0x0016B100
		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[keyBindingDef];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if (callBackOnErase != null)
				{
					callBackOnErase(keyBindingDef);
				}
			}
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0016CF80 File Offset: 0x0016B180
		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x0016CFAC File Offset: 0x0016B1AC
		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyValuePair.Key] = new KeyBindingData(keyValuePair.Value.keyBindingA, keyValuePair.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x0016D030 File Offset: 0x0016B230
		public void ErrorCheck()
		{
			foreach (KeyBindingDef keyDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.B);
			}
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0016D088 File Offset: 0x0016B288
		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
					string[] array = new string[8];
					array[0] = "Key binding conflict: ";
					int num = 1;
					KeyBindingDef keyBindingDef2 = keyBindingDef;
					array[num] = ((keyBindingDef2 != null) ? keyBindingDef2.ToString() : null);
					array[2] = " and ";
					array[3] = ((keyDef != null) ? keyDef.ToString() : null);
					array[4] = " are both bound to ";
					array[5] = boundKeyCode.ToString();
					array[6] = ".";
					array[7] = (flag ? " Fixed automatically." : "");
					Log.Warning(string.Concat(array));
					if (flag)
					{
						if (slot == KeyPrefs.BindingSlot.A)
						{
							this.keyPrefs[keyDef].keyBindingA = keyDef.defaultKeyCodeA;
						}
						else
						{
							this.keyPrefs[keyDef].keyBindingB = keyDef.defaultKeyCodeB;
						}
						KeyPrefs.Save();
					}
				}
			}
		}

		// Token: 0x040024FD RID: 9469
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();
	}
}
