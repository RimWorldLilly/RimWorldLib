using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE8 RID: 7144
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		// Token: 0x0600B6CD RID: 46797 RVA: 0x004069F7 File Offset: 0x00404BF7
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B6CE RID: 46798 RVA: 0x00406A30 File Offset: 0x00404C30
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + this.validationInfo;
		}

		// Token: 0x0600B6CF RID: 46799 RVA: 0x00406A50 File Offset: 0x00404C50
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Throne))
				{
					if (!thing.Fogged() && thing.Faction == Faction.OfPlayer)
					{
						this.validationInfo = RoomRoleWorker_ThroneRoom.Validate(thing.GetRoom(RegionType.Set_All));
						if (this.validationInfo != null)
						{
							return AlertReport.CulpritIs(thing);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040068F0 RID: 26864
		private string validationInfo;
	}
}
