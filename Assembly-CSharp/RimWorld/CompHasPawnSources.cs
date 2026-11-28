using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BA RID: 6330
	public class CompHasPawnSources : ThingComp
	{
		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x06009EAC RID: 40620 RVA: 0x0038F0AB File Offset: 0x0038D2AB
		public CompProperties_HasPawnSources Props
		{
			get
			{
				return (CompProperties_HasPawnSources)this.props;
			}
		}

		// Token: 0x06009EAD RID: 40621 RVA: 0x0038F0B8 File Offset: 0x0038D2B8
		public void AddSource(Pawn pawn)
		{
			if (pawn == null || this.pawnSources.Contains(pawn))
			{
				return;
			}
			Find.WorldPawns.AddPawnSource(pawn, this);
			this.pawnSources.Add(pawn);
		}

		// Token: 0x06009EAE RID: 40622 RVA: 0x0038F0E4 File Offset: 0x0038D2E4
		public override bool AllowStackWith(Thing other)
		{
			CompHasPawnSources compHasPawnSources = other.TryGetComp<CompHasPawnSources>();
			if (compHasPawnSources == null || compHasPawnSources.pawnSources.Count != this.pawnSources.Count)
			{
				return false;
			}
			foreach (Pawn item in this.pawnSources)
			{
				if (!compHasPawnSources.pawnSources.Contains(item))
				{
					return false;
				}
			}
			return base.AllowStackWith(other);
		}

		// Token: 0x06009EAF RID: 40623 RVA: 0x0038F170 File Offset: 0x0038D370
		public override string TransformLabel(string label)
		{
			if (!this.Props.affectLabel || this.pawnSources.NullOrEmpty<Pawn>() || this.pawnSources.Count > 2)
			{
				return label;
			}
			if (this.pawnSources.Count == 2)
			{
				return "ThingOfTwoSources".Translate(label, this.pawnSources[0].LabelShortCap, this.pawnSources[1].LabelShortCap);
			}
			return "ThingOfSource".Translate(label, this.pawnSources[0].LabelShortCap);
		}

		// Token: 0x06009EB0 RID: 40624 RVA: 0x0038F222 File Offset: 0x0038D422
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			Find.WorldPawns.RemovePawnSources(this.pawnSources, this);
		}

		// Token: 0x06009EB1 RID: 40625 RVA: 0x0038F240 File Offset: 0x0038D440
		public override void PostSplitOff(Thing piece)
		{
			CompHasPawnSources compHasPawnSources = piece.TryGetComp<CompHasPawnSources>();
			if (compHasPawnSources == null || piece == this.parent)
			{
				return;
			}
			foreach (Pawn pawn in this.pawnSources)
			{
				compHasPawnSources.AddSource(pawn);
			}
		}

		// Token: 0x06009EB2 RID: 40626 RVA: 0x0038F2A8 File Offset: 0x0038D4A8
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnSources, "pawnSources", true, LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnSources.RemoveAll((Pawn x) => x == null);
				foreach (Pawn pawn in this.pawnSources)
				{
					Find.WorldPawns.AddPawnSource(pawn, this);
				}
			}
		}

		// Token: 0x04005DE7 RID: 24039
		public List<Pawn> pawnSources = new List<Pawn>();
	}
}
