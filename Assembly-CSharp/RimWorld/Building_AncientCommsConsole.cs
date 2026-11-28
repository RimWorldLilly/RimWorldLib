using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A93 RID: 6803
	public class Building_AncientCommsConsole : Building
	{
		// Token: 0x0600ABA9 RID: 43945 RVA: 0x003C9DF0 File Offset: 0x003C7FF0
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + "LinkedTo".Translate() + ": " + "SupplySatellite".Translate());
		}
	}
}
