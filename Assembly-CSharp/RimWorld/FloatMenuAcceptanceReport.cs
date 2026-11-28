using System;

namespace RimWorld
{
	// Token: 0x02001767 RID: 5991
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x06009552 RID: 38226 RVA: 0x003657C7 File Offset: 0x003639C7
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x06009553 RID: 38227 RVA: 0x003657CF File Offset: 0x003639CF
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x06009554 RID: 38228 RVA: 0x003657D7 File Offset: 0x003639D7
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x06009555 RID: 38229 RVA: 0x003657E0 File Offset: 0x003639E0
		public static FloatMenuAcceptanceReport WasAccepted
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x06009556 RID: 38230 RVA: 0x00365800 File Offset: 0x00363A00
		public static FloatMenuAcceptanceReport WasRejected
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06009557 RID: 38231 RVA: 0x0036581E File Offset: 0x00363A1E
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		// Token: 0x06009558 RID: 38232 RVA: 0x0036582E File Offset: 0x00363A2E
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06009559 RID: 38233 RVA: 0x00365838 File Offset: 0x00363A38
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x0600955A RID: 38234 RVA: 0x00365860 File Offset: 0x00363A60
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x0600955B RID: 38235 RVA: 0x00365888 File Offset: 0x00363A88
		public static FloatMenuAcceptanceReport WithFailReasonAndMessage(string failReason, string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason,
				failMessageInt = failMessage
			};
		}

		// Token: 0x04005A0E RID: 23054
		private string failMessageInt;

		// Token: 0x04005A0F RID: 23055
		private string failReasonInt;

		// Token: 0x04005A10 RID: 23056
		private bool acceptedInt;
	}
}
