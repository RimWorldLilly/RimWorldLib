using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCA RID: 4042
	public class PreceptWorker
	{
		// Token: 0x06006694 RID: 26260 RVA: 0x00246B4D File Offset: 0x00244D4D
		public virtual bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x06006695 RID: 26261 RVA: 0x00246B50 File Offset: 0x00244D50
		public virtual float GetThingOrder(PreceptThingChance thingChance)
		{
			return 0f;
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06006696 RID: 26262 RVA: 0x00246B57 File Offset: 0x00244D57
		public virtual IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				return from dc in this.def.buildingDefChances
				select dc;
			}
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x00246B88 File Offset: 0x00244D88
		public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			return true;
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x00246B90 File Offset: 0x00244D90
		[Obsolete("Will be removed in a future update")]
		public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			return this.CanUse(def, ideo, null);
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x00246B9B File Offset: 0x00244D9B
		public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo, FactionDef generatingFor = null)
		{
			PreceptWorker.<ThingDefsForIdeo>d__7 <ThingDefsForIdeo>d__ = new PreceptWorker.<ThingDefsForIdeo>d__7(-2);
			<ThingDefsForIdeo>d__.<>4__this = this;
			<ThingDefsForIdeo>d__.<>3__ideo = ideo;
			<ThingDefsForIdeo>d__.<>3__generatingFor = generatingFor;
			return <ThingDefsForIdeo>d__;
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x00246BB9 File Offset: 0x00244DB9
		[Obsolete("Will be removed in a future update")]
		public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo)
		{
			return this.ThingDefsForIdeo(ideo, null);
		}

		// Token: 0x04003F82 RID: 16258
		public PreceptDef def;
	}
}
