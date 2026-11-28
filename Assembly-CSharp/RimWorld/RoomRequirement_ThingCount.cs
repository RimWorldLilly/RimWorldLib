using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002005 RID: 8197
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		// Token: 0x0600CFAE RID: 53166 RVA: 0x004C6B4E File Offset: 0x004C4D4E
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		// Token: 0x0600CFAF RID: 53167 RVA: 0x004C6B62 File Offset: 0x004C4D62
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		// Token: 0x0600CFB0 RID: 53168 RVA: 0x004C6B70 File Offset: 0x004C4D70
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.thingDef.label;
			if (r != null)
			{
				return string.Concat(new string[]
				{
					text,
					" ",
					this.Count(r).ToString(),
					"/",
					this.count.ToString()
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
			return text + " x" + this.count.ToString();
		}

		// Token: 0x0600CFB1 RID: 53169 RVA: 0x004C6C1C File Offset: 0x004C4E1C
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_ThingCount.<ConfigErrors>d__4 <ConfigErrors>d__ = new RoomRequirement_ThingCount.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFB2 RID: 53170 RVA: 0x004C6C2C File Offset: 0x004C4E2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x0400841F RID: 33823
		public int count;
	}
}
