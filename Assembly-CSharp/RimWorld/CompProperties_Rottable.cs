using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE4 RID: 3812
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x060063B9 RID: 25529 RVA: 0x0023C3B6 File Offset: 0x0023A5B6
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x060063BA RID: 25530 RVA: 0x0023C3C9 File Offset: 0x0023A5C9
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x060063BB RID: 25531 RVA: 0x0023C3DC File Offset: 0x0023A5DC
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x0023C415 File Offset: 0x0023A615
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x060063BD RID: 25533 RVA: 0x0023C445 File Offset: 0x0023A645
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Rottable.<ConfigErrors>d__12 <ConfigErrors>d__ = new CompProperties_Rottable.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003816 RID: 14358
		public float daysToRotStart = 2f;

		// Token: 0x04003817 RID: 14359
		public bool rotDestroys;

		// Token: 0x04003818 RID: 14360
		public float rotDamagePerDay = 40f;

		// Token: 0x04003819 RID: 14361
		public float daysToDessicated = 999f;

		// Token: 0x0400381A RID: 14362
		public float dessicatedDamagePerDay;

		// Token: 0x0400381B RID: 14363
		public bool disableIfHatcher;
	}
}
