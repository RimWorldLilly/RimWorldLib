using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF3 RID: 7411
	public abstract class Transferable : IExposable
	{
		// Token: 0x170022B2 RID: 8882
		// (get) Token: 0x0600BEF2 RID: 48882
		public abstract Thing AnyThing { get; }

		// Token: 0x170022B3 RID: 8883
		// (get) Token: 0x0600BEF3 RID: 48883
		public abstract ThingDef ThingDef { get; }

		// Token: 0x170022B4 RID: 8884
		// (get) Token: 0x0600BEF4 RID: 48884
		public abstract bool Interactive { get; }

		// Token: 0x170022B5 RID: 8885
		// (get) Token: 0x0600BEF5 RID: 48885
		public abstract bool HasAnyThing { get; }

		// Token: 0x170022B6 RID: 8886
		// (get) Token: 0x0600BEF6 RID: 48886 RVA: 0x00452E1F File Offset: 0x0045101F
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170022B7 RID: 8887
		// (get) Token: 0x0600BEF7 RID: 48887
		public abstract string Label { get; }

		// Token: 0x170022B8 RID: 8888
		// (get) Token: 0x0600BEF8 RID: 48888 RVA: 0x00452E22 File Offset: 0x00451022
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x170022B9 RID: 8889
		// (get) Token: 0x0600BEF9 RID: 48889
		public abstract string TipDescription { get; }

		// Token: 0x170022BA RID: 8890
		// (get) Token: 0x0600BEFA RID: 48890
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x170022BB RID: 8891
		// (get) Token: 0x0600BEFB RID: 48891
		// (set) Token: 0x0600BEFC RID: 48892
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x170022BC RID: 8892
		// (get) Token: 0x0600BEFD RID: 48893 RVA: 0x00452E35 File Offset: 0x00451035
		public int CountToTransferToSource
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return -this.CountToTransfer;
				}
				return this.CountToTransfer;
			}
		}

		// Token: 0x170022BD RID: 8893
		// (get) Token: 0x0600BEFE RID: 48894 RVA: 0x00452E4D File Offset: 0x0045104D
		public int CountToTransferToDestination
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return this.CountToTransfer;
				}
				return -this.CountToTransfer;
			}
		}

		// Token: 0x170022BE RID: 8894
		// (get) Token: 0x0600BEFF RID: 48895 RVA: 0x00452E65 File Offset: 0x00451065
		// (set) Token: 0x0600BF00 RID: 48896 RVA: 0x00452E6D File Offset: 0x0045106D
		public string EditBuffer
		{
			get
			{
				return this.editBuffer;
			}
			set
			{
				this.editBuffer = value;
			}
		}

		// Token: 0x0600BF01 RID: 48897
		public abstract int GetMinimumToTransfer();

		// Token: 0x0600BF02 RID: 48898
		public abstract int GetMaximumToTransfer();

		// Token: 0x0600BF03 RID: 48899 RVA: 0x00452E76 File Offset: 0x00451076
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x0600BF04 RID: 48900 RVA: 0x00452E85 File Offset: 0x00451085
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x0600BF05 RID: 48901 RVA: 0x00452E99 File Offset: 0x00451099
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x0600BF06 RID: 48902 RVA: 0x00452EA9 File Offset: 0x004510A9
		public AcceptanceReport CanAdjustTo(int destination)
		{
			if (destination == this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (this.ClampAmount(destination) != this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (destination < this.CountToTransfer)
			{
				return this.UnderflowReport();
			}
			return this.OverflowReport();
		}

		// Token: 0x0600BF07 RID: 48903 RVA: 0x00452EE5 File Offset: 0x004510E5
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x0600BF08 RID: 48904 RVA: 0x00452EF8 File Offset: 0x004510F8
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts");
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		// Token: 0x0600BF09 RID: 48905 RVA: 0x00452F2E File Offset: 0x0045112E
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x0600BF0A RID: 48906 RVA: 0x00452F37 File Offset: 0x00451137
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		// Token: 0x0600BF0B RID: 48907 RVA: 0x00452F51 File Offset: 0x00451151
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		// Token: 0x0600BF0C RID: 48908 RVA: 0x00452F6B File Offset: 0x0045116B
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		// Token: 0x0600BF0D RID: 48909 RVA: 0x00452F6D File Offset: 0x0045116D
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x0600BF0E RID: 48910 RVA: 0x00452F75 File Offset: 0x00451175
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0600BF0F RID: 48911 RVA: 0x00452F7D File Offset: 0x0045117D
		public virtual void ExposeData()
		{
		}

		// Token: 0x04006DAB RID: 28075
		private string editBuffer = "0";
	}
}
