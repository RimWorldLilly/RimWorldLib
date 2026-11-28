using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000342 RID: 834
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x0009AAE8 File Offset: 0x00098CE8
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredAnyOfPackageIds.NullOrEmpty<string>() || ModLister.AnyModActiveNoSuffix(this.requiredAnyOfPackageIds)) && (this.requiredAllOfPackageIds.NullOrEmpty<string>() || ModLister.AllModsActiveNoSuffix(this.requiredAllOfPackageIds)) && (this.disallowedAnyOfPackageIds.NullOrEmpty<string>() || !ModLister.AnyModActiveNoSuffix(this.disallowedAnyOfPackageIds));
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0009AB48 File Offset: 0x00098D48
		public LoadFolder(string folderName, List<string> requiredAnyOfPackageIds, List<string> requiredAllOfPackageIds, List<string> disallowedAnyOfPackageIds)
		{
			this.folderName = folderName;
			this.requiredAnyOfPackageIds = requiredAnyOfPackageIds;
			this.requiredAllOfPackageIds = requiredAllOfPackageIds;
			this.disallowedAnyOfPackageIds = disallowedAnyOfPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredAnyOfPackageIds != null) ? requiredAnyOfPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredAllOfPackageIds != null) ? requiredAllOfPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedAnyOfPackageIds != null) ? disallowedAnyOfPackageIds.GetHashCode() : 0);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0009ABE3 File Offset: 0x00098DE3
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0009ABF8 File Offset: 0x00098DF8
		public override bool Equals(object obj)
		{
			LoadFolder loadFolder = obj as LoadFolder;
			return loadFolder != null && this.Equals(loadFolder);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0009AC18 File Offset: 0x00098E18
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x0400153A RID: 5434
		public string folderName;

		// Token: 0x0400153B RID: 5435
		public List<string> requiredAnyOfPackageIds;

		// Token: 0x0400153C RID: 5436
		public List<string> requiredAllOfPackageIds;

		// Token: 0x0400153D RID: 5437
		public List<string> disallowedAnyOfPackageIds;

		// Token: 0x0400153E RID: 5438
		private readonly int hashCodeCached;
	}
}
