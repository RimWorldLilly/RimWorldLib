using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6B RID: 7019
	public class ThingSetMaker_Books : ThingSetMaker
	{
		// Token: 0x0600B42C RID: 46124 RVA: 0x003F82A0 File Offset: 0x003F64A0
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			ThingDef thingDef;
			return (parms.countRange == null || parms.countRange.Value.max > 0) && TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(parms.makingFaction, out thingDef, null, (((parms.totalMarketValueRange != null) ? new float?(parms.totalMarketValueRange.GetValueOrDefault().max) : null) * this.marketValueFactor).GetValueOrDefault(float.MaxValue), true);
		}

		// Token: 0x0600B42D RID: 46125 RVA: 0x003F8344 File Offset: 0x003F6544
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 1;
			if (parms.countRange != null)
			{
				num = Mathf.Max(parms.countRange.Value.RandomInRange, num);
			}
			for (int i = 0; i < num; i++)
			{
				outThings.Add(BookUtility.MakeBook(ArtGenerationContext.Outsider, parms.qualityGenerator));
			}
		}

		// Token: 0x0600B42E RID: 46126 RVA: 0x003F839A File Offset: 0x003F659A
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp<CompBook>()
			select x;
		}

		// Token: 0x040067D0 RID: 26576
		private float marketValueFactor = 1f;
	}
}
