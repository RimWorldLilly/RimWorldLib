using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C1 RID: 6081
	public static class ScenSummaryList
	{
		// Token: 0x060097AC RID: 38828 RVA: 0x003703F0 File Offset: 0x0036E5F0
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			if (!text.NullOrEmpty())
			{
				return "\n" + intro + ":\n" + text;
			}
			return null;
		}

		// Token: 0x060097AD RID: 38829 RVA: 0x00370420 File Offset: 0x0036E620
		private static string SummaryList(Scenario scen, string tag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (ScenPart scenPart in scen.AllParts)
			{
				if (!scenPart.summarized)
				{
					foreach (string str in scenPart.GetSummaryListEntries(tag))
					{
						if (!flag)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("   -" + str);
						scenPart.summarized = true;
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
