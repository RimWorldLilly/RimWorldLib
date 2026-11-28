using System;

namespace Verse
{
	// Token: 0x020005A2 RID: 1442
	public interface IThingHolderEvents<in T> where T : Thing
	{
		// Token: 0x06002B0F RID: 11023
		void Notify_ItemAdded(T item);

		// Token: 0x06002B10 RID: 11024
		void Notify_ItemRemoved(T item);
	}
}
