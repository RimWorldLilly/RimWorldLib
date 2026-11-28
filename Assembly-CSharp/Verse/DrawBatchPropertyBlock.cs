using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025E RID: 606
	public class DrawBatchPropertyBlock
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x00065BA9 File Offset: 0x00063DA9
		public void Clear()
		{
			this.properties.Clear();
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00065BB6 File Offset: 0x00063DB6
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00065BC8 File Offset: 0x00063DC8
		public void SetFloat(int propertyId, float val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Float,
				floatVal = val
			});
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00065C01 File Offset: 0x00063E01
		public void SetColor(string name, Color val)
		{
			this.SetColor(Shader.PropertyToID(name), val);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00065C10 File Offset: 0x00063E10
		public void SetColor(int propertyId, Color val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Color,
				vectorVal = val
			});
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00065C4E File Offset: 0x00063E4E
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00065C60 File Offset: 0x00063E60
		public void SetVector(int propertyId, Vector4 val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Vector,
				vectorVal = val
			});
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00065C9C File Offset: 0x00063E9C
		public void Write(MaterialPropertyBlock propertyBlock)
		{
			foreach (DrawBatchPropertyBlock.Property property in this.properties)
			{
				property.Write(propertyBlock);
			}
		}

		// Token: 0x04000FFF RID: 4095
		private List<DrawBatchPropertyBlock.Property> properties = new List<DrawBatchPropertyBlock.Property>();

		// Token: 0x04001000 RID: 4096
		public string leakDebugString;

		// Token: 0x020025CE RID: 9678
		private enum PropertyType
		{
			// Token: 0x04009B0F RID: 39695
			Float,
			// Token: 0x04009B10 RID: 39696
			Color,
			// Token: 0x04009B11 RID: 39697
			Vector
		}

		// Token: 0x020025CF RID: 9679
		private struct Property
		{
			// Token: 0x0600F0E9 RID: 61673 RVA: 0x00589444 File Offset: 0x00587644
			public void Write(MaterialPropertyBlock propertyBlock)
			{
				switch (this.type)
				{
				case DrawBatchPropertyBlock.PropertyType.Float:
					propertyBlock.SetFloat(this.propertyId, this.floatVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Color:
					propertyBlock.SetColor(this.propertyId, this.vectorVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Vector:
					propertyBlock.SetVector(this.propertyId, this.vectorVal);
					return;
				default:
					return;
				}
			}

			// Token: 0x04009B12 RID: 39698
			public int propertyId;

			// Token: 0x04009B13 RID: 39699
			public DrawBatchPropertyBlock.PropertyType type;

			// Token: 0x04009B14 RID: 39700
			public float floatVal;

			// Token: 0x04009B15 RID: 39701
			public Vector4 vectorVal;
		}
	}
}
