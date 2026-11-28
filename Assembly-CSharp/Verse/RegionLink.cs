using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020002FC RID: 764
	public class RegionLink
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x0008C57F File Offset: 0x0008A77F
		// (set) Token: 0x06001754 RID: 5972 RVA: 0x0008C589 File Offset: 0x0008A789
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x0008C594 File Offset: 0x0008A794
		// (set) Token: 0x06001756 RID: 5974 RVA: 0x0008C59E File Offset: 0x0008A79E
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0008C5AC File Offset: 0x0008A7AC
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to double-register region ");
				defaultInterpolatedStringHandler.AppendFormatted<Region>(reg);
				defaultInterpolatedStringHandler.AppendLiteral(" in ");
				defaultInterpolatedStringHandler.AppendFormatted<RegionLink>(this);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
				return;
			}
			if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(77, 4);
			defaultInterpolatedStringHandler2.AppendLiteral("Could not register region ");
			defaultInterpolatedStringHandler2.AppendFormatted<Region>(reg);
			defaultInterpolatedStringHandler2.AppendLiteral(" in link ");
			defaultInterpolatedStringHandler2.AppendFormatted<RegionLink>(this);
			defaultInterpolatedStringHandler2.AppendLiteral(": > 2 regions on link!\nRegionA: ");
			defaultInterpolatedStringHandler2.AppendFormatted(this.RegionA.DebugString);
			defaultInterpolatedStringHandler2.AppendLiteral("\nRegionB: ");
			defaultInterpolatedStringHandler2.AppendFormatted(this.RegionB.DebugString);
			Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0008C6C4 File Offset: 0x0008A8C4
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					Map map = reg.Map;
					if (map == null)
					{
						return;
					}
					map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
					return;
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					Map map2 = reg.Map;
					if (map2 == null)
					{
						return;
					}
					map2.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0008C72E File Offset: 0x0008A92E
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0008C746 File Offset: 0x0008A946
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0008C754 File Offset: 0x0008A954
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false, false);
			string text2 = "span=" + this.span.ToString() + " hash=" + this.UniqueHashCode().ToString();
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		// Token: 0x040013DB RID: 5083
		public Region[] regions = new Region[2];

		// Token: 0x040013DC RID: 5084
		public EdgeSpan span;
	}
}
