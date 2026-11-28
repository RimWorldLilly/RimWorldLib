using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D5 RID: 8917
	public class GameSetupStep_TileElementsCore : GameSetupStep
	{
		// Token: 0x1700263C RID: 9788
		// (get) Token: 0x0600DBE2 RID: 56290 RVA: 0x0051E89A File Offset: 0x0051CA9A
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x0600DBE3 RID: 56291 RVA: 0x0051E8A1 File Offset: 0x0051CAA1
		public override void GenerateFresh()
		{
			Find.World.features = new WorldFeatures();
		}
	}
}
