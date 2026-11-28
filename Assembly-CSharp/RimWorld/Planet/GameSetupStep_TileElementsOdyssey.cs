using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D6 RID: 8918
	public class GameSetupStep_TileElementsOdyssey : GameSetupStep
	{
		// Token: 0x1700263D RID: 9789
		// (get) Token: 0x0600DBE5 RID: 56293 RVA: 0x0051E8BA File Offset: 0x0051CABA
		public override int SeedPart
		{
			get
			{
				return 689393;
			}
		}

		// Token: 0x0600DBE6 RID: 56294 RVA: 0x0051E8C1 File Offset: 0x0051CAC1
		public override void GenerateFresh()
		{
			Find.World.landmarks = new WorldLandmarks();
		}
	}
}
