using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102C RID: 4140
	public class ExternalHistory : IExposable
	{
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06006852 RID: 26706 RVA: 0x0024DE34 File Offset: 0x0024C034
		public string AllInformation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("storyteller: ");
				stringBuilder.Append(this.storytellerName);
				stringBuilder.Append("   userName: ");
				stringBuilder.Append(this.userName);
				stringBuilder.Append("   realWorldDate(UTC): ");
				stringBuilder.Append(this.realWorldDate);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x0024DE98 File Offset: 0x0024C098
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
		}

		// Token: 0x04004267 RID: 16999
		public string gameVersion = "?";

		// Token: 0x04004268 RID: 17000
		public string gameplayID = "?";

		// Token: 0x04004269 RID: 17001
		public string userName = "?";

		// Token: 0x0400426A RID: 17002
		public string storytellerName = "?";

		// Token: 0x0400426B RID: 17003
		public string realWorldDate = "?";

		// Token: 0x0400426C RID: 17004
		public string firstUploadDate = "?";

		// Token: 0x0400426D RID: 17005
		public int firstUploadTime;

		// Token: 0x0400426E RID: 17006
		public bool devMode;

		// Token: 0x0400426F RID: 17007
		public History history = new History();

		// Token: 0x04004270 RID: 17008
		public static string defaultUserName = "Anonymous";
	}
}
