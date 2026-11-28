using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000267 RID: 615
	public struct FleckSplash : IFleck
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x0006687B File Offset: 0x00064A7B
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00066898 File Offset: 0x00064A98
		public float Alpha
		{
			get
			{
				Mathf.Clamp01(this.ageSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - this.ageSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000668E8 File Offset: 0x00064AE8
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			if (this.def.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
			float d = this.ageSecs * this.velocity;
			this.exactScale = Vector3.one * d;
			this.position += map.waterInfo.GetWaterMovement(this.position) * deltaTime;
			return false;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000669F4 File Offset: 0x00064BF4
		public void Draw(DrawBatch batch)
		{
			this.position.y = this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset);
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = Color.white,
				drawLayer = 0,
				pos = this.position,
				rotation = 0f,
				scale = this.exactScale,
				ageSecs = this.ageSecs,
				calculatedShockwaveSpan = this.CalculatedShockwaveSpan(),
				id = (float)num
			}, batch);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00066AD0 File Offset: 0x00064CD0
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.position = creationData.spawnPosition;
			this.spawnPosition = creationData.spawnPosition;
			this.velocity = creationData.velocitySpeed;
			this.targetSize = creationData.targetSize;
			this.setupTick = Find.TickManager.TicksGame;
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00066B3E File Offset: 0x00064D3E
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00066B51 File Offset: 0x00064D51
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00066B80 File Offset: 0x00064D80
		public void ForceSpawnTick(int tick)
		{
			this.setupTick = tick;
			this.ageSecs = (Find.TickManager.TicksGame - tick).TicksToSeconds();
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00066BA0 File Offset: 0x00064DA0
		public Vector3 GetPosition()
		{
			return this.position;
		}

		// Token: 0x0400103C RID: 4156
		public const float VelocityFootstep = 1.5f;

		// Token: 0x0400103D RID: 4157
		public const float SizeFootstep = 2f;

		// Token: 0x0400103E RID: 4158
		public const float VelocityGunfire = 4f;

		// Token: 0x0400103F RID: 4159
		public const float SizeGunfire = 1f;

		// Token: 0x04001040 RID: 4160
		public const float VelocityExplosion = 20f;

		// Token: 0x04001041 RID: 4161
		public const float SizeExplosion = 6f;

		// Token: 0x04001042 RID: 4162
		public FleckDef def;

		// Token: 0x04001043 RID: 4163
		private float ageSecs;

		// Token: 0x04001044 RID: 4164
		private int setupTick;

		// Token: 0x04001045 RID: 4165
		private float targetSize;

		// Token: 0x04001046 RID: 4166
		private float velocity;

		// Token: 0x04001047 RID: 4167
		private Vector3 position;

		// Token: 0x04001048 RID: 4168
		private Vector3 spawnPosition;

		// Token: 0x04001049 RID: 4169
		private Vector3 exactScale;
	}
}
