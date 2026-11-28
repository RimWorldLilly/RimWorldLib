using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B61 RID: 7009
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x0600B3EB RID: 46059 RVA: 0x003F6994 File Offset: 0x003F4B94
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].thingSetMaker.CanGenerate(parms) && this.GetSelectionWeight(this.options[i], parms) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B3EC RID: 46060 RVA: 0x003F69F0 File Offset: 0x003F4BF0
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_RandomOption.Option option;
			if (!(from x in this.options
			where x.thingSetMaker.CanGenerate(parms)
			select x).TryRandomElementByWeight((ThingSetMaker_RandomOption.Option x) => this.GetSelectionWeight(x, parms), out option))
			{
				return;
			}
			outThings.AddRange(option.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600B3ED RID: 46061 RVA: 0x003F6A58 File Offset: 0x003F4C58
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option, ThingSetMakerParams parms)
		{
			if (option.weightIfPlayerHasNoItem != null && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem, 1))
			{
				return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			if (ModsConfig.BiotechActive && option.weightIfPlayerHasXenotype != null && option.weightIfPlayerHasXenotypeXenotype != null && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype(option.weightIfPlayerHasXenotypeXenotype).Count > 0)
			{
				return option.weightIfPlayerHasXenotype.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}

		// Token: 0x0600B3EE RID: 46062 RVA: 0x003F6AF4 File Offset: 0x003F4CF4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600B3EF RID: 46063 RVA: 0x003F6B33 File Offset: 0x003F4D33
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_RandomOption.<AllGeneratableThingsDebugSub>d__6 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_RandomOption.<AllGeneratableThingsDebugSub>d__6(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}

		// Token: 0x0600B3F0 RID: 46064 RVA: 0x003F6B4A File Offset: 0x003F4D4A
		public override IEnumerable<string> ConfigErrors()
		{
			ThingSetMaker_RandomOption.<ConfigErrors>d__7 <ConfigErrors>d__ = new ThingSetMaker_RandomOption.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040067BF RID: 26559
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x0200385A RID: 14426
		public class Option
		{
			// Token: 0x0400E0F0 RID: 57584
			public ThingSetMaker thingSetMaker;

			// Token: 0x0400E0F1 RID: 57585
			public float weight;

			// Token: 0x0400E0F2 RID: 57586
			public float? weightIfPlayerHasNoItem;

			// Token: 0x0400E0F3 RID: 57587
			public ThingDef weightIfPlayerHasNoItemItem;

			// Token: 0x0400E0F4 RID: 57588
			public float? weightIfPlayerHasXenotype;

			// Token: 0x0400E0F5 RID: 57589
			public XenotypeDef weightIfPlayerHasXenotypeXenotype;
		}
	}
}
