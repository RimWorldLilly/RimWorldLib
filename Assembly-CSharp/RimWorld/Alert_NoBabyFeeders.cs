using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC8 RID: 7112
	public class Alert_NoBabyFeeders : Alert
	{
		// Token: 0x0600B646 RID: 46662 RVA: 0x00402D84 File Offset: 0x00400F84
		public Alert_NoBabyFeeders()
		{
			this.defaultLabel = "AlertNoBabyFeeder".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B647 RID: 46663 RVA: 0x00402DAF File Offset: 0x00400FAF
		public override TaggedString GetExplanation()
		{
			return "AlertNoBabyFeederDesc".Translate(Faction.OfPlayer.Named("FACTION"));
		}

		// Token: 0x0600B648 RID: 46664 RVA: 0x00402DCA File Offset: 0x00400FCA
		public override AlertReport GetReport()
		{
			if (Alert_NoBabyFeeders.MapWithNoBabyFeeder(Alert_NoBabyFeeders.tmpPawnList) != null)
			{
				return AlertReport.CulpritsAre(Alert_NoBabyFeeders.tmpPawnList);
			}
			return false;
		}

		// Token: 0x0600B649 RID: 46665 RVA: 0x00402DEC File Offset: 0x00400FEC
		private static Map MapWithNoBabyFeeder(List<Pawn> babiesOut)
		{
			if (babiesOut != null)
			{
				babiesOut.Clear();
			}
			foreach (Map map in Find.Maps)
			{
				List<Pawn> freeColonistsAndPrisonersSpawned = map.mapPawns.FreeColonistsAndPrisonersSpawned;
				for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
				{
					Pawn pawn = freeColonistsAndPrisonersSpawned[i];
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
					{
						if (!pawn.Spawned)
						{
							IThingHolder parentHolder = pawn.ParentHolder;
							if (parentHolder != null && !(parentHolder is Pawn_CarryTracker))
							{
								goto IL_DA;
							}
						}
						if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, delegate(Pawn _baby, Pawn _mom)
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
							return ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
						}, null))
						{
							if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, delegate(Pawn _baby, Pawn _mom)
							{
								ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
								return ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
							}, null))
							{
								if (babiesOut == null)
								{
									return map;
								}
								babiesOut.Add(pawn);
							}
						}
					}
					IL_DA:;
				}
				if (babiesOut != null && babiesOut.Count > 0)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x040068B5 RID: 26805
		private static List<Pawn> tmpPawnList = new List<Pawn>(32);
	}
}
