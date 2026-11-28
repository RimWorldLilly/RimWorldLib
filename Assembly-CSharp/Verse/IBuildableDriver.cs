using System;

namespace Verse
{
	// Token: 0x020000EF RID: 239
	public interface IBuildableDriver
	{
		// Token: 0x0600089D RID: 2205
		bool TryGetBuildableRect(out CellRect rect);
	}
}
