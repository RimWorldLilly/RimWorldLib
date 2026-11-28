using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000341 RID: 833
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x0009A2A0 File Offset: 0x000984A0
		public Texture2D PreviewImage
		{
			get
			{
				if (this.previewImageWasLoaded)
				{
					return this.previewImage;
				}
				if (File.Exists(this.PreviewImagePath))
				{
					this.previewImage = new Texture2D(0, 0);
					this.previewImage.LoadImage(File.ReadAllBytes(this.PreviewImagePath));
				}
				this.previewImageWasLoaded = true;
				return this.previewImage;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0009A2FC File Offset: 0x000984FC
		public Texture2D Icon
		{
			get
			{
				if (this.iconImageWasLoaded)
				{
					return this.iconImage;
				}
				if (!this.ModIconPath.NullOrEmpty())
				{
					this.iconImage = ContentFinder<Texture2D>.Get(this.ModIconPath, true);
				}
				else if (File.Exists(this.ModIconImagePath))
				{
					this.iconImage = new Texture2D(0, 0);
					this.iconImage.LoadImage(File.ReadAllBytes(this.ModIconImagePath));
				}
				else
				{
					this.iconImage = ModMetaData.ModIcon.Texture;
				}
				this.iconImageWasLoaded = true;
				return this.iconImage;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x0009A389 File Offset: 0x00098589
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0009A396 File Offset: 0x00098596
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x0009A39E File Offset: 0x0009859E
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId("ludeon.rimworld", false);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060019AD RID: 6573 RVA: 0x0009A3AC File Offset: 0x000985AC
		// (set) Token: 0x060019AE RID: 6574 RVA: 0x0009A3B4 File Offset: 0x000985B4
		public bool Active
		{
			get
			{
				return ModsConfig.IsActive(this);
			}
			set
			{
				ModsConfig.SetActive(this, value);
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x0009A3BD File Offset: 0x000985BD
		public bool VersionCompatible
		{
			get
			{
				if (this.IsCoreMod)
				{
					return true;
				}
				return this.meta.SupportedVersions.Any((Version v) => VersionControl.IsCompatible(v));
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0009A3F8 File Offset: 0x000985F8
		public bool MadeForNewerVersion
		{
			get
			{
				if (this.VersionCompatible)
				{
					return false;
				}
				return this.meta.SupportedVersions.Any((Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0009A433 File Offset: 0x00098633
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0009A440 File Offset: 0x00098640
		public string Name
		{
			get
			{
				ExpansionDef expansion = this.Expansion;
				if (expansion == null)
				{
					return this.meta.name;
				}
				return expansion.label;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0009A469 File Offset: 0x00098669
		public string ShortName
		{
			get
			{
				if (!this.meta.shortName.NullOrEmpty())
				{
					return this.meta.shortName;
				}
				return this.Name;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0009A490 File Offset: 0x00098690
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(this.PackageId);
					this.descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : this.meta.description);
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0009A4D3 File Offset: 0x000986D3
		public string AuthorsString
		{
			get
			{
				return this.Authors.ToCommaList(true, false);
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x0009A4E2 File Offset: 0x000986E2
		public IEnumerable<string> Authors
		{
			get
			{
				ModMetaData.<get_Authors>d__51 <get_Authors>d__ = new ModMetaData.<get_Authors>d__51(-2);
				<get_Authors>d__.<>4__this = this;
				return <get_Authors>d__;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x0009A4F2 File Offset: 0x000986F2
		public virtual string ModVersion
		{
			get
			{
				return this.meta.modVersion;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x0009A4FF File Offset: 0x000986FF
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0009A50C File Offset: 0x0009870C
		public uint SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x0009A519 File Offset: 0x00098719
		public List<Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0009A526 File Offset: 0x00098726
		IEnumerable<Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x0009A52E File Offset: 0x0009872E
		public string PreviewImagePath
		{
			get
			{
				return GenFile.ResolveCaseInsensitiveFilePath(Path.Combine(this.rootDirInt.FullName, "About"), "Preview.png");
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x0009A54F File Offset: 0x0009874F
		public string ModIconImagePath
		{
			get
			{
				return GenFile.ResolveCaseInsensitiveFilePath(Path.Combine(this.rootDirInt.FullName, "About"), "ModIcon.png");
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x0009A570 File Offset: 0x00098770
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x0009A585 File Offset: 0x00098785
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0009A58D File Offset: 0x0009878D
		public string PackageId
		{
			get
			{
				if (!this.appendPackageIdSteamPostfix)
				{
					return this.packageIdLowerCase;
				}
				return this.packageIdLowerCase + ModMetaData.SteamModPostfix;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0009A5AE File Offset: 0x000987AE
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0009A5B6 File Offset: 0x000987B6
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x0009A5C3 File Offset: 0x000987C3
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0009A5D0 File Offset: 0x000987D0
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0009A5DD File Offset: 0x000987DD
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0009A5EA File Offset: 0x000987EA
		public List<string> ForceLoadBefore
		{
			get
			{
				return this.meta.forceLoadBefore;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x0009A5F7 File Offset: 0x000987F7
		public List<string> ForceLoadAfter
		{
			get
			{
				return this.meta.forceLoadAfter;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x0009A604 File Offset: 0x00098804
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0009A611 File Offset: 0x00098811
		public string ModIconPath
		{
			get
			{
				return this.meta.modIconPath;
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0009A620 File Offset: 0x00098820
		public List<string> UnsatisfiedDependencies()
		{
			this.unsatisfiedDepsList.Clear();
			for (int i = 0; i < this.Dependencies.Count; i++)
			{
				ModDependency modDependency = this.Dependencies[i];
				if (!modDependency.IsSatisfied)
				{
					this.unsatisfiedDepsList.Add(modDependency.displayName);
				}
			}
			return this.unsatisfiedDepsList;
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0009A67A File Offset: 0x0009887A
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0009A682 File Offset: 0x00098882
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0009A68B File Offset: 0x0009888B
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x0009A693 File Offset: 0x00098893
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x060019CF RID: 6607 RVA: 0x0009A69C File Offset: 0x0009889C
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0009A700 File Offset: 0x00098900
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0009A75B File Offset: 0x0009895B
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0009A764 File Offset: 0x00098964
		public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
		{
			if (this.PackageId == null)
			{
				return false;
			}
			if (ignorePostfix)
			{
				return this.packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
			}
			return this.PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0009A78E File Offset: 0x0009898E
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0009A7A4 File Offset: 0x000989A4
		private void Init()
		{
			ReadOnlySpan<char> str = this.RootDir.FullName;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(GenFile.ResolveCaseInsensitiveFilePath(str + new ReadOnlySpan<char>(ref directorySeparatorChar) + "About", "About.xml"), false);
			this.loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(GenFile.ResolveCaseInsensitiveFilePath(this.RootDir.FullName, "LoadFolders.xml"), false);
			bool shouldLogIssues = ModLister.ShouldLogIssues;
			this.HadIncorrectlyFormattedVersionInMetadata = !this.meta.TryParseSupportedVersions(!this.OnSteamWorkshop && shouldLogIssues);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.FolderName;
				}
				else
				{
					this.meta.name = this.FolderName;
				}
			}
			this.HadIncorrectlyFormattedPackageId = !this.meta.TryParsePackageId(this.Official, !this.OnSteamWorkshop && shouldLogIssues);
			this.packageIdLowerCase = this.meta.packageId.ToLower();
			this.meta.InitVersionedData();
			this.meta.ValidateDependencies(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0009A902 File Offset: 0x00098B02
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0009A915 File Offset: 0x00098B15
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x0009A920 File Offset: 0x00098B20
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"PublishedFileId.txt"
				});
			}
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0009A970 File Offset: 0x00098B70
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0009A972 File Offset: 0x00098B72
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0009A999 File Offset: 0x00098B99
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0009A9A1 File Offset: 0x00098BA1
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0009A9D1 File Offset: 0x00098BD1
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0009A9D9 File Offset: 0x00098BD9
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0009A9E1 File Offset: 0x00098BE1
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0009A9E9 File Offset: 0x00098BE9
		public IList<string> GetWorkshopTags()
		{
			if (!this.translationMod)
			{
				return new List<string>
				{
					"Mod"
				};
			}
			return new List<string>
			{
				"Translation"
			};
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0009AA14 File Offset: 0x00098C14
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.workshopUploadDirectoryOverride ?? this.RootDir;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0009AA26 File Offset: 0x00098C26
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0009AA42 File Offset: 0x00098C42
		public IEnumerable<ModRequirement> GetRequirements()
		{
			ModMetaData.<GetRequirements>d__122 <GetRequirements>d__ = new ModMetaData.<GetRequirements>d__122(-2);
			<GetRequirements>d__.<>4__this = this;
			return <GetRequirements>d__;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0009AA52 File Offset: 0x00098C52
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0009AA5F File Offset: 0x00098C5F
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.PackageIdPlayerFacing,
				"|",
				this.Name,
				"]"
			});
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0009AA96 File Offset: 0x00098C96
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x04001521 RID: 5409
		private DirectoryInfo rootDirInt;

		// Token: 0x04001522 RID: 5410
		private ContentSource source;

		// Token: 0x04001523 RID: 5411
		private Texture2D previewImage;

		// Token: 0x04001524 RID: 5412
		private bool previewImageWasLoaded;

		// Token: 0x04001525 RID: 5413
		private Texture2D iconImage;

		// Token: 0x04001526 RID: 5414
		private bool iconImageWasLoaded;

		// Token: 0x04001527 RID: 5415
		public bool enabled = true;

		// Token: 0x04001528 RID: 5416
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04001529 RID: 5417
		public ModLoadFolders loadFolders;

		// Token: 0x0400152A RID: 5418
		private WorkshopItemHook workshopHookInt;

		// Token: 0x0400152B RID: 5419
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x0400152C RID: 5420
		public bool appendPackageIdSteamPostfix;

		// Token: 0x0400152D RID: 5421
		public bool translationMod;

		// Token: 0x0400152E RID: 5422
		public bool uploadBuildAssetPack = true;

		// Token: 0x0400152F RID: 5423
		public bool uploadSkipSourceTextures;

		// Token: 0x04001530 RID: 5424
		public DirectoryInfo workshopUploadDirectoryOverride;

		// Token: 0x04001531 RID: 5425
		public string packageIdLowerCase;

		// Token: 0x04001532 RID: 5426
		private string descriptionCached;

		// Token: 0x04001533 RID: 5427
		private const string AboutFolderName = "About";

		// Token: 0x04001534 RID: 5428
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x04001535 RID: 5429
		private static readonly CachedTexture ModIcon = new CachedTexture("UI/Icons/Options/OptionsMod");

		// Token: 0x04001536 RID: 5430
		private static readonly string[] AndToken = new string[]
		{
			" and "
		};

		// Token: 0x04001537 RID: 5431
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x02002675 RID: 9845
		private class ModMetaDataInternal
		{
			// Token: 0x17002ADD RID: 10973
			// (get) Token: 0x0600F3C4 RID: 62404 RVA: 0x00591D31 File Offset: 0x0058FF31
			// (set) Token: 0x0600F3C5 RID: 62405 RVA: 0x00591D39 File Offset: 0x0058FF39
			public List<Version> SupportedVersions { get; private set; }

			// Token: 0x0600F3C6 RID: 62406 RVA: 0x00591D44 File Offset: 0x0058FF44
			private bool TryParseVersion(string str, bool logIssues = true)
			{
				Version version;
				if (!VersionControl.TryParseVersionString(str, out version))
				{
					if (logIssues)
					{
						Log.Error(string.Concat(new string[]
						{
							"Unable to parse version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\""
						}));
					}
					return false;
				}
				this.SupportedVersions.Add(version);
				if (!VersionControl.IsWellFormattedVersionString(str))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Malformed (correct format is Major.Minor) version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\" - parsed as \"",
							version.Major.ToString(),
							".",
							version.Minor.ToString(),
							"\""
						}));
					}
					return false;
				}
				return true;
			}

			// Token: 0x0600F3C7 RID: 62407 RVA: 0x00591E40 File Offset: 0x00590040
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.");
				}
				bool flag = false;
				this.SupportedVersions = new List<Version>();
				if (this.packageId.ToLower() == "ludeon.rimworld")
				{
					this.SupportedVersions.Add(VersionControl.CurrentVersion);
				}
				else if (this.supportedVersions == null)
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)");
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.");
					}
					flag = true;
				}
				else
				{
					for (int i = 0; i < this.supportedVersions.Count; i++)
					{
						flag |= !this.TryParseVersion(this.supportedVersions[i], logIssues);
					}
				}
				this.SupportedVersions = this.SupportedVersions.OrderBy(delegate(Version v)
				{
					if (!VersionControl.IsCompatible(v))
					{
						return 100;
					}
					return -100;
				}).ThenByDescending((Version v) => v.Major).ThenByDescending((Version v) => v.Minor).Distinct<Version>().ToList<Version>();
				return !flag;
			}

			// Token: 0x0600F3C8 RID: 62408 RVA: 0x00591FBC File Offset: 0x005901BC
			public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
			{
				bool flag = false;
				if (this.packageId.NullOrEmpty())
				{
					string text = "none";
					if (!this.description.NullOrEmpty())
					{
						text = GenText.StableStringHash(this.description).ToString().Replace("-", "");
						text = text.Substring(0, Math.Min(3, text.Length));
					}
					this.packageId = this.ConvertToASCII(this.author + text) + "." + this.ConvertToASCII(this.name);
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)");
					}
					flag = true;
				}
				if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(this.packageId))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Mod ",
							this.name,
							" <packageId> (",
							this.packageId,
							") is not in valid format."
						}));
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains("ludeon"))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.");
					}
					flag = true;
				}
				return !flag;
			}

			// Token: 0x0600F3C9 RID: 62409 RVA: 0x00592100 File Offset: 0x00590300
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600F3CA RID: 62410 RVA: 0x00592158 File Offset: 0x00590358
			public void ValidateDependencies(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.");
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId);
						}
						flag = true;
					}
					if (modDependency.displayName.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" has a dependency (",
								modDependency.packageId,
								") with empty display name."
							}));
						}
						flag = true;
					}
					if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains("ludeon"))
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" dependency (",
								modDependency.packageId,
								") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified."
							}));
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			// Token: 0x0600F3CB RID: 62411 RVA: 0x005922C4 File Offset: 0x005904C4
			public void InitVersionedData()
			{
				string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
				string fallbackVer = null;
				if (!this.SupportedVersions.Any(new Predicate<Version>(VersionControl.IsCompatible)))
				{
					Version version = (from v in this.SupportedVersions
					where v < VersionControl.CurrentVersion
					select v).Max<Version>();
					if (version != null)
					{
						fallbackVer = version.Major.ToString() + "." + version.Minor.ToString();
					}
				}
				ModMetaData.VersionedData<string> versionedData = this.descriptionsByVersion;
				string text = (versionedData != null) ? versionedData.GetItemForVersion(currentVersionStringWithoutBuild, fallbackVer) : null;
				if (text != null)
				{
					this.description = text;
				}
				ModMetaData.VersionedData<List<ModDependency>> versionedData2 = this.modDependenciesByVersion;
				List<ModDependency> list = (versionedData2 != null) ? versionedData2.GetItemForVersion(currentVersionStringWithoutBuild, fallbackVer) : null;
				if (list != null)
				{
					this.modDependencies = list;
				}
				ModMetaData.VersionedData<List<string>> versionedData3 = this.loadBeforeByVersion;
				List<string> list2 = (versionedData3 != null) ? versionedData3.GetItemForVersion(currentVersionStringWithoutBuild, fallbackVer) : null;
				if (list2 != null)
				{
					this.loadBefore = list2;
				}
				ModMetaData.VersionedData<List<string>> versionedData4 = this.loadAfterByVersion;
				List<string> list3 = (versionedData4 != null) ? versionedData4.GetItemForVersion(currentVersionStringWithoutBuild, fallbackVer) : null;
				if (list3 != null)
				{
					this.loadAfter = list3;
				}
				ModMetaData.VersionedData<List<string>> versionedData5 = this.incompatibleWithByVersion;
				List<string> list4 = (versionedData5 != null) ? versionedData5.GetItemForVersion(currentVersionStringWithoutBuild, fallbackVer) : null;
				if (list4 != null)
				{
					this.incompatibleWith = list4;
				}
			}

			// Token: 0x04009D8E RID: 40334
			public string packageId = "";

			// Token: 0x04009D8F RID: 40335
			public string name = "";

			// Token: 0x04009D90 RID: 40336
			public string shortName = "";

			// Token: 0x04009D91 RID: 40337
			public string author = "Anonymous";

			// Token: 0x04009D92 RID: 40338
			public List<string> authors;

			// Token: 0x04009D93 RID: 40339
			public string modIconPath = "";

			// Token: 0x04009D94 RID: 40340
			public string modVersion = "";

			// Token: 0x04009D95 RID: 40341
			public string url = "";

			// Token: 0x04009D96 RID: 40342
			public string description = "No description provided.";

			// Token: 0x04009D97 RID: 40343
			public uint steamAppId;

			// Token: 0x04009D98 RID: 40344
			public List<string> supportedVersions;

			// Token: 0x04009D99 RID: 40345
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x04009D9A RID: 40346
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x04009D9B RID: 40347
			public List<string> loadBefore = new List<string>();

			// Token: 0x04009D9C RID: 40348
			public List<string> loadAfter = new List<string>();

			// Token: 0x04009D9D RID: 40349
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x04009D9E RID: 40350
			public List<string> forceLoadBefore = new List<string>();

			// Token: 0x04009D9F RID: 40351
			public List<string> forceLoadAfter = new List<string>();

			// Token: 0x04009DA0 RID: 40352
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x04009DA1 RID: 40353
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x04009DA2 RID: 40354
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x04009DA3 RID: 40355
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x04009DA4 RID: 40356
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x04009DA5 RID: 40357
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x02002676 RID: 9846
		private class VersionedData<T> where T : class
		{
			// Token: 0x0600F3CE RID: 62414 RVA: 0x005924BC File Offset: 0x005906BC
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				foreach (object obj in xmlRoot.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!(xmlNode is XmlComment))
					{
						string text = xmlNode.Name.ToLower();
						if (text.StartsWith("v"))
						{
							text = text.Substring(1);
						}
						if (!this.itemForVersion.ContainsKey(text))
						{
							this.itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)((object)xmlNode.FirstChild.Value)) : DirectXmlToObject.ObjectFromXml<T>(xmlNode, false));
						}
						else
						{
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name);
						}
					}
				}
			}

			// Token: 0x0600F3CF RID: 62415 RVA: 0x005925BC File Offset: 0x005907BC
			public T GetItemForVersion(string ver, string fallbackVer)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				if (fallbackVer != null && this.itemForVersion.ContainsKey(fallbackVer))
				{
					return this.itemForVersion[fallbackVer];
				}
				return default(T);
			}

			// Token: 0x04009DA7 RID: 40359
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
