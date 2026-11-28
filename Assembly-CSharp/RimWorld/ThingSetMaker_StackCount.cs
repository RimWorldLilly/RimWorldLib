using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B68 RID: 7016
	public class ThingSetMaker_StackCount : ThingSetMaker
	{
		// Token: 0x0600B416 RID: 46102 RVA: 0x003F7A1C File Offset: 0x003F5C1C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.AllowedThingDefs(parms).Any<ThingDef>() && (parms.countRange == null || parms.countRange.Value.max > 0) && (parms.maxTotalMass == null || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.GetValueOrDefault().max : 1));
		}

		// Token: 0x0600B417 RID: 46103 RVA: 0x003F7AC4 File Offset: 0x003F5CC4
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
			int num3 = num;
			ThingStuffPair thingStuffPair;
			while (num3 > 0 && ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, valueOrDefault, (valueOrDefault2 == 3.4028235E+38f) ? 3.4028235E+38f : (valueOrDefault2 - num2), parms.qualityGenerator, out thingStuffPair))
			{
				Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
				int num4 = num3;
				if (valueOrDefault2 != 3.4028235E+38f && !(thing is Pawn))
				{
					num4 = Mathf.Min(num4, Mathf.FloorToInt((valueOrDefault2 - num2) / thing.GetStatValue(StatDefOf.Mass, true, -1)));
				}
				num4 = Mathf.Clamp(num4, 1, thing.def.stackLimit);
				thing.stackCount = num4;
				num3 -= num4;
				outThings.Add(thing);
				if (!(thing is Pawn))
				{
					num2 += thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)thing.stackCount;
				}
			}
		}

		// Token: 0x0600B418 RID: 46104 RVA: 0x003F7C16 File Offset: 0x003F5E16
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600B419 RID: 46105 RVA: 0x003F7C1E File Offset: 0x003F5E1E
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_StackCount.<AllGeneratableThingsDebugSub>d__3 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_StackCount.<AllGeneratableThingsDebugSub>d__3(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}
	}
}
