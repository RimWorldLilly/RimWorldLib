using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002EF RID: 751
	public class ReachabilityCache
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x00088AF4 File Offset: 0x00086CF4
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00088B04 File Offset: 0x00086D04
		public BoolUnknown CachedResultFor(District A, District B, TraverseParms traverseParams)
		{
			bool flag;
			if (!this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return BoolUnknown.Unknown;
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.True;
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00088B3C File Offset: 0x00086D3C
		public void AddCachedResult(District A, District B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00088B79 File Offset: 0x00086D79
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00088B88 File Offset: 0x00086D88
		public void ClearFor(Pawn p)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				if (keyValuePair.Key.TraverseParms.pawn == p)
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00088C3C File Offset: 0x00086E3C
		public void ClearForHostile(Thing hostileTo)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				Pawn pawn = keyValuePair.Key.TraverseParms.pawn;
				if (pawn != null && pawn.HostileTo(hostileTo))
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x0400138E RID: 5006
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x0400138F RID: 5007
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x02002628 RID: 9768
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x17002AA4 RID: 10916
			// (get) Token: 0x0600F240 RID: 62016 RVA: 0x0058D4EB File Offset: 0x0058B6EB
			// (set) Token: 0x0600F241 RID: 62017 RVA: 0x0058D4F3 File Offset: 0x0058B6F3
			public int FirstID { readonly get; private set; }

			// Token: 0x17002AA5 RID: 10917
			// (get) Token: 0x0600F242 RID: 62018 RVA: 0x0058D4FC File Offset: 0x0058B6FC
			// (set) Token: 0x0600F243 RID: 62019 RVA: 0x0058D504 File Offset: 0x0058B704
			public int SecondID { readonly get; private set; }

			// Token: 0x17002AA6 RID: 10918
			// (get) Token: 0x0600F244 RID: 62020 RVA: 0x0058D50D File Offset: 0x0058B70D
			// (set) Token: 0x0600F245 RID: 62021 RVA: 0x0058D515 File Offset: 0x0058B715
			public TraverseParms TraverseParms { readonly get; private set; }

			// Token: 0x0600F246 RID: 62022 RVA: 0x0058D51E File Offset: 0x0058B71E
			public CachedEntry(int firstID, int secondID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstID < secondID)
				{
					this.FirstID = firstID;
					this.SecondID = secondID;
				}
				else
				{
					this.FirstID = secondID;
					this.SecondID = firstID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x0600F247 RID: 62023 RVA: 0x0058D550 File Offset: 0x0058B750
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600F248 RID: 62024 RVA: 0x0058D55A File Offset: 0x0058B75A
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x0600F249 RID: 62025 RVA: 0x0058D567 File Offset: 0x0058B767
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x0600F24A RID: 62026 RVA: 0x0058D57F File Offset: 0x0058B77F
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstID == other.FirstID && this.SecondID == other.SecondID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x0600F24B RID: 62027 RVA: 0x0058D5B3 File Offset: 0x0058B7B3
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstID, this.SecondID), this.TraverseParms);
			}
		}
	}
}
