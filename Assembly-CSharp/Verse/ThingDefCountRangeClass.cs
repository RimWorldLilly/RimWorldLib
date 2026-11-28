using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020006B4 RID: 1716
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600368E RID: 13966 RVA: 0x0015E78C File Offset: 0x0015C98C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x0015E799 File Offset: 0x0015C999
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x0015E7A6 File Offset: 0x0015C9A6
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x0015E7B3 File Offset: 0x0015C9B3
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x0015E7C0 File Offset: 0x0015C9C0
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x0015E7C8 File Offset: 0x0015C9C8
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x0015E7D8 File Offset: 0x0015C9D8
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x0015E7F0 File Offset: 0x0015C9F0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x0015E828 File Offset: 0x0015CA28
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null, null);
			this.countRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0015E884 File Offset: 0x0015CA84
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			IntRange intRange = this.countRange;
			array[num] = intRange.ToString();
			array[2] = "x ";
			array[3] = ((this.thingDef != null) ? this.thingDef.defName : "null");
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0015E8E8 File Offset: 0x0015CAE8
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x0015E8FD File Offset: 0x0015CAFD
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0015E919 File Offset: 0x0015CB19
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04002341 RID: 9025
		public ThingDef thingDef;

		// Token: 0x04002342 RID: 9026
		public IntRange countRange;
	}
}
