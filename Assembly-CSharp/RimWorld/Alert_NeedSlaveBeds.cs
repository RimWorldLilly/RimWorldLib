using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCF RID: 7119
	public class Alert_NeedSlaveBeds : Alert
	{
		// Token: 0x0600B667 RID: 46695 RVA: 0x00404080 File Offset: 0x00402280
		public Alert_NeedSlaveBeds()
		{
			this.defaultLabel = "NeedSlaveBeds".Translate();
			this.defaultExplanation = "NeedSlaveBedsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
		}

		// Token: 0x0600B668 RID: 46696 RVA: 0x004040C0 File Offset: 0x004022C0
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed > 30)
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					bool flag;
					bool flag2;
					Alert_NeedSlaveBeds.CheckSlaveBeds(map, out flag, out flag2);
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B669 RID: 46697 RVA: 0x00404144 File Offset: 0x00402344
		public static void CheckSlaveBeds(Map map, out bool enoughBeds, out bool enoughBabyCribs)
		{
			int num = 0;
			int num2 = 0;
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building_Bed building_Bed = allBuildingsColonist[i] as Building_Bed;
				if (building_Bed != null && building_Bed.ForSlaves && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForHumanBabies)
					{
						num2 += building_Bed.TotalSleepingSlots;
					}
					else
					{
						num += building_Bed.TotalSleepingSlots;
					}
				}
			}
			int num3 = 0;
			int num4 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.IsSlave)
				{
					if (pawn.DevelopmentalStage.Baby())
					{
						num4++;
					}
					else
					{
						num3++;
					}
				}
			}
			enoughBeds = (num >= num3);
			int num5 = Mathf.Max(0, num - num3);
			enoughBabyCribs = (num5 + num2 >= num4);
		}
	}
}
