using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E2 RID: 4834
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x060077F3 RID: 30707 RVA: 0x002A719F File Offset: 0x002A539F
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x002A71A6 File Offset: 0x002A53A6
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
