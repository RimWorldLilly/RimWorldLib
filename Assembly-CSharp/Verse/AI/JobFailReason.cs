using System;

namespace Verse.AI
{
	// Token: 0x0200084A RID: 2122
	public static class JobFailReason
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060042AF RID: 17071 RVA: 0x001A4C22 File Offset: 0x001A2E22
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x001A4C29 File Offset: 0x001A2E29
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x001A4C33 File Offset: 0x001A2E33
		public static bool Silent
		{
			get
			{
				return JobFailReason.silent;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x001A4C3A File Offset: 0x001A2E3A
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x001A4C41 File Offset: 0x001A2E41
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x001A4C4F File Offset: 0x001A2E4F
		public static void IsSilent()
		{
			JobFailReason.silent = true;
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x001A4C57 File Offset: 0x001A2E57
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
			JobFailReason.silent = false;
		}

		// Token: 0x04002B6C RID: 11116
		private static string lastReason;

		// Token: 0x04002B6D RID: 11117
		private static string lastCustomJobString;

		// Token: 0x04002B6E RID: 11118
		private static bool silent;
	}
}
