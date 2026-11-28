using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001AE RID: 430
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06000C0B RID: 3083 RVA: 0x0004027C File Offset: 0x0003E47C
		public int CompletedProjects()
		{
			int num = 0;
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}
