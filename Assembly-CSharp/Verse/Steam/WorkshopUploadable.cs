using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200074D RID: 1869
	public interface WorkshopUploadable
	{
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003B3E RID: 15166
		IEnumerable<Version> SupportedVersions { get; }

		// Token: 0x06003B3F RID: 15167
		bool CanToUploadToWorkshop();

		// Token: 0x06003B40 RID: 15168
		void PrepareForWorkshopUpload();

		// Token: 0x06003B41 RID: 15169
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x06003B42 RID: 15170
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x06003B43 RID: 15171
		string GetWorkshopName();

		// Token: 0x06003B44 RID: 15172
		string GetWorkshopDescription();

		// Token: 0x06003B45 RID: 15173
		string GetWorkshopPreviewImagePath();

		// Token: 0x06003B46 RID: 15174
		IList<string> GetWorkshopTags();

		// Token: 0x06003B47 RID: 15175
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x06003B48 RID: 15176
		WorkshopItemHook GetWorkshopItemHook();
	}
}
