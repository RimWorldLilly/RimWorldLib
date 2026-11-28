using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000748 RID: 1864
	public class WorkshopItem
	{
		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x00179EE3 File Offset: 0x001780E3
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x00179EEB File Offset: 0x001780EB
		// (set) Token: 0x06003B32 RID: 15154 RVA: 0x00179EF3 File Offset: 0x001780F3
		public virtual PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.pfidInt;
			}
			set
			{
				this.pfidInt = value;
			}
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x00179EFC File Offset: 0x001780FC
		public static WorkshopItem MakeFrom(PublishedFileId_t pfid)
		{
			ulong num;
			string text;
			uint num2;
			bool itemInstallInfo = SteamUGC.GetItemInstallInfo(pfid, out num, out text, 257U, out num2);
			WorkshopItem workshopItem = null;
			if (itemInstallInfo)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(text);
					if (directoryInfo != null && directoryInfo.Exists)
					{
						FileInfo[] files = directoryInfo.GetFiles();
						for (int i = 0; i < files.Length; i++)
						{
							if (files[i].Extension == ".rsc")
							{
								workshopItem = new WorkshopItem_Scenario();
								break;
							}
						}
						if (workshopItem == null)
						{
							workshopItem = new WorkshopItem_Mod();
						}
						workshopItem.directoryInt = directoryInfo;
						goto IL_B2;
					}
				}
				string str = "Created WorkshopItem for ";
				PublishedFileId_t publishedFileId_t = pfid;
				Log.Error(str + publishedFileId_t.ToString() + " but there is no folder for it.");
				return new WorkshopItem_NotInstalled();
			}
			workshopItem = new WorkshopItem_Downloading();
			IL_B2:
			workshopItem.PublishedFileId = pfid;
			return workshopItem;
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x00179FC4 File Offset: 0x001781C4
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId.ToString();
		}

		// Token: 0x040026D0 RID: 9936
		protected DirectoryInfo directoryInt;

		// Token: 0x040026D1 RID: 9937
		private PublishedFileId_t pfidInt;
	}
}
