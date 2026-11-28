using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200033D RID: 829
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06001952 RID: 6482 RVA: 0x000980C7 File Offset: 0x000962C7
		public LoadedContentItem(VirtualFile internalFile, T contentItem)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
		}

		// Token: 0x040014F4 RID: 5364
		public VirtualFile internalFile;

		// Token: 0x040014F5 RID: 5365
		public T contentItem;
	}
}
