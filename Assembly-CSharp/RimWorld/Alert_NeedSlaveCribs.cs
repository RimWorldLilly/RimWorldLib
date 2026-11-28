using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD0 RID: 7120
	public class Alert_NeedSlaveCribs : Alert
	{
		// Token: 0x0600B66A RID: 46698 RVA: 0x00404270 File Offset: 0x00402470
		public Alert_NeedSlaveCribs()
		{
			this.defaultLabel = "AlertNeedSlaveCribs".Translate();
			this.defaultExplanation = "AlertNeedSlaveCribsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
			this.requireBiotech = true;
		}

		// Token: 0x0600B66B RID: 46699 RVA: 0x004042C4 File Offset: 0x004024C4
		public override AlertReport GetReport()
		{
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					bool flag;
					bool flag2;
					Alert_NeedSlaveBeds.CheckSlaveBeds(map, out flag, out flag2);
					if (!flag2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
