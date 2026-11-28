using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020007B0 RID: 1968
	public class Rule_File : Rule
	{
		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06003F76 RID: 16246 RVA: 0x0019681C File Offset: 0x00194A1C
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0019682C File Offset: 0x00194A2C
		public override Rule DeepCopy()
		{
			Rule_File rule_File = (Rule_File)base.DeepCopy();
			rule_File.path = this.path;
			if (this.pathList != null)
			{
				rule_File.pathList = this.pathList.ToList<string>();
			}
			if (this.cachedStrings != null)
			{
				rule_File.cachedStrings = this.cachedStrings.ToList<string>();
			}
			return rule_File;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00196884 File Offset: 0x00194A84
		public override string Generate()
		{
			if (this.cachedStrings.NullOrEmpty<string>())
			{
				return "Filestring";
			}
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x001968A4 File Offset: 0x00194AA4
		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string filePath in this.pathList)
			{
				this.LoadStringsFromFile(filePath);
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00196910 File Offset: 0x00194B10
		private void LoadStringsFromFile(string filePath)
		{
			List<string> list;
			if (Translator.TryGetTranslatedStringsForFile(filePath, out list))
			{
				foreach (string item in list)
				{
					this.cachedStrings.Add(item);
				}
			}
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00196970 File Offset: 0x00194B70
		public override string ToString()
		{
			if (!this.path.NullOrEmpty())
			{
				return string.Concat(new string[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count.ToString(),
					" strings from file: ",
					this.path,
					")"
				});
			}
			if (this.pathList.Count > 0)
			{
				return string.Concat(new string[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count.ToString(),
					" strings from ",
					this.pathList.Count.ToString(),
					" files)"
				});
			}
			return this.keyword + "->(Rule_File with no configuration)";
		}

		// Token: 0x040029FC RID: 10748
		[MayTranslate]
		public string path;

		// Token: 0x040029FD RID: 10749
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x040029FE RID: 10750
		[Unsaved(false)]
		private List<string> cachedStrings = new List<string>();
	}
}
