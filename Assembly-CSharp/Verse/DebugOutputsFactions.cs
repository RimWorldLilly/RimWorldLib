using System;
using LudeonTK;

namespace Verse
{
	// Token: 0x020005C8 RID: 1480
	public class DebugOutputsFactions
	{
		// Token: 0x06002E79 RID: 11897 RVA: 0x00120386 File Offset: 0x0011E586
		[DebugOutput("Factions", false)]
		public static void AllFactions()
		{
			Find.FactionManager.LogAllFactions();
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x00120392 File Offset: 0x0011E592
		[DebugOutput("Factions", false)]
		public static void AllFactionsToRemove()
		{
			Find.FactionManager.LogFactionsToRemove();
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0012039E File Offset: 0x0011E59E
		[DebugOutput("Factions", false)]
		public static void AllFactionsFromPawns()
		{
			Find.FactionManager.LogFactionsOnPawns();
		}
	}
}
