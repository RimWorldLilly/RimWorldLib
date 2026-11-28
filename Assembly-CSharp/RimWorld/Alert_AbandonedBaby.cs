using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC9 RID: 7113
	public class Alert_AbandonedBaby : Alert
	{
		// Token: 0x0600B64B RID: 46667 RVA: 0x00402F34 File Offset: 0x00401134
		public Alert_AbandonedBaby()
		{
			this.defaultLabel = "AlertAbandonedBaby".Translate();
			this.defaultExplanation = "AlertAbandonedBabyDesc".Translate("DesignatorAdopt".Translate());
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B64C RID: 46668 RVA: 0x00402F8E File Offset: 0x0040118E
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(Alert_AbandonedBaby.AbandonedBabies());
		}

		// Token: 0x0600B64D RID: 46669 RVA: 0x00402F9C File Offset: 0x0040119C
		private static List<Pawn> AbandonedBabies()
		{
			Alert_AbandonedBaby.tmpAllBabiesList.Clear();
			Alert_AbandonedBaby.tmpAbandonedBabiesList.Clear();
			using (new ProfilerBlock("AllBabies"))
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.AllPawns)
					{
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							Alert_AbandonedBaby.tmpAllBabiesList.Add(pawn);
						}
					}
				}
			}
			using (new ProfilerBlock("AbandonedBabiesFilter"))
			{
				foreach (Pawn pawn2 in Alert_AbandonedBaby.tmpAllBabiesList)
				{
					if (pawn2.Faction != Faction.OfPlayer)
					{
						if (!pawn2.Spawned)
						{
							IThingHolder parentHolder = pawn2.ParentHolder;
							if (parentHolder != null && !(parentHolder is Pawn_CarryTracker))
							{
								continue;
							}
						}
						Map mapHeld = pawn2.MapHeld;
						bool flag = true;
						foreach (Pawn pawn3 in pawn2.mindState.Autofeeders())
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
							if (pawn3.MapHeld == mapHeld && ChildcareUtility.CanFeedBaby(pawn3, pawn2, out breastfeedFailReason))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							using (new ProfilerBlock("AdoptableBy"))
							{
								if (!pawn2.AdoptableBy(Faction.OfPlayer, null))
								{
									continue;
								}
							}
							Alert_AbandonedBaby.tmpAbandonedBabiesList.Add(pawn2);
						}
					}
				}
			}
			return Alert_AbandonedBaby.tmpAbandonedBabiesList;
		}

		// Token: 0x040068B6 RID: 26806
		private static List<Pawn> tmpAllBabiesList = new List<Pawn>(32);

		// Token: 0x040068B7 RID: 26807
		private static List<Pawn> tmpAbandonedBabiesList = new List<Pawn>(32);
	}
}
