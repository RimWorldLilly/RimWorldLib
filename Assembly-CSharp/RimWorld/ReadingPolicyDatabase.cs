using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001023 RID: 4131
	public class ReadingPolicyDatabase : IExposable
	{
		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x06006809 RID: 26633 RVA: 0x0024C280 File Offset: 0x0024A480
		public List<ReadingPolicy> AllReadingPolicies
		{
			get
			{
				return this.readingPolicies;
			}
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x0024C288 File Offset: 0x0024A488
		public ReadingPolicyDatabase()
		{
			this.GenerateStartingPolicies();
		}

		// Token: 0x0600680B RID: 26635 RVA: 0x0024C2A1 File Offset: 0x0024A4A1
		public void ExposeData()
		{
			Scribe_Collections.Look<ReadingPolicy>(ref this.readingPolicies, "readingPolicies", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600680C RID: 26636 RVA: 0x0024C2BF File Offset: 0x0024A4BF
		public ReadingPolicy DefaultReadingPolicy()
		{
			if (this.readingPolicies.Count == 0)
			{
				this.MakeNewReadingPolicy();
			}
			return this.readingPolicies[0];
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x0024C2E4 File Offset: 0x0024A4E4
		public void SetDefault(ReadingPolicy policy)
		{
			int index = this.readingPolicies.IndexOf(policy);
			ReadingPolicy value = this.readingPolicies[0];
			this.readingPolicies[0] = policy;
			this.readingPolicies[index] = value;
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x0024C328 File Offset: 0x0024A528
		public AcceptanceReport TryDelete(ReadingPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.reading != null && pawn.reading.CurrentPolicy == policy)
				{
					return new AcceptanceReport("ReadingPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.reading != null && pawn2.reading.CurrentPolicy == policy)
				{
					pawn2.reading.CurrentPolicy = null;
				}
			}
			this.readingPolicies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x0024C418 File Offset: 0x0024A618
		public ReadingPolicy MakeNewReadingPolicy()
		{
			int num;
			if (!this.readingPolicies.Any<ReadingPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.readingPolicies.Max((ReadingPolicy o) => o.id) + 1;
			}
			int num2 = num;
			int id = num2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ReadingPolicy".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(num2.ToString());
			ReadingPolicy readingPolicy = new ReadingPolicy(id, defaultInterpolatedStringHandler.ToStringAndClear());
			this.readingPolicies.Add(readingPolicy);
			return readingPolicy;
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x0024C4B4 File Offset: 0x0024A6B4
		private void GenerateStartingPolicies()
		{
			ReadingPolicy readingPolicy = this.MakeNewReadingPolicy();
			readingPolicy.label = "AllReadingPolicy".Translate();
			readingPolicy.defFilter.SetDisallowAll(null, null);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (thingDef.thingClass.SameOrSubclassOf<Book>())
				{
					readingPolicy.defFilter.SetAllow(thingDef, true);
				}
			}
			ReadingPolicy readingPolicy2 = this.MakeNewReadingPolicy();
			readingPolicy2.label = "TextbookPolicy".Translate();
			readingPolicy2.defFilter.SetDisallowAll(null, null);
			readingPolicy2.defFilter.SetAllow(ThingDefOf.TextBook, true);
			ReadingPolicy readingPolicy3 = this.MakeNewReadingPolicy();
			readingPolicy3.label = "SchematicPolicy".Translate();
			readingPolicy3.defFilter.SetDisallowAll(null, null);
			readingPolicy3.defFilter.SetAllow(ThingDefOf.Schematic, true);
			ReadingPolicy readingPolicy4 = this.MakeNewReadingPolicy();
			readingPolicy4.label = "NovelPolicy".Translate();
			readingPolicy4.defFilter.SetDisallowAll(null, null);
			readingPolicy4.defFilter.SetAllow(ThingDefOf.Novel, true);
			if (ModsConfig.AnomalyActive)
			{
				ReadingPolicy readingPolicy5 = this.MakeNewReadingPolicy();
				readingPolicy5.label = "TomePolicy".Translate();
				readingPolicy5.defFilter.SetDisallowAll(null, null);
				readingPolicy5.defFilter.SetAllow(ThingDefOf.Tome, true);
			}
			if (ModsConfig.OdysseyActive)
			{
				ReadingPolicy readingPolicy6 = this.MakeNewReadingPolicy();
				readingPolicy6.label = "MapPolicy".Translate();
				readingPolicy6.defFilter.SetDisallowAll(null, null);
				readingPolicy6.defFilter.SetAllow(ThingDefOf.Map, true);
			}
			ReadingPolicy readingPolicy7 = this.MakeNewReadingPolicy();
			readingPolicy7.label = "NoneReadingPolicy".Translate();
			readingPolicy7.defFilter.SetDisallowAll(null, null);
		}

		// Token: 0x0400424D RID: 16973
		private List<ReadingPolicy> readingPolicies = new List<ReadingPolicy>();
	}
}
