using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000142 RID: 322
	public class HediffCompProperties
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x000384F8 File Offset: 0x000366F8
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000384FA File Offset: 0x000366FA
		public virtual void ResolveReferences(HediffDef parent)
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000384FC File Offset: 0x000366FC
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties.<ConfigErrors>d__3 <ConfigErrors>d__ = new HediffCompProperties.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400060E RID: 1550
		[TranslationHandle]
		public Type compClass;
	}
}
