using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017CF RID: 6095
	public abstract class TaleData : IExposable
	{
		// Token: 0x0600981A RID: 38938
		public abstract void ExposeData();

		// Token: 0x0600981B RID: 38939 RVA: 0x00372668 File Offset: 0x00370868
		public virtual IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			TaleData.<GetRules>d__1 <GetRules>d__ = new TaleData.<GetRules>d__1(-2);
			<GetRules>d__.<>4__this = this;
			return <GetRules>d__;
		}

		// Token: 0x0600981C RID: 38940 RVA: 0x00372678 File Offset: 0x00370878
		public virtual IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			TaleData.<GetRules>d__2 <GetRules>d__ = new TaleData.<GetRules>d__2(-2);
			<GetRules>d__.<>4__this = this;
			return <GetRules>d__;
		}
	}
}
