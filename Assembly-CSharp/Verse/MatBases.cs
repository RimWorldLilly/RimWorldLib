using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000706 RID: 1798
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04002508 RID: 9480
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04002509 RID: 9481
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x0400250A RID: 9482
		public static readonly Material SunShadowFade = MatLoader.LoadMat("Lighting/SunShadowFade", -1);

		// Token: 0x0400250B RID: 9483
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x0400250C RID: 9484
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x0400250D RID: 9485
		public static readonly Material FilledMask = MatLoader.LoadMat("Misc/FilledMask", -1);

		// Token: 0x0400250E RID: 9486
		public static readonly Material GravshipMask = MatLoader.LoadMat("Map/Gravship/GravshipMask", -1);

		// Token: 0x0400250F RID: 9487
		public static readonly Material LightOverlayGravship = MatLoader.LoadMat("Lighting/LightOverlayGravship", -1);

		// Token: 0x04002510 RID: 9488
		public static readonly Material ShadowMask = MatLoader.LoadMat("Misc/ShadowMask", -1);

		// Token: 0x04002511 RID: 9489
		public static readonly Material RoofedOutdoorMask = MatLoader.LoadMat("Misc/RoofedOutdoorMask", -1);

		// Token: 0x04002512 RID: 9490
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04002513 RID: 9491
		public static readonly Material Darkness = MatLoader.LoadMat("Misc/Darkness", -1);

		// Token: 0x04002514 RID: 9492
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);

		// Token: 0x04002515 RID: 9493
		public static readonly Material DebugOverlay = MatLoader.LoadMat("Misc/DebugOverlay", -1);

		// Token: 0x04002516 RID: 9494
		public static readonly Material Sand = MatLoader.LoadMat("Misc/Sand", -1);
	}
}
