using System;

namespace Verse
{
	// Token: 0x020004D3 RID: 1235
	public class NameSingle : Name
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x000CC5F0 File Offset: 0x000CA7F0
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x000CC5F8 File Offset: 0x000CA7F8
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x000CC600 File Offset: 0x000CA800
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x000CC608 File Offset: 0x000CA808
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x000CC618 File Offset: 0x000CA818
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x000CC620 File Offset: 0x000CA820
		private int FirstDigitPosition
		{
			get
			{
				if (!this.numerical)
				{
					return -1;
				}
				if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					return -1;
				}
				for (int i = this.nameInt.Length - 2; i >= 0; i--)
				{
					if (!char.IsDigit(this.nameInt[i]))
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x000CC698 File Offset: 0x000CA898
		public string NameWithoutNumber
		{
			get
			{
				if (!this.numerical)
				{
					return this.nameInt;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return this.nameInt;
				}
				int num = firstDigitPosition;
				if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
				{
					num--;
				}
				if (num <= 0)
				{
					return "";
				}
				return this.nameInt.Substring(0, num);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x000CC6FC File Offset: 0x000CA8FC
		public int Number
		{
			get
			{
				if (!this.numerical)
				{
					return 0;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return 0;
				}
				return int.Parse(this.nameInt.Substring(firstDigitPosition));
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000CC731 File Offset: 0x000CA931
		public NameSingle()
		{
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000CC739 File Offset: 0x000CA939
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000CC74F File Offset: 0x000CA94F
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000CC778 File Offset: 0x000CA978
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				return true;
			}
			NameTriple nameTriple = other as NameTriple;
			return nameTriple != null && nameTriple.Nick == this.nameInt;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000CC7C4 File Offset: 0x000CA9C4
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000CC7CC File Offset: 0x000CA9CC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameSingle))
			{
				return false;
			}
			NameSingle nameSingle = (NameSingle)obj;
			return this.nameInt == nameSingle.nameInt;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000CC800 File Offset: 0x000CAA00
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x040019CD RID: 6605
		private string nameInt;

		// Token: 0x040019CE RID: 6606
		private bool numerical;
	}
}
