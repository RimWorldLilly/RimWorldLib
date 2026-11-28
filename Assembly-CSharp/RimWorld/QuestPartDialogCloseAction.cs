using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B8 RID: 4280
	public class QuestPartDialogCloseAction
	{
		// Token: 0x0400440D RID: 17421
		public SoundDef dialogCloseSound;

		// Token: 0x0400440E RID: 17422
		public Action dialogCloseAction;

		// Token: 0x02002FFD RID: 12285
		public enum CloseActionKey
		{
			// Token: 0x0400C2F1 RID: 49905
			None,
			// Token: 0x0400C2F2 RID: 49906
			ArchonexusVictorySound2nd,
			// Token: 0x0400C2F3 RID: 49907
			ArchonexusVictorySound3rd
		}
	}
}
