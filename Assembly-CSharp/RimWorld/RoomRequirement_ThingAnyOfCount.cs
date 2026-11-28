using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002007 RID: 8199
	public class RoomRequirement_ThingAnyOfCount : RoomRequirement_ThingAnyOf
	{
		// Token: 0x0600CFBD RID: 53181 RVA: 0x004C6DF4 File Offset: 0x004C4FF4
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.things[0].label;
			if (r != null)
			{
				return string.Concat(new string[]
				{
					text,
					" ",
					this.ThingCount(r).ToString(),
					"/",
					this.count.ToString()
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.things[0], null, this.count);
			}
			return text + " x" + this.count.ToString();
		}

		// Token: 0x0600CFBE RID: 53182 RVA: 0x004C6EAC File Offset: 0x004C50AC
		public override bool Met(Room r, Pawn p = null)
		{
			return this.ThingCount(r) >= this.count;
		}

		// Token: 0x0600CFBF RID: 53183 RVA: 0x004C6EC0 File Offset: 0x004C50C0
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.SameOrSubsetOf(other) && this.count <= (other as RoomRequirement_ThingAnyOfCount).count;
		}

		// Token: 0x0600CFC0 RID: 53184 RVA: 0x004C6EE4 File Offset: 0x004C50E4
		private int ThingCount(Room r)
		{
			int num = 0;
			foreach (Thing t in r.ContainedThingsList(this.things))
			{
				if (this.CountThing(t))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600CFC1 RID: 53185 RVA: 0x004C6F40 File Offset: 0x004C5140
		protected virtual bool CountThing(Thing t)
		{
			return true;
		}

		// Token: 0x0600CFC2 RID: 53186 RVA: 0x004C6F43 File Offset: 0x004C5143
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_ThingAnyOfCount.<ConfigErrors>d__6 <ConfigErrors>d__ = new RoomRequirement_ThingAnyOfCount.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFC3 RID: 53187 RVA: 0x004C6F53 File Offset: 0x004C5153
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04008421 RID: 33825
		public int count;
	}
}
