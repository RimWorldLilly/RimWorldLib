using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001799 RID: 6041
	public abstract class ScenPart_ConfigPage_ConfigureStartingPawnsBase : ScenPart_ConfigPage
	{
		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x0600969B RID: 38555
		protected abstract int TotalPawnCount { get; }

		// Token: 0x0600969C RID: 38556
		protected abstract void GenerateStartingPawns();

		// Token: 0x0600969D RID: 38557 RVA: 0x0036A0B4 File Offset: 0x003682B4
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnCount = this.TotalPawnCount;
			if (ModsConfig.BiotechActive)
			{
				Current.Game.customXenotypeDatabase.customXenotypes.Clear();
				foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						Precept_Xenotype precept_Xenotype = precept as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.customXenotype != null && !Current.Game.customXenotypeDatabase.customXenotypes.Contains(precept_Xenotype.customXenotype))
						{
							Current.Game.customXenotypeDatabase.customXenotypes.Add(precept_Xenotype.customXenotype);
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				bool flag;
				if (ofPlayerSilentFail == null)
				{
					flag = (null != null);
				}
				else
				{
					FactionIdeosTracker ideos = ofPlayerSilentFail.ideos;
					flag = (((ideos != null) ? ideos.PrimaryIdeo : null) != null);
				}
				if (flag)
				{
					foreach (Precept precept2 in Faction.OfPlayerSilentFail.ideos.PrimaryIdeo.PreceptsListForReading)
					{
						if (precept2.def.defaultDrugPolicyOverride != null)
						{
							Current.Game.drugPolicyDatabase.MakePolicyDefault(precept2.def.defaultDrugPolicyOverride);
						}
					}
				}
			}
			this.GenerateStartingPawns();
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				StartingPawnUtility.AddNewPawn(-1);
			}
		}

		// Token: 0x0600969E RID: 38558 RVA: 0x0036A274 File Offset: 0x00368474
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x0600969F RID: 38559 RVA: 0x0036A28E File Offset: 0x0036848E
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.pawnChoiceCount;
		}

		// Token: 0x04005A50 RID: 23120
		public int pawnChoiceCount = 10;

		// Token: 0x04005A51 RID: 23121
		protected const int MaxPawnCount = 10;
	}
}
