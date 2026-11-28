using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LudeonTK;
using RimWorld;
using Steamworks;
using UnityStubs;
using Waylith.AssetBake;

namespace Verse.Steam
{
	// Token: 0x02000747 RID: 1863
	public static class Workshop
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x00178E77 File Offset: 0x00177077
		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x00178E80 File Offset: 0x00177080
		internal static void Init()
		{
			Workshop.subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(Workshop.OnItemSubscribed));
			Workshop.installedCallback = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(Workshop.OnItemInstalled));
			Workshop.unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(Workshop.OnItemUnsubscribed));
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x00178ED0 File Offset: 0x001770D0
		internal static void Upload(WorkshopUploadable item)
		{
			if (Workshop.curStage != WorkshopInteractStage.None)
			{
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Workshop.uploadingHook = item.GetWorkshopItemHook();
			if (Workshop.uploadingHook.PublishedFileId != PublishedFileId_t.Invalid)
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Starting item update for mod '" + Workshop.uploadingHook.Name + "' with PublishedFileId " + Workshop.uploadingHook.PublishedFileId.ToString());
				}
				Workshop.curStage = WorkshopInteractStage.SubmittingItem;
				Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
				Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, false);
				SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Update on " + DateTime.Now.ToString() + ".");
				Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
				Workshop.submitResult.Set(hAPICall, null);
			}
			else
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Starting item creation for mod '" + Workshop.uploadingHook.Name + "'.");
				}
				Workshop.curStage = WorkshopInteractStage.CreatingItem;
				SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
				Workshop.createResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(Workshop.OnItemCreated));
				Workshop.createResult.Set(hAPICall2, null);
			}
			Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x00179044 File Offset: 0x00177244
		internal static void BakeAndUpload(ModMetaData mod)
		{
			if (Workshop.curStage != WorkshopInteractStage.None)
			{
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<string> loadFolders = mod.uploadBuildAssetPack ? ModContentPack.AllLoadFolders(mod).Where(new Func<string, bool>(ModAssetPacks.HasBakeableContent)).ToList<string>() : new List<string>();
			bool baking = loadFolders.Count > 0;
			if (!baking && !Workshop.ContainsLinks(mod.RootDir))
			{
				Workshop.Upload(mod);
				return;
			}
			bool skipSourceTextures = baking && mod.uploadSkipSourceTextures;
			string progressPrefix = "BakingModAssetPacksFor".Translate(mod.Name);
			List<string> errors = new List<string>();
			DirectoryInfo staging = null;
			Action<string> <>9__3;
			LongEventHandler.QueueLongEvent(delegate()
			{
				foreach (string loadFolder in loadFolders)
				{
					Action<string> progress;
					if ((progress = <>9__3) == null)
					{
						progress = (<>9__3 = delegate(string file)
						{
							LongEventHandler.SetCurrentEventText(progressPrefix + ": " + file);
						});
					}
					ContentBakeResult contentBakeResult = ModAssetPacks.BakeForUpload(loadFolder, progress, default(CancellationToken));
					if (!contentBakeResult.Succeeded)
					{
						errors.AddRange(contentBakeResult.Errors);
					}
				}
				if (errors.Count == 0)
				{
					staging = new DirectoryInfo(Path.Combine(GenFilePaths.SaveDataFolderPath, "WorkshopUpload", mod.PackageIdPlayerFacing));
					Workshop.BuildStagingDirectory(mod, staging, skipSourceTextures);
				}
			}, baking ? "BakingModAssetPacks" : "PreparingWorkshopUpload", true, delegate(Exception e)
			{
				errors.Add(e.Message);
			}, true, false, delegate()
			{
				if (errors.Count > 0)
				{
					Workshop.TryDeleteStaging(staging);
					Find.WindowStack.Add(new Dialog_MessageBox((baking ? "ModAssetPackBakeFailed" : "WorkshopUploadStagingFailed").Translate(mod.Name) + "\n" + errors.ToLineList("  - "), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				if (Workshop.curStage != WorkshopInteractStage.None)
				{
					Workshop.TryDeleteStaging(staging);
					Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
					return;
				}
				mod.workshopUploadDirectoryOverride = staging;
				Workshop.stagedMod = ((staging != null) ? mod : null);
				Workshop.Upload(mod);
			});
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x00179188 File Offset: 0x00177388
		private static bool IsLink(FileSystemInfo entry)
		{
			return (entry.Attributes & FileAttributes.ReparsePoint) != FileAttributes.None && entry.LinkTarget != null;
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x001791A3 File Offset: 0x001773A3
		private static bool ContainsLinks(DirectoryInfo dir)
		{
			return dir.EnumerateFileSystemInfos("*", Workshop.AllEntries).Any(delegate(FileSystemInfo e)
			{
				if (!Workshop.IsLink(e))
				{
					DirectoryInfo directoryInfo = e as DirectoryInfo;
					return directoryInfo != null && Workshop.ContainsLinks(directoryInfo);
				}
				return true;
			});
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x001791DC File Offset: 0x001773DC
		private static void BuildStagingDirectory(ModMetaData mod, DirectoryInfo staging, bool skipSourceTextures)
		{
			staging.Refresh();
			if (staging.Exists)
			{
				Workshop.DeleteDirectory(staging);
			}
			DirectoryInfo rootDir = mod.RootDir;
			FileSystemInfo fileSystemInfo = rootDir.ResolveLinkTarget(true);
			string item = ((fileSystemInfo != null) ? fileSystemInfo.FullName : null) ?? rootDir.FullName;
			Workshop.CopyFlattened(rootDir, staging.FullName, skipSourceTextures, new List<string>
			{
				item
			});
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0017923C File Offset: 0x0017743C
		private static void CopyFlattened(DirectoryInfo source, string target, bool skipSourceTextures, List<string> realChain)
		{
			Directory.CreateDirectory(target);
			foreach (FileSystemInfo fileSystemInfo in source.EnumerateFileSystemInfos("*", Workshop.AllEntries))
			{
				FileSystemInfo fileSystemInfo2 = Workshop.IsLink(fileSystemInfo) ? fileSystemInfo.ResolveLinkTarget(true) : null;
				if (fileSystemInfo2 != null && !fileSystemInfo2.Exists)
				{
					Log.Warning("Workshop: Skipping broken link " + fileSystemInfo.FullName + " -> " + fileSystemInfo.LinkTarget);
				}
				else
				{
					string text = Path.Combine(target, fileSystemInfo.Name);
					DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
					if (directoryInfo != null)
					{
						string realDir = ((fileSystemInfo2 != null) ? fileSystemInfo2.FullName : null) ?? Path.Combine(realChain[realChain.Count - 1], fileSystemInfo.Name);
						if (fileSystemInfo2 != null && realChain.Any((string r) => Workshop.IsSameOrUnder(r, realDir)))
						{
							Log.Warning("Workshop: Skipping link " + fileSystemInfo.FullName + ", it points back up into itself");
						}
						else
						{
							realChain.Add(realDir);
							Workshop.CopyFlattened(directoryInfo, text, skipSourceTextures, realChain);
							realChain.RemoveAt(realChain.Count - 1);
						}
					}
					else if (!ContentPacks.IsPackSidecar(fileSystemInfo.FullName) && (!skipSourceTextures || !ContentPacks.IsPackedSource(fileSystemInfo.FullName)))
					{
						File.Copy(fileSystemInfo.FullName, text);
					}
				}
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x001793C0 File Offset: 0x001775C0
		private static bool IsSameOrUnder(string path, string dir)
		{
			StringComparison comparisonType = OperatingSystem.IsLinux() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
			dir = Path.TrimEndingDirectorySeparator(dir);
			if (!path.Equals(dir, comparisonType))
			{
				ReadOnlySpan<char> str = dir;
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				return path.StartsWith(str + new ReadOnlySpan<char>(ref directorySeparatorChar), comparisonType);
			}
			return true;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0017940C File Offset: 0x0017760C
		private static void DeleteDirectory(DirectoryInfo dir)
		{
			foreach (FileSystemInfo fileSystemInfo in dir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
			{
				if ((fileSystemInfo.Attributes & FileAttributes.ReadOnly) != FileAttributes.None)
				{
					fileSystemInfo.Attributes &= ~FileAttributes.ReadOnly;
				}
			}
			dir.Delete(true);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x00179478 File Offset: 0x00177678
		private static void CopyPublishedFileIdToStaging()
		{
			if (Workshop.stagedMod == null)
			{
				return;
			}
			string text = Path.Combine(Workshop.stagedMod.workshopUploadDirectoryOverride.FullName, "About/PublishedFileId.txt");
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			File.Copy(Path.Combine(Workshop.stagedMod.RootDir.FullName, "About/PublishedFileId.txt"), text, true);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x001794D3 File Offset: 0x001776D3
		private static void ClearStaging()
		{
			if (Workshop.stagedMod == null)
			{
				return;
			}
			Workshop.TryDeleteStaging(Workshop.stagedMod.workshopUploadDirectoryOverride);
			Workshop.stagedMod.workshopUploadDirectoryOverride = null;
			Workshop.stagedMod = null;
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x00179500 File Offset: 0x00177700
		private static void TryDeleteStaging(DirectoryInfo staging)
		{
			if (staging == null)
			{
				return;
			}
			try
			{
				staging.Refresh();
				if (staging.Exists)
				{
					Workshop.DeleteDirectory(staging);
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Workshop: Could not delete upload staging directory " + staging.FullName + ": " + ex.Message);
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0017955C File Offset: 0x0017775C
		internal static void Unsubscribe(PublishedFileId_t pfid)
		{
			SteamUGC.UnsubscribeItem(pfid);
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00179565 File Offset: 0x00177765
		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x00179574 File Offset: 0x00177774
		internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
		{
			if (Workshop.detailsQueryCount >= 0)
			{
				Log.Error("Requested Workshop item details while a details request was already pending.");
				return;
			}
			Workshop.detailsQueryCount = publishedFileIds.Length;
			Workshop.detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)Workshop.detailsQueryCount);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(Workshop.detailsQueryHandle);
			Workshop.requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(Workshop.OnGotItemDetails));
			Workshop.requestDetailsResult.Set(hAPICall, null);
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x001795DC File Offset: 0x001777DC
		internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item subscribed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemSubscribed(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x00179648 File Offset: 0x00177848
		internal static void OnItemInstalled(ItemInstalled_t result)
		{
			if (!Workshop.IsOurAppId(result.m_unAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item installed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemInstalled(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x001796B4 File Offset: 0x001778B4
		internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				string str = "Workshop: Item unsubscribed: ";
				PublishedFileId_t nPublishedFileId = result.m_nPublishedFileId;
				Log.Message(str + nPublishedFileId.ToString());
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
			if (page_SelectScenario != null)
			{
				page_SelectScenario.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Unsubscribed(result.m_nPublishedFileId);
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00179738 File Offset: 0x00177938
		private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				Workshop.ClearStaging();
				Workshop.curStage = WorkshopInteractStage.None;
				Workshop.createResult = null;
				return;
			}
			Workshop.uploadingHook.PublishedFileId = result.m_nPublishedFileId;
			Workshop.CopyPublishedFileIdToStaging();
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item created. PublishedFileId: " + Workshop.uploadingHook.PublishedFileId.ToString());
			}
			Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
			Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, true);
			Workshop.curStage = WorkshopInteractStage.SubmittingItem;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Submitting item.");
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Initial upload.");
			Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
			Workshop.submitResult.Set(hAPICall, null);
			Workshop.createResult = null;
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x0017987C File Offset: 0x00177A7C
		private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
			else
			{
				SteamUtility.OpenWorkshopPage(Workshop.uploadingHook.PublishedFileId);
				Messages.Message("WorkshopUploadSucceeded".Translate(Workshop.uploadingHook.Name), MessageTypeDefOf.TaskCompletion, false);
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item submit result: " + result.m_eResult.ToString());
				}
			}
			Workshop.ClearStaging();
			Workshop.curStage = WorkshopInteractStage.None;
			Workshop.submitResult = null;
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00179964 File Offset: 0x00177B64
		private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			if (IOFailure)
			{
				Log.Error("Workshop: OnGotItemDetails IOFailure.");
				Workshop.detailsQueryCount = -1;
				return;
			}
			if (Workshop.detailsQueryCount < 0)
			{
				Log.Warning("Got unexpected Steam Workshop item details response.");
			}
			string text = "Steam Workshop Item details received:";
			for (int i = 0; i < Workshop.detailsQueryCount; i++)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(Workshop.detailsQueryHandle, (uint)i, out steamUGCDetails_t);
				if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
				{
					text = text + "\n  Query result: " + steamUGCDetails_t.m_eResult.ToString();
				}
				else
				{
					text = text + "\n  Title: " + steamUGCDetails_t.m_rgchTitle;
					string str = text;
					string str2 = "\n  PublishedFileId: ";
					PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
					text = str + str2 + nPublishedFileId.ToString();
					text = text + "\n  Created: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeCreated)).ToString();
					text = text + "\n  Updated: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeUpdated)).ToString();
					text = text + "\n  Added to list: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeAddedToUserList)).ToString();
					text = text + "\n  File size: " + steamUGCDetails_t.m_nFileSize.ToStringKilobytes("F2");
					text = text + "\n  Preview size: " + steamUGCDetails_t.m_nPreviewFileSize.ToStringKilobytes("F2");
					text = text + "\n  File name: " + steamUGCDetails_t.m_pchFileName;
					string str3 = text;
					string str4 = "\n  CreatorAppID: ";
					AppId_t appId_t = steamUGCDetails_t.m_nCreatorAppID;
					text = str3 + str4 + appId_t.ToString();
					string str5 = text;
					string str6 = "\n  ConsumerAppID: ";
					appId_t = steamUGCDetails_t.m_nConsumerAppID;
					text = str5 + str6 + appId_t.ToString();
					text = text + "\n  Visibiliy: " + steamUGCDetails_t.m_eVisibility.ToString();
					text = text + "\n  FileType: " + steamUGCDetails_t.m_eFileType.ToString();
					text = text + "\n  Owner: " + steamUGCDetails_t.m_ulSteamIDOwner.ToString();
				}
				text += "\n";
			}
			Log.Message(text.TrimEndNewlines());
			Workshop.detailsQueryCount = -1;
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00179B84 File Offset: 0x00177D84
		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00179BAA File Offset: 0x00177DAA
		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (pfid != PublishedFileId_t.Invalid) ? "UpdateOnSteamWorkshop".Translate() : "UploadToSteamWorkshop".Translate();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00179BD4 File Offset: 0x00177DD4
		private static void SetWorkshopItemDataFrom(UGCUpdateHandle_t updateHandle, WorkshopItemHook hook, bool creating)
		{
			hook.PrepareForWorkshopUpload();
			SteamUGC.SetItemTitle(updateHandle, hook.Name);
			if (creating)
			{
				SteamUGC.SetItemDescription(updateHandle, hook.Description);
			}
			if (!File.Exists(hook.PreviewImagePath))
			{
				Log.Warning("Missing preview file at " + hook.PreviewImagePath);
			}
			else
			{
				SteamUGC.SetItemPreview(updateHandle, hook.PreviewImagePath);
			}
			IList<string> tags = hook.Tags;
			foreach (Version version in hook.SupportedVersions)
			{
				tags.Add(version.Major.ToString() + "." + version.Minor.ToString());
			}
			SteamUGC.SetItemTags(updateHandle, tags);
			SteamUGC.SetItemContent(updateHandle, hook.Directory.FullName);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x00179CBC File Offset: 0x00177EBC
		internal static IEnumerable<PublishedFileId_t> AllSubscribedItems()
		{
			return new Workshop.<AllSubscribedItems>d__40(-2);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x00179CC8 File Offset: 0x00177EC8
		[DebugOutput("System", false)]
		internal static void SteamWorkshopStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All subscribed items (" + SteamUGC.GetNumSubscribedItems().ToString() + " total):");
			List<PublishedFileId_t> list = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.AppendLine("   " + Workshop.ItemStatusString(list[i]));
			}
			stringBuilder.AppendLine("All installed mods:");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				stringBuilder.AppendLine("   " + modMetaData.PackageIdPlayerFacing + ": " + Workshop.ItemStatusString(modMetaData.GetPublishedFileId()));
			}
			Log.Message(stringBuilder.ToString());
			List<PublishedFileId_t> list2 = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
			}
			Workshop.RequestItemsDetails(array);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00179E08 File Offset: 0x00178008
		private static string ItemStatusString(PublishedFileId_t pfid)
		{
			if (pfid == PublishedFileId_t.Invalid)
			{
				return "[unpublished]";
			}
			string str = "[";
			PublishedFileId_t publishedFileId_t = pfid;
			string text = str + publishedFileId_t.ToString() + "] ";
			ulong num;
			string str2;
			uint num2;
			if (SteamUGC.GetItemInstallInfo(pfid, out num, out str2, 257U, out num2))
			{
				text += "\n      installed";
				text = text + "\n      folder=" + str2;
				text = text + "\n      sizeOnDisk=" + (num / 1024f).ToString("F2") + "Kb";
			}
			else
			{
				text += "\n      not installed";
			}
			return text;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x00179EAB File Offset: 0x001780AB
		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}

		// Token: 0x040026C2 RID: 9922
		private static WorkshopItemHook uploadingHook;

		// Token: 0x040026C3 RID: 9923
		private static ModMetaData stagedMod;

		// Token: 0x040026C4 RID: 9924
		private static UGCUpdateHandle_t curUpdateHandle;

		// Token: 0x040026C5 RID: 9925
		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		// Token: 0x040026C6 RID: 9926
		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		// Token: 0x040026C7 RID: 9927
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		// Token: 0x040026C8 RID: 9928
		private static Callback<ItemInstalled_t> installedCallback;

		// Token: 0x040026C9 RID: 9929
		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		// Token: 0x040026CA RID: 9930
		private static CallResult<CreateItemResult_t> createResult;

		// Token: 0x040026CB RID: 9931
		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		// Token: 0x040026CC RID: 9932
		private static UGCQueryHandle_t detailsQueryHandle;

		// Token: 0x040026CD RID: 9933
		private static int detailsQueryCount = -1;

		// Token: 0x040026CE RID: 9934
		public const uint InstallInfoFolderNameMaxLength = 257U;

		// Token: 0x040026CF RID: 9935
		private static readonly EnumerationOptions AllEntries = new EnumerationOptions
		{
			AttributesToSkip = FileAttributes.None,
			IgnoreInaccessible = false
		};
	}
}
