using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020002FD RID: 765
	public class RegionLinkDatabase
	{
		// Token: 0x0600175D RID: 5981 RVA: 0x0008C828 File Offset: 0x0008AA28
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0008C868 File Offset: 0x0008AA68
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0008C87C File Offset: 0x0008AA7C
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x040013DD RID: 5085
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
