using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200210B RID: 8459
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x0600D467 RID: 54375 RVA: 0x004E81E8 File Offset: 0x004E63E8
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			Action<Thing> prevPostThingSpawn = resolveParams.postThingSpawn;
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				if (prevPostThingSpawn != null)
				{
					prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			};
			BaseGen.symbolStack.Push("bed", resolveParams, null);
		}
	}
}
