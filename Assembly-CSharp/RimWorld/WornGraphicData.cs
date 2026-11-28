using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F28 RID: 3880
	public class WornGraphicData
	{
		// Token: 0x06006455 RID: 25685 RVA: 0x0023DAB0 File Offset: 0x0023BCB0
		public WornGraphicDirectionData GetDirectionalData(Rot4 facing)
		{
			switch (facing.AsInt)
			{
			case 0:
				return this.north;
			case 1:
				return this.east;
			case 2:
				return this.south;
			case 3:
				return this.west;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unhandled Rot4: ");
				defaultInterpolatedStringHandler.AppendFormatted<Rot4>(facing);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x0023DB24 File Offset: 0x0023BD24
		public Vector2 BeltOffsetAt(Rot4 facing, BodyTypeDef bodyType)
		{
			WornGraphicDirectionData directionalData = this.GetDirectionalData(facing);
			Vector2 vector = directionalData.offset;
			if (bodyType == BodyTypeDefOf.Male)
			{
				vector += directionalData.male.offset;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				vector += directionalData.female.offset;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				vector += directionalData.thin.offset;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				vector += directionalData.hulk.offset;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				vector += directionalData.fat.offset;
			}
			return vector;
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x0023DBCC File Offset: 0x0023BDCC
		public Vector2 BeltScaleAt(Rot4 facing, BodyTypeDef bodyType)
		{
			Vector2 vector = this.GetDirectionalData(facing).Scale;
			if (bodyType == BodyTypeDefOf.Male)
			{
				vector *= this.male.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				vector *= this.female.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				vector *= this.thin.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				vector *= this.hulk.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				vector *= this.fat.Scale;
			}
			return vector;
		}

		// Token: 0x04003953 RID: 14675
		public bool renderUtilityAsPack;

		// Token: 0x04003954 RID: 14676
		public WornGraphicDirectionData north;

		// Token: 0x04003955 RID: 14677
		public WornGraphicDirectionData south;

		// Token: 0x04003956 RID: 14678
		public WornGraphicDirectionData east;

		// Token: 0x04003957 RID: 14679
		public WornGraphicDirectionData west;

		// Token: 0x04003958 RID: 14680
		public WornGraphicBodyTypeData male;

		// Token: 0x04003959 RID: 14681
		public WornGraphicBodyTypeData female;

		// Token: 0x0400395A RID: 14682
		public WornGraphicBodyTypeData thin;

		// Token: 0x0400395B RID: 14683
		public WornGraphicBodyTypeData hulk;

		// Token: 0x0400395C RID: 14684
		public WornGraphicBodyTypeData fat;
	}
}
