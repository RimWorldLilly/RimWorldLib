using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001985 RID: 6533
	public class CompProperties_SpawnerItems : CompProperties
	{
		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x0600A40B RID: 41995 RVA: 0x003A9CC9 File Offset: 0x003A7EC9
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		// Token: 0x0600A40C RID: 41996 RVA: 0x003A9CE1 File Offset: 0x003A7EE1
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		// Token: 0x0600A40D RID: 41997 RVA: 0x003A9D1A File Offset: 0x003A7F1A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_SpawnerItems.<ConfigErrors>d__7 <ConfigErrors>d__ = new CompProperties_SpawnerItems.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040060EA RID: 24810
		public float approxMarketValuePerDay;

		// Token: 0x040060EB RID: 24811
		public int spawnInterval = 60000;

		// Token: 0x040060EC RID: 24812
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		// Token: 0x040060ED RID: 24813
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
