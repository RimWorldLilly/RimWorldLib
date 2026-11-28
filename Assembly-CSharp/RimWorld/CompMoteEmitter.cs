using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200190F RID: 6415
	public class CompMoteEmitter : ThingComp
	{
		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x0600A0CE RID: 41166 RVA: 0x00397EFC File Offset: 0x003960FC
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x0600A0CF RID: 41167 RVA: 0x00397F09 File Offset: 0x00396109
		public bool MoteLive
		{
			get
			{
				return this.mote != null && !this.mote.Destroyed;
			}
		}

		// Token: 0x0600A0D0 RID: 41168 RVA: 0x00397F23 File Offset: 0x00396123
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.Props.ticksSinceLastEmittedMaxOffset > 0)
			{
				this.ticksSinceLastEmitted = Rand.Range(0, this.Props.ticksSinceLastEmittedMaxOffset);
			}
		}

		// Token: 0x0600A0D1 RID: 41169 RVA: 0x00397F54 File Offset: 0x00396154
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			Skyfaller skyfaller = this.parent as Skyfaller;
			if (skyfaller != null && skyfaller.FadingOut)
			{
				return;
			}
			if (this.Props.emissionInterval != -1 && !this.Props.maintain)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
			}
			else if (this.mote == null || this.mote.Destroyed)
			{
				this.Emit();
			}
			if (this.mote != null && !this.mote.Destroyed)
			{
				if (typeof(MoteAttached).IsAssignableFrom(this.Props.mote.thingClass) && skyfaller != null)
				{
					this.mote.exactRotation = skyfaller.DrawAngle();
				}
				if (this.Props.maintain)
				{
					this.Maintain();
				}
			}
		}

		// Token: 0x0600A0D2 RID: 41170 RVA: 0x00398095 File Offset: 0x00396295
		public void Maintain()
		{
			this.mote.Maintain();
		}

		// Token: 0x0600A0D3 RID: 41171 RVA: 0x003980A4 File Offset: 0x003962A4
		public virtual void Emit()
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Thing tried spawning mote without being spawned!");
				return;
			}
			Vector3 vector = this.Props.offset + this.Props.RotationOffset(this.parent.Rotation);
			if (this.Props.offsetMin != Vector3.zero || this.Props.offsetMax != Vector3.zero)
			{
				vector = this.Props.EmissionOffset;
			}
			ThingDef thingDef = this.Props.RotationMote(this.parent.Rotation) ?? this.Props.mote;
			if (typeof(MoteAttached).IsAssignableFrom(thingDef.thingClass))
			{
				this.mote = MoteMaker.MakeAttachedOverlay(this.parent, thingDef, vector, 1f, -1f);
			}
			else
			{
				Vector3 vector2 = this.parent.DrawPos + vector;
				if (vector2.InBounds(this.parent.Map))
				{
					this.mote = MoteMaker.MakeStaticMote(vector2, this.parent.Map, thingDef, 1f, false, 0f);
				}
			}
			if (this.mote != null && this.Props.useParentRotation)
			{
				this.mote.exactRotation = this.parent.Rotation.AsAngle;
			}
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x0600A0D4 RID: 41172 RVA: 0x00398234 File Offset: 0x00396434
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x04005EF4 RID: 24308
		public int ticksSinceLastEmitted;

		// Token: 0x04005EF5 RID: 24309
		protected Mote mote;
	}
}
