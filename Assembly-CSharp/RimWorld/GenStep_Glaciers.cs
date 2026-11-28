using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A6 RID: 4774
	public class GenStep_Glaciers : GenStep_ScatterLumpsMineable
	{
		// Token: 0x0600765B RID: 30299 RVA: 0x0029AC4B File Offset: 0x00298E4B
		public override void Generate(Map map, GenStepParams parms)
		{
			this.forcedDefToScatter = ThingDefOf.SolidIce;
			base.Generate(map, parms);
		}
	}
}
