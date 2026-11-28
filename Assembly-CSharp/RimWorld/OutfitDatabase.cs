using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001022 RID: 4130
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x06006801 RID: 26625 RVA: 0x0024BC58 File Offset: 0x00249E58
		public List<ApparelPolicy> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x0024BC60 File Offset: 0x00249E60
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x06006803 RID: 26627 RVA: 0x0024BC79 File Offset: 0x00249E79
		public void ExposeData()
		{
			Scribe_Collections.Look<ApparelPolicy>(ref this.outfits, "outfits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x0024BC91 File Offset: 0x00249E91
		public ApparelPolicy DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x0024BCB4 File Offset: 0x00249EB4
		public void SetDefault(ApparelPolicy policy)
		{
			int index = this.outfits.IndexOf(policy);
			ApparelPolicy value = this.outfits[0];
			this.outfits[0] = policy;
			this.outfits[index] = value;
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x0024BCF8 File Offset: 0x00249EF8
		public AcceptanceReport TryDelete(ApparelPolicy apparelPolicy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.outfits != null && pawn.outfits.CurrentApparelPolicy == apparelPolicy)
				{
					return new AcceptanceReport("OutfitInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.outfits != null && pawn2.outfits.CurrentApparelPolicy == apparelPolicy)
				{
					pawn2.outfits.CurrentApparelPolicy = null;
				}
			}
			this.outfits.Remove(apparelPolicy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006807 RID: 26631 RVA: 0x0024BDE8 File Offset: 0x00249FE8
		public ApparelPolicy MakeNewOutfit()
		{
			int num;
			if (!this.outfits.Any<ApparelPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.outfits.Max((ApparelPolicy o) => o.id) + 1;
			}
			int id = num;
			ApparelPolicy apparelPolicy = new ApparelPolicy(id, "ApparelPolicy".Translate() + " " + id.ToString());
			apparelPolicy.filter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			this.outfits.Add(apparelPolicy);
			return apparelPolicy;
		}

		// Token: 0x06006808 RID: 26632 RVA: 0x0024BE80 File Offset: 0x0024A080
		private void GenerateStartingOutfits()
		{
			this.MakeNewOutfit().label = "OutfitAnything".Translate();
			ApparelPolicy apparelPolicy = this.MakeNewOutfit();
			apparelPolicy.label = "OutfitWorker".Translate();
			apparelPolicy.filter.SetDisallowAll(null, null);
			apparelPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.apparel != null && ((thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker")) || thingDef.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
				{
					apparelPolicy.filter.SetAllow(thingDef, true);
				}
			}
			ApparelPolicy apparelPolicy2 = this.MakeNewOutfit();
			apparelPolicy2.label = "OutfitSoldier".Translate();
			apparelPolicy2.filter.SetDisallowAll(null, null);
			apparelPolicy2.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.apparel != null && ((thingDef2.apparel.defaultOutfitTags != null && thingDef2.apparel.defaultOutfitTags.Contains("Soldier")) || thingDef2.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
				{
					apparelPolicy2.filter.SetAllow(thingDef2, true);
				}
			}
			ApparelPolicy apparelPolicy3 = this.MakeNewOutfit();
			apparelPolicy3.label = "OutfitNudist".Translate();
			apparelPolicy3.filter.SetDisallowAll(null, null);
			apparelPolicy3.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.apparel != null && (thingDef3.apparel.defaultOutfitTags.NotNullAndContains("Nudist") || (!thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))))
				{
					apparelPolicy3.filter.SetAllow(thingDef3, true);
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				ApparelPolicy apparelPolicy4 = this.MakeNewOutfit();
				apparelPolicy4.label = "OutfitSlave".Translate();
				apparelPolicy4.filter.SetDisallowAll(null, null);
				apparelPolicy4.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
				foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef4.apparel != null && thingDef4.apparel.defaultOutfitTags != null && thingDef4.apparel.defaultOutfitTags.Contains("Slave"))
					{
						apparelPolicy4.filter.SetAllow(thingDef4, true);
					}
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				ApparelPolicy apparelPolicy5 = this.MakeNewOutfit();
				apparelPolicy5.label = "OutfitSpacefarer".Translate();
				apparelPolicy5.filter.SetDisallowAll(null, null);
				apparelPolicy5.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
				foreach (ThingDef thingDef5 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef5.apparel != null && ((thingDef5.apparel.defaultOutfitTags != null && thingDef5.apparel.defaultOutfitTags.Contains("Spacefarer")) || thingDef5.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
					{
						apparelPolicy5.filter.SetAllow(thingDef5, true);
					}
				}
			}
		}

		// Token: 0x0400424C RID: 16972
		private List<ApparelPolicy> outfits = new List<ApparelPolicy>();
	}
}
