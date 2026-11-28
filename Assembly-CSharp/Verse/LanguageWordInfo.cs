using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000213 RID: 531
	public class LanguageWordInfo
	{
		// Token: 0x06000F67 RID: 3943 RVA: 0x00056BB8 File Offset: 0x00054DB8
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00056C1C File Offset: 0x00054E1C
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			if (str == null)
			{
				return defaultGender;
			}
			Gender result;
			if (this.TryResolveGender(str, out result))
			{
				return result;
			}
			if (fallback != null && this.TryResolveGender(str, out result))
			{
				return result;
			}
			return defaultGender;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00056C4C File Offset: 0x00054E4C
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00056CAC File Offset: 0x00054EAC
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00056D2C File Offset: 0x00054F2C
		public void RegisterLut(string name)
		{
			if (this.lookupTables.ContainsKey(name.ToLower()))
			{
				Log.Error("Tried registering language look up table named " + name + " twice.");
				return;
			}
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			try
			{
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in activeLanguage.AllDirectories)
				{
					VirtualFile file = tuple.Item1.GetDirectory("WordInfo").GetFile(name + ".txt");
					if (file.Exists)
					{
						foreach (string text in GenText.LinesFromString(file.ReadAllText()))
						{
							string[] array;
							if (GenText.TryGetSeparatedValues(text, ';', out array))
							{
								string key = array[0].ToLower();
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, array);
								}
							}
							else
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Failed parsing lookup items from line ");
								defaultInterpolatedStringHandler.AppendFormatted(text);
								defaultInterpolatedStringHandler.AppendLiteral(" in ");
								defaultInterpolatedStringHandler.AppendFormatted(file.FullPath);
								defaultInterpolatedStringHandler.AppendLiteral(". Line: ");
								Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear() + text, name.GetHashCode() ^ 1857221523);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Exception parsing a language lookup table: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.lookupTables.Add(name.ToLower(), dictionary);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00056F14 File Offset: 0x00055114
		public Dictionary<string, string[]> GetLookupTable(string name)
		{
			string text = name.ToLower();
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			this.RegisterLut(text);
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			return null;
		}

		// Token: 0x04000EC5 RID: 3781
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x04000EC6 RID: 3782
		private Dictionary<string, Dictionary<string, string[]>> lookupTables = new Dictionary<string, Dictionary<string, string[]>>();

		// Token: 0x04000EC7 RID: 3783
		private const string FolderName = "WordInfo";

		// Token: 0x04000EC8 RID: 3784
		private const string GendersFolderName = "Gender";

		// Token: 0x04000EC9 RID: 3785
		private const string MaleFileName = "Male.txt";

		// Token: 0x04000ECA RID: 3786
		private const string FemaleFileName = "Female.txt";

		// Token: 0x04000ECB RID: 3787
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x04000ECC RID: 3788
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
