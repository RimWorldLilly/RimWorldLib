using System;
using LudeonTK;

namespace Verse
{
	// Token: 0x020005BD RID: 1469
	public static class DebugActionsTranslations
	{
		// Token: 0x06002D1F RID: 11551 RVA: 0x00112CF8 File Offset: 0x00110EF8
		[DebugAction("Translation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x00112CFF File Offset: 0x00110EFF
		[DebugAction("Translation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
