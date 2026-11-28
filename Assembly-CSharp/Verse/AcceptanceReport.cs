using System;

namespace Verse
{
	// Token: 0x0200069F RID: 1695
	public struct AcceptanceReport
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x0015AFD5 File Offset: 0x001591D5
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600355B RID: 13659 RVA: 0x0015AFDD File Offset: 0x001591DD
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600355C RID: 13660 RVA: 0x0015AFE8 File Offset: 0x001591E8
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600355D RID: 13661 RVA: 0x0015B00C File Offset: 0x0015920C
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x0015B02E File Offset: 0x0015922E
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x0015B03E File Offset: 0x0015923E
		public static implicit operator bool(AcceptanceReport report)
		{
			return report.Accepted;
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x0015B047 File Offset: 0x00159247
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x0015B057 File Offset: 0x00159257
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x0015B05F File Offset: 0x0015925F
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x040022E6 RID: 8934
		private string reasonTextInt;

		// Token: 0x040022E7 RID: 8935
		private bool acceptedInt;
	}
}
