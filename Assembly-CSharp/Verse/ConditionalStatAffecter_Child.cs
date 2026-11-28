using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E2 RID: 226
	public class ConditionalStatAffecter_Child : ConditionalStatAffecter
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x000301F8 File Offset: 0x0002E3F8
		public override string Label
		{
			get
			{
				return "Child".Translate();
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0003020C File Offset: 0x0002E40C
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					return pawn.DevelopmentalStage.Child();
				}
			}
			return false;
		}
	}
}
