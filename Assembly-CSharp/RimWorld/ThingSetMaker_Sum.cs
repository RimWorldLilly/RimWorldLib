using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B63 RID: 7011
	public class ThingSetMaker_Sum : ThingSetMaker
	{
		// Token: 0x0600B3F6 RID: 46070 RVA: 0x003F6BAC File Offset: 0x003F4DAC
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f && this.options[i].thingSetMaker.CanGenerate(parms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B3F7 RID: 46071 RVA: 0x003F6C04 File Offset: 0x003F4E04
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			this.optionsInResolveOrder.Clear();
			this.optionsInResolveOrder.AddRange(this.options);
			if (!this.resolveInOrder)
			{
				this.optionsInResolveOrder.Shuffle<ThingSetMaker_Sum.Option>();
			}
			int i = 0;
			while (i < this.optionsInResolveOrder.Count)
			{
				ThingSetMakerParams parms2 = parms;
				if (parms2.countRange == null)
				{
					goto IL_B0;
				}
				parms2.countRange = new IntRange?(new IntRange(Mathf.Max(parms2.countRange.Value.min - num, 0), parms2.countRange.Value.max - num));
				if (parms2.countRange.Value.max > 0)
				{
					goto IL_B0;
				}
				IL_3D6:
				i++;
				continue;
				IL_B0:
				if (parms2.totalMarketValueRange != null)
				{
					if (parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minTotalMarketValue)
					{
						goto IL_3D6;
					}
					float min = (i < this.optionsInResolveOrder.Count - 1) ? 0f : Mathf.Max(parms2.totalMarketValueRange.Value.min - num2, 0f);
					float max = parms2.totalMarketValueRange.Value.max - num2;
					parms2.totalMarketValueRange = new FloatRange?(new FloatRange(min, max));
					if (this.optionsInResolveOrder[i].maxMarketValue != null)
					{
						parms2.totalMarketValueRange = new FloatRange?(new FloatRange(Mathf.Min(parms2.totalMarketValueRange.Value.min, this.optionsInResolveOrder[i].maxMarketValue.Value * 0.8f), Mathf.Min(new float[]
						{
							Mathf.Min(parms2.totalMarketValueRange.Value.max, this.optionsInResolveOrder[i].maxMarketValue.Value)
						})));
					}
					if (parms2.totalMarketValueRange.Value.max <= 0f || parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minMarketValue)
					{
						goto IL_3D6;
					}
				}
				if (parms2.totalNutritionRange != null)
				{
					parms2.totalNutritionRange = new FloatRange?(new FloatRange(Mathf.Max(parms2.totalNutritionRange.Value.min - num3, 0f), parms2.totalNutritionRange.Value.max - num3));
				}
				if (parms2.maxTotalMass != null)
				{
					parms2.maxTotalMass -= num4;
				}
				if (Rand.Chance(this.optionsInResolveOrder[i].chance) && this.optionsInResolveOrder[i].thingSetMaker.CanGenerate(parms2))
				{
					List<Thing> list = this.optionsInResolveOrder[i].thingSetMaker.Generate(parms2);
					num += list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						num2 += list[j].MarketValue * (float)list[j].stackCount;
						if (list[j].def.IsIngestible)
						{
							num3 += list[j].GetStatValue(StatDefOf.Nutrition, true, -1) * (float)list[j].stackCount;
						}
						if (!(list[j] is Pawn))
						{
							num4 += list[j].GetStatValue(StatDefOf.Mass, true, -1) * (float)list[j].stackCount;
						}
					}
					outThings.AddRange(list);
					goto IL_3D6;
				}
				goto IL_3D6;
			}
		}

		// Token: 0x0600B3F8 RID: 46072 RVA: 0x003F7000 File Offset: 0x003F5200
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x003F703F File Offset: 0x003F523F
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_Sum.<AllGeneratableThingsDebugSub>d__7 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_Sum.<AllGeneratableThingsDebugSub>d__7(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x003F7056 File Offset: 0x003F5256
		public override IEnumerable<string> ConfigErrors()
		{
			ThingSetMaker_Sum.<ConfigErrors>d__8 <ConfigErrors>d__ = new ThingSetMaker_Sum.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040067C1 RID: 26561
		public List<ThingSetMaker_Sum.Option> options;

		// Token: 0x040067C2 RID: 26562
		public bool resolveInOrder;

		// Token: 0x040067C3 RID: 26563
		private List<ThingSetMaker_Sum.Option> optionsInResolveOrder = new List<ThingSetMaker_Sum.Option>();

		// Token: 0x0200385E RID: 14430
		public class Option
		{
			// Token: 0x0400E106 RID: 57606
			public ThingSetMaker thingSetMaker;

			// Token: 0x0400E107 RID: 57607
			public float chance = 1f;

			// Token: 0x0400E108 RID: 57608
			public float? maxMarketValue;

			// Token: 0x0400E109 RID: 57609
			public float minMarketValue;

			// Token: 0x0400E10A RID: 57610
			public float minTotalMarketValue;

			// Token: 0x0400E10B RID: 57611
			public const float MaxMarketValueLeewayFactor = 0.8f;
		}
	}
}
