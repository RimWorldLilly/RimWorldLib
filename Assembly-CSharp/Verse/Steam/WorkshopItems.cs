using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200074F RID: 1871
	public static class WorkshopItems
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x0017A209 File Offset: 0x00178409
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x0017A210 File Offset: 0x00178410
		public static IEnumerable<WorkshopItem_Downloading> AllDownloadingItems
		{
			get
			{
				return WorkshopItems.downloadingItems;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06003B58 RID: 15192 RVA: 0x0017A218 File Offset: 0x00178418
		public static int DownloadingItemsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
				{
					if (WorkshopItems.subbedItems[i] is WorkshopItem_Downloading)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0017A253 File Offset: 0x00178453
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0017A26E File Offset: 0x0017846E
		public static void EnsureInit()
		{
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0017A270 File Offset: 0x00178470
		public static WorkshopItem GetItem(PublishedFileId_t pfid)
		{
			for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
			{
				if (WorkshopItems.subbedItems[i].PublishedFileId == pfid)
				{
					return WorkshopItems.subbedItems[i];
				}
			}
			return null;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0017A2B7 File Offset: 0x001784B7
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0017A2C4 File Offset: 0x001784C4
		private static void RebuildItemsList()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			WorkshopItems.subbedItems.Clear();
			WorkshopItems.downloadingItems.Clear();
			foreach (PublishedFileId_t pfid in Workshop.AllSubscribedItems())
			{
				WorkshopItem workshopItem = WorkshopItem.MakeFrom(pfid);
				if (workshopItem != null)
				{
					WorkshopItems.subbedItems.Add(workshopItem);
					if (workshopItem is WorkshopItem_Downloading)
					{
						WorkshopItems.downloadingItems.Add(workshopItem as WorkshopItem_Downloading);
					}
				}
			}
			ScenarioLister.MarkDirty();
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0017A358 File Offset: 0x00178558
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0017A35F File Offset: 0x0017855F
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0017A366 File Offset: 0x00178566
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0017A370 File Offset: 0x00178570
		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			foreach (WorkshopItem workshopItem in WorkshopItems.subbedItems)
			{
				stringBuilder.AppendLine("  " + workshopItem.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040026D6 RID: 9942
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();

		// Token: 0x040026D7 RID: 9943
		private static List<WorkshopItem_Downloading> downloadingItems = new List<WorkshopItem_Downloading>();
	}
}
