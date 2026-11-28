using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFF RID: 7167
	public class Alert_PredatorInPen : Alert
	{
		// Token: 0x0600B720 RID: 46880 RVA: 0x00408940 File Offset: 0x00406B40
		public override string GetLabel()
		{
			return "AlertPredatorInAnimalPen".Translate(this.targets.First<GlobalTargetInfo>().Thing.Named("ANIMAL")).CapitalizeFirst();
		}

		// Token: 0x0600B721 RID: 46881 RVA: 0x00408984 File Offset: 0x00406B84
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
					{
						CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
						if (!compAnimalPenMarker.PenState.Unenclosed)
						{
							foreach (Region region in compAnimalPenMarker.PenState.ConnectedRegions)
							{
								foreach (Thing thing2 in region.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn))
								{
									Pawn pawn = (Pawn)thing2;
									if (pawn.IsAnimal && pawn.RaceProps.predator && pawn.Faction == null && pawn.GetRegion(RegionType.Set_Passable) == region)
									{
										this.targets.Add(pawn);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B722 RID: 46882 RVA: 0x00408B48 File Offset: 0x00406D48
		public override TaggedString GetExplanation()
		{
			return "AlertPredatorInAnimalPenDesc".Translate(this.targets.First<GlobalTargetInfo>().Thing.Named("ANIMAL")).CapitalizeFirst();
		}

		// Token: 0x0600B723 RID: 46883 RVA: 0x00408B84 File Offset: 0x00406D84
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400690F RID: 26895
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
