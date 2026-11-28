using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B2 RID: 9138
	public class PocketMapParent : MapParent
	{
		// Token: 0x170027FD RID: 10237
		// (get) Token: 0x0600E3A0 RID: 58272 RVA: 0x0054A4F0 File Offset: 0x005486F0
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.mapGenerator;
			}
		}

		// Token: 0x0600E3A2 RID: 58274 RVA: 0x0054A500 File Offset: 0x00548700
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.sourceMap, "sourceMap", false);
			Scribe_Defs.Look<MapGeneratorDef>(ref this.mapGenerator, "mapGenerator");
		}

		// Token: 0x04009112 RID: 37138
		public Map sourceMap;

		// Token: 0x04009113 RID: 37139
		public MapGeneratorDef mapGenerator;
	}
}
