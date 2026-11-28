using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8C RID: 7308
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x170021D9 RID: 8665
		// (get) Token: 0x0600BA88 RID: 47752 RVA: 0x0041BDAC File Offset: 0x00419FAC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x0600BA89 RID: 47753 RVA: 0x0041BDC0 File Offset: 0x00419FC0
		public Dialog_AddPreferredName() : base(null)
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.lastSearch = "";
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
			this.CacheBlacklist();
		}

		// Token: 0x0600BA8A RID: 47754 RVA: 0x0041BE60 File Offset: 0x0041A060
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f, null);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(' ', StringSplitOptions.None);
			}
			listing_Standard.Gap(4f);
			int num = Mathf.FloorToInt((inRect.height - (Window.CloseButSize.y + 8f) - listing_Standard.CurHeight) / (30f + listing_Standard.verticalSpacing));
			if (this.searchName.Length > 1)
			{
				if (this.searchName != this.lastSearch)
				{
					this.lastSearch = this.searchName;
					Dialog_AddPreferredName.cachedSearch.Clear();
					foreach (NameTriple nameTriple in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
					{
						Dialog_AddPreferredName.cachedSearch.Add(nameTriple);
						if (listing_Standard.ButtonText(nameTriple.ToString(), null, 1f))
						{
							this.TryChooseName(nameTriple);
						}
						if (Dialog_AddPreferredName.cachedSearch.Count >= num)
						{
							break;
						}
					}
				}
				foreach (NameTriple nameTriple2 in Dialog_AddPreferredName.cachedSearch)
				{
					if (listing_Standard.ButtonText(nameTriple2.ToString(), null, 1f))
					{
						this.TryChooseName(nameTriple2);
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x0600BA8B RID: 47755 RVA: 0x0041C030 File Offset: 0x0041A230
		private bool FilterMatch(NameTriple n)
		{
			if (Dialog_AddPreferredName.IsBlacklisted(n))
			{
				return false;
			}
			if (this.searchWords.Length == 0)
			{
				return false;
			}
			if (this.searchWords.Length == 1)
			{
				return n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase);
			}
			if (this.searchWords.Length > 1)
			{
				string value = this.searchName.Replace("'", "");
				if (n.First.IndexOf(' ') >= 0)
				{
					return n.First.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
				if (n.Last.IndexOf(' ') >= 0)
				{
					return n.Last.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
				if (n.Nick.IndexOf(' ') >= 0)
				{
					return n.Nick.StartsWith(value, StringComparison.OrdinalIgnoreCase);
				}
			}
			return this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x0041C165 File Offset: 0x0041A365
		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Prefs.PreferredNames.Add(name.ToString());
			this.Close(true);
		}

		// Token: 0x0600BA8D RID: 47757 RVA: 0x0041C1A2 File Offset: 0x0041A3A2
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		// Token: 0x0600BA8E RID: 47758 RVA: 0x0041C1B4 File Offset: 0x0041A3B4
		private void CacheBlacklist()
		{
			if (Dialog_AddPreferredName.Blacklist.Any<NameTriple>())
			{
				return;
			}
			Dialog_AddPreferredName.Blacklist.AddRange((from x in SolidBioDatabase.allBios
			where x.rare
			select x into b
			select b.name).ToList<NameTriple>());
		}

		// Token: 0x0600BA8F RID: 47759 RVA: 0x0041C22C File Offset: 0x0041A42C
		private static bool IsBlacklisted(NameTriple triple)
		{
			foreach (NameTriple nameTriple in Dialog_AddPreferredName.Blacklist)
			{
				if (nameTriple.First.EqualsIgnoreCase(triple.First) && nameTriple.Last.EqualsIgnoreCase(triple.Last))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040069F9 RID: 27129
		private string searchName = "";

		// Token: 0x040069FA RID: 27130
		private string[] searchWords;

		// Token: 0x040069FB RID: 27131
		private List<NameTriple> cachedNames;

		// Token: 0x040069FC RID: 27132
		private static readonly List<NameTriple> Blacklist = new List<NameTriple>();

		// Token: 0x040069FD RID: 27133
		private static readonly List<NameTriple> cachedSearch = new List<NameTriple>();

		// Token: 0x040069FE RID: 27134
		private string lastSearch;
	}
}
