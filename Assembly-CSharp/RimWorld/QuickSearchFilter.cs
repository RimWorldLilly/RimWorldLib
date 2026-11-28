using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE3 RID: 7651
	public class QuickSearchFilter
	{
		// Token: 0x170024C8 RID: 9416
		// (get) Token: 0x0600C6F0 RID: 50928 RVA: 0x0048DB8F File Offset: 0x0048BD8F
		// (set) Token: 0x0600C6F1 RID: 50929 RVA: 0x0048DB97 File Offset: 0x0048BD97
		public string Text
		{
			get
			{
				return this.inputText;
			}
			set
			{
				this.inputText = value;
				this.searchText = value.Trim();
				this.cachedMatches.Clear();
			}
		}

		// Token: 0x170024C9 RID: 9417
		// (get) Token: 0x0600C6F2 RID: 50930 RVA: 0x0048DBB7 File Offset: 0x0048BDB7
		public bool Active
		{
			get
			{
				return !this.inputText.NullOrEmpty();
			}
		}

		// Token: 0x0600C6F3 RID: 50931 RVA: 0x0048DBC8 File Offset: 0x0048BDC8
		public bool Matches(string value)
		{
			if (!this.Active)
			{
				return true;
			}
			if (value.NullOrEmpty())
			{
				return false;
			}
			bool flag;
			if (!this.cachedMatches.TryGetValue(value, out flag))
			{
				flag = this.MatchImpl(value);
				this.cachedMatches.Add(value, flag);
			}
			return flag;
		}

		// Token: 0x0600C6F4 RID: 50932 RVA: 0x0048DC0F File Offset: 0x0048BE0F
		private bool MatchImpl(string value)
		{
			return value.IndexOf(this.searchText, StringComparison.InvariantCultureIgnoreCase) != -1;
		}

		// Token: 0x0600C6F5 RID: 50933 RVA: 0x0048DC24 File Offset: 0x0048BE24
		public bool Matches(ThingDef td)
		{
			return !Find.HiddenItemsManager.Hidden(td) && this.Matches(td.label);
		}

		// Token: 0x0600C6F6 RID: 50934 RVA: 0x0048DC41 File Offset: 0x0048BE41
		public bool Matches(SpecialThingFilterDef sfDef)
		{
			return this.Matches(sfDef.label);
		}

		// Token: 0x0400711C RID: 28956
		private string inputText = "";

		// Token: 0x0400711D RID: 28957
		private string searchText = "";

		// Token: 0x0400711E RID: 28958
		private readonly LRUCache<string, bool> cachedMatches = new LRUCache<string, bool>(5000);
	}
}
