using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E7 RID: 4839
	public class GenStep_AncientComplex_Mechanitor : GenStep_AncientComplex
	{
		// Token: 0x06007814 RID: 30740 RVA: 0x002A79E3 File Offset: 0x002A5BE3
		protected override void GenerateComplex(Map map, ResolveParams parms)
		{
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientMechanitorComplex", parms, null);
			BaseGen.Generate();
		}
	}
}
