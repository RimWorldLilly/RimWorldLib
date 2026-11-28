using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020000C5 RID: 197
	public class AbilityCompProperties
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0002C598 File Offset: 0x0002A798
		public virtual bool OverridesPsyfocusCost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0002C59B File Offset: 0x0002A79B
		public virtual FloatRange PsyfocusCostRange
		{
			get
			{
				return FloatRange.ZeroToOne;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0002C5A2 File Offset: 0x0002A7A2
		public virtual string PsyfocusCostExplanation
		{
			get
			{
				return "";
			}
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002C5A9 File Offset: 0x0002A7A9
		public virtual IEnumerable<string> ExtraStatSummary()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
		public virtual IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			AbilityCompProperties.<ConfigErrors>d__8 <ConfigErrors>d__ = new AbilityCompProperties.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000363 RID: 867
		[TranslationHandle]
		public Type compClass;
	}
}
