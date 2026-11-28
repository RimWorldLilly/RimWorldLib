using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDA RID: 7130
	public class Alert_SlavesUnattended : Alert
	{
		// Token: 0x170020FD RID: 8445
		// (get) Token: 0x0600B690 RID: 46736 RVA: 0x00405088 File Offset: 0x00403288
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (SlaveRebellionUtility.IsUnattendedByColonists(maps[i]))
					{
						this.targetsResult.AddRange(maps[i].mapPawns.SlavesOfColonySpawned);
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x004050E8 File Offset: 0x004032E8
		public Alert_SlavesUnattended()
		{
			this.defaultLabel = "SlaveUnattendedLabel".Translate();
			this.labelMultiple = "SlaveUnattendedMultipleLabel".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
		}

		// Token: 0x0600B692 RID: 46738 RVA: 0x0040513E File Offset: 0x0040333E
		public override string GetLabel()
		{
			if (this.targetsResult.Count != 1)
			{
				return this.labelMultiple;
			}
			return this.defaultLabel;
		}

		// Token: 0x0600B693 RID: 46739 RVA: 0x0040515B File Offset: 0x0040335B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B694 RID: 46740 RVA: 0x00405168 File Offset: 0x00403368
		public override TaggedString GetExplanation()
		{
			return "SlavesUnattendedDesc".Translate(this.targetsResult[0]);
		}

		// Token: 0x040068D6 RID: 26838
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x040068D7 RID: 26839
		private string labelMultiple;
	}
}
