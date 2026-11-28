using System;

namespace Verse
{
	// Token: 0x020006AE RID: 1710
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x0015DD7D File Offset: 0x0015BF7D
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x0015DD85 File Offset: 0x0015BF85
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x0015DD90 File Offset: 0x0015BF90
		public ThingCount(Thing thing, int count, bool ignoreStackLimit = false)
		{
			if (count < 0)
			{
				Log.Warning("Tried to set ThingCount stack count to " + count.ToString() + ". thing=" + ((thing != null) ? thing.ToString() : null));
				count = 0;
			}
			if (count > thing.stackCount && !ignoreStackLimit)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ThingCount stack count to ",
					count.ToString(),
					", but thing's stack count is only ",
					thing.stackCount.ToString(),
					". thing=",
					(thing != null) ? thing.ToString() : null
				}));
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x0015DE3E File Offset: 0x0015C03E
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x0015DE63 File Offset: 0x0015C063
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount, false);
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x0015DE72 File Offset: 0x0015C072
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x0015DE8A File Offset: 0x0015C08A
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x0015DE98 File Offset: 0x0015C098
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x0015DEB8 File Offset: 0x0015C0B8
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x0015DEC4 File Offset: 0x0015C0C4
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x0015DED8 File Offset: 0x0015C0D8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.count.ToString(),
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x0015DF33 File Offset: 0x0015C133
		public static implicit operator ThingCount(ThingCountClass t)
		{
			if (t == null)
			{
				return new ThingCount(null, 0, false);
			}
			return new ThingCount(t.thing, t.Count, false);
		}

		// Token: 0x04002333 RID: 9011
		private Thing thing;

		// Token: 0x04002334 RID: 9012
		private int count;
	}
}
