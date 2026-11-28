using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCE RID: 7118
	public class Alert_NeedBabyCribs : Alert
	{
		// Token: 0x0600B665 RID: 46693 RVA: 0x00403FC8 File Offset: 0x004021C8
		public Alert_NeedBabyCribs()
		{
			this.defaultLabel = "AlertNeedBabyCribs".Translate();
			this.defaultExplanation = "AlertNeedBabyCribsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B666 RID: 46694 RVA: 0x00404008 File Offset: 0x00402208
		public override AlertReport GetReport()
		{
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					int num;
					int num2;
					int num3;
					Alert_NeedColonistBeds.AvailableColonistBeds(map, true, out num, out num2, out num3);
					if (num3 < 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
