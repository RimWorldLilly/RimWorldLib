using System;

namespace Verse
{
	// Token: 0x020004D4 RID: 1236
	public class NameTriple : Name
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x000CC817 File Offset: 0x000CAA17
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000CC81F File Offset: 0x000CAA1F
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x000CC828 File Offset: 0x000CAA28
		public string Nick
		{
			get
			{
				if (!this.nickInt.NullOrEmpty())
				{
					return this.nickInt;
				}
				if (this.Last == "")
				{
					return this.First;
				}
				if (this.First == "")
				{
					return this.Last;
				}
				if ((Gen.HashCombine<int>(this.First.GetHashCode(), this.Last.GetHashCode()) & 1) == 1)
				{
					return this.First;
				}
				return this.Last;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000CC8A8 File Offset: 0x000CAAA8
		public override string ToStringFull
		{
			get
			{
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					return this.First + " " + this.Last;
				}
				return string.Concat(new string[]
				{
					this.First,
					" '",
					this.Nick,
					"' ",
					this.Last
				});
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x000CC928 File Offset: 0x000CAB28
		public override string ToStringShort
		{
			get
			{
				return this.Nick;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000CC930 File Offset: 0x000CAB30
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x000CC94F File Offset: 0x000CAB4F
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x000CC952 File Offset: 0x000CAB52
		public bool NickSet
		{
			get
			{
				return !this.nickInt.NullOrEmpty();
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x000CC962 File Offset: 0x000CAB62
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000CC969 File Offset: 0x000CAB69
		public NameTriple()
		{
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000CC971 File Offset: 0x000CAB71
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = ((nick != null) ? nick.Trim() : null);
			this.lastInt = last.Trim();
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000CC9A3 File Offset: 0x000CABA3
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000CC9DC File Offset: 0x000CABDC
		public void PostLoad()
		{
			string text = this.firstInt;
			this.firstInt = ((text != null) ? text.Trim() : null);
			string text2 = this.nickInt;
			this.nickInt = ((text2 != null) ? text2.Trim() : null);
			string text3 = this.lastInt;
			this.lastInt = ((text3 != null) ? text3.Trim() : null);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000CCA34 File Offset: 0x000CAC34
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve missing pieces in PawnName: No name data.");
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
				return;
			}
			if (this.First == null)
			{
				this.firstInt = "";
			}
			if (this.Last == null)
			{
				this.lastInt = "";
			}
			if (overrideLastName != null)
			{
				this.lastInt = overrideLastName;
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000CCAC0 File Offset: 0x000CACC0
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000CCB3C File Offset: 0x000CAD3C
		public static NameTriple FromString(string rawName, bool forceNoNick = false)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.");
				return NameTriple.Invalid;
			}
			NameTriple nameTriple = new NameTriple();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < rawName.Length - 1; i++)
			{
				if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
				{
					num = i;
				}
				if (rawName[i] == '\'' && rawName[i + 1] == ' ')
				{
					num2 = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				if (!rawName.Contains(' '))
				{
					nameTriple.nickInt = rawName.Trim();
				}
				else
				{
					string[] array = rawName.Split(' ', StringSplitOptions.None);
					if (array.Length == 1)
					{
						nameTriple.nickInt = array[0].Trim();
					}
					else if (array.Length == 2)
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = array[1].Trim();
					}
					else
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = "";
						for (int j = 1; j < array.Length; j++)
						{
							NameTriple nameTriple2 = nameTriple;
							nameTriple2.lastInt += array[j];
							if (j < array.Length - 1)
							{
								NameTriple nameTriple3 = nameTriple;
								nameTriple3.lastInt += " ";
							}
						}
					}
				}
			}
			else
			{
				nameTriple.firstInt = rawName.Substring(0, num).Trim();
				if (!forceNoNick)
				{
					nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
				}
				nameTriple.lastInt = ((num2 < rawName.Length - 2) ? rawName.Substring(num2 + 2).Trim() : "");
			}
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000CCCF3 File Offset: 0x000CAEF3
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000CCD2C File Offset: 0x000CAF2C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameTriple))
			{
				return false;
			}
			NameTriple nameTriple = (NameTriple)obj;
			return this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000CCD88 File Offset: 0x000CAF88
		public NameTriple WithoutNick()
		{
			return new NameTriple(this.firstInt, null, this.lastInt);
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000CCD9C File Offset: 0x000CAF9C
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x170006C4 RID: 1732
		public string this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.First;
				case 1:
					return this.Nick;
				case 2:
					return this.Last;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x040019CF RID: 6607
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x040019D0 RID: 6608
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x040019D1 RID: 6609
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x040019D2 RID: 6610
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
