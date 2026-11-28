using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C9 RID: 8905
	public interface ISlateRef
	{
		// Token: 0x17002633 RID: 9779
		// (get) Token: 0x0600DBA4 RID: 56228
		// (set) Token: 0x0600DBA5 RID: 56229
		string SlateRef { get; set; }

		// Token: 0x0600DBA6 RID: 56230
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
