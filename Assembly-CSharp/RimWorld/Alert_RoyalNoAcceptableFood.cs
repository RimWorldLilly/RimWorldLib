using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001BE7 RID: 7143
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		// Token: 0x0600B6C9 RID: 46793 RVA: 0x00406714 File Offset: 0x00404914
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x1700210A RID: 8458
		// (get) Token: 0x0600B6CA RID: 46794 RVA: 0x00406770 File Offset: 0x00404970
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						if (pawn.Spawned && (pawn.story == null || !pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
						{
							Pawn_RoyaltyTracker royalty = pawn.royalty;
							RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
							if (royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined)
							{
								bool flag = false;
								foreach (KeyValuePair<Alert_RoyalNoAcceptableFood.CachedResult, bool> keyValuePair in this.cachedResults)
								{
									Alert_RoyalNoAcceptableFood.CachedResult cachedResult;
									bool flag2;
									keyValuePair.Deconstruct(out cachedResult, out flag2);
									Alert_RoyalNoAcceptableFood.CachedResult cachedResult2 = cachedResult;
									bool flag3 = flag2;
									RoyalTitleFoodRequirement requirement = cachedResult2.requirement;
									if (requirement.Equals(royalTitle.def.foodRequirement) && pawn.CanReach(cachedResult2.cell, PathEndMode.ClosestTouch, Danger.Unspecified, false, false, TraverseMode.ByPawn))
									{
										flag = true;
										if (!flag3)
										{
											this.targetsResult.Add(pawn);
											break;
										}
										break;
									}
								}
								if (!flag)
								{
									Alert_RoyalNoAcceptableFood.CachedResult key = new Alert_RoyalNoAcceptableFood.CachedResult(royalTitle.def.foodRequirement, pawn.Position);
									Thing thing;
									ThingDef thingDef;
									this.cachedResults[key] = FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, false, false, false, false, true, false, false, FoodPreferability.DesperateOnly);
									if (!this.cachedResults[key])
									{
										this.targetsResult.Add(pawn);
									}
								}
							}
						}
					}
				}
				this.cachedResults.Clear();
				return this.targetsResult;
			}
		}

		// Token: 0x0600B6CB RID: 46795 RVA: 0x00406990 File Offset: 0x00404B90
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B6CC RID: 46796 RVA: 0x004069A0 File Offset: 0x00404BA0
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle mostSeniorTitle = t.royalty.MostSeniorTitle;
				string[] array = new string[5];
				array[0] = t.LabelShort;
				array[1] = " (";
				array[2] = mostSeniorTitle.def.GetLabelFor(t.gender);
				array[3] = "):\n";
				array[4] = (from m in mostSeniorTitle.def.SatisfyingMeals(false)
				select m.LabelCap).ToLineList("  - ", false);
				string text = string.Concat(array);
				if (ModsConfig.IdeologyActive && t.Ideo != null && t.Ideo.VeneratedAnimals.Any<ThingDef>())
				{
					text = text + "\n\n" + "AlertRoyalTitleNoVeneratedAnimalMeat".Translate(t.Named("PAWN"), t.Ideo.Named("IDEO"), (from x in t.Ideo.VeneratedAnimals
					select x.label).ToCommaList(false, false).Named("ANIMALS")).Resolve();
				}
				return text;
			}).ToLineList("\n", false);
		}

		// Token: 0x040068EE RID: 26862
		private readonly List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x040068EF RID: 26863
		private readonly Dictionary<Alert_RoyalNoAcceptableFood.CachedResult, bool> cachedResults = new Dictionary<Alert_RoyalNoAcceptableFood.CachedResult, bool>();

		// Token: 0x020038B8 RID: 14520
		private struct CachedResult
		{
			// Token: 0x06014B10 RID: 84752 RVA: 0x006A07FB File Offset: 0x0069E9FB
			public CachedResult(RoyalTitleFoodRequirement requirement, IntVec3 cell)
			{
				this.requirement = requirement;
				this.cell = cell;
			}

			// Token: 0x0400E29E RID: 58014
			public RoyalTitleFoodRequirement requirement;

			// Token: 0x0400E29F RID: 58015
			public IntVec3 cell;
		}
	}
}
