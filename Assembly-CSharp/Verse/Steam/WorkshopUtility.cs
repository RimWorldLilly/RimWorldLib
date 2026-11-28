using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000751 RID: 1873
	internal static class WorkshopUtility
	{
		// Token: 0x06003B62 RID: 15202 RVA: 0x0017A3EC File Offset: 0x001785EC
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + stage.ToString()).Translate();
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0017A422 File Offset: 0x00178622
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0017A445 File Offset: 0x00178645
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}
