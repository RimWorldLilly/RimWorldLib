using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC4 RID: 7108
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x0600B639 RID: 46649 RVA: 0x0040262B File Offset: 0x0040082B
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B63A RID: 46650 RVA: 0x00402664 File Offset: 0x00400864
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					Alert_NeedMiner.tmpDesignations.Clear();
					Alert_NeedMiner.tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.Mine]);
					Alert_NeedMiner.tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.MineVein]);
					if (!Alert_NeedMiner.tmpDesignations.NullOrEmpty<Designation>())
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfPlayer))
						{
							if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed)
							{
								if (pawn.IsFreeColonist && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
								{
									flag = true;
									break;
								}
								if (pawn.IsColonyMechPlayerControlled && pawn.RaceProps.mechEnabledWorkTypes.Contains(WorkTypeDefOf.Mining) && pawn.RaceProps.mechFixedSkillLevel > 0)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(new GlobalTargetInfo(Alert_NeedMiner.tmpDesignations[0].target.Cell, map, false));
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040068AE RID: 26798
		private static List<Designation> tmpDesignations = new List<Designation>();
	}
}
