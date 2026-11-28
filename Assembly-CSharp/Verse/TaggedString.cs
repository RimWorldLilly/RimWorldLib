using System;

namespace Verse
{
	// Token: 0x02000638 RID: 1592
	public struct TaggedString
	{
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x0013EA25 File Offset: 0x0013CC25
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x1700098E RID: 2446
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x0013EA3B File Offset: 0x0013CC3B
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x0013EA48 File Offset: 0x0013CC48
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x0013EA5A File Offset: 0x0013CC5A
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x0013EA63 File Offset: 0x0013CC63
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x0013EA70 File Offset: 0x0013CC70
		public TaggedString CapitalizeFirst()
		{
			if (this.rawText.NullOrEmpty())
			{
				return this;
			}
			int num = this.FirstLetterBetweenTags();
			if (char.ToUpper(this.rawText[num]) == this.rawText[num])
			{
				return this;
			}
			if (this.rawText.Length == 1)
			{
				return new TaggedString(this.rawText.ToUpper());
			}
			if (num == 0)
			{
				char c = char.ToUpper(this.rawText[num]);
				return new TaggedString(new ReadOnlySpan<char>(ref c) + this.rawText.Substring(num + 1));
			}
			ReadOnlySpan<char> str = this.rawText.Substring(0, num);
			char c2 = char.ToUpper(this.rawText[num]);
			return new TaggedString(str + new ReadOnlySpan<char>(ref c2) + this.rawText.Substring(num + 1));
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x0013EB60 File Offset: 0x0013CD60
		public TaggedString EndWithPeriod()
		{
			if (this.rawText.NullOrEmpty())
			{
				return this;
			}
			if (this.rawText[this.rawText.Length - 1] == '.')
			{
				return this;
			}
			return this.rawText + ".";
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x0013EBBC File Offset: 0x0013CDBC
		private int FirstLetterBetweenTags()
		{
			bool flag = false;
			for (int i = 0; i < this.rawText.Length - 1; i++)
			{
				if (this.rawText[i] == '(' && this.rawText[i + 1] == '*')
				{
					flag = true;
				}
				else
				{
					if (flag && this.rawText[i] == ')' && this.rawText[i + 1] != '(')
					{
						return i + 1;
					}
					if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0013EC38 File Offset: 0x0013CE38
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x0013EC45 File Offset: 0x0013CE45
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x0013EC5A File Offset: 0x0013CE5A
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0013EC6C File Offset: 0x0013CE6C
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0013EC7E File Offset: 0x0013CE7E
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x0013EC97 File Offset: 0x0013CE97
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x0013ECA9 File Offset: 0x0013CEA9
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0013ECBD File Offset: 0x0013CEBD
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x0013ECCB File Offset: 0x0013CECB
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x0013ECD3 File Offset: 0x0013CED3
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0013ECED File Offset: 0x0013CEED
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x0013ED01 File Offset: 0x0013CF01
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x0013ED15 File Offset: 0x0013CF15
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x04001FEF RID: 8175
		private string rawText;

		// Token: 0x04001FF0 RID: 8176
		public static readonly TaggedString Empty = new TaggedString("");
	}
}
