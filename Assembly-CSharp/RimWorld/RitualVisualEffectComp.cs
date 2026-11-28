using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001732 RID: 5938
	public abstract class RitualVisualEffectComp : IExposable
	{
		// Token: 0x0600940D RID: 37901 RVA: 0x0035FBF4 File Offset: 0x0035DDF4
		public virtual bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return false;
		}

		// Token: 0x0600940E RID: 37902
		protected abstract Vector3 SpawnPos(LordJob_Ritual ritual);

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x0600940F RID: 37903 RVA: 0x0035FBF7 File Offset: 0x0035DDF7
		protected virtual ThingDef MoteDef
		{
			get
			{
				return this.props.moteDef;
			}
		}

		// Token: 0x06009410 RID: 37904 RVA: 0x0035FC04 File Offset: 0x0035DE04
		public virtual Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return null;
			}
			Mote mote = MoteMaker.MakeStaticMote(vector.Value, ritual.Map, this.MoteDef, 1f, false, 0f);
			if (mote == null)
			{
				return null;
			}
			mote.exactRotation = this.props.rotation.RandomInRange;
			mote.Scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			mote.rotationRate = this.props.rotationRate.RandomInRange;
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				mote.instanceColor = this.props.overrideColors.RandomElement<Color>();
			}
			if (mote.def.mote.needsMaintenance)
			{
				mote.Maintain();
			}
			return mote;
		}

		// Token: 0x06009411 RID: 37905 RVA: 0x0035FCF0 File Offset: 0x0035DEF0
		public virtual void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector.Value, ritual.Map, this.props.fleckDef, 1f);
			if (exactRotation != null)
			{
				dataStatic.rotation = exactRotation.Value;
			}
			else
			{
				dataStatic.rotation = this.props.rotation.RandomInRange;
			}
			dataStatic.scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			dataStatic.rotationRate = this.props.rotationRate.RandomInRange;
			dataStatic.velocity = new Vector3?(this.props.velocityDir * this.props.velocity.RandomInRange);
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				dataStatic.instanceColor = new Color?(this.props.overrideColors.RandomElement<Color>());
			}
			else if (this.props.colorOverride != null)
			{
				dataStatic.instanceColor = this.props.colorOverride;
			}
			ritual.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06009412 RID: 37906 RVA: 0x0035FE40 File Offset: 0x0035E040
		public Vector3? ScaledPos(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3 vector = ritual.Spot.ToVector3Shifted();
			Vector3 vector2 = (forcedPos ?? this.SpawnPos(ritual)) + this.props.offset;
			if (this.props.scalePositionWithRoom)
			{
				float d = this.ScaleForRoom(ritual);
				Vector3 b = (vector2 - vector) * d;
				vector2 = vector + b;
			}
			if (this.props.onlySpawnInSameRoom && (new IntVec3(vector2) + this.props.roomCheckOffset).GetRoom(ritual.Map) != ritual.GetRoom)
			{
				return null;
			}
			return new Vector3?(vector2);
		}

		// Token: 0x06009413 RID: 37907 RVA: 0x0035FF00 File Offset: 0x0035E100
		public virtual Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			TargetInfo a = target.IsValid ? target : ritual.selectedTarget;
			Effecter effecter = this.props.effecterDef.Spawn(a.Cell, this.parent.ritual.Map, this.props.offset, 1f);
			effecter.Trigger(a, TargetInfo.Invalid, -1);
			return effecter;
		}

		// Token: 0x06009414 RID: 37908 RVA: 0x0035FF65 File Offset: 0x0035E165
		public virtual void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
		}

		// Token: 0x06009415 RID: 37909 RVA: 0x0035FF70 File Offset: 0x0035E170
		public virtual void TickInterval(int delta)
		{
			if (!this.ShouldSpawnNow(this.parent.ritual))
			{
				return;
			}
			if (this.MoteDef != null)
			{
				Mote mote = this.SpawnMote(this.parent.ritual, null);
				if (mote != null && mote.def.mote.needsMaintenance)
				{
					this.parent.AddMoteToMaintain(mote);
				}
			}
			if (this.props.fleckDef != null)
			{
				this.SpawnFleck(this.parent.ritual, null, null);
			}
			if (this.props.effecterDef != null)
			{
				this.SpawnEffecter(this.parent.ritual, TargetInfo.Invalid, null);
			}
		}

		// Token: 0x06009416 RID: 37910 RVA: 0x00360034 File Offset: 0x0035E234
		public float ScaleForRoom(LordJob_Ritual ritual)
		{
			Room getRoom = ritual.GetRoom;
			if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom || ritual.RoomBoundsCached.IsInvalid)
			{
				return 1f;
			}
			float value = (float)(ritual.RoomBoundsCached.x + ritual.RoomBoundsCached.z) / 2f;
			return Mathf.Lerp(0.35f, 1f, Mathf.Clamp01(Mathf.InverseLerp((float)RitualVisualEffectComp.roomDimensionRange.min, (float)RitualVisualEffectComp.roomDimensionRange.max, value)));
		}

		// Token: 0x06009417 RID: 37911 RVA: 0x003600C0 File Offset: 0x0035E2C0
		public virtual void ExposeData()
		{
		}

		// Token: 0x04005973 RID: 22899
		protected RitualVisualEffect parent;

		// Token: 0x04005974 RID: 22900
		public CompProperties_RitualVisualEffect props;

		// Token: 0x04005975 RID: 22901
		public const float minScaleForRoom = 0.35f;

		// Token: 0x04005976 RID: 22902
		public static readonly IntRange roomDimensionRange = new IntRange(7, 18);
	}
}
