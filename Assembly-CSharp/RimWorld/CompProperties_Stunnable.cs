using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE8 RID: 3816
	public class CompProperties_Stunnable : CompProperties
	{
		// Token: 0x060063C4 RID: 25540 RVA: 0x0023C4F5 File Offset: 0x0023A6F5
		public CompProperties_Stunnable()
		{
			this.compClass = typeof(CompStunnable);
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x0023C523 File Offset: 0x0023A723
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Stunnable.<ConfigErrors>d__7 <ConfigErrors>d__ = new CompProperties_Stunnable.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003823 RID: 14371
		public List<DamageDef> affectedDamageDefs = new List<DamageDef>();

		// Token: 0x04003824 RID: 14372
		public List<DamageDef> adaptableDamageDefs = new List<DamageDef>();

		// Token: 0x04003825 RID: 14373
		public bool useLargeEMPEffecter;

		// Token: 0x04003826 RID: 14374
		public Vector3? empEffecterDimensions;

		// Token: 0x04003827 RID: 14375
		public Vector3? empEffecterOffset;

		// Token: 0x04003828 RID: 14376
		public float? empChancePerTick;
	}
}
