using System;

namespace Verse
{
	// Token: 0x020005A0 RID: 1440
	public interface IThingHolderTickable : IThingHolder
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002B0B RID: 11019
		bool ShouldTickContents { get; }
	}
}
