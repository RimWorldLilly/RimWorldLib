using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001828 RID: 6184
	[StaticConstructorOnStartup]
	public class CompBandNode : ThingComp
	{
		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x06009AA4 RID: 39588 RVA: 0x0037C64F File Offset: 0x0037A84F
		private CompPowerTrader PowerTrader
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x06009AA5 RID: 39589 RVA: 0x0037C65C File Offset: 0x0037A85C
		public CompProperties_BandNode Props
		{
			get
			{
				return (CompProperties_BandNode)this.props;
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x06009AA6 RID: 39590 RVA: 0x0037C669 File Offset: 0x0037A869
		private int RetuneTimeTicks
		{
			get
			{
				return (int)(60000f * this.Props.retuneDays);
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x06009AA7 RID: 39591 RVA: 0x0037C67D File Offset: 0x0037A87D
		private int TuningTimeTicks
		{
			get
			{
				return (int)(this.Props.tuneSeconds * 60f);
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x06009AA8 RID: 39592 RVA: 0x0037C691 File Offset: 0x0037A891
		private BandNodeState State
		{
			get
			{
				if (this.tunedTo != null && this.tuningTo != null)
				{
					return BandNodeState.Retuning;
				}
				if (this.tuningTo != null)
				{
					return BandNodeState.Tuning;
				}
				if (this.tunedTo != null)
				{
					return BandNodeState.Tuned;
				}
				return BandNodeState.Untuned;
			}
		}

		// Token: 0x06009AA9 RID: 39593 RVA: 0x0037C6BA File Offset: 0x0037A8BA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Band node"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06009AAA RID: 39594 RVA: 0x0037C6DC File Offset: 0x0037A8DC
		public override void PostExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.tunedTo, "tunedTo", false);
			Scribe_References.Look<Pawn>(ref this.tuningTo, "tuningTo", false);
			Scribe_Values.Look<int>(ref this.tuningTimeLeft, "tuningTimeLeft", 0, false);
		}

		// Token: 0x06009AAB RID: 39595 RVA: 0x0037C712 File Offset: 0x0037A912
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompBandNode.<CompGetGizmosExtra>d__21 <CompGetGizmosExtra>d__ = new CompBandNode.<CompGetGizmosExtra>d__21(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009AAC RID: 39596 RVA: 0x0037C722 File Offset: 0x0037A922
		public void TuneTo(Pawn pawn)
		{
			this.tuningTimeLeft = ((this.tunedTo == null) ? this.TuningTimeTicks : this.RetuneTimeTicks);
			this.tuningTo = pawn;
		}

		// Token: 0x06009AAD RID: 39597 RVA: 0x0037C748 File Offset: 0x0037A948
		public override void PostDraw()
		{
			base.PostDraw();
			BandNodeState state = this.State;
			Material material;
			if (state != BandNodeState.Untuned)
			{
				if (state == BandNodeState.Tuned)
				{
					material = CompBandNode.TunedMaterial;
				}
				else
				{
					material = CompBandNode.TuningMaterial;
				}
			}
			else
			{
				material = CompBandNode.UntunedMaterial;
			}
			Vector3 s = new Vector3(CompBandNode.TuningBarSize.x, 1f, CompBandNode.TuningBarSize.y);
			Vector3 pos = this.parent.DrawPos + new Vector3(0f, 0f, -0.4f);
			pos.y = this.parent.def.altitudeLayer.AltitudeFor() + 0.03658537f;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
		}

		// Token: 0x06009AAE RID: 39598 RVA: 0x0037C81C File Offset: 0x0037AA1C
		public override void CompTick()
		{
			this.PowerTrader.PowerOutput = ((this.tunedTo == null && this.tuningTo == null) ? ((float)(-(float)this.Props.powerConsumptionIdle)) : (-this.PowerTrader.Props.PowerConsumption));
			if (this.tunedTo != null && this.tunedTo.Dead)
			{
				this.tunedTo = null;
			}
			if (this.tuningTo != null && this.tuningTo.Dead)
			{
				this.tuningTo = null;
			}
			if (this.PowerTrader != null && !this.PowerTrader.PowerOn)
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			if (this.tuningTo != null)
			{
				this.tuningTimeLeft--;
				if (this.tuningTimeLeft <= 0)
				{
					this.tunedTo = this.tuningTo;
					this.tuningTo = null;
					if (this.Props.tuningCompleteSound != null)
					{
						this.Props.tuningCompleteSound.PlayOneShot(this.parent);
					}
				}
			}
			if (this.tuningTo == null && this.tunedTo != null && !this.tunedTo.health.hediffSet.HasHediff(this.Props.hediff, false))
			{
				this.tunedTo.health.AddHediff(this.Props.hediff, this.tunedTo.health.hediffSet.GetBrain(), null, null);
			}
			if (this.State == BandNodeState.Untuned)
			{
				if (this.effecter == null || this.effecter.def != this.Props.untunedEffect)
				{
					Effecter effecter2 = this.effecter;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecter = this.Props.untunedEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Tuned)
			{
				if (this.effecter == null || this.effecter.def != this.Props.tunedEffect)
				{
					Effecter effecter3 = this.effecter;
					if (effecter3 != null)
					{
						effecter3.Cleanup();
					}
					this.effecter = this.Props.tunedEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Tuning)
			{
				if (this.effecter == null || this.effecter.def != this.Props.tuningEffect)
				{
					Effecter effecter4 = this.effecter;
					if (effecter4 != null)
					{
						effecter4.Cleanup();
					}
					this.effecter = this.Props.tuningEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Retuning)
			{
				if (this.effecter == null || this.effecter.def != this.Props.retuningEffect)
				{
					Effecter effecter5 = this.effecter;
					if (effecter5 != null)
					{
						effecter5.Cleanup();
					}
					this.effecter = this.Props.retuningEffect.Spawn();
				}
			}
			else
			{
				Effecter effecter6 = this.effecter;
				if (effecter6 != null)
				{
					effecter6.Cleanup();
				}
				this.effecter = null;
			}
			if (this.effecter != null)
			{
				this.effecter.EffectTick(this.parent, this.parent);
			}
		}

		// Token: 0x06009AAF RID: 39599 RVA: 0x0037CB2C File Offset: 0x0037AD2C
		public override string CompInspectStringExtra()
		{
			string t = null;
			if (!this.PowerTrader.PowerOn)
			{
				t = "\n" + "Unpowered".Translate().CapitalizeFirst().Resolve();
			}
			if (this.tuningTo != null)
			{
				return "BandNodeTuningTo".Translate() + ": " + this.tuningTo.Name.ToStringFull + " - " + this.tuningTimeLeft.ToStringTicksToPeriod(true, false, true, true, false) + t;
			}
			return "BandNodeTunedTo".Translate() + ": " + ((this.tunedTo == null) ? "Nobody".Translate().Resolve() : this.tunedTo.Name.ToStringFull) + t;
		}

		// Token: 0x04005BFB RID: 23547
		private static readonly Material UntunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.red, false);

		// Token: 0x04005BFC RID: 23548
		private static readonly Material TuningMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.yellow, false);

		// Token: 0x04005BFD RID: 23549
		private static readonly Material TunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.green, false);

		// Token: 0x04005BFE RID: 23550
		private static readonly Vector2 TuningBarSize = new Vector3(0.255f, 0.035f);

		// Token: 0x04005BFF RID: 23551
		private const float TuningBarYOffset = -0.4f;

		// Token: 0x04005C00 RID: 23552
		public Pawn tunedTo;

		// Token: 0x04005C01 RID: 23553
		public int tuningTimeLeft;

		// Token: 0x04005C02 RID: 23554
		public Pawn tuningTo;

		// Token: 0x04005C03 RID: 23555
		private Effecter effecter;
	}
}
