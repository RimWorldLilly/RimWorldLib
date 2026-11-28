using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B83 RID: 7043
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x0600B4C3 RID: 46275 RVA: 0x003FB2D2 File Offset: 0x003F94D2
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Animals.<GenerateThings>d__9 <GenerateThings>d__ = new StockGenerator_Animals.<GenerateThings>d__9(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__forTile = forTile;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4C4 RID: 46276 RVA: 0x003FB2E9 File Offset: 0x003F94E9
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.race.GetStatValueAbstract(StatDefOf.Wildness, null));
		}

		// Token: 0x0600B4C5 RID: 46277 RVA: 0x003FB308 File Offset: 0x003F9508
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x0600B4C6 RID: 46278 RVA: 0x003FB384 File Offset: 0x003F9584
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			bool flag = false;
			bool flag2 = false;
			if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Buyable) && this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
			{
				flag = true;
			}
			if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Sellable) && this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
			{
				flag2 = true;
			}
			if (flag2 && flag)
			{
				return Tradeability.All;
			}
			if (flag2)
			{
				return Tradeability.Sellable;
			}
			if (flag)
			{
				return Tradeability.Buyable;
			}
			return Tradeability.None;
		}

		// Token: 0x0600B4C7 RID: 46279 RVA: 0x003FB434 File Offset: 0x003F9634
		private bool PawnKindAllowed(PawnKindDef kind, PlanetTile forTile)
		{
			float statValueAbstract = kind.race.GetStatValueAbstract(StatDefOf.Wildness, null);
			if (!kind.RaceProps.Animal || statValueAbstract < this.minWildness || statValueAbstract > this.maxWildness || statValueAbstract > 1f)
			{
				return false;
			}
			if (this.checkTemperature)
			{
				PlanetTile tile = forTile;
				if (!tile.Valid && Find.AnyPlayerHomeMap != null)
				{
					tile = Find.AnyPlayerHomeMap.Tile;
				}
				if (tile.Valid && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, kind.race))
				{
					return false;
				}
			}
			return kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell();
		}

		// Token: 0x0600B4C8 RID: 46280 RVA: 0x003FB52C File Offset: 0x003F972C
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B4C9 RID: 46281 RVA: 0x003FB5B0 File Offset: 0x003F97B0
		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Token: 0x17002096 RID: 8342
		// (get) Token: 0x0600B4CA RID: 46282 RVA: 0x003FB5E7 File Offset: 0x003F97E7
		public IEnumerable<string> AllRelevantTradeTags
		{
			get
			{
				return this.tradeTagsSell.Concat(this.tradeTagsBuy).Concat(this.createMatingPair);
			}
		}

		// Token: 0x0600B4CB RID: 46283 RVA: 0x003FB608 File Offset: 0x003F9808
		public string GetInterestInTradeTag(string tag)
		{
			bool flag = this.tradeTagsBuy.Contains(tag);
			bool flag2 = this.tradeTagsSell.Contains(tag);
			if (flag && flag2)
			{
				return "Buy/Sell";
			}
			if (flag)
			{
				return "Buy";
			}
			if (flag2)
			{
				return "Sell";
			}
			return "-";
		}

		// Token: 0x040067FA RID: 26618
		[NoTranslate]
		private List<string> tradeTagsSell = new List<string>();

		// Token: 0x040067FB RID: 26619
		[NoTranslate]
		private List<string> tradeTagsBuy = new List<string>();

		// Token: 0x040067FC RID: 26620
		[NoTranslate]
		private List<string> createMatingPair = new List<string>();

		// Token: 0x040067FD RID: 26621
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x040067FE RID: 26622
		private float minWildness;

		// Token: 0x040067FF RID: 26623
		private float maxWildness = 1f;

		// Token: 0x04006800 RID: 26624
		private bool checkTemperature = true;

		// Token: 0x04006801 RID: 26625
		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		// Token: 0x04006802 RID: 26626
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;
	}
}
