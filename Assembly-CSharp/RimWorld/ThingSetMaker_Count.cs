using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B64 RID: 7012
	public class ThingSetMaker_Count : ThingSetMaker
	{
		// Token: 0x0600B3FC RID: 46076 RVA: 0x003F707C File Offset: 0x003F527C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.AllowedThingDefs(parms).Any<ThingDef>() && (parms.countRange == null || parms.countRange.Value.max > 0) && (parms.maxTotalMass == null || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.GetValueOrDefault().max : 1));
		}

		// Token: 0x0600B3FD RID: 46077 RVA: 0x003F7124 File Offset: 0x003F5324
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				return;
			}
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			IntRange intRange = parms.countRange ?? IntRange.One;
			float valueOrDefault2 = parms.maxTotalMass.GetValueOrDefault(float.MaxValue);
			int num = Mathf.Max(intRange.RandomInRange, 1);
			float num2 = 0f;
			int num3 = 0;
			ThingStuffPair thingStuffPair;
			while (num3 < num && ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, valueOrDefault, (valueOrDefault2 == 3.4028235E+38f) ? 3.4028235E+38f : (valueOrDefault2 - num2), parms.qualityGenerator, out thingStuffPair))
			{
				Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
				outThings.Add(thing);
				if (!(thing is Pawn))
				{
					num2 += thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)thing.stackCount;
				}
				num3++;
			}
		}

		// Token: 0x0600B3FE RID: 46078 RVA: 0x003F721B File Offset: 0x003F541B
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600B3FF RID: 46079 RVA: 0x003F7223 File Offset: 0x003F5423
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_Count.<AllGeneratableThingsDebugSub>d__3 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_Count.<AllGeneratableThingsDebugSub>d__3(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}
	}
}
