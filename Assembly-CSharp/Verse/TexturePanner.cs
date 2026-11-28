using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000323 RID: 803
	public class TexturePanner
	{
		// Token: 0x060018A8 RID: 6312 RVA: 0x000943C0 File Offset: 0x000925C0
		public TexturePanner(Material material, Vector2 direction, float speed) : this(material, Shader.PropertyToID("_MainTex"), direction, speed)
		{
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000943D5 File Offset: 0x000925D5
		public TexturePanner(Material material, string property, Vector2 direction, float speed) : this(material, Shader.PropertyToID(property), direction, speed)
		{
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000943E7 File Offset: 0x000925E7
		public TexturePanner(Material material, int propertyID, Vector2 direction, float speed)
		{
			this.material = material;
			this.propertyID = propertyID;
			this.direction = direction;
			this.direction.Normalize();
			this.speed = speed;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00094418 File Offset: 0x00092618
		public virtual void Tick()
		{
			this.pan -= this.direction * this.speed * this.material.GetTextureScale(this.propertyID).x * Find.TickManager.TickRateMultiplier;
			this.material.SetTextureOffset(this.propertyID, this.pan);
		}

		// Token: 0x04001480 RID: 5248
		protected Material material;

		// Token: 0x04001481 RID: 5249
		protected int propertyID;

		// Token: 0x04001482 RID: 5250
		protected Vector2 pan;

		// Token: 0x04001483 RID: 5251
		public Vector2 direction;

		// Token: 0x04001484 RID: 5252
		public float speed;
	}
}
