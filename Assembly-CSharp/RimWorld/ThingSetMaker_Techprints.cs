using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B70 RID: 7024
	public class ThingSetMaker_Techprints : ThingSetMaker
	{
		// Token: 0x0600B44B RID: 46155 RVA: 0x003F8CEC File Offset: 0x003F6EEC
		public override float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			if (!this.weightAccordingToPlayerNeeds)
			{
				return 1f;
			}
			int num = 0;
			bool flag = false;
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (!researchProjectDef.IsFinished && researchProjectDef.PrerequisitesCompleted)
				{
					if (!researchProjectDef.TechprintRequirementMet && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(researchProjectDef.Techprint, researchProjectDef.TechprintCount - researchProjectDef.TechprintsApplied))
					{
						flag = true;
					}
					else
					{
						num++;
					}
				}
			}
			if (!flag)
			{
				return 1f;
			}
			return (float)Mathf.RoundToInt(ThingSetMaker_Techprints.ResearchableProjectsCountToSelectionWeightCurve.Evaluate((float)num));
		}

		// Token: 0x0600B44C RID: 46156 RVA: 0x003F8D98 File Offset: 0x003F6F98
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			ThingDef thingDef;
			return (parms.countRange == null || parms.countRange.Value.max > 0) && TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef, null, (parms.totalMarketValueRange == null) ? float.MaxValue : (parms.totalMarketValueRange.Value.max * this.marketValueFactor), true);
		}

		// Token: 0x0600B44D RID: 46157 RVA: 0x003F8E08 File Offset: 0x003F7008
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_Techprints.tmpGenerated.Clear();
			ThingDef thingDef3;
			if (parms.countRange != null)
			{
				int num = Mathf.Max(parms.countRange.Value.RandomInRange, 1);
				for (int i = 0; i < num; i++)
				{
					ThingDef thingDef;
					if (!TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f, this.weightAccordingToPlayerNeeds))
					{
						break;
					}
					ThingSetMaker_Techprints.tmpGenerated.Add(thingDef);
					outThings.Add(ThingMaker.MakeThing(thingDef, null));
				}
			}
			else if (parms.totalMarketValueRange != null)
			{
				float num2 = parms.totalMarketValueRange.Value.RandomInRange * this.marketValueFactor;
				float num3 = 0f;
				ThingDef thingDef2;
				while (TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef2, ThingSetMaker_Techprints.tmpGenerated, num2 - num3, this.weightAccordingToPlayerNeeds) || (!ThingSetMaker_Techprints.tmpGenerated.Any<ThingDef>() && TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef2, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f, this.weightAccordingToPlayerNeeds)))
				{
					ThingSetMaker_Techprints.tmpGenerated.Add(thingDef2);
					outThings.Add(ThingMaker.MakeThing(thingDef2, null));
					num3 += thingDef2.BaseMarketValue;
				}
			}
			else if (TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef3, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f, this.weightAccordingToPlayerNeeds))
			{
				ThingSetMaker_Techprints.tmpGenerated.Add(thingDef3);
				outThings.Add(ThingMaker.MakeThing(thingDef3, null));
			}
			ThingSetMaker_Techprints.tmpGenerated.Clear();
		}

		// Token: 0x0600B44E RID: 46158 RVA: 0x003F8F81 File Offset: 0x003F7181
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp(typeof(CompTechprint))
			select x;
		}

		// Token: 0x040067DA RID: 26586
		public bool weightAccordingToPlayerNeeds = true;

		// Token: 0x040067DB RID: 26587
		private float marketValueFactor = 1f;

		// Token: 0x040067DC RID: 26588
		private static readonly SimpleCurve ResearchableProjectsCountToSelectionWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 1f),
				true
			},
			{
				new CurvePoint(0f, 5f),
				true
			}
		};

		// Token: 0x040067DD RID: 26589
		private static List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
