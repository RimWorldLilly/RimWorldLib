using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CC RID: 1228
	public class ImmunityHandler : IExposable
	{
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x000CB255 File Offset: 0x000C9455
		public List<ImmunityRecord> ImmunityListForReading
		{
			get
			{
				return this.immunityList;
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000CB25D File Offset: 0x000C945D
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000CB277 File Offset: 0x000C9477
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000CB290 File Offset: 0x000C9490
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000CB2AC File Offset: 0x000C94AC
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.RaceProps.isImmuneToInfections)
			{
				return 0f;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.preventIllnesses && this.pawn.mutant.Def.isImmuneToInfections)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(diseaseDef, out hediff))
			{
				immunityCause = hediff.def;
				return 0f;
			}
			if (this.AnyGeneMakesFullyImmuneTo(diseaseDef))
			{
				return 0f;
			}
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
				{
					return 0f;
				}
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				if (this.immunityList[j].hediffDef == diseaseDef)
				{
					immunityCause = this.immunityList[j].source;
					return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
				}
			}
			return 1f;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000CB404 File Offset: 0x000C9604
		public float GetImmunity(HediffDef def, bool naturalImmunityOnly = false)
		{
			float num = 0f;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					num = immunityRecord.immunity;
					break;
				}
			}
			Hediff hediff;
			if (!naturalImmunityOnly && (this.AnyHediffMakesFullyImmuneTo(def, out hediff) || this.AnyGeneMakesFullyImmuneTo(def)) && num < 0.65000004f)
			{
				num = 0.65000004f;
			}
			return num;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000CB474 File Offset: 0x000C9674
		internal void ImmunityHandlerTickInterval(int delta)
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTickInterval(this.pawn, firstHediffOfDef != null, firstHediffOfDef, delta);
			}
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000CB590 File Offset: 0x000C9790
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000CB618 File Offset: 0x000C9818
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def, out Hediff sourceHediff)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							sourceHediff = hediffs[i];
							return true;
						}
					}
				}
			}
			sourceHediff = null;
			return false;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000CB698 File Offset: 0x000C9898
		public bool AnyGeneMakesFullyImmuneTo(HediffDef def)
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return false;
			}
			for (int i = 0; i < this.pawn.genes.GenesListForReading.Count; i++)
			{
				Gene gene = this.pawn.genes.GenesListForReading[i];
				if (gene.def.makeImmuneTo != null)
				{
					for (int j = 0; j < gene.def.makeImmuneTo.Count; j++)
					{
						if (gene.def.makeImmuneTo[j] == def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000CB734 File Offset: 0x000C9934
		public void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() == null)
			{
				return;
			}
			if (this.ImmunityRecordExists(def))
			{
				return;
			}
			ImmunityRecord immunityRecord = new ImmunityRecord();
			immunityRecord.hediffDef = def;
			immunityRecord.source = source;
			this.immunityList.Add(immunityRecord);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000CB774 File Offset: 0x000C9974
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			ImmunityRecord immunityRecord = null;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					immunityRecord = this.immunityList[i];
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.65000004f))
			{
				immunityRecord = new ImmunityRecord
				{
					immunity = 0.65000004f,
					hediffDef = def,
					source = hediff.def
				};
			}
			return immunityRecord;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000CB7FC File Offset: 0x000C99FC
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x040019BD RID: 6589
		public Pawn pawn;

		// Token: 0x040019BE RID: 6590
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x040019BF RID: 6591
		private const float ForcedImmunityLevel = 0.65000004f;

		// Token: 0x040019C0 RID: 6592
		private static readonly List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02002700 RID: 9984
		public struct ImmunityInfo
		{
			// Token: 0x04009FD5 RID: 40917
			public HediffDef immunity;

			// Token: 0x04009FD6 RID: 40918
			public HediffDef source;
		}
	}
}
