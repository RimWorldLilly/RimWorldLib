using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001159 RID: 4441
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06006F8B RID: 28555
		PlanetTile Tile { get; }

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06006F8C RID: 28556
		StoryState StoryState { get; }

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06006F8D RID: 28557
		GameConditionManager GameConditionManager { get; }

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06006F8E RID: 28558
		float PlayerWealthForStoryteller { get; }

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06006F8F RID: 28559
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06006F90 RID: 28560
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06006F91 RID: 28561
		int ConstantRandSeed { get; }

		// Token: 0x06006F92 RID: 28562
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
