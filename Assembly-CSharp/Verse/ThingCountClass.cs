using System;

namespace Verse
{
	// Token: 0x020006B0 RID: 1712
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x0015E002 File Offset: 0x0015C202
		// (set) Token: 0x06003659 RID: 13913 RVA: 0x0015E00C File Offset: 0x0015C20C
		public int Count
		{
			get
			{
				return this.countInt;
			}
			set
			{
				if (value < 0)
				{
					string str = "Tried to set ThingCountClass stack count to ";
					string str2 = value.ToString();
					string str3 = ". thing=";
					Thing thing = this.thing;
					Log.Warning(str + str2 + str3 + ((thing != null) ? thing.ToString() : null));
					this.countInt = 0;
					return;
				}
				if (this.thing != null && value > this.thing.stackCount)
				{
					string[] array = new string[6];
					array[0] = "Tried to set ThingCountClass stack count to ";
					array[1] = value.ToString();
					array[2] = ", but thing's stack count is only ";
					array[3] = this.thing.stackCount.ToString();
					array[4] = ". thing=";
					int num = 5;
					Thing thing2 = this.thing;
					array[num] = ((thing2 != null) ? thing2.ToString() : null);
					Log.Warning(string.Concat(array));
					this.countInt = this.thing.stackCount;
					return;
				}
				this.countInt = value;
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x0015E0DB File Offset: 0x0015C2DB
		public ThingCountClass()
		{
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x0015E0E3 File Offset: 0x0015C2E3
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x0015E0F9 File Offset: 0x0015C2F9
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x0015E120 File Offset: 0x0015C320
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.Count.ToString(),
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x0015E17E File Offset: 0x0015C37E
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x04002335 RID: 9013
		public Thing thing;

		// Token: 0x04002336 RID: 9014
		private int countInt;
	}
}
