using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020005A1 RID: 1441
	public interface IThingHolderWithDrawnPawn : IThingHolder
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002B0C RID: 11020
		float HeldPawnDrawPos_Y { get; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002B0D RID: 11021
		float HeldPawnBodyAngle { get; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002B0E RID: 11022
		PawnPosture HeldPawnPosture { get; }
	}
}
