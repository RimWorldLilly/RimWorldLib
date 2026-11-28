using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B9 RID: 4281
	public static class QuestPartDialogCloseActionRegistry
	{
		// Token: 0x06006B15 RID: 27413 RVA: 0x002595C0 File Offset: 0x002577C0
		public static QuestPartDialogCloseAction CloseActionOf(QuestPartDialogCloseAction.CloseActionKey key)
		{
			return QuestPartDialogCloseActionRegistry.CloseActions.TryGetValue(key, null);
		}

		// Token: 0x0400440F RID: 17423
		private static readonly Dictionary<QuestPartDialogCloseAction.CloseActionKey, QuestPartDialogCloseAction> CloseActions = new Dictionary<QuestPartDialogCloseAction.CloseActionKey, QuestPartDialogCloseAction>
		{
			{
				QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd,
				new QuestPartDialogCloseAction
				{
					dialogCloseSound = SoundDefOf.GameStartSting_FirstArchonexusCycle,
					dialogCloseAction = delegate()
					{
						Find.MusicManagerPlay.ForceSilenceFor(8f);
					}
				}
			},
			{
				QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd,
				new QuestPartDialogCloseAction
				{
					dialogCloseSound = SoundDefOf.GameStartSting_SecondArchonexusCycle,
					dialogCloseAction = delegate()
					{
						Find.MusicManagerPlay.ForceSilenceFor(8f);
					}
				}
			}
		};
	}
}
