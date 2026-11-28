using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D3 RID: 6099
	public class TaleData_Thing : TaleData
	{
		// Token: 0x0600982D RID: 38957 RVA: 0x00372CB0 File Offset: 0x00370EB0
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600982E RID: 38958 RVA: 0x00372D13 File Offset: 0x00370F13
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			TaleData_Thing.<GetRules>d__6 <GetRules>d__ = new TaleData_Thing.<GetRules>d__6(-2);
			<GetRules>d__.<>4__this = this;
			<GetRules>d__.<>3__prefix = prefix;
			return <GetRules>d__;
		}

		// Token: 0x0600982F RID: 38959 RVA: 0x00372D2C File Offset: 0x00370F2C
		public static TaleData_Thing GenerateFrom(Thing t)
		{
			TaleData_Thing taleData_Thing = new TaleData_Thing();
			taleData_Thing.thingID = t.thingIDNumber;
			taleData_Thing.thingDef = t.def;
			taleData_Thing.stuff = t.Stuff;
			t.TryGetQuality(out taleData_Thing.quality);
			CompArt compArt = t.TryGetComp<CompArt>();
			if (compArt != null && compArt.Active)
			{
				taleData_Thing.title = compArt.Title;
			}
			Book book = t as Book;
			if (book != null)
			{
				taleData_Thing.title = book.Title;
			}
			return taleData_Thing;
		}

		// Token: 0x06009830 RID: 38960 RVA: 0x00372DA8 File Offset: 0x00370FA8
		public static TaleData_Thing GenerateRandom()
		{
			ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
				}
				return false;
			}).RandomElement<ThingDef>();
			ThingDef thingDef2 = GenStuff.RandomStuffFor(thingDef);
			Thing thing = ThingMaker.MakeThing(thingDef, thingDef2);
			ArtGenerationContext artGenerationContext = (Rand.Value < 0.5f) ? ArtGenerationContext.Colony : ArtGenerationContext.Outsider;
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null && compQuality.Quality < thing.TryGetComp<CompArt>().Props.minQualityForArtistic)
			{
				compQuality.SetQuality(thing.TryGetComp<CompArt>().Props.minQualityForArtistic, new ArtGenerationContext?(artGenerationContext));
			}
			thing.TryGetComp<CompArt>().InitializeArt(artGenerationContext);
			return TaleData_Thing.GenerateFrom(thing);
		}

		// Token: 0x04005B16 RID: 23318
		public int thingID;

		// Token: 0x04005B17 RID: 23319
		public ThingDef thingDef;

		// Token: 0x04005B18 RID: 23320
		public ThingDef stuff;

		// Token: 0x04005B19 RID: 23321
		public string title;

		// Token: 0x04005B1A RID: 23322
		public QualityCategory quality;
	}
}
