using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200026C RID: 620
	public struct FleckThrown : IFleck
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00067151 File Offset: 0x00065351
		public bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x00067160 File Offset: 0x00065360
		public bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x00067179 File Offset: 0x00065379
		public bool Orbiting
		{
			get
			{
				return this.orbitSpeed != 0f;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0006718B File Offset: 0x0006538B
		public bool Arching
		{
			get
			{
				return this.archHeight != 0f;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x0006719D File Offset: 0x0006539D
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x000671A5 File Offset: 0x000653A5
		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x000671AE File Offset: 0x000653AE
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x000671BB File Offset: 0x000653BB
		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000671CC File Offset: 0x000653CC
		private void ApplyOrbit(float deltaTime, ref Vector3 nextPosition)
		{
			this.orbitAccum += deltaTime * this.orbitSpeed;
			Vector3 b = new Vector3(Mathf.Cos(this.orbitAccum), 0f, Mathf.Sin(this.orbitAccum)) * this.orbitDistance;
			Vector3 vector = this.link.Target.CenterVector3 + b;
			this.MoveAngle = (vector - this.baseData.position.worldPosition).AngleFlat();
			nextPosition = Vector3.Lerp(nextPosition, vector, this.orbitSnapStrength);
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0006726D File Offset: 0x0006546D
		// (set) Token: 0x0600120D RID: 4621 RVA: 0x0006727C File Offset: 0x0006547C
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
				{
					this.velocity = Vector3.zero;
					return;
				}
				if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
					return;
				}
				this.velocity = this.velocity.normalized * value;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x000672E0 File Offset: 0x000654E0
		public void Setup(FleckCreationData creationData)
		{
			this.baseData = default(FleckStatic);
			this.baseData.Setup(creationData);
			this.airTimeLeft = creationData.airTimeLeft.GetValueOrDefault(999999f);
			this.attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
			this.link = creationData.link;
			if (this.link.Linked)
			{
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			this.rotationRate = creationData.rotationRate;
			this.SetVelocity(creationData.velocityAngle, creationData.velocitySpeed);
			if (creationData.velocity != null)
			{
				this.velocity += creationData.velocity.Value;
			}
			this.orbitSpeed = creationData.orbitSpeed;
			this.orbitSnapStrength = creationData.orbitSnapStrength;
			if (this.Orbiting)
			{
				this.orbitDistance = (this.link.Target.CenterVector3 - this.baseData.position.worldPosition).MagnitudeHorizontal();
				this.orbitAccum = Rand.Range(0f, 6.2831855f);
				if (Mathf.Abs(this.orbitSpeed) < 0.2f)
				{
					this.orbitSpeed = 0.2f * GenMath.Sign(this.orbitSpeed);
				}
			}
			this.archHeight = creationData.def.archHeight.RandomInRange;
			this.archDuration = new float?(creationData.def.archDuration.RandomInRange);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0006746C File Offset: 0x0006566C
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.baseData.TimeInterval(deltaTime, map))
			{
				return true;
			}
			if (!this.Flying && !this.Skidding)
			{
				return false;
			}
			if (this.baseData.def.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.baseData.exactRotation = this.velocity.AngleFlat() + this.baseData.def.rotateTowardsMoveDirectionExtraAngle;
			}
			else
			{
				this.baseData.exactRotation = this.baseData.exactRotation + this.rotationRate * deltaTime;
			}
			this.velocity += this.baseData.def.acceleration * deltaTime;
			if (this.baseData.def.speedPerTime != FloatRange.Zero)
			{
				this.Speed = Mathf.Max(this.Speed + this.baseData.def.speedPerTime.RandomInRange * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.baseData.def.landSound.NullOrUndefined())
				{
					this.baseData.def.landSound.PlayOneShot(new TargetInfo(new IntVec3(this.baseData.position.worldPosition), map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.baseData.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.baseData.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
			FleckDrawPosition fleckDrawPosition = this.NextPosition(deltaTime);
			if (this.Orbiting)
			{
				this.ApplyOrbit(deltaTime, ref fleckDrawPosition.worldPosition);
			}
			IntVec3 intVec = new IntVec3(fleckDrawPosition.worldPosition);
			if (intVec != new IntVec3(this.baseData.position.worldPosition))
			{
				if (!intVec.InBounds(map))
				{
					return true;
				}
				if (this.baseData.def.collide && intVec.Filled(map))
				{
					this.WallHit();
					return false;
				}
			}
			this.baseData.position = fleckDrawPosition;
			return false;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x000676D8 File Offset: 0x000658D8
		private FleckDrawPosition NextPosition(float deltaTime)
		{
			Vector3 vector = this.baseData.position.worldPosition + this.velocity * deltaTime;
			float height = 0f;
			Vector3 attachedOffset = Vector3.zero;
			if (this.Arching)
			{
				float x = Mathf.Clamp01(this.baseData.ageSecs / this.archDuration.Value);
				if (this.baseData.def.archCurve == null)
				{
					height = this.archHeight * GenMath.InverseParabola(x);
				}
				else
				{
					height = this.archHeight * this.baseData.def.archCurve.Evaluate(x);
				}
			}
			if (this.link.Target.HasThing)
			{
				bool flag = this.link.detachAfterTicks == -1 || this.baseData.ageTicks < this.link.detachAfterTicks;
				if (!this.link.Target.ThingDestroyed && flag)
				{
					this.link.UpdateDrawPos();
				}
				Vector3 attachedDrawOffset = this.baseData.def.attachedDrawOffset;
				if (this.baseData.def.attachedToHead)
				{
					Pawn pawn = this.link.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null && pawn.SpawnedOrAnyParentSpawned)
					{
						attachedOffset = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
					}
				}
				Vector3 b = this.link.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector += attachedDrawOffset;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			Vector3 anchorOffset = (new Vector3(0.5f, 0f, 0.5f) - this.baseData.def.scalingAnchor).ScaledBy(this.baseData.AddedScale);
			return new FleckDrawPosition(vector, height, anchorOffset, this.baseData.def.unattachedDrawOffset, attachedOffset);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00067921 File Offset: 0x00065B21
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00067944 File Offset: 0x00065B44
		public void Draw(DrawBatch batch)
		{
			this.baseData.Draw(batch);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00067952 File Offset: 0x00065B52
		private void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00067975 File Offset: 0x00065B75
		public Vector3 GetPosition()
		{
			return this.baseData.position.worldPosition;
		}

		// Token: 0x0400105D RID: 4189
		public FleckStatic baseData;

		// Token: 0x0400105E RID: 4190
		public float airTimeLeft;

		// Token: 0x0400105F RID: 4191
		public Vector3 velocity;

		// Token: 0x04001060 RID: 4192
		public float rotationRate;

		// Token: 0x04001061 RID: 4193
		public FleckAttachLink link;

		// Token: 0x04001062 RID: 4194
		private Vector3 attacheeLastPosition;

		// Token: 0x04001063 RID: 4195
		public float orbitSpeed;

		// Token: 0x04001064 RID: 4196
		public float archHeight;

		// Token: 0x04001065 RID: 4197
		public float? archDuration;

		// Token: 0x04001066 RID: 4198
		public float orbitSnapStrength;

		// Token: 0x04001067 RID: 4199
		public const float MinSpeed = 0.02f;

		// Token: 0x04001068 RID: 4200
		public const float MinOrbitSpeed = 0.2f;

		// Token: 0x04001069 RID: 4201
		public float orbitDistance;

		// Token: 0x0400106A RID: 4202
		private float orbitAccum;
	}
}
