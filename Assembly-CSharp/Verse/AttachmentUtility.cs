using System;

namespace Verse
{
	// Token: 0x0200053B RID: 1339
	public static class AttachmentUtility
	{
		// Token: 0x06002764 RID: 10084 RVA: 0x000F642C File Offset: 0x000F462C
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000F644C File Offset: 0x000F464C
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
