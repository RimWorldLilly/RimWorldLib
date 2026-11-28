using System;

namespace Verse
{
	// Token: 0x020004F8 RID: 1272
	public struct TextureAtlasGroupKey : IEquatable<TextureAtlasGroupKey>
	{
		// Token: 0x060025B0 RID: 9648 RVA: 0x000E5488 File Offset: 0x000E3688
		public bool Equals(TextureAtlasGroupKey other)
		{
			return other.group == this.group && other.hasMask == this.hasMask;
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000E54A8 File Offset: 0x000E36A8
		public override bool Equals(object obj)
		{
			if (obj is TextureAtlasGroupKey)
			{
				TextureAtlasGroupKey other = (TextureAtlasGroupKey)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000E54D0 File Offset: 0x000E36D0
		public override int GetHashCode()
		{
			int num = this.group.GetHashCode();
			if (this.hasMask)
			{
				num *= -1;
			}
			return num;
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000E54FC File Offset: 0x000E36FC
		public override string ToString()
		{
			return this.group.ToString() + "_" + this.hasMask.ToString();
		}

		// Token: 0x04001B36 RID: 6966
		public TextureAtlasGroup group;

		// Token: 0x04001B37 RID: 6967
		public bool hasMask;
	}
}
