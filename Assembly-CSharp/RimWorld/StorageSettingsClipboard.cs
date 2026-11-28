using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8C RID: 6796
	public static class StorageSettingsClipboard
	{
		// Token: 0x17001E82 RID: 7810
		// (get) Token: 0x0600AB82 RID: 43906 RVA: 0x003C9566 File Offset: 0x003C7766
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x0600AB83 RID: 43907 RVA: 0x003C956D File Offset: 0x003C776D
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
			Messages.Message("StorageSettingsCopiedToClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600AB84 RID: 43908 RVA: 0x003C959B File Offset: 0x003C779B
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
			Messages.Message("StorageSettingsPastedFromClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600AB85 RID: 43909 RVA: 0x003C95C3 File Offset: 0x003C77C3
		public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
		{
			StorageSettingsClipboard.<CopyPasteGizmosFor>d__6 <CopyPasteGizmosFor>d__ = new StorageSettingsClipboard.<CopyPasteGizmosFor>d__6(-2);
			<CopyPasteGizmosFor>d__.<>3__s = s;
			return <CopyPasteGizmosFor>d__;
		}

		// Token: 0x040063DC RID: 25564
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x040063DD RID: 25565
		private static bool copied = false;
	}
}
