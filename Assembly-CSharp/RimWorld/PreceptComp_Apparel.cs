using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001630 RID: 5680
	public class PreceptComp_Apparel : PreceptComp
	{
		// Token: 0x06008E75 RID: 36469 RVA: 0x00345E40 File Offset: 0x00344040
		public Gender AffectedGender(Ideo ideo)
		{
			switch (this.gender)
			{
			case IdeoApparelGender.Any:
				return Gender.None;
			case IdeoApparelGender.SupremeGender:
				return ideo.SupremeGender;
			case IdeoApparelGender.SubordinateGender:
				return ideo.SupremeGender.Opposite();
			default:
				Log.Error("Unimplemented gender: " + this.gender.ToString());
				return Gender.None;
			}
		}

		// Token: 0x06008E76 RID: 36470 RVA: 0x00345E9E File Offset: 0x0034409E
		public virtual bool CanApplyToApparel(ThingDef apparelDef)
		{
			return true;
		}

		// Token: 0x06008E77 RID: 36471 RVA: 0x00345EA4 File Offset: 0x003440A4
		protected bool AppliesToPawn(Pawn pawn, Precept precept)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null)
				{
					Gender targetGender = precept_Apparel.TargetGender;
					if (targetGender != Gender.None && pawn.gender != targetGender)
					{
						return false;
					}
					if (pawn.royalty != null)
					{
						foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
						{
							if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								using (List<ApparelRequirement>.Enumerator enumerator2 = royalTitle.def.requiredApparel.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										string text;
										if (ApparelUtility.IsRequirementActive(enumerator2.Current, ApparelRequirementSource.Title, pawn, out text))
										{
											return false;
										}
									}
								}
							}
						}
					}
					using (List<Apparel>.Enumerator enumerator3 = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.def == precept_Apparel.apparelDef)
							{
								return false;
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008E78 RID: 36472 RVA: 0x00345FE8 File Offset: 0x003441E8
		protected void GiveApparelToPawn(Pawn pawn, Precept_Apparel precept)
		{
			Apparel apparel = PawnApparelGenerator.GenerateApparelOfDefFor(pawn, precept.apparelDef);
			if (apparel != null && apparel.PawnCanWear(pawn, false))
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
				if (!pawn.kindDef.ignoreIdeoApparelColors)
				{
					apparel.SetColor(pawn.Ideo.ApparelColor, false);
				}
				pawn.apparel.Wear(apparel, false, false);
			}
		}

		// Token: 0x04005754 RID: 22356
		private IdeoApparelGender gender;
	}
}
