using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B71 RID: 7025
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x0600B451 RID: 46161 RVA: 0x003F9018 File Offset: 0x003F7218
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement<TraderKindDef>();
			Faction makingFaction = parms.makingFaction;
			PlanetTile forTile;
			if (parms.tile != null)
			{
				forTile = parms.tile.Value;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				forTile = Find.AnyPlayerHomeMap.Tile;
			}
			else if (Find.CurrentMap != null)
			{
				forTile = Find.CurrentMap.Tile;
			}
			else
			{
				forTile = PlanetTile.Invalid;
			}
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				foreach (Thing thing in traderKindDef.stockGenerators[i].GenerateThings(forTile, parms.makingFaction))
				{
					if (!thing.def.tradeability.TraderCanSell())
					{
						TraderKindDef traderKindDef2 = traderKindDef;
						string str = (traderKindDef2 != null) ? traderKindDef2.ToString() : null;
						string str2 = " generated carrying ";
						Thing thing2 = thing;
						Log.Error(str + str2 + ((thing2 != null) ? thing2.ToString() : null) + " which can't be sold by traders. Ignoring...");
					}
					else
					{
						thing.PostGeneratedForTrader(traderKindDef, forTile, makingFaction);
						outThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x0600B452 RID: 46162 RVA: 0x003F9150 File Offset: 0x003F7350
		public float DebugAverageTotalStockValue(TraderKindDef td)
		{
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				traderDef = td,
				tile = new PlanetTile?(PlanetTile.Invalid)
			};
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing thing in base.Generate(parms))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			return num / 50f;
		}

		// Token: 0x0600B453 RID: 46163 RVA: 0x003F91F0 File Offset: 0x003F73F0
		public string DebugGenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.DebugAverageTotalStockValue(td).ToString("F0"));
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				traderDef = td,
				tile = new PlanetTile?(PlanetTile.Invalid)
			};
			(from x in Find.FactionManager.AllFactionsListForReading
			where x.def.baseTraderKinds.Contains(td) || x.def.visitorTraderKinds.Contains(td) || x.def.caravanTraderKinds.Contains(td)
			select x).TryRandomElement(out parms.makingFaction);
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing thing in base.Generate(parms))
			{
				MinifiedThing minifiedThing = thing as MinifiedThing;
				Thing thing2;
				if (minifiedThing != null)
				{
					thing2 = minifiedThing.InnerThing;
				}
				else
				{
					thing2 = thing;
				}
				string text = thing2.LabelCap;
				Book book = thing2 as Book;
				if (book != null)
				{
					text = "[" + book.def.defName + "]: " + text;
				}
				text = text + " [" + (thing2.MarketValue * (float)thing2.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B454 RID: 46164 RVA: 0x003F9384 File Offset: 0x003F7584
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_TraderStock.<AllGeneratableThingsDebugSub>d__3 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_TraderStock.<AllGeneratableThingsDebugSub>d__3(-2);
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}
	}
}
