using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F1 RID: 4849
	public class GenStep_InsectLairSite : GenStep
	{
		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x0600785A RID: 30810 RVA: 0x002A8E2D File Offset: 0x002A702D
		public override int SeedPart
		{
			get
			{
				return 49817324;
			}
		}

		// Token: 0x0600785B RID: 30811 RVA: 0x002A8E34 File Offset: 0x002A7034
		public override void Generate(Map map, GenStepParams parms)
		{
			IntVec3 entranceCell;
			if (!TileMutatorWorker_InsectMegahive.TryGetEntranceCell(map, out entranceCell))
			{
				return;
			}
			TileMutatorWorker_InsectMegahive.GenerateMegahiveEntrance(map, entranceCell, true);
		}
	}
}
