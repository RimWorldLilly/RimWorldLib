using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A4 RID: 4260
	public class QuestPart_AddHediff : QuestPart
	{
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06006A97 RID: 27287 RVA: 0x002577DA File Offset: 0x002559DA
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_AddHediff.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_AddHediff.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06006A98 RID: 27288 RVA: 0x002577EA File Offset: 0x002559EA
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_AddHediff.<get_Hyperlinks>d__9 <get_Hyperlinks>d__ = new QuestPart_AddHediff.<get_Hyperlinks>d__9(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x06006A99 RID: 27289 RVA: 0x002577FC File Offset: 0x002559FC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull() && (!this.checkDiseaseContractChance || Rand.Chance(this.pawns[i].health.immunity.DiseaseContractChanceFactor(this.hediffDef, null))))
					{
						HediffGiverUtility.TryApply(this.pawns[i], this.hediffDef, this.partsToAffect, false, 1, null, true);
					}
				}
			}
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x002578A0 File Offset: 0x00255AA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<BodyPartDef>(ref this.partsToAffect, "partsToAffect", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Values.Look<bool>(ref this.checkDiseaseContractChance, "checkDiseaseContractChance", false, false);
			Scribe_Values.Look<bool>(ref this.addToHyperlinks, "addToHyperlinks", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x00257958 File Offset: 0x00255B58
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x002579A8 File Offset: 0x00255BA8
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040043BE RID: 17342
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040043BF RID: 17343
		public List<BodyPartDef> partsToAffect;

		// Token: 0x040043C0 RID: 17344
		public string inSignal;

		// Token: 0x040043C1 RID: 17345
		public HediffDef hediffDef;

		// Token: 0x040043C2 RID: 17346
		public bool checkDiseaseContractChance;

		// Token: 0x040043C3 RID: 17347
		public bool addToHyperlinks;
	}
}
