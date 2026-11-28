using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B8 RID: 6328
	public class CompHasSources : ThingComp
	{
		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x06009EA2 RID: 40610 RVA: 0x0038EDDE File Offset: 0x0038CFDE
		public CompProperties_HasSources Props
		{
			get
			{
				return (CompProperties_HasSources)this.props;
			}
		}

		// Token: 0x06009EA3 RID: 40611 RVA: 0x0038EDEB File Offset: 0x0038CFEB
		public void AddSource(string sourceLabel)
		{
			if (!sourceLabel.NullOrEmpty() && !this.sources.Contains(sourceLabel))
			{
				this.sources.Add(sourceLabel);
			}
		}

		// Token: 0x06009EA4 RID: 40612 RVA: 0x0038EE10 File Offset: 0x0038D010
		public void TransferSourcesTo(CompHasSources other, int count = -1)
		{
			if (count < 0)
			{
				count = this.sources.Count;
			}
			int num = 0;
			string text;
			while (num < count && this.sources.TryRandomElement(out text))
			{
				other.AddSource(text);
				this.sources.Remove(text);
				num++;
			}
		}

		// Token: 0x06009EA5 RID: 40613 RVA: 0x0038EE60 File Offset: 0x0038D060
		public override string TransformLabel(string label)
		{
			if (!this.Props.affectLabel || this.sources.NullOrEmpty<string>() || this.sources.Count > 1 || this.parent.stackCount > 1)
			{
				return label;
			}
			return "ThingOfSource".Translate(label, this.sources[0]);
		}

		// Token: 0x06009EA6 RID: 40614 RVA: 0x0038EECC File Offset: 0x0038D0CC
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			CompHasSources compHasSources = otherStack.TryGetComp<CompHasSources>();
			if (compHasSources == null)
			{
				return;
			}
			compHasSources.TransferSourcesTo(this, count);
		}

		// Token: 0x06009EA7 RID: 40615 RVA: 0x0038EEEC File Offset: 0x0038D0EC
		public override void PostSplitOff(Thing piece)
		{
			CompHasSources compHasSources = piece.TryGetComp<CompHasSources>();
			if (compHasSources == null || piece == this.parent)
			{
				return;
			}
			compHasSources.sources.Clear();
			this.TransferSourcesTo(compHasSources, piece.stackCount);
		}

		// Token: 0x06009EA8 RID: 40616 RVA: 0x0038EF28 File Offset: 0x0038D128
		public override string CompInspectStringExtra()
		{
			if (this.sources.NullOrEmpty<string>())
			{
				return base.CompInspectStringExtra();
			}
			string text = string.Empty;
			if (this.sources.Count > 1 || !this.Props.affectLabel)
			{
				this.tmpSourceLabels.Clear();
				int num = Mathf.Min(this.sources.Count, 9);
				for (int i = 0; i < num; i++)
				{
					this.tmpSourceLabels.Add(this.sources[i]);
				}
				if (this.sources.Count > 9)
				{
					this.tmpSourceLabels.Add("Etc".Translate() + "...");
				}
				string str = (this.Props.inspectStringLabel ?? "ThingOf".Translate(Find.ActiveLanguageWorker.Pluralize(this.parent.LabelNoCount, -1))).CapitalizeFirst();
				text = text + str + ": " + this.tmpSourceLabels.ToCommaList(false, false);
			}
			return text;
		}

		// Token: 0x06009EA9 RID: 40617 RVA: 0x0038F03B File Offset: 0x0038D23B
		public override void PostExposeData()
		{
			Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.sources == null)
			{
				this.sources = new List<string>();
			}
		}

		// Token: 0x04005DE3 RID: 24035
		private List<string> sources = new List<string>();

		// Token: 0x04005DE4 RID: 24036
		private const int MaxSourcesToDisplayInInspectString = 9;

		// Token: 0x04005DE5 RID: 24037
		private List<string> tmpSourceLabels = new List<string>();
	}
}
