using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDF RID: 4063
	public class ScenarioDef : Def
	{
		// Token: 0x060066D1 RID: 26321 RVA: 0x002478CC File Offset: 0x00245ACC
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x00247931 File Offset: 0x00245B31
		public override IEnumerable<string> ConfigErrors()
		{
			ScenarioDef.<ConfigErrors>d__2 <ConfigErrors>d__ = new ScenarioDef.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400400A RID: 16394
		public Scenario scenario;
	}
}
