using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002098 RID: 8344
	public class CompReleaseGas : ThingComp
	{
		// Token: 0x170025C4 RID: 9668
		// (get) Token: 0x0600D24E RID: 53838 RVA: 0x004D3FB7 File Offset: 0x004D21B7
		private CompProperties_ReleaseGas Props
		{
			get
			{
				return (CompProperties_ReleaseGas)this.props;
			}
		}

		// Token: 0x170025C5 RID: 9669
		// (get) Token: 0x0600D24F RID: 53839 RVA: 0x004D3FC4 File Offset: 0x004D21C4
		private int TotalGas
		{
			get
			{
				return Mathf.CeilToInt(this.Props.cellsToFill * 255f);
			}
		}

		// Token: 0x170025C6 RID: 9670
		// (get) Token: 0x0600D250 RID: 53840 RVA: 0x004D3FDC File Offset: 0x004D21DC
		private float GasReleasedPerTick
		{
			get
			{
				return (float)this.TotalGas / this.Props.durationSeconds / 60f;
			}
		}

		// Token: 0x170025C7 RID: 9671
		// (get) Token: 0x0600D251 RID: 53841 RVA: 0x004D3FF8 File Offset: 0x004D21F8
		private Thing EffecterSourceThing
		{
			get
			{
				ThingWithComps result = this.parent;
				if (!this.parent.Spawned)
				{
					IThingHolder parentHolder = this.parent.ParentHolder;
					if (parentHolder != null)
					{
						Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
						if (pawn_ApparelTracker != null)
						{
							result = pawn_ApparelTracker.pawn;
						}
						else
						{
							Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
							if (pawn_CarryTracker != null)
							{
								result = pawn_CarryTracker.pawn;
							}
							else
							{
								Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
								if (pawn_EquipmentTracker != null)
								{
									result = pawn_EquipmentTracker.pawn;
								}
							}
						}
					}
				}
				return result;
			}
		}

		// Token: 0x0600D252 RID: 53842 RVA: 0x004D4063 File Offset: 0x004D2263
		public override void PostPostMake()
		{
			this.remainingGas = this.TotalGas;
		}

		// Token: 0x0600D253 RID: 53843 RVA: 0x004D4071 File Offset: 0x004D2271
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x0600D254 RID: 53844 RVA: 0x004D408B File Offset: 0x004D228B
		public void StartRelease()
		{
			this.started = true;
		}

		// Token: 0x0600D255 RID: 53845 RVA: 0x004D4094 File Offset: 0x004D2294
		public override void Notify_WearerDied()
		{
			this.started = false;
			this.remainingGas = this.TotalGas;
		}

		// Token: 0x0600D256 RID: 53846 RVA: 0x004D40AC File Offset: 0x004D22AC
		public override void CompTick()
		{
			if (!this.started || this.parent.MapHeld == null)
			{
				return;
			}
			if (this.Props.effecterReleasing != null)
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.effecterReleasing.Spawn(this.EffecterSourceThing, TargetInfo.Invalid, 1f);
				}
				this.effecter.EffectTick(this.EffecterSourceThing, TargetInfo.Invalid);
			}
			if (this.remainingGas > 0 && this.parent.IsHashIntervalTick(30))
			{
				int num = Mathf.Min(this.remainingGas, Mathf.RoundToInt(this.GasReleasedPerTick * 30f));
				GasUtility.AddGas(this.parent.PositionHeld, this.parent.MapHeld, this.Props.gasType, num);
				this.remainingGas -= num;
				if (this.remainingGas <= 0)
				{
					this.started = false;
					this.remainingGas = this.TotalGas;
				}
			}
		}

		// Token: 0x0600D257 RID: 53847 RVA: 0x004D41B0 File Offset: 0x004D23B0
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.remainingGas, "remainingGas", 0, false);
			Scribe_Values.Look<bool>(ref this.started, "started", false, false);
		}

		// Token: 0x0400853C RID: 34108
		private int remainingGas;

		// Token: 0x0400853D RID: 34109
		private bool started;

		// Token: 0x0400853E RID: 34110
		[Unsaved(false)]
		private Effecter effecter;

		// Token: 0x0400853F RID: 34111
		private const int ReleaseGasInterval = 30;
	}
}
