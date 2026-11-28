using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000590 RID: 1424
	public class MoteThrown : Mote
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002A04 RID: 10756 RVA: 0x00100B50 File Offset: 0x000FED50
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x00100B5F File Offset: 0x000FED5F
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x00100B78 File Offset: 0x000FED78
		// (set) Token: 0x06002A07 RID: 10759 RVA: 0x00100B80 File Offset: 0x000FED80
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

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002A08 RID: 10760 RVA: 0x00100B89 File Offset: 0x000FED89
		// (set) Token: 0x06002A09 RID: 10761 RVA: 0x00100B96 File Offset: 0x000FED96
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

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x00100BA5 File Offset: 0x000FEDA5
		// (set) Token: 0x06002A0B RID: 10763 RVA: 0x00100BB4 File Offset: 0x000FEDB4
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

		// Token: 0x06002A0C RID: 10764 RVA: 0x00100C18 File Offset: 0x000FEE18
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			if (!this.Flying && !this.Skidding)
			{
				return;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != base.Position)
			{
				if (!intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (this.def.mote.collide && intVec.Filled(base.Map))
				{
					this.WallHit();
					return;
				}
			}
			base.Position = intVec;
			this.exactPosition = vector;
			if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.exactRotation += this.rotationRate * deltaTime;
			}
			this.velocity += this.def.mote.acceleration * deltaTime;
			if (this.def.mote.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
				{
					this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x00100E2F File Offset: 0x000FF02F
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x00100E48 File Offset: 0x000FF048
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x00100E6B File Offset: 0x000FF06B
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x04001D1A RID: 7450
		public float airTimeLeft = 999999f;

		// Token: 0x04001D1B RID: 7451
		protected Vector3 velocity = Vector3.zero;
	}
}
