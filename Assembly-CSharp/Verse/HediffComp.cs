using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public class HediffComp
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000BC0C4 File Offset: 0x000BA2C4
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000BC0D1 File Offset: 0x000BA2D1
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000BC0DE File Offset: 0x000BA2DE
		public virtual string CompLabelPrefix
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000BC0E1 File Offset: 0x000BA2E1
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000BC0E4 File Offset: 0x000BA2E4
		public virtual string CompDescriptionExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000BC0E7 File Offset: 0x000BA2E7
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000BC0EA File Offset: 0x000BA2EA
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000BC0F1 File Offset: 0x000BA2F1
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000BC0F4 File Offset: 0x000BA2F4
		public virtual void CompPostMake()
		{
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000BC0F6 File Offset: 0x000BA2F6
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000BC0F8 File Offset: 0x000BA2F8
		public virtual void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000BC0FA File Offset: 0x000BA2FA
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000BC0FC File Offset: 0x000BA2FC
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x000BC0FE File Offset: 0x000BA2FE
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x000BC100 File Offset: 0x000BA300
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000BC102 File Offset: 0x000BA302
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000BC105 File Offset: 0x000BA305
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x000BC107 File Offset: 0x000BA307
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000BC109 File Offset: 0x000BA309
		public virtual void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x000BC10B File Offset: 0x000BA30B
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000BC10D File Offset: 0x000BA30D
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000BC10F File Offset: 0x000BA30F
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000BC111 File Offset: 0x000BA311
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000BC113 File Offset: 0x000BA313
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000BC115 File Offset: 0x000BA315
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000BC117 File Offset: 0x000BA317
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000BC119 File Offset: 0x000BA319
		public virtual void Notify_Spawned()
		{
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000BC11B File Offset: 0x000BA31B
		public virtual void Notify_SurgicallyRemoved(Pawn surgeon)
		{
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000BC11D File Offset: 0x000BA31D
		public virtual void Notify_SurgicallyReplaced(Pawn surgeon)
		{
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000BC11F File Offset: 0x000BA31F
		public virtual void CopyFrom(HediffComp other)
		{
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000BC121 File Offset: 0x000BA321
		public virtual IEnumerable<Gizmo> CompGetGizmos()
		{
			return null;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000BC124 File Offset: 0x000BA324
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x04001810 RID: 6160
		public HediffWithComps parent;

		// Token: 0x04001811 RID: 6161
		public HediffCompProperties props;
	}
}
