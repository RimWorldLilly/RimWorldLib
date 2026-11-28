using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020006B1 RID: 1713
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x0015E193 File Offset: 0x0015C393
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x0015E19B File Offset: 0x0015C39B
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x0015E1A3 File Offset: 0x0015C3A3
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x0015E1B7 File Offset: 0x0015C3B7
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x0015E1CC File Offset: 0x0015C3CC
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set ");
				defaultInterpolatedStringHandler.AppendFormatted("ThingDefCount");
				defaultInterpolatedStringHandler.AppendLiteral(" count to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral(". thingDef=");
				defaultInterpolatedStringHandler.AppendFormatted<ThingDef>(thingDef);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0015E244 File Offset: 0x0015C444
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x0015E268 File Offset: 0x0015C468
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x0015E276 File Offset: 0x0015C476
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x0015E28E File Offset: 0x0015C48E
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x0015E29C File Offset: 0x0015C49C
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x0015E2BC File Offset: 0x0015C4BC
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x0015E2C8 File Offset: 0x0015C4C8
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x0015E2DC File Offset: 0x0015C4DC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.count);
			defaultInterpolatedStringHandler.AppendLiteral("x ");
			defaultInterpolatedStringHandler.AppendFormatted((this.thingDef != null) ? this.thingDef.defName : "null");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x0015E34B File Offset: 0x0015C54B
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x04002337 RID: 9015
		private ThingDef thingDef;

		// Token: 0x04002338 RID: 9016
		private int count;
	}
}
