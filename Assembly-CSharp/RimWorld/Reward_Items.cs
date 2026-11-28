using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3C RID: 7996
	public class Reward_Items : Reward
	{
		// Token: 0x17002531 RID: 9521
		// (get) Token: 0x0600CBFB RID: 52219 RVA: 0x004B2A2B File Offset: 0x004B0C2B
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17002532 RID: 9522
		// (get) Token: 0x0600CBFC RID: 52220 RVA: 0x004B2A33 File Offset: 0x004B0C33
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_Items.<get_StackElements>d__8 <get_StackElements>d__ = new Reward_Items.<get_StackElements>d__8(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x17002533 RID: 9523
		// (get) Token: 0x0600CBFD RID: 52221 RVA: 0x004B2A44 File Offset: 0x004B0C44
		public override float TotalMarketValue
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					return this.lastTotalMarketValue;
				}
				float num = 0f;
				for (int i = 0; i < this.items.Count; i++)
				{
					Thing innerIfMinified = this.items[i].GetInnerIfMinified();
					num += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0600CBFE RID: 52222 RVA: 0x004B2AAB File Offset: 0x004B0CAB
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		// Token: 0x0600CBFF RID: 52223 RVA: 0x004B2AB9 File Offset: 0x004B0CB9
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		// Token: 0x0600CC00 RID: 52224 RVA: 0x004B2AC8 File Offset: 0x004B0CC8
		private void RememberItems()
		{
			if (this.usedOrCleanedUp)
			{
				return;
			}
			this.itemDefs.Clear();
			this.lastTotalMarketValue = 0f;
			for (int i = 0; i < this.items.Count; i++)
			{
				Thing innerIfMinified = this.items[i].GetInnerIfMinified();
				if (innerIfMinified != null && !innerIfMinified.Destroyed)
				{
					QualityCategory quality;
					if (!innerIfMinified.TryGetQuality(out quality))
					{
						quality = QualityCategory.Normal;
					}
					this.itemDefs.Add(new Reward_Items.RememberedItem(new ThingStuffPairWithQuality(innerIfMinified.def, innerIfMinified.Stuff, quality), this.items[i].stackCount, this.items[i].LabelNoCount));
					this.lastTotalMarketValue += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
			}
		}

		// Token: 0x0600CC01 RID: 52225 RVA: 0x004B2BA4 File Offset: 0x004B0DA4
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.items.Clear();
			bool flag = true;
			float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
			if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && (Faction.OfEmpire == null || parms.giverFaction != Faction.OfEmpire))
			{
				this.items.Add(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null));
				rewardValue -= this.items[0].MarketValue;
				if (rewardValue < 100f)
				{
					flag = false;
				}
			}
			if (flag)
			{
				FloatRange value = rewardValue * new FloatRange(0.7f, 1.3f);
				ThingSetMakerParams parms2 = default(ThingSetMakerParams);
				parms2.totalMarketValueRange = new FloatRange?(value);
				parms2.makingFaction = parms.giverFaction;
				if (!parms.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					parms2.validator = ((ThingDef x) => !parms.disallowedThingDefs.Contains(x));
				}
				this.items.AddRange(ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms2));
			}
			valueActuallyUsed = this.TotalMarketValue;
		}

		// Token: 0x0600CC02 RID: 52226 RVA: 0x004B2D05 File Offset: 0x004B0F05
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Reward_Items.<GenerateQuestParts>d__15 <GenerateQuestParts>d__ = new Reward_Items.<GenerateQuestParts>d__15(-2);
			<GenerateQuestParts>d__.<>4__this = this;
			<GenerateQuestParts>d__.<>3__parms = parms;
			<GenerateQuestParts>d__.<>3__customLetterLabel = customLetterLabel;
			<GenerateQuestParts>d__.<>3__customLetterText = customLetterText;
			<GenerateQuestParts>d__.<>3__customLetterLabelRules = customLetterLabelRules;
			<GenerateQuestParts>d__.<>3__customLetterTextRules = customLetterTextRules;
			return <GenerateQuestParts>d__;
		}

		// Token: 0x0600CC03 RID: 52227 RVA: 0x004B2D3C File Offset: 0x004B0F3C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		// Token: 0x0600CC04 RID: 52228 RVA: 0x004B2DBC File Offset: 0x004B0FBC
		public override string ToString()
		{
			string text = base.GetType().Name;
			text = text + "(value " + this.TotalMarketValue.ToStringMoney(null) + ")";
			foreach (Thing thing in this.items)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n  -",
					thing.LabelCap,
					" ",
					(thing.MarketValue * (float)thing.stackCount).ToStringMoney(null)
				});
			}
			return text;
		}

		// Token: 0x0600CC05 RID: 52229 RVA: 0x004B2E74 File Offset: 0x004B1074
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Reward_Items.RememberedItem>(ref this.itemDefs, "itemDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.lastTotalMarketValue, "lastTotalMarketValue", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04008200 RID: 33280
		public List<Thing> items = new List<Thing>();

		// Token: 0x04008201 RID: 33281
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		// Token: 0x04008202 RID: 33282
		private float lastTotalMarketValue;

		// Token: 0x04008203 RID: 33283
		private const string RootSymbol = "root";

		// Token: 0x02003BE7 RID: 15335
		public struct RememberedItem : IExposable
		{
			// Token: 0x06015731 RID: 87857 RVA: 0x006C694F File Offset: 0x006C4B4F
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			// Token: 0x06015732 RID: 87858 RVA: 0x006C6966 File Offset: 0x006C4B66
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			// Token: 0x0400ED44 RID: 60740
			public ThingStuffPairWithQuality thing;

			// Token: 0x0400ED45 RID: 60741
			public int stackCount;

			// Token: 0x0400ED46 RID: 60742
			public string label;
		}
	}
}
