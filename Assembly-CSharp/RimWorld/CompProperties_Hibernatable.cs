using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF3 RID: 3827
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x060063D1 RID: 25553 RVA: 0x0023C703 File Offset: 0x0023A903
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x0023C726 File Offset: 0x0023A926
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Hibernatable.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_Hibernatable.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400384F RID: 14415
		public float startupDays = 14f;

		// Token: 0x04003850 RID: 14416
		public IncidentTargetTagDef incidentTargetWhileStarting;

		// Token: 0x04003851 RID: 14417
		public SoundDef sustainerActive;
	}
}
