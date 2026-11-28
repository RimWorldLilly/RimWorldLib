using System;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200004E RID: 78
	public class TableDataGetter<T>
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000FB48 File Offset: 0x0000DD48
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000FB84 File Offset: 0x0000DD84
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return "";
				}
				return thingDef.defName;
			};
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04000126 RID: 294
		public string label;

		// Token: 0x04000127 RID: 295
		public Func<T, string> getter;
	}
}
