using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E3 RID: 4835
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x060077F6 RID: 30710 RVA: 0x002A71C0 File Offset: 0x002A53C0
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x060077F7 RID: 30711 RVA: 0x002A71C7 File Offset: 0x002A53C7
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x060077F8 RID: 30712 RVA: 0x002A71D8 File Offset: 0x002A53D8
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			if (root.IsValid)
			{
				signalAction_Ambush.spawnNear = root;
			}
			else
			{
				signalAction_Ambush.spawnAround = rectToDefend;
			}
			return signalAction_Ambush;
		}
	}
}
