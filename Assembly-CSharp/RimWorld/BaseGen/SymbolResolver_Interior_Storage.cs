using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200214B RID: 8523
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x0600D526 RID: 54566 RVA: 0x004F039C File Offset: 0x004EE59C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			BaseGen.symbolStack.Push("stockpile", rp, null);
			if (map.mapTemperature.OutdoorTemp > 15f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x04008723 RID: 34595
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
