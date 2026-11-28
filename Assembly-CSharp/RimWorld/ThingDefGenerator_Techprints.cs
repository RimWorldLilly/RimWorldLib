using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA3 RID: 3747
	public static class ThingDefGenerator_Techprints
	{
		// Token: 0x060062A6 RID: 25254 RVA: 0x00238B98 File Offset: 0x00236D98
		public static IEnumerable<ThingDef> ImpliedTechprintDefs(bool hotReload = false)
		{
			ThingDefGenerator_Techprints.<ImpliedTechprintDefs>d__1 <ImpliedTechprintDefs>d__ = new ThingDefGenerator_Techprints.<ImpliedTechprintDefs>d__1(-2);
			<ImpliedTechprintDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedTechprintDefs>d__;
		}

		// Token: 0x04003653 RID: 13907
		public const string Tag = "Techprint";
	}
}
