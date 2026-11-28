using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AA RID: 6058
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x060096FA RID: 38650 RVA: 0x0036C748 File Offset: 0x0036A948
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x060096FB RID: 38651
		protected abstract string IncidentTag { get; }

		// Token: 0x060096FC RID: 38652 RVA: 0x0036C750 File Offset: 0x0036A950
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.incident == null)
			{
				this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
				Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>());
			}
		}

		// Token: 0x060096FD RID: 38653 RVA: 0x0036C7AE File Offset: 0x0036A9AE
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.incident != null) ? this.incident.GetHashCode() : 0);
		}

		// Token: 0x060096FE RID: 38654 RVA: 0x0036C7D0 File Offset: 0x0036A9D0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x060096FF RID: 38655 RVA: 0x0036C7F4 File Offset: 0x0036A9F4
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x06009700 RID: 38656 RVA: 0x0036C829 File Offset: 0x0036AA29
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_IncidentBase.<GetSummaryListEntries>d__9 <GetSummaryListEntries>d__ = new ScenPart_IncidentBase.<GetSummaryListEntries>d__9(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x06009701 RID: 38657 RVA: 0x0036C840 File Offset: 0x0036AA40
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x06009702 RID: 38658 RVA: 0x0036C854 File Offset: 0x0036AA54
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x06009703 RID: 38659 RVA: 0x0036C87C File Offset: 0x0036AA7C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x06009704 RID: 38660 RVA: 0x0036C8A6 File Offset: 0x0036AAA6
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x06009705 RID: 38661 RVA: 0x0036C8B0 File Offset: 0x0036AAB0
		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate()
				{
					this.incident = id;
				});
			}
		}

		// Token: 0x04005A7D RID: 23165
		protected IncidentDef incident;
	}
}
