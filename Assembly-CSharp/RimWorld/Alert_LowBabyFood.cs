using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC7 RID: 7111
	public class Alert_LowBabyFood : Alert
	{
		// Token: 0x0600B642 RID: 46658 RVA: 0x00402A6C File Offset: 0x00400C6C
		public Alert_LowBabyFood()
		{
			this.defaultLabel = "AlertLowBabyFood".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
			this.babyFoodList = (from def in DefDatabase<ThingDef>.AllDefs
			where def.IsNutritionGivingIngestible && def.ingestible.HumanEdible && def.ingestible.babiesCanIngest
			select def.label).ToCommaListOr(false);
		}

		// Token: 0x0600B643 RID: 46659 RVA: 0x00402B14 File Offset: 0x00400D14
		public override TaggedString GetExplanation()
		{
			float? num;
			if (!this.LowBabyFoodNutrition(this.babiesOutExplanation, out num))
			{
				return string.Empty;
			}
			return "AlertLowBabyFoodDesc".Translate(this.babiesOutExplanation.Count.ToStringCached(), Mathf.FloorToInt(num.Value / (0.25f * (float)this.babiesOutExplanation.Count)).ToStringCached(), this.babyFoodList);
		}

		// Token: 0x0600B644 RID: 46660 RVA: 0x00402B90 File Offset: 0x00400D90
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 150000f)
			{
				return false;
			}
			float? num;
			return new AlertReport
			{
				active = this.LowBabyFoodNutrition(this.babiesOutReport, out num),
				culpritsPawns = this.babiesOutReport
			};
		}

		// Token: 0x0600B645 RID: 46661 RVA: 0x00402BE4 File Offset: 0x00400DE4
		private bool LowBabyFoodNutrition(List<Pawn> lowFoodBabiesOut, out float? babyFoodNutrition)
		{
			foreach (Map map in Find.Maps)
			{
				using (new ProfilerBlock("Populate lowFoodBabiesOut"))
				{
					lowFoodBabiesOut.Clear();
					List<Pawn> freeColonistsAndPrisoners;
					using (new ProfilerBlock("FreeColonistsAndPrisoners"))
					{
						freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < freeColonistsAndPrisoners.Count; i++)
					{
						Pawn pawn = freeColonistsAndPrisoners[i];
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason) && pawn.SpawnedParentOrMe is Pawn)
						{
							Predicate<Pawn, Pawn> feederPredicate = delegate(Pawn _baby, Pawn _mom)
							{
								ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
								return ChildcareUtility.CanMomBreastfeedBaby(_mom, _baby, out breastfeedFailReason2) && ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
							};
							if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, feederPredicate, null) && !pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, feederPredicate, null))
							{
								lowFoodBabiesOut.Add(pawn);
							}
						}
					}
				}
				if (lowFoodBabiesOut.Count > 0)
				{
					float totalHumanBabyEdibleNutrition = map.resourceCounter.TotalHumanBabyEdibleNutrition;
					if (totalHumanBabyEdibleNutrition < 1f * (float)lowFoodBabiesOut.Count)
					{
						babyFoodNutrition = new float?(totalHumanBabyEdibleNutrition);
						return true;
					}
				}
			}
			babyFoodNutrition = null;
			return false;
		}

		// Token: 0x040068B0 RID: 26800
		private const float NutritionPerDayPerBaby = 0.25f;

		// Token: 0x040068B1 RID: 26801
		private const float NutritionThresholdPerBaby = 1f;

		// Token: 0x040068B2 RID: 26802
		private List<Pawn> babiesOutReport = new List<Pawn>(8);

		// Token: 0x040068B3 RID: 26803
		private List<Pawn> babiesOutExplanation = new List<Pawn>(8);

		// Token: 0x040068B4 RID: 26804
		private string babyFoodList;
	}
}
