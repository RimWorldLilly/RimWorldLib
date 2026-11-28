using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000135 RID: 309
	public class CompProperties
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x00036FEB File Offset: 0x000351EB
		public CompProperties()
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00037003 File Offset: 0x00035203
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00037022 File Offset: 0x00035222
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00037024 File Offset: 0x00035224
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003703B File Offset: 0x0003523B
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0003703D File Offset: 0x0003523D
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00037044 File Offset: 0x00035244
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00037046 File Offset: 0x00035246
		public virtual void Notify_PostUnlockedByResearch(ThingDef parent)
		{
		}

		// Token: 0x04000594 RID: 1428
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
