using System;
using System.Collections.Generic;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079D RID: 1949
	public sealed class ProfilerSearchModel
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x0018BE61 File Offset: 0x0018A061
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x0018BE69 File Offset: 0x0018A069
		public string Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.query == value)
				{
					return;
				}
				this.query = value;
				this.matchCache.Clear();
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x0018BE96 File Offset: 0x0018A096
		public bool IsActive
		{
			get
			{
				return this.Query.Length > 0;
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x0018BEA8 File Offset: 0x0018A0A8
		public bool Matches(ProfilerLabelId labelId)
		{
			if (!this.IsActive)
			{
				return true;
			}
			if (!labelId.IsValid)
			{
				return false;
			}
			bool flag;
			if (!this.matchCache.TryGetValue(labelId, out flag))
			{
				flag = FrameProfiler.GetLabel(labelId).Contains(this.Query, StringComparison.OrdinalIgnoreCase);
				this.matchCache.Add(labelId, flag);
			}
			return flag;
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0018BEFB File Offset: 0x0018A0FB
		public bool Matches(string label)
		{
			return !this.IsActive || (label != null && label.Contains(this.Query, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x040028FA RID: 10490
		private readonly Dictionary<ProfilerLabelId, bool> matchCache = new Dictionary<ProfilerLabelId, bool>();

		// Token: 0x040028FB RID: 10491
		private string query = "";
	}
}
