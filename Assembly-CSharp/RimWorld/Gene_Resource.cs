using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207D RID: 8317
	public abstract class Gene_Resource : Gene
	{
		// Token: 0x1700258C RID: 9612
		// (get) Token: 0x0600D1A4 RID: 53668 RVA: 0x004D12AC File Offset: 0x004CF4AC
		public virtual string ResourceLabel
		{
			get
			{
				return this.def.resourceLabel;
			}
		}

		// Token: 0x1700258D RID: 9613
		// (get) Token: 0x0600D1A5 RID: 53669
		public abstract float InitialResourceMax { get; }

		// Token: 0x1700258E RID: 9614
		// (get) Token: 0x0600D1A6 RID: 53670
		protected abstract Color BarColor { get; }

		// Token: 0x1700258F RID: 9615
		// (get) Token: 0x0600D1A7 RID: 53671
		protected abstract Color BarHighlightColor { get; }

		// Token: 0x17002590 RID: 9616
		// (get) Token: 0x0600D1A8 RID: 53672
		public abstract float MinLevelForAlert { get; }

		// Token: 0x17002591 RID: 9617
		// (get) Token: 0x0600D1A9 RID: 53673 RVA: 0x004D12B9 File Offset: 0x004CF4B9
		public virtual int ValueForDisplay
		{
			get
			{
				return this.PostProcessValue(this.cur);
			}
		}

		// Token: 0x17002592 RID: 9618
		// (get) Token: 0x0600D1AA RID: 53674 RVA: 0x004D12C7 File Offset: 0x004CF4C7
		public virtual int MaxForDisplay
		{
			get
			{
				return this.PostProcessValue(this.max);
			}
		}

		// Token: 0x17002593 RID: 9619
		// (get) Token: 0x0600D1AB RID: 53675 RVA: 0x004D12D5 File Offset: 0x004CF4D5
		public virtual float MaxLevelOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002594 RID: 9620
		// (get) Token: 0x0600D1AC RID: 53676 RVA: 0x004D12DC File Offset: 0x004CF4DC
		public virtual float Max
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x17002595 RID: 9621
		// (get) Token: 0x0600D1AD RID: 53677 RVA: 0x004D12E4 File Offset: 0x004CF4E4
		// (set) Token: 0x0600D1AE RID: 53678 RVA: 0x004D12EC File Offset: 0x004CF4EC
		public virtual float Value
		{
			get
			{
				return this.cur;
			}
			set
			{
				this.cur = Mathf.Clamp(value, 0f, this.max);
			}
		}

		// Token: 0x17002596 RID: 9622
		// (get) Token: 0x0600D1AF RID: 53679 RVA: 0x004D1305 File Offset: 0x004CF505
		// (set) Token: 0x0600D1B0 RID: 53680 RVA: 0x004D1314 File Offset: 0x004CF514
		public virtual float ValuePercent
		{
			get
			{
				return this.cur / this.max;
			}
			set
			{
				this.cur = this.max * value;
			}
		}

		// Token: 0x17002597 RID: 9623
		// (get) Token: 0x0600D1B1 RID: 53681 RVA: 0x004D1324 File Offset: 0x004CF524
		private List<IGeneResourceDrain> DrainGenes
		{
			get
			{
				this.tmpDrainGenes.Clear();
				List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					IGeneResourceDrain geneResourceDrain = genesListForReading[i] as IGeneResourceDrain;
					if (geneResourceDrain != null && geneResourceDrain.Resource == this)
					{
						this.tmpDrainGenes.Add(geneResourceDrain);
					}
				}
				return this.tmpDrainGenes;
			}
		}

		// Token: 0x0600D1B2 RID: 53682 RVA: 0x004D1389 File Offset: 0x004CF589
		public override void PostAdd()
		{
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x0600D1B3 RID: 53683 RVA: 0x004D1397 File Offset: 0x004CF597
		public void SetMax(float newMax)
		{
			this.max = newMax;
			this.cur = Mathf.Clamp(this.cur, 0f, this.max);
			this.SetTargetValuePct(this.targetValue);
		}

		// Token: 0x0600D1B4 RID: 53684 RVA: 0x004D13C8 File Offset: 0x004CF5C8
		public void ResetMax()
		{
			this.max = this.InitialResourceMax;
			this.cur = Mathf.Clamp(this.cur, 0f, this.max);
			this.SetTargetValuePct(this.targetValue);
		}

		// Token: 0x0600D1B5 RID: 53685 RVA: 0x004D1400 File Offset: 0x004CF600
		public override void Reset()
		{
			this.cur = (this.max = this.InitialResourceMax);
			this.targetValue = 0.5f;
		}

		// Token: 0x0600D1B6 RID: 53686 RVA: 0x004D142D File Offset: 0x004CF62D
		public virtual void SetTargetValuePct(float val)
		{
			this.targetValue = val * this.Max;
		}

		// Token: 0x0600D1B7 RID: 53687 RVA: 0x004D143D File Offset: 0x004CF63D
		public virtual int PostProcessValue(float value)
		{
			return Mathf.RoundToInt(value * 100f);
		}

		// Token: 0x0600D1B8 RID: 53688 RVA: 0x004D144B File Offset: 0x004CF64B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_Resource.<GetGizmos>d__37 <GetGizmos>d__ = new Gene_Resource.<GetGizmos>d__37(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600D1B9 RID: 53689 RVA: 0x004D145C File Offset: 0x004CF65C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cur, "cur", 0f, false);
			Scribe_Values.Look<float>(ref this.max, "max", 0f, false);
			Scribe_Values.Look<float>(ref this.targetValue, "targetValue", 0.5f, false);
		}

		// Token: 0x04008502 RID: 34050
		public float targetValue = 0.5f;

		// Token: 0x04008503 RID: 34051
		protected float cur;

		// Token: 0x04008504 RID: 34052
		protected float max;

		// Token: 0x04008505 RID: 34053
		[Unsaved(false)]
		protected GeneGizmo_Resource gizmo;

		// Token: 0x04008506 RID: 34054
		[Unsaved(false)]
		private List<IGeneResourceDrain> tmpDrainGenes = new List<IGeneResourceDrain>();
	}
}
