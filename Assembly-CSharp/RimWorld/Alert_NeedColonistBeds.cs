using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCD RID: 7117
	public class Alert_NeedColonistBeds : Alert
	{
		// Token: 0x0600B660 RID: 46688 RVA: 0x00403B96 File Offset: 0x00401D96
		public Alert_NeedColonistBeds()
		{
			this.defaultLabel = "NeedColonistBeds".Translate();
			this.defaultExplanation = "NeedColonistBedsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B661 RID: 46689 RVA: 0x00403BD0 File Offset: 0x00401DD0
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed > 30)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedColonistBeds(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B662 RID: 46690 RVA: 0x00403C20 File Offset: 0x00401E20
		private bool NeedColonistBeds(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			int num;
			int num2;
			int num3;
			Alert_NeedColonistBeds.AvailableColonistBeds(map, false, out num, out num2, out num3);
			return num < 0 || num2 < 0;
		}

		// Token: 0x0600B663 RID: 46691 RVA: 0x00403C50 File Offset: 0x00401E50
		public static void AvailableColonistBeds(Map map, bool includeBabies, out int singleBeds, out int doubleBeds, out int cribs)
		{
			singleBeds = 0;
			doubleBeds = 0;
			cribs = 0;
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building_Bed building_Bed = allBuildingsColonist[i] as Building_Bed;
				if (building_Bed != null && building_Bed.ForColonists && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForHumanBabies)
					{
						cribs++;
					}
					else if (building_Bed.SleepingSlotsCount == 1)
					{
						singleBeds++;
					}
					else
					{
						doubleBeds++;
					}
				}
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			IEnumerable<Pawn> enumerable = from p in map.mapPawns.FreeColonistsSpawned
			where !p.IsSlave
			select p;
			Alert_NeedColonistBeds.tmpAssignedPawns.Clear();
			foreach (Pawn pawn in enumerable)
			{
				if (pawn.Spawned || pawn.BrieflyDespawned())
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.rest : null) != null && !Alert_NeedColonistBeds.tmpAssignedPawns.Contains(pawn) && !pawn.DevelopmentalStage.Baby())
					{
						List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(pawn, false);
						if (list.NullOrEmpty<DirectPawnRelation>())
						{
							num2++;
							Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn);
						}
						else
						{
							Pawn pawn2 = null;
							int num4 = int.MaxValue;
							for (int j = 0; j < list.Count; j++)
							{
								Pawn otherPawn = list[j].otherPawn;
								if (otherPawn != null && otherPawn.Spawned && otherPawn.Map == pawn.Map && otherPawn.Faction == Faction.OfPlayer && otherPawn.HostFaction == null && !Alert_NeedColonistBeds.tmpAssignedPawns.Contains(otherPawn))
								{
									Pawn_NeedsTracker needs2 = otherPawn.needs;
									if (((needs2 != null) ? needs2.rest : null) != null)
									{
										int num5 = LovePartnerRelationUtility.ExistingLovePartnersCount(otherPawn, false);
										if (pawn2 == null || num5 < num4)
										{
											pawn2 = otherPawn;
											num4 = num5;
										}
									}
								}
							}
							if (pawn2 != null)
							{
								Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn2);
								Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn);
								num3++;
							}
						}
					}
				}
			}
			foreach (Pawn pawn3 in enumerable)
			{
				Pawn_NeedsTracker needs3 = pawn3.needs;
				if (((needs3 != null) ? needs3.rest : null) != null && !Alert_NeedColonistBeds.tmpAssignedPawns.Contains(pawn3))
				{
					if (pawn3.DevelopmentalStage.Baby())
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			Alert_NeedColonistBeds.tmpAssignedPawns.Clear();
			for (int k = 0; k < num3; k++)
			{
				if (doubleBeds > 0)
				{
					doubleBeds--;
				}
				else
				{
					singleBeds -= 2;
				}
			}
			for (int l = 0; l < num2; l++)
			{
				if (doubleBeds > 0)
				{
					doubleBeds--;
				}
				else
				{
					singleBeds--;
				}
			}
			if (includeBabies)
			{
				for (int m = 0; m < num; m++)
				{
					if (doubleBeds > 0)
					{
						doubleBeds--;
					}
					else if (singleBeds > 0)
					{
						singleBeds--;
					}
					else
					{
						cribs--;
					}
				}
			}
		}

		// Token: 0x040068C7 RID: 26823
		private static List<Pawn> tmpAssignedPawns = new List<Pawn>();
	}
}
