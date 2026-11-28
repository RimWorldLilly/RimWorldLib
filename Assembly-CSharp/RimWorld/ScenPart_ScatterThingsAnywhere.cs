using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BD RID: 6077
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x06009795 RID: 38805 RVA: 0x0036FBE0 File Offset: 0x0036DDE0
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009796 RID: 38806 RVA: 0x0036FBE3 File Offset: 0x0036DDE3
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06009797 RID: 38807 RVA: 0x0036FBFF File Offset: 0x0036DDFF
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>d__4 <GetSummaryListEntries>d__ = new ScenPart_ScatterThingsAnywhere.<GetSummaryListEntries>d__4(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x04005AC1 RID: 23233
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
