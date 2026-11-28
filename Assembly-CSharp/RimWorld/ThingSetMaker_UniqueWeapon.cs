using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B72 RID: 7026
	public class ThingSetMaker_UniqueWeapon : ThingSetMaker
	{
		// Token: 0x0600B456 RID: 46166 RVA: 0x003F939C File Offset: 0x003F759C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return ModsConfig.OdysseyActive && (parms.countRange == null || parms.countRange.Value.max > 0) && (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max > 0f) && this.AllGeneratableThingsDebugSub(parms).Any<ThingDef>();
		}

		// Token: 0x0600B457 RID: 46167 RVA: 0x003F940C File Offset: 0x003F760C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = (parms.countRange != null) ? parms.countRange.GetValueOrDefault().RandomInRange : 1;
			if (parms.countRange != null)
			{
				num = Mathf.Max(parms.countRange.Value.RandomInRange, num);
			}
			FloatRange floatRange = parms.totalMarketValueRange ?? new FloatRange(0f, float.MaxValue);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				bool flag = i == num - 1;
				int num3 = 999;
				Thing thing;
				do
				{
					thing = ThingMaker.MakeThing((from x in DefDatabase<ThingDef>.AllDefs
					where x.HasComp<CompUniqueWeapon>()
					select x).RandomElement<ThingDef>(), null);
				}
				while (num3-- > 0 && (num2 + thing.MarketValue > floatRange.max || (flag && num2 + thing.MarketValue < floatRange.min)));
				if (num3 <= 0)
				{
					break;
				}
				num2 += thing.MarketValue;
				outThings.Add(thing);
			}
		}

		// Token: 0x0600B458 RID: 46168 RVA: 0x003F9536 File Offset: 0x003F7736
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp<CompUniqueWeapon>()
			select x;
		}
	}
}
