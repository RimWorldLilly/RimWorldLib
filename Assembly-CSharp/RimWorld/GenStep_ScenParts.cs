using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CA RID: 4810
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06007761 RID: 30561 RVA: 0x002A1C48 File Offset: 0x0029FE48
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x002A1C4F File Offset: 0x0029FE4F
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
