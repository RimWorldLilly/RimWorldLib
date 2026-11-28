using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011BB RID: 4539
	public static class NeutralGroupIncidentUtility
	{
		// Token: 0x0600716B RID: 29035 RVA: 0x0027B9B8 File Offset: 0x00279BB8
		public static bool AnyBlockingHostileLord(Map map, Faction forFaction)
		{
			Faction faction = map.ParentFaction ?? Faction.OfPlayer;
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].faction != null && lords[i].faction != forFaction && lords[i].faction != faction && lords[i].AnyActivePawn)
				{
					LordJob lordJob = lords[i].LordJob;
					if ((lordJob == null || lordJob.CanBlockHostileVisitors) && !(lordJob is LordJob_VoluntarilyJoinable) && lords[i].faction.HostileTo(forFaction) && !lords[i].faction.HostileTo(faction))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
