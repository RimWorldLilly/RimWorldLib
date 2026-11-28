using System;
using System.Runtime.CompilerServices;

namespace RimWorld
{
	// Token: 0x02001FB8 RID: 8120
	public struct StatCacheEntry
	{
		// Token: 0x0600CE2C RID: 52780 RVA: 0x004BB1D2 File Offset: 0x004B93D2
		public StatCacheEntry(float statValue, int gameTick)
		{
			this.statValue = statValue;
			this.gameTick = gameTick;
		}

		// Token: 0x0600CE2D RID: 52781 RVA: 0x004BB1E4 File Offset: 0x004B93E4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
			defaultInterpolatedStringHandler.AppendLiteral("StatCacheEntry(");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.statValue);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gameTick);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600CE2E RID: 52782 RVA: 0x004BB240 File Offset: 0x004B9440
		public override int GetHashCode()
		{
			return this.statValue.GetHashCode() ^ this.gameTick.GetHashCode();
		}

		// Token: 0x0600CE2F RID: 52783 RVA: 0x004BB25C File Offset: 0x004B945C
		public override bool Equals(object obj)
		{
			if (obj is StatCacheEntry)
			{
				StatCacheEntry statCacheEntry = (StatCacheEntry)obj;
				return this.Equals(statCacheEntry);
			}
			return false;
		}

		// Token: 0x0400834D RID: 33613
		public float statValue;

		// Token: 0x0400834E RID: 33614
		public int gameTick;
	}
}
