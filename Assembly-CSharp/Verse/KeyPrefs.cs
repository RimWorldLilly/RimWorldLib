using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000700 RID: 1792
	public class KeyPrefs
	{
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0016CB19 File Offset: 0x0016AD19
		// (set) Token: 0x060038D3 RID: 14547 RVA: 0x0016CB20 File Offset: 0x0016AD20
		public static KeyPrefsData KeyPrefsData
		{
			get
			{
				return KeyPrefs.data;
			}
			set
			{
				KeyPrefs.data = value;
			}
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0016CB28 File Offset: 0x0016AD28
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.KeyPrefsFilePath).Exists;
			Dictionary<string, KeyBindingData> dictionary = DirectXmlLoader.ItemFromXmlFile<Dictionary<string, KeyBindingData>>(GenFilePaths.KeyPrefsFilePath, true);
			KeyPrefs.data = new KeyPrefsData();
			KeyPrefs.unresolvedBindings = new Dictionary<string, KeyBindingData>();
			foreach (KeyValuePair<string, KeyBindingData> keyValuePair in dictionary)
			{
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(keyValuePair.Key);
				if (namedSilentFail != null)
				{
					KeyPrefs.data.keyPrefs[namedSilentFail] = keyValuePair.Value;
				}
				else
				{
					KeyPrefs.unresolvedBindings[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (flag)
			{
				KeyPrefs.data.ResetToDefaults();
			}
			KeyPrefs.data.AddMissingDefaultBindings();
			KeyPrefs.data.ErrorCheck();
			if (flag)
			{
				KeyPrefs.Save();
			}
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0016CC0C File Offset: 0x0016AE0C
		public static void Save()
		{
			try
			{
				Dictionary<string, KeyBindingData> dictionary = new Dictionary<string, KeyBindingData>();
				foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in KeyPrefs.data.keyPrefs)
				{
					dictionary[keyValuePair.Key.defName] = keyValuePair.Value;
				}
				foreach (KeyValuePair<string, KeyBindingData> keyValuePair2 in KeyPrefs.unresolvedBindings)
				{
					try
					{
						dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					catch (ArgumentException)
					{
					}
				}
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(dictionary, typeof(KeyPrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.KeyPrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.KeyPrefsFilePath, ex.ToString()));
				string str = "Exception saving keyprefs: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x040024FB RID: 9467
		private static KeyPrefsData data;

		// Token: 0x040024FC RID: 9468
		private static Dictionary<string, KeyBindingData> unresolvedBindings;

		// Token: 0x02002A6A RID: 10858
		public enum BindingSlot : byte
		{
			// Token: 0x0400ADD8 RID: 44504
			A,
			// Token: 0x0400ADD9 RID: 44505
			B
		}
	}
}
