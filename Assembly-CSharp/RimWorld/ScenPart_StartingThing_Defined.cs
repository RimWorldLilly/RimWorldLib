using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BF RID: 6079
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600979D RID: 38813 RVA: 0x0036FC52 File Offset: 0x0036DE52
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x0600979E RID: 38814 RVA: 0x0036FC63 File Offset: 0x0036DE63
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600979F RID: 38815 RVA: 0x0036FC75 File Offset: 0x0036DE75
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_StartingThing_Defined.<GetSummaryListEntries>d__4 <GetSummaryListEntries>d__ = new ScenPart_StartingThing_Defined.<GetSummaryListEntries>d__4(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x060097A0 RID: 38816 RVA: 0x0036FC8C File Offset: 0x0036DE8C
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart_StartingThing_Defined.<PlayerStartingThings>d__5 <PlayerStartingThings>d__ = new ScenPart_StartingThing_Defined.<PlayerStartingThings>d__5(-2);
			<PlayerStartingThings>d__.<>4__this = this;
			return <PlayerStartingThings>d__;
		}

		// Token: 0x060097A1 RID: 38817 RVA: 0x0036FC9C File Offset: 0x0036DE9C
		private Thing GenerateThing(int num)
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			CompQuality compQuality;
			if (thing.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(this.quality.GetValueOrDefault(QualityCategory.Normal), new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			if (this.thingDef.IsIngestible && this.thingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			thing.stackCount = num;
			return thing;
		}

		// Token: 0x04005AC2 RID: 23234
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
