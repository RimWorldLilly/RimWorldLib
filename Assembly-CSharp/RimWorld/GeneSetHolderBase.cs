using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1D RID: 6941
	public abstract class GeneSetHolderBase : ThingWithComps
	{
		// Token: 0x17002006 RID: 8198
		// (get) Token: 0x0600B1AD RID: 45485 RVA: 0x003E98E2 File Offset: 0x003E7AE2
		public GeneSet GeneSet
		{
			get
			{
				return this.geneSet;
			}
		}

		// Token: 0x17002007 RID: 8199
		// (get) Token: 0x0600B1AE RID: 45486 RVA: 0x003E98EA File Offset: 0x003E7AEA
		protected virtual string InspectGeneDescription
		{
			get
			{
				return "InspectGenesDesc".Translate(this);
			}
		}

		// Token: 0x17002008 RID: 8200
		// (get) Token: 0x0600B1AF RID: 45487 RVA: 0x003E9904 File Offset: 0x003E7B04
		public override string DescriptionDetailed
		{
			get
			{
				this.tmpGeneLabelsDesc.Clear();
				string text = base.DescriptionDetailed;
				if (this.geneSet == null || !this.geneSet.GenesListForReading.Any<GeneDef>())
				{
					return text;
				}
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				for (int i = 0; i < this.geneSet.GenesListForReading.Count; i++)
				{
					this.tmpGeneLabelsDesc.Add(this.geneSet.GenesListForReading[i].label);
				}
				return text + ("Genes".Translate().CapitalizeFirst() + ":\n" + this.tmpGeneLabelsDesc.ToLineList("  - ", true));
			}
		}

		// Token: 0x0600B1B0 RID: 45488 RVA: 0x003E99CE File Offset: 0x003E7BCE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			GeneSetHolderBase.<SpecialDisplayStats>d__10 <SpecialDisplayStats>d__ = new GeneSetHolderBase.<SpecialDisplayStats>d__10(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600B1B1 RID: 45489 RVA: 0x003E99E0 File Offset: 0x003E7BE0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			this.tmpGeneLabels.Clear();
			if (this.geneSet != null && this.geneSet.GenesListForReading.Any<GeneDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				List<GeneDef> genesListForReading = this.geneSet.GenesListForReading;
				int num = Mathf.Min(5, genesListForReading.Count);
				for (int i = 0; i < num; i++)
				{
					string text2 = genesListForReading[i].label;
					if (this.geneSet.IsOverridden(genesListForReading[i]))
					{
						text2 += " (" + "Overridden".Translate() + ")";
					}
					this.tmpGeneLabels.Add(text2);
				}
				if (genesListForReading.Count > num)
				{
					this.tmpGeneLabels.Add("Etc".Translate() + "...");
				}
				text += "Genes".Translate().CapitalizeFirst() + ":\n" + this.tmpGeneLabels.ToLineList("  - ", true);
			}
			return text;
		}

		// Token: 0x0600B1B2 RID: 45490 RVA: 0x003E9B20 File Offset: 0x003E7D20
		public override IEnumerable<Gizmo> GetGizmos()
		{
			GeneSetHolderBase.<GetGizmos>d__13 <GetGizmos>d__ = new GeneSetHolderBase.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B1B3 RID: 45491 RVA: 0x003E9B30 File Offset: 0x003E7D30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<GeneSet>(ref this.geneSet, "geneSet", Array.Empty<object>());
		}

		// Token: 0x0400669F RID: 26271
		protected GeneSet geneSet;

		// Token: 0x040066A0 RID: 26272
		private const int MaxGeneLabels = 5;

		// Token: 0x040066A1 RID: 26273
		public static readonly CachedTexture GeneticInfoTex = new CachedTexture("UI/Gizmos/ViewGenes");

		// Token: 0x040066A2 RID: 26274
		private List<string> tmpGeneLabelsDesc = new List<string>();

		// Token: 0x040066A3 RID: 26275
		private List<string> tmpGeneLabels = new List<string>();
	}
}
