using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x0200008F RID: 143
	public static class CultureInfoUtility
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00025971 File Offset: 0x00023B71
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			}
		}

		// Token: 0x0400022A RID: 554
		private const string EnglishCulture = "en-US";
	}
}
