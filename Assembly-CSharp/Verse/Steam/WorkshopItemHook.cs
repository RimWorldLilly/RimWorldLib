using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200074E RID: 1870
	public class WorkshopItemHook
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x0017A0C5 File Offset: 0x001782C5
		// (set) Token: 0x06003B4A RID: 15178 RVA: 0x0017A0D2 File Offset: 0x001782D2
		public PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.owner.GetPublishedFileId();
			}
			set
			{
				this.owner.SetPublishedFileId(value);
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x0017A0E0 File Offset: 0x001782E0
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x0017A0ED File Offset: 0x001782ED
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06003B4D RID: 15181 RVA: 0x0017A0FA File Offset: 0x001782FA
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x0017A107 File Offset: 0x00178307
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003B4F RID: 15183 RVA: 0x0017A114 File Offset: 0x00178314
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x0017A121 File Offset: 0x00178321
		public IEnumerable<Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003B51 RID: 15185 RVA: 0x0017A12E File Offset: 0x0017832E
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0017A168 File Offset: 0x00178368
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0017A19A File Offset: 0x0017839A
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x0017A1A8 File Offset: 0x001783A8
		private void SendSteamDetailsQuery()
		{
			if (SteamManager.Initialized)
			{
				SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999U);
				this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
				this.queryResult.Set(hAPICall, null);
			}
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x0017A1F1 File Offset: 0x001783F1
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x040026D3 RID: 9939
		private WorkshopUploadable owner;

		// Token: 0x040026D4 RID: 9940
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x040026D5 RID: 9941
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
